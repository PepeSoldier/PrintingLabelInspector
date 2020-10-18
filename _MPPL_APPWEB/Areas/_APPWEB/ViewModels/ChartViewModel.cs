using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _LABELINSP_APPWEB.Areas._APPWEB.ViewModels
{
    public class TestChart
    {
        public TestChart()
        {
            VM = new ChartMultiViewModel();
            VM.title = "PRASY - RADAR";
            VM.ChartViewModels = new List<ChartViewModel>();
            VM.ChartViewModels.Add(new ChartViewModel()
            {
                title = "PRASA 1 - RADAR",
                labels = new List<string>() { "OEE", "A", "P", "Q" },
                datasets = new List<ChartDataSetViewModel>() {
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){10.0m,20.0m,30.0m,40.0m },
                        label = "BRYGADA A(OEE-A-P-Q)",
                        backgroundColor = new List<string>{"blue"},
                        borderColor = new List<string>{"blue"},
                    },
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){12.0m,22.0m,32.0m,42.0m },
                        label = "BRYGADA B",
                        backgroundColor = new List<string>{"green"},
                        borderColor = new List<string>{"green"},
                    },
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){14.0m,24.0m,34.0m,44.0m },
                        label = "BRYGADA C",
                        backgroundColor = new List<string>{"orange"},
                        borderColor = new List<string>{"orange"},
                    }
                },
            });
            VM.ChartViewModels.Add(new ChartViewModel()
            {
                title = "PRASA 2 - RADAR",
                labels = new List<string>() { "OEE", "A", "P", "Q" },
                datasets = new List<ChartDataSetViewModel>() {
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){10.0m,20.0m,30.0m,40.0m },
                        label = "BRYGADA A(OEE-A-P-Q)",
                        backgroundColor = new List<string>{"green"},
                        borderColor = new List<string>{"green"},
                    },
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){12.0m,22.0m,32.0m,42.0m },
                        label = "BRYGADA B",
                        backgroundColor = new List<string>{"blue"},
                        borderColor = new List<string>{"blue"},
                    },
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){14.0m,24.0m,34.0m,44.0m },
                        label = "BRYGADA C",
                        backgroundColor = new List<string>{"orange"},
                        borderColor = new List<string>{"orange"},
                    }
                },
            });
            VM.ChartViewModels.Add(new ChartViewModel()
            {
                title = "PRASA 3 - RADAR",
                labels = new List<string>() { "OEE", "A", "P", "Q" },
                datasets = new List<ChartDataSetViewModel>() {
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){10.0m,20.0m,30.0m,40.0m },
                        label = "BRYGADA A(OEE-A-P-Q)",
                        backgroundColor = new List<string>{"red","blue","green","orange"},
                        borderColor = new List<string>{"red","blue","green","orange"},
                    },
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){12.0m,22.0m,32.0m,42.0m },
                        label = "BRYGADA B",
                        backgroundColor = new List<string>{"red","blue","green","orange"},
                        borderColor = new List<string>{"red","blue","green","orange"},
                    },
                    new ChartDataSetViewModel(){
                        data = new List<decimal>(){14.0m,24.0m,34.0m,44.0m },
                        label = "BRYGADA C",
                        backgroundColor = new List<string>{"red","blue","green","orange"},
                        borderColor = new List<string>{"red","blue","green","orange"},
                    }
                },
            });
        }
        public ChartMultiViewModel VM { get; set; }

    }


    public class ChartMultiViewModel
    {
        public string title { get; set; }
        public List<ChartViewModel> ChartViewModels { get; set; }
    }

    public class ChartViewModel
    {
        public ChartViewModel()
        {
            labels = new List<string>();
            datasets = new List<ChartDataSetViewModel>();
            targets = new List<ChartDataSetViewModel>();
        }
        
        public string title { get; set; }
        public int id { get; set; }

        public List<string> labels { get; set; }
        public List<ChartDataSetViewModel> datasets { get; set; }
        public List<ChartDataSetViewModel> targets { get; set; }
    }

    public class ChartDataSetViewModel
    {
        public ChartDataSetViewModel()
        {
            label = "Seria Danych";
            data = new List<decimal>();
            comments = new List<string>();
            backgroundColor = new List<string>();
            borderColor = new List<string>();
            borderWidth = 1;
            type = null;
            pointBackgroundColor = null;
            yAxisID = null;
            fill = false;
            displayUnit = "";
            fontSize = 10;
            fontColor = "#FFFFFF";
            stack = null;
        }

        public int borderWidth { get; set; }
        public int lineTension { get; set; }
        public int fontSize { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string fontColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public int pointBorderWidth { get; set; }
        public int pointRadius { get; set; }
        public string yAxisID { get; set; }
        public string displayUnit { get; set; }
        public bool fill { get; set; }
        public bool steppedLine { get; set; }
        public string datalabels { get; set; }
        public string stack { get; set; }
        

        public List<decimal> data { get; set; }     //[25,40,50]
        public List<string> comments { get; set; } //["","dupa",""]

        public List<string> backgroundColor { get; set; }
        public List<string> borderColor { get; set; }
    }

    //public class ChartDataLabels
    //{
    //    public string anchor { get; set; }
    //    public string align { get; set; }
    //}
}