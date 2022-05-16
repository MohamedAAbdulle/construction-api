using ConstructionApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Data
{
    public class DocumentDb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public DocumentCategory FileCategory { get; set; }
        [Required]
        public DocumentType FileType { get; set; }
        [Required]
        [MaxLength(50)]
        public string FileName { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public int? CustomerId { get; set; }
    }
}
