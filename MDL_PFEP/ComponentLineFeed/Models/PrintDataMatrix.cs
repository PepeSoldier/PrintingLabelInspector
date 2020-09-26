using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_PFEP.Model.PFEP;
using MDL_PFEP.Models.PFEP;
using MDL_PRD.Model;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.ComponentLineFeed.Models
{
    public class PrintDataMatrix
    {
        public PrintDataMatrix()
        {
            Ancs = new List<PrintModel_ANC>();
            ORDER_Ancs = new List<PrintModel_Order_ANCs>();
        }

        public List<PrintModel_ANC> Ancs { get; set; }
        public List<PrintModel_Order_ANCs> ORDER_Ancs { get; set; }

        public PrintModel_Order_ANCs AddOrder(ProductionOrder order)
        {
            PrintModel_Order_ANCs order_ancs = ORDER_Ancs.FirstOrDefault(x => x.order.Id == order.Id);

            if (order_ancs == null)
            {
                ORDER_Ancs.Add(new PrintModel_Order_ANCs(order));
                order_ancs = ORDER_Ancs.FirstOrDefault(x => x.order.Id == order.Id);
            }
            return order_ancs;
        }
        public void AddANCToOrder(ProductionOrder order, PrintModel_ANC anc)
        {
            AddToAncList(anc, order);
            AddOrder(order).AddANC(anc);
        }
        private void AddToAncList(PrintModel_ANC anc, ProductionOrder order)
        {
            PrintModel_ANC anc1 = Ancs.FirstOrDefault(x => x.Id == anc.Id);
            if (anc1 == null)
            {
                anc1 = anc;
                anc1.Count = 0;
                Ancs.Add(anc1);
                //Ancs.Add(anc);
                //anc.TotalQty = order.Qty_PlannedOrRemain * anc.BomQty;
            }
            //else
            //{
            //    //anc1.TotalQty += order.Qty_PlannedOrRemain * anc.BomQty;
            //}

            anc1.TotalQty += order.Qty_PlannedOrRemain * anc.BomQty;
            anc1.Count++;
        }
    }

    public class PrintModel_Order_ANCs
    {
        public ProductionOrder order { get; set; }
        public List<PrintModel_ANC> Ancs { get; private set; }
        public int Counter { get; private set; }

        public PrintModel_Order_ANCs(ProductionOrder order)
        {
            this.order = order;
            Counter = 0;
            Ancs = new List<PrintModel_ANC>();
        }

        public void AddANC(PrintModel_ANC anc)
        {
            PrintModel_ANC anc2 = Ancs.FirstOrDefault(x => x.Id == anc.Id && x.BomQty == anc.BomQty);
            if (anc2 == null)
            {
                Ancs.Add(new PrintModel_ANC { Id = anc.Id, Code = anc.Code, Name = anc.Name, Used = false, BomQty = anc.BomQty, TotalQty = anc.BomQty * order.Qty_PlannedOrRemain });
                Counter++;
            }
            else
            {
                anc2.TotalQty += anc.BomQty * order.Qty_PlannedOrRemain;
            }
        }
        public void AddANCs(List<PrintModel_ANC> ancs)
        {
            foreach (PrintModel_ANC anc in ancs)
            {
                AddANC(anc);
            }
        }
    }
    public class PrintModel_ANC
    {
        public PrintModel_ANC() { }
        public PrintModel_ANC(Item anc)
        {
            Id = (anc != null) ? anc.Id : 0;
            Code = (anc != null) ? anc.Code : "000000000";
            Name = (anc != null) ? anc.Name : "NO NAME";
            PackageQtyPerBox = 0;
            WorkstationName = string.Empty;
            Used = false;
        }
        public PrintModel_ANC(Item anc, decimal bomQty) : this(anc)
        {
            BomQty = bomQty;
        }

        public int Id { get; set; }
        public int WorkstationOrder { get; set; }
        public string WorkstationName { get; set; }
        public string Location { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal PackageQtyPerBox { get; set; }
        public decimal PackagesCount { get; set; }
        public decimal BomQty { get; set; }
        public decimal TotalQty { get; set; }
        public decimal Count { get; set; }
        public bool Used { get; set; }

        public void SetPackageQtyPerBox(PackageItem ancpckg)
        {
            //qty per package
            PackageQtyPerBox = Convert.ToInt32(ancpckg != null && ancpckg.QtyPerPackage > 0 ? ancpckg.QtyPerPackage : 0);
            //number of packages
            //Packages = Convert.ToDecimal(ancpckg != null && ancpckg.Quantity > 0 ? Math.Round(TotalQty / ancpckg.Quantity, 2) : 0);
        }
        public void SetPackagesCount(PackageItem ancpckg, decimal sum = 0)
        {
            sum = sum > 0 ? sum : TotalQty;
            PackagesCount = Convert.ToDecimal(ancpckg != null && ancpckg.QtyPerPackage > 0 ? Math.Round(sum / ancpckg.QtyPerPackage, 2) : 0);
        }
        public void SetWorkstation(WorkstationItem ancwrkst)
        {
            if (ancwrkst != null && ancwrkst.Workstation != null)
            {
                WorkstationName = ancwrkst.Workstation.Name;
                WorkstationOrder = ancwrkst.Workstation.SortOrder;
            }
            else
            {
                WorkstationName = "N/A";
                WorkstationOrder = 9999;
            }
        }
        public void SetLocation(AncFixedLocation location)
        {
            if (location != null && location.FixedLocation != null)
            {
                this.Location = location.FixedLocation;
            }
            else
            {
                this.Location = string.Empty;
            }
        }
    }
}