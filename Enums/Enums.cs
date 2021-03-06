using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionApi.Enums
{
    public enum DocumentType
    {
        Delivery,
        Invoice,
        Receipt,
        
    };

    public enum DocumentCategory
    {
        Order,

    };

    public enum InvHistType { 
        Removed =1,
        Added,
    };
    public enum OrderStatus
    {
        //inventory quantity update is dependent on this order
        Pending = 1,
        Delivered ,
        Invoiced ,
        Settled ,
    };

    public enum QuoteStatus
    {
        Created = 1,
        Modified,
        Deleted
    };

    public enum AppEnumType { 
        WorkerType=1,
    }

}
