class GanttChartTimer extends HTMLElement {
    ViewModel = {
        SelectedDateFrom: "",
        SelectedDateTo: "",
        SecToPixelRatio: "",
        Zoom: 1, //[pixels/minute]
        //wyliczane
        TimeBlocks: []
    }

    constructor() {
        super();
        this.Actions();
    }
    connectedCallback() {
    }
    disconnectedCallback() {
    }

    static get observedAttributes() {
        return ['data'];
    }
    attributeChangedCallback(name, oldValue, newValue) {
        this.ViewModel = JSON.parse(newValue);
        this.Render(this);
        this.removeAttribute('data');
    }
    adoptedCallback() {
    }

    Render(targetElement) {
        let vm = this.ViewModel;
        if (vm != null)
        {
            this.PrepareViewModel(vm);
            
            fetch('/Areas/ONEPROD/Views/APS/GanttChartTimer.Template.cshtml')
                .then((response) => response.text())
                .then((template) => {
                    var rendered = Mustache.render(template, vm);
                    $(targetElement).html(rendered);
                });
        }
    }
    PrepareViewModel(vm) {
        vm.SecToPixelRatio = parseFloat(parseFloat(vm.Zoom) / 60);
        let duration = moment.duration(new moment(vm.SelectedDateTo).diff(new moment(vm.SelectedDateFrom)));
        let totalMinutes = duration.asMinutes();
        let TimerClass = ["scgBlockTimerLight", "scgBlockTimerDark"];
        let scalePx = [1, 2, 4, 6, 8, 12, 20, 24, 30, 40, 60, 120];
        let scaleMin = [120, 60, 30, 20, 15, 10, 6, 5, 4, 3, 2, 1];
        let minutesPerInterval = 1;

        for (let k = 0; k < scalePx.length; k++) {
            if (scalePx[k] > vm.Zoom) {
                if (k > 0) {
                    minutesPerInterval = scaleMin[k - 1];
                }
                break;
            }
        }

        this.ViewModel.TimeBlocks = [];

        let i = 0;
        var currentMinute = 0;
        while (currentMinute < totalMinutes) {
            vm.TimeBlocks[i] = {};
            vm.TimeBlocks[i].CssClass = TimerClass[i % 2];
            vm.TimeBlocks[i].Width = parseInt(vm.SecToPixelRatio * 60 * minutesPerInterval);
            vm.TimeBlocks[i].TimeString = new moment(vm.SelectedDateFrom).add(currentMinute, 'minutes').format('MM-DD HH:mm');
            currentMinute += minutesPerInterval;
            i++;
        }

        let totalSeconds = duration.asSeconds();
        vm.width = parseInt((totalSeconds * vm.SecToPixelRatio) + 120 * 48);
    }

    Actions() {
    }
}

customElements.define('oneprod-aps-ganttchart-timer', GanttChartTimer);

function FormatDotNetDate(d) {
    return new moment(d).format("YYYY-MM-DD HH:mm:ss");
}