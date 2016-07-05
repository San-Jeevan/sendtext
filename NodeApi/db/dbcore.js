var sql = require('mssql');
var util = require('util');
var Moniker = require('moniker-native');
var Chance = require('chance');
var bcrypt = require('bcryptjs');
var config = require('../config.js');
var sqlstrings = require('./queryStrings.js');
var AbstractResponse = require('../domain/responses.js');

sql.connect(config.sql.uri).then(function () {
    console.log("connected to SQL: " + config.sql.uri);
    //GET SESSION
    exports.getsession = function (shortname, callback) {
        if (shortname == null) callback(new Error('shortname is null'));
        new sql.Request().query(util.format(sqlstrings.getSession, shortname)).then(function (recordset) {
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
        new sql.Request().query(util.format(sqlstrings.createSession, guid, shortname)).then(function (recordset) {
            callback(shortname);
        }).catch(function (err) {
            callback(err);
        });
    };
    
  

    
    //CREATE ANON USER
    exports.createanonuser = function (params, callback) {
        var email = "";
        var country = params.Country;
        var phone = params.MobilePhone;
        var fcm = params.Fcm ? params.Fcm : "";
        var apn = params.Apn ? params.Apn : "";
        
        function createUser(callback){
            new sql.Request().query(util.format(sqlstrings.createAnonUser, phone, phone, country, fcm, apn, phone)).then(function (recordset) {
                new sql.Request().query(util.format("SELECT * from Member WHERE PhoneNumber='%s'", phone)).then(function (recordsettwo) {
                    var response = new AbstractResponse(true, "Anon user created", recordsettwo);
                    callback(response);
                }).catch(function (err) {
                    var response = new AbstractResponse(false, err.message, {});
                    callback(response);
                });
			
            }).catch(function (err) {
                var response = new AbstractResponse(false, err.message, {});
                callback(response);
            });
        }

        //deletes existing
        new sql.Request().query(util.format("SELECT * from Member WHERE PhoneNumber='%s' AND PasswordHash IS NULL", phone)).then(function (oldentry) {
            if (oldentry.length != 0) {
                new sql.Request().query(util.format("DELETE from Member WHERE Id='%s' AND PasswordHash IS NULL", oldentry[0].Id)).then(function (deleteresult) {
                    createUser(callback);
                }).catch(function (err) {
                    var response = new AbstractResponse(false, err.message, {});
                    callback(response);
                });
                
            }
            else createUser(callback);
        }).catch(function (err) {
            var response = new AbstractResponse(false, err.message, {});
            callback(response);
        });

    };

    
    //CREATE USER
    exports.createuser = function (params, callback) {
        bcrypt.genSalt(10, function (err, salt) {
            bcrypt.hash("heyhey", salt, function (err, hash) {
                var chance = new Chance();
                var email = chance.email();
                var country = chance.country();
                var phone = chance.phone({ country: 'us', mobile: true, formatted: false });
                var fcm = chance.android_id();
                var username = chance.first();
                new sql.Request().query(util.format("INSERT INTO Member VALUES('%s', '%s', NULL, NULL, '%s', '%s', '%s', NULL, 0, NULL, 0, 0, '%s', GetDate(), null, null);",
			    email, hash, phone, country, fcm, username)).then(function (recordset) {
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