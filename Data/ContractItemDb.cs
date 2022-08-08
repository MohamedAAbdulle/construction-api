using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class ContractItemDb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SubContractId { get; set; }
        [Required]
        public int? Price { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ContractStatus Status { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public int? CustomerId { get; set; }

    }
}
