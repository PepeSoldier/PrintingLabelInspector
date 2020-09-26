using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDLX_MASTERDATA.Models
{
    public class ConverterUoM
    {
        private static List<Converter> converters = new List<Converter>() {
            new Converter { Source = UnitOfMeasure.kg, Dest = UnitOfMeasure.g, Ratio = 1000 },
            new Converter { Source = UnitOfMeasure.m, Dest = UnitOfMeasure.mm, Ratio = 1000 },
            new Converter { Source = UnitOfMeasure.m, Dest = UnitOfMeasure.cm, Ratio = 100 },
            new Converter { Source = UnitOfMeasure.FT, Dest = UnitOfMeasure.cm, Ratio = 30.48m },
            new Converter { Source = UnitOfMeasure.mi, Dest = UnitOfMeasure.FT, Ratio = 5280 },
            new Converter { Source = UnitOfMeasure.l, Dest = UnitOfMeasure.ML, Ratio = 1000 },
        };

        public static decimal Convert(decimal qty, UnitOfMeasure sourceUnitOfMeasure, UnitOfMeasure destUnitOfMeasure, List<ItemUoM> itemUoMs = null)
        {
            Converter converter_std = null;
            Converter converter_rev = null;

            ItemUoM itemUoM_std = itemUoMs == null? null : itemUoMs.FirstOrDefault(x => x.DefaultUnitOfMeasure == sourceUnitOfMeasure && x.AlternativeUnitOfMeasure == destUnitOfMeasure);
            ItemUoM itemUoM_rev = itemUoMs == null? null : itemUoMs.FirstOrDefault(x => x.DefaultUnitOfMeasure == destUnitOfMeasure && x.AlternativeUnitOfMeasure == sourceUnitOfMeasure);

            if (itemUoM_std != null)
            {
                converter_std = new Converter() { Source = itemUoM_std.DefaultUnitOfMeasure, Dest = itemUoM_std.AlternativeUnitOfMeasure, Ratio = itemUoM_std.QtyForAlternativeUnitOfMeasure / itemUoM_std.QtyForDefaultUnitOfMeasure };
            }
            else if (itemUoM_rev != null)
            {
                converter_rev = new Converter() { Source = itemUoM_rev.DefaultUnitOfMeasure, Dest = itemUoM_rev.AlternativeUnitOfMeasure, Ratio = itemUoM_rev.QtyForAlternativeUnitOfMeasure / itemUoM_rev.QtyForDefaultUnitOfMeasure };
            }
            else
            {
                converter_std = converters.FirstOrDefault(x => x.Source == sourceUnitOfMeasure && x.Dest == destUnitOfMeasure);
                converter_rev = converters.FirstOrDefault(x => x.Dest == sourceUnitOfMeasure && x.Source == destUnitOfMeasure);
            }

            if (converter_std != null) 
                return _StandardConversion(qty, converter_std);
            else if (converter_rev != null) 
                return _ReversedConversion(qty, converter_rev);
            else
                return _CombinedConversion(qty, sourceUnitOfMeasure, destUnitOfMeasure);
        }
        public static List<UnitOfMeasure> AlternativeUnitOfMeasures(UnitOfMeasure unitOfMeasure)
        {
            List<UnitOfMeasure> sourceUnits = converters.Where(x => x.Dest == unitOfMeasure).Select(s => s.Source).Distinct().ToList();
            sourceUnits.Add(unitOfMeasure);

            List<UnitOfMeasure> potentialUnits = converters.Where(x => sourceUnits.Contains(x.Source)).Select(s => s.Dest).Distinct().ToList();
            potentialUnits.AddRange(sourceUnits);

            return potentialUnits.Distinct().ToList();
        }

        private static decimal _StandardConversion(decimal qty, Converter converter)
        {
            return qty * converter.Ratio;
        }
        private static decimal _ReversedConversion(decimal qty, Converter converter)
        {
            return qty / converter.Ratio;
        }
        private static decimal _CombinedConversion(decimal qty, UnitOfMeasure sourceUnitOfMeasure, UnitOfMeasure destUnitOfMeasure)
        {
            var _sharedConverters = converters
                .Where(x => x.Dest == sourceUnitOfMeasure || x.Dest == destUnitOfMeasure)
                .GroupBy(g => g.Source)
                .Select(x => new { Source = x.Key, Count = x.Count() })
                .FirstOrDefault(f => f.Count > 1);

            if (_sharedConverters != null)
            {
                //istnieje wspólna jednostka główna, przez ktora mozna dokonac konwersji 
                //np. cm -> mm = (cm -> m).then(m -> mm) 
                UnitOfMeasure sharedMainUnitOfMeasure = _sharedConverters.Source;
                Converter converter_s = converters.FirstOrDefault(x => x.Source == sharedMainUnitOfMeasure && x.Dest == sourceUnitOfMeasure);
                Converter converter_d = converters.FirstOrDefault(x => x.Source == sharedMainUnitOfMeasure && x.Dest == destUnitOfMeasure);

                return qty * (converter_d.Ratio / converter_s.Ratio);
            }
            else
            {
                //trudniejsze przejście pomiedzy jednostami. 
                //np. mile -> mm = (mile -> cm).then(cm -> mm) = (cm -> m).then(m -> mm) 
                var sourceUnits = converters.Where(x => x.Dest == destUnitOfMeasure).Select(s => s.Source).Distinct();
                var potentialUnits = converters.Where(x => sourceUnits.Contains(x.Source)).Select(s => s.Dest).Distinct();
                var converter_partial = converters.FirstOrDefault(x => x.Source == sourceUnitOfMeasure && potentialUnits.Contains(x.Dest));

                if (converter_partial != null)
                {
                    decimal partialyConverted = _StandardConversion(qty, converter_partial);
                    return _CombinedConversion(partialyConverted, converter_partial.Dest, destUnitOfMeasure);
                }
                else
                {
                    return 0;
                }
            }
        }

        class Converter
        {
            public UnitOfMeasure Source { get; set; }
            public UnitOfMeasure Dest { get; set; }
            public decimal Ratio { get; set; }
        }
    }
}
