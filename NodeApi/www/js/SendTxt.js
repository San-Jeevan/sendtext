var api_endpoint = '';

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


}


function sendClicked() {
    event.preventDefault();
    var sendText = $("#inputSend").val();
    if (sendText == null || sendText === '') {
        alert("Cannot send empty message!");
        $("#inputSend").focus();
        return;
    }
}


function initBindings() {
    $("#btnReceive").click(rcvClicked);
    $("#btnSend").click(sendClicked);
}



jQuery(document).ready(function () {
    "use strict";
    initBindings();
});



function StartSendSession(data) {
    $.ajax({
        type: "POST",
        url: api_endpoint + "send",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        dataType: "json",
        success: function (data, status, jqXHR) {
            alert("success");
        },

        error: function (jqXHR, status) {
            console.log(jqXHR);
           
        }
    });
}



function StartReceiveSession(data) {
    $.ajax({
        type: "POST",
        url: api_endpoint + "recv",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        dataType: "json",
        success: function (data, status, jqXHR) {
            alert("success");
        },

        error: function (jqXHR, status) {
            console.log(jqXHR);

        }
    });
}