using MDL_BASE.Models.Base;
using MDL_PFEP.Model.ELDISY_PFEP;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using XLIB_COMMON.Interface;

namespace MDL_PFEP.ViewModel
{
    public class PackingInstructionViewModel
    {
        public PackingInstructionViewModel()
        {
            Correction = new Correction();
            PackingInstructionPhotos = new List<PackingInstructionPhoto>();
            ConfirmedDate = new DateTime(1, 1, 1);
            ExaminedDate = new DateTime(1, 1, 1);
        }
        public PackingInstruction PackinInstructionObject { get; set; }
        public Correction Correction { get; set; }
        public List<PackingInstructionPackage> PackingInstructionPackageList { get; set; }
        public List<PackingInstructionPhoto> PackingInstructionPhotos { get; set; }
        public List<Correction> Corrections { get; set; }
        public List<ChangeLog> ChangeLogs { get; set; }

        public IEnumerable<SelectListItem> EngineersSelectList { get; set; }
        public IEnumerable<SelectListItem> ManagersSelectList { get; set; }
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }

        public DateTime ExaminedDate { get; set; }
        public DateTime ConfirmedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool StepTwo { get; set; }
        public string AreaName { get; set; }
        public int PrevIntrId { get; set; }
        public int NextIntrId { get; set; }
        public bool IsUserAllowedToExamine { get; set; }
        public bool IsUserAllowedToConfirm { get; set; }
        public int Authorize { get; set; }
    }
}