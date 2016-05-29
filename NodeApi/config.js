var config = {};

config.sql = {};
config.redis = {};
config.web = {};

config.sql.uri = "mssql://admin:heyhey@JEEVANPC22/FULLSQL/gpsfix";

config.redis.host = 'localhost';
config.redis.port = 6379;
config.redis.password = 'heyhey11';
config.web.port = 3000;

module.exports = config;