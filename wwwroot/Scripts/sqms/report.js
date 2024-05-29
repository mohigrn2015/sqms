$(document).ready(function () {
    

    InitiateReportBox();
    $('#branch_id option:eq(0)').val(0);

    $('#txtCounter').append($("<option value='0'>All Counters</option>"));
    //$('#txtCounter').attr('disabled', disabled);
    $('#txtUser').append($("<option value=''>All Users</option>"));

    isOnlyServiceHolder = true;
    if ($("#is_success").length) isOnlyServiceHolder = false;

    $("#branch_id").change(function () {
        // debugger;
        LoadCountersUsers(isOnlyServiceHolder);

    });



    

    //inp = $("#branch_id option:selected").text();
    //if ($("#branch_id option:selected").val() > 0) {
    //    LoadCountersUsers(isOnlyServiceHolder);
    //    $("#branch_id").attr('disabled', true);
    //}
    //else $("#branch_id").attr('disabled', false);
    
});


function LoadCountersUsers(isOnlyServiceHolder) {
    var selectedBranch = $("#branch_id option:selected").text();
    var selectedVal = $("#branch_id option:selected").val();

    $('#txtUser').empty();
    $('#txtCounter').empty();
    if (selectedVal > 0) {
        $.ajax({
            url: "../Home/GetUserAndCounterByBranchId",
            type: "GET",
            dataType: "json",
            data: { branchId: selectedVal, isOnlyServiceHolder: isOnlyServiceHolder },
            success: function (data) {
                if (data.userList.length > 0) {
                    $('#txtUser').append($("<option value=''>All Users</option>"));

                    for (var i = 0; i < data.userList.length; i++) {
                        $('#txtUser').append($("<option></option>").attr("value", data.userList[i].user_id).text(data.userList[i].user_name));
                    }
                } else {
                    $('#txtUser').append($("<option value=''>All Users</option>"));
                }
                if (data.counterList.length > 0) {
                    $('#txtCounter').append($("<option value='0' selected>All Counters</option>"));

                    for (var i = 0; i < data.counterList.length; i++) {
                        $('#txtCounter').append($("<option></option>").attr("value", data.counterList[i].counter_id).text(data.counterList[i].counter_no));
                    }
                } else {
                    $('#txtCounter').append($("<option value='0' selected>All Counters</option>"));
                }
            },
            error: function (response) {
                alert(response);
            }
        });
    }
    else {
        $('#txtUser').append($("<option value=''>All Users</option>"));
        $('#txtCounter').append($("<option value='0' selected>All Counters</option>"));
    }
}


function ShowPannel(pannel_index) {
    if (pannel_index == 1) {
        $("#collapse1").collapse('show');
        $("#collapse2").collapse('hide');
    }
    else {
        $("#collapse1").collapse('hide');
        $("#collapse2").collapse('show');
    }
}

function GenerateLocalCustomerList() {

    $('#tablebody').empty();

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    var customer_type_id = $('#customer_type_id').val();
    //var token = $('#txtToken').val();
    var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

   
    $.ajax({
        url: "../Report/GetLocalCustomerInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            customer_type_id: customer_type_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            //debugger;

            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].branch_name + '</td><td>'
                        + response.Message[i].service_datetime + '</td><td>'
                        + response.Message[i].service_sub_type_name + '</td><td>'
                        + response.Message[i].end_time + '</td><td>'
                        + response.Message[i].contact_no + '</td><td>'
                        + response.Message[i].customer_name + '</td><td>'
                        + response.Message[i].im_msisdn + '</td><td>'
                        + response.Message[i].im_name + '</td><td>'
                        + response.Message[i].remarks + '</td><td>'
                        + response.Message[i].further_followUp_needed + '</td><td>'
                        + response.Message[i].FollowUp_date + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });

}

