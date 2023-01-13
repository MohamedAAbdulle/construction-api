using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class InventoryHistoryDb
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public int? InvId { get; set; }
        [Required]
        public int? Quantity { get; set; }
        [Required]
        public InvHistType Type { get; set; }
        [Required]
        public DateTime? DateDone { get; set; }
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }

    }
}
