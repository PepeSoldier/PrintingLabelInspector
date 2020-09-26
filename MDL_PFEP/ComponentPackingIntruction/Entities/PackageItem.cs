using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Model.ELDISY_PFEP
{
    //[Table("Package_Item", Schema = "PFEP")]
    //public class PackageItem : IModelEntity
    //{
    //    //w sumie model nie jest potrzebny to taka tabelka robi wiązanie wiele do wiele
    //    //a my potrzebujemy wiązanie 1 intrukcja pakowania do wielu Itemów
    //    //w związku z tym dodałem relacje w PackingInstructionItem na PackingInstruction i jest git.

    //    //model zostawiłem dla ciebie do podglądu ale z dbContext wyleciał
    //    public int Id { get; set; }
        
    //    public virtual PackingInstruction PackingInstruction { get; set; }
    //    public int PackingInstructionId { get; set; }

    //    public virtual PackingInstructionPackage PackageItemEldisy { get; set; }
    //    public int PackageItemEldisyId { get; set; }
    //}
}