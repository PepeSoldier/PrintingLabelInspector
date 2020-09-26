$.fn.hScroll = function (amount) {
    amount = amount || 120;
    $(this).bind("DOMMouseScroll mousewheel", function (event) {
        var oEvent = event.originalEvent,
            direction = oEvent.detail ? oEvent.detail * -amount : oEvent.wheelDelta,
            position = $(this).scrollLeft();
        position += direction > 0 ? -amount : amount;
        $(this).scrollLeft(position);
        event.preventDefault();
    });
};

function Chart(dateFrom) {

    var elements = new Array();
    var prevBckgr = new Array();
    var self = this;
    var dateFrom1 = dateFrom;
    var iSa;
    var saElements;

    this.showBatch = function ($el) {
        deselectElements();
        elements = $("#GantChart").find("[batchNumber='" + $el.attr("batchNumber") + "']");
        selectElements();
    };
    this.workorderContextMenu = function (mainDiv, workorderDiv) {
        $(mainDiv).contextmenu({
            delegate: workorderDiv,
            menu: [
                { title: "Zaznacz batch", cmd: "selectBatch" },
                { title: "Pokaż powiązane", cmd: "showConnected" },
                { title: "----" },
                {
                    title: "Inne", children: [
                        { title: "Skasuj", cmd: "delete" },
                        { title: "Ukryj", cmd: "hide" }
                    ]
                }
            ],
            select: function (event, ui) {
                if (ui.cmd == "hide") {
                    ui.target.hide();
                }
                else if (ui.cmd == "delete") {
                    ui.target.remove();
                }
                else if (ui.cmd == "selectBatch") {
                    self.showBatch($(ui.target));
                }
                else if (ui.cmd == "showConnected") {
                    self.showConnected($(ui.target));
                }
            }
        });
        //$("#menu1").mouseleave(function () { $(this).hide(); });
    };
    this.doDroppable = function (droppableDiv, draggableDivAccepted) {
        console.log("do draggable");
        console.log(droppableDiv);
        console.log(draggableDivAccepted);

        $(droppableDiv).droppable({
            tolerance: "intersect",
            accept: draggableDivAccepted,
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover",
            drop: function (event, ui) {
                //console.log("event:" + event);
                //console.log("ui:" + ui);
                var horizontalShift = 0;
                var verticalShift = 0;

                var scrollLeft = null;

                if (ui.draggable[0].parentElement.id == "Workorderbuffer" && event.target.id != "Workorderbuffer") {
                    //console.log("move from buffer");
                    scrollLeft = $(".sgcWorkordersCol")[0].scrollLeft;
                    horizontalShift += scrollLeft - 140;
                    verticalShift -= 38;
                }
                if (ui.draggable[0].parentElement.id != "Workorderbuffer" && event.target.id == "Workorderbuffer") {
                    //console.log("move to buffer");
                    scrollLeft = $(".sgcWorkordersCol")[0].scrollLeft;
                    horizontalShift -= scrollLeft - 140;
                    verticalShift += 38;
                }

                //console.log("offset: " + ui.draggable.offset().left + ", shiftH: " + horizontalShift);
                var off = ui.draggable.offset();
                off.left += horizontalShift;
                off.top += verticalShift;
                ui.position.left += horizontalShift;

                ui.draggable.offset(off);
                //console.log("offset po: " + ui.draggable.offset().left);

                $(this).append($(ui.draggable));

                ui.helper[0].Update();

                //var workorderId = $(ui.helper[0]).attr('workorderId');
                //var batchNumber = $(ui.helper[0]).attr('batchNumber');
                //console.log("workorderid: " + workorderId + ", batchid: " + batchNumber);
                //console.log(ui.helper[0]);

                //if (workorderId > 0) {
                //    self.updateWorkorder(ui.helper[0], ui.position.left, workorderId, $(ui.helper[0].parentNode).attr('resourceId'));
                //}
                //else if (batchNumber > 0) {
                //    self.UpdateBatch(ui.helper[0], ui.position.left, batchNumber, $(ui.helper[0].parentNode).attr('resourceId'));
                //}
            }
        });
    };

    this.findBestPlace = function (workorderId, startDate) {
        $.ajax({
            url: BaseUrl + "APS/FindBestPlace",
            type: 'POST',
            data: '{workorderId: ' + workorderId + ', startDate: "' + startDate + '"}',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                console.log(result);
                var workorder = document.getElementById("workorder_" + result.WorkorderId); //$("#workorder_" + result.WorkorderId);
                var resource = document.getElementById("resource_" + result.ResourceId);

                $(workorder).css('left', 0);
                $(workorder).detach();
                resource.appendChild(workorder);
                $(workorder).css('left', result.MarginLeft);

                self.updateWorkorder($(workorder), result.MarginLeft, result.WorkorderId, result.ResourceId)
            }
        });
    };
    this.moveWorkorder = function (workorderId, workorderResource, newDate) {

    };
    this.initSA = function (elements) {
        saElements = elements;
        iSa = 0;
    };
    this.stepSA = function () {
        if (iSa > saElements.length - 1) {
            iSa = 0;
        }

        var workorderId = $(saElements[iSa]).attr('workorderId');
        self.showConnected($(saElements[iSa]));
        self.findBestPlace(workorderId, dateFrom1);
        iSa++;
    };
}

