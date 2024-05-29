$(function () {
    $.support.cors = true;
    var connection = new signalR.HubConnectionBuilder()
        //.withUrl("../signalr/hubs/signalr")
        .withUrl("../notifyDisplay")
        .build();

    console.log("counter-status.js : connection: ", connection);
    connection.start().then(function () {
    }).catch(function (err) {
        modalAlert(err);
    });
    connection.on("CounterStatusChanged", function (branch_id) {

        // Text to Speech if any text available for this branch to speech
        if ($('#hid_branch_id').val() == branch_id) ReLoadCounterStatus();
    });
});
//$(function () {
//    $.support.cors = true;
//    // Declare a proxy to reference the hub.
//    var notifications = $.connection.notifyDisplay;

//    // Create a function that the hub can call to broadcast messages.
//    notifications.client.counterStatusChanged = function (branch_id) {

//        // Text to Speech if any text available for this branch to speech
//        if ($('#hid_branch_id').val() == branch_id) ReLoadCounterStatus();
//    };

//    // Start the connection.
//    $.connection.hub.start().done(function () {
//        //ReLoadCounterStatus();
//    }).fail(function (e) {
//        modalAlert(e);
//    });

//});

function ReLoadCounterStatus() {
    $.ajax({
        url: "../Branches/GetCounterCurrentStatus",
        type: 'POST',
        dataType: "json",
        success: function (data) {
            if (data.success == false) {

                modalAlert(data.message);
            } else {
                $("#tblCounterStatus").find("tr:gt(0)").remove();
                $.each(data.counterStatusList, function (i, item) {
                    $("#tblCounterStatus")
                        .append($('<tr>')
                            .append($('<td>')
                                .append(item.counter_no)
                            )
                            .append($('<td>')
                                .append(item.Status)
                            )
                            .append($('<td>')
                                .append(item.user_full_name)
                            )
                            .append($('<td>')
                                .append(item.login_time_formated)
                            )
                            .append($('<td>')
                                .append(item.idle_from_formated)
                            )
                            .append($('<td>')
                                .append(item.token_no_formated)
                            )
                            .append($('<td>')
                                .append(item.call_time_formated)
                            )
                        );
                });

            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}
