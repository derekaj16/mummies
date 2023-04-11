using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mummies.Models;
using Mummies.Data;
using Mummies.Models;
using System.Linq;
using mummies.Models.ViewModels;
using Mummies.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult BurialInfo(string category, int pageNum = 1)
        {
            ViewBag.HairViewModel = new HairColorViewModel();
            // Get unique values for textile functions (because there are so many repeats)
            var functions = mummyContext.Textilefunctions.Select(t => t.Value).ToList();
            HashSet<String> textileSet = new HashSet<String>(functions);

            var ages = mummyContext.Burialmains.Select(a => a.Ageatdeath).ToList();
            HashSet<String> ageSet = new HashSet<String>(ages);

            var areas = mummyContext.Burialmains.Select(a => a.Area).ToList();
            HashSet<String> areaSet = new HashSet<String>(areas);

            var hairColors = mummyContext.Burialmains.Select(h => h.Haircolor).ToList();
            HashSet<String> hairSet = new HashSet<String>(hairColors);

            ViewBag.TextileFunctions = textileSet.ToList();
            ViewBag.Colors = mummyContext.Colors.ToList();
            ViewBag.Ages = ageSet.ToList();
            ViewBag.Areas = areaSet.ToList();
            ViewBag.HairColors = hairSet.ToList();
            

            // Create Pagination
            int pageSize = 30;

            var x = new BurialsViewModel
            {
                Burials = mummyContext.Burialmains
                // .Where(b => b.Category == category || category == null)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                pageInfo = new PageInfo
                {
                    totalNumBurials = (mummyContext.Burialmains.Count()),
                        //: mummyContext.Books.Where(x => x.Category == category).Count()),
                    burialsPerPage = pageSize,
                    currentPage = pageNum
                }
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