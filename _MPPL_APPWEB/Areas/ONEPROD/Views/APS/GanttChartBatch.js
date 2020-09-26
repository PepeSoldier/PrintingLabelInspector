class GanttChartBatch extends HTMLElement
{
    ViewModel = {
        ItemCode: "A",
        //....
        BatchStartDate: null,
        selectedDateFrom: null,
        zoom: 1
    }
    SelectedDateFrom = null;
    Zoom = 1;
    SecToPixelRatio = 1;

    constructor() {
        super();
        this.Actions();
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
        }
        else if (name == 'data-datefrom') {
            this.SelectedDateFrom = newValue;
            this.ViewModel.selectedDateFrom = newValue;
        }
        else if (name == 'data-zoom') {
            this.Zoom = newValue;
            this.SecToPixelRatio = parseFloat(parseFloat(this.Zoom) / 60);
            this.ViewModel.zoom = newValue;
        }
    }
    adoptedCallback() {
        //console.log("adoptedCallback");
    }

    Render(targetElement) {
        let vm = this.ViewModel;
        if (vm != null)
        {
            this.CalcBatchParams(this.SelectedDateFrom, vm, 0);

            this.classList.add("sgcBlockBatch");
            this.classList.add("sgcBlockWorkorderGradient");
            this.classList.add("draggableWorkorder");
            this.classList.add("workorderArea_" + this.ViewModel.ResourceGroupId);

            this.style.left = this.ViewModel.MarginLeft + "px";
            this.style.width = this.ViewModel.Width + "px";
            this.style.backgroundColor = this.ViewModel.BackgroundColor;

            fetch('/Areas/ONEPROD/Views/APS/GanttChartBatch.Template.cshtml')
                .then((response) => response.text())
                .then((template) => {
                    var rendered = Mustache.render(template, vm);
                    $(targetElement).html(rendered);
                });
        }
    }
    CalcBatchParams(SelectedDateFrom, batch, areaSafetyTime) {
        if (new moment(batch.StartTime) > new moment(SelectedDateFrom)) {
            batch.BatchStartDate = batch.StartTime;
            batch.Width = parseFloat(batch.ProcessingTime * this.SecToPixelRatio);
            batch.MarginLeft = parseFloat(moment.duration(new moment(batch.StartTime).diff(new moment(SelectedDateFrom))).asSeconds() * this.SecToPixelRatio);
        }
    }

    Update() {
        console.log("UPDATE BATCH -----------------------------------------");
        this.SelectedDateFrom = new moment(document.querySelector('oneprod-aps-ganttchart').ViewModel.SelectedDateFrom);
        this.Zoom = document.querySelector('oneprod-aps-ganttchart').ViewModel.Zoom;
        this.SecToPixelRatio = parseFloat(parseFloat(this.Zoom) / 60);

        let obj = this;
        let selectedDateFrom_tem = new moment(document.querySelector('oneprod-aps-ganttchart').ViewModel.SelectedDateFrom);
        let batchNumber = this.querySelector(".ganttchartBatch").getAttribute("batchnumber");
        let resourceId = this.closest('oneprod-aps-ganttchart-resourcerow').getAttribute("data-resourceid");
        let secFromDateFrom = parseFloat(this.style.left.replace("px", "")) * 60 / this.Zoom;
        let newStartDate = selectedDateFrom_tem.add(secFromDateFrom, 'seconds');
        
        //console.log("left: " + this.style.left);
        //console.log("width: " + this.style.width);
        //console.log("batchNumber: " + batchNumber);
        //console.log("resourceid: " + resourceId);
        //console.log("zoom:" + this.Zoom);
        //console.log("DateFrom:" + this.SelectedDateFrom.format("YYYY-MM-DD HH:mm"));
        //console.log("newStartTime:" + newStartDate.format("YYYY-MM-DD HH:mm:ss"));

        $.ajax({
            url: "/ONEPROD/APS/UpdateBatch",
            type: 'POST',
            data: '{ batchNumber: ' + batchNumber + ', resourceId: ' + resourceId + ', newStartTime: "' + newStartDate.format("YYYY-MM-DD HH:mm:ss") + '"}',
            contentType: 'application/json; charset=utf-8',
            success: function (WorkorderBatch) {
                console.log("zapis powiódł się");
                obj.ViewModel = WorkorderBatch;
                obj.ViewModel.zoom = obj.Zoom;
                obj.Render(obj);
                obj.ViewModel = null;
                new Alert().Show("success", "Nowa pozycja partii produkcyjnej została zapisana");
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

customElements.define('oneprod-aps-ganttchart-batch', GanttChartBatch);
