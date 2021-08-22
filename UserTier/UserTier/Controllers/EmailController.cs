using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserTier.Models;
using BusinessLogicTier;
using Newtonsoft.Json;

namespace UserTier.Controllers
{
    public class EmailController : Controller
    {
        public ActionResult LowStock()
        {
            ViewBag.Message = "View Emails";

            var data = ProductProcessor.GetLowStock();

            List<ProductModel> product = new List<ProductModel>();

            foreach (var row in data)
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

            return View(product);
        }
    }
}