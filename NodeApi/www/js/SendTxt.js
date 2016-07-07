var api_endpoint = 'http://localhost:1234';
//var api_endpoint = '';
var options = {};
var inst;
var instrecv;
var clipboard = new Clipboard('#btnCopy');
var clipboardCopyCode = new Clipboard('#btnCopyCode');
var sendmodalopen = false;
var timerid = 0;

function rcvClicked() {
    event.preventDefault();

    var recvText = $("#inputReceive").val();
    recvText  = recvText.replace(/ /g, "");
    if (recvText == null || recvText === '') {
        alert("No Code entered!");
        $("#inputReceive").focus();
        return;
    }
    if (recvText.length !== 6) {
        alert("Code is always 6 digits!");
        $("#inputReceive").focus();
        return;
    }
    StartReceiveSession(recvText);
}


function sendClicked() {
    event.preventDefault();
    var sendText = $("#inputSend").val();
    if (sendText == null || sendText === '') {
        alert("Cannot send empty message!");
        $("#inputSend").focus();
        return;
    }
    StartSendSession({ data: sendText });
}


function initClipboardjs() {

    //Copy Received Text
    clipboard.on('success', function (e) {
        e.clearSelection();
        $("#btnCopy").text("Copied successfully!");
    });

    clipboard.on('error', function (e) {
        $("#btnCopy").text("Could not copy to clipboard");
        $("#btnCopy").css('background', 'red');
    });


    //Copy Code
    clipboardCopyCode.on('success', function (e) {
        e.clearSelection();
        $("#btnCopyCode").css('background', 'lightgreen');
    });

    clipboardCopyCode.on('error', function (e) {
        $("#btnCopyCode").css('background', 'red');
    });
}


function initBindings() {
    $("#btnReceive").click(rcvClicked);
    $("#btnSend").click(sendClicked);
    $('[data-toggle="popover"]').popover();
    inst = $('[data-remodal-id=modal]').remodal();
    instrecv = $('[data-remodal-id=modalrecv]').remodal();
    $(document).on('closed', '.remodal', function (e) {
        sendmodalopen = false;
        $("#btnCopyCode").css('background', '#1ab0db');
    });

}


jQuery(document).ready(function () {
    "use strict";
    initBindings();
    initClipboardjs();
});



function StartReceiveSession(code) {
    $.ajax({
        type: "GET",
        url: api_endpoint + "/api/getSession/" + code,
        crossDomain: true,
        success: function (data, status) {
            if (data.success) {
                $("#popuptextarea").val(data.returnobject);
                instrecv.open();
                $("#popuptextarea").select();
            }
        },
        error: function (jqXHR, status) {
            console.log(jqXHR);
        }
    });
}



function StartSendSession(data) {
    $.ajax({
        type: "POST",
        url: api_endpoint + "/api/createSession",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        dataType: "json",
        success: function (data, status) {
            if (data.success) {
                var code = data.returnobject.toString().substring(0, 3) + " " + data.returnobject.toString().substring(3, 6);
                $("#popupcodespan").val(code);
                inst.open();

                var tenminutes = 60 * 10,
                    display = $('#popuptimeleft');
                sendmodalopen = true;
                startTimer(tenminutes, display);

            }
        },
        error: function (jqXHR, status) {
            console.log(jqXHR);
        }
    });
}





function startTimer(duration, display) {
    $('#progressbar').show();
    var timer = duration, minutes, seconds;
    timerid = setInterval(function () {
        if (!sendmodalopen) clearInterval(timerid);
        minutes = parseInt(timer / 60, 10);
        seconds = parseInt(timer % 60, 10);

        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;

        display.text(minutes + ":" + seconds + " remain before expiry");

        var percent = (timer / duration) * 100;
        $('#progressbar').attr('aria-valuenow', percent).css('width', percent + "%");
        if (--timer < 0) {
            $('#progressbar').hide();
            $("#popupcodespan").val("EXPIRED");
            clearInterval(timerid);
        }
    }, 1000);
}

