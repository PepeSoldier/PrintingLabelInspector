class GanttChartItem extends HTMLElement
{
    ViewModel = {
        Workorder: {
            Id: 0,
            Number: "",
            ItemCode: "",
            ItemName: "",
            ItemGroupName: "",
            ClientOrderId: 0,
            ClientOrderNo: 0,
            ResourceGroupId: 0,
            ResourceGroupShowBatches: false,
            Qty_Total: 0,
            Qty_Produced: 0,
            Qty_Remain: 0,
            BatchNumber: 0,
            ProcessingTime: 0,
            StartTime: new Date(),
            EndTime: new Date(),
            ToolName: "",
            Param1: "",
            ItemColor2: "",
            BackgroundColor: "gray",
            //wyliczane
            Width: 0,
            draggableWorkorderClass: "cssClass",
            MarginLeftStr: 0,
            WidthStr: 0,
            cssShadow: ""
        },
        Test: ""

    }
    TemplateStr = "";
    SelectedDateFrom = null;
    Zoom = 1;
    SecToPixelRatio = 1;

    attrCounter = 0;

    constructor() {
        super();
        this.Actions();
        //komentarz
        //dupa
    }
    connectedCallback() {
        this.Render(this);
    }
    disconnectedCallback() {
    }

    static get observedAttributes() {
        return ['data', 'data-datefrom', 'data-zoom'];
    }
    attributeChangedCallback(name, oldValue, newValue) {
        if (name == 'data') {
            this.ViewModel = JSON.parse(newValue);
            this.removeAttribute('data');
            this.attrCounter++;
        }
        else if (name == 'data-datefrom') {
            this.SelectedDateFrom = newValue;
            this.removeAttribute('data-datefrom');
            this.attrCounter++;
        }
        else if (name == 'data-zoom') {
            this.Zoom = newValue;
            this.SecToPixelRatio = parseFloat(parseFloat(this.Zoom) / 60);
            this.removeAttribute('data-zoom');
            this.attrCounter++;
        }
    }
    adoptedCallback() {
    }

    Render(targetElement) {
        let obj = this;
        let vm = this.ViewModel;

        if (vm != null)
        {
            let templateName = "";

            this.CalcWorkorderParams(this.SelectedDateFrom, vm, 0);

            this.classList.add("sgcBlock");
            this.classList.add("sgcBlockWorkorder");
            this.style.left = this.ViewModel.MarginLeft + "px";
            this.style.width = this.ViewModel.Width + "px";
            this.style.backgroundColor = this.ViewModel.BackgroundColor;


            if (vm.Param1 == -2 || vm.LV == 0) {
                //templateName = "GanttChartItem.Template.ClientOrder.cshtml";
                //vm.QtyRemain = vm.Qty_Total - vm.Qty_Produced;
                //vm.width = (vm.Qty_Produced) * 100 / vm.Qty_Total;
                //this.style.backgroundColor = "#7dbdd4";

                templateName = "GanttChartItem.Template.cshtml";
                vm.ItemColor2 = vm.ItemColor2 != null ? vm.ItemColor2 : "none";
                vm.cssShadow = (vm.ItemColor2 == "#ffffff" || vm.ItemColor2 == "rgb(255, 255, 255)" || vm.ItemColor2 == "White") ? " text-shadow: 1px 1px 2px #444444;" : "";
                vm.workorderReady = null;
                vm.workorderUsed = null;
                vm.isStarted = vm.Qty_Produced > 0 ? true : false;
                vm.width = (vm.Qty_Produced) * 100 / vm.Qty_Total;
                vm.workorderLabel = vm.Width > 19 && !vm.ResourceGroupShowBatches ? true : false;
                
                if (window.template1Cache == null) {
                    fetch('/Areas/ONEPROD/Views/APS/' + templateName)
                        .then((response) => response.text())
                        .then((template) => {
                            window.template1Cache = template;
                            $(targetElement).html(Mustache.render(window.template1Cache, vm));
                            obj.PostRender();
                        });
                }
                else {
                    $(targetElement).html(Mustache.render(window.template1Cache, vm));
                    obj.PostRender();
                }
            }
            else if (vm.ItemCode != null && vm.ItemCode.length > 0) {
                templateName = "GanttChartItem.Template.cshtml";
                vm.ItemColor2 = vm.ItemColor2 != null ? vm.ItemColor2 : "none";
                vm.cssShadow = (vm.ItemColor2 == "#ffffff" || vm.ItemColor2 == "rgb(255, 255, 255)" || vm.ItemColor2 == "White") ? " text-shadow: 1px 1px 2px #444444;" : "";
                vm.workorderReady = vm.Qty_Produced > 0 && vm.Qty_Produced < vm.Qty_Total ? vm.Qty_Produced * 100 / vm.Qty_Total : null;
                vm.workorderUsed = vm.Qty_Used > 0 && vm.Qty_Used < vm.Qty_Total ? vm.Qty_Used * 100 / vm.Qty_Total : null;
                vm.workorderLabel = vm.Width > 19 && !vm.ResourceGroupShowBatches ? true : false;
                //vm.draggableWorkorderClass = (vm.ResourceGroupShowBatches) ? "" : "draggableWorkorder";
                if (vm.ResourceGroupShowBatches == false) {
                    this.classList.add("draggableWorkorder");
                }
                this.classList.add("workorderArea_" + this.ViewModel.ResourceGroupId);

                if (window.template2Cache == null) {
                    fetch('/Areas/ONEPROD/Views/APS/' + templateName)
                        .then((response) => response.text())
                        .then((template) => {
                            window.template2Cache = template;
                            $(targetElement).html(Mustache.render(window.template2Cache, vm));
                            obj.PostRender();
                        });
                }
                else {
                    $(targetElement).html(Mustache.render(window.template2Cache, vm));
                    obj.PostRender();
                }
            }
            else{
                templateName = "GanttChartItem.Template.ChangeOver.cshtml";
                vm.widthStr = vm.ProcessingTime / 30;
                vm.isStarted = vm.Qty_Produced > 0 ? true : false;
                vm.width = (vm.Qty_Produced) * 100 / vm.Qty_Total;

                if (window.template3Cache == null) {
                    fetch('/Areas/ONEPROD/Views/APS/' + templateName)
                        .then((response) => response.text())
                        .then((template) => {
                            window.template3Cache = template;
                            $(targetElement).html(Mustache.render(window.template3Cache, vm));
                            obj.PostRender();
                        });
                }
                else {
                    $(targetElement).html(Mustache.render(window.template3Cache, vm));
                    obj.PostRender();
                }

            }

            window.EndTime = new Date();
        }
    }
    PostRender() {
        //chart.doDroppable(".resourceArea_16", ".workorderArea_16");
        //this.doDroppable(".resourceArea_" + this.ViewModel.ResourceGroupId, ".workorderArea_" + this.ViewModel.ResourceGroupId);
        //this.doDroppable(".resourceArea_" + this.ViewModel.ResourceGroupId, ".workorderArea_" + this.ViewModel.ResourceGroupId);
    }
    CalcWorkorderParams(SelectedDateFrom, wo, areaSafetyTime) {
        //if (wo.StartTime >= SelectedDateFrom) {
        let duration = moment.duration(new moment(wo.StartTime).diff(new moment(SelectedDateFrom)));
        let seconds = duration.asSeconds();
        let processingime = Math.min(wo.ProcessingTime, 28800);
        let safeEndTime = new moment(wo.DueDate).add(-areaSafetyTime * 0.5, 'minutes');
        let TaskScheduleStatus_used = 90;
        let TaskScheduleStatus_produced = 70;

        wo.Width = Math.min(parseFloat(processingime * this.SecToPixelRatio), 28800);
        wo.MarginLeft = parseFloat(seconds * this.SecToPixelRatio);
        wo.MarginRight = 0; //ustawić dla production order....               
        wo.BackgroundColor = wo.ItemId == null ? "#d9d9d9" : wo.ItemGroupColor;
        wo.SpecialCssClass =
            wo.ItemId == null ? "taskSetup" :
                wo.EndTime > wo.DueDate ? "taskDangerous" :
                    wo.EndTime > safeEndTime ? "taskWarning" : "";

        if (wo.Status == TaskScheduleStatus_used || wo.Qty_Used == wo.Qty_Total) {
            wo.BackgroundColor = "#377316";
            wo.FontColor = "white";
        }
        else if (wo.Status == TaskScheduleStatus_produced || wo.Qty_Remain == 0) {
            wo.BackgroundColor = "#35ad32";
            wo.FontColor = "rgb(0,109,69)";
        }
        else {
            wo.BackgroundColor = wo.BackgroundColor;
            wo.FontColor = wo.FontColor;
        }
        //}
    }

    
    Test() {
        console.log("Test działa :) " + this.querySelector(".ganttchartItem").getAttribute("data-orderno"));
    }

    Update() {
        this.SelectedDateFrom = new moment(document.querySelector('oneprod-aps-ganttchart').ViewModel.SelectedDateFrom);
        this.Zoom = document.querySelector('oneprod-aps-ganttchart').ViewModel.Zoom;
        this.SecToPixelRatio = parseFloat(parseFloat(this.Zoom) / 60);
    
        let obj = this;
        let selectedDateFrom_tem = new moment(document.querySelector('oneprod-aps-ganttchart').ViewModel.SelectedDateFrom);
        let workorderId = this.querySelector(".ganttchartItem").getAttribute("workorderid");
        let resourceId = this.closest('oneprod-aps-ganttchart-resourcerow').getAttribute("data-resourceid");
        let secFromDateFrom = parseFloat(this.style.left.replace("px", "")) * 60 / this.Zoom;
        let newStartDate = selectedDateFrom_tem.add(secFromDateFrom, 'seconds');

        //console.log("left: " + this.style.left);
        //console.log("width: " + this.style.width);
        //console.log("workorderid: " + workorderId);
        //console.log("resourceid: " + resourceId);
        //console.log("zoom:" + this.Zoom);
        //console.log("DateFrom:" + this.SelectedDateFrom.format("YYYY-MM-DD HH:mm"));
        //console.log("newStartTime:" + newStartDate.format("YYYY-MM-DD HH:mm:ss"));

        $.ajax({
            url: "/ONEPROD/APS/UpdateWorkorder",
            type: 'POST',
            data: '{ workorderId: ' + workorderId + ', resourceId: ' + resourceId + ', newStartTime: "' + newStartDate.format("YYYY-MM-DD HH:mm:ss") + '"}',
            contentType: 'application/json; charset=utf-8',
            success: function (workorderViewModel) {
                console.log("zapis powiódł się");
                obj.ViewModel = workorderViewModel;
                obj.Render(obj);
                obj.ViewModel = null;
                new Alert().Show("success", "Nowa pozycja zlecenia została zapisana");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                new Alert().Show("warning", "Zapis nie powiódł się");
                new Alert().Show("danger", errorThrown);
                new Alert().Show("danger", $(XMLHttpRequest.responseText).filter("title").text());
            }
        });
    }

    Actions() {
        $(document).off("click", "#testbtn")
        $(document).on("click", "#testbtn", function () {
            console.log("testbtn clicked 2");
        });
    }
}
customElements.define('oneprod-aps-ganttchart-item', GanttChartItem);
