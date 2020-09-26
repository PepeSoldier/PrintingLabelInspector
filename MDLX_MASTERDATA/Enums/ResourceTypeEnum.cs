using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_BASE.Interfaces;

namespace MDLX_MASTERDATA.Enums
{
    
    public enum ResourceTypeEnum
    {
        Group = 0,
        VirtualResource = 10,
        Resource = 20,
        Subresource = 60,
        Workplace = 80,
    }
}
