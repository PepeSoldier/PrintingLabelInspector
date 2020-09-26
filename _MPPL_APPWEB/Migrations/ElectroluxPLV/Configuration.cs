namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using _MPPL_WEB_START.Migrations;
    using MDL_AP.Repo;
    using MDL_BASE.Models.IDENTITY;
    using XLIB_COMMON.Repo.IDENTITY;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MDL_WMS.ComponentConfig.UnitOfWorks;
    using MDL_iLOGIS.ComponentConfig.Entities;
    using System.Web;
    using Microsoft.AspNet.Identity.Owin;

    internal sealed class Configuration : DbMigrationsConfiguration<_MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV_Prod>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ElectroluxPLV";

            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV -MigrationsDirectory:Migrations.ElectroluxPLV
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration 1K
            //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration
        }

        protected override void Seed(_MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV_Prod context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            CnfigurationHelper.SeedDataNewRoles(context);
            CnfigurationHelper.AddAccountAdminRoleAndAssignToAdmin(context);
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            //RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            //RoleRepo _roleManager = new RoleRepo(new RoleStore<IdentityRole>(context), context);
            //UserRepo _userManager = new UserRepo(new UserStore<User>(context), context);


            //_roleManager.AddRole(DefRoles.Admin);
            //_roleManager.AddRole(DefRoles.User);
            //_roleManager.AddRole(DefRoles.ProdLeader);
            //_roleManager.AddRole(DefRoles.ProdManager);
            //_roleManager.AddRole(DefRoles.Engineer);
            //_roleManager.AddRole(DefRoles.Planner);
            //_roleManager.AddRole(DefRoles.WarehouseAdmin);
            //_roleManager.AddRole(DefRoles.TechnologyUser);
            //_roleManager.AddRole(DefRoles.TechnologyOperator);
            //_roleManager.AddRole(DefRoles.TechnologyAdmin);
            //_roleManager.AddRole(DefRoles.TechnologyLider);

            //_roleManager.AddRole(DefRoles.iLogisUser);
            //_roleManager.AddRole(DefRoles.iLogisOperator);
            //_roleManager.AddRole(DefRoles.iLogisAdmin);
            //_roleManager.AddRole(DefRoles.iLogisPFEP_PRD_Editor);
            //_roleManager.AddRole(DefRoles.iLogisPFEP_WH_Editor);

            //_userManager.AddUser("Admin", DefRoles.Admin);
            //_userManager.AddUser("Piotr", DefRoles.Admin);
            //_userManager.AddUser("Kamil", DefRoles.Admin);
            //_userManager.AddOperator("Operator");
            //_userManager.AddUser("Janusz", DefRoles.Admin);
            //_userManager.AddUser("Tomasz", DefRoles.Admin);

            //Seed_TechLocations(context);
            //Seed_TechPackageItem(context);
        }

        public void Seed_TechPackageItem(DbContextAPP_ElectroluxPLV_Prod context)
        {
            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(context);
            // Package p = new Package() { Code = "0000", Name = "luzem", PackagesPerPallet = 1, Type = EnumPackageType.Undefined };
            //uow.PackageRepo.Add(p);
            Package p = uow.PackageRepo.GetById(501);

            Warehouse wh = uow.WarehouseRepo.GetList().Where(x => x.Name == "WH.TECHNOLOGIA").FirstOrDefault();

            CreatePackageItem(uow, 1651, 360, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1659, 360, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1653, 240, 4, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1661, 200, 4, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1654, 42, 8, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1658, 60, 9, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1656, 38, 7, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1664, 240, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1668, 240, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1665, 180, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1669, 180, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1666, 120, 4, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1670, 100, 4, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1657, 28, 10, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1662, 57, 6, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1652, 180, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1660, 180, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1671, 180, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1663, 42, 8, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1672, 20, 10, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1673, 114, 6, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1655, 30, 5, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1674, 60, 5, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1662, 30, 5, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1673, 60, 5, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1667, 63, 9, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1675, 320, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1676, 320, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1679, 80, 4, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1678, 80, 4, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1677, 120, 4, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1680, 60, 9, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1684, 38, 7, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1681, 42, 8, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1688, 57, 6, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1685, 114, 6, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1687, 30, 5, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1686, 60, 5, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1682, 360, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1683, 360, 3, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1689, 200, 4, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1690, 38, 7, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1691, 38, 7, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1693, 20, 10, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1692, 28, 10, 1, wh.Id, p.Id);
            CreatePackageItem(uow, 1694, 180, 3, 1, wh.Id, p.Id);
        }
        public void Seed_TechLocations(DbContextAPP_ElectroluxPLV_Prod context)
        {
            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(context);

            Warehouse wh1 = new Warehouse() { Name = "MAG TECHNOLOGIA", WarehouseType = MDL_iLOGIS.ComponentWMS.Enums.WarehouseTypeEnum.AccountingWarehouse };
            uow.WarehouseRepo.AddOrUpdate(wh1);

            //Warehouse wh2 = new Warehouse() { Name = "BUFOR PRASY", ParentWarehouseId = wh1.Id, WarehouseType = MDL_iLOGIS.ComponentWMS.Enums.WarehouseTypeEnum.SubWarehouse };
            //Warehouse wh3 = new Warehouse() { Name = "BUFOR PIECE", ParentWarehouseId = wh1.Id, WarehouseType = MDL_iLOGIS.ComponentWMS.Enums.WarehouseTypeEnum.SubWarehouse };
            //Warehouse wh4 = new Warehouse() { Name = "BUFOR TUBY", ParentWarehouseId = wh1.Id, WarehouseType = MDL_iLOGIS.ComponentWMS.Enums.WarehouseTypeEnum.SubWarehouse };
            //uow.WarehouseRepo.AddOrUpdate(wh2);
            //uow.WarehouseRepo.AddOrUpdate(wh3);
            //uow.WarehouseRepo.AddOrUpdate(wh4);

            Warehouse wh2 = context.Warehouses.FirstOrDefault(x => x.Name == "WH-TECH-PRASY");
            Warehouse wh3 = context.Warehouses.FirstOrDefault(x => x.Name == "WH.TECH-PIECE");
            Warehouse wh4 = context.Warehouses.FirstOrDefault(x => x.Name == "WH.TECH-TUBY");


            int i;

            //KTB000-KTB244 -top/bottom
            WarehouseLocationType wlt_TOPBOTTOM = new WarehouseLocationType()
            {
                Name = "Kontener-TOB/BOTTOM",
                TypeEnum = WarehouseLocationTypeEnum.Trolley,
            };
            uow.WarehouseLocationTypeRepo.Add(wlt_TOPBOTTOM);

            for (i = 0; i <= 244; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "KTB" + i.ToString("D3"),
                    RegalNumber = "KTB",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_TOPBOTTOM.Id,
                    WarehouseId = wh2.Id,
                    UpdateDate = DateTime.Now
                });
            }

            //KW000-KW142
            WarehouseLocationType wlt_WRAP = new WarehouseLocationType()
            {
                Name = "Kontener-WRAP",
                TypeEnum = WarehouseLocationTypeEnum.Trolley,
            };
            uow.WarehouseLocationTypeRepo.Add(wlt_WRAP);

            for (i = 0; i <= 142; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "KW" + i.ToString("D3"),
                    RegalNumber = "KW",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_WRAP.Id,
                    WarehouseId = wh2.Id,
                    UpdateDate = DateTime.Now
                });
            }

            //KW500-KW530
            for (i = 500; i <= 530; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "KW" + i.ToString("D3"),
                    RegalNumber = "KW",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_WRAP.Id,
                    WarehouseId = wh2.Id,
                    UpdateDate = DateTime.Now
                });
            }
            //KOD000-KOD276
            WarehouseLocationType wlt_OD = new WarehouseLocationType()
            {
                Name = "Kontener-OD_60",
                TypeEnum = WarehouseLocationTypeEnum.Trolley,
            };
            uow.WarehouseLocationTypeRepo.Add(wlt_OD);

            for (i = 0; i <= 276; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "KOD" + i.ToString("D3"),
                    RegalNumber = "KOD",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_OD.Id,
                    WarehouseId = wh2.Id,
                    UpdateDate = DateTime.Now
                });
            }

            //KOD900- KOD966
            WarehouseLocationType wlt_OD45 = new WarehouseLocationType()
            {
                Name = "Kontener-OD_45",
                TypeEnum = WarehouseLocationTypeEnum.Trolley,
            };
            uow.WarehouseLocationTypeRepo.Add(wlt_OD45);

            for (i = 900; i <= 966; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "KOD" + i.ToString("D3"),
                    RegalNumber = "KOD",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_OD45.Id,
                    WarehouseId = wh2.Id,
                    UpdateDate = DateTime.Now
                });
            }

            //KSP000-KSP344
            WarehouseLocationType wlt_SP = new WarehouseLocationType()
            {
                Name = "Kontener-SP",
                TypeEnum = WarehouseLocationTypeEnum.Trolley,
            };
            uow.WarehouseLocationTypeRepo.Add(wlt_SP);

            for (i = 0; i <= 344; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "KSP" + i.ToString("D3"),
                    RegalNumber = "KSP",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_SP.Id,
                    WarehouseId = wh2.Id,
                    UpdateDate = DateTime.Now
                });
            }
            //KID000-KID231
            WarehouseLocationType wlt_ID = new WarehouseLocationType()
            {
                Name = "Kontener-ID",
                TypeEnum = WarehouseLocationTypeEnum.Trolley,
            };
            uow.WarehouseLocationTypeRepo.Add(wlt_ID);

            for (i = 0; i <= 231; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "KID" + i.ToString("D3"),
                    RegalNumber = "KID",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_ID.Id,
                    WarehouseId = wh2.Id,
                    UpdateDate = DateTime.Now
                });
            }
            //KID500-KID580
            WarehouseLocationType wlt_ID45 = new WarehouseLocationType()
            {
                Name = "Kontener-ID45",
                TypeEnum = WarehouseLocationTypeEnum.Trolley,
            };
            uow.WarehouseLocationTypeRepo.Add(wlt_ID45);

            for (i = 500; i <= 580; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "KID" + i.ToString("D3"),
                    RegalNumber = "KID",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_ID45.Id,
                    WarehouseId = wh2.Id,
                    UpdateDate = DateTime.Now
                });
            }

            //WT000-WT090
            WarehouseLocationType wlt_WT = new WarehouseLocationType()
            {
                Name = "Kontener-TUB",
                TypeEnum = WarehouseLocationTypeEnum.Trolley,
            };
            uow.WarehouseLocationTypeRepo.Add(wlt_WT);

            for (i = 0; i <= 90; i++)
            {
                uow.WarehouseLocationRepo.Add(new WarehouseLocation()
                {
                    ColumnNumber = i,
                    Name = "WT" + i.ToString("D3"),
                    RegalNumber = "WT",
                    QtyOfSubLocations = 0,
                    TypeId = wlt_WT.Id,
                    WarehouseId = wh4.Id,
                    UpdateDate = DateTime.Now
                });
            }
            //P100-P199
            //P200-P299
            //P300-P399

        }

        private void CreatePackageItem(UnitOfWork_iLogis uow, int itemWMSId, int qtyPerPackage, int locationTypeId, int packagerPerPallet, int whId, int packageId)
        {
            PackageItem pi = new PackageItem()
            {
                ItemWMSId = itemWMSId,
                PackageId = packageId,
                PackagesPerPallet = packagerPerPallet,
                PickingStrategy = MDL_iLOGIS.ComponentWMS.Enums.PickingStrategyEnum.UpToOrderQty,
                QtyPerPackage = qtyPerPackage,
                WarehouseId = whId,
                WarehouseLocationTypeId = locationTypeId,
            };

            uow.PackageItemRepo.Add(pi);
        }
    }
}
