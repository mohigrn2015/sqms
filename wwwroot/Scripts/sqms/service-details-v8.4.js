
function FilterTable() {
    index = -1;
    inp = $('#filterBox').val();
    $("#data-skipped:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
}



function FilterTable2() {
    index = -1;
    inp = $("#branch_name option:selected").text();
    if (inp == "All Branch") {
        inp = "";
    }
    $("#data-skipped:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
};

function SelectBranch() {
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
}

var hourslabel = document.getElementById("txtServiceTimerHour");
var minutesLabel = document.getElementById("txtServiceTimerMinute");
var secondsLabel = document.getElementById("txtServiceTimerSecond");
let timerId = 0;
var totalSeconds = 0;
//timerId = setInterval(setTimer, 1000);

function setTimer() {
    ++totalSeconds;
    //var timerSeconds = pad(totalSeconds % 60);
    //var timerMinutes = pad(parseInt(totalSeconds / 60));
    //var timerHour = pad(parseInt(totalSeconds / 3600));
    secondsLabel.innerHTML = pad(totalSeconds % 60);
    minutesLabel.innerHTML = pad(parseInt(totalSeconds / 60));
    hourslabel.innerHTML = pad(parseInt(totalSeconds / 3600));

    if (parseInt(totalSeconds / 60) == warningTime && parseInt(totalSeconds / 3600) == "00" && parseInt(totalSeconds % 60) == "00") {
        warningToast(tat_time, tat_visibility_time);
    };
}

function stopTimer() {
    if (timerId != 0) {
        totalSeconds = 0;
        clearTimeout(timerId);
        secondsLabel.innerHTML = "00";
        minutesLabel.innerHTML = "00";
        hourslabel.innerHTML = "00";
    }

    totalSeconds = 0;
}

function pad(val) {
    var valString = val + "";
    if (valString.length < 2) {
        return "0" + valString;
    } else {
        return valString;
    }
}
///////////// Timer End ////////////////////

///////////// Warning Start //////////////////
var warningTime = 0;
var service_max = 0;
var tat_time = 0
var tat_visibility_time = 0;

function setWarningTime(max, tat) {
    service_max = max;
    tat_time = tat;
    warningTime = max - tat;
}

function setTAT_visibility_time(t) {
    tat_visibility_time = t * 1000;//In milisecond
}

function warningToast(min, period) {
    Toastify({
        text: "You have only " + min + " minutes yet to complete the service.",
        close: true,
        duration: period,
        style: {
            background: "linear-gradient(to right, #FF0000, #FF0000)",
        }
    }).showToast();
}
//////////// Warning End ///////////////////

//var user_id;
var notificationDataTable;
var isNotificationNotSeen = false;
$(document).ready(function () {
    $("#dialog-notification-list").dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        height: 500,
        width: 700,
        closeOnEscape: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#txtContact").keypress(function (e) {
        var charCode = (e.which) ? e.which : e.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
    });

    $("#txtContact").keyup(function () {
        GetDeviceLoanEligibilityInfo();
    });

    $("#txtContact").blur(function () {
        let msisdn = $.trim($("#txtContact").val());

        if (msisdn == "") {
            modalAlert("Please Enter Mobile No.....");
            return false;
        }

        if (msisdn.length != 11) {
            modalAlert("Please Enter Valid Mobile No.....");
            return false;
        }
    });

    $('#tablebody').empty();
    $("input[type=radio]").checkboxradio();
    //user_id = $("#hiduserId").val();
    //alert(user_id);
    modalServiceTypeCreate(AddServiceCall);
    modalBreakCreate(breakAdd);
    modalHistoryCreate();
    modalMissingListCreate(missingNewCall);
    modalDashboardDialogCreate();
    LoadNotifcationListForUser();
    LoadRecentNotification();
    $("txtServiceTimer").html("Hello");
    $("#is_break").val("0");

    $("#logout").click(function (event) {
        if ($("#hidtokenNo").val() != "") {
            event.preventDefault();

            var service_sub_type_id = $('#service_sub_type_id').val();

            if (service_sub_type_id == null || service_sub_type_id == "") {
                modalConfirm("Current token is not closed yet. If press 'Ok', current token will free to queue for next counter call. <br />Do you want to logoff without close current token?"
                    , function () {
                        window.location.href = '../Account/LogOff';

                    }, null);
            }
            else {
                modalConfirm("Current service is not finished yet. If press 'Ok', current service will finish. <br />Do you want to finish current service and logoff from system?"
                    , function () {
                        SaveLogout();
                    }, null);
            }
        }
    });

    let addedService = sessionStorage.getItem("service");
    if (addedService == 1) {  //window.performance.navigation.type == 1
        $('#mdlRefreshReasonModal').modal({
            backdrop: 'static',
            keyboard: false
        });
        $("#start_time").val(sessionStorage.getItem("serviceTime"));
        $("#service_sub_type_name").val(sessionStorage.getItem("serviceName"));
        $("#service_sub_type_id").val(sessionStorage.getItem("serviceId"));

        totalSeconds = sessionStorage.getItem("serviceSecond");
        timerId = setInterval(setTimer, 1000);
    }
    window.onbeforeunload = function () {
        if ($("#service_sub_type_id").val() != "" || sessionStorage.getItem("service") == 1) {
            sessionStorage.setItem("service", 1);
            sessionStorage.setItem("serviceSecond", totalSeconds);
        } else {
            sessionStorage.setItem("service", 0);
        }
        return ("Are you sure?");
    }

});


