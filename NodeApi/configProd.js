var config = {};

config.sql = {};
config.redis = {};
config.web = {};

config.sql.uri = "mssql://rsbetaling:Heyhey11@altil6oq0x.database.windows.net/gpsfrontdb?encrypt=true";

config.redis.host = 'localhost';
config.redis.port = 6379;
config.redis.password = 'heyhey11';
config.web.port = 3000;
config.redis.ttl = 600;

module.exports = config;