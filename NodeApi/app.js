var express = require('express');
var app = express();
var bodyParser = require('body-parser'); 
var morgan = require('morgan'); 
var db = require('./db/dbcore.js');
var async = require('async');
var config = require('./config.js');

var PORT = config.web.port;
var auth = require('./config/auth');


app.listen(PORT);
app.use(bodyParser());
app.use(morgan());


app.all('*', function(req, res, next) {
	res.set('Access-Control-Allow-Origin', '*');
	res.set('Access-Control-Allow-Credentials', true);
	res.set('Access-Control-Allow-Methods', 'GET, POST, DELETE, PUT');
	res.set('Access-Control-Allow-Headers', 'X-Requested-With, Content-Type');
	res.set("X-Powered-By", "IIS 8.0");
	if ('OPTIONS' == req.method) return res.send(200);
	next();
});


app.get('/', function (req, res) {
	return res.send(200,'Nothing to see here :)');
});


app.get('/about', function (req, res) {
		return res.json('hey');
});



app.post('/getcontacts', function (req, res) {
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


app.post('/signin', function(req, res) {
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


app.get('/createsession', auth.verify, function (req, res) {
	if (req._user) {
		db.createsession(function (response) {
			return res.json(response);
		});
	}
});



app.get('/createuser', function (req, res) {
		db.createuser(function (response) {
			return res.json(response);
		});
});

app.get('/expire', function(req, res) {
	auth.expireToken(req.headers, function(err, success) {
		if (err) {
			console.log(err);
			return res.send(401);
		}

		if (success) res.send(200);
		else res.send(401);
	});
});

app.get('/getsession/:session', function (req, res) {
	db.getsession(req.params.session, function (response) {
		return res.json(response);
	});
	
});
