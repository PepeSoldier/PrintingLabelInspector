using MDL_OTHER.ComponentHSE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MDL_BASE.Interfaces;

namespace MDL_OTHER.ComponentHSE._Interfaces
{
    public interface IDbContextOtherHSE : IDbContextCore
    {
        DbSet<SafetyCross> SafetyCrosses { get; set; }
    }
}
