using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_PRD.Model
{
    public interface IDataVisualizer
    {
        //void MarkAsRemoved(int row);
        //void MarkAsAdded(int row);
        //void MarkBadSequence(int row);
        //void MarkProperSequence(int row);
        void MarkArchPlanStatus(int row, EnumStatus status);
        void MarkPlanStatus(int row, EnumStatus status);
    }
}