class GanttChart extends HTMLElement {

    ResourceGroupsIdFilter = null;

    ViewModel = {
        data: {
            ResourceGroups: [{
                Id: 0,
                Name: "",
                StageNo: 0,
                Resources: [{
                    Id: "ResourceId",
                    Name: "ResourceName",
                    ResourceGroupId: 1,
                    ResourceGroupName: "ResourceGroupName",
                    ShowBatches: false,
                    Workorders: []
                }]
            }]
        },
        CurrentTimeRedLinePosition: 0,
        SelectedDateFrom: "YYYY-MM-DD HH:mm",
        SelectedDateTo: "YYYY-MM-DD HH:mm",
        SecToPixelRatio: 2,
        Zoom: 2,
        width: "0",
        isData: false
    }

    constructor() {
        super();
        this.Refresh = this.Refresh.bind(this);
        this.ShowConnected = this.ShowConnected.bind(this);
        this.ShowBatch = this.ShowBatch.bind(this);
        this.Actions(this);
    }
    static get observedAttributes() {
        return ['data', 'data-refreshbtnid', 'data-resourcegroupsid'];
    }
    attributeChangedCallback(name, oldValue, newValue) {

        console.log("attributeChangedCallback: " + name);

        if (name == 'data-refreshbtnid') {
            console.log("tutaj");
            this.incrementBtn = document.querySelector("#" + newValue);
        }
        else if (name == 'data-resourcegroupsid') {
            console.log("resourceGroupsId: " + newValue);
            if (newValue != 0) {
                this.ResourceGroupsIdFilter = newValue;
            }
        }
    }
    connectedCallback() {
        console.log(this);
        this.incrementBtn.addEventListener('click', this.Refresh);
    }
    disconnectedCallback() {
        //this.incrementBtn.removeEventListener('click', this.Refresh);
    }
    adoptedCallback() { }

    Refresh() {
        let obj = this;
        this.ReadFilters();

        let js = new JsonHelper().GetPostData("/ONEPROD/APS/GanttChartJS",
            {
                zoom: this.ViewModel.Zoom,
                resourceGroupsIds: this.ResourceGroupsIdFilter
            });
        js.done(function (GantChart2ViewModel) {
            obj.ViewModel.data = GantChart2ViewModel;
            obj.CacheTemplatesAndRender();
        });
    }
    ReadFilters() {
        this.ViewModel.SelectedDateFrom = $("#SelectedDateFrom").val();
        this.ViewModel.SelectedDateTo = $("#SelectedDateTo").val();
        this.ViewModel.Zoom = $("#Zoom").val();
    }
    CacheTemplatesAndRender() {
        let obj = this;
        let t = 0;

        if (window.template1Cache == null || window.template2Cache == null || window.template3Cache == null) {
            if (window.template1Cache == null) {
                //fetch('/Areas/ONEPROD/Views/APS/GanttChartItem.Template.ClientOrder.cshtml')
                fetch('/Areas/ONEPROD/Views/APS/GanttChartItem.Template.cshtml')
                    .then((response) => response.text())
                    .then((template) => {
                        window.template1Cache = template;
                        t++;
                        if (t >= 3) {
                            obj.Render();
                        }
                    });
            }
            if (window.template2Cache == null) {
                fetch('/Areas/ONEPROD/Views/APS/GanttChartItem.Template.cshtml')
                    .then((response) => response.text())
                    .then((template) => {
                        window.template2Cache = template;
                        t++;
                        if (t >= 3) {
                            obj.Render();
                        }
                    });
            }
            if (window.template3Cache == null) {
                fetch('/Areas/ONEPROD/Views/APS/GanttChartItem.Template.ChangeOver.cshtml')
                    .then((response) => response.text())
                    .then((template) => {
                        window.template3Cache = template;
                        t++;
                        if (t >= 3) {
                            obj.Render();
                        }
                    });
            }
        }
        else {
            obj.Render();
        }
    }
    Render() {
        let targetElement = this;
        let vm = this.ViewModel;
        if (vm != null) {
            let templateName = "GanttChart.Template.cshtml";
            let duration = moment.duration(new moment(vm.SelectedDateTo).diff(new moment(vm.SelectedDateFrom)));
            let seconds = duration.asSeconds();
            vm.SecToPixelRatio = parseFloat(parseFloat(this.ViewModel.Zoom) / 60);
            vm.width = parseInt((seconds * vm.SecToPixelRatio) + 120 * 48);
            vm.isData = vm.data.ResourceGroups != null ? true : false;
            vm.CurrentTimeRedLinePosition = moment.duration(new moment(new Date()).diff(new moment(vm.SelectedDateFrom))).asSeconds() * vm.SecToPixelRatio;
            //window.StartTime = new Date();

            fetch('/Areas/ONEPROD/Views/APS/' + templateName)
                .then((response) => response.text())
                .then((template) => {
                    var rendered = Mustache.render(template, vm);
                    $(targetElement).html(rendered);
                    $(".sgcWorkordersCol").hScroll(100);
                });
        }
    }
    ShowConnected($el) {
        if ($el.hasClass("sgcBlockSelected")) {
            this.DeselectElements();
        }
        else {
            this.DeselectElements();
            this.SelectElements($el.find(".ganttchartItem").attr("orderNo"));
        }
    }
    ShowBatch($el) {
        this.DeselectElements();
        this.SelectElements($el.attr("orderNo"));
    }
    SelectElements(orderNo) {
        console.log("SelectElements");
        $(this).find(".sgcBlockWorkorder").addClass('grayout');
        $(this).find("[orderNo='" + orderNo + "']").parent().removeClass('grayout');
        $(this).find("[orderNo='" + orderNo + "']").parent().addClass('sgcBlockSelected');
        $(this).find("[orderNo='" + orderNo + "'] .workorderGrnd").removeClass('sgcBlockWorkorderGradient');
    }
    DeselectElements() {
        $(this).find(".sgcBlockWorkorder").removeClass('grayout');
        $(this).find(".sgcBlockSelected .workorderGrnd ").addClass('sgcBlockWorkorderGradient');
        $(this).find(".sgcBlock.sgcBlockSelected").removeClass('sgcBlockSelected');
    }
    DisplayDeteils(data) {
        if (data != null) {
            var dataElements = data.split("%");
            var descr = "";
            var value = "";

            for (var i = 0; i < dataElements.length; i++) {
                descr = dataElements[i].split("$")[0].replace(" ", "");
                value = dataElements[i].split("$")[1];
                $("#sbc" + descr).text(value);
            }
        }
    }

    Actions(obj) {
        $(obj).off("click", ".sgcBlockWorkorder, .sgcBlockBatchWorkorder");
        $(obj).on("click", ".sgcBlockWorkorder, .sgcBlockBatchWorkorder", function () {
            obj.ShowConnected($(this));
            obj.DisplayDeteils($(this).find(".ganttchartItem").attr('data-content'));
        });
    }
}

customElements.define('oneprod-aps-ganttchart', GanttChart);
