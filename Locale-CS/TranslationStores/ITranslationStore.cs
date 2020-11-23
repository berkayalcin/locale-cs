using Microsoft.Extensions.Configuration;

namespace Locale_CS
{
    public interface ITranslationStore
    {
        Translation GetByKey(string key);
    }
}