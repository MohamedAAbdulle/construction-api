using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Contracts
{
    public class MyRequest
    {
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
    }
}
