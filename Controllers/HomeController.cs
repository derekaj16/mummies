using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mummies.Models;
using Mummies.Data;
using Mummies.Models;
using System.Linq;
using Mummies.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;

namespace mummies.Controllers
{

        public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private MummyDbContext mummyContext { get; set; }

        private IMummyRepository repo;

        private ApplicationDbContext applicationContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext appContext, MummyDbContext tempContext, IMummyRepository temp)
        {
            _logger = logger;
            repo = temp;
            applicationContext = appContext;
            mummyContext = tempContext;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BurialInfo(
            int pageNum = 1,
            string? Squarenorthsouth = null,
            string? Headdirection = null,
            string? Northsouth = null,
            string? Depth = null,
            string? Eastwest = null,
            string? Squareeastwest = null,
            string? Area = null,
            string? Burialnumber = null,
            string? Wrapping = null,
            string? Haircolor = null,
            string? Ageatdeath = null,
            string? Sex = null,
            double? Estimatestature = null,
            string? TextileDescription = null,
            string? TextileFunction = null,
            string? TextileColor = null,
            string? TextileStructure = null
        )
        {
            //ViewBag.HairViewModel = new HairColorViewModel();
            ////// Get unique values for textile functions (because there are so many repeats)
            ////var functions = mummyContext.Textilefunctions.Select(t => t.Value).ToList();
            ////HashSet<String> textileSet = new HashSet<String>(functions);

            ////var structures = mummyContext.Structures.Select(t => t.Value).ToList();
            ////HashSet<String> structureSet = new HashSet<String>(structures);

            ////var areas = mummyContext.Burialmains.Select(a => a.Area).ToList();
            ////HashSet<String> areaSet = new HashSet<String>(areas);

  


            ////ViewBag.TextileFunctions = textileSet.ToList();
            ////ViewBag.TextileStructures = structureSet.ToList();
            ////ViewBag.Colors = mummyContext.Colors.ToList();
            //ViewBag.Ages = ageSet.ToList();
            ////ViewBag.Areas = areaSet.ToList();


            // Create Pagination
            int pageSize = 30;

            //var query = mummyContext.Burialmains.AsQueryable();

            //var mummyList = (from p in mummyContext.Burialmains
            //                 join pm in mummyContext.BurialmainTextiles on p.Id equals pm.MainBurialmainid
            //                 join pd in mummyContext.Textiles on pm.MainTextileid equals pd.Id
            //                 select new Mummy()
            //                 {
            //                     MummyId = p.Id
            //                 }).ToList();


            //var mummyList = (from bm in repo.Mummies
            //                 select new Mummy
            //                 {
            //                     mummyId = bm.mummyId,
            //                     burialDepth = bm.burialDepth,
            //                     ageAtDeath = bm.ageAtDeath
            //                 }).AsEnumerable();

            //var filteredMummies = mummyList;

            //if (burialDepth != null)
            //{
            //    filteredMummies = mummyList
            //        .Where(x => double.TryParse(x.burialDepth, out var result)
            //                    ? (result < (double.Parse(burialDepth)) && result > (double.Parse(burialDepth) - 0.5))
            //                    : false)
            //        .Select(x => new Mummy
            //        {
            //            mummyId = x.mummyId,
            //            burialDepth = x.burialDepth,
            //            ageAtDeath = x.ageAtDeath,
            //            hairColor = x.hairColor
            //        });
            //}

            //if (ageAtDeath != null)
            //{
            //    filteredMummies = (from bm in repo.Mummies
            //                       join fm in filteredMummies on bm.mummyId equals fm.mummyId
            //                       where bm.ageAtDeath == ageAtDeath
            //                       select new Mummy
            //                       {
            //                           mummyId = fm.mummyId,
            //                           burialDepth = fm.burialDepth,
            //                           ageAtDeath = bm.ageAtDeath,
            //                           hairColor = fm.hairColor
            //                       }).AsEnumerable();
            //}

            //if (hairColor != null)
            //{
            //    filteredMummies = (from bm in repo.Mummies
            //                       join fm in filteredMummies on bm.mummyId equals fm.mummyId
            //                       where bm.hairColor == hairColor
            //                       select new Mummy
            //                       {
            //                           mummyId = fm.mummyId,
            //                           burialDepth = fm.burialDepth,
            //                           ageAtDeath = fm.ageAtDeath,
            //                           hairColor = bm.hairColor
            //                       }).AsEnumerable();
            //}

            ViewBag.Test = repo.Mummies;

            var x = new BurialsViewModel
            {
                Burials = repo.GetBurials(new Dictionary<string, string?> { { "Ageatdeath", Request.Form["Ageatdeath"] }, { "Haircolor", Request.Form["Haircolor"] }, { "Sex", Request.Form["Sex"] }, { "Wrapping", Request.Form["Wrapping"] }, { "Depth", Request.Form["Depth"] }, { "Northsouth", Request.Form["Northsouth"] }, { "Eastwest", Request.Form["Eastwest"] }, { "Squarenorthsouth", Request.Form["Squarenorthsouth"] }, { "Squareeastwest", Request.Form["Squareeastwest"] } })
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                pageInfo = new PageInfo
                {
                    totalNumBurials = (mummyContext.Burialmains.Count()),
                    burialsPerPage = pageSize,
                    currentPage = pageNum
                },

                Mummies = repo.GetBurials(new Dictionary<string, string?> { { "Ageatdeath", Request.Form["Ageatdeath"] }, { "Haircolor", Request.Form["Haircolor"] }, { "Sex", Request.Form["Sex"] }, { "Wrapping", Request.Form["Wrapping"] }, { "Depth", Request.Form["Depth"] }, { "Northsouth", Request.Form["Northsouth"] }, { "Eastwest", Request.Form["Eastwest"] }, { "Squarenorthsouth", Request.Form["Squarenorthsouth"] }, { "Squareeastwest", Request.Form["Squareeastwest"] } }).ToList(),

                filterSettings = new FilterSettings(),

                formValues = new FormValues()
            };

            return View(x);
        }

        [HttpPost]
        public IActionResult BurialInfo(int pageNum = 1)
        {
            Dictionary<string, string?> burialDict = new Dictionary<string, string?>();

            foreach (string key in Request.Form.Keys)
            {
                burialDict.Add(key, Request.Form[key]);
            }


            int pageSize = 30;
            var mummyQueryable = repo.GetBurials(new Dictionary<string, string?> { { "Ageatdeath", Request.Form["Ageatdeath"] }, { "Haircolor", Request.Form["Haircolor"] }, { "Sex", Request.Form["Sex"] }, { "Wrapping", Request.Form["Wrapping"] }, { "Depth", Request.Form["Depth"] }, { "Northsouth", Request.Form["Northsouth"] }, { "Eastwest", Request.Form["Eastwest"] }, { "Squarenorthsouth", Request.Form["Squarenorthsouth"] }, { "Squareeastwest", Request.Form["Squareeastwest"] } });

            var x = new BurialsViewModel
            {
                Burials = mummyQueryable // repo.GetBurials(burialDict)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                pageInfo = new PageInfo
                {
                    totalNumBurials = (mummyContext.Burialmains.Count()),
                    burialsPerPage = pageSize,
                    currentPage = pageNum
                },

                Mummies = mummyQueryable.ToList(),

                filterSettings = new FilterSettings
                {
                    Ageatdeath = Request.Form["Ageatdeath"],
                    Haircolor = Request.Form["Haircolor"],
                    Sex = Request.Form["Sex"],
                    Wrapping = Request.Form["Wrapping"],
                    Depth = Request.Form["Depth"],
                    Northsouth = Request.Form["Northsouth"],
                    Squarenorthsouth = Request.Form["Squarenorthsouth"],
                    Eastwest = Request.Form["Eastwest"],
                    Squareeastwest = Request.Form["Squareeastwest"]
                },

                formValues = new FormValues()
            };
            return View(x);
        }


        public IActionResult SupervisedAnalysis()
        {
            return View();
        }

        public IActionResult UnsupervisedAnalysis()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        //public IActionResult ManageAccounts()
        //{
        //    return View();
        //}

        public IActionResult BurialDetails(long burialId)
        {
            
            var mummy = mummyContext.Burialmains.Single(x => x.Id == burialId);

            return View(mummy);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}