using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class OrderObj
    {
        [Required]
        public int? Price { get; set; }
        [Required]
        public int? SupplierId { get; set; }
        [Required]
        public int? InventoryId { get; set; }
        [Required]
        public int? Quantity { get; set; }
        public int Delivered { get; set; }
    }
}
