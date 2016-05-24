
var _recentCordinates = [];
var _SessionParticipants = [];
var _myMarker;

var signalrConnected = false;
var mySignalRId = "0";



function SendLocationToInternet(position) {
  
    if (signalrConnected && mySignalRId!= "0")
    {
        var update = { SignalRId : mySignalRId, Latitude : position.coords.latitude, Longitude : position.coords.longitude, Type : 0};
        var wordsToSend = ["testroom", JSON.stringify(update)];
     
        contosoChatHubProxy.invoke("sendSessionMessage", "testroom", JSON.stringify(eval(update))).done(function (result) {
            console.log(result);
        });
       
    }
}


function findParticipant(id) {
    for (var i = 0; i < _SessionParticipants.length; i++) {
        if (_SessionParticipants[i].SignalRId === id) return _SessionParticipants[i];
    }
    return null;
}

function ParticipantLocationReceived(messagestring) {

    if (nMap == null) return;
    var message = JSON.parse(messagestring);
    var _newPosition = new google.maps.LatLng(message.Latitude, message.Longitude);

    var existingMarker = findParticipant(message.SignalRId);
   
    if (existingMarker != null)
    {
        existingMarker.Marker.Visible=false;
        existingMarker.Marker.setMap(null);
        existingMarker.Marker = new google.maps.Marker({
            position: _newPosition,
            map: nMap,
            title: 'Hello World!'
        });
    }
    else
    {
        _SessionParticipants.push(
        {
            Marker : new google.maps.Marker({
                position: _newPosition,
                map: nMap,
                title: 'Hello World!'
            }),
            SignalRId : message.SignalRId,
            Username : "lol"
    });

  
}

}

function MyLocationReceived(position) {
   
    if (nMap == null) return;
    var _newPosition = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    if (_recentCordinates.length == 0) {
        nMap.panTo(_newPosition);
    }

    if (_myMarker != null)
    {
        _myMarker.Visible = false;
        _myMarker.setMap(null);
    }
   
    _myMarker = new google.maps.Marker({
        position: _newPosition,
        map: nMap,
        title: 'Hello World!'
    });
    _recentCordinates.push(_newPosition);
    SendLocationToInternet(position);
}


var connection = $.hubConnection("http://snuskelabben.cloudapp.net:8022");
var contosoChatHubProxy = connection.createHubProxy('gpsHub');
contosoChatHubProxy.on('LocationUpdate', function (message) {
    console.log(message);
    ParticipantLocationReceived(message);
});
connection.start()
    .done(function() {
        console.log('Now connected, connection ID=' + connection.id);

      



        mySignalRId = connection.id;
        contosoChatHubProxy.invoke("JoinSession", "testroom").done(function (result) {
            signalrConnected = true;
            //start own gps
            if (navigator.geolocation) {
                navigator.geolocation.watchPosition(MyLocationReceived);
            } else {
                console.log("Geolocation is not supported by this browser.");
            }
        });
    })
    .fail(function () { console.log('Could not connect'); });



