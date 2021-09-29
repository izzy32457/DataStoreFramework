using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using JetBrains.Annotations;

namespace DataStoreFramework.AwsS3.AwsS3Services
{
    /// <summary>A stream implementation that uploads data to an S3 object.</summary>
    /// <remarks>
    /// Code taken and adapted from: https://github.com/mlhpdx/s3-upload-stream/blob/main/S3UploadStream/S3UploadStream.cs.
    ///
    /// NOTE: The maximum size (at the time of writing) of a file in S3 is 5TB.
    /// So, it isn't safe to assume all uploads will work here.
    /// MAX_PART_SIZE times MAX_PART_COUNT is ~50TB, which is too big for S3.
    /// </remarks>
    [UsedImplicitly] // TODO: remove this when we actually start to use it.
    internal class S3UploadStream : Stream
    {
        private const long MinPartLength = 5L * 1024 * 1024; // 5MB
        private const long MaxPartLength = 5L * 1024 * 1024 * 1024; // 5GB max per PUT
        private const long MaxPartCount = 10000; // no more than 10,000 parts total
        private const long DefaultPartLength = MinPartLength;

        private readonly Metadata _metadata;
        private readonly IAmazonS3 _s3;

        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="S3UploadStream"/> class.
        /// </summary>
        /// <param name="s3">An Amazon S3 Client instance.</param>
        /// <param name="s3Uri">An S3 Object Uri.</param>
        /// <param name="partLength">The length in bytes of the uploaded object parts (Default: 5MB).</param>
        public S3UploadStream(IAmazonS3 s3, string s3Uri, long partLength = DefaultPartLength)
            : this(s3, new Uri(s3Uri), partLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="S3UploadStream"/> class.
        /// </summary>
        /// <param name="s3">An Amazon S3 Client instance.</param>
        /// <param name="s3Uri">An S3 Object Uri.</param>
        /// <param name="partLength">The length in bytes of the uploaded object parts (Default: 5MB).</param>
        public S3UploadStream(IAmazonS3 s3, Uri s3Uri, long partLength = DefaultPartLength)
            : this(s3, s3Uri.Host, s3Uri.LocalPath[1..], partLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="S3UploadStream"/> class.
        /// </summary>
        /// <param name="s3">An Amazon S3 Client instance.</param>
        /// <param name="bucket">The Bucket name to upload object to.</param>
        /// <param name="key">he object Key for the uploaded object.</param>
        /// <param name="partLength">The length in bytes of the uploaded object parts (Default: 5MB).</param>
        public S3UploadStream(IAmazonS3 s3, string bucket, string key, long partLength = DefaultPartLength)
        {
            _s3 = s3;
            _metadata = new Metadata
            {
                BucketName = bucket,
                Key = key,
                PartLength = partLength,
            };
        }

        /// <inheritdoc/>
        public override bool CanRead => false;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <inheritdoc/>
        public override long Length => _metadata.Length = Math.Max(_metadata.Length, _metadata.Position);

        /// <inheritdoc/>
        public override long Position
        {
            get => _metadata.Position;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            _metadata.Length = Math.Max(_metadata.Length, value);
            _metadata.PartLength = Math.Max(MinPartLength, Math.Min(MaxPartLength, _metadata.Length / MaxPartCount));
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return;
            }

            // write as much of the buffer as will fit to the current part, and if needed
            // allocate a new part and continue writing to it (and so on).
            var o = offset;
            var c = Math.Min(count, buffer.Length - offset); // don't over-read the buffer, even if asked to
            do
            {
                if (_metadata.CurrentStream == null || _metadata.CurrentStream.Length >= _metadata.PartLength)
                {
                    StartNewPart();
                }

                var remaining = _metadata.PartLength - _metadata.CurrentStream?.Length ?? 0;
                var w = Math.Min(c, (int)remaining);
                _metadata.CurrentStream?.Write(buffer, o, w);

                _metadata.Position += w;
                c -= w;
                o += w;
            }
            while (c > 0);
        }

        /// <inheritdoc/>
        public override void Flush() => Flush(false);

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    Flush(true);
                    CompleteUpload();
                }
            }

            _disposed = true;
            base.Dispose(disposing);
        }

        private void StartNewPart()
        {
            if (_metadata.CurrentStream != null)
            {
                Flush(false);
            }

            _metadata.CurrentStream = new MemoryStream();
            _metadata.PartLength = Math.Min(MaxPartLength, Math.Max(_metadata.PartLength, (_metadata.PartCount / 2 + 1) * MinPartLength));
        }

        private void Flush(bool disposing)
        {
            if (!disposing && (_metadata.CurrentStream == null || _metadata.CurrentStream.Length < MinPartLength))
            {
                return;
            }

            _metadata.UploadId ??= _s3.InitiateMultipartUploadAsync(
                    new InitiateMultipartUploadRequest
                    {
                        BucketName = _metadata.BucketName,
                        Key = _metadata.Key,
                    })
                .GetAwaiter()
                .GetResult()
                .UploadId;

            if (_metadata.CurrentStream == null)
            {
                return;
            }

            var i = ++_metadata.PartCount;

            _metadata.CurrentStream.Seek(0, SeekOrigin.Begin);
            var request = new UploadPartRequest
            {
                BucketName = _metadata.BucketName,
                Key = _metadata.Key,
                UploadId = _metadata.UploadId,
                PartNumber = i,
                IsLastPart = disposing,
                InputStream = _metadata.CurrentStream,
            };
            _metadata.CurrentStream = null;

            var upload = Task.Run(async () =>
            {
                var response = await _s3.UploadPartAsync(request).ConfigureAwait(false);
                _metadata.PartETags.AddOrUpdate(i, response.ETag, (_, _) => response.ETag);
                await request.InputStream.DisposeAsync().ConfigureAwait(false);
            });
            _metadata.Tasks.Add(upload);
        }

        private void CompleteUpload()
        {
            Task.WaitAll(_metadata.Tasks.ToArray());

            if (Length > 0)
            {
                _s3.CompleteMultipartUploadAsync(
                        new CompleteMultipartUploadRequest
                        {
                            BucketName = _metadata.BucketName,
                            Key = _metadata.Key,
                            PartETags = _metadata.PartETags.Select(e => new PartETag(e.Key, e.Value)).ToList(),
                            UploadId = _metadata.UploadId,
                        })
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private class Metadata
        {
            public string BucketName { get; init; }

            public string Key { get; init; }

            public long PartLength { get; set; } = DefaultPartLength;

            public int PartCount { get; set; }

            public string UploadId { get; set; }

            public MemoryStream CurrentStream { get; set; }

            public long Position { get; set; } // based on bytes written

            public long Length { get; set; } // based on bytes written or SetLength, whichever is larger (no truncation)

            public List<Task> Tasks { get; } = new ();

            public ConcurrentDictionary<int, string> PartETags { get; } = new ();
        }
    }
}
