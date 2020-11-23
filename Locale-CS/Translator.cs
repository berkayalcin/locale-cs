using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Locale_CS
{
    public class Translator : ITranslator
    {
        private readonly TranslationStore _translationStore;
        private readonly IMemoryCache _memoryCache;
        private TranslationOptions _options;

        public Translator(IServiceProvider serviceProvider)
        {
            _options = serviceProvider.GetRequiredService<TranslationOptions>();
            _translationStore = TranslationStoreMapper.MapProviderToTranslationStore(_options);
            _memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
        }

        public async Task<string> TranslateAsync(string key)
        {
            var currentCultureName = !string.IsNullOrEmpty(Thread.CurrentThread.CurrentCulture.DisplayName)
                ? Thread.CurrentThread.CurrentCulture.DisplayName
                : _options.DefaultCulture;
            var translation = await _translationStore.GetByKey(key, currentCultureName);
            return translation.FirstOrDefault()?.Value;
        }

        public async Task<string> TranslateAsync(string key, string toCulture)
        {
            var translations = (await _translationStore.GetByKey(key, toCulture)).ToList();
            return translations.FirstOrDefault()?.Value;
        }

        public string Translate(string key)
        {
            return TranslateAsync(key).GetAwaiter().GetResult();
        }

        public string Translate(string key, string toCulture)
        {
            return TranslateAsync(key, toCulture).GetAwaiter().GetResult();
        }
    }
}