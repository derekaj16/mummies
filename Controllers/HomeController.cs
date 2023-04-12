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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using Newtonsoft.Json.Linq;

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
        [HttpGet]
        public IActionResult SupervisedAnalysis()
        {

            return View();
        }
        [HttpPost]
        public IActionResult HeadDirectionAPI(HeadDirectionModel x)
        {
            HeadDirectionAPI jsonData = new HeadDirectionAPI();
            jsonData.depth = x.Depth;
            jsonData.sex_Unknown = 0;
            jsonData.sex_M = 0;
            jsonData.sex_F = 0;
            jsonData.goods_Yes = 0;
            jsonData.wrapping_Unknown = 0;
            jsonData.wrapping_B = 0;
            jsonData.wrapping_W = 0;
            jsonData.ageatdeath_A = 0;
            jsonData.ageatdeath_I = 0;
            jsonData.ageatdeath_N = 0;
            jsonData.adultsubadult_C = 0;
            jsonData.count = x.Count;
            jsonData.length = x.Length;
            if (x.Sex == "U")
            {
                jsonData.sex_Unknown = 1;
            }
            else if (x.Sex == "M")
            {
                jsonData.sex_M = 1;
            }
            else if (x.Sex == "F")
            {
                jsonData.sex_F = 1;
            }
            if (x.Goods)
            {
                jsonData.goods_Yes = 1;
            }
            if (x.Wrapping == "U")
            {
                jsonData.wrapping_Unknown = 1;
            }
            else if (x.Wrapping == "B")
            {
                jsonData.wrapping_B = 1;
            }
            else if (x.Wrapping == "W")
            {
                jsonData.wrapping_W = 1;
            }
            if (x.AgeAtDeath == "A")
            {
                jsonData.ageatdeath_A = 1;
            }
            else if (x.AgeAtDeath == "I")
            {
                jsonData.ageatdeath_I = 1;
            }
            else if (x.AgeAtDeath == "N")
            {
                jsonData.ageatdeath_N = 1;
            }
            else if (x.AgeAtDeath == "C")
            {
                jsonData.adultsubadult_C = 1;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://54.145.41.204:8080/predict");
                var postJob = client.PostAsJsonAsync<HeadDirectionAPI>("predict", jsonData);
                postJob.Wait();

                var response = postJob.Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;
                JObject jsonObject = JObject.Parse(responseContent);

                // Access the desired value by its key
                if (jsonObject.ContainsKey("prediction"))
                {
                    var desiredValue = jsonObject["prediction"].Value<string>();
                    ViewBag.postResult = desiredValue;
                }
                else
                {
                    ViewBag.postResult = "Key not found in response";
                }
                return View("Result");
            }
        }
        [HttpGet]
        public IActionResult SexPrediction()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SexPrediction(SexModel s)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://54.145.41.204:8080/predict");
                var postJob = client.PostAsJsonAsync<SexModel>("predict", s);
                postJob.Wait();

                var response = postJob.Result;
                ViewBag.postResult = response.Content.ReadAsStringAsync().Result;
                //JObject jsonObject = JObject.Parse(responseContent);

                //// Access the desired value by its key
                //if (jsonObject.ContainsKey("prediction"))
                //{
                //    var desiredValue = jsonObject["prediction"].Value<string>();
                //    ViewBag.postResult = desiredValue;
                //}
                //else
                //{
                //    ViewBag.postResult = "Key not found in response";
                //}
               return View("Result");
            }
        }

        public IActionResult UnsupervisedAnalysis()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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