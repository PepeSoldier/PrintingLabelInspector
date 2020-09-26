using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Models
{
    public class PLWOviewModel
    {
        public PLWOviewModel() {
            list = new List<PickingListWorkOrdersStatus>();
            transporter = new Transporter();
        }

        public List<PickingListWorkOrdersStatus> list;
        public Transporter transporter;
    }
}
