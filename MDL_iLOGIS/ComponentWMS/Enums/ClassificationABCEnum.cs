using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Enums
{
    public enum ClassificationABCEnum
    {
        Unknow = -1,
        Undefined = 0,
        /// <summary>A - materiały najdroższe, ich wartość wynosi około 80% całości, ogólna masa wynosi mniej więcej 20%</summary>
        A = 1,
        /// <summary>B - materiały średniej wartości, ich wartość wynosi około 15% całości, ogólna masa wynosi mniej więcej 30%</summary>
        B = 2,
        /// <summary>C - materiały niskiej wartości, ich wartość wynosi około 5% całości, ogólna masa wynosi mniej więcej 50%</summary>
        C = 3,
    }
}
