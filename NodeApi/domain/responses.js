function AbstractResponse (success, description, returnobject) {
    this.success = success;
    this.description = description;
    this.returnobject = returnobject;
};


AbstractResponse.prototype.fooBar = function () {

};
module.exports = AbstractResponse;