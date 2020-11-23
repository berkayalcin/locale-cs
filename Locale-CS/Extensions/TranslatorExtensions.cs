using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Locale_CS
{
    public static class TranslatorExtensions
    {
        public static IServiceCollection AddGlobalization(this IServiceCollection services,
            [NotNull] string connectionString,
            [NotNull] string defaultCulture = "tr-TR",
            TranslationStoreProvider storeProvider = TranslationStoreProvider.Postgres)
        {
            var options = new TranslationOptionsBuilder()
                .SetProvider(storeProvider)
                .SetConnectionString(connectionString)
                .SetDefaultCulture(defaultCulture)
                .Build();
            services.AddSingleton(options);

            services.AddScoped(typeof(TranslationStore),
                TranslationStoreMapper.MapProviderToSystemType(options.Provider));
            services.AddScoped<ITranslator, Translator>();
            services.AddMemoryCache(cacheOptions => { });
            return services;
        }

        private class TranslationOptionsBuilder : ITranslationOptionsBuilder
        {
            private readonly TranslationOptions _translationOptions;

            public TranslationOptionsBuilder()
            {
                _translationOptions = new TranslationOptions();
            }

            public ITranslationOptionsBuilder SetDefaultCulture(string cultureName)
            {
                _translationOptions.DefaultCulture = cultureName;
                return this;
            }

            public ITranslationOptionsBuilder UsePostgresql(string connectionString)
            {
                _translationOptions.ConnectionString = connectionString;
                _translationOptions.Provider = TranslationStoreProvider.Postgres;
                return this;
            }

            public TranslationOptions Build()
            {
                _translationOptions.CreateIfNotExists = true;
                return _translationOptions;
            }

            public ITranslationOptionsBuilder SetProvider(TranslationStoreProvider storeProvider)
            {
                _translationOptions.Provider = storeProvider;
                return this;
            }

            public ITranslationOptionsBuilder SetConnectionString(string connectionString)
            {
                _translationOptions.ConnectionString = connectionString;
                return this;
            }

            public static implicit operator TranslationOptions(TranslationOptionsBuilder builder)
            {
                return builder.Build();
            }
        }
    }
}