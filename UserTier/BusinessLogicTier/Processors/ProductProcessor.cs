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
        public static int Create(int productId, string name, int price, int stock, int category, int offer, bool delivery)
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

        public static List<ProductModel> Load(int id = -1)
        {
            string sql;

            if(id == -1)
                sql = @"select Id, ProductId, Name, Price, Stock, Category, Offer, Delivery from dbo.tblProduct;";
            else
                sql = @"select Id, ProductId, Name, Price, Stock, Category, Offer, Delivery from dbo.tblProduct where ProductId=" + id + ";";

            return SqlDataAccess.LoadData<ProductModel>(sql);
        }

        public static int Edit(int productId, string name, int price, int stock, int category, int offer, bool delivery)
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

        public static int Delete(int id)
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
