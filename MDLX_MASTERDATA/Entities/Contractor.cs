using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_BASE.Models.MasterData
{
    [Table("MASTERDATA_Contractor")]
    public class Contractor : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string Country { get; set; }
        public string Language { get; set; }

        public string NIP { get; set; }

        public string ContactPersonName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAdress { get; set; }

        public bool Deleted { get; set; }
        //inne pola...
    }
}