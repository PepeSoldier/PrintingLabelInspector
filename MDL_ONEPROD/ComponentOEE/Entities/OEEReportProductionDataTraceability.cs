using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.Model.OEEProd
{
    [Table("OEE_OEEReportProductionDataTraceability", Schema = "ONEPROD")]
    public class OEEReportProductionDataTraceability : IModelDeletableEntity
    {
        //Model reprezentuje bardziej szczegółowe dane dotyczące produkcji.

        //W formularzu widziałeś, że chcią zapisywać z której rolki stali została 
        //wyprodukowana część i do tego będzie słyżył ten model.

        //Uniwersalność modelu pozwala rejestrowac szczególy nie tylko rolka -> tłoczenie
        //ale każdy kolejny etap produkcji np. z której wytłoczonej cześci zrobiliśmy komponetn na piecach.

        //FUTURE
        //w przyszłości ten model będzie rejestrował na bieżąco produkcję sztuka po sztuce, lub pojemnik po pojemniku.
        //dane będa tu trafiać na podstawie tego co klikną ludzie na ekranach.
        //Będziemy co określony czas sumowac dane i je wpisywać do tabeli nadrzędnej ->> OEEReportProductionData

        //Z ekranów będizemy tez mieć dane o postojach, które będziemy pisać również na bieżąco do  ->> OEEReportProductionData


        public OEEReportProductionDataTraceability()
        { 
        }

        public int Id { get; set; }
        public bool Deleted { get; set; }

        public virtual OEEReportProductionData OEEReportProductionData { get; set; }
        public int OEEReportProductionDataId { get; set; }

        //-----------------PRODUKT
        //kod produktu
        public string ProductPartCode { get; set; }
        //numer seryjny produktu/partii
        public string SerialNo { get; set; }
        //wyprodukowana ilość
        public int ProducedQty { get; set; }

        //-----------------SUROWIEC
        //kod surowca
        public string MaterialPartCode { get; set; }
        //numer seryjny surowca/partii
        public string MaterialSerialNo { get; set; } 
        //ilość zużytego surowca (wyprodukowana ilość * BOM -> ilość/sztukę )
        public int UsedQty { get; set; }
        
    }
}