
class GanttChartResourceRow extends HTMLElement {
    Filters = {
        resourceId: 0,
        selectedDateFrom: null,
        selectedDateTo: null,
        zoom: 1
    }
    ViewModel = {
        Id: "ResourceId",
        Name: "ResourceName",
        ResourceGroupId: 1,
        ResourceGroupName: "ResourceGroupName",
        ResourceGroupSafetyTime: 0,
        ShowBatches: false,
        Batches: [],
        Workorders: [],
        //wyliczane;
        width: "0",
        selectedDateFrom: null,
        zoom: 1
    }
    SecToPixelRatio = 1;

    constructor() {
        super();
        this.Refresh = this.Refresh.bind(this);
        this.Actions(this);
    }
    static get observedAttributes() {
        return ['data-resourceid'];
    }
    attributeChangedCallback(name, oldValue, newValue) {
        this.Filters.resourceId = newValue;
    }
    connectedCallback() {
        this.Render();
        this.Refresh();
    }
    disconnectedCallback() {
    }
    adoptedCallback() { }

    Refresh() {
        let obj = this;
        this.Filters.selectedDateFrom = $("#SelectedDateFrom").val();
        this.Filters.selectedDateTo = $("#SelectedDateTo").val();
        this.Filters.zoom = $("#Zoom").val();
        this.SecToPixelRatio = parseFloat(parseFloat(this.Filters.zoom) / 60);

        let js = new JsonHelper().GetPostData("/ONEPROD/APS/GanttChartResource", this.Filters);
        js.done(function (ResourceViewModel) {
            obj.ViewModel = ResourceViewModel;
            obj.ViewModel.selectedDateFrom = obj.Filters.selectedDateFrom;
            obj.ViewModel.zoom = obj.Filters.zoom;
            obj.Render();

            if (obj.ViewModel.Batches.length <= 0 && obj.ViewModel.Workorders.length <= 0) {
                console.log("no data for resource: " + obj.Filters.resourceId);
                $("[data-resourceId=" + obj.Filters.resourceId + "]").addClass("hidden");
            }
        });
    }
    Render() {
        let vm = this.ViewModel;
        if (vm != null) {
            let duration = moment.duration(new moment(this.Filters.selectedDateTo).diff(new moment(this.Filters.selectedDateFrom)));
            let seconds = duration.asSeconds();
            vm.width = parseInt((seconds * this.SecToPixelRatio) + 120 * 48);

            //this.classList.add("sgcRowSnappable");
            //this.classList.add("resourceArea_" + this.ViewModel.ResourceGroupId);

            let targetElement = this;
            fetch('/Areas/ONEPROD/Views/APS/GanttChartResourceRow.Template.cshtml')
                .then((response) => response.text())
                .then((template) => {
                    $(targetElement).html(Mustache.render(template, vm));
                });
        }
    }

    Actions(obj) {
        $(obj).off("click", "");
        $(obj).on("click", "", function () {

        });
    }
}


customElements.define('oneprod-aps-ganttchart-resourcerow', GanttChartResourceRow);
