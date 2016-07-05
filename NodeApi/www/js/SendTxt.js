var api_endpoint = 'http://localhost:1234';
//var api_endpoint = '';
var options = {};
var inst;
var instrecv;
var clipboard = new Clipboard('#btnCopy');

function rcvClicked() {
    event.preventDefault();
    var recvText = $("#inputReceive").val();
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
    clipboard.on('success', function (e) {
        e.clearSelection();
        $("#btnCopy").text("Copied successfully!");
    });

    clipboard.on('error', function (e) {
        $("#btnCopy").text("Could not copy to clipboard");
        $("#btnCopy").css('background', 'red');
    });
}


function initBindings() {
    $("#btnReceive").click(rcvClicked);
    $("#btnSend").click(sendClicked);
    $('[data-toggle="tooltip"]').tooltip();
    inst = $('[data-remodal-id=modal]').remodal();
    instrecv = $('[data-remodal-id=modalrecv]').remodal();
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
        success: function (data, status, jqXHR) {
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
        success: function (data, status, jqXHR) {
            if (data.success) {
                var code = data.returnobject.toString().substring(0, 3) + " " + data.returnobject.toString().substring(3, 6);
                document.getElementById("popupcodespan").innerHTML = code;
                inst.open();
            }
        },
        error: function (jqXHR, status) {
            console.log(jqXHR);
        }
    });
}