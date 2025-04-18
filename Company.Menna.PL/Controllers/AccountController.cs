﻿using System.Security.Claims;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;
using Company.Menna.PL.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Menna.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ITwilioService _twilioService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, ITwilioService twilioService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _twilioService = twilioService;
        }

        #region SingUp

        [HttpGet] // GET : /Account/SingUp
        public IActionResult SingUp()
        {
            return View();
        }

        // P@ssW0rd
        [HttpPost]
        public async Task<IActionResult> SingUp(SingUpDto model)
        {
            if (ModelState.IsValid) // Server sid Validation
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        // Register User
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree

                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            // Sned Email to Confirm Email
                            return RedirectToAction("SingIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }


                    }
                }
                ModelState.AddModelError("", "Invalid SingUp !!");

            }

            return View(model);
        }

        #endregion




        #region SingIn

        [HttpGet]
        public IActionResult SingIn()
        {
            return View();
        }

        //P@ssW0rd
        [HttpPost] // Account/SingIn
        public async Task<IActionResult> SingIn(SingInDto model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (flag)
                    {
                        // SignIn the user
                        var result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        }

                    }

                }
                ModelState.AddModelError("", "Invalid Login !!");

            }
            return View(model);
        }

        #endregion


        #region SingOut

        [HttpGet]
      
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(SingIn));
        }


        #endregion

        #region  Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate Token

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);



                    // Create URL
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);



                    // Create Email 

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url

                    };

                    // Send Email
                    //var flag = EmailSettings.SendEmail(email);
                    //if (flag)
                    //{
                    //    //  Check Your Index

                    //    return RedirectToAction("CheckYourIndex");

                    //}

                    _mailService.SendEmail(email);

                    return RedirectToAction("CheckYourIndex");


                }
                ModelState.AddModelError("", "Invalid Reset Password  !!");


            }

            return View("ForgetPassword", model);
        }
        #endregion


        #region CheckYourPhone

        [HttpGet]
        public IActionResult CheckYourPhone()
        {
            return View();
        }
        #endregion

        [HttpPost]

        public async Task<IActionResult> SendResentPasswordSms(ForgetPasswordDto model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {

                    // Generate Token 
                    var Token = await _userManager.GeneratePasswordResetTokenAsync(user);


                    // Create URL 

                    //var url = Url.Action("ResetPassword", "Account", new { model.Email, Token }, Request.Scheme);

                    var PasswordURL = Url.Action("ResetPassword", "Account", new { email = model.Email, Token }, Request.Scheme);

                    // Create Sms 

                    var sms = new Sms()
                    {
                        PhoneNumber = user.PhoneNumber,
                        Body = PasswordURL
                    };

                    // Send Email 

                    // Old Way :
                    //var flag = EmailSetting.SendEmail(email);

                    // New Way :

                    _twilioService.SendSms(sms);

                    // Send Check Your Phone 
                    return RedirectToAction(nameof(CheckYourPhone));
                    //  return Ok("Check Your Phone");
                }


            }
            ModelState.AddModelError("", "Invalid Resert Password ");

            return View("ForgetPassword", model);
        }


        #region CheckYourIndex

        [HttpGet]
        public IActionResult CheckYourIndex()
        {
            return View();
        }
        #endregion


        public IActionResult AccessDenied()
        {
            return View();
        }




        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null || token is null) return BadRequest("Invalid Operation");
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SingIn");
                    }

                }
                ModelState.AddModelError("", "Invalid Reset Password Operation  !!");

            }
            return View();
        }


        #endregion



        public IActionResult GoogleLogin ()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);

        }

        public async Task <IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
                Claim => new
                {
                    Claim.Type,
                    Claim.Value,
                    Claim.Issuer,
                    Claim.OriginalIssuer
                }


                );

            return RedirectToAction("Index", "Home");

        }




        public IActionResult FacebookLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("FacebookResponse")
            };
            return Challenge(prop, FacebookDefaults.AuthenticationScheme);

        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
                Claim => new
                {
                    Claim.Type,
                    Claim.Value,
                    Claim.Issuer,
                    Claim.OriginalIssuer
                }


                );

            return RedirectToAction("Index", "Home");

        }

    }
}