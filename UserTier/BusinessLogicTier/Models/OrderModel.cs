using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicTier.Models
{
    public class OrderModel
    {
        public List<CustomerModel> CustomerModels  { get; set; }
        public List<ProductModel> ProductModels { get; set; }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
