
var express = require('express');
var fs = require('fs');
var app = express();
var bodyParser = require('body-parser');

var config = require('./config.js');
var PORT = config.web.port;
var rediscore = require('./redis/rediscore.js');
var path = require('path');
var winston = require('winston');

var logDirectory = path.join(__dirname, 'log');
fs.existsSync(logDirectory) || fs.mkdirSync(logDirectory);



winston.add(winston.transports.File, {
    filename: path.join(logDirectory, 'accesslog.log'),
    json: false,
    maxsize: '10000',
    maxFiles: '100',
    timestamp: true,
    level: 'silly'
});
//winston.remove(winston.transports.Console);

app.listen(1234);
app.use(bodyParser());



// API ENDPOINTS
app.all('*', function (req, res, next) {
    res.set('Access-Control-Allow-Origin', '*');
    res.set('Access-Control-Allow-Credentials', true);
    res.set('Access-Control-Allow-Methods', 'GET, POST, DELETE, PUT');
    res.set('Access-Control-Allow-Headers', 'X-Requested-With, Content-Type');
    res.set("X-Powered-By", "IIS 7.0");
    res.set('Cache-Control', 'public, max-age=' + (3600000 / 1000));
    if ('OPTIONS' === req.method) return res.send(200);
    next();
});





app.post('/api/createSession', function (req, res) {
    winston.info('CREATESESSION');
    var parameters = req.body;
    if (parameters.length === 0) return res.json("");
    rediscore.startSession(parameters.data, function (response) {
        return res.json(response);
    });
   
});



app.get('/api/getSession/:code', function (req, res) {
    if (req.params.code.length === 0) return res.json("");
    rediscore.getSession(req.params.code, function (response) {
        return res.json(response);
    });
});

