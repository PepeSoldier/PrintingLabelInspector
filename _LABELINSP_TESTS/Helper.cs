using MDL_LABELINSP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _LABELINSP_TESTS
{
    public class Helper
    {
        public static string TestLabelsPath { get { return @"C:\temp\IKEA\"; } }
        public static List<ItemData> ItemData = new List<ItemData>() {
            new ItemData(){ ItemCode = "911076044", ExpectedProductCode = "20385799", ExpectedWeightKG = "34", ExpectedName = "" },
            new ItemData(){ ItemCode = "911076047", ExpectedProductCode = "10475502", ExpectedWeightKG = "33", ExpectedName = "" },
            new ItemData(){ ItemCode = "911076076", ExpectedProductCode = "10475502", ExpectedWeightKG = "36", ExpectedName = "" },
            new ItemData(){ ItemCode = "911076077", ExpectedProductCode = "70475504", ExpectedWeightKG = "35", ExpectedName = "" },
            new ItemData(){ ItemCode = "911076078", ExpectedProductCode = "40475553", ExpectedWeightKG = "35", ExpectedName = "" },
            new ItemData(){ ItemCode = "911079036", ExpectedProductCode = "30385789", ExpectedWeightKG = "33", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535086", ExpectedProductCode = "80376320", ExpectedWeightKG = "37", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535219", ExpectedProductCode = "80426179", ExpectedWeightKG = "35", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535233", ExpectedProductCode = "40443901", ExpectedWeightKG = "37", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535234", ExpectedProductCode = "10443243", ExpectedWeightKG = "37", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535235", ExpectedProductCode = "90443244", ExpectedWeightKG = "37", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535236", ExpectedProductCode = "60443245", ExpectedWeightKG = "37", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535250", ExpectedProductCode = "50475425", ExpectedWeightKG = "36", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535252", ExpectedProductCode = "30475426", ExpectedWeightKG = "35", ExpectedName = "" },
            new ItemData(){ ItemCode = "911535253", ExpectedProductCode = "10475427", ExpectedWeightKG = "35", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536310", ExpectedProductCode = "60376321", ExpectedWeightKG = "40", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536377", ExpectedProductCode = "60426180", ExpectedWeightKG = "35", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536492", ExpectedProductCode = "90475616", ExpectedWeightKG = "41", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536493", ExpectedProductCode = "70475617", ExpectedWeightKG = "40", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536494", ExpectedProductCode = "50475618", ExpectedWeightKG = "40", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536495", ExpectedProductCode = "30475619", ExpectedWeightKG = "40", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536497", ExpectedProductCode = "20475610", ExpectedWeightKG = "42", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536499", ExpectedProductCode = "00475611", ExpectedWeightKG = "41", ExpectedName = "" },
            new ItemData(){ ItemCode = "911536500", ExpectedProductCode = "80475612", ExpectedWeightKG = "41", ExpectedName = "" },
            new ItemData(){ ItemCode = "911539181", ExpectedProductCode = "20376318", ExpectedWeightKG = "36", ExpectedName = "" },
            new ItemData(){ ItemCode = "911539237", ExpectedProductCode = "00426178", ExpectedWeightKG = "35", ExpectedName = "" },
        };

        public static string GetImgPath(string labelFileName)
        {
            return TestLabelsPath + labelFileName;
        }

    }
}
