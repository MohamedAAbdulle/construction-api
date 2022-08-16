using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class MiscContract
    {
        [Required]
        public int? Price { get; set; }
        [Required]
        public MiscType MiscType { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
