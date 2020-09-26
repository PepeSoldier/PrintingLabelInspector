using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_ONEPROD.ComponentMes._Interfaces
{
    public interface ISerialNumberGenerator
    {
        int GenerateSerialNumber(int resourceId);
    }
}
