using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.DTO;

namespace WebShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountDAO _accountDao;

        public AccountController(AccountDAO accountDao)
        {
            _accountDao = accountDao;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var account = _accountDao.Login(loginModel.Email, loginModel.Password);
                if (account != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Email, account.Email)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Optional: you can set properties like IsPersistent, ExpiresUtc, etc.
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreateAccountModel createAccountModel)
        {
            if (ModelState.IsValid)
            {
                // Create account logic
                var newAccount = new Account
                {
                    UserName = createAccountModel.UserName,
                    Email = createAccountModel.Email,
                    Password = createAccountModel.Password
                };

                // Call the Add method on the AccountDAO to save the new account
                var newAccountId = _accountDao.Add(newAccount);

                if (newAccountId > 0)
                {
                    // Redirect to login page after successful account creation
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Error creating account.");
            }
            return View(createAccountModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the home page or login page after logging out
            return RedirectToAction("Index", "Home");
        }


    }
}
