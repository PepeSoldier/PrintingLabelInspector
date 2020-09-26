using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Models.DEF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Interface
{
    public interface IDbContextPFEP_Eldisy : IDbContextCore
    {
        //Def entieties
        DbSet<Package> Packages { get; set; }
        DbSet<PackingInstructionPackage> PackingInstructionPackages { get; set; }
        DbSet<PackingInstruction> PackingInstructions { get; set; }
        DbSet<PackingInstructionPhoto> PackingInstructionPhotos { get; set; }
        DbSet<Correction> Corrections { get; set; }
        DbSet<Calculation> Calculations { get; set; }
        //DbSet<PackageItem> Items { get; set; } //wyleciał sprawdz komentarze w środku
    }
}