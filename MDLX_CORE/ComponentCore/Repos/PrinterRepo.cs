using MDL_BASE.Interfaces;
using System.Linq;
using XLIB_COMMON.Repo;
using XLIB_COMMON.Enums;
using MDL_CORE.ComponentCore.Entities;

namespace MDLX_CORE.ComponentCore.Repos
{
    public class PrinterRepo : RepoGenericAbstract<Printer>
    {
        protected new IDbContextCore db;

        public PrinterRepo(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        public override Printer GetById(int id)
        {
            return db.Printers.FirstOrDefault(d => d.Id == id);
        }

        public Printer GetByName(string printerName)
        {
            return db.Printers.FirstOrDefault(d => d.Name == printerName);
        }

        public override IQueryable<Printer> GetList()
        {
            return db.Printers.OrderByDescending(x => x.Id);
        }

        public IQueryable<Printer> GetList(Printer filter)
        {
            string Name = filter.Name != null ? filter.Name : null;
            string IpAdress = filter.IpAdress != null ? filter.IpAdress : null;
            string Model = filter.Model != null ? filter.Model : null;
            string SerialNumber = filter.SerialNumber != null ? filter.SerialNumber : null;
           PrinterType PrinterType = filter.PrinterType != PrinterType.None ? filter.PrinterType : PrinterType.None;

            var query = db.Printers
                    .Where(x =>
                        (Name == null || x.Name.Contains(Name)) &&
                        (IpAdress == null || x.IpAdress.Contains(IpAdress)) &&
                        (Model == null || x.Model.Contains(Model)) &&
                        (SerialNumber == null || x.SerialNumber.Contains(SerialNumber)) &&
                        (PrinterType == PrinterType.None || x.PrinterType == PrinterType));
            return query;
        }
    }
}