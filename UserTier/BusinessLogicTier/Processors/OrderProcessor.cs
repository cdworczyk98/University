using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessTier;
using BusinessLogicTier.Models;

namespace BusinessLogicTier.Processors
{
    public class OrderProcessor
    {
        public static int Create(int orderId, int productId, int customerId, int quantity, DateTime orderDate)
        {
            var productData = ProductProcessor.Load(productId);
            var customerData = CustomerProcessor.Load(customerId);

            List<ProductModel> productList = new List<ProductModel>();
            ProductModel product = new ProductModel();

            List<CustomerModel> customerList = new List<CustomerModel>();
            CustomerModel customer = new CustomerModel();

            foreach (var row in productData)
            {
                productList.Add(new ProductModel
                {
                    ProductId = row.ProductId,
                    Name = row.Name,
                    Price = row.Price,
                    Stock = row.Stock,
                    Category = row.Category,
                    Offer = row.Offer,
                    Delivery = row.Delivery,
                });
            }

            foreach (var row in customerData)
            {
                customerList.Add(new CustomerModel
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

            customer = customerList[0];
            product = productList[0];

            float offer;

            switch (customer.LoyaltyCard)
            {
                case 1:
                    offer = 0.05f;
                    break;
                case 2:
                    offer = 0.15f;
                    break;
                default:
                    offer = 0;
                    break;
            }


            OrderModel orderData = new OrderModel
            {
                OrderId = orderId,
                ProductId = productId,
                CustomerId = customerId,
                Quantity = quantity,
                Total = (quantity * product.Price) - ((quantity * product.Price) * offer),
                OrderDate = orderDate


            };


            string sql = @"insert into dbo.tblOrder (OrderId, ProductId, CustomerId, Quantity, Total, OrderDate)
                            values (@OrderId, @ProductId, @CustomerId, @Quantity, @Total, @OrderDate);";

            return SqlDataAccess.SaveData(sql, orderData);
        }

        public static List<OrderModel> Load(int id = -1)
        {
            string sql;

            if (id == -1)
                sql = @"select Id, OrderId, ProductId, CustomerId, Quantity, Total, OrderDate from dbo.tblOrder;";
            else
                sql = sql = @"select Id, OrderId, ProductId, CustomerId, Quantity, Total, OrderDate from dbo.tblOrder where OrderId=" + id + ";";

            return SqlDataAccess.LoadData<OrderModel>(sql);
        }

        public static int Edit(int orderId, int productId, int customerId, int quantity, int total, DateTime orderDate)
        {
            OrderModel data = new OrderModel
            {
                OrderId = orderId,
                ProductId = productId,
                CustomerId = customerId,
                Quantity = quantity,
                Total = total,
                OrderDate = orderDate

            };

            string sql = @"UPDATE dbo.tblOrder SET OrderId=@OrderId, ProductId=@ProductId, CustomerId=@CustomerId, Quantity=@Quantity, Total=@Total, OrderDate=@OrderDate WHERE OrderId=@OrderId;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int Delete(int id)
        {
            string sql = @"DELETE FROM dbo.tblOrder WHERE OrderId=" + id + ";";

            return SqlDataAccess.DeleteData(sql);
        }
    }
}
