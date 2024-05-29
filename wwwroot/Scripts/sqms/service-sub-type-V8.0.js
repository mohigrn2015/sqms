
function SetStatus(service_sub_type_id, is_activate) {
    $.ajax({
        url: '../ServiceSubTypes/SetStatus',
        type: 'POST',
        dataType: 'json',
        data: { service_sub_type_id: service_sub_type_id, is_activate: is_activate },
        success: function (data) {
            if (data.success == false) {

                alert(data.Message);
            }
            else {

                var viewURL = '../ServiceSubTypes/index';
                window.location = viewURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function TatTime() {
    $("#SetTatTime").hide();
    $(".tatCheckBoxFunc").show();
    $(".tatCheckBoxFuncAll").show();

    serviceSubTypeIds = "";
}

function CancelTat() {
    $(".tatCheckBoxFunc").hide();
    $(".tatCheckBoxFuncAll").hide();
    $("#SetTatTime").show();
    $(".service_sub_type_check_box:checked").each(function () {
        $(this).prop('checked', false);
    });
    $("#checkAll").prop('checked', false);
    serviceSubTypeIds = "";
    $("#tatTime").val("");
}

function CheckAll() {
    $(!".service_sub_type_check_box:checked").each(function () {
        $(this).prop('checked', true);
    });
}
$("#checkAll").click(function () {
    $('input:checkbox').not(this).prop('checked', this.checked);
});

var serviceSubTypeIds = "";
function Timer() {
    $(".service_sub_type_check_box:checked").each(function () {
        serviceSubTypeIds += $(this).attr("data") + ",";
    });
    serviceSubTypeIds = serviceSubTypeIds.substring(0, serviceSubTypeIds.length - 1);
    if (serviceSubTypeIds != "") {
        $("#mdlTatTimeSet").modal({
            backdrop: 'static',
            keyboard: false
        });
    } else {
        modalAlert('Please Select Atleast 1 Service');
    }
}

function SaveTatTime() {
    var tatTime = $("#tatTime").val();
    if (tatTime == "") {
        modalAlert('Please Enter TAT Warning Time');        
    } else if (tatTime > 5) {
        modalAlert('Please enter a value less or equal to 5 minutes');
    }
    else {
        $.ajax({
            url: "../ServiceSubTypes/EditServiceTatTimeBulk",
            type: 'POST',
            dataType: 'json',
            data: { service_id: serviceSubTypeIds, time: tatTime },
            success: function (data) {
                debugger;
                if (data.success == true) {
                    modalAlert('Service warning time updated.');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }
        });
        $("#mdlTatTimeSet").modal('hide');
        CancelTat();
    }
}

function warningTimeValidation() {
    var warningTime = $('#tat_warning_time').val();
    var max_duration = $('#max_duration').val();
    if (parseInt(max_duration) > parseInt(warningTime)) {
        return true;
    } else {
        modalAlert("Warning time can not be greater than service max duration.")
        return false;
    }
}
