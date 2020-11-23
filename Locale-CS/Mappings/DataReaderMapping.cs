using System.Collections.Generic;
using System.Data;

namespace Locale_CS
{
    public static class DataReaderMapping
    {
        public static IEnumerable<Translation> MapFrom(IDataReader dataReader)
        {
            if (dataReader == null)
                yield return null;
            while (dataReader != null && dataReader.Read())
            {
                var translation = new Translation()
                {
                    Id = dataReader.GetInt64(0),
                    Key = dataReader.GetString(1),
                    Value = dataReader.GetString(2),
                    Culture = dataReader.GetString(3),
                    LastModifiedDate = dataReader.GetDateTime(4),
                    CreatedDate = dataReader.GetDateTime(5),
                    IsDeleted = dataReader.GetBoolean(6)
                };
                yield return translation;
            }
        }
    }
}