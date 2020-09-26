using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentWHDOC.Enums;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_iLOGIS.ComponentWHDOC.Entities
{
    [Table("WHDOC_WhDocument", Schema = "iLOGIS")]
    public class WhDocumentCMR : WhDocumentAbstract, IModelEntity
    {
        public WhDocumentCMR()
        {
            DocumentDate = new DateTime(1900,1,1);
            StampTime = DateTime.Now;
        }

        public string ForwadrerComments { get; set; }

       
    }
}