using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class EditQuote
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public QuoteStatus Status { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public int? InventoryId { get; set; }
        [Required]
        public float? Amount { get; set; }
        [Required]
        public int? Price { get; set; }

    }
}
