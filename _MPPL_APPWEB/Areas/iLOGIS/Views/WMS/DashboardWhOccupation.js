function DashboardWhOccupation() {
    var pagingItems = 8;
    var whOccupationListView = [];
    var viewModel = {
        data: null,
        currentPage : 0,
    };

    this.Init = function () {
        setInterval(function () { $("#carouselBtnRight").click() }, 3000);
        LoadWarehouseOccupationSection();
        Actions();
    };

    function LoadWarehouseOccupationSection() {
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/WMS/DashboardWarehouseOccupation");
        ReturnJson.done(function (whOccupationList) {
            whOccupationList = InitializeData();
            whOccupationListView = whOccupationList;
            viewModel.data = whOccupationListView.slice(0, pagingItems),
            RenderTemplate("#warehouseOccupationTemplate", "#warehouseOccupationView", viewModel);
            RenderTemplate("#warehouseOccupationDataTemplate", "#warehouseOccupationData", viewModel);
        });
        ReturnJson.fail(function () {
            new Alert().Show("success", "Problem z połączeniem z bazą danych");
        });
    }

    

    function InitializeData() {
        let whCount = 37;
        ManagePaging(whCount, pagingItems);
        return GenerateWhFakeData(whCount);
    }

    function GenerateWhFakeData(whCount) {
        let whOcList = [];
        for (let i = 0; i < whCount; i++) {
            let whOcc = parseInt((Math.random() * 100));
            let zm = {
                WarehouseName: "WH_" + i,
                WarehouseId: i,
                NumberOfWarehauseLocations: parseInt((Math.random() * 100)),
                WarehouseOccupation: whOcc,
                barType: GetBarType(whOcc)
            };
            whOcList.push(zm);
        }
        return whOcList;
    }

    function GetBarType(progressValue) {
        if (progressValue > 90) {
            return "bg-danger";
        } else if (progressValue > 60 && progressValue <= 90) {
            return "bg-warning";
        } else {
            return "bg-success";
        }
    }

    function ManagePaging(whCount, pagingItems) {
        let isPageFull = whCount / pagingItems;
        let pagingNumber = 0;

        if (isPageFull > 1) {
            pagingNumber = isPageFull > parseInt(whCount / pagingItems) ? parseInt(isPageFull) + 1 : parseInt(isPageFull);
            viewModel.Paging = true;
            viewModel.PagingList = CreatePagingList(pagingNumber);
        } else {
            viewModel.Paging = false;
        }
    }

    function CreatePagingList(pagingNumber) {
        let pagingList = [];
        let flague = 0;
        for (let k = 0; k < pagingNumber; k++) {
            if (flague == 0) {
                pagingList.push({ PageId: k, PageIcon: "fas" });
                flague += 1;
            } else {
                pagingList.push({ PageId: k, PageIcon: "far" });
            }
        }
        return pagingList;
    }

    function RenderWarehouseData() {
        let startItem = viewModel.currentPage * pagingItems;
        viewModel.data = whOccupationListView.slice(startItem, startItem + pagingItems);
        RenderTemplate("#warehouseOccupationDataTemplate", "#warehouseOccupationData", viewModel);
    }

    function ManageChevronClick(currChevronId) {
        //let currChevronId = $(currentTarget).parent().attr("Id");
        SetCurrentPageForChevrons(currChevronId);
        $.each($.find(".circleIcon"), function () {
            let circleId = $(this).parent().attr("Id");
            if (circleId == viewModel.currentPage) {
                $(this).removeClass("far").addClass("fas");
            } else {
                $(this).removeClass("fas").addClass("far");
            }
        });
    }

    function ManageCircleClick(currentTarget) {
        $.each($.find(".circleIcon"), function () {
            $(this).removeClass("fas").addClass("far");
        });
        $(currentTarget).removeClass("far").addClass("fas");
        viewModel.currentPage = $(currentTarget).parent().attr("Id");
    }

    function SetCurrentPageForChevrons(currChevronId) {
        if (currChevronId == "leftArrow") {
            if (viewModel.currentPage > 0) {
                viewModel.currentPage = parseInt(viewModel.currentPage) - 1;
            }
        } else {
            console.log(viewModel.PagingList.length);
            if (viewModel.currentPage < (viewModel.PagingList.length - 1)) {
                viewModel.currentPage = parseInt(viewModel.currentPage) + 1;
            } else {
                viewModel.currentPage = 0;
            }

        }
    }

    function Actions() {
        $(document).off("click", ".circleIcon");
        $(document).on("click", ".circleIcon", function (event) {
            ManageCircleClick(event.currentTarget);
            RenderWarehouseData();
        });

        $(document).off("click", ".chevrons");
        $(document).on("click", ".chevrons", function (event) {
            ManageChevronClick($(event.currentTarget).parent().attr("Id"));
            RenderWarehouseData();
        });

        $(document).off("click", ".whSelection");
        $(document).on("click", ".whSelection", function (event) {
            var JsonHelp = new JsonHelper();
            let ReturnJson = JsonHelp.GetPostData("/iLogis/WMS/DashboardWarehouseOccupation");
            ReturnJson.done(function (whOccupationList) {
                whOccupationList = InitializeData();
                whOccupationListView = whOccupationList;
                viewModel.ParenWarehouse = true;
                viewModel.data = whOccupationListView.slice(0, pagingItems),
                RenderTemplate("#warehouseOccupationTemplate", "#warehouseOccupationView", viewModel);
                RenderWarehouseData();
            });
            ReturnJson.fail(function () {
                new Alert().Show("success", "Problem z połączeniem z bazą danych");
            });
        });

    };


};