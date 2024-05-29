function FilterTable() {
    index = -1;
    searchInp = $('#filterBox').val();

    inp = $("#branch_name option:selected").text();
    if (inp == "All Branch") {
        $("#data:visible tr:not(:has(>th))").each(function () {
            if (~$(this).text().toLowerCase().indexOf(searchInp.toLowerCase())) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    } else {
        $("#data:visible tr:not(:has(>th))").each(function () {
            if (~$(this).text().toLowerCase().indexOf(searchInp.toLowerCase()) && ~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }


    $('#Hedding').show();
}
$(document).ready(function () {
    inp = $("#branch_name option:selected").text();
    if (inp == "All Branch") {
        $("#branch_name").attr('disabled', false);
    }
    else $("#branch_name").attr('disabled', true);

    FilterTable2();
    $("#branch_name").change(function () {
        // var selectedBranch = $("#branch_name option:selected").text();
        FilterTable2();

    });
});

function FilterTable2() {
    index = -1;
    inp = $("#branch_name option:selected").text();
    if (inp == "All Branch") {
        inp = "";
    }
    $("#data:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
};