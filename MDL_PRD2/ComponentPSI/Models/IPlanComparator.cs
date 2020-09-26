using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_PRD.Model
{
    public interface IPlanComparator
    {
        void CompareOrdersSequence(List<Model.OrderArchiveModel> la, List<Model.OrderArchiveModel> l);
        void CheckAdditionalOrders(List<Model.OrderArchiveModel> la, List<Model.OrderArchiveModel> l);
        void CheckRemovedOrders(List<Model.OrderArchiveModel> la, List<Model.OrderArchiveModel> l);
    }
}
