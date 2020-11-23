using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Locale_CS
{
    public abstract class TranslationStore
    {
        protected readonly TranslationOptions TranslationOptions;
        protected readonly string SchemeName;
        protected readonly string TableName;
        protected string GetByKeyQuery;
        protected string InsertQuery;


        protected TranslationStore(TranslationOptions options)
        {
            TranslationOptions = options;
            if (TranslationOptions == null)
                throw new ArgumentNullException(nameof(TranslationOptions),
                    "To Use Translation, You need to register TranslationOptions");

            var tableAttribute =
                (TableAttribute) typeof(Translation).GetCustomAttribute(typeof(TableAttribute));

            SchemeName = tableAttribute?.Schema;
            TableName = tableAttribute?.Name;
            InitializeAsync().GetAwaiter().GetResult();
        }

        public abstract Task<IEnumerable<Translation>> GetByKey(string key, string toLocale);
        protected abstract Task<long> Insert(Translation translation);
        protected abstract Task InitializeAsync();
    }
}