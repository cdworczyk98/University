using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientWebAppMVC.Models;
using System.Diagnostics;

namespace ClientWebAppMVC.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UserController(HttpClient httpclient, IConfiguration configuration)
        {
            _httpClient = httpclient;
            _configuration = configuration;
        }
        [Authorize (Roles = "sysadmin,shiftman,staff")] 
        public async Task <IActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri(_configuration["UserUrl"]);
            var response = await _httpClient.GetAsync("");
            var responsebody = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<List<UserModel>>(responsebody);

            foreach (var item in models)
            {
                Debug.WriteLine("EMAIL ADDRESS: " + item.Email);
            }

            return View(models);
        }

        [Authorize(Roles = "sysadmin,shiftman")]
        [Route("AddUser")]
        public IActionResult AddUser()
        {
            return View(new UserModel());
        }

        [Authorize(Roles = "sysadmin,shiftman")]
        [HttpPost]
        [Route("AddUser")]
        public async Task <IActionResult> AddUser(UserModel model)
        {
            
            var json = JsonConvert.SerializeObject(model);
            Debug.WriteLine(json.ToString());
            _httpClient.BaseAddress = new Uri(_configuration["UserUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            return RedirectToAction("index");
        }

        [Authorize(Roles = "sysadmin,shiftman")]
        [HttpPost]
        [Route("UpdateUser")]
        public async Task <IActionResult> UpdateUser(UserModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            _httpClient.BaseAddress = new Uri(_configuration["UserUpUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(model.UserId.ToString(), content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Updateuser", new { id = model.UserId });
            }
            return View(model);
        }

        [Authorize(Roles = "sysadmin,shiftman")]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id )
        {
            _httpClient.BaseAddress = new Uri(_configuration["UserUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<UserModel>(responseBody);
            return View(model);
        }
        [Authorize(Roles = "sysadmin,shiftman")]
        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(UserModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            _httpClient.BaseAddress = new Uri(_configuration["UserDelUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.DeleteAsync(model.UserId.ToString());
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DeleteUser");
            }
            return View(model);
        }
        [Authorize(Roles = "sysadmin,shiftman")]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _httpClient.BaseAddress = new Uri(_configuration["UserUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<UserModel>(responseBody);
            return View(model);
        }

    }
}
