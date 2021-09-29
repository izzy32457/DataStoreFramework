using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using JetBrains.Annotations;

namespace DataStoreFramework.AwsS3.AwsS3Services
{
    /// <summary>A stream implementation that retrieves an S3 object in a seekable way.</summary>
    /// <remarks>
    /// Code taken and adapted from: https://github.com/mlhpdx/seekable-s3-stream/blob/master/SeekableS3Stream/SeekableS3Stream.cs.
    /// </remarks>
    [UsedImplicitly] // TODO: remove this when we actually start to use it.
    internal class SeekableS3Stream : Stream
    {
        private const long DefaultPageLength = 25 * 1024 * 1024; // 25MB
        private const int DefaultMaxPageCount = 20;

        private readonly IAmazonS3 _s3;
        private readonly MetaData _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeekableS3Stream"/> class.
        /// </summary>
        /// <param name="s3">An Amazon S3 Client instance.</param>
        /// <param name="bucket">The Bucket name to be accessed.</param>
        /// <param name="key">The object Key to be accessed.</param>
        /// <param name="page">The size for pages to be retrieved.</param>
        /// <param name="maxPageCacheSize">The maximum number of pages to be cached.</param>
        public SeekableS3Stream(IAmazonS3 s3, string bucket, string key, long page = DefaultPageLength, int maxPageCacheSize = DefaultMaxPageCount)
        {
            _s3 = s3;
            var m = _s3.GetObjectMetadataAsync(bucket, key).GetAwaiter().GetResult();

            _metadata = new MetaData
            {
                Bucket = bucket,
                Key = key,
                PageSize = page,
                MaxPages = maxPageCacheSize,
                Pages = new Dictionary<long, byte[]>(maxPageCacheSize),
                HotList = new Dictionary<long, long>(maxPageCacheSize),
                Length = m.ContentLength,
                S3ETag = m.ETag,
            };
        }

        /// <summary>
        /// Gets the total number of bytes read from the S3 Object.
        /// </summary>
        public long TotalRead { get; private set; }

        /// <summary>
        /// Gets the total number of bytes of the S3 Object that have been downloaded.
        /// </summary>
        public long TotalLoaded { get; private set; }

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => true;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override long Length => _metadata.Length;

        /// <inheritdoc/>
        public override long Position
        {
            get => _metadata.Position;
            set => Seek(value, value >= 0 ? SeekOrigin.Begin : SeekOrigin.End);
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_metadata.Position < 0 || _metadata.Position >= Length)
            {
                return 0;
            }

            var p = _metadata.Position;
            do
            {
                var i = p / _metadata.PageSize;
                var o = p % _metadata.PageSize;

                byte[] b = null;
                if (_metadata.Pages.ContainsKey(i))
                {
                    b = _metadata.Pages[i];
                }

                if (b == null)
                {
                    // if we have too many pages, drop the coolest
                    while (_metadata.Pages.Count >= _metadata.MaxPages)
                    {
                        var trim = _metadata.Pages.OrderBy(kv => _metadata.HotList[kv.Key]).First().Key;
                        _metadata.Pages.Remove(trim);
                    }

                    var s = i * _metadata.PageSize;
                    var e = s + Math.Min(_metadata.PageSize, _metadata.Length - s); // read in a single page (we're looping)

                    var go = new GetObjectRequest
                    {
                        BucketName = _metadata.Bucket,
                        Key = _metadata.Key,
                        EtagToMatch = _metadata.S3ETag, // ensure the object hasn't change under us
                        ByteRange = new ByteRange(s, e),
                    };

                    _metadata.Pages[i] = b = new byte[e - s];
                    if (!_metadata.HotList.ContainsKey(i))
                    {
                        _metadata.HotList[i] = 1;
                    }

                    var read = 0;
                    using (var r = _s3.GetObjectAsync(go).GetAwaiter().GetResult())
                    {
                        do
                        {
                            read += r.ResponseStream.Read(b, read, b.Length - read);
                        }
                        while (read < b.Length);
                    }

                    TotalLoaded += read;
                }
                else
                {
                    _metadata.HotList[i] += 1;
                }

                var l = Math.Min(b.Length - o, count);
                Array.Copy(b, (int)o, buffer, offset, (int)l);
                offset += (int)l;
                count -= (int)l;
                p += (int)l;
            }
            while (count > 0 && p < _metadata.Length);

            var c = p - _metadata.Position;
            TotalRead += c;
            _metadata.Position = p;
            return (int)c;
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            var newPosition = _metadata.Position;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPosition = offset; // offset must be positive
                    break;
                case SeekOrigin.Current:
                    newPosition += offset; // + or -
                    break;
                case SeekOrigin.End:
                    newPosition = _metadata.Length - Math.Abs(offset); // offset must be negative?
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            if (newPosition < 0 || newPosition > _metadata.Length)
            {
                throw new InvalidOperationException("Stream position is invalid.");
            }

            return _metadata.Position = newPosition;
        }

        /// <inheritdoc/>
        public override void SetLength(long value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override void Flush() => throw new NotImplementedException();

        /// <summary>An internal class that keeps state for the current S3 object stream.</summary>
        private class MetaData
        {
            public string Bucket { get; init; }

            public string Key { get; init; }

            public string S3ETag { get; init; }

            public long Length { get; init; }

            public long PageSize { get; init; } = DefaultPageLength;

            public long MaxPages { get; init; } = DefaultMaxPageCount;

            public Dictionary<long, byte[]> Pages { get; init; }

            public Dictionary<long, long> HotList { get; init; }

            public long Position { get; set; }
        }
    }
}
