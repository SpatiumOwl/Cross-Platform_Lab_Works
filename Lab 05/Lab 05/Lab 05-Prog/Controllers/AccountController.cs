using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace Lab_05_Prog.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignIn()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return Challenge(OktaDefaults.MvcAuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public new IActionResult SignOut()
        {
            return new SignOutResult(
                new[]
                {
                    OktaDefaults.MvcAuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme
                },
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = "/Home/"});
        }
        [HttpGet]
        public IActionResult Profile()
        {
            string phoneNumber;
            if (HttpContext.User.Claims.Where(x => x.Type == "phone_number").Count() != 0)
                phoneNumber = HttpContext.User.Claims.Where(x => x.Type == "phone_number").FirstOrDefault().Value.ToString();
            else
                phoneNumber = "";
            return View(new Models.UserProfileModel()
            {
                Email = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault().Value.ToString(),
                FirstName = HttpContext.User.Claims.Where(x => x.Type == "given_name").FirstOrDefault().Value.ToString(),
                LastName = HttpContext.User.Claims.Where(x => x.Type == "family_name").FirstOrDefault().Value.ToString(),
                UserName = HttpContext.User.Claims.Where(x => x.Type == "preferred_username").FirstOrDefault().Value.ToString(),
                PhoneNumber = phoneNumber
            });
        }
    }
}
