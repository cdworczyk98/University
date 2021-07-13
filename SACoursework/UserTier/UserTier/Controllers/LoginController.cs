using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserTier.Models;
using BusinessLogicTier;
using Newtonsoft.Json;
using BusinessLogicTier.Processors;

namespace UserTier.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult LoginUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(LoginModel objUser)
        {
            if(ModelState.IsValid)
            {
                var data = LoginProcessor.LoadUser(objUser.Username, objUser.Password);

                List<LoginModel> login = new List<LoginModel>();

                foreach (var user in data)
                {
                    login.Add(new LoginModel
                    {
                        Username = user.Username,
                        Password = user.Password
                    });
                }

                if(login.Count == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("IncorrectLogin", "Login");
        }

        public ActionResult IncorrectLogin()
        {
            return View();
        }
    }
}