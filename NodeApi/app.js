var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var morgan = require('morgan');
var db = require('./db/dbcore.js');
var async = require('async');
var config = require('./config.js');
var PORT = config.web.port;
var auth = require('./config/auth');
var https = require('https');
var path = require('path');
var fs = require('fs');
var apn = require('apn');



var options = {
    key  : fs.readFileSync(path.join(__dirname, '2_www.gpsfix.io.key')),
    cert : fs.readFileSync(path.join(__dirname, '2_www.gpsfix.io.crt'))
};


https.createServer(options, app).listen(443, function () {
    console.log('Started!');
});

app.set('trust proxy', '162.158.222.62')
app.listen(80);

app.use(bodyParser());
app.use(morgan());



// APN

var apnoptions = {
    pfx: 'APNcert.p12',
    passphrase: 'heyhey',
    gateway: 'gateway.sandbox.push.apple.com',
    production: false
};
var apnConnection = new apn.Connection(apnoptions);

apnConnection.addListener('timeout', function (err) {
    console.log('timeout', err);
});

apnConnection.addListener('connected', function (openSockets) {
    console.log('connected', openSockets);
});

apnConnection.addListener('error', console.error);
apnConnection.addListener('socketError', console.error);



app.all('*', function (req, res, next) {
    res.set('Access-Control-Allow-Origin', '*');
    res.set('Access-Control-Allow-Credentials', true);
    res.set('Access-Control-Allow-Methods', 'GET, POST, DELETE, PUT');
    res.set('Access-Control-Allow-Headers', 'X-Requested-With, Content-Type');
    res.set("X-Powered-By", "IIS 7.0");
    res.set('Cache-Control', 'public, max-age=' + (3600000 / 1000));
    if ('OPTIONS' == req.method) return res.send(200);
    next();
});





app.get('/api/sendtoken/:token', function (req, res) {
    var token = req.params.token;
    if (token == null) return res.json('Empty token');
    var myDevice = new apn.Device(token);
    
    var note = new apn.Notification();
    
    note.expiry = Math.floor(Date.now() / 1000) + 3600; // Expires 1 hour from now.
    note.alert = "You have a new message";
    note.payload = { 'messageFrom': 'Caroline' };
    
    apnConnection.pushNotification(note, myDevice);
    return res.json(note);
});




app.get('/about', function (req, res) {
    return res.json('hey');
});



app.post('/api/getcontacts', function (req, res) {
    var contacts = req.body;
    if (contacts.length == 0) return res.json("");
    
    //check that no invalid numbers are in the list
    async.each(contacts, function (number, callback) {
        if (!(/^\d+$/.test(number))) callback(number + " is not valid");
        callback();
    }, function (err) {
        if (err != null) res.json(err);
    });
    
    
    db.getcontacts(contacts, function (response) {
        return res.json(response);
    });
});


app.post('/api/signin', function (req, res) {
    db.authenticateuser(req.body.user, req.body.pass, function (result, authuser) {
        if (result) {
            var userData = authuser[0];
            delete userData.PasswordHash;
            delete userData.PasswordSalt;
            auth.createAndStoreToken(userData, 60 * 60 * 48, function (err, token) {
                if (err) {
                    return res.send(400);
                }
                return res.send(200, token);
            });
        }
        else res.send(200, "wrong user/pw");
    });

});


app.get('/api/createsession', auth.verify, function (req, res) {
    if (req._user) {
        db.createsession(function (response) {
            return res.json(response);
        });
    }
});



app.get('/api/createuser', function (req, res) {
    db.createuser(function (response) {
        return res.json(response);
    });
});

app.get('/api/expire', function (req, res) {
    auth.expireToken(req.headers, function (err, success) {
        if (err) {
            console.log(err);
            return res.send(401);
        }
        
        if (success) res.send(200);
        else res.send(401);
    });
});

app.get('/api/getsession/:session', function (req, res) {
    db.getsession(req.params.session, function (response) {
        return res.json(response);
    });
	
});

app.use('/', express.static('www'), { maxAge: 3600000 });



app.use('/:sessionid', express.static('www'), { maxAge: 3600000 });
