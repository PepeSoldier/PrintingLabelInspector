using MDL_BASE.ViewModel;
using MDL_PRD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MDL_PRD.ViewModel
{
    public class PsiViewModel
    {
        public List<OrderArchiveModel> Orders { get; set; }
        public List<OrderArchiveModel> OrdersArch { get; set; }
        public ChartJSViewModel ChartJsData { get; set; }
        public string DSA {get; set; }
        public string SeqCnt {get; set; }
        public string SeqSum {get; set; }
        public string PSI {get; set; }
    }

    public class PsiFormViewModel
    {
        public DateTime SelectedDate { get; set; }
        public int SelectedLine { get; set; }
        public int SelectedShift { get; set; }

        public IEnumerable<SelectListItem> Lines { get; set; }
        public IEnumerable<SelectListItem> Shifts { get; set; }
    }

    public class PsiResultFormViewModel
    {
        public DateTime SelectedDateFrom { get; set; }
        public DateTime SelectedDateTo { get; set; }
        public int SelectedLine { get; set; }
        public int SelectedShift { get; set; }

        public IEnumerable<SelectListItem> Lines { get; set; }
        public IEnumerable<SelectListItem> Shifts { get; set; }
    }

}