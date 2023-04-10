﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mummies.Models;
using Mummies.Data;
using Mummies.Models;

namespace mummies.Controllers;

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

    public IActionResult BurialInfo()
    {
        return View();
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

    public IActionResult ManageAccounts()
    {
        return View();
    }

    public IActionResult BurialDetails()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
