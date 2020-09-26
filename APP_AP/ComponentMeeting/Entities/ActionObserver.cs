using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_AP.Models.ActionPlan
{
    [Table("ActionObserver", Schema = "AP")]
    public class ActionObserver : IModelEntity
    {
        public int Id { get; set; }

        public int ActionId { get; set; }

        public string UserId { get; set; }

        public int ObserverId { get; set; }

        public int ObserverType { get; set; }
       
        public bool Hidden { get; set; }
        
    }
    

    public enum ObserverType
    {
        meeting = 1,
    }
}