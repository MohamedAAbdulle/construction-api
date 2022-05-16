using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class WorkerTypeDb
    {
        [Key]
        public int TypeValue { get; set; }
        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; }
        
        public int Rate { get; set; }
        public int? CustomerId { get; set; }
    }
}
