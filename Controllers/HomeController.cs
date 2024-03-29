﻿using HogeschoolPXL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HogeschoolPXL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (TempData["LoginTitle"] != null)
            {
                ViewBag.LoginTitle = TempData["LoginTitle"];
                ViewBag.LoginMessage = TempData["LoginMessage"];
                ViewBag.LoginImg = TempData["LoginImg"];
                TempData.Remove("LoginTitle");
                TempData.Remove("LoginMessage");
                TempData.Remove("LoginImg");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}