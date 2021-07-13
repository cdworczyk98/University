using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessTier;
using BusinessLogicTier.Models;

namespace BusinessLogicTier.Processors
{
    public class ReportProcessor
    {
        public static List<OrderModel> Load(int id = -1)
        {
            string sql;

            if (id == -1)
                sql = @"select Id, OrderId, ProductId, CustomerId, Quantity, Total, OrderDate from dbo.tblOrder;";
            else
                sql = sql = @"select Id, OrderId, ProductId, CustomerId, Quantity, Total, OrderDate from dbo.tblOrder where OrderId=" + id + ";";

            return SqlDataAccess.LoadData<OrderModel>(sql);
        }
    }
}
