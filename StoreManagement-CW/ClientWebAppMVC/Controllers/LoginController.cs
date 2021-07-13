using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using ClientWebAppMVC.Models;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ClientWebAppMVC.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly HttpClient _httpclient;
        private readonly IConfiguration _configuration;


        public LoginController(HttpClient httpclient, IConfiguration configuration)
        {
            _httpclient = httpclient;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View(new LoginModel());
        }
        [HttpPost]
        [Route("LoginAsync")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                _httpclient.BaseAddress = new Uri(_configuration["UserLoginUrl"]);
                var response = await _httpclient.GetAsync("");
                var responsebody = await response.Content.ReadAsStringAsync();
                var models = JsonConvert.DeserializeObject<List<UserModel>>(responsebody);


                var claims = new List<Claim>();
                foreach (var item in models)
                {

                    if (item.Email == model.Email && item.Password == model.Password)
                    {
                        if (item.UserType == "staff") { claims.Add(new Claim(ClaimTypes.Role, "staff")); }
                        else if (item.UserType == "shiftman") { claims.Add(new Claim(ClaimTypes.Role, "shiftman")); }
                        else if (item.UserType == "sysadmin") { claims.Add(new Claim(ClaimTypes.Role, "sysadmin")); }
                        else { ModelState.AddModelError("Email", "Email is not found in database."); }
                    }
                  
                }

                if(claims.Count > 0)
                {
                    var claimsIdentity = new ClaimsIdentity
                    (
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                     );

                    var authProperties = new AuthenticationProperties()
                    {
                        IsPersistent = false,
                        IssuedUtc = DateTime.Now
                    };

                    await HttpContext.SignInAsync
                    (
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );
                }

                

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [HttpGet]
        [Route("LogoutAsync")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login","Login");
        }
    }
}
