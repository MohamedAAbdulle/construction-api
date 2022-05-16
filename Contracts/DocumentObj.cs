using ConstructionApi.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class DocumentObj
    {
        [Key]
        public int Id { get; set; }
        public QuoteStatus Status { get; set; }
        [Required]
        public DocumentType FileType { get; set; }
        [Required]
        [MaxLength(50)]
        public string FileName { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
    }
}
