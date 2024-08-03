using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.DTO;
using Microsoft.AspNetCore.Identity;

namespace WebShop.Controllers
{
    public class AccountController : Controller
    {
        AccountDAO _accountDao;

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
                new Claim(ClaimTypes.Email, account.Email)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Optional: you can set properties like IsPersistent, ExpiresUtc, etc.
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginModel);
        }


        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountModel createAccountModel)
        {
            if (ModelState.IsValid)
            {
                var newAccount = new Account
                {
                    Email = createAccountModel.Email,
                    Password = createAccountModel.Password // Plain text
                };

                var newAccountId = _accountDao.Add(newAccount);

                if (newAccountId > 0)
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Error creating account.");
            }
            return View(createAccountModel);
        }




        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }

}


