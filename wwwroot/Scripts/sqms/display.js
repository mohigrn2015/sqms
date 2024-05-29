/*
            window.addEventListener('load', function(){
                var newVideo = document.getElementById('myVideo');
                newVideo.addEventListener('ended', function() {
                    this.currentTime = 0;
                    this.play();
                }, false);

                newVideo.play();

            });
        */
var speakLanguage, speakGender, speakRate;
$(document).ready(function () {
    speakLanguage = $('#speakLanguage').val();
    speakGender = $('#speakGender').val();
    speakRate = $('#speakRate').val();
    modalConfirm("Do you want to display as full screen?", function () {
        toggleFullScreen(document.body);
    });
})

function toggleFullScreen(elem) {
    // ## The below if statement seems to work better ## if ((document.fullScreenElement && document.fullScreenElement !== null) || (document.msfullscreenElement && document.msfullscreenElement !== null) || (!document.mozFullScreen && !document.webkitIsFullScreen)) {
    if ((document.fullScreenElement !== undefined && document.fullScreenElement === null) || (document.msFullscreenElement !== undefined && document.msFullscreenElement === null) || (document.mozFullScreen !== undefined && !document.mozFullScreen) || (document.webkitIsFullScreen !== undefined && !document.webkitIsFullScreen)) {
        if (elem.requestFullScreen) {
            elem.requestFullScreen();
        } else if (elem.mozRequestFullScreen) {
            elem.mozRequestFullScreen();
        } else if (elem.webkitRequestFullScreen) {
            elem.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
        } else if (elem.msRequestFullscreen) {
            elem.msRequestFullscreen();
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
    }
}

var vid = document.getElementById("myVideo");
vid.addEventListener('ended', videoEndHandler, false);
function videoEndHandler(e) {
    vid.load();
    vid.mute = true;
    vid.play();
}

$(function () {
    $.support.cors = true;
    var connection = new signalR.HubConnectionBuilder()
        //.withUrl("../signalr/hubs/signalr")
        .withUrl("../notifyDisplay")
        .build();
    console.log("display.js : connection: ", connection);
    connection.start().then(function () {
        getAllMessages();
        welComeSpeech();
    }).catch(function (err) {
        modalAlert(err);
    });
    connection.on("updateMessages", function (text, branch_id, counter_no, token_no, tokenAdded, tokenCalled, playListChanged, footerChanged) {
        getAllMessages();
        if (text.length > 0 && $('#branch_id').val() == branch_id) textToTalk(text);
    });
});
//$(function () {
//    $.support.cors = true;
//    // Declare a proxy to reference the hub.
//    var notifications = $.connection.notifyDisplay;

//    // Create a function that the hub can call to broadcast messages.
//    notifications.client.updateMessages = function (text, branch_id, counter_no, token_no, tokenAdded, tokenCalled, playListChanged, footerChanged) {
//        //Call API for Display Data Information [Tokens in progress at counters, next tokens and video content]
//        getAllMessages();  

//        // Text to Speech if any text available for this branch to speech
//        if (text.length > 0 && $('#branch_id').val() == branch_id) textToTalk(text);
//    };

//    // Start the connection.
//    $.connection.hub.start().done(function () {
//        //After load UI: Call API for Display Data Information [Tokens in progress at counters, next tokens and video content]
//        getAllMessages();
//        //After load UI: Text to Speech for Welcome message
//        welComeSpeech();
//    }).fail(function (e) {
//        modalAlert(e);
//    });

//});

vid.onpause = function () {
    vid.load();
    vid.mute = true;
    vid.play();
}

function welComeSpeech() {
    var text = $('#welcome').val();

    responsiveVoice.setDefaultVoice(speakLanguage + " " + speakGender);
    responsiveVoice.speak(text, speakLanguage + " " + speakGender, { rate: speakRate });
}

function textToTalk(text) {
    responsiveVoice.setDefaultVoice(speakLanguage + " " + speakGender);
    responsiveVoice.speak(text, speakLanguage + " " + speakGender, { rate: speakRate });
}

function getAllMessages() {
    var branch_id = $('#branch_id').val();
    $.ajax({
        url: '../Counters/GetDisplayInfo?branch_id=' + branch_id,
        type: 'GET',
    }).done(function (data) {
        if (data.success === true || data.success === "true") {
            $("#divCurrentTokens").empty();
            var row = '<div class="col-sm-6 bg-color-head"><h2 onclick="return toggleFullScreen(document.body);" style="text-align:center">Counter</h2></div><div class="col-sm-6 bg-color-head"><h2 style="text-align:center">Token</h2></div>';
            $('#divCurrentTokens').append(row);
            $.each(data.tokenInProgress, function (key, token) {
                console.log("token: ", token);
                row = '<div class="col-sm-6 bg-color"><h1>' + token.counter_no + '</h1></div><div class="col-sm-6 bg-color"><h1>' + token.token_no_formated + '</h1></div>';
                $('#divCurrentTokens').append(row);
            });

            $("#nextToken").text('Next Token: ' + data.nextTokens);

        } else {
            modalAlert(data.message);
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        modalAlert(textStatus + ": " + errorThrown, 'Error!!!');
    });
}

//function getAllMessages() {
    
//    var branch_id = $('#branch_id').val();
//    $.ajax({
//        url: '../Counters/GetDisplayInfo?branch_id=' + branch_id,
//        type: 'GET',
//    }).success(function (data) {
//        console.log("result:-", data);
//        if (data.success == "true") {
//            $("#divCurrentTokens").empty();
//            var row = '<div class="col-sm-6 bg-color-head"><h2 onclick="return toggleFullScreen(document.body);" style="text-align:center">Counter</h2></div><div class="col-sm-6 bg-color-head"><h2 style="text-align:center">Token</h2></div>';
//            $('#divCurrentTokens').append(row);
//            $.each(data.tokenInProgress, function (key, token) {
//                row = '<div class="col-sm-6 bg-color"><h1>' + token.counter_no + '</h1></div><div class="col-sm-6 bg-color"><h1>' + token.token_no_formated + '</h1></div>';
//                $('#divCurrentTokens').append(row);
//            });

//            $("#nextToken").text('Next Token: ' + data.nextTokens);

           
//        }
//        else {
//            modalAlert(data.message);
//        }
//    }).error(function (XMLHttpRequest, textStatus, errorThrown) {
//        modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
//    });
//}
