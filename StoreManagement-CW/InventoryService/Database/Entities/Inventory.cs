using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Database.Entities
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        [Required(ErrorMessage = "Item name must be at least 5 characters")]
        [StringLength(25, MinimumLength = 5)]
        public string InventoryName { get; set; }
        [Required(ErrorMessage = "Item description must be at least 5 characters")]
        [StringLength(30, MinimumLength = 5)]
        public string InventoryDescription { get; set; }
        [Required(ErrorMessage = "Must enter a valid number")]
        public int InventoryCount { get; set; }
        [Required(ErrorMessage = "Must enter a valid price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public bool FreeDelivery { get; set; }


    }
}
