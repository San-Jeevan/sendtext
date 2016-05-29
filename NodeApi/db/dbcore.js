var sql = require('mssql');
var util = require('util');
var Moniker = require('moniker-native');
var Chance = require('chance');
var bcrypt = require('bcryptjs');
var config = require('../config.js');

sql.connect(config.sql.uri).then(function () {
	console.log("connected to SQL: " + config.sql.uri)
	//GET SESSION
	exports.getsession = function (shortname, callback) {
		if (shortname == null) callback(new Error('shortname is null'));
		new sql.Request().query(util.format("SELECT SignalRRoom from Session WHERE Shortname='%s'", shortname)).then(function (recordset) {
			callback(recordset);
		}).catch(function (err) {
			callback(err);
		});
	};
	
	
	//CREATE SESSION
	exports.createsession = function (callback) {
		var chance = new Chance();
		var shortname = Moniker.generator([Moniker.adjective, Moniker.noun]).choose();
		var guid = chance.guid();
		new sql.Request().query(util.format("INSERT INTO [dbo].[Session] VALUES ('%s','%s', Getdate());", guid, shortname)).then(function (recordset) {
			callback(shortname);
		}).catch(function (err) {
			callback(err);
		});
	};
	

	//AUTH USER
	exports.authenticateuser = function (user, password, callback) {
		new sql.Request().query(util.format("SELECT Id, UserName, PhoneNumber, PasswordHash, PasswordSalt FROM Member WHERE UserName='%s'",
			 user)).then(function (recordset) {
		    //if(recordset.length==0) callback(false, recordset);
			bcrypt.compare(password, recordset[0].PasswordHash, function (err, res) {
				callback(res, recordset);
			});
		}).catch(function (err) {
			callback(false, []);
		});
	};
		
	
	//CONTACTS WITH APP
	exports.getcontacts = function (contacts, callback) {
		var contactlist = contacts.join(", ")

		new sql.Request().query(util.format("SELECT Username, PhoneNumber FROM Member WHERE PhoneNumber IN (%s)",
			 contactlist)).then(function (recordset) {
			callback(recordset);	
		}).catch(function (err) {
			callback(err);
		});
	};
	
	
	//CREATE USER
	exports.createuser = function (callback) {
		bcrypt.genSalt(10, function (err, salt) {
			bcrypt.hash("heyhey", salt, function (err, hash) {
				var chance = new Chance();
				var email = chance.email();
				var pwhash = hash;
				var pwsalt = salt;
				var country = chance.country();
				var phone = chance.phone({ country: 'us', mobile: true, formatted: false });
				var fcm = chance.android_id();
				var username = chance.first();
				
				new sql.Request().query(util.format("INSERT INTO Member VALUES('%s', '%s', '%s', NULL, '%s', '%s', '%s', NULL, 0, NULL, 0, 0, '%s', GetDate(), null, null);",
			 email, pwhash, pwsalt, phone, country, fcm, username)).then(function (recordset) {
					new sql.Request().query(util.format("SELECT * from Member WHERE PhoneNumber='%s'",
			 phone)).then(function (recordsettwo) {
						callback(recordsettwo);
					}).catch(function (err) {
						callback(err);
					});
			
				}).catch(function (err) {
					callback(err);
				});
			});
		});
		
		
		
		
	};




}).catch(function (err) {
	console.log(err);
});;