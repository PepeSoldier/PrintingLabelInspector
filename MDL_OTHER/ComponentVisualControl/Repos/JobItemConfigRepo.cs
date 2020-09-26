using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_OTHER.ComponentHSE.Entities;
using MDL_OTHER.ComponentVisualControl.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Repo;

namespace MDL_OTHER.ComponentHSE.Repos
{
    public class JobItemConfigRepo : RepoGenericAbstract<JobItemConfig>
    {
        protected new IDbContextVisualControl db;

        public JobItemConfigRepo(IDbContextVisualControl db) : base(db)
        {
            this.db = db;
        }

        public override JobItemConfig GetById(int id)
        {
            return db.JobItemConfigs.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<JobItemConfig> GetListFiltered(JobItemConfig filter)
        {
            return db.JobItemConfigs.Where(x =>
                (filter.ItemCode == null || filter.ItemCode == x.Item.Code) &&
                (filter.CameraNo <= 0 || filter.CameraNo == x.CameraNo) &&
                (filter.JobNo <= 0 || filter.JobNo == x.JobNo) &&
                (filter.PairNo <= 0 || filter.PairNo == x.PairNo) &&
                (filter.Type == JobItemTypeEnum.Unknown || filter.Type == x.Type) &&
                (filter.Location == JobItemLocationEnum.Unknown || filter.Location == x.Location)
            ).OrderBy(x=>x.CameraNo).ThenBy(x=>x.JobNo).ThenBy(x=>x.PairNo).ThenBy(x=>x.Item.Code);
        }

        private class data1
        {
            public int JobNo { get; set; }
            public int PairNo { get; set; }
            public int CameraNo { get; set; }
        }

        public List<JobItemConfig> GetJOBsByItemCode(string itemCode)
        {
            var pncParam = new SqlParameter("@pnc", itemCode);

            var result = db.Database
                .SqlQuery<data1>("[dbo].[OTHER_VisualControl_GetJOBsForCameras] @pnc", pncParam)
                .Select(x => new JobItemConfig() {
                    Id=-1,
                    JobNo = x.JobNo,
                    PairNo = x.PairNo,
                    CameraNo = x.CameraNo
                })
                .ToList();

            return result;
            //db.Database.ExecuteSqlCommand("EXEC [dbo].[OTHER_VisualControl_GetJOBsForCameras] @pnc = '" + itemCode + "'");
        }
    }
}
