using MDL_iLOGIS.ComponentWMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class DeliveryInspectionSummaryViewModel
    {
        public DeliveryInspectionSummaryViewModel()
        {
            DeliveryItems = new List<DeliveryItemViewModel>();
        }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int DeliveryId { get; set; }
        public string DeliveryDocument { get; set; }
        public string Guid { get; set; }

        public string SelectedItemCode { get; set; }
        public string SelectedItemName { get; set; }
        public decimal SelectedItemDocumentQty { get; set; }

        public List<Delivery> DeliveriesByGroup { get; set; }
        public List<DeliveryItemViewModel> DeliveryItems { get; set; }
    }
}
