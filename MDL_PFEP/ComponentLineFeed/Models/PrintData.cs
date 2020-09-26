using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.ComponentLineFeed.Models
{

    //public class PrintData
    //{
    //    public PrintData()
    //    {
    //        ANC_Orders = new List<PrintModelANC_Order>();
    //        ORDER_Ancs = new List<PrintModelORDER_Ancs>();
    //    }

    //    public List<PrintModelANC_Order> ANC_Orders { get; set; }
    //    public List<PrintModelORDER_Ancs> ORDER_Ancs { get; set; }
    //    public List<PrintModelANCs_Orders> ANCs_Orders_ManyToMany { get; set; }

    //    public PrintModelANC_Order AddAnc(Anc2 anc)
    //    {
    //        PrintModelANC_Order printDefModelANC = ANC_Orders.FirstOrDefault(x => x.Anc2.Id == anc.Id && x.Anc2.BomQty == anc.BomQty);

    //        if (printDefModelANC == null)
    //        {
    //            ANC_Orders.Add(new PrintModelANC_Order(anc));
    //            printDefModelANC = ANC_Orders.FirstOrDefault(x => x.Anc2.Id == anc.Id && x.Anc2.BomQty == anc.BomQty);
    //        }

    //        return printDefModelANC;
    //    }
    //    public void AddOrderToANC(Anc2 anc, ProductionOrder order)
    //    {
    //        AddAnc(anc).AddOrder(order);
    //    }

    //    public PrintModelORDER_Ancs AddOrder(ProductionOrder order)
    //    {
    //        PrintModelORDER_Ancs printDefModelOrder = ORDER_Ancs.FirstOrDefault(x => x.order.Id == order.Id);

    //        if (printDefModelOrder == null)
    //        {
    //            ORDER_Ancs.Add(new PrintModelORDER_Ancs(order));
    //            printDefModelOrder = ORDER_Ancs.FirstOrDefault(x => x.order.Id == order.Id);
    //        }

    //        return printDefModelOrder;
    //    }
    //    public void AddANCToOrder(ProductionOrder order, Anc2 anc)
    //    {
    //        AddOrder(order).AddANC(anc);
    //    }
    //}
    //public class PrintDataPreparator
    //{        
    //    public PrintDataPreparator(){ }

    //    public List<PrintModelANCs_Orders> PrepareDataView1(List<PrintModelORDER_Ancs> order_ancs)
    //    {
    //        List<PrintModelANCs_Orders> ancs_order_list = new List<PrintModelANCs_Orders>();
    //        PrintModelANCs_Orders ancs_orders;
    //        List<PrintModelORDER_Ancs> orderWithAnc;
    //        Anc2 anc;

    //        foreach (PrintModelORDER_Ancs pmo in order_ancs)
    //        {
    //            //wez kod
    //            anc = (Anc2)pmo.Ancs.FirstOrDefault(a => a.Used == false);

    //            while(anc != null)
    //            { 
    //                ancs_orders = new PrintModelANCs_Orders();

    //                //pozostałe ordery zawierające to anc
    //                orderWithAnc = order_ancs.Where(x => x.Ancs.Select(y=>y.Id).Contains(anc.Id)).ToList();

    //                DeleteOrdersWhereAncIsAlreadyUsed(orderWithAnc, anc);

    //                //sprawdz czy wystepuje w innych orderach
    //                AddOtherOrdersWithThisANC(anc, ancs_orders, orderWithAnc);
    //                //sprawdz inne wspolne kody
    //                AddOtherANCsConnectedWithTheOrders(ancs_orders, orderWithAnc);

    //                ancs_order_list.Add(ancs_orders);
    //                anc = (Anc2)pmo.Ancs.FirstOrDefault(a => a.Used == false);
    //            }
    //        }

    //        ancs_order_list = ancs_order_list.OrderByDescending(x => x.Orders.Count)
    //                        .ThenBy(x => x.Orders.Min(y=>y.StartDate))
    //                        .ThenBy(x => x.Ancs.Min(y => y.WorkstationOrder))
    //                        .ToList();
    //        return ancs_order_list;

    //    }
    //    private void AddOtherOrdersWithThisANC(Anc2 anc, PrintModelANCs_Orders ancs_orders, List<PrintModelORDER_Ancs> orderWithAncs)
    //    {

    //        Anc2 anc2;
    //        foreach (PrintModelORDER_Ancs pmo2 in orderWithAncs)
    //        {
    //            anc2 = (Anc2)pmo2.Ancs.FirstOrDefault(a => a.Id == anc.Id && a.Used == false);

    //            if (anc2 != null)
    //            {
    //                anc2.Used = true;
    //                ancs_orders.AddPair(pmo2.order, anc2);
    //            }
    //            else
    //            {
    //                //temp.Remove(pmo2); ---> DeleteOrdersWhereAncIsAlreadyUsed
    //            }
    //        }
    //    }
    //    private void AddOtherANCsConnectedWithTheOrders(PrintModelANCs_Orders ancs_orders, List<PrintModelORDER_Ancs> orderWithAnc)
    //    {
    //        if(orderWithAnc.Count > 1)
    //        {
    //            //bierze kolejne ANC wystepujace w orderach z listy
    //            Anc2 anc = (Anc2)orderWithAnc[0].Ancs.FirstOrDefault(a => a.Used == false);
    //            int counter = 0;
    //            int count = orderWithAnc[0].Ancs.Count * 10;

    //            while (anc != null && counter < count)
    //            {
    //                //sprawdza czy anc istnieje w pozostalych zleceniach
    //                bool lipa = false;
    //                for (int i = 0; i < orderWithAnc.Count; i++)
    //                {
    //                    Anc2 anc2 = orderWithAnc[i].Ancs.FirstOrDefault(x => x.Id == anc.Id);
    //                    if (anc2 == null || anc2.Used == true)
    //                    {
    //                        lipa = true;
    //                        break;
    //                    }
    //                }

    //                //dodaje anc jezeli ustnieje we wszystkich pozostalych zleceniach.
    //                if (!lipa)
    //                {
    //                    ancs_orders.AddPair(orderWithAnc[0].order, anc);

    //                    for (int i = 0; i < orderWithAnc.Count; i++)
    //                    {
    //                        Anc2 anc3 = orderWithAnc[i].Ancs.FirstOrDefault(x => x.Id == anc.Id);
    //                        if (anc3 != null)
    //                        {
    //                            anc3.Used = true;
    //                        }
    //                    }
    //                }

    //                counter++;
    //                anc = orderWithAnc[0].Ancs.FirstOrDefault(a => a.Used == false);
    //            }
    //        }

    //    }
    //    private void DeleteOrdersWhereAncIsAlreadyUsed(List<PrintModelORDER_Ancs> orderWithAnc, Anc2 anc)
    //    {
    //        List<PrintModelORDER_Ancs> toBeRemoved = new List<PrintModelORDER_Ancs>();
    //        foreach (PrintModelORDER_Ancs pmo2 in orderWithAnc)
    //        {
    //            if (pmo2.Ancs.FirstOrDefault(a => a.Id == anc.Id && a.Used == false) == null)
    //            {
    //                toBeRemoved.Add(pmo2);
    //            }
    //        }

    //        for (int i = 0; i < toBeRemoved.Count; i++)
    //        {
    //            orderWithAnc.RemoveAll(x => x.order.Id == toBeRemoved[i].order.Id);
    //        }
    //    }

    //    public List<PrintModelANCs_Orders> PrepareDataView2(List<PrintModelORDER_Ancs> order_ancs)
    //    {
    //        List<PrintModelANCs_Orders> AncsOrders = new List<PrintModelANCs_Orders>();
    //        PrintModelANCs_Orders pmAncOrder;

    //        foreach (PrintModelORDER_Ancs pmo in order_ancs)
    //        {
    //            pmAncOrder = new PrintModelANCs_Orders();

    //            foreach (Anc2 anc in pmo.Ancs)
    //            {
    //                pmAncOrder.AddPair(pmo.order, anc);
    //            }
    //            AncsOrders.Add(pmAncOrder);
    //        }

    //        AncsOrders = AncsOrders.OrderBy(x => x.Orders.Min(y => y.StartDate))
    //                                .ThenBy(x => x.Ancs.Min(y => y.WorkstationOrder))
    //                                .ToList();

    //        return AncsOrders;
    //    }
    //}
    //public class PrintModelANCs_Orders
    //{
    //    public List<Anc2> Ancs { get; set; }
    //    public List<ProductionOrder> Orders { get; set; }
    //    //public List<AncPackage> AncPackages { get; set; }
    //    //public List<AncWorkstation> AncWorkstations { get; set; }
    //    public int Sum { get; set; }

    //    public PrintModelANCs_Orders()
    //    {
    //        Ancs = new List<Anc2>();
    //        //AncPackages = new List<AncPackage>();
    //       // AncWorkstations = new List<AncWorkstation>();
    //        Orders = new List<ProductionOrder>();
    //        Sum = 0;
    //    }

    //    public void AddPair(ProductionOrder order, Anc2 anc)
    //    {
    //        ProductionOrder o = Orders.FirstOrDefault(x => x.Id == order.Id);
    //        if(o == null)
    //        {
    //            Orders.Add(order);
    //            Sum += order.QtyRemain;
    //        }

    //        Anc2 a = Ancs.FirstOrDefault(x => x.Id == anc.Id);
    //        if (a == null)
    //        {
    //            anc.TotalQty = anc.BomQty * order.QtyRemain;
    //            Ancs.Add(anc);
    //        }
    //        else
    //        {
    //            a.TotalQty += a.BomQty * order.QtyRemain;
    //        }

    //    }
    //}
    //public class PrintModelANC_Order
    //{
    //    public Anc2 Anc2 { get; set; }
    //    public int SumQty { get; private set; }
    //    public List<ProductionOrder> ProductionOrders { get; private set; }

    //    public PrintModelANC_Order(Anc2 anc)
    //    {
    //        Anc2 = new Anc2 { Id = anc.Id, Code = anc.Code, Name = anc.Name, Used = false, BomQty = anc.BomQty, TotalQty = 0 };
    //        SumQty = 0;
    //        ProductionOrders = new List<ProductionOrder>();
    //    }

    //    public void AddOrder(ProductionOrder order)
    //    {
    //        if (ProductionOrders.FirstOrDefault(x => x.Id == order.Id) == null)
    //        {
    //            ProductionOrders.Add(order);
    //            SumQty += order.QtyRemain;
    //            Anc2.TotalQty += Anc2.BomQty * order.QtyRemain;
    //        }
    //    }
    //    public void AddOrders(List<ProductionOrder> orders)
    //    {
    //        foreach (ProductionOrder order in orders)
    //        {
    //            AddOrder(order);
    //        }
    //    }
    //}
}