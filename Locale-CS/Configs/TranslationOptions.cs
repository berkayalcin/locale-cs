namespace Locale_CS
{
    public sealed class TranslationOptions
    {
        public string ConnectionString { get; set; }
        public bool CreateIfNotExists { get; set; } = true;
        public string DefaultCulture { get; set; } = "tr-TR";
        public TranslationStoreProvider Provider { get; set; }
    }
}