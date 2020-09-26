


function CircleProgressbar(parentElId,id) {

    var self = this;
    this.Id = id;
    var el = $(Draw());
    var pElId = parentElId;
    var currentProc = 0;
    
    function Draw() {
        return '<div class="progress blue" id=' + self.Id + '>' +
                '<span class="progress-left">' +
                    '<span class="progress-bar"></span>' +
                '</span>' +
                '<span class="progress-right">' +
                    '<span class="progress-bar"></span>' +
                '</span>' +
                '<div class="progress-value">0%</div>' +
                '</div>';
    }

    this.Rotate = function (proc) {
        
        var proc1 = proc > 50 ? 50 : proc;
        var proc2 = proc > 50 ? proc - 50 : 0;
        var deg1 = proc1 * 180 / 50;
        var deg2 = proc2 * 180 / 50;
        var dur1 = proc1 - currentProc;
        var dur2 = proc - (currentProc > 50? currentProc : 50);

        dur1 = dur1 < 0 ? 0 : dur1 * 10;
        dur2 = dur2 < 0 ? 0 : dur2 * 10;

        el.children(".progress-value").text(proc + "%");

        el.children(".progress-right").children(".progress-bar")[0].style.transform = "rotate(" + deg1 + "deg)";
        el.children(".progress-left").children(".progress-bar")[0].style.transform = "rotate(" + deg2 + "deg)";
        
        //var el1 = $(el.children(".progress-right").children(".progress-bar")[0]);
        //var el2 = $(el.children(".progress-left").children(".progress-bar")[0]);

        //el1.animate({ borderSpacing: deg1 }, {
        //    step: function (now,fx) {
        //        $(this).css('-webkit-transform', 'rotate(' + now + 'deg)');
        //        $(this).css('-moz-transform', 'rotate(' + now + 'deg)');
        //        $(this).css('transform', 'rotate(' + now + 'deg)');
        //    },
        //    duration: dur1,
        //    complete: function () {
        //        el2.animate({ borderSpacing: deg2 }, {
        //            step: function (now, fx) {
        //                $(this).css('-webkit-transform', 'rotate(' + now + 'deg)');
        //                $(this).css('-moz-transform', 'rotate(' + now + 'deg)');
        //                $(this).css('transform', 'rotate(' + now + 'deg)');
        //            },
        //            duration: dur2,
        //        }, 'linear');
        //    }
        //}, 'linear');

        currentProc = proc;
    }
    this.Show = function(){
        $(document).find("#" + pElId).append(el);
        return el;
    }

}

function Progressbar(selector, id) {

    var self = this;
    this.Id = id;
    var el = $(Draw());
    var pElId = selector;
    var currentProc = 0;

    function Draw() {
        console.log("pb draw");
        return
        '<div class="progress" id=' + self.Id + '>' +
            '<div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" ' +
                'style="min-width: 2em; width: 0%;">' +
                '0%' +
            '</div>' +
        '</div>';
    }

    this.Update = function (value) {
        currentProc = value;
        console.log("pb update: " + currentProc);
        el.children(".progress-bar").text(currentProc);
        el.children(".progress-bar").attr("aria-valuenow", currentProc);

    }
    this.Show = function () {
        console.log("pb show. el: " + pElId);
        console.log(el);
        $(document).find(pElId).append(el);
        return el;
    }
}