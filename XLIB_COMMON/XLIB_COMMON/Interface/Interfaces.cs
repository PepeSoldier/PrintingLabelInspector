using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDLX_CORE.Interfaces
{
    public interface IModelEntity
    {
        int Id { get; set; }
    }

    public interface IModelDeletableEntity : IModelEntity
    {
        bool Deleted { get; set; }
    }
}