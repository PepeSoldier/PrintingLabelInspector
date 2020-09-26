using System;

namespace MDLX_CORE.Model
{
    public class LabelExtraData
    {
        public string ExtraLabel1 { get; set; }
        public string ExtraLabel2 { get; set; }
        public string ExtraLabel3 { get; set; }
        public string ExtraData1 { get; set; }
        public string ExtraData2 { get; set; }
        public string ExtraData3 { get; set; }
    }

    public class LabelData : LabelExtraData
    {
        public string PrintDateTime { get; set; }
        public string PrintDate { get; set; }
        public string PrintTime { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Weight { get; set; }
        public string SerialNumber { get; set; }
        public string Location { get; set; }
        public string OrderNo { get; set; }
        public string Barcode { get; set; }
        public string ClientName { get; set; }
        public string MachineNumber { get; set; }
        public string WarehouseName { get; set; }
        public string Qty { get; set; }
        public string UserName { get; set; }

        private int lenLimit = 40;
        public string WarehouseName_1 { get { return WarehouseName.Substring(0, Math.Min(WarehouseName.Length, lenLimit)); } }
        public string WarehouseName_2 { get { return WarehouseName.Length > lenLimit ? WarehouseName.Substring(lenLimit, Math.Min(WarehouseName.Length- lenLimit, lenLimit)) : string.Empty; } }
    }
}