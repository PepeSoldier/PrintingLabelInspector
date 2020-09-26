﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Enums
{
    public enum EnumPickingListItemStatus
    {
        Unknow = -1,
        Unassigned = 0, 
        //Created = 10, //6
        //CreatedEmpty = 15,
        Pending = 20, //1
        //Processing = 30, //2
        Problem = 40, //4
        Completed = 50, //3
        CompletedWithProblem = 60, //5
        Feeding = 70,
        Closed = 80, // 50dla pickera = dostarczone do linii 50
    }

    public enum EnumDeliveryListStatus
    {
        Unknow = -1,
        Unassigned = 0,
        Pending = 20,
        Processing = 30,
        Completed = 50,
        CompletedWithProblem = 60,
    }

    public enum EnumDeliveryListItemStatus
    {
        Unknow = -1,
        Unassigned = 0,
        Pending = 20,
        Completed = 50,
        CompletedWithProblem = 60,
    }
}
