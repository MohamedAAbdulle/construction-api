using ConstructionApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class EditSupplierAndQuotes
    {
        [Required]
        public SupplierDb Supplier { get; set; }
        [Required]
        public List<EditQuote> Quotes { get; set; }
    }
}
