using CsvHelper.Configuration.Attributes;

namespace FBZ.Web.Models
{
    public class TitleRecord
    {
        [Name("BL record ID")]
        public string BL_Record_ID { get; set; }

        [Name("Title")]
        public string Title { get; set; }

        [Name("Dates associated with name")]
        public string Date_of_publication { get; set; }
    }
}