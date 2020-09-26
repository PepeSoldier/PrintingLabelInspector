using MDL_BASE.Enums;
using MDL_BASE.Interfaces;

using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_PFEP.Model.ELDISY_PFEP
{
    [Table("PackingInstruction", Schema = "PFEP")]
    public class PackingInstruction : IModelEntity
    {
        public int Id { get; set; }

        [Display(Name="Numer Instrukcji", GroupName = "Loggable")]
        public int InstructionNumber { get; set; }

        [Display(Name = "Wersja Instrukcji", GroupName = "Loggable")]
        public InstructionVersion InstructionVersion { get; set; }

        [Display(Name ="Opis", GroupName = "Loggable")]
        public string Description { get; set; }

        [Display(Name = "Ilość na warstwie", GroupName = "Loggable")]
        public int AmountOnLayer { get; set; }

        [Display(Name = "Ilość w pojemniku", GroupName = "Loggable")]
        public int AmountOnBox { get; set; }

        [Display(Name = "Ilość na palecie", GroupName = "Loggable")]
        public int AmountOnPallet { get; set; }

        [Display(Name ="Jednostka Miary", GroupName = "Loggable")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}"), Display(Name = "Data ostatniej modyfikacji")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Sprawdził QS")]
        public bool Examined { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}"), Display(Name = "Data zatwierdzenia")]
        public DateTime ExaminedDate { get; set; }

        [Display(Name = "Zatwierdził")]
        public bool Confirmed { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}"), Display(Name="Data zatwierdzenia")]
        public DateTime ConfirmedDate { get; set; }

        public int? NumberOfCorrections { get; set; }

        [Display(Name = "Sporządził")]
        public virtual User Creator { get; set; }
        public string CreatorId { get; set; }

        [Display(Name = "Sprawdził", GroupName = "Loggable")]
        public virtual User Examiner { get; set; }
        public string ExaminerId { get; set; }

        [Display(Name = "Zatwierdził", GroupName = "Loggable")]
        public virtual User Confirm { get; set; }
        public string ConfirmId { get; set; }

        [Display(Name = "Obszar", GroupName = "Loggable")]
        public virtual Area Area { get; set; }
        [Display(Name = "Obszar", GroupName = "Loggable")]
        public int AreaId { get; set; }

        [MaxLength(200), Display(Name = "Komentarz")]
        public string ExamineComment { get; set; }
        [MaxLength(200), Display(Name = "Komentarz")]
        public string ConfirmComment { get; set; }

        [Required, StringLength(100), Display(Name = "Kod Profilu", GroupName = "Loggable")]
        public string ProfileCode { get; set; } //1212312,12312312

        [Required, StringLength(100), Display(Name="Nazwa Profilu", GroupName = "Loggable")]
        public string ProfileName { get; set; }

        [Display(Name = "Klient",GroupName ="Loggable")]
        public string ClientName { get; set; }

        [Display(Name = "Numer wyrobu klienta", GroupName = "Loggable")]
        public string ClientProfileCode { get; set; }

        public decimal CurrentPrice { get; set; }
        public decimal CalculationPrice { get; set; }

        public bool Deleted { get; set; }

        //NOT MAPPED FIELDS
        [NotMapped]
        public int pageIndex { get; set; }
        [NotMapped]
        public int pageSize { get; set; }
        [NotMapped]
        public string sortField { get; set; }
        [NotMapped]
        public string sortOrder { get; set; }
        [NotMapped]
        public string PackageCode { get; set; }

        //TODO: Do wyrzucenia po implementacji apki
        [MaxLength(64)]
        public string TempPhoto1 { get; set; }
        [MaxLength(64)]
        public string TempPhoto2 { get; set; }
        [MaxLength(64)]
        public string TempPhoto3 { get; set; }
        [MaxLength(64)]
        public string TempPhoto4 { get; set; }
        public string TmpConfirmName { get; set; }
        public string TmpExaminerName { get; set; }
        public string TmpCreatorName { get; set; }
        
    }
}