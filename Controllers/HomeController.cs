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

            private MummyDbContext mummyContext { get; set; }

            private IMummyRepository repo;

            private ApplicationDbContext applicationContext;
            private UserManager<IdentityUser> _userManager;
            private RoleManager<IdentityRole> _roleManager;


            public HomeController(ILogger<HomeController> logger, MummyDbContext context, IMummyRepository temp, ApplicationDbContext appContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                _logger = logger;
                repo = temp;
                applicationContext = appContext;
                mummyContext = context;
                _userManager = userManager;
                _roleManager = roleManager;
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
            public IActionResult BurialInfoAsync(string category, int pageNum = 1)
            {
                int pageSize = 30;
                //var currentUser = await _userManager.GetUserAsync(User);

                // Check if the user is in the "Admin" role
                //ViewBag.isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

                IQueryable<Mummy> mummyQueryable = repo.GetBurials(); // Get empty filtering
            
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
                IQueryable<Mummy> mummyQueryable = repo.GetBurials(new Dictionary<string, string?> { { "Ageatdeath", Request.Form["Ageatdeath"] }, { "Haircolor", Request.Form["Haircolor"] }, { "Sex", Request.Form["Sex"] }, { "Wrapping", Request.Form["Wrapping"] }, { "Depth", Request.Form["Depth"] }, { "Northsouth", Request.Form["Northsouth"] }, { "Eastwest", Request.Form["Eastwest"] }, { "Squarenorthsouth", Request.Form["Squarenorthsouth"] }, { "Squareeastwest", Request.Form["Squareeastwest"] }, { "Area", Request.Form["Area"] } });

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
                        Squareeastwest = Request.Form["Squareeastwest"],
                        Area = Request.Form["Area"]
                    },

                    formValues = new FormValues()
                };
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
            public IActionResult SexPrediction(SexModel jsonData)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://3.82.139.123:8080/predict");
                    var postJob = client.PostAsJsonAsync<SexModel>("predict", jsonData);
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

            public IActionResult UnsupervisedAnalysis()
            {
                return View();
            }

            public IActionResult Account()
            {
                return View();
            }
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> Admin()
            {
                //var users = _userManager.Users;
                var users = new List<EditUserViewModel>();
                foreach (var user in _userManager.Users)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var userRoleViewModel = new EditUserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Roles = (List<string>)userRoles
                    };
                    users.Add(userRoleViewModel);
                }
                return View(users);
            }
            [Authorize(Roles = "Admin")]
            [HttpGet]
            public async Task<IActionResult> EditUser(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                var userRoles = await _userManager.GetRolesAsync(user);
                var model = new EditUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = (List<string>)userRoles

                };
                return View("UserForm", model);
            }
            [Authorize(Roles = "Admin")]
            [HttpPost]
            public async Task<IActionResult> EditUser(EditUserViewModel model)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                user.Email = model.Email;
                user.UserName = model.UserName;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("UserForm", model);
            }
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> DeleteUser(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("Admin");
            }
            [Authorize(Roles = "Admin")]
            [HttpGet]
            public async Task<IActionResult> ManageAdmin()
            {
                var roleId = "1";
                var role = await _roleManager.FindByIdAsync(roleId);
                var model = new List<UserRoleViewModel>();
                foreach(var user in _userManager.Users)
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,

                    };
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected= false;
                    }
                    model.Add(userRoleViewModel);

                }
                return View("RoleManager", model);
            }
            [Authorize(Roles = "Admin")]
            [HttpPost]
            public async Task<IActionResult> ManageAdmin(List<UserRoleViewModel> model, string roleId)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                for (int i =0; i < model.Count; i++)
                {
                    var user = await _userManager.FindByIdAsync(model[i].UserId);
                    IdentityResult result = null;
                    if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                }
                return RedirectToAction("Admin");
            }

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