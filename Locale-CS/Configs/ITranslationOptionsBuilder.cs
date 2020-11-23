namespace Locale_CS
{
    public interface ITranslationOptionsBuilder
    {
        ITranslationOptionsBuilder UsePostgresql(string connectionString);
        TranslationOptions Build();
        ITranslationOptionsBuilder SetProvider(TranslationStoreProvider storeProvider);
        ITranslationOptionsBuilder SetConnectionString(string connectionString);
        ITranslationOptionsBuilder SetDefaultCulture(string cultureName);
    }
}