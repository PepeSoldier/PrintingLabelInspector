using MDLX_CORE.Interfaces;
using MDL_CORE.ComponentCore.Enums;
using MDL_CORE.ComponentCore.Models;
using MDLX_CORE.ComponentCore.Entities;
using System;
using System.IO;
using System.Linq;
using XLIB_COMMON.Model;

namespace XLIB_COMMON.Repo.Base
{
    public class SystemVariableRepo : RepoGenericAbstract<SystemVariable>
    {
        protected new IDbContextCore db;

        public SystemVariableRepo(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        public override SystemVariable GetById(int id)
        {
            return db.SystemVariables.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<SystemVariable> GetList()
        {
            return db.SystemVariables.OrderByDescending(x => x.Id);
        }

        public object GetValue(string variableName)
        {
            var temp = db.SystemVariables.FirstOrDefault(x => x.Name == variableName && x.UserId == null).Value;

            if (temp == null) 
                Logger2FileSingleton.Instance.SaveLog(string.Format("SystemVariable {0} not found", variableName));

            return temp;
        }
        public object GetValue(string variableName, string userId)
        {
            var temp = db.SystemVariables.FirstOrDefault(x => x.Name == variableName && x.UserId == userId).Value;

            if(temp == null) 
                Logger2FileSingleton.Instance.SaveLog(string.Format("SystemVariable {0} not found ({1})", variableName, userId ?? ""));

            return temp;
        }
        public int GetValueInt(string variableName, string userId = null)
        {
            SystemVariable sv = db.SystemVariables.FirstOrDefault(x => x.Name == variableName && x.UserId == userId);
            int value = 0;

            if (sv != null && int.TryParse(sv.Value, out value))
            {
                return value;
            }
            else
            {
                Logger2FileSingleton.Instance.SaveLog(string.Format("SystemVariable {0} not found ({1})", variableName, userId?? ""));
                return 0;
            }
        }
        public string GetValueString(string variableName, string userId = null)
        {
            var temp = db.SystemVariables.FirstOrDefault(x => x.Name == variableName && x.UserId == userId && x.Type == EnumVariableType.String);

            if(temp != null)
            {
                return temp.Value;
            }
            else
            {
                Logger2FileSingleton.Instance.SaveLog(string.Format("SystemVariable {0} not found ({1})", variableName, userId ?? ""));
                return string.Empty;
            }
        }
        public int UpdateValue(string variableName, string value, EnumVariableType type, string userId = null)
        {
            SystemVariable variable = null;

            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    variable = db.SystemVariables.FirstOrDefault(x => x.Name == variableName && x.Type == type && x.UserId == userId);

                    if (variable == null)
                    {
                        variable = new SystemVariable{ Name = variableName, Type = type, UserId = userId, Value = value };
                    }
                    
                    variable.Value = value;
                    AddOrUpdate(variable);
                    transaction.Commit();
                }
                catch
                {
                    variable = new SystemVariable { Id = 0 };
                    transaction.Rollback();
                }
            }

            return variable.Id;
        }

        public DateTime GetLastEnergyImportDate()
        {
            SystemVariable svDateStamp = db.SystemVariables.Where(x => x.Name == "EnergyImportDate").FirstOrDefault();
            if (svDateStamp == null)
            {
                svDateStamp = new SystemVariable();
                svDateStamp.Name = "EnergyImportDate";
                svDateStamp.Value = DateTime.Now.ToString("yyyy-MM-dd");
                svDateStamp.Type = EnumVariableType.DateTime;
                Add(svDateStamp);
                //db.SystemVariables.Add(svDateStamp);
            }
            return DateTime.Parse(svDateStamp.Value);
        }
        public void UpdateEnergyImportDate(DateTime date)
        {
            SystemVariable svDateStamp = db.SystemVariables.Where(x => x.Name == "EnergyImportDate").FirstOrDefault();
            svDateStamp.Value = date.ToString("yyyy-MM-dd");
            AddOrUpdate(svDateStamp);
        }
        public Int64 GetLastEnergyImportRow()
        {
            SystemVariable svRowRead = db.SystemVariables.Where(x => x.Name == "EnergyImportRow").FirstOrDefault();
            if (svRowRead == null)
            {
                svRowRead = new SystemVariable();
                svRowRead.Name = "EnergyImportRow";
                svRowRead.Value = 1.ToString();
                svRowRead.Type = EnumVariableType.Int;
                Add(svRowRead);
            }
            return Int64.Parse(svRowRead.Value);
        }
        public void UpdateEnergyImportRow(Int64 rowNumber)
        {
            SystemVariable sv = db.SystemVariables.Where(x => x.Name == "EnergyImportRow").FirstOrDefault();
            sv.Value = rowNumber.ToString();
            AddOrUpdate(sv);
        }
        public long GetLastEnergyImportFileSize() //Stream responseStream)
        {
            SystemVariable svFileSize = db.SystemVariables.Where(x => x.Name == "EnergyImportFileSize").FirstOrDefault();
            if (svFileSize == null)
            {
                svFileSize = new SystemVariable();
                svFileSize.Name = "EnergyImportFileSize";
                svFileSize.Value = "0"; //responseStream.Length.ToString();
                svFileSize.Type = MDL_CORE.ComponentCore.Enums.EnumVariableType.Decimal;
                Add(svFileSize);
            }
            return long.Parse(svFileSize.Value);
        }
        public void UpdateEnergyImportFileSize(decimal size) //Stream responseStream)
        {
            SystemVariable svFileSize = db.SystemVariables.Where(x => x.Name == "EnergyImportFileSize").FirstOrDefault();
            svFileSize.Value = size.ToString(); //responseStream.Length.ToString();
            AddOrUpdate(svFileSize);
        }
        public void UpdateJobEnergyMetersImportLastRun (){
            SystemVariable sysvar = db.SystemVariables.Where(x => x.Name == "JobEnergyMetersImportLastRun").FirstOrDefault();
            if(sysvar == null)
            {
                sysvar = new SystemVariable();
                sysvar.Name = "JobEnergyMetersImportLastRun";
                sysvar.Type = EnumVariableType.DateTime;
            }
            sysvar.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            AddOrUpdate(sysvar);
        }
        public decimal GetMaxAvailableWeightToPick()
        {
            decimal result = 0m;
            var obj = db.SystemVariables.Where(x => x.Name == "MaxAvailableWeightToPick").FirstOrDefault();
            if(obj!= null)
            {
                string maxAvailableWeightToPick = obj.Value;
                maxAvailableWeightToPick = maxAvailableWeightToPick.Replace('.', ',');
                if (!Decimal.TryParse(maxAvailableWeightToPick, out result))
                {
                    result = 0;
                }
            }
            return result;
        }
        public decimal GetMaxWeightForPackage()
        {
            decimal result = 0m;
            var obj = db.SystemVariables.Where(x => x.Name == "MaxWeightForPackage").FirstOrDefault();
            if(obj != null)
            {
                string maxWeightForPackage = obj.Value;
                maxWeightForPackage = maxWeightForPackage.Replace('.', ',');
                if (!Decimal.TryParse(maxWeightForPackage, out result))
                {
                    result = 0;
                }
            }
           
            return result;
        }
    }
}