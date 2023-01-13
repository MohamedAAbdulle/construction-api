using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class InUseToolObj
    {
        [Required]
        public int? ToolId { get; set; }
        [Required]
        public int? WorkerId { get; set; }
        [Required]
        public int? Amount { get; set; }
        public DateTime? DateAssigned { get; set; }
    }
}
