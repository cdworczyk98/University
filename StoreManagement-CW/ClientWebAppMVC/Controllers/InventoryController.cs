using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientWebAppMVC.Models;
using Xceed.Wpf.Toolkit;
using System.Diagnostics;

namespace WebUserInterfaceService.Controllers
{
    [Route("[controller]")]
   
    public class InventoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public InventoryController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [Authorize(Roles = "staff,shiftman,sysadmin" )]
        public async Task<IActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri(_configuration["InventoryUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<List<InventoryModel>>(responebody);
            return View(models);
        }

        [Authorize(Roles = "staff,shiftman,sysadmin")]
        [Route("AddInventory")]
        public IActionResult AddInventory()
        {
           return View(new InventoryModel()); 
        }
        [Authorize(Roles = "staff,shiftman,sysadmin")]
        [HttpPost]
        [Route("AddInventory")]
        public async Task <IActionResult> AddInventory(InventoryModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            Debug.WriteLine(json);
            _httpClient.BaseAddress = new Uri(_configuration["InventoryUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            return RedirectToAction("index");
        }

        [Authorize(Roles = "staff,shiftman")]
        [HttpPost]
        [Route("UpdateInventory")]
        public async Task<IActionResult> UpdateInventory(InventoryModel model)
        {
           
            if (model.InventoryCount < 5)
            {
                var json = JsonConvert.SerializeObject(model);
                _httpClient.BaseAddress = new Uri(_configuration["InventoryUpUrl"]);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(model.InventoryId.ToString(), content);
              

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("ShiftManagercstore@gmail.com");
                    mail.To.Add("cdworczyk@gmail.com");
                    mail.Subject = "Low stock warning";
                    mail.Body = "Hello shift manager, The following product has a warning of low stock. The stock id is " + model.InventoryId + " The name of the product is " + model.InventoryName +
                        " The description of the product is " + model.InventoryDescription + " the count of the product is " + model.InventoryCount + " the price of the product is " + model.Price;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("ShiftManagercstore@gmail.com", "Shiftman1234!");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("UpdateInventory", new { id = model.InventoryId });
                        }
                    }
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("index");
            }


        }
        [Authorize(Roles = "staff,shiftman")]
        [Route("UpdateInventory")]
        public async Task<IActionResult> UpdateInventory(int id)
        {
            
           
                _httpClient.BaseAddress = new Uri(_configuration["InventoryUrl"]);
                var response = await _httpClient.GetAsync(id.ToString());
                var responseBody = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<InventoryModel>(responseBody);
                return View(model);
           
        }
        [Authorize(Roles = "staff,shiftman")]
        [HttpPost]
        [Route("DeleteInventory")]
        public async Task <IActionResult> DeleteInventory(InventoryModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            _httpClient.BaseAddress = new Uri(_configuration["InventoryDelUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.DeleteAsync(model.InventoryId.ToString());
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DeleteInventory");
            }
            return View(model);
        }
        [Authorize(Roles = "staff,shiftman")]
        [Route("DeleteInventory")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            _httpClient.BaseAddress = new Uri(_configuration["InventoryUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<InventoryModel>(responseBody);
            return View(model);
        }


   

    }


}
