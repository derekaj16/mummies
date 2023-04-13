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

            // Create Pagination
            int pageSize = 30;
            IQueryable<Mummy> mummyQueryable = repo.GetBurials();

            var x = new BurialsViewModel
            {
                Burials = mummyQueryable
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                pageInfo = new PageInfo
                {
                    totalNumBurials = (mummyQueryable.Count()),
                    burialsPerPage = pageSize,
                    currentPage = pageNum
                },

                Mummies = mummyQueryable.ToList(),

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
            IQueryable<Mummy> mummyQueryable = repo.GetBurials(new Dictionary<string, string?> { { "Ageatdeath", Request.Form["Ageatdeath"] }, { "Haircolor", Request.Form["Haircolor"] }, { "Sex", Request.Form["Sex"] }, { "Wrapping", Request.Form["Wrapping"] }, { "Depth", Request.Form["Depth"] }, { "Northsouth", Request.Form["Northsouth"] }, { "Eastwest", Request.Form["Eastwest"] }, { "Squarenorthsouth", Request.Form["Squarenorthsouth"] }, { "Squareeastwest", Request.Form["Squareeastwest"] } });

            var x = new BurialsViewModel
            {
                Burials = mummyQueryable
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                pageInfo = new PageInfo
                {
                    totalNumBurials = (mummyQueryable.Count()),
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