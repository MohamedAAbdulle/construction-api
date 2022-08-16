using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class SiteCashDb
    {
        [Required]
        public int? CustomerId { get; set; }
        [Required]
        public int? Amount { get; set; }
        [Required]
        public DateTime? LastModified { get; set; }
        
        
    }
}
