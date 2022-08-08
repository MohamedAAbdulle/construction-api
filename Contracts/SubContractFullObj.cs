using ConstructionApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class SubContractFullObj
    {
        [Required]
        public SubContractDb SubContract { get; set; }
        [Required]
        public List<ContractItemObj> ContractItems { get; set; }
    }
}
