﻿using System;
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
        DailyUsage =1,
        Delivery,
        CorrectionRemove,
        CorrectionAdd,
        UndoDelivery
    };
    public enum OrderStatus
    {
        //inventory quantity update is dependent on this order
        Ordered = 1,
        Delivered ,
        Paid ,
    };

    public enum ContractStatus
    {
        Ready = 1,
        Inprogress,
        Completed,
        Invoiced,
        Settled,
    };

    public enum EditedAction
    {
        Created = 1,
        Modified,
        Deleted
    };

    public enum AppEnumType { 
        WorkerType=1,
    }

    public enum MiscType
    {
        Buy = 1,
        Fine,
        Salary,
        Correction,
        Others,
        Deposit
    }

}
