using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCThreeTier.Models;
using BusinessLogicTier;
using Newtonsoft.Json;

namespace MVCThreeTier.Controllers
{
    public class HomeController : Controller
    {
        public DataPoint DataPoint
        {
            get => default;
            set
            {
            }
        }

        public CustomerModel CustomerModel
        {
            get => default;
            set
            {
            }
        }

        public ProductModel ProductModel
        {
            get => default;
            set
            {
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewProducts()
        {
            ViewBag.Message = "View Products";

            var data = ProductProcessor.LoadProducts();

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

        public ActionResult DetailsProduct(int id)
        {
            ViewBag.Message = "Product Details";

            var data = ProductProcessor.LoadProduct(id);

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

        public ActionResult CreateProduct()
        {
            ViewBag.Message = "Create Products";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(ProductModel model)
        {
            if(ModelState.IsValid)
            {
                ProductProcessor.CreateProduct(model.ProductId, model.Name, model.Price, model.Stock, model.Category, model.Offer, model.Delivery);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult EditProduct(int id = 0)
        {
            var data = ProductProcessor.LoadProduct(id);

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
        public ActionResult EditProduct(ProductModel model)
        {
            if(ModelState.IsValid)
            {
                ProductProcessor.EditProduct(model.ProductId, model.Name, model.Price, model.Stock, model.Category, model.Offer, model.Delivery);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult DeleteProduct(int id)
        {
            var data = ProductProcessor.LoadProduct(id);

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
        public ActionResult DeleteProduct(ProductModel model)
        {
            ProductProcessor.DeleteProduct(model.ProductId);
            return RedirectToAction("Index");
        }


        //-----------------------------CUSTOMER STUFF------------------------------------------------//
        //-----------------------------CUSTOMER STUFF------------------------------------------------//
        //-----------------------------CUSTOMER STUFF------------------------------------------------//
        //-----------------------------CUSTOMER STUFF------------------------------------------------//
        //-----------------------------CUSTOMER STUFF------------------------------------------------//
        //-----------------------------CUSTOMER STUFF------------------------------------------------//
        //-----------------------------CUSTOMER STUFF------------------------------------------------//

        public ActionResult CreateCustomer()
        {
            ViewBag.Message = "Create Customers";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                CustomerProcessor.CreateCustomer(model.CustomerId, model.FirstName, model.LastName, model.Email, model.PhoneNo, model.Address, model.LoyaltyCard);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult ViewCustomers()
        {
            ViewBag.Message = "View Customers";

            var data = CustomerProcessor.LoadCustomers();

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

        public ActionResult EditCustomer(int id = 0)
        {
            var data = CustomerProcessor.LoadCustomer(id);

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
        public ActionResult EditCustomer(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                CustomerProcessor.EditCustomer(model.CustomerId, model.FirstName, model.LastName, model.Email, model.PhoneNo, model.Address, model.LoyaltyCard);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult DeleteCustomer(int id)
        {
            var data = CustomerProcessor.LoadCustomer(id);

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
        public ActionResult DeleteCustomer(CustomerModel model)
        {
            CustomerProcessor.DeleteCustomer(model.CustomerId);
            return RedirectToAction("Index");
        }

        public ActionResult ViewEmail()
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

        public ActionResult ViewReports()
        {
            var data = ProductProcessor.LoadProducts();

            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (var row in data)
            {
                dataPoints.Add(new DataPoint(row.Name, row.Stock));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }
    }
}