using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mummies.Models;
using Mummies.Data;
using Mummies.Models;
using System.Linq;
using mummies.Models.ViewModels;
using Mummies.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace mummies.Controllers
{

        public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private MummyDbContext mummyContext;
        private ApplicationDbContext applicationContext;

        public HomeController(ILogger<HomeController> logger, MummyDbContext context, ApplicationDbContext appContext)
        {
            _logger = logger;
            mummyContext = context;
            applicationContext = appContext;
        }

        public IActionResult Index()
        {
            ViewBag.things = mummyContext.Burialmains.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult BurialInfo(
            int pageNum = 1,
            string? ageAtDeath = null,
            //string? squareNorthSouth = null,
            //string? northSouth = null,
            //string? squareEastWest = null,
            //string? eastWest = null,
            //string? textileColor = null,
            //string? textileStructure = null,
            //string? textileFunction = null,
            string? burialDepth = null,
            //string? estimateStature = null,
            //string? headDirection = null,
            //string? area = null,
            //long? burialId = null,
            string? hairColor = null
            //string? sex = null,
            //string? burialNumber = null
        )
        {
            ViewBag.HairViewModel = new HairColorViewModel();
            //// Get unique values for textile functions (because there are so many repeats)
            //var functions = mummyContext.Textilefunctions.Select(t => t.Value).ToList();
            //HashSet<String> textileSet = new HashSet<String>(functions);

            //var structures = mummyContext.Structures.Select(t => t.Value).ToList();
            //HashSet<String> structureSet = new HashSet<String>(structures);

            var ages = mummyContext.Burialmains.Select(a => a.Ageatdeath).ToList();
            HashSet<String> ageSet = new HashSet<String>(ages);

            //var areas = mummyContext.Burialmains.Select(a => a.Area).ToList();
            //HashSet<String> areaSet = new HashSet<String>(areas);

  


            //ViewBag.TextileFunctions = textileSet.ToList();
            //ViewBag.TextileStructures = structureSet.ToList();
            //ViewBag.Colors = mummyContext.Colors.ToList();
            ViewBag.Ages = ageSet.ToList();
            //ViewBag.Areas = areaSet.ToList();


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

           
            var mummyList = (from bm in mummyContext.Burialmains
                             select new Mummy
                             {
                                 mummyId = bm.Id,
                                 burialDepth = bm.Depth,
                                 ageAtDeath = bm.Ageatdeath
                             }).AsEnumerable();



            var filteredMummies = mummyList;

            

            if (burialDepth != null)
            {
                filteredMummies = mummyList
                    .Where(x => double.TryParse(x.burialDepth, out var result)
                                ? (result < (double.Parse(burialDepth)) && result > (double.Parse(burialDepth) - 0.5))
                                : false)
                    .Select(x => new Mummy
                    {
                        mummyId = x.mummyId,
                        burialDepth = x.burialDepth,
                        ageAtDeath = x.ageAtDeath,
                        hairColor = x.hairColor
                    });
            }

            if (ageAtDeath != null)
            {
                filteredMummies = filteredMummies
                    .Where(x => x.ageAtDeath == ageAtDeath);
            }

            if (hairColor != null)
            {
                filteredMummies = (from bm in mummyContext.Burialmains
                                   join fm in filteredMummies on bm.Id equals fm.mummyId
                                   where bm.Haircolor == "B"
                                   select new Mummy
                                   {
                                       mummyId = fm.mummyId,
                                       burialDepth = fm.burialDepth,
                                       ageAtDeath = fm.ageAtDeath,
                                       hairColor = bm.Haircolor
                                   });
            }
            //if (hairColor != null)
            //{
            //    query.Where(x => x.Haircolor != null ? x.Haircolor.ToString() == hairColor : false);
            //}
            

            ViewBag.MummyList = filteredMummies.ToList();

            var x = new BurialsViewModel
            {
                Burials = mummyContext.Burialmains
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                pageInfo = new PageInfo
                {
                    totalNumBurials = (mummyContext.Burialmains.Count()),
                    burialsPerPage = pageSize,
                    currentPage = pageNum
                },

                //ageAtDeath = ageAtDeath != null ? ageAtDeath : null,

                //burialDepth = burialDepth != null ? burialDepth : null
            };
            // var burialInfo = mummyContext.Burialmains.ToList();
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
            var mummyList = (from p in mummyContext.Burialmains
                            join pm in mummyContext.BurialmainTextiles on p.Id equals pm.MainBurialmainid
                            join pd in mummyContext.Textiles on pm.MainTextileid equals pd.Id
                            where p.Haircolor == "B"
                            select new Mummy()
                            {
                                mummyId = p.Id,
                                hairColor = p.Haircolor
                            }).ToList();

            ViewBag.MummyList = mummyList;
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