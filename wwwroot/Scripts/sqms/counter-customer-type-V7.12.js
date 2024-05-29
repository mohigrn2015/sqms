$(document).ready(function () {
    inp = $("#branch_id option:selected").text();
    if (inp == "Select Branch Name") {
        $("#branch_id").attr('disabled', false);
    }
    else {
        $("#counter_id").empty();
        getCountersByBrunchId($("#branch_id").val());
        $("#branch_id").attr('disabled', true);
    }

    //Dropdownlist Selectedchange event
    $("#branch_id").change(function () {

        var brunchId = $("#branch_id").val();
        $("#counter_id").empty();
        getCountersByBrunchId(brunchId);
        return false;
    })
});



function getCountersByBrunchId(brunchId) {
    $("#counter_id").empty();
    $.ajax({
        type: 'GET',
        url: basedURL + "../Counters/GetCounterByBrunchId?id=" + brunchId,
        //dataType: 'json',
        //data: { id: brunchId },
        success: function (data) {
            if (data.success == "true") {
                $.each(data.counters, function (i, counter) {
                    $("#counter_id").append('<option value="' + counter.counter_id + '">' +
                        counter.counter_no + '</option>');
                });
            }
            else {
                modalAlert(data.message);
            }
        },
        error: function (ex) {
            modalAlert('Failed to retrieve counters.' + ex);
        }
    });
}