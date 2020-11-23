using System;

namespace Locale_CS
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TranslateFromAttribute : Attribute
    {
        public TranslateFromAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; set; }
    }
}