using MDL_BASE.Interfaces;
using MDL_AP.Models.DEF;
using MDL_BASE.Models.IDENTITY;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using MDL_BASE.ComponentBase.Entities;

namespace MDL_AP.Models.ActionPlan
{
    [Table("Action", Schema = "AP")]
    public class ActionModel : IModelEntity
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("ParentActionIs")]
        public int ParentActionId { get; set; }

        [DisplayName("Ilość podakcji")]
        public int SubactionsCount { get; set; }

        [DisplayName("Postęp %")]
        public int Progress { get; set; }

        [DisplayName("Data Utworzenia")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Data Rozpoczęcia")]
        public DateTime StartDate { get; set; }

        [DisplayName("Data wykonania")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Planowana Data Wykonania")]
        public DateTime PlannedEndDate { get; set; }

        [Display(Name = "Krótki Tytuł")] [MaxLength(60)]
        [Required, StringLength(60, MinimumLength = 3, ErrorMessage = "Maksymalna liczba znaków - 60")]
        public string Title { get; set; }

        [Display(Name = "Problem")]
        public string Problem { get; set; }

        public virtual Department Department { get; set; }
        [Display(Name = "Dział")]
        public int? DepartmentId { get; set; }

        public virtual User Creator { get; set; }
        [DisplayName("Tworzący")]
        public string CreatorId { get; set; }

        public virtual User Assigned { get; set; }
        [DisplayName("Odpowiedzialny")]
        public string AssignedId { get; set; }

        [Display(Name = "Obszar")]
        public virtual Area Area { get; set; }
        [Required(ErrorMessage = "Wybór obszaru jest obowiązkowy")]
        public int AreaId { get; set; }

        [Display(Name = "Linia")]
        public virtual Resource2 Line { get; set; }
        [Required(ErrorMessage = "Wybór liniii jest obowiązkowy")]
        public int LineId { get; set; }

        [Display(Name = "Zmiana")]
        public virtual LabourBrigade ShiftCode { get; set; }
        [Required(ErrorMessage = "Wybór zmiany jest obowiązkowy")]
        public int ShiftCodeId { get; set; }

        [Display(Name = "Stanowisko")]
        public virtual Workstation Workstation { get; set; }
        [Required(ErrorMessage = "Wybór stanowiska jest obowiązkowy")]
        public int WorkstationId { get; set; }

        [Display(Name = "Kategoria")]
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        [DisplayName("Status")]
        public ActionStateEnum State { get; set; }

        [DisplayName("Typ")]
        public virtual DEF.Type Type { get; set; }
        public int TypeId { get; set; }

        //[DisplayName("Załącznik")]
        //public virtual ExtensionFile ExtensionFile { get; set; }

        [NotMapped]
        public string Variable1 { get; set; }
    }
}