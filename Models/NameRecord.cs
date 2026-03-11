using CsvHelper.Configuration.Attributes;

namespace FBZ.Web.Models
{
    public class NameRecord
    {
        [Name("BL record ID")]
        public string BL_Record_ID { get; set; }

        [Name("Name")]
        public string Name { get; set; }
    }
}