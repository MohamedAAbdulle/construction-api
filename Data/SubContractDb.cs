using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class SubContractDb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ContractorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int? TotalPrice { get; set; }
        [Required]
        public ContractStatus Status { get; set; }
        public DateTime LastModified { get; set; }
        public int? CustomerId { get; set; }

    }
}
