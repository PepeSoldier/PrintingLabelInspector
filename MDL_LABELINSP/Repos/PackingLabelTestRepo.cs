using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo;

namespace MDL_LABELINSP.Models.Repos
{
    public class WorkorderLabelInspectionRepo : RepoGenericAbstract<WorkorderLabelInspection>
    {
        protected new IDbContextLabelInsp db;

        public WorkorderLabelInspectionRepo(IDbContextLabelInsp db) : base(db)
        {
            this.db = db;
        }

        public override WorkorderLabelInspection GetById(int id)
        {
            return db.WorkorderLabelInspections.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<WorkorderLabelInspection> GetList()
        {
            return db.WorkorderLabelInspections.OrderBy(x => x.Id);
        }

        public List<WorkorderLabelInspection> GetByPackingLabelId(int packingLabelId)
        {
            return db.WorkorderLabelInspections.Where(x => x.WorkorderLabelId == packingLabelId).ToList();
        }

        public bool SaveInspectionResults(WorkorderLabel workorderLabel, ItemData itemData, EnumLabelType labelType)
        {
            try
            {
                if (workorderLabel.Id > 0 && itemData.IsDataEmpty == false)
                {
                    List<WorkorderLabelInspection> workorderLabelInspections = new List<WorkorderLabelInspection>();

                    workorderLabelInspections.Add(new WorkorderLabelInspection
                    {
                        TestName = "Nazwa Modelu",
                        ExpectedValueText = itemData.ExpectedName,
                        ActualValueText = itemData.ActualName,
                        Result = itemData.ExpectedName == itemData.ActualName,
                        LabelType = labelType,
                        TimeStamp = DateTime.Now,
                        WorkorderLabelId = workorderLabel.Id
                    });
                    
                    workorderLabelInspections.Add(new WorkorderLabelInspection
                    {
                        TestName = "Kod Produktu",
                        ExpectedValueText = itemData.ExpectedProductCode,
                        ActualValueText = itemData.ActualProductCode,
                        Result = itemData.ExpectedProductCode == itemData.ActualProductCode,
                        LabelType = labelType,
                        TimeStamp = DateTime.Now,
                        WorkorderLabelId = workorderLabel.Id
                    });
                    
                    workorderLabelInspections.Add(new WorkorderLabelInspection
                    {
                        TestName = "Kod Kreskowy",
                        ExpectedValueText = itemData.ExpectedBarcodeSmall,
                        ActualValueText = itemData.ActualBarcode,
                        Result = itemData.ExpectedBarcodeSmall == itemData.ActualBarcode,
                        LabelType = labelType,
                        TimeStamp = DateTime.Now,
                        WorkorderLabelId = workorderLabel.Id
                    });

                    workorderLabelInspections.Add(new WorkorderLabelInspection
                    {
                        TestName = "Waga",
                        ExpectedValueText = itemData.ExpectedWeightKG,
                        ActualValueText = itemData.ActualWeightKG,
                        Result = itemData.ExpectedWeightKG == itemData.ActualWeightKG,
                        LabelType = labelType,
                        TimeStamp = DateTime.Now,
                        WorkorderLabelId = workorderLabel.Id
                    });

                    workorderLabelInspections.RemoveAll(x => x.ActualValueText.Length < 1);
                    AddOrUpdateRange(workorderLabelInspections);
                    
                    int positiveCount = workorderLabelInspections.Count(x => x.Result == true);
                    return positiveCount == workorderLabelInspections.Count;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("PrepareInspectionResultToSave Exception. " + ex.Message);
            }

            return true;
        }

    }
}