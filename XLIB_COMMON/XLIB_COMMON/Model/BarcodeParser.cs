using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using XLIB_COMMON.Interface;

namespace XLIB_COMMON.Model
{
    public class BarcodeManager : IBarcodeParser
    {
        List<BarcodeParserProperty> properties;
        List<BarcodeParserProperty> propertiesForGenerate;

        public int StockUnitId { get; set; }
        public string ItemCode { get; set; }
        public decimal Qty { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string SerialNumber { get; private set; }
        public string Barcode { get; private set; }
        public bool Error { get; set; }
        public bool ErrorWrongLength { get; set; }
        public bool ErrorUnexpectedQtyChar { get; set; }
        public bool ErrorReapeatedChar { get; set; }

        public BarcodeManager()
        {
            Qty = 0;
            Error = false;
            properties = new List<BarcodeParserProperty>();
            properties.Add(new BarcodeParserProperty() { Char = 'S', Name = "barcodeSerialChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'Q', Name = "barcodeQtyChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'D', Name = "barcodeDecimalChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'C', Name = "barcodeItemCodeChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'L', Name = "barcodeLocationChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'Y', Name = "barcodeYearChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'M', Name = "barcodeMonthChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'd', Name = "barcodeDayChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'H', Name = "barcodeHourChar" });
            properties.Add(new BarcodeParserProperty() { Char = 'm', Name = "barcodeMinuteChar" });
            properties.Add(new BarcodeParserProperty() { Char = 's', Name = "barcodeSecondChar" });
        }

        public virtual void Parse(string barcode, string template)
        {
            if (template.Length != barcode.Length)
            {
                Error = true;
                ErrorWrongLength = true;
            }
            else
            {
                AnalyzeTemplate(template);
                DateTime = new DateTime();

                foreach (BarcodeParserProperty prop in properties)
                {
                    switch (prop.Name)
                    {
                        case "barcodeSerialChar": SerialNumber = TrimHashes(TrimSerialNumer(GetString(barcode, prop))); break;
                        case "barcodeItemCodeChar": ItemCode = TrimHashes(GetString(barcode, prop)); break;
                        case "barcodeQtyChar": Qty += GetInt(barcode, prop); break;
                        case "barcodeDecimalChar": Qty += GetDecimal(barcode, prop); break;
                        case "barcodeLocationChar": Location = TrimHashes(GetString(barcode, prop)); break;

                        case "barcodeYearChar": DateTime = DateTime.AddYears(Math.Max(0, GetYear(barcode, prop) - 1)); break;
                        case "barcodeMonthChar": DateTime = DateTime.AddMonths(Math.Max(0, GetInt(barcode, prop) - 1)); break;
                        case "barcodeDayChar": DateTime = DateTime.AddDays(Math.Max(0, GetInt(barcode, prop) - 1)); break;
                        case "barcodeHourChar": DateTime = DateTime.AddHours(GetInt(barcode, prop)); break;
                        case "barcodeMinuteChar": DateTime = DateTime.AddMinutes(GetInt(barcode, prop)); break;
                        case "barcodeSecondChar": DateTime = DateTime.AddSeconds(GetInt(barcode, prop)); break;
                    }
                }
            }
        }        
        public void Generate(string rawSerialNumber, string template)
        {
            StringBuilder barcodeBuilder = new StringBuilder();
            SerialNumber = rawSerialNumber;
            AnalyzeTemplate(template, true);

            foreach (BarcodeParserProperty prop in propertiesForGenerate)
            {
                switch (prop.Name)
                {
                    case "barcodeSerialChar": barcodeBuilder.Append(PrepareValueForBarcode(SerialNumber, prop)); break;
                    case "barcodeItemCodeChar": barcodeBuilder.Append(PrepareValueForBarcode(ItemCode, prop)); break;
                    case "barcodeLocationChar": barcodeBuilder.Append(PrepareValueForBarcode(Location, prop)); break;
                    case "barcodeQtyChar": barcodeBuilder.Append(PrepareValueForBarcode_int(Convert.ToInt32(Qty).ToString(), prop)); break;
                    case "barcodeDecimalChar": barcodeBuilder.Append(PrepareValueForBarcode_decimal(Qty, prop)); break;
                    case "barcodeYearChar": barcodeBuilder.Append(PrepareValueForBarcode_int(DateTime.Year.ToString(), prop)); break;
                    case "barcodeMonthChar": barcodeBuilder.Append(PrepareValueForBarcode_int(DateTime.Month.ToString(), prop)); break;
                    case "barcodeDayChar": barcodeBuilder.Append(PrepareValueForBarcode_int(DateTime.Day.ToString(), prop)); break;
                    case "barcodeHourChar": barcodeBuilder.Append(PrepareValueForBarcode_int(DateTime.Hour.ToString(), prop)); break;
                    case "barcodeMinuteChar": barcodeBuilder.Append(PrepareValueForBarcode_int(DateTime.Minute.ToString(), prop)); break;
                    case "barcodeSecondChar": barcodeBuilder.Append(PrepareValueForBarcode_int(DateTime.Second.ToString(), prop)); break;
                    default: barcodeBuilder.Append(prop.Char); break;
                }
            }

            Barcode = barcodeBuilder.ToString();
        }

        //-----------------------------------------------------------------GENERAL
        private void AnalyzeTemplate(string template, bool forGenerate =  false)
        {
            if (forGenerate)
            {
                propertiesForGenerate = new List<BarcodeParserProperty>();
            }

            BarcodeParserProperty prop = null;
            char prevChar = '?';
            int i = 0;
            foreach (char c in template)
            {
                if (c != prevChar)
                {
                    if (prop != null)
                    {
                        prop.Range[1] = i;
                        prop.Ready = true;
                    }

                    prop = properties.FirstOrDefault(x => x.Char == c);

                    if (prop != null)
                    {
                        if (prop.Ready == true)
                        {
                            Error = true;
                            ErrorReapeatedChar = true;
                        }
                        else
                        {
                            prop.Range[0] = i;
                            prop.Sort = i;
                        }                        
                    }


                    if (forGenerate)
                    {
                        if (prop != null)
                        {
                            propertiesForGenerate.Add(prop);
                        }
                        else
                        {
                            propertiesForGenerate.Add(new BarcodeParserProperty()
                            {
                                Char = c,
                                Sort = i,
                            });
                        }
                    }
                }
                prevChar = c;
                i++;
            }

            if (prop != null)
            {
                //dla ostatniego trzeba ustawić koncowy zakres
                prop.Range[1] = template.Length;
            }
        }
        private string TrimHashes(string value)
        {
            return value.Replace("#", "");
        }
        private string TrimSerialNumer(string serialNumber)
        {
            int firstNoZero = 0;
            for(int i =0; i < serialNumber.Length; i++)
            {
                if(serialNumber[i] != '0')
                {
                    firstNoZero = i;
                    break;
                }
            }

            return serialNumber.Substring(firstNoZero, serialNumber.Length - firstNoZero);
        }
        private string GetString(string barcode, BarcodeParserProperty p)
        {
            int length = p.Range[1] - p.Range[0];

            if (barcode != null && length > 0)
            {
                return barcode.Substring(p.Range[0], length);
            }
            else
            {
                return "";
            }
        }
        private decimal GetDecimal(string barcode, BarcodeParserProperty p)
        {
            int decimals = p.Range[1] - p.Range[0];
            decimal value = 0;

            if (decimals > 0)
            {
                try
                {
                    value = Convert.ToDecimal(barcode.Substring(p.Range[0], p.Range[1] - p.Range[0]));
                    value = value / (decimal)Math.Pow(10, decimals);
                }
                catch {
                    Error = true;
                    ErrorUnexpectedQtyChar = true;
                }
            }

            return value;
        }
        
        private int GetInt(string barcode, BarcodeParserProperty p)
        {
            int length = p.Range[1] - p.Range[0];
            int value = 0;

            if (length > 0)
            {
                try
                {
                    value = Convert.ToInt32(barcode.Substring(p.Range[0], p.Range[1] - p.Range[0]));
                }
                catch
                {
                    Error = true;
                    ErrorUnexpectedQtyChar = true;
                }
            }

            return value;
        }
        private int GetYear(string barcode, BarcodeParserProperty p)
        {
            int length = p.Range[1] - p.Range[0];
            int year = 0;

            if (length > 0)
            {
                year = Convert.ToInt32(barcode.Substring(p.Range[0], length));

                if (length <= 2)
                {
                    year += 2000;
                }
            }

            return year;
        }
        private string PrepareValueForBarcode(string value, BarcodeParserProperty p)
        {
            int length = p.Range[1] - p.Range[0];
            string newValue = "";
            
            if (value != null && length > 0)
            {
                if(value.Length > length)
                {
                    value = value.Substring(value.Length - length, length);
                }
                newValue = FillHashes(value, length);
            }

            return newValue;
        }
        private string PrepareValueForBarcode_int(string value, BarcodeParserProperty p)
        {
            int length = p.Range[1] - p.Range[0];
            string newValue = "";

            if (value != null && length > 0)
            {
                if (value.Length > length)
                {
                    value = value.Substring(value.Length - length, length);
                }
                newValue = FillZeros(value, length);
            }

            return newValue;
        }
        private string PrepareValueForBarcode_decimal(decimal value, BarcodeParserProperty p)
        {
            int length = p.Range[1] - p.Range[0];
            string newValue = "";

            if (length > 0)
            {
                decimal intVal = Convert.ToInt32(value);
                value -= intVal;
                value *= (decimal)Math.Pow(10, length);
                newValue = Convert.ToInt32(value).ToString();
                
                if (newValue.Length > length)
                {
                    newValue = newValue.Substring(newValue.Length - length, length);
                }
                newValue = FillZeros(newValue, length);
            }

            return newValue;
        }
        private string FillZeros(string value, int desiredLength)
        {
            string zeros = "";
            for(int i = value.Length; i < desiredLength; i ++)
            {
                zeros += "0";
            }

            return zeros + value;
        }
        private string FillHashes(string value, int desiredLength)
        {
            string hashes = "";
            for (int i = value.Length; i < desiredLength; i++)
            {
                hashes += "#";
            }

            return hashes + value;
        }

        //--------------------------------------------------TO-BE-DELETED
        public virtual int GetQty(string barcode)
        {
            return 0;
        }
        public virtual string GetName(string barcode)
        {
            return "?";
        }
        public virtual string GetSerialNumber(string barcode)
        {
            return "0";
        }
        public virtual string GetCode(string barcode)
        {
            return barcode;
        }
    }

    public class BarcodeParserProperty
    {
        public BarcodeParserProperty()
        {
            Range = new int[2] { -1, -1 };
        }
        public string Name { get; set; }
        public int[] Range { get; set; }
        public char Char { get; set; }
        public bool Ready { get; set; }
        public int Sort { get; set; }
    }

    public class BarcodeParserEluxPLVTech : BarcodeManager
    {
        //CCCCCCCCCQQQQSSSSSSSSS
        public override string GetName(string barcode)
        {
            return "";
        }

        public override int GetQty(string barcode)
        {
            return 10;
        }

        public override string GetSerialNumber(string barcode)
        {
            return barcode;
        }

        public override string GetCode(string barcode)
        {
            return barcode;
        }
    }
}