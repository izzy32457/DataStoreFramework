using System;
using System.Globalization;
using System.Reflection;
using DataStoreFramework.Exceptions;
using DataStoreFramework.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace DataStoreFramework
{
    /// <summary>A collection of extensions for <see cref="IConfiguration"/> to simplify Data Store configurations.</summary>
    [PublicAPI]
    public static class ConfigurationExtensions
    {
        /// <summary>The default section where settings are read from the IConfiguration object. This is set to "DSF".</summary>
        public const string DefaultConfigSection = "DSF";

        /// <summary>Constructs a Provider Options class with the options specified in the "DSF" section in the <see cref="IConfiguration"/> object.</summary>
        /// <typeparam name="TOptions">The type of <see cref="ProviderOptions"/> to construct.</typeparam>
        /// <param name="config">An <see cref="IConfiguration"/> instance.</param>
        /// <returns>The Provider Options containing the values set in configuration system.</returns>
        [NotNull]
        public static TOptions GetProviderOptions<TOptions>([NotNull] this IConfiguration config)
            where TOptions : ProviderOptions, new()
            => GetProviderOptions<TOptions>(config, DefaultConfigSection);

        /// <summary>Constructs a Provider Options class with the options specified in the provided in the <see cref="IConfiguration"/> object.</summary>
        /// <typeparam name="TOptions">The type of <see cref="ProviderOptions"/> to construct.</typeparam>
        /// <param name="config">An <see cref="IConfiguration"/> instance.</param>
        /// <param name="configSection">The config section to extract data store provider options from.</param>
        /// <returns>The AWSOptions containing the values set in configuration system.</returns>
        [NotNull]
        public static TOptions GetProviderOptions<TOptions>([NotNull] this IConfiguration config, [CanBeNull] string configSection)
            where TOptions : ProviderOptions, new()
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var options = new TOptions();
            var section = string.IsNullOrEmpty(configSection) ? config : config.GetSection(configSection);
            if (section is null)
            {
                return options;
            }

            // TODO: test this
            var optionsTypeInfo = typeof(TOptions).GetTypeInfo();
            foreach (var configurationSection in section.GetChildren())
            {
                try
                {
                    var property = optionsTypeInfo.GetDeclaredProperty(configurationSection.Key);
                    if (property == null || property.SetMethod == null)
                    {
                        continue;
                    }

                    if (property.PropertyType == typeof(string) || property.PropertyType.GetTypeInfo().IsPrimitive)
                    {
                        var value = Convert.ChangeType(configurationSection.Value, property.PropertyType, CultureInfo.InvariantCulture);
                        property.SetMethod.Invoke(options, new[] { value });
                    }
                    else if (property.PropertyType == typeof(TimeSpan) || property.PropertyType == typeof(TimeSpan?))
                    {
                        var milliseconds = Convert.ToInt64(configurationSection.Value, CultureInfo.InvariantCulture);
                        var timespan = TimeSpan.FromMilliseconds(milliseconds);
                        property.SetMethod.Invoke(options, new object[] { timespan });
                    }
                }
                catch (Exception e)
                {
                    throw new ConfigurationException($"Error reading value for property {configurationSection.Key}.", e)
                    {
                        PropertyName = configurationSection.Key,
                        PropertyValue = configurationSection.Value,
                    };
                }
            }

            return options;
        }
    }
}
