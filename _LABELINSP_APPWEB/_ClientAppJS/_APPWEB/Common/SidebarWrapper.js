function SidebarWrapper() {

    this.Init = function () {
        $("#sidebar-wrapper-right2").prepend(
            '<div id="menu-right-toggle" style="width:65px; height:20px;">' +
            '<span class="fas fa-arrows-alt-h" style="font-size: 20px; color: green;"></span></div>'
        );

        $("#menu-right-toggle").click(function (e) {
            ExpandRightMenu(e);
        });
    }

    function ExpandRightMenu(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("active-right");
        RenderUpToSize();
    }

}