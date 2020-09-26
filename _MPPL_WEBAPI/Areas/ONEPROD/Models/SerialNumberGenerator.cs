using MDL_ONEPROD.ComponentMes._Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEBAPI.Areas.ONEPROD.Models
{
    public class SerialNumberGenerator : ISerialNumberGenerator
    {
        public int GenerateSerialNumber(int resourceGroup)
        {
            return 1;
        }
    }
}