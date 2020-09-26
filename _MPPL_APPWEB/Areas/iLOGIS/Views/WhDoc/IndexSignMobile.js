function WhDocIndexSignMobile() {
    var self = this;

    var viewModel = {
    };

    this.Init = function () {
        console.log("WhDocIndexSignMobile.Init");
        Render();
        Actions();
    };

    function Render() {

    }
    function Actions() {
        $(document).off("click", "#btn__");
        $(document).on("click", "#btn__", function (event) {
            
        });
    }
} 
