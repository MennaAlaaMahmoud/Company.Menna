using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Menna.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser>  signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                if (user is null )
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is  null)
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
        public async Task< IActionResult> SingIn(SingInDto model)
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
            }    return View();
        }

        #endregion


        #region SingOut

        [HttpGet]
        public new async Task< IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(SingIn));
        }


        #endregion

    }
}
