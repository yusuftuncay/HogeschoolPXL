﻿using HogeschoolPXL.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HogeschoolPXL.Controllers
{
    public class AccountController : Controller
	{
		UserManager<IdentityUser> _userManager;
		SignInManager<IdentityUser> _signInManager;
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		#region login
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		// returnUrl works but has a problem with the Popup Toast after login that appears on the Home Page only
		// Uncomment the commented lines below to test returnUrl
		public async Task<IActionResult> LoginAsync(LoginViewModel login/*, string returnUrl*/)
        {
            if (login.Email == null || login.Password == null)
                return View("Login");

            var signInResult = await _signInManager.PasswordSignInAsync(
			login.Email, login.Password, false, lockoutOnFailure: false);

			if (signInResult.Succeeded)
			{
                TempData["Login"] = "Succeeded";
                //if (!string.IsNullOrEmpty(returnUrl))
                //    return Redirect(returnUrl);
                //else
					return RedirectToAction("Index", "Home");
            }
			else
			{
                ModelState.AddModelError("", "Something went wrong");
                return View("Login");
            }
		}
		#endregion

        #region register
        [HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> RegisterAsync(RegisterViewModel register)
		{
			var identityUser = new IdentityUser
			{
				Email = register.Email,
				UserName = register.Email
			};
			var identityResult = await _userManager.CreateAsync(identityUser, register.Password);

			if (identityResult.Succeeded)
				return View("RegistrationCompleted");
			foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View();
		}
		#endregion

		#region logout
		[HttpGet]
		public async Task<IActionResult> LogOut()
		{
            TempData["Login"] = "LogOut";

            await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home"); // RedirectToAction Belangrijk!!
		}
		#endregion
	}

}