function GenerateVisitedCustomerList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
   
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();
    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetVisitedCustomerInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].branch_name + '</td><td>'
                        + response.Message[i].service_sub_type_name + '</td><td>'
                        + response.Message[i].total_served_token + '</td><td>'
                        + response.Message[i].single_visit_customer + '</td><td>'
                        + response.Message[i].multiple_visit_customer + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateAgentWiseReportList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();
    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetAgentWiseReportInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].branch_name + '</td><td>'
                        + response.Message[i].user_name + '</td><td>'
                        + response.Message[i].handled_customer + '</td><td>'
                        + response.Message[i].average_waiting_time + '</td><td>'
                        + response.Message[i].average_service_time + '</td><td>'
                        + response.Message[i].average_TAT + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateServiceSummaryList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();
    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetServiceSummaryInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            if (response.Success) {
                var select = $('#tablebody');
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].branch_name + '</td><td>'
                        + response.Message[i].service_sub_type_name + '</td><td>'
                        + response.Message[i].token_served + '</td><td>'
                        + response.Message[i].total_percentage + '</td><td>'
                        + response.Message[i].standard_time + '</td><td>'
                        + response.Message[i].actual_time + '</td><td>'
                        + response.Message[i].variance + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateGeneralSearchList() {
    $('#tablebody').empty();
     var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    var msisdn_no = $('#txtMSISDN').val();
    var service_sub_type_id = $('#service_sub_type_id').val();
    var token_no = $('#txtToken').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetGeneralSearchInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            msisdn_no: msisdn_no,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate,
            token_no : token_no
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {

                    select.append($('<tr><td>' + response.Message[i].branch_name + '</td><td>'
                        + response.Message[i].Service_date + '</td><td>'
                        + response.Message[i].user_name + '</td><td>'
                        + response.Message[i].token_no_formated + '</td><td>'
                        + response.Message[i].mobile_no + '</td><td>'
                        + response.Message[i].service_sub_type_name + '</td><td>'
                        + response.Message[i].issue_time + '</td><td>'
                        + response.Message[i].start_time + '</td><td>'
                        + response.Message[i].end_time + '</td><td>'
                        + response.Message[i].wating_time + '</td><td>'
                        + response.Message[i].std_time + '</td><td>'
                        + response.Message[i].actual_time + '</td><td>'
                        + response.Message[i].variance + '</td><td>'
                        + response.Message[i].remarks + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
            
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateBreakReportList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    var customer_type_id = 0;
    //if ($('#service_sub_type_id').val())
    var break_type_id = $('#break_type_id').val();
    //var token_no = $('#txtToken').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetBreakReportInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            break_type_id: break_type_id,
            customer_type_id: customer_type_id,
            start_date: fromDate,
            end_date: toDate,
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].branch_name + '</td><td>'
                        + response.Message[i].create_time + '</td><td style="text-align: left;">'
                        + response.Message[i].username + '</td><td>'
                        + response.Message[i].counter_no + '</td><td>'
                        + response.Message[i].break_type_name + '</td><td>'
                        + response.Message[i].start_time + '</td><td>'
                        + response.Message[i].end_time + '</td><td>'
                        + response.Message[i].duration + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateNServiceList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();

    var topn = $('#txtTopN').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetTopNServiceInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            start_date: fromDate,
            end_date: toDate,
            topn: topn,
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].branch_name + '</td><td style="text-align: left;">'
                        + response.Message[i].service_name + '</td><td>'
                        + response.Message[i].total_service + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateTokenExceedingList() {
    $('#tablebody').empty();
    var flag = $('#txtFlag').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetTokenExceedingInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate,
            flag : flag
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td style="text-align: left;">' + response.Message[i].branch_name + '</td><td style="text-align: left;">'
                        + response.Message[i].user_name + '</td><td>'
                        + response.Message[i].total_served_token + '</td><td>'
                        + response.Message[i].total_exceedig_token + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}



function GenerateLogoutDetailList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetLogoutDetailInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td style="text-align: left;">' + response.Message[i].branch_name + '</td><td style="text-align: left;">'
                        + response.Message[i].user_name + '</td><td>'
                        + response.Message[i].service_date_formated + '</td><td>'
                        + response.Message[i].counter_no + '</td><td>'
                        + response.Message[i].login_time_formated + '</td><td>'
                        + response.Message[i].logout_time_formated + '</td><td>'
                        + response.Message[i].duration + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}


function GenerateLoginAttemptDetailsList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var is_success = $('#is_success').val();
    var user_id = $('#txtUser').val();


    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetLoginAttemptDetailsInformation",
        type: 'POST',
        dataType: "json",
        data: {
            branch_id: branch_id,
            user_id: user_id,
            counter_id: counter_id,
            is_success: is_success,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td style="text-align: left;">' + response.Message[i].branch_name + '</td><td style="text-align: left;">'
                        + response.Message[i].UserName + '</td><td>'
                        + response.Message[i].FullName + '</td><td>'
                        + response.Message[i].RoleName + '</td><td>'
                        + response.Message[i].attempt_time_formatted + '</td><td>'
                        + response.Message[i].counter_no + '</td><td>'
                        + response.Message[i].ip_address + '</td><td>'
                        + response.Message[i].machine_name + '</td><td>'
                        + response.Message[i].status + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}