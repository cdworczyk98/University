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
    public class ReportController : Controller
    {
        public ActionResult BarGraph()
        {
            var data = ProductProcessor.Load();

            List<BarDataPointModel> dataPoints = new List<BarDataPointModel>();

            foreach (var row in data)
            {
                dataPoints.Add(new BarDataPointModel(row.Name, row.Stock));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        public ActionResult LineGraph()
        {
            List<LineDataPointModel> dataPoints1 = new List<LineDataPointModel>();
            List<LineDataPointModel> dataPoints2 = new List<LineDataPointModel>();
            List<LineDataPointModel> dataPoints3 = new List<LineDataPointModel>();

            var odata = OrderProcessor.Load();
            var pdata = ProductProcessor.Load();

            List<float> foodTotals = new List<float>(new float[] { 0,0,0,0});
            List<float> toyTotals = new List<float>(new float[] { 0, 0, 0,0 });
            List<float> elecTotals = new List<float>(new float[] { 0, 0, 0,0 });



            foreach (var order in odata)
            {
                var product = pdata.Find(x => x.ProductId == order.ProductId);

                ProductModel productModel = new ProductModel
                {
                    Category = product.Category
                };

                switch (order.OrderDate.Month)
                {
                    case 1:
                        switch (product.Category)
                        {
                            case 0:
                                toyTotals[0] += order.Total;
                                break;
                            case 1:
                                elecTotals[0] += order.Total;
                                break;
                            case 2:
                                foodTotals[0] += order.Total;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        switch (product.Category)
                        {
                            case 0:
                                toyTotals[1] += order.Total;
                                break;
                            case 1:
                                elecTotals[1] += order.Total;
                                break;
                            case 2:
                                foodTotals[1] += order.Total;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 3:
                        switch (product.Category)
                        {
                            case 0:
                                toyTotals[2] += order.Total;
                                break;
                            case 1:
                                elecTotals[2] += order.Total;
                                break;
                            case 2:
                                foodTotals[2] += order.Total;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 4:
                        switch (product.Category)
                        {
                            case 0:
                                toyTotals[3] += order.Total;
                                break;
                            case 1:
                                elecTotals[3] += order.Total;
                                break;
                            case 2:
                                foodTotals[3] += order.Total;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            dataPoints1.Add(new LineDataPointModel("Jan", foodTotals[0]));
            dataPoints1.Add(new LineDataPointModel("Feb", foodTotals[1]));
            dataPoints1.Add(new LineDataPointModel("Mar", foodTotals[2]));
            dataPoints1.Add(new LineDataPointModel("Apr", foodTotals[3]));

            dataPoints2.Add(new LineDataPointModel("Jan", toyTotals[0]));
            dataPoints2.Add(new LineDataPointModel("Feb", toyTotals[1]));
            dataPoints2.Add(new LineDataPointModel("Mar", toyTotals[2]));
            dataPoints2.Add(new LineDataPointModel("Apr", toyTotals[3]));

            dataPoints3.Add(new LineDataPointModel("Jan", elecTotals[0]));
            dataPoints3.Add(new LineDataPointModel("Feb", elecTotals[1]));
            dataPoints3.Add(new LineDataPointModel("Mar", elecTotals[2]));
            dataPoints3.Add(new LineDataPointModel("Apr", elecTotals[3]));

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);

            return View();
        }

        public ActionResult PieChart()
        {
            List<PieDataPointModel> dataPoints = new List<PieDataPointModel>();

            dataPoints.Add(new PieDataPointModel("Samsung", 25));
            dataPoints.Add(new PieDataPointModel("Micromax", 13));
            dataPoints.Add(new PieDataPointModel("Lenovo", 8));
            dataPoints.Add(new PieDataPointModel("Intex", 7));
            dataPoints.Add(new PieDataPointModel("Reliance", 6.8));
            dataPoints.Add(new PieDataPointModel("Others", 40.2));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }
    }
}