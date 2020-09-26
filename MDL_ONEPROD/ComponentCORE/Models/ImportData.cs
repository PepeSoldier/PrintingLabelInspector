using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;

namespace MDL_ONEPROD.ComponentBase.Models
{
    public abstract class ImportData
    {
        public List<ClientOrder> clientOrderFile;
        public List<ClientOrder> clientOrderDataBase;

        public abstract void ImportOrders();
        
    }

    public class ImportDataEldisy : ImportData
    {
        

        public override void ImportOrders()
        {
            Console.WriteLine("Im in the house");
        }

        public void CompareOrders()
        {
            Console.WriteLine("");
        }

        public void InsertNewOrders()
        {
            Console.WriteLine("Wstawia nowe zamowienia");
        }

        public void UpdateExistingOrders()
        {
            Console.WriteLine("Aktualizacja zamowien");
        }
    }

    public class ImportDataGrandhome : ImportData
    {
        public override void ImportOrders()
        {

            //https://blogs.msdn.microsoft.com/freddyk/2010/01/19/connecting-to-nav-web-services-from-c-using-web-reference/

            //string baseURL = "http://localhost:7047/DynamicsNAV/WS/";

            //SystemService systemService = new SystemService();
            //systemService.Url = baseURL + "SystemService";
            //systemService.UseDefaultCredentials = true;

            //Console.WriteLine("Companies:");
            //string[] companies = systemService.Companies();
            //foreach (string company in companies)
            //    Console.WriteLine(company);
            //string cur = companies[0];

            //string customerPageURL = baseURL + Uri.EscapeDataString(cur) + "/Page/Customer";
            //Console.WriteLine("\nURL of Customer Page: " + customerPageURL);

            //Customer_Service customerService = new Customer_Service();
            //customerService.Url = customerPageURL;
            //customerService.UseDefaultCredentials = true;

            //Customer cust10000 = customerService.Read("10000");
            //Console.WriteLine("\nName of Customer 10000: " + cust10000.Name);

            //Customer_Filter filter1 = new Customer_Filter();
            //filter1.Field = Customer_Fields.Country_Region_Code;
            //filter1.Criteria = "GB";

            //Customer_Filter filter2 = new Customer_Filter();
            //filter2.Field = Customer_Fields.Location_Code;
            //filter2.Criteria = "RED|BLUE";

            //Console.WriteLine("\nCustomers in GB served by RED or BLUE warehouse:");
            //Customer_Filter[] filters = new Customer_Filter[] { filter1, filter2 };
            //Customer[] customers = customerService.ReadMultiple(filters, null, 0);
            //foreach (Customer customer in customers)
            //    Console.WriteLine(customer.Name);

            //Console.WriteLine("\nTHE END");
            //Console.ReadLine();
        }
    }

}