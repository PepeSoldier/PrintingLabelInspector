function Window1(width, height) {

    var width1 = width;
    var height1 = height;
    var style = setStyle();

    function setStyle() {

        if (height1 <= 0 || typeof height1 === 'undefined') height1 = 300;
        if (width1 <= 0 || typeof width1 === 'undefined') width1 = 600;

        return 'style="width: ' + width1 + 'px; min-height: ' + height1 + 'px;"';
    }
    
    this.show = function (title, htmlContent) {
        var window =
            '<div class="WindowJs" ' + style + '>' +
            '<div class="WindowTitle"><span>' + title + '</span><div class="btn btn-danger btnClose">X</div></div>' +
            '<div class="WindowContent">' +
            '<div class="WindowContentAdj h-100 d-flex flex-column" style="position: relative;">' +
                htmlContent +
            '</div>' +
            '</div>' +
            '<div style="clear: both;"></div>' +
            '</div>';

        return window;
    };
    
}