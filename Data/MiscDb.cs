using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class MiscDb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? Price { get; set; }
        [Required]
        public MiscType MiscType { get; set; }
        [Required]
        public DateTime? DateCreated { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int? CustomerId { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
