using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Locale_CS
{
    public sealed class PostgresTranslationStore : TranslationStore
    {
        public PostgresTranslationStore(TranslationOptions options) : base(options)
        {
            GetByKeyQuery =
                @$"SELECT * FROM {SchemeName}.""{TableName}"" WHERE ""Key"" = @Key AND ""Culture"" = @Culture ";
            InsertQuery =
                @$"INSERT INTO {SchemeName}.""{TableName}"" (""Key"",""Value"",""Culture"") VALUES (@Key,@Value,@Culture)";
        }

        public override async Task<IEnumerable<Translation>> GetByKey(string key, string toLocale)
        {
            var conn = new NpgsqlConnection(TranslationOptions.ConnectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(GetByKeyQuery, conn);
            cmd.Parameters.AddWithValue("Key", key);
            cmd.Parameters.AddWithValue("Culture", toLocale);
            var reader = await cmd.ExecuteReaderAsync();
            var translations = DataReaderMapping.MapFrom(reader).ToList();
            if (translations.Count != 0 || !TranslationOptions.CreateIfNotExists) return translations;
            var translation = new Translation()
            {
                Key = key,
                Value = key,
                Culture = toLocale
            };
            var id = await Insert(translation);
            translation.Id = id;
            await conn.CloseAsync();
            return new List<Translation>() {translation};
        }

        protected override async Task<long> Insert(Translation translation)
        {
            await using var conn = new NpgsqlConnection(TranslationOptions.ConnectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(InsertQuery, conn);
            cmd.Parameters.AddWithValue("Key", translation.Key);
            cmd.Parameters.AddWithValue("Value", translation.Key);
            cmd.Parameters.AddWithValue("Culture", translation.Culture);
            var result = await cmd.ExecuteNonQueryAsync();
            return result;
        }

        protected override async Task InitializeAsync()
        {
            await using var conn = new NpgsqlConnection(TranslationOptions.ConnectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(@$"SELECT EXISTS (
                                                               SELECT FROM information_schema.tables 
                                                               WHERE  table_schema = '{SchemeName}'
                                                               AND    table_name   = '{TableName}'
                                                               );", conn);
            var isTableExists = Convert.ToBoolean(cmd.ExecuteScalar()?.ToString());
            if (!isTableExists)
            {
                var createQuery =
                    "create table \"Translations\"\n(\n\t\"Id\" bigserial not null\n\t\tconstraint \"\"\"translations\"\"_pk\"\n\t\t\tprimary key,\n\t\"Key\" varchar(500) not null,\n\t\"Value\" varchar(500) not null,\n\t\"Culture\" varchar(500) not null ,\n\t\"LastModifiedDate\" timestamp default now(),\n\t\"CreatedDate\" timestamp default now(),\n\t\"IsDeleted\" bool default false\n);\n\n";
                createQuery = createQuery.Replace("%TableNameToken%", TableName);
                await using var createCommand = new NpgsqlCommand(createQuery, conn);
                createCommand.ExecuteNonQuery();
            }

            await conn.CloseAsync();
        }
    }
}