using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserTier.Models;
using BusinessLogicTier.Processors;
using Newtonsoft.Json;

namespace UserTier.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Create()
        {
            ViewBag.Message = "Create Customers";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                CustomerProcessor.Create(model.CustomerId, model.FirstName, model.LastName, model.Email, model.PhoneNo, model.Address, model.LoyaltyCard);
                return RedirectToAction("List");
            }

            return View();
        }

        public ActionResult List()
        {
            ViewBag.Message = "View Customers";

            var data = CustomerProcessor.Load();

            List<CustomerModel> customer = new List<CustomerModel>();

            foreach (var row in data)
            {
                customer.Add(new CustomerModel
                {
                    CustomerId = row.CustomerId,
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Email = row.Email,
                    PhoneNo = row.PhoneNo,
                    Address = row.Address,
                    LoyaltyCard = row.LoyaltyCard
                });
            }

            return View(customer);
        }

        public ActionResult Edit(int id = 0)
        {
            var data = CustomerProcessor.Load(id);

            CustomerModel customer = new CustomerModel();

            foreach (var row in data)
            {
                customer = new CustomerModel
                {
                    CustomerId = row.CustomerId,
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Email = row.Email,
                    PhoneNo = row.PhoneNo,
                    Address = row.Address,
                    LoyaltyCard = row.LoyaltyCard
                };
            }

            return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                CustomerProcessor.Edit(model.CustomerId, model.FirstName, model.LastName, model.Email, model.PhoneNo, model.Address, model.LoyaltyCard);
                return RedirectToAction("List");
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var data = CustomerProcessor.Load(id);

            CustomerModel customer = new CustomerModel();

            foreach (var row in data)
            {
                customer = new CustomerModel
                {
                    CustomerId = row.CustomerId,
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Email = row.Email,
                    PhoneNo = row.PhoneNo,
                    Address = row.Address,
                    LoyaltyCard = row.LoyaltyCard
                };
            }

            return View(customer);
        }

        [HttpPost]
        public ActionResult Delete(CustomerModel model)
        {
            CustomerProcessor.Delete(model.CustomerId);
            return RedirectToAction("List");
        }
    }
}