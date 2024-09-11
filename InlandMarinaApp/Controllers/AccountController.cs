using MarinaData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RentalsMVC.Controllers
{
    public class AccountController : Controller
    {
        private InlandMarinaContext _context { get; set; }

        // context injected to the constructor
        public AccountController(InlandMarinaContext context)
        {
            _context = context;
        }
        // HttpGet method; default. It's IActionResult because it can be a redirect
        public IActionResult Login(string? returnUrl = "") // when user logs in, send them back to the page they requested from
        {
            if (returnUrl != "")
            {
                TempData["ReturnUrl"] = returnUrl;
            }
            return View();
        }

        // post method for login using async call to http context
        [HttpPost]
        public async Task<IActionResult> LoginAsync(Customer user) // user data from the login form
        {
            Customer usr = CustomerManager.Authenticate(_context, user.Username, user.Password);
            if (usr == null)
            {
                return View(); // stay on the login page
            }
            // authentication passed. ie. usr is not null

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usr.Username)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); // could've just type "Cookies"
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.Session.SetInt32("CurrentCustomer", usr.ID!); 

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            if (String.IsNullOrEmpty(TempData["ReturnUrl"]?.ToString())) // no return url
            {
                return RedirectToAction("Index", "Home"); // home page
            }
            else
            {
                return Redirect(TempData["ReturnUrl"]!.ToString()!);
            }
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // "Cookies" .. but text is text. 
            HttpContext.Session.Remove("CurrentCustomer");
            return RedirectToAction("Index", "Home"); // home page
        }

        public IActionResult AccessDenied() // automatically picked up by MVC to a more friendly page when failed authorization attribute.
        {
            return View();
        }
    }
}
