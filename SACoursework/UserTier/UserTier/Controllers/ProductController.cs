using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserTier.Models;
using BusinessLogicTier;
using Newtonsoft.Json;
using IronPdf;

namespace UserTier.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Create()
        {
            ViewBag.Message = "Create Products";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                ProductProcessor.Create(model.ProductId, model.Name, model.Price, model.Stock, model.Category, model.Offer, model.Delivery);
                return RedirectToAction("List");
            }

            return View();
        }

        public ActionResult List()
        {
            ViewBag.Message = "View Products";

            var data = ProductProcessor.Load();

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

        public ActionResult Edit(int id = 0)
        {
            var data = ProductProcessor.Load(id);

            ProductModel product = new ProductModel();

            foreach (var row in data)
            {
                product = new ProductModel
                {
                    ProductId = row.ProductId,
                    Name = row.Name,
                    Price = row.Price,
                    Stock = row.Stock,
                    Category = row.Category,
                    Offer = row.Offer,
                    Delivery = row.Delivery
                };
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                ProductProcessor.Edit(model.ProductId, model.Name, model.Price, model.Stock, model.Category, model.Offer, model.Delivery);
                return RedirectToAction("List");
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var data = ProductProcessor.Load(id);

            ProductModel product = new ProductModel();

            foreach (var row in data)
            {
                product = new ProductModel
                {
                    ProductId = row.ProductId,
                    Name = row.Name,
                    Price = row.Price,
                    Stock = row.Stock,
                    Category = row.Category,
                    Offer = row.Offer,
                    Delivery = row.Delivery
                };
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(ProductModel model)
        {
            ProductProcessor.Delete(model.ProductId);
            return RedirectToAction("List");
        }
    }
}