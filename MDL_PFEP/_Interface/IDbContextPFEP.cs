using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Model.PFEP;
using MDL_PFEP.Models.DEF;
using MDL_PFEP.Models.PFEP;
using MDL_PRD.Model;
using System.Data.Entity;

namespace MDL_PFEP.Interface
{
    public interface IDbContextPFEP : IDbContextiLOGIS
    {
        //DEF entities
        //DbSet<Package> Packages { get; set; }
        //DbSet<MontageType> MontageTypes { get; set; }
        //DbSet<FeederType> FeederTypes { get; set; }
        //DbSet<BufferType> BufferTypes { get; set; }
        ////DbSet<AncType> AncTypes { get; set; }
        ////DbSet<PncType> PncTypes { get; set; }

        //PFEP entities
        //DbSet<Models.PFEP.PackageItem> PackageItems { get; set; }
        //DbSet<Models.PFEP.WorkstationItem> WorkstationItems { get; set; }
        ////DbSet<Results1> Results1 { get; set; }
        ////DbSet<Results2> Results2 { get; set; }
        DbSet<PrintHistory> PrintHistory { get; set; }
        DbSet<AncFixedLocation> AncFixedLocations { get; set; }

        ////DbSet<ProductionOrder> ProductionOrders { get; set; }
        DbSet<Prodorder20> ProdOrder20 { get; set; }
    }

    //TODO: 1. Wiele linii 101,104
    //TODO: 2. Zaznaczanie na strcie pierwszego zakresu zleceń
    //TODO: 3. Kolumna z faktyczną ilością zlecenia
    //TODO: 4. Ilosć faktyczna na wydruku
    //TODO: 5. Nazwy zmienić na angielskie (komponenty)
    //TODO: 6. Wyświetlać podmontaż priorytetowo zamiast linii
    //TODO: 7. Dla DEF >= 6h wyświetlać skumulowane ilości, ale chwytać tylko 20 z zakresu
    //TODO: 7. ??? Wybieranie ręcznie orderów do druku
    //TODO: 8. Moduł do zarządzania lokalizacjami

}