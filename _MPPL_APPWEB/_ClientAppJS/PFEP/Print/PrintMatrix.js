function PrintMatrix() {

    var table = $(".tblMatrix")[0];
    var cellsQty = [];

    this.Start = function () {
        AnalyzeCells();
    }

    function AnalyzeCells() {

        tableCount = $(".tblMatrix").length;

        for (var t = 0; t < tableCount; t++)
        {
            table = $(".tblMatrix")[t];

            for (var i = 1, row; row = table.rows[i]; i++)
            {
                var value = 0;
                var empty = 0;
                for (var j = 0, col; col = row.cells[j]; j++) {
                    $cell = $(table.rows[i].cells[j]);
                    if ($cell.hasClass("defTd")) {

                        value = parseFloat($(table.rows[i].cells[j]).text());

                        if (value > 0) {
                            cellsQty.push(j);
                        }
                        else {
                            SumCells(i);
                            empty++;
                        }
                    }
                }
                SumCells(i);

                if (empty == 0) {
                    $(table.rows[i]).addClass("trFull");
                }
            }
        }
    }

    function SumCells(row) {

        var sum = 0;
        var value = 0;
        var opakCol = $(table.rows[1]).first(".tdPckg").index()-1;
        //console.log("opak col: " + opakCol);
        var opak = parseInt($(table.rows[row].cells[0]).text());
        //console.log("opak qty: " + opak);
        var lbBoxText = "";
        var width = 45;

        if (cellsQty.length > 0) {
            //console.log(cellsQty);
            for (c = 0; c < cellsQty.length; c++) {
                value = parseFloat($(table.rows[row].cells[cellsQty[c]]).text());
                sum += value > 0 ? value : 0;
            }
            //console.log("Suma:" + sum);
            if (cellsQty.length > 1) {
                lbBoxText = parseInt(sum / opak) + " [+" + sum % opak + "szt]";
                width = width * cellsQty.length;
                //console.log("Opakowań:" + lbBoxText);
            }
            else {
                lbBoxText = parseInt(sum / opak) + "[" + sum % opak + "]";
                //console.log("Opakowań:" + lbBoxText);
            }
            $(table.rows[row].cells[cellsQty[0]]).prepend('<div class="lbBox" style="width: ' + (width -1) + 'px">' + lbBoxText + '</div>');
            cellsQty = [];
        }
    }

    function MoveFullRows() {

        var it = $('.trFull');
        var last = $(".tblMatrix").find("tr:last");

        it.remove();
        it.insertAfter(last);
    }
}