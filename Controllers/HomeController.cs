using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mummies.Models;
using Mummies.Data;
using Mummies.Models;
using System.Linq;
using mummies.Models.ViewModels;
using Mummies.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace mummies.Controllers
{

        public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private MummyDbContext mummyContext;
        private ApplicationDbContext applicationContext;
        private UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, MummyDbContext context, ApplicationDbContext appContext, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            mummyContext = context;
            applicationContext = appContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult BurialInfoAsync(string category, int pageNum = 1)
        {
            int pageSize = 30;
            //var currentUser = await _userManager.GetUserAsync(User);

            // Check if the user is in the "Admin" role
            //ViewBag.isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddBurial()
        {
            return View("BurialForm");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddBurial(Burialmain b)
        {
            if (ModelState.IsValid)
            {
                mummyContext.Add(b);
                mummyContext.SaveChanges();
                return RedirectToAction("BurialInfo");
            }
            //if form does not validate sends user back to the form
            else
            {
                return View("BurialForm", b);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditBurial(long burialId)
        {
            var mummy = mummyContext.Burialmains.Single(x => x.Id == burialId);
            return View("BurialForm", mummy);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditBurial(Burialmain b)
        {
            mummyContext.Update(b);
            mummyContext.SaveChanges();
            return RedirectToAction("BurialInfo");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeleteBurial(long burialId)
        {
            var mummy = mummyContext.Burialmains.Single(x => x.Id == burialId);
            return View(mummy);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteBurial(Burialmain b)
        {
            mummyContext.Burialmains.Remove(b);
            mummyContext.SaveChanges();
            return RedirectToAction("BurialInfo");
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