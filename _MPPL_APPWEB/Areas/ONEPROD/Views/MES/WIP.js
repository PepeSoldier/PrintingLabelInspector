function WIP(_resourceGroupId)
{
    var self = this;
    let resourceGroupId = _resourceGroupId;
    let viewModel = {
        ItemGroups: [
            { Id: 1, Name: "TUB45", Qty: "541" },
            { Id: 2, Name: "TUB60 14 PS", Qty: "780" },
            { Id: 3, Name: "TUB60", Qty: "101" }
        ],
        Items: [
            { Id: 1, Code: "A11919517", Name: "TUB 2/2/3,5 ADO 60 14PS", Qty: 0, ItemGroupId: 2, CoveredSeconds: 9000 },
            { Id: 2, Code: "A11919519", Name: "TUB 2/2/3,5 ADO 60 NOH", Qty: 0, ItemGroupId: 2, CoveredSeconds: 9000 },
            { Id: 3, Code: "A11919518", Name: "TUB 3,5/3,5/4 ADO 60 14PS", Qty: 0, ItemGroupId: 2, CoveredSeconds: 9000 },
            { Id: 4, Code: "807044149", Name: "TUB 2/2/3,5 ADO 45", Qty: 0, ItemGroupId: 1, CoveredSeconds: 9000 }
        ]
    };
    
    this.Init = function () {
        console.log("WIP.Init. ResourceGroupId=" + resourceGroupId);
        Actions();    
        GetData();
    };
    
    function Actions() {
        $(document).off("click", ".aa");
        $(document).on("click", ".aa ", function () {

        });
    }
    function GetData() {
        let json = new JsonHelper().GetPostData("/ONEPROD/MES/WIPGetData",
            {
                resourceGroupId
            });
        json.done(function (data) {
            console.log(data);
            let divider = 30;
            let currentTime = new moment();
            let timerVM = { Hours: [] };
            let minutes = 60 - (currentTime.minute() % 60);

            data.workordersVM.forEach(function (wo) {
                wo.width = parseFloat(wo.ProcessingTime / divider);
            });

            for (let i = 0; i <= 16; i++) {
                timerVM.Hours.push({
                    width: minutes * 60 / divider,
                    hour: currentTime.format('HH:mm')
                });
                currentTime = currentTime.add(minutes, 'minutes');
                minutes = 60;
            }

            RenderWipGanttTimer(timerVM);
            CalculateCoverage(data);
            RenderWippGanttResources(data);

            CopyDataToViewModel(data);
            RefreshTrolleys();
            RenderWipItems();
            RenderWipItemsGroup();
        });
    }

    function RefreshTrolleys() {
        let trolleysUsed = 86;
        let trolleysAvailable = 100;

        let val = parseInt((20 * trolleysUsed / 100).toFixed(0));

        let elements = $(".chartBuff").filter(function () {
            return parseInt($(this).attr('data-val')) <= val;
        });

        $(elements).addClass("used");
    }
    function CalculateCoverage(data) {
        data.items.forEach(function (item) {
            let availableQty = item.Qty;
            let coveredQty = 0;
            i = 0;

            while (availableQty > 0 && i < data.workordersVM.length)
            {
                if (data.workordersVM[i].Qty_Covered == null) {
                    data.workordersVM[i].Qty_Covered = 0;
                }

                if (data.workordersVM[i].Param1 == item.Id)
                {
                    coveredQty = Math.min(availableQty, data.workordersVM[i].Qty_Remain);
                    data.workordersVM[i].Qty_Covered = coveredQty;
                    data.workordersVM[i].Width_Covered = parseInt(100 * coveredQty / data.workordersVM[i].Qty_Remain);
                    availableQty -= coveredQty;
                }
                i++;
            }
        });
    }
    function CopyDataToViewModel(data) {
        viewModel.ItemGroups = data.itemGroups;
        viewModel.Items = data.items;
    }

    function RenderWipGanttTimer(vm) {
        RenderTemplate("#WipGanttTimerTemplate", "#gantt22", vm, true);
    }
    function RenderWippGanttResources(data) {
        data.resourcesVM.forEach(function (resource) {
            woList = data.workordersVM.filter(x => x.ResourceId == resource.Id && x.Qty_Remain > 0);
            resource.Workorders = woList;
            RenderWipGantt(resource);
        });
    }
    function RenderWipGantt(vm) {
        RenderTemplate("#WipGanttTemplate", "#gantt22", vm, true);
    }
    function RenderWipItems() {
        let itemsOfGroup = {
            Items: []};

        for (let i = 0; i < viewModel.ItemGroups.length; i++) {
            itemsOfGroup.Items = viewModel.Items.filter(x => x.ItemGroupId == viewModel.ItemGroups[i].Id);
            RenderTemplate("#WipItemTemplate", "#WipItems", itemsOfGroup, true);
        }
    }
    function RenderWipItemsGroup() {
        RenderTemplate("#WipItemGroupTemplate", "#WipItemGroups", viewModel);
    }
    
}