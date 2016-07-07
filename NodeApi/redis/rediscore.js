var config = require('../config.js');
var AbstractResponse = require('../domain/responses.js');
var redis = require('redis');
var redisClient = redis.createClient(config.redis.port, config.redis.host);
var timeToLive = config.redis.ttl;
redisClient.auth(config.redis.password);

redisClient.on('error', function (err) {
    throw err;
});

redisClient.on('connect', function (err) {
    console.log("connected to redis");
});



function randomIntByModulo(min, max) {
    var i = (Math.random() * 32768) >>> 0;
    return (i % (min - max)) + min;
}


/*
* Stores a token with user data for a ttl period of time
* token: String - Token used as the key in redis 
* data: Object - value stored with the token 
* ttl: Number - Time to Live in seconds (default: 24Hours)
* callback: Function
*/
exports.setTokenWithData = function (token, data, ttl, callback) {
    if (token == null) throw new Error('Token is null');
    if (data != null && typeof data !== 'object') throw new Error('data is not an Object');

    var userData = data || {};
    userData._ts = new Date();

    var timeToLive = ttl || 3600;
    if (timeToLive != null && typeof timeToLive !== 'number') throw new Error('TimeToLive is not a Number');


    redisClient.setex(token, timeToLive, JSON.stringify(userData), function (err, reply) {
        if (err) callback(err);

        if (reply) {
            callback(null, true);
        } else {
            callback(new Error('Token not set in redis'));
        }
    });

};

/*
* Gets the associated data of the token.
* token: String - token used as the key in redis
* callback: Function - returns data
*/
exports.getDataByToken = function (token, callback) {
    if (token == null) callback(new Error('Token is null'));

    redisClient.get(token, function (err, userData) {
        if (err) callback(err);

        if (userData != null) callback(null, JSON.parse(userData));
        else callback(new Error('Token Not Found'));
    });
};






//STARTSESSION
exports.startSession = function (data, callback) {
    var response;
    if (data == null) {
        response = new AbstractResponse(false, "Sending text is empty", null);
        callback(response);
    }

    if (data != null && typeof data !== 'string') {
        response = new AbstractResponse(false, "Malformed request. Attempt logged!", null);
        callback(response);
    }
  
    if (timeToLive != null && typeof timeToLive !== 'number') throw new Error('TimeToLive is not a Number');
    var secretKey = randomIntByModulo(100000, 999999);
    redisClient.setex(secretKey, timeToLive, data, function (err, reply) {
        if (err) callback(new AbstractResponse(false, "Error storing key in REDIS", err));
        if (reply) {
            callback(new AbstractResponse(true, reply, secretKey));
        } else {
            callback(new AbstractResponse(false, "Error storing key in REDIS", null));
        }
    });

};



//GETSESSION
exports.getSession = function (secretkey, callback) {
    var response;
    secretkey = secretkey.replace(/ /g, "");

    if (secretkey == null || secretkey.length!==6) {
        response = new AbstractResponse(false, "Code is empty or not 6 digits", null);
        callback(response);
    }

    if (secretkey != null && typeof secretkey !== 'string') {
        response = new AbstractResponse(false, "Malformed request. Attempt logged!", null);
        callback(response);
    }

    redisClient.get(secretkey, function (err, userData) {
        if (err) callback(new AbstractResponse(false, "Could not find code", null));

        if (userData != null) callback(new AbstractResponse(true, "", userData));
        else callback(new AbstractResponse(false, "Could not find code", null));
    });
};
