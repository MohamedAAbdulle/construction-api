using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class InventoryDb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? Threshold { get; set; }
        [Required]
        public int? Quantity { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        public string Unit { get; set; }
        [Required]

        public DateTime ModifiedDate { get; set; }
        public int? CustomerId { get; set; }
    }
}
