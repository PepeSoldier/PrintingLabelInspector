﻿using MDL_BASE.Interfaces;
using MDL_ONEPROD.ComponentMes.Enums;
using MDL_ONEPROD.Model.Scheduling;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentQuality.ViewModels
{
    public class ItemParameterViewModel
    {
        public int Id { get; set; }

        public int ItemOPId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Depth { get; set; }

        public decimal Length_Tolerance { get; set; }
        public decimal Width_Tolerance { get; set; }
        public decimal Depth_Tolerance { get; set; }

        public decimal Weight { get; set; }
        public string Color { get; set; }

        public int ProgramNumber { get; set; }
        public string ProgramName { get; set; }

        public bool Deleted { get; set; }
    }
}