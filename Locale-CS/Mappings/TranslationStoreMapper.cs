using System;
using System.Collections.Concurrent;

namespace Locale_CS
{
    public static class TranslationStoreMapper
    {
        private static readonly ConcurrentDictionary<TranslationStoreProvider, Type> TranslationStoreProviders;

        static TranslationStoreMapper()
        {
            TranslationStoreProviders = new ConcurrentDictionary<TranslationStoreProvider, Type>();
            TranslationStoreProviders.TryAdd(TranslationStoreProvider.Postgres, typeof(PostgresTranslationStore));
        }

        public static Type MapProviderToSystemType(TranslationStoreProvider provider) =>
            TranslationStoreProviders[provider];

        public static TranslationStore MapProviderToTranslationStore(TranslationOptions options)
        {
            return options.Provider switch
            {
                TranslationStoreProvider.Postgres => new PostgresTranslationStore(options),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}