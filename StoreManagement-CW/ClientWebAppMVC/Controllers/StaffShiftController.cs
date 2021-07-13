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
    public class StaffShiftController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public StaffShiftController(HttpClient httpclient, IConfiguration configuration)
        {
            _httpClient = httpclient;
            _configuration = configuration;
        }
        [Authorize(Roles = "sysadmin,shiftman,staff")]
        public async Task<IActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri(_configuration["StaffUrl"]);
            var response = await _httpClient.GetAsync("");
            var responsebody = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<List<StaffShiftModel>>(responsebody);
            return View(models);
        }

        [Authorize(Roles = "sysadmin,shiftman")]
        [Route("AddStaffShift")]
        public IActionResult AddStaffShift()
        {
            return View(new StaffShiftModel());
        }

        [Authorize(Roles = "sysadmin,shiftman")]
        [HttpPost]
        [Route("AddStaffShift")]
        public async Task<IActionResult> AddStaffShift(StaffShiftModel model)
        {

            var json = JsonConvert.SerializeObject(model);
            Debug.WriteLine(json.ToString());
            _httpClient.BaseAddress = new Uri(_configuration["StaffUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            return RedirectToAction("index");
        }

        [Authorize(Roles = "sysadmin,shiftman")]
        [HttpPost]
        [Route("UpdateStaffShift")]
        public async Task<IActionResult> UpdateStaffShift(StaffShiftModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            _httpClient.BaseAddress = new Uri(_configuration["StaffUpUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(model.ShiftID.ToString(), content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("UpdateStaffShift", new { id = model.ShiftID });
            }
            return View(model);
        }

        [Authorize(Roles = "sysadmin,shiftman")]
        [Route("UpdateStaffShift")]
        public async Task<IActionResult> UpdateStaffShift(int id)
        {
            _httpClient.BaseAddress = new Uri(_configuration["StaffUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<StaffShiftModel>(responseBody);
            return View(model);
        }
        [Authorize(Roles = "sysadmin,shiftman")]
        [HttpPost]
        [Route("DeleteStaffShift")]
        public async Task<IActionResult> DeleteStaffShift(StaffShiftModel model)
        {
            Debug.WriteLine("hereeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
            var json = JsonConvert.SerializeObject(model);
            _httpClient.BaseAddress = new Uri(_configuration["StaffDelUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.DeleteAsync(model.ShiftID.ToString());
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("hereeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee2");
                return RedirectToAction("DeleteStaffShift");
            }
            return View(model);
        }
        [Authorize(Roles = "sysadmin,shiftman")]
        [Route("DeleteStaffShift")]
        public async Task<IActionResult> DeleteStaffShift(int id)
        {
            Debug.WriteLine("hereeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee3");
            _httpClient.BaseAddress = new Uri(_configuration["StaffUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<StaffShiftModel>(responseBody);
            return View(model);
        }
    }
}
