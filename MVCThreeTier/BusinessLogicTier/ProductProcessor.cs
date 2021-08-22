using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DataAccessTier;
using BusinessLogicTier.Models;

namespace BusinessLogicTier
{
    public class ProductProcessor
    {
        public static int CreateProduct(int productId, string name, int price, int stock, int category, int offer, bool delivery)
        {
            ProductModel data = new ProductModel
            {
                ProductId = productId,
                Name = name,
                Price = price,
                Stock = stock,
                Category = category,
                Offer = offer,
                Delivery = delivery

            };

            string sql = @"insert into dbo.tblProduct (ProductId, Name, Price, Stock, Category, Offer, Delivery)
                            values (@ProductId, @Name, @Price, @Stock, @Category, @Offer, @Delivery);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ProductModel> LoadProducts()
        {
            string sql = @"select Id, ProductId, Name, Price, Stock, Category, Offer, Delivery from dbo.tblProduct;";

            return SqlDataAccess.LoadData<ProductModel>(sql);
        }

        public static List<ProductModel> LoadProduct(int id)
        {
            string sql = @"select Id, ProductId, Name, Price, Stock, Category, Offer, Delivery from dbo.tblProduct where ProductId=" + id + ";";

            return SqlDataAccess.LoadData<ProductModel>(sql);
        }

        public static int EditProduct(int productId, string name, int price, int stock, int category, int offer, bool delivery)
        {
            ProductModel data = new ProductModel
            {
                ProductId = productId,
                Name = name,
                Price = price,
                Stock = stock,
                Category = category,
                Offer = offer,
                Delivery = delivery

            };

            string sql = @"UPDATE dbo.tblProduct SET ProductId=@ProductId, Name=@Name, Price=@Price, Stock=@Stock, Category=@Category, Offer=@Offer, Delivery=@Delivery WHERE ProductId=@ProductId;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteProduct(int id)
        {
            string sql = @"DELETE FROM dbo.tblProduct WHERE ProductId=" + id + ";";

            return SqlDataAccess.DeleteData(sql);
        }

        public static List<ProductModel> GetLowStock()
        {
            string sql = @"select Id, ProductId, Name, Price, Stock, Category, Offer, Delivery from dbo.tblProduct where Stock < 10;";

            return SqlDataAccess.LoadData<ProductModel>(sql);
        }
    }
}
