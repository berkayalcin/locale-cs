using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locale_CS
{
    [Table("Translations", Schema = "public")]
    public sealed class Translation
    {
        [Key] public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Culture { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}