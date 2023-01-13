using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class InvQuantity
    {
        [Required]
        public int? Quantity { get; set; }
        [Required]
        public InvHistType Type { get; set; }
        [Required]
        public DateTime? ModifiedDate { get; set; }
    }

}
