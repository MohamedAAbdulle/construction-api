using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class WorkerDb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        public byte WorkerType { get; set; }
        [Required]
        public string IdNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int? CustomerId { get; set; }


    }
}
