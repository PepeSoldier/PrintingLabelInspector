using MDL_BASE.Interfaces;
using MDLX_CORE.ComponentCore.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_CORE.ComponentCore.Models
{
    public class SerialNumberManager
    {
        IDbContextCore db;
        public SerialNumberManager(IDbContextCore db)
        {
            this.db = db;
        }

        public int GetNextForONEPROD(int resourceId)
        {
            string variableName = "SerialNumber_resourceId_" + resourceId.ToString();
            return GetSerialNumber_RawSQL(GenerateQuery(variableName));
            //return GetSerialNumber_EF(variableName);
        }
        public int GetNextForILOGIS(int? warehouseId = null)
        {
            string whSuffix = warehouseId != null && warehouseId > 0 ? "_WH_" + warehouseId : string.Empty;
            string variableName = "SerialNumber_StockUnit " + whSuffix;

            return GetSerialNumber_RawSQL(GenerateQuery(variableName));
            //return GetSerialNumber_EF(variableName);
        }
        public int[] GetManyForILOGIS(int qty, int? warehouseId = null)
        {
            string whSuffix = warehouseId != null && warehouseId > 0 ? "_" + warehouseId : string.Empty;
            string variableName = "SerialNumber_StockUnit" + whSuffix;
            
            int min, max;
            max = GetSerialNumber_RawSQL(GenerateQuery(variableName, qty));
            //max = GetSerialNumber_EF(variableName, qty);
            min = max - qty;
            return new int[2] { min, max };
        }
        public static string Format(int value, SerialNumberType formatType)
        {
            if (value < 0) { value = 0; }

            if(formatType == SerialNumberType.D6)
            {
                return Generate_Dx(value, 6);
            }
            else if (formatType == SerialNumberType.D9)
            {
                return Generate_Dx(value, 9);
            }
            else if (formatType == SerialNumberType.HEX4)
            {
                return Generate_HEXx(value, 4);
            }
            else if (formatType == SerialNumberType.YWWD5)
            {
                return Generate_YWWDx(value, 5);
            }
            else if (formatType == SerialNumberType.YWWD6)
            {
                return Generate_YWWDx(value, 6);
            }
            else
            {
                return value.ToString();
            }
        }

        private int GetSerialNumber_EF(string variableName, int qty = 1)
        {
            int? serialNumber = null;

            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    SystemVariable sn_SysVar = db.SystemVariables.FirstOrDefault(x => x.Name == variableName);

                    if(sn_SysVar == null)
                    {
                        sn_SysVar = new SystemVariable() { Name = variableName, Value = "0", Type = Enums.EnumVariableType.Int };
                        db.SetEntryState_Added(sn_SysVar);
                        db.SaveChanges();
                    }

                    serialNumber = Convert.ToInt32(sn_SysVar.Value) + qty;
                    sn_SysVar.Value = Convert.ToString(serialNumber);
                    db.SetEntryState_Modified(sn_SysVar);
                    db.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    serialNumber = null;
                    transaction.Rollback();
                }
            }

            return serialNumber ?? 0;
        }
        private int GetSerialNumber_RawSQL(string query)
        {
            int? serialNumber = null;
            
            try
            {
                serialNumber = db.Database.SqlQuery<int>(query).FirstOrDefault();
            }
            catch (Exception ex)
            {
                serialNumber = -1;
                Console.WriteLine(ex.Message);
            }

            return serialNumber ?? 0;
        }
        private static string GenerateQuery(string variableName, int qty = 1)
        {
            string tblname = "[CORE].[SystemVariables]";

            //var QueryI = "INSERT INTO [EPSS_2B].[CORE].[SystemVariables](Name,Value,Type,UserId) VALUES ('SerialNumber_resourceId_1','1',0,null)";

            string queryUpdate = "UPDATE t SET " +
                "t.[Value] = (SELECT ISNULL((SELECT CONVERT(int, [Value]) FROM " + tblname + " WHERE [Name] = '" + variableName + "'),0) + " + qty + " ) " +
                "FROM " + tblname + " t " +
                "WHERE t.[Name] = '" + variableName + "'";

            string querySelect = "SELECT ISNULL((SELECT CONVERT(int, [Value]) FROM " + tblname + " WHERE [Name] = '" + variableName + "'),0) as val";

            string queryTransaction =
                "BEGIN TRANSACTION " +
                queryUpdate + ";" + querySelect +
                " COMMIT";

            return queryTransaction;
        }
        private bool CheckIfVariableExists(string variableName)
        {
            string tblname = "[CORE].[SystemVariables]";
            string queryGetVar = "SELECT COUNT([Name]) FROM " + tblname + " WHERE [Name] = '" + variableName + "'";
            int count = db.Database.SqlQuery<int>(queryGetVar).FirstOrDefault();

            return count > 0;
        }
        private bool AddVariable(string variableName)
        {
            string tblname = "[CORE].[SystemVariables]";
            string queryInsert = "INSERT INTO " + tblname + "(Name,Value,Type,UserId) VALUES ('" + variableName + "','0',0,null)";
            db.Database.ExecuteSqlCommand(queryInsert);
            string queryGetVar = "SELECT COUNT([Name]) FROM " + tblname + " WHERE [Name] = '" + variableName + "'";
            int count = db.Database.SqlQuery<int>(queryGetVar).FirstOrDefault();

            return count > 0;
        }
        private static string Generate_YWWDx(int value, int decimalLength)
        {
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int weekNum = cul.Calendar.GetWeekOfYear(
                DateTime.Now,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);

            //string y = DateTime.Now.ToString("yyyy").Substring(3, 1);
            //string ww = weekNum > 9 ? weekNum.ToString() : "0" + weekNum.ToString();
            string y = FormatToLength(DateTime.Now.Year.ToString(), 1);
            string ww = FormatToLength(weekNum.ToString(), 2);

            return y + ww + Generate_Dx(value, decimalLength);
        }
        private static string Generate_Dx(int value, int decimalLength)
        {
            string val = value.ToString();
            return FormatToLength(val, decimalLength);
        }
        private static string Generate_HEXx(int value, int decimalLength)
        {
            string val = Convert.ToString(value, 16);
            return FormatToLength(val, decimalLength);
        }
        private static string FormatToLength(string val, int decimalLength)
        {
            val = val.Length > decimalLength ? val.Substring(val.Length - decimalLength, decimalLength) : val;
            return Convert.ToInt32(val).ToString("D" + decimalLength.ToString());
        }
    }
}