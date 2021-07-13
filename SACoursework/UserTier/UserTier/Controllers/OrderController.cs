using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserTier.Models;
using BusinessLogicTier.Processors;
using Newtonsoft.Json;
using BusinessLogicTier;

namespace UserTier.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult List()
        {
            ViewBag.Message = "View Orders";

            var data = OrderProcessor.Load();

            List<OrderModel> order = new List<OrderModel>();

            foreach (var row in data)
            {
                order.Add(new OrderModel
                {
                    OrderId = row.OrderId,
                    ProductId = row.ProductId,
                    CustomerId = row.CustomerId,
                    Quantity = row.Quantity,
                    Total = row.Total,
                    OrderDate = row.OrderDate

                });
                
            }

            return View(order);
        }

        public ActionResult Create()
         {
            ViewBag.Message = "Create Orders";

            var cdata = CustomerProcessor.Load();
            var pdata = ProductProcessor.Load();

            List<CustomerModel> customer = new List<CustomerModel>();
            List<ProductModel> product = new List<ProductModel>();
            OrderModel order = new OrderModel();

            foreach (var row in cdata)
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

            foreach (var row in pdata)
            {
                product.Add(new ProductModel
                {
                    ProductId = row.ProductId,
                    Name = row.Name,
                    Price = row.Price,
                    Stock = row.Stock,
                    Category = row.Category,
                    Offer = row.Offer,
                    Delivery = row.Delivery
                });

            }

            order.ProductModel = product;
            order.CustomerModel = customer;

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                OrderProcessor.Create(model.OrderId, model.ProductId, model.CustomerId, model.Quantity, model.OrderDate);
                return RedirectToAction("List");
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var data = OrderProcessor.Load(id);

            OrderModel order = new OrderModel();

            foreach (var row in data)
            {
                order = new OrderModel
                {
                    OrderId = row.OrderId,
                    ProductId = row.ProductId,
                    CustomerId = row.CustomerId,
                    Quantity = row.Quantity,
                    Total = row.Total,
                    OrderDate = row.OrderDate
                };
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult Delete(OrderModel model)
        {
            OrderProcessor.Delete(model.OrderId);
            return RedirectToAction("List");
        }
    }
}