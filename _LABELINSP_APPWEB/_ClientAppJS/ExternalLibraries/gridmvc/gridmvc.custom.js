
$(document).on('click', '.edit-btn, .cancel-btn', function () {
    var tr = $(this).parents('tr:first');
    //tr.find('.edit-mode, .display-mode').toggle();

    var editMode = tr.find('.display-mode').css("display") == "none";

    if (!editMode) {
        tr.find('.display-mode').css("display", "none");
        tr.find('.edit-mode').css("display", "inline-block");
    }
    else {
        tr.find('.display-mode').css("display", "inline-block");
        tr.find('.edit-mode').css("display", "none");
    }
});
$(document).on('click', '.save-btn', function () {
    var Id = $(this).attr("data-id");
    var url1 = $(this).attr("data-href");
    var params = "";

    var tr = $(this).parents('tr:first');
    var columns = tr.find("td").length;

    var attr1 = "";
    var input;
    var label;
    var inputTxt = "";
    var inputValue = "";
    var td = tr.children('td:first');

    params += 'Id:"' + Id + '"';
    var i = 0;
    while (i <= columns) {
        label = td.find("label, input.display-mode");
        input = td.find("input.edit-mode, select.edit-mode");

        attr1 = input.attr("id");

        if (!input.is("select")) {
            if (input.is(':checkbox')) {
                inputTxt = input.prop('checked');
                inputValue = input.prop('checked');
            }
            else {
                inputTxt = input.val();
                inputValue = input.val();
            }
        }
        else {
            inputTxt = input.find(":selected").text();
            inputValue = input.val();
        }

        if (typeof attr1 !== "undefined") {
            if (label.is(':checkbox'))
                label.prop('checked', inputValue);
            else
                label.html(inputTxt);

            params += ', ' + attr1 + ':"' + inputValue + '"';
        }

        td = td.next("td");
        i++;
    }

    $.ajax({
        url: url1,
        data: '{' + params + '}',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            tr.find('.edit-mode, .display-mode').toggle();
        }
    });
});
$(document).on('click', '.delete-btn', function () {
    var url1 = $(this).attr("data-href");
    var tr = $(this).parents('tr:first');

    $.ajax({
        url: url1,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function () {
            $(tr).remove();
        }
    });
});