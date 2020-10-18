using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDLX_CORE.Interfaces
{
    public interface IDefModel : IModelEntity
    {
        string Name { get; set; }
        bool Deleted { get; set; }
    }

    public interface IDefComboModel : IDefModel
    {
        int? AreaId { get; set; }
    }


}