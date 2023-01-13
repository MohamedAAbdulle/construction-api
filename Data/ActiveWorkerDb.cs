using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class ActiveWorkerDb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int WorkerId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(50)]
        public string Chart { get; set; }
        public int? CustomerId { get; set; }


    }
}
