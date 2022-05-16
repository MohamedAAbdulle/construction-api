using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class InUseToolDb
    {
        [Required]
        public int ToolId { get; set; }
        [Required]
        public int WorkerId { get; set; }
        [Required]
        public DateTime DateAssigned { get; set; }
        public int? CustomerId { get; set; }
    }
}
