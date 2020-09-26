using MDL_OTHER.ComponentPickByLight._Interfaces;
using MDL_OTHER.ComponentPickByLight.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Repo;

namespace MDL_OTHER.ComponentPickByLight.Repos
{
    public class PickByLightInstanceElementRepo : RepoGenericAbstract<PickByLightInstanceElement>
    {
        protected new IDbContextPickByLight db;

        public PickByLightInstanceElementRepo(IDbContextPickByLight db) : base(db)
        {
            this.db = db;
        }

        public override PickByLightInstanceElement GetById(int id)
        {
            return db.PickByLightInstanceElements.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<PickByLightInstanceElement> GetListFiltered(PickByLightInstanceElement filter)
        {
            return db.PickByLightInstanceElements.Where(x =>
                (filter.PickByLightInstanceId == x.PickByLightInstanceId) &&
                (filter.ItemCode == null || filter.ItemCode == x.ItemCode) &&
                (filter.Name == null || filter.Name== x.Name) &&
                (filter.PLCMemoryAdress == null || filter.PLCMemoryAdress == x.PLCMemoryAdress)
             ).OrderBy(x=>x.Name).ThenBy(x=>x.ItemCode);
        }


        //public List<PickByLightInstanceElement> GetJOBsByItemCode(string itemCode)
        //{
        //    //var pncParam = new SqlParameter("@pnc", itemCode);

        //    //var result = db.Database
        //    //    .SqlQuery<data1>("[dbo].[OTHER_VisualControl_GetJOBsForCameras] @pnc", pncParam)
        //    //    .Select(x => new JobItemConfig()
        //    //    {
        //    //        Id = -1,
        //    //        JobNo = x.JobNo,
        //    //        PairNo = x.PairNo,
        //    //        CameraNo = x.CameraNo
        //    //    })
        //    //    .ToList();

        //    //return result;
        //    ////db.Database.ExecuteSqlCommand("EXEC [dbo].[OTHER_VisualControl_GetJOBsForCameras] @pnc = '" + itemCode + "'");
        //}
    }
}
