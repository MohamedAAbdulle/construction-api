using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class ContractorDb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public long? Phone { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public int? CustomerId { get; set; }
    }
}
