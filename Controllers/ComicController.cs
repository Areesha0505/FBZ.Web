using Microsoft.AspNetCore.Mvc;
using FBZ.Web.Models;
using Microsoft.VisualBasic.FileIO;
using System.Text;


namespace FBZ.Web.Controllers
{
    public class ComicController : Controller
    {
        public IActionResult Index(string search, string genre, string sort, int page = 1)
        {
            var comics = LoadData();

            // Title search
            if (!string.IsNullOrWhiteSpace(search))
            {
                comics = comics
                    .Where(c => c.Title != null &&
                    c.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Genre filter ONLY if user selects a genre
            if (!string.IsNullOrWhiteSpace(genre))
            {
                comics = comics
                    .Where(c => c.Genre != null &&
                    c.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (sort == "az")
            {
                comics = comics.OrderBy(c => c.Title).ToList();
            }
            else if (sort == "za")
            {
                comics = comics.OrderByDescending(c => c.Title).ToList();
            }
            int pageSize = 100;
            int totalRecords = comics.Count();

            comics = comics
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);


            return View(comics);
        }
        public IActionResult Export(string search, string genre, string sort)
        {
            var comics = LoadData();

            if (!string.IsNullOrWhiteSpace(search))
            {
                comics = comics
                .Where(c => c.Title != null &&
                c.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                comics = comics
                .Where(c => c.Genre != null &&
                c.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase))
                .ToList();
            }

            if (sort == "az")
            {
                comics = comics.OrderBy(c => c.Title).ToList();
            }
            else if (sort == "za")
            {
                comics = comics.OrderByDescending(c => c.Title).ToList();
            }

            var csv = new StringBuilder();
            csv.AppendLine("Title,Name,Genre,Year");

            foreach (var comic in comics)
            {
                csv.AppendLine($"{comic.Title},{comic.Name},{comic.Genre},{comic.DateOfPublication}");
            }

            return File(
                Encoding.UTF8.GetBytes(csv.ToString()),
                "text/csv",
                "results.csv");
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