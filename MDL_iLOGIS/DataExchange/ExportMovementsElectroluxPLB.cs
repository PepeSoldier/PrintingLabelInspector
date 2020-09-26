using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.DataExchange
{
    public class ExportMovementsElectroluxPLB : ExportMovementsAbstract
    {
        List<MovementHeader> movementHeaders;
        List<EnumMovementType> consideredMovements;
        DateTime exportDateTime;

        public ExportMovementsElectroluxPLB(IDbContextiLOGIS db) : base(db)
        {
            exportDateTime = DateTime.Now;
            movementHeaders = new List<MovementHeader>();
            consideredMovements = new List<EnumMovementType>() {
                EnumMovementType.CODE_309, EnumMovementType.CODE_310,
                EnumMovementType.CODE_561, EnumMovementType.CODE_562,
                EnumMovementType.CODE_711, EnumMovementType.CODE_712 
            };
        }

        protected override void AdaptData()
        {
            foreach (Movement mov in movementsToBeExported)
            {
                string sourceAccountingWhCode = mov.SourceWarehouse.AccountingWarehouse != null ? mov.SourceWarehouse.AccountingWarehouse.Code : mov.SourceWarehouse.Code;
                string destAccountingWhCode = mov.DestinationWarehouse.AccountingWarehouse != null ? mov.DestinationWarehouse.AccountingWarehouse.Code : mov.DestinationWarehouse.Code;

                if ((mov.Type == EnumMovementType.CODE_311 && sourceAccountingWhCode != destAccountingWhCode) || (consideredMovements.Contains(mov.Type)))
                {
                    MovementHeader movH = new MovementHeader();
                    movH.RECType = "H";
                    movH.DocID = mov.Id.ToString();
                    movH.LIFNR = "";
                    movH.KUNNR = "";
                    movH.NAME1 = "";
                    movH.BUDAT = mov.Date;
                    movH.CPUDT = mov.Date;
                    movH.CPUTM = mov.Date;
                    movH.TCODE = "MB1B";
                    movH.USNAM = mov.User != null ? mov.User.UserName : "";

                    MovementData movD = new MovementData();
                    movD.RECType = "P";
                    movD.DocID = mov.Id.ToString();
                    movD.MATNR = mov.ItemWMS.Item.Code;
                    movD.MAKTX = mov.ItemWMS.Item.Name;
                    movD.MENGE = mov.QtyMoved;
                    movD.MEINS = ConvertUnitOfMeasure(mov.UnitOfMeasure);
                    movD.BWART = Convert.ToString((int)mov.Type);
                    movD.LGFrom = sourceAccountingWhCode;
                    movD.ISFrom = "";
                    movD.MatnrTo = "";
                    movD.MaktxTo = "";
                    movD.LGTo = "";
                    movD.ISTo = "";

                    if (mov.Type == EnumMovementType.CODE_311 || mov.Type == EnumMovementType.CODE_312)
                    {
                        movH.TCODE = "MB1B";
                        movD.LGTo = destAccountingWhCode;
                        movD.ISTo = "";
                    }
                    else if (mov.Type == EnumMovementType.CODE_309 || mov.Type == EnumMovementType.CODE_310)
                    {
                        movH.TCODE = "MB1B";
                        movD.MatnrTo = "?";
                        movD.MaktxTo = "?";
                        movD.LGTo = mov.DestinationWarehouse.AccountingWarehouse.Code;
                        movD.ISTo = "";
                    }
                    else if (mov.Type == EnumMovementType.CODE_561 || mov.Type == EnumMovementType.CODE_562)
                    {
                        movH.TCODE = "MB1C";
                        
                    }
                    else if (mov.Type == EnumMovementType.CODE_711 || mov.Type == EnumMovementType.CODE_712)
                    {
                        movH.TCODE = "MB11";
                    }

                    mov.ExportDateTime = exportDateTime;

                    movH.MovementDataList.Add(movD);
                    movementHeaders.Add(movH);
                }
            }

            uow.MovementRepo.AddOrUpdateRange(movementsToBeExported);
        }
        protected override void SaveData()
        {
            if (movementHeaders.Count <= 0)
            {
                Errors.Add("There are 0 movements to be exported.");
                return;
            }
            //string filePath = @"C:\Users\kamil\Desktop\WORKPLACE\Elux - PLB\SAP Interfaces\ExportMovement_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            string filePath = @"\\plws3807\iLOGIS\iLOGIS_Out\WMS_MOV_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            try
            {
                using (var w = new StreamWriter(filePath))
                {
                    foreach (MovementHeader movH in movementHeaders)
                    {
                        var lineH = string.Format(
                        "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}"
                        ,movH.RECType
                        ,movH.DocID
                        ,movH.LIFNR
                        ,movH.KUNNR
                        ,movH.NAME1
                        ,movH.BUDAT.ToString("yyyyMMdd")
                        ,movH.CPUDT.ToString("yyyyMMdd")
                        ,movH.CPUTM.ToString("HHmmss")
                        ,movH.TCODE
                        ,movH.USNAM
                        );

                        w.WriteLine(lineH);
                        w.Flush();

                        foreach (MovementData movD in movH.MovementDataList)
                        {
                            var lineD = string.Format(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}"
                            ,movD.RECType
                            ,movD.DocID
                            ,movD.MATNR
                            ,movD.MAKTX
                            ,movD.MENGE.ToString("0.000").Replace('.',',')
                            ,movD.MEINS
                            ,movD.BWART
                            ,movD.LGFrom
                            ,movD.ISFrom
                            ,movD.MatnrTo
                            ,movD.MaktxTo
                            ,movD.LGTo
                            ,movD.ISTo
                            );

                            w.WriteLine(lineD);
                            w.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add("Movement export error." + ex.Message);
            }
        }

        private string ConvertUnitOfMeasure(UnitOfMeasure unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case UnitOfMeasure.szt: return "ST";
                case UnitOfMeasure.kg: return "KG";
                case UnitOfMeasure.l: return "L";
                case UnitOfMeasure.m: return "M";
                case UnitOfMeasure.g: return "G";
                case UnitOfMeasure.m2: return "M2";
                case UnitOfMeasure.mm: return "MM";
                case UnitOfMeasure.cm: return "CM";
                case UnitOfMeasure.CS: return "CS";
                case UnitOfMeasure.FT: return "FT";
                case UnitOfMeasure.BOT: return "BOT";
                case UnitOfMeasure.ML: return "ML";
                case UnitOfMeasure.m3: return "M3";
                case UnitOfMeasure.TH: return "TH";
                default: return "ST";
            }
        }
    }
}
