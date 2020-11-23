using System.Threading.Tasks;

namespace Locale_CS
{
    public interface ITranslator
    {
        Task<string> TranslateAsync(string key);
        Task<string> TranslateAsync(string key, string toCulture);
        string Translate(string key);
        string Translate(string key, string toCulture);
    }
}