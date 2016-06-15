module.exports = Object.freeze({
    getSession: "SELECT SignalRRoom from Session WHERE Shortname='%s'",
    createSession: "INSERT INTO [dbo].[Session] VALUES ('%s','%s', Getdate());",
    authUser: "SELECT Id, UserName, PhoneNumber, PasswordHash, PasswordSalt FROM Member WHERE UserName='%s'",
    getContacts: "SELECT Username, PhoneNumber FROM Member WHERE PhoneNumber IN (%s)",
    isregisteredApn: "SELECT Username, PhoneNumber, APNId, FCMId FROM Member WHERE PhoneNumber='%s' OR APNId='%s'",
    isregisteredFcm: "SELECT Username, PhoneNumber, APNId, FCMId FROM Member WHERE PhoneNumber='%s' OR FCMId='%s'",
    isregistered: "SELECT Username, PhoneNumber, APNId, FCMId FROM Member WHERE PhoneNumber='%s'",
    createAnonUser: "INSERT INTO Member VALUES('%s', NULL, NULL, NULL, '%s', '%s', '%s', '%s', 0, NULL, 0, 0, '%s', GetDate(), NULL, NULL);"
});