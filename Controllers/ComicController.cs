using Microsoft.AspNetCore.Mvc;
using FBZ.Web.Models;
using Microsoft.VisualBasic.FileIO;
using System.Text;


namespace FBZ.Web.Controllers
{
    public class ComicController : Controller
    {
        public IActionResult Index(string search)
        {
            var comics = LoadData();

            if (!string.IsNullOrWhiteSpace(search))
            {
                comics = comics
                    .Where(c => c.Title != null &&
                    c.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(comics);
        }

        private List<ComicRecord> LoadData()
        {
            var list = new List<ComicRecord>();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "names.csv");

            using (var parser = new TextFieldParser(path, Encoding.GetEncoding(1252)))
            {
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                var headers = parser.ReadFields();

                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();

                    var row = headers.Zip(fields, (h, v) => new { h, v })
                                     .ToDictionary(x => x.h, x => x.v);

                    list.Add(new ComicRecord
                    {
                        Title = row.ContainsKey("Title") ? row["Title"] : "",
                        Name = row.ContainsKey("Name") ? row["Name"] : "",
                        Genre = row.ContainsKey("Genre") ? row["Genre"] : "",
                        DateOfPublication = row.ContainsKey("Date of publication") ? row["Date of publication"] : "",
                        ISBN = row.ContainsKey("ISBN") ? row["ISBN"] : "",
                        Edition = row.ContainsKey("Edition") ? row["Edition"] : "",
                        Languages = row.ContainsKey("Languages") ? row["Languages"] : "",
                        NameType = row.ContainsKey("Name type") ? row["Name type"] : ""
                    });
                }
            }

            return list;
        }
    }
}