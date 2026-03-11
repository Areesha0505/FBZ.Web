using CsvHelper.Configuration.Attributes;

namespace FBZ.Web.Models
{
    public class ComicRecord
    {
        [Name("BL record ID")]
        public string BL_Record_ID { get; set; }

        [Name("Type of resource")]
        public string Type_of_resource { get; set; }

        [Name("Content type")]
        public string Content_type { get; set; }

        [Name("Material type")]
        public string Material_type { get; set; }

        [Name("BNB number")]
        public string BNB_number { get; set; }

        [Name("ISBN")]
        public string ISBN { get; set; }

        [Name("Name")]
        public string Name { get; set; }

        [Name("Dates associated with name")]
        public string Dates_associated_with_name { get; set; }

        [Name("Type of name")]
        public string Type_of_name { get; set; }

        [Name("Role")]
        public string Role { get; set; }
    }
}


     