var dialogBox, dialogBody, inputBox, serviceDialog, breakDialog, missingListDialog, historyDialog, reportDialog, dashboardDialog, galleryDialog;

$(document).ready(function () {
    dialogBox = $("#dialog-message");
    dialogBody = dialogBox.find("#body");
    
    inputBox = '<br/><input type="text" id="inputBox" placeholder="XXXXXXX" />'

    
})

function modalAlert(msg, callBack){
    dialogBody.html(msg);
    dialogBox.dialog({
        resizable: false,
        modal: true,
        closeOnEscape: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
                if (callBack != null) {
                    callBack();
                }
            }
        }
    });
}

function modalHistoryCreate() {
    historyDialog=
        $("#div-history").dialog({
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
}

function modalConfirm(msg, callback, cancelCallBack) {
    dialogBody.html(msg);
    $("#dialog-message").dialog({
        autoOpen: true,
        resizable: false,
        modal: true,
        width: "auto",
        closeOnEscape: false,
        buttons: {
            "Yes": function () {
                $(this).dialog("close");
                callback();
            },
            "No": function () {
                $(this).dialog("close");
                if (cancelCallBack != null) {
                    cancelCallBack();
                }
            }
        }
    });
}

function modalPrompt(msg, callback) {

    dialogBody.html(msg + inputBox);
    
    $("#dialog-message").dialog({
        autoOpen: true,
        resizable: false,
        modal: true,
        height: "auto",
        width: "auto",
        closeOnEscape: false,
        buttons: {
            "Ok": function () {
                var value = dialogBody.find("#inputBox").val();
                if (value == null || value == "") {
                    //modalAlert("Please input a value then press Ok or press Cancel");
                    return;
                }
                
                $(this).dialog("close");
                callback(value);
            },
            "Cancel": function () {
                $(this).dialog("close");
                //callback("close");
            }
        }
    });
}



function modalServiceTypeCreate(callback) {

     serviceDialog=

     $("#div-services").dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        height: 400,
        width: 800,
        closeOnEscape: false,
        buttons: {
            "Ok": function () {
                
                var value = $("input[name=radio-service]:checked").val();
                var text = $("input[name=radio-service]:checked").next('label').text();
                var max_duration = $("input[name=radio-service]:checked").next('label').attr('max_duration');
                var tat_warning_time = $("input[name=radio-service]:checked").next('label').attr('tat_warning_time');

                setWarningTime(max_duration, tat_warning_time);

                if (value == null || value == "") {
                    modalAlert("Please select a service then press Ok or press Cancel");
                    return;
                }

                $(this).dialog("close");
                callback(value, text, max_duration);

                stopTimer();
                timerId = setInterval(setTimer, 1000);
                
                sessionStorage.setItem("serviceName", $("#service_sub_type_name").val());
                sessionStorage.setItem("serviceId", $("#service_sub_type_id").val());
            },
            "Cancel": function () {
                $(this).dialog("close");
                //callback("close");
            }
        }
        });

    
}


function modalBreakCreate(callback) {
    
    breakDialog =

        $("#dialog-url-break").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Take a break',
            height: 'auto',
            width: 650,
            closeOnEscape: false,
            buttons: {
                "Ok": function () {
                    var break_type_id = $("#dialog-url-break").find("#break_type_id").val();
                    var remarks = $("#dialog-url-break").find("#remarks").val();
                    callback(break_type_id, remarks);
                    $(this).dialog("close");
                },
                "Cancel": function () {
                    $(this).dialog("close");
                    //callback("close");
                }
            }
        });
}

function loadBreakDialog() {
    if ($("#is_break").val() == 1) {
        modalAlert("You already defined a break, please complete the break first.");
        return;
    }
    breakDialog.load("../DailyBreaks/Create", function () {
        breakDialog.dialog('open');
    });

}

function modalDashboardDialogCreate() {

    dashboardDialog =

        $("#dialog-url-dashboard").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Service List',
            height: 'auto',
            width: 850,
            closeOnEscape: true,
            buttons: {
                "Ok": function () {
                    $(this).dialog("close");
                }
            }
        });
}

function loadDashboardBranchListDialog() {
    
    dashboardDialog.load("../ServiceDetails/ServiceList", function () {
        dashboardDialog.dialog('option', 'title', 'Service List');
        dashboardDialog.dialog('open');
    });

}

function loadDashboardCounterStatusDialog() {

    dashboardDialog.load("../ServiceDetails/CounterServiceList", function () {
        dashboardDialog.dialog('option', 'title', 'Counter Status');
        dashboardDialog.dialog('open');
    });

}

function loadDashboardUserStatusDialog() {

    dashboardDialog.load("../ServiceDetails/UserServiceList", function () {
        dashboardDialog.dialog('option', 'title', 'User Status');
        dashboardDialog.dialog('open');
    });

}



function modalMissingListCreate(callBack) {
    missingListDialog =

        $("#dialog-url-skipped").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Customer Missing List',
            height: 600,
            width: 950,
            closeOnEscape: true,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack();
                    }
                }
            }
        });
}

function loadMissingListDialog() {
    missingListDialog.load("../TokenQueues/Skipped", function () {
        missingListDialog.dialog('open');
        SelectBranch();
    });

}


function modalGalleryCreate(callBack) {
    galleryDialog =

        $("#dialog-url-gallery").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Media Gallery',
            height: 600,
        width: 950,
        closeOnEscape: false,
            buttons: {
                Ok: function () {
                    var value = $("input[name=radio-gallery]:checked").val();
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack(value);
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
}

function loadGalleryDialog(directory) {
    debugger
    this.event.preventDefault();
    galleryDialog.load("../Gallery/List?directory=" + directory, function () {
        galleryDialog.dialog('open');
    });
}

//function loadGalleryDialog(directory) {
//    debugger;
//    event.preventDefault(); // Use event directly
//    if (galleryDialog) {
//        galleryDialog.load("../Gallery/List?directory=" + directory, function () {
//            galleryDialog.dialog('open');
//        });
//    } else {
//        console.error("galleryDialog is not initialized properly.");
//    }
//}


function InitiateReportBox() {
    reportDialog =
        $("#dialog-report").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: "Report Viewer",
            height: 600,
            width: 950,
            closeOnEscape: true,
            buttons: {
                Close: function () {
                    $(this).dialog("close");
                }
            }
        });
}


function loadReport(url, fileName) {
    reportDialog.load(url, function () {
        var object = "<iframe src='../../../GeneratedReports/" + fileName + ".pdf' width='100%' height='97%'>";
        object += "</iframe>";
        //object = object.replace(/{FileName}/g, "Files/" + fileName);
        $("#dialog-report").html(object);
        reportDialog.dialog('open');
    });
}
