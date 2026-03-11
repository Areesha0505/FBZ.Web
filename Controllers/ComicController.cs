using Microsoft.AspNetCore.Mvc;
using FBZ.Web.Models;
using CsvHelper;
using System.Globalization;

namespace FBZ.Web.Controllers
{
    public class ComicController : Controller
    {
        public IActionResult Index()
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
                    r.ISBN
                };

            return View(merged.Take(100).ToList());
        }
    }
}