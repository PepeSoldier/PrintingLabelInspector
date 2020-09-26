function LeftMenu() {

    this.Init = function () {

        $("#main_icon").addClass("fa-angle-double-right");

        $("#menu-toggle").click(function (e) {
            ExpandLeftMenu(e);
        });
        
        $(".menuLi").on("mouseover", function () {
            $(this).find(".subMenu").removeClass("hidden");
        });
        $(".menuLi").on("mouseout", function () {
            $(this).find(".subMenu").addClass("hidden");
        });
    }

    function ExpandLeftMenu(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("active");

        if ($("#wrapper").hasClass("active")) {
            $("#main_icon").addClass("fa-angle-double-left");
            $("#main_icon").removeClass("fa-angle-double-right");
            $(".menuItem").fadeIn(300);

        }
        else {
            $("#main_icon").addClass("fa-angle-double-right");
            $("#main_icon").removeClass("fa-angle-double-left");
            $(".menuItem").fadeOut(100);
        }
    }
    function RenderUpToSize() {
        var width = $("#sidebar-wrapper-right2").width();
        console.log(width);

        if (width > 100) {
            console.log("ukryj");
            $(".sidebarOpened, #sidebar-wrapper-right2 .caption").addClass("hidden");
            $(".sidebarClosed").removeClass("hidden");
        }
        else {
            console.log("pokaz");
            $(".sidebarOpened, #sidebar-wrapper-right2 .caption").removeClass("hidden");
            $(".sidebarClosed").addClass("hidden");
        }
    }
}