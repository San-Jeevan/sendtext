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


//https.createServer(options, app).listen(443, function () {
//    console.log('Started!');
//});


app.listen(1234);

app.use(bodyParser());
app.use(morgan());





// API ENDPOINTS
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





app.post('/api/createanonuser', function (req, res) {
    var parameters = req.body;
    if (parameters.length == 0) return res.json("");
    
    
    db.createanonuser(parameters, function (response) {
        return res.json(response);
    });
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




app.get('/api/getsession/:session', function (req, res) {
    db.getsession(req.params.session, function (response) {
        return res.json(response);
    });
	
});

app.use('/', express.static('www'), { maxAge: 3600000 });



app.use('/:sessionid', express.static('www'), { maxAge: 3600000 });
