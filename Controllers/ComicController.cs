using Microsoft.AspNetCore.Mvc;
using FBZ.Web.Models;
using CsvHelper;
using System.Globalization;

namespace FBZ.Web.Controllers
{
    public class ComicController : Controller
    {
        public IActionResult Index(string searchTitle, string genre, string sortOrder, string groupBy, int page = 1)
        {
            var recordsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "records.csv");
            var namesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "names.csv");
            var titlesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "titles.csv");

            List<ComicRecord> records;
            List<NameRecord> names;
            List<TitleRecord> titles;

            using (var reader = new StreamReader(recordsPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<ComicRecord>().ToList();
            }

            using (var reader = new StreamReader(namesPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                names = csv.GetRecords<NameRecord>().ToList();
            }

            using (var reader = new StreamReader(titlesPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                titles = csv.GetRecords<TitleRecord>().ToList();
            }
            var genres = records
    .Where(x => !string.IsNullOrEmpty(x.Content_type))
    .Select(x => x.Content_type)
    .Distinct()
    .OrderBy(x => x)
    .ToList();

            ViewBag.Genres = genres;

            var merged =
                from r in records
                join n in names on r.BL_Record_ID equals n.BL_Record_ID into rn
                from n in rn.DefaultIfEmpty()

                join t in titles on r.BL_Record_ID equals t.BL_Record_ID into rt
                from t in rt.DefaultIfEmpty()

                select new
                {
                    r.BL_Record_ID,
                    Title = t?.Title,
                    Author = n?.Name,
                    Genre = r.Content_type,
                    Material_type = r.Material_type,
                    Year = t.Date_of_publication,
                    ISBN = string.IsNullOrEmpty(r.ISBN) ? "missing" : r.ISBN,


                };
            merged = merged
.GroupBy(x => x.BL_Record_ID)
.Select(g => new
{
    BL_Record_ID = g.Key,
    Title = g.First().Title,
    Author = string.Join(", ", g.Select(x => x.Author).Where(a => !string.IsNullOrEmpty(a)).Distinct()),
    Genre = g.First().Genre,
    Material_type = g.First().Material_type,
    Year = string.Join(", ", g.Select(x => x.Year).Where(y => !string.IsNullOrEmpty(y)).Distinct()),
    ISBN = string.Join(", ", g.Select(x => string.IsNullOrEmpty(x.ISBN) ? "missing" : x.ISBN).Distinct())
});

            if (!string.IsNullOrEmpty(searchTitle))
            {
                merged = merged.Where(x => x.Title != null && x.Title.Contains(searchTitle));
            }
            if (!string.IsNullOrEmpty(genre))
            {
                merged = merged.Where(x => x.Genre != null && x.Genre == genre);
            }
            if (sortOrder == "az")
            {
                merged = merged.OrderBy(x => x.Title);
            }
            else if (sortOrder == "za")
            {
                merged = merged.OrderByDescending(x => x.Title);
            }
            if (groupBy == "author")
            {
                merged = merged.OrderBy(x => x.Author);
            }
            if (groupBy == "year")
            {
                merged = merged.OrderBy(x => x.Year);
            }
            ViewBag.TotalRecords = merged.Count();
            int pageSize = 100;

            var pagedData = merged
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
           

            var totalRecords = merged.Count();

            ViewBag.TotalRecords = totalRecords;

            var displayRecords = merged.Take(100).ToList();

            return View(pagedData);
        }
        [HttpPost]
        public IActionResult Save(string id)
        {
            Console.WriteLine("Saved comic: " + id);
            var saved = HttpContext.Session.GetString("SavedComics");

            if (saved == null)
                saved = id;
            else
                saved += "," + id;

            HttpContext.Session.SetString("SavedComics", saved);

            return RedirectToAction("Index");
        }
        public IActionResult Saved()
        {
            var saved = HttpContext.Session.GetString("SavedComics");

            if (string.IsNullOrEmpty(saved))
            {
                return View(new List<string>());
            }

            var savedIds = saved.Split(',').ToList();

            return View(savedIds);
        }
    }
}