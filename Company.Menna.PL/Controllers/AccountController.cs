using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Menna.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
        #endregion


        #region SingOut
        #endregion

    }
}
