using System;
using System.ComponentModel.DataAnnotations;

namespace MDL_AP.Models.ActionPlan
{
    public enum ActionStateEnum
    {
        [Display(Name = "Zgłoszenie")]
        stateInit = 0,
        [Display(Name="Plan")]
        statePlan = 1,
        [Display(Name = "Do")]
        stateDo = 2,
        [Display(Name = "Check")]
        stateCheck = 3,
        [Display(Name = "Act")]
        stateAct = 4
    };

    public class ActionState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public bool Selected { get; set; }
    }
}