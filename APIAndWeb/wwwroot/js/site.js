
StatusType = {
    Connected: 0,
    Disconnected: 1
}

UpdateType = {
    GpsUpdate: 0,
    Message: 1,
    StatusUpdate: 2
}

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


function StatusUpdate(message) {
    if (message.Status === StatusType.Disconnected) {
        for (var i = 0; i < _SessionParticipants.length; i++) {
            if (_SessionParticipants[i].SignalRId === message.SignalRId) {
                var existingMarker = _SessionParticipants[i];
                if (existingMarker != null) {
                    existingMarker.Marker.Visible = false;
                    existingMarker.Marker.setMap(null);
                }
                _SessionParticipants.splice(i, 1);
            }
        }
    
    }
}

function ParticipantLocationReceived(messagestring) {

    if (nMap == null) return;
    var message = JSON.parse(messagestring);
    if (message.Type === UpdateType.StatusUpdate) {
        StatusUpdate(message);
        return;
    }
  
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

        //ZOOM into all the participants
        var bounds = new google.maps.LatLngBounds();
        if (_myMarker != null) bounds.extend(_myMarker.position);
        for (var i = 0; i < _SessionParticipants.length; i++) {
            bounds.extend(_SessionParticipants[i].Marker.position);
        }
        nMap.fitBounds(bounds);

}

}

function MyLocationReceived(position) {
   
    if (nMap == null) return;
    var _newPosition = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    if (_recentCordinates.length == 0) {
        var bounds = new google.maps.LatLngBounds();
        bounds.extend(_newPosition);
        for (var i = 0; i < _SessionParticipants.length; i++) {
            bounds.extend(_SessionParticipants[i].Marker.position);
        }
        nMap.fitBounds(bounds);
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


//var connection = $.hubConnection("http://localhost:8022");
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



