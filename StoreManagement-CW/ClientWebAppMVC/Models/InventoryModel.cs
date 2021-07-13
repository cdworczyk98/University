using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientWebAppMVC.Models
{
    public class InventoryModel
    {
  
        public int InventoryId { get; set; }
        public string InventoryName { get; set; }
        public string InventoryDescription { get; set; }
        public int InventoryCount { get; set; }
        public decimal Price { get; set; }
        public bool FreeDelivery { get; set; }
    }
}

  