function OpenNotifcationPanel() {
    
    notificationDataTable.ajax.reload();
    $("#dialog-notification-list").dialog("open");

}

function GetDeviceLoanEligibilityInfo() {

    $(".dummy_device_loan").text("NO");
    let msisdn = $.trim($("#txtContact").val());
    $("#txtHandsetCategory").val("Not Found");

    if (msisdn.length == 11) {

        $.ajax({
            url: "../MSISDNAndDeviceInfo/handset-category?msisdn=" + msisdn,
            type: 'GET',
            dataType: "json",
            success: function (data) {
                if (data.data != null) {

                    if (data.data != null) {
                        $("#txtHandsetCategory").val(data.data);
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }

        });

        $.ajax({
            url: "../MSISDNAndDeviceInfo/dfp-info?msisdn=" + msisdn,
            type: 'GET',
            dataType: "json",
            success: function (data) {
                if (data.data != null) {

                    if (data.data.is_eligible) {
                        $(".dummy_device_loan").text("YES");
                    } else {
                        $(".dummy_device_loan").text("NO");
                    }
                } else {
                    $(".dummy_device_loan").text("NO");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }

        });

    }

}

function LoadNotifcationListForUser() {
    var column = [
        { "data": "notificationDate" },
        { "data": "sender" },
        { "data": "message" },
        { "data": "is_seen" },
        { "data": "have_attachment" },
    ];

    notificationDataTable = $('#tblUserNotification').DataTable({
        "ajax": "../Notifications/users-notification",        
        columnDefs: [
            {
                'targets': 2,
                'render': function (data, type, row, meta) {
                    let dataString = "";
                    if (type === 'display') {
                       
                        if (row.message.length > 20) {
                            //dataString = "<div data-toggle='tooltip' data-placement='top' title='" + row.message +"'>" + row.message.substring(0, 20) + "...</div>";
                            dataString = '<div class="ctooltip" style="cursor: pointer;">' + row.message.substring(0, 20) + ' ... <span class="tooltiptext"> ' + row.message + '</span></div>';
                        } else {
                            dataString = '<div style="cursor: pointer;" onclick="SeenNotfication(' + row.notification_id + ')">' + row.message + '</div>';
                        }
                    }

                    return dataString;
                }
            },
            {
                'targets': 3,
                'render': function (data, type, row, meta) {
                    let dataString = "";
                    if (type === 'display') {
                        if (row.is_seen) {
                            dataString = "<div>Seen</div>";
                        } else {
                            dataString = "<div>Not Seen</div>";
                        }
                    }

                    return dataString;
                }
            },
            {
                'targets': 4,
                'render': function (data, type, row, meta) {
                    let dataString = "";
                    if (type === 'display') {
                        if (row.have_attachment) {
                            dataString = "<a href='../Notifications/ViewAttachment?id=" + row.notification_id + "' class='btn btn-color' target='_blank'>View</a>";
                        } else {
                            dataString = "";
                        }
                    }

                    return dataString;
                },
            }
        ],
        dom: 'B<"clear">lfrtip',
        aoColumns: column,
        rowId: 'id',
        "bLengthChange": false,
        "searching": false,
        "order": [[3, 'desc']],
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search..."
        },
        deferRender: true
    });
    notificationDataTable.on('draw', function () {
        $('[data-toggle="tooltip"]').tooltip({ effect: "blind", duration: 1000, tooltipClass: 'tooltipclass' });
    });
}

function SaveLogout() {
    var contactNo = $("#txtContact").val();
    var Customername = $("#txtName").val();
    var is_primary_contact = 0;
    if ($("#is_primary_contact")[0].checked) is_primary_contact = 1;
    var Customeraddress = $("#txtAddress").val();
    var customerissues = $("#txtIssues").val();
    var customersolutions = $("#txtsolutions").val();
    var customertokenno = $("#hidtokenNo").val();
    var service_sub_type_id = $('#service_sub_type_id').val();
    if (customertokenno == "") {
        ShowPannel(1);
        modalAlert("Please First Generate New Service No.....");
        return false;
    }

    if (service_sub_type_id == null || service_sub_type_id == "") {
        modalAlert("Please add a service.....");
        return false;
    }

    if (contactNo == "") {
        modalAlert("Please Enter Mobile No.....");
        return false;
    }

    if (contactNo.length != 11) {
        modalAlert("Please Enter Valid Mobile No.....");
        return false;
    }

    if (customerissues == "") {
        customerissues = " ";
    }

    if (customersolutions == "") {
        customersolutions = " ";
    }


    var data0 = {
        "contact_no": contactNo,
        "is_primary_contact": is_primary_contact,
        "customer_name": Customername,
        "issues": customerissues,
        "address": Customeraddress,
        "solutions": customersolutions,
        "token_id": customertokenno,
        "start_time": $("#start_time").val(),
        "service_sub_type_id": service_sub_type_id
    };

    $.ajax({
        url: '../ServiceDetails/Create',
        type: 'POST',
        dataType: 'json',
        data: { model: data0 },
        success: function (data) {
            if (data.success == false) {
                modalAlert(data.message);
                return;
            }
            window.location.href = '../Account/LogOff';
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

$(function () {
    $.support.cors = true;
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("../notifyDisplay")
        //.withUrl("../signalr/hubs/signalr")
        .build();
    console.log("service-details-v8.4.js : connection: ", connection);
    connection.serverTimeoutInMilliseconds = 1000 * 60 * 60; // 1 hour
    connection.start().then(function () {
        LoadBreakCount(); 
    }).catch(function (err) {
        modalAlert(err); 
    });

    connection.on("CallToken", function (counterId) {
        if ($('#hid_counter_id').val() == counterId) {
            NewServiceNo();
        }
    });
    connection.on("SendNotification", function (users, message, hasAttachment, notificationId) {
        var userId = $("#hid_user_id").val();

        if (users.includes(userId)) {
            let oldNotification = parseInt($.trim($(".dummy_notification_count").text()));
            oldNotification = isNaN(oldNotification) ? 0 : oldNotification;
            VisibleNotificationBadge(oldNotification + 1);
            PushNotificationPopUp(message, hasAttachment, notificationId, true);
            LoadRecentNotification();
        }
    });
    connection.onclose(function () {
        setTimeout(function () {
            connection.start();
        }, 1000); // Restart connection after 1 second.
    });    
});


//$(function () {
//    $.support.cors = true;
//    var notifications = $.connection.notifyDisplay;

//    notifications.serverTimeoutInMilliseconds = 1000 * 60 * 60; // 1 hour
//    $.connection.client.callToken = function (counter_id) {

//        // Text to Speech if any text available for this branch to speech
//        if ($('#hid_counter_id').val() == counter_id) NewServiceNo();
//    };

//    // Create a function that the hub can call to sendNotification
//    $.connection.client.sendNotification = function (users, message, hasAttachment, notification_id) {
//        var user_id = $("#hid_user_id").val();

//        if (users.indexOf(user_id) != -1) {

//            let oldNotification = parseInt($.trim($(".dummy_notification_count").text()));
//            oldNotification = (isNaN(oldNotification)) ? 0 : oldNotification;
//            VisibleNotificationBadge((oldNotification + 1));

//            PushNotificationPopUp(message, hasAttachment, notification_id, true);
//            LoadRecentNotification();
//        }
//    };

//    $.connection.hub.disconnected(function () {
//        setTimeout(function () {
//            $.connection.hub.start();
//        }, 1000); // Restart connection after 1 seconds.
//    });

//    // Start the connection.
//    $.connection.hub.start().done(function () {
//        LoadBreakCount();
//    }).fail(function (e) {
//        modalAlert(e);
//    });


//});


function LoadRecentNotification() {
    $(".dummy_recent_notification").empty();
    $.ajax({
        url: "../Notifications/recent-users-notification",
        type: 'GET',
        dataType: "json",
        success: function (data) {

            //debugger;
            $(data.data).each(function (index, item) {

                let notifyMessage = "";
                if (item.message.length > 50) {
                    notifyMessage = item.message.substring(0, 50) + " ... ";
                } else {
                    notifyMessage = item.message;
                }
                let notifyString = "";
                if (item.is_seen) {
                    notifyString = '<a class="hide-on-click"  onclick="ShowNotificationPopUp(' + item.notification_id + ')" href="javascript:void(0)">From <strong>' + item.sender + '</strong> ' + notifyMessage + '</a>';
                } else {
                    notifyString = '<a class="hide-on-click not-seen" id="notify_list_' + item.notification_id + '" onclick="ShowNotificationPopUp(' + item.notification_id + ', true)" href="javascript:void(0)">From <strong>' + item.sender + '</strong> ' + notifyMessage + '</a>';
                }

                $(".dummy_recent_notification").append(notifyString);
            });

            VisibleNotificationBadge(data.unseen);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }

    });
}

function VisibleNotificationBadge(count) {
    if (count > 0) {
        $(".dummy_notification_count").text(count);
        $(".dummy_notification_count").show();
    } else {
        $(".dummy_notification_count").text(0);
        $(".dummy_notification_count").hide();
    }
}

function ShowNotificationPopUp(notification_id, isNotSeen) {
    $.ajax({
        url: "../Notifications/get-notification?id=" + notification_id,
        type: 'GET',
        dataType: "json",
        success: function (data) {
            if (data.data != null) {
                PushNotificationPopUp(data.data.message, data.data.have_attachment, notification_id, isNotSeen);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function ScheduleNotification() {

    $.ajax({
        url: "../Notifications/scheduled-notification",
        type: 'GET',
        dataType: "json",
        success: function (data) {
            //debugger;
            if (data == true) {

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function PushNotificationPopUp(message, hasAttachment, notification_id, isNotSeen) {

    isNotificationNotSeen = (isNotSeen) ? true : false;
    $("#hdnNotificationId").val(notification_id);
    $(".notify-btn-area .btn-attachment").attr("href", "#");
    $(".notify_message_area .notify-message").html(message);
    //$(".dummy_notification_count").text((oldNotification + 1));

    if (hasAttachment) {
        $(".notify-btn-area .btn-attachment").attr("href", "../Notifications/ViewAttachment?id=" + notification_id);
        $(".notify-btn-area .btn-attachment").show();
    } else {
        $(".notify-btn-area .btn-attachment").hide();
    }

    $('#mdlNotificationModal').modal({
        backdrop: 'static',
        keyboard: false
    })
}

function SeenNotfication(notifyId) {

    console.log("Come to SeeNotification: ", notifyId);

    let notficationId = 0;
    if (typeof notifyId === "undefined") {
        notficationId = $.trim($("#hdnNotificationId").val());
    } else {
        notficationId = notifyId;
    }

    if (notficationId != "") {
        $.ajax({
            url: "../Notifications/SeenNotification?notificationId=" + notficationId,
            type: 'POST',
            dataType: "json",
            success: function (data) {
                if (data.success == true) {
                    $("#mdlNotificationModal").modal('hide');
                    if (isNotificationNotSeen) {

                        let oldNotification = parseInt($.trim($(".dummy_notification_count").text()));
                        oldNotification = (isNaN(oldNotification)) ? 0 : oldNotification;


                        VisibleNotificationBadge((oldNotification - 1));
                    }


                    //$(".dummy_notification_count").text((oldNotification - 1));

                    $("#notify_list_" + notficationId).removeClass("not-seen");
                }

                if (typeof notifyId === "undefined") {

                } else {
                    LoadRecentNotification();
                }


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }

        });
    }
    LoadRecentNotification();
}


function LoadBreakCount() {
    $.ajax({
        url: "../DailyBreaks/GetCountByUserId",
        type: 'POST',
        dataType: "json",
        success: function (data) {
            if (data.success == false) {

                modalAlert(data.Message);
            } else {
                if (data.is_break > 0)
                    $("#is_break").val('1');

                NewServiceNo();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown, url) {  
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });

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


function LoadServices(type_id) {
    $.ajax({
        //url: "/SQMS/ServiceDetails/NewTokenNo",
        url: "../ServiceSubTypes/GetByTypeId",
        type: 'POST',
        dataType: "json",
        data: { service_type_id: type_id },
        success: function (data) {
            var serviceSubTypes = data.serviceSubTypes;

            setTAT_visibility_time(data.globalSet);

            $('#div-sub-type').empty();
            $.each(serviceSubTypes, function (index, service) {
                var div = '<div class="col-lg-6 col-md-6 col-sm-6" style="text-align: left;" ><input type="radio" style="cursor:pointer" name="radio-service" id="' + service.service_sub_type_id + '" value="' + service.service_sub_type_id + '"/>'
                    + '<label for="' + service.service_sub_type_id + '" style="cursor:pointer;" hidden="hidden" max_duration="' + service.max_duration + '", tat_warning_time="' + service.tat_warning_time + '" style="padding:10px">' + service.service_sub_type_name + '</label></div>';
                $('#div-sub-type').append(div);
            });
            $("input[type=radio]").checkboxradio();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}


$("#service_type_id").change(function () {
    console.log("this.value: ", this.value);
    var type_id = this.value;

    LoadServices(type_id);

});


function breakAdd(break_type_id, remarks) {

    $.ajax({
        //daily_break_id, break_type_id, user_id, start_time, end_time, remarks//
        url: "../DailyBreaks/Create",
        type: 'POST',
        dataType: "json",
        data: {
            break_type_id: break_type_id,
            remarks: remarks
        },
        success: function (data) {
            if (data.success == false) {
                modalAlert(data.Message);
            } else {
                $("#is_break").val('1');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function breakcall() {
    // user_id = $("#hiduserId").val();

    $.ajax({
        //url: "/SQMS/ServiceDetails/NewTokenNo",
        url: "../DailyBreaks/Update",
        type: 'POST',
        dataType: "json",
        //data: { user_id: user_id },
        success: function (data) {
            if (data.success == true) {
                //window.location.href(webRootAddtionalPath + "/DailyBreaks/Index");
                $(location).attr('href', "../DailyBreaks/Index");
            }
            else {
                modalAlert(data.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });

    return;
}

function Recall() {

    $('#tablebody').empty();

    ShowPannel(1);

    $.ajax({
        //url: "/SQMS/ServiceDetails/NewTokenNo",
        url: "../ServiceDetails/NewTokenNo",
        type: 'POST',
        dataType: "json",
        success: function (data) {
           
            if (data.success == true) {

                $("#is_break").val(data.message.isBreak);
                $("#update-message").html('');
                $("#hiduserId").val(data.message.user_id);
                $("#update-message").html(data.message.token);
                //$("#start_time").val(data.message.start_time);
                $("#start_time").prop('disabled', true);
                $("#txtServiceType").val(data.message.serviceType);
                $("#txtServiceType").prop('disabled', true);
                $("#service_sub_type_name").prop('disabled', true);
                $("#hidtokenNo").val(data.message.tokenid);
                $("#token").val(data.message.token);
                $("#txtIssues").val('');
                $("#txtsolutions").val('');
                $("#txtServiceTimer").val('');
                $("#txtCallTime").val(data.message.call_time);
                $("#txtCallTime").prop('disabled', true);
                $("#txtgnTime").val(data.message.generate_time);
                $("#txtgnTime").prop('disabled', true);
                $("#txtWtTime").val(data.message.waitingtime);
                $("#txtWtTime").prop('disabled', true);
                if (data.message.mobile_no != "") {
                    $("#txtContact").val(data.message.mobile_no);
                    $("#txtName").val(data.message.customer_name);
                    $("#txtAddress").val(data.message.address);
                } else {
                    $("#txtContact").prop('disabled', false);
                    $("#txtName").val("");
                    $("#txtAddress").val("");
                }
                $("#is_primary_contact").attr('checked', true);
                $("#service_type_id").val(data.message.service_type_id);

                LoadServices(data.message.service_type_id);

                GetDeviceLoanEligibilityInfo();

            } else {
                $("#txtServiceType").val('');
                $("#txtServiceType").prop('disabled', true);
                $("#txtCallTime").val('');
                $("#txtCallTime").prop('disabled', true);
                $("#txtgnTime").val('');
                $("#txtgnTime").prop('disabled', true);
                $("#txtWtTime").val('');
                $("#txtWtTime").prop('disabled', true);
                modalAlert(data.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#txtServiceType").val('');
            $("#txtServiceType").prop('disabled', true);
            $("#txtCallTime").val('');
            $("#txtCallTime").prop('disabled', true);
            $("#txtgnTime").val('');
            $("#txtgnTime").prop('disabled', true);
            $("#txtWtTime").val('');
            $("#txtWtTime").prop('disabled', true);
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function NewServiceNo() {
    if ($("#is_break").val() == 1) {
        modalConfirm("Do you want to Take a Break?", breakcall, Recall);
    }
    else {
        Recall();
    }
}



function AddServiceCall(value, text, max_duration) {
    ShowPannel(2);
    $('#service_sub_type_id').val(value);
    $('#service_sub_type_name').val(text + ' (Maximum duration: ' + max_duration + ' minuties)');
    $.ajax({
        url: "../ApiService/GetDBDate",
        type: 'GET',
        dataType: "json",
        success: function (data) {
            if (data.success == true) {
                $("#start_time").val(data.dbdate);
                sessionStorage.setItem("serviceTime", $("#start_time").val());
            } else {
                modalAlert(data.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }

    });

}

var serviceTaken = 0;
function AddServiceSave() {

    ShowPannel(2);
    var contactNo = $("#txtContact").val();
    var is_primary_contact = 0;
    if ($("#is_primary_contact")[0].checked) is_primary_contact = 1;
    var Customername = $("#txtName").val();
    var Customeraddress = $("#txtAddress").val();
    var customerissues = $("#txtIssues").val();
    var customersolutions = $("#txtsolutions").val();
    var customertokenno = $("#hidtokenNo").val();
    var service_sub_type_id = $('#service_sub_type_id').val();
    let refresh_reason = sessionStorage.getItem("reason");

    if (contactNo == "") {
        modalAlert("Please Enter Mobile No.....");
        return false;
    }

    if (contactNo.length != 11) {
        modalAlert("Please Enter Valid Mobile No.....");
        return false;
    }

    if (customerissues == "") {
        customerissues = " ";
    }

    if (customersolutions == "") {
        customersolutions = " ";
    }

    var data0 = {
        "contact_no": contactNo,
        "is_primary_contact": is_primary_contact,
        "customer_name": Customername,
        "issues": customerissues,
        "address": Customeraddress,
        "solutions": customersolutions,
        "token_id": customertokenno,
        "start_time": $("#start_time").val(),
        "service_sub_type_id": service_sub_type_id,
        "refresh_reason": refresh_reason
    };

    $.ajax({
        url: '../ServiceDetails/AddService',
        async: false,
        type: 'POST',
        dataType: 'json',
        data: { model: data0 },
        success: function (data) {
            if (data.success) {
                $("#txtIssues").val('');
                $("#txtsolutions").val('');
                //$("#txtServiceTimer").innerHTML('');
                $("#start_time").val('');
                $("#service_sub_type_name").val('');
                $("#service_sub_type_id").val('');
                $("#is_primary_contact").attr('checked', false);
                serviceTaken += 1;
                serviceDialog.dialog('open');
                sessionStorage.setItem("reason", "");
            }
            else {
                modalAlert(data.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest.responseText + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}


function AddService() {
    ShowPannel(2);
    var contactNo = $("#txtContact").val();
    var Customername = $("#txtName").val();
    var Customeraddress = $("#txtAddress").val();
    var customerissues = $("#txtIssues").val();
    var customersolutions = $("#txtsolutions").val();
    var customertokenno = $("#hidtokenNo").val();
    var service_sub_type_id = $('#service_sub_type_id').val();

    if (customertokenno == "") {
        ShowPannel(1);
        modalAlert("Please First Generate New Service No.....");
        return false;
    }

    if (contactNo == "") {
        modalAlert("Please Enter Mobile No.....");
        return false;
    }

    if (contactNo.length != 11) {
        modalAlert("Please Enter Valid Mobile No.....");
        return false;
    }

    if (service_sub_type_id == null || service_sub_type_id == "") {
        serviceDialog.dialog('open');
        return false;
    }
    else {
        modalConfirm("Do you want to finish current service?", AddServiceSave, IgnoreCurrentService);
    }
}

function IgnoreCurrentService() {
    serviceDialog.dialog('open');
}



function CallToken(token_no) {
    if (token_no == null || token_no == "") {
        modalAlert("Please input a token no for next service");
        return;
    }

    $.ajax({
        url: "../ServiceDetails/CallManualTokenNo",
        type: 'POST',
        data: { token_no_string: token_no },
        dataType: "json",
        success: function (data) {
            //debugger;
            if (data.success == true) {

                modalAlert("Token No# " + token_no + " is now in your queue list. After current service, it will automatically call.", function () {
                    var token_id = $("#hidtokenNo").val();
                    if (token_id == null || token_id == "") {
                        NewServiceNo();
                    }
                });
            } else {
                modalAlert(data.message);
            }


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }

    });
}

function ManualCall() {
    var token = $("#hidtokenNo").val();

    $('#tablebody').empty();

    modalPrompt("Please enter token no which is not served yet or missed:", CallToken);

}



function CounterTransfer(counter_no) {
    if (counter_no == null || counter_no == "") {
        modalAlert("Please input a counter no for transfer this service");

        return;
    } else if ($("#hid_counter_no").val() == counter_no) {
        modalAlert("You can not transfer token yourself, pelase input another counter no.");
        return;
    }



    var token = $("#hidtokenNo").val();

    $.ajax({
        url: "../ServiceDetails/Transfer",
        type: 'POST',
        dataType: "json",
        data: { token_id: token, counter_no: counter_no },
        success: function (data) {

            if (data.success == true) {
                $("#txtContact").val('');
                $("#txtName").val('');
                $("#txtIssues").val('');
                $("#txtsolutions").val('');
                $("#txtServiceTimer").val('');
                $("#txtAddress").val('');
                $("#update-message").html('');
                $("#hidtokenNo").val('');
                $("#txtServiceType").val('');
                $("#start_time").val('');

                NewServiceNo();
                sessionStorage.setItem("service", 0);
            } else {
                modalAlert(data.message);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function Transfer() {

    $('#tablebody').empty();

    modalPrompt("Please enter counter no where you transfer this token:", CounterTransfer);
}



function CancelNext() {
    var token = $("#hidtokenNo").val();
    if (token == null || token == "") {
        NewServiceNo();
    }
    else {
        $.ajax({
            url: "../ServiceDetails/CancelTokenNo",
            type: 'POST',
            dataType: "json",
            data: { tokenID: token },
            success: function (data) {

                $("#txtContact").val('');
                $("#txtName").val('');
                $("#txtIssues").val('');
                $("#txtsolutions").val('');
                $("#txtServiceTimer").val('');
                $("#txtAddress").val('');
                $("#update-message").html('');
                $("#hidtokenNo").val('');

                NewServiceNo();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }
        });
    }

}

function GetCustomerInformation() {
    $('#tablebody').empty();
    var token = $("#hidtokenNo").val();
    var contact_number = $("#txtContact").val();
    console.log("GetCustomerInformation: ");
    $.ajax({
        url: "../ServiceDetails/GetCustomerInformation",
        type: 'POST',
        dataType: "json",
        data: {
            token_id: token,
            contact_no: contact_number
        }
    }).done(function (response) {

        console.log("response: ", response);

        var select = $('#tablebody');

        for (var i = 0; i < response.message.length; i++) {
            select.append($('<tr><td>' + response.message[i].service_datetime_string + '</td><td>'
                + response.message[i].issues + '</td><td>'
                + response.message[i].solutions + '</td></tr>'));
        }

        historyDialog.dialog('open');

    }).fail(function (XMLHttpRequest, textStatus, errorThrown) {
        console.log("Error: ", textStatus);
        modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
    });
    console.log("GetCustomerInformation: Done ");
}


//function GetCustomerInformation() {
//    $('#tablebody').empty();
//    var token = $("#hidtokenNo").val();
//    var contact_nember = $("#txtContact").val();
//    $.ajax({
//        url: "../ServiceDetails/GetCustomerInformation",
//        type: 'POST',
//        dataType: "json",
//        data: {
//            token_id: token,
//            contact_no: contact_nember
//        },
//        success: function (response) {
//            console.log("response: ", response);
//            var select = $('#tablebody');
//            for (var i = 0; i < response.message.length; i++) {
//                select.append($('<tr><td>' + response.message[i].service_datetime_string + '</td><td>'
//                    + response.message[i].issues + '</td><td>'
//                    + response.message[i].solutions + '</td></tr>'));


//            }

//            historyDialog.dialog('open');


//        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
//            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
//        }
//    });

//}


function SaveNext() {
    ShowPannel(2);
    var contactNo = $("#txtContact").val();
    var Customername = $("#txtName").val();
    var is_primary_contact = 0;
    if ($("#is_primary_contact")[0].checked) is_primary_contact = 1;
    var Customeraddress = $("#txtAddress").val();
    var customerissues = $("#txtIssues").val();
    var customersolutions = $("#txtSolutions").val();
    var customertokenno = $("#hidtokenNo").val();
    var service_sub_type_id = $('#service_sub_type_id').val();
    let refresh_reason = sessionStorage.getItem("reason");

    if (customertokenno == "") {
        ShowPannel(1);
        modalAlert("Please First Generate New Service No.....");
        return false;
    }

    if (service_sub_type_id == null || service_sub_type_id == "") {
        modalAlert("Please add a service.....");
        return false;
    }

    if (contactNo == "") {
        modalAlert("Please Enter Mobile No.....");
        return false;
    }

    if (contactNo.length != 11) {
        modalAlert("Please Enter Valid Mobile No.....");
        return false;
    }

    stopTimer();

    if (customerissues == "") {
        customerissues = " ";
    }

    if (customersolutions == "") {
        customersolutions = " ";
    }


    var data0 = {
        "contact_no": contactNo,
        "is_primary_contact": is_primary_contact,
        "customer_name": Customername,
        "issues": customerissues,
        "address": Customeraddress,
        "solutions": customersolutions,
        "token_id": customertokenno,
        "start_time": $("#start_time").val(),
        "service_sub_type_id": service_sub_type_id,
        "refresh_reason": refresh_reason
    }
    console.log("data0: ", data0);
    $.ajax({
        url: '../ServiceDetails/Create',
        type: 'POST',
        dataType: 'json',
        data: { model: data0 },
        success: function (data) {
            if (data.success == false) {
                modalAlert(data.message);
                return;
            }
            $("#update-message").html('');
            $("#txtContact").val('');
            $("#is_primary_contact").attr("checked", false);
            $("#txtName").val('');
            $("#txtIssues").val('');
            $("#txtsolutions").val('');
            $("#txtServiceTimer").val('');
            $("#txtAddress").val('');
            $("#txtServiceType").val('');
            $("#start_time").val('');
            $("#service_sub_type_name").val('');
            $("#service_sub_type_id").val('');
            serviceTaken = 0;
            NewServiceNo();
            clearSession();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
    // }



}

function clearSession() {
    sessionStorage.setItem("service", 0);
    sessionStorage.setItem("serviceSecond", 0);
    sessionStorage.setItem("serviceTime", 0);
    sessionStorage.setItem("reason", "");
}


function TokenReInitiate(token_id) {
    $.ajax({
        url: '../TokenQueues/ReInitiate',
        type: 'POST',
        dataType: 'json',
        data: { token_id: token_id },
        success: function (data) {
            if (data.success == true) {
                $("#data-skipped").find("#" + token_id).remove();
                modalAlert(data.message);
            }
            else {
                modalAlert(data.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function TokenAssignToMe(token_id) {
    $.ajax({
        url: '../TokenQueues/AssignToMe',
        type: 'POST',
        dataType: 'json',
        data: { token_id: token_id },
        success: function (data) {
            if (data.success == true) {
                $("#data-skipped").find("#" + token_id).remove();
                modalAlert(data.message);
            }
            else {
                modalAlert(data.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function missingNewCall() {
    var token_id = $("#hidtokenNo").val();
    if (token_id == null || token_id == "") {
        NewServiceNo();
    }
}

function WriteLog(data) {
    $.ajax({
        url: "../ApiService/WriteLog",
        type: 'POST',
        dataType: 'json',
        contentType:'application/json',
        data: { model: data },
        success: function (data) { }
    });
}

function getBrowser() {
    var ua = navigator.userAgent, tem, M = ua.match
        (/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
        return 'IE' + ' ' + (tem[1] || '');
    }
    if (M[1] === 'Chrome') {
        tem = ua.match(/\bOPR\/(\d+)/)
        if (tem != null)
            return 'Opera' + tem[1];
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/(\d+)/i)) != null) { M.splice(1, 1, tem[1]); }
    return M[0] + ' ' + M[1];
}

function RefreshReasonSave() {
    if ($("#refeshReason").val().trim() != "") {
        $("#mdlRefreshReasonModal").modal('hide');
        sessionStorage.setItem("reason", $("#refeshReason").val());
    }
}

window.onerror = function (message, source, lineno, colno, error) {
    // modalAlert(XMLHttpRequest.responseText + ": " + textStatus + ": " + errorThrown, 'Error!!!');

    var errorLoggerObj = {
        "exceptionLogTime": new Date().toLocaleString(),
        "errorSource": 'client_end',
        "message": getBrowser() + '|| Source: ' + source + ' || line no: ' + lineno + ' || column:' + colno + '||  ' + message + ' || Error: ' + error,
        "controllerName": '',
        "actionName": ''
    };

    WriteLog(errorLoggerObj);
};

