using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA_Labels_TEST
{
    public class Helper
    {
        public static string TestLabelsPath { get { return @"C:\temp\IKEA\"; } }

        public static string GetImgPath(string labelFileName)
        {
            return TestLabelsPath + labelFileName;
        }

    }
}
