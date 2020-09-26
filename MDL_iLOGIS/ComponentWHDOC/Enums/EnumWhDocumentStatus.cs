using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWHDOC.Enums
{
    public enum EnumWhDocumentStatus
    {
        deleted = -99,
        unknown = -1,
        undefined = 0,
        init = 1,
        ready = 10,
        approved = 20,
        rejected = 25,
        signed = 30,
        completed = 50,
    }
}
