$.validator.methods.number = function (value, element) {
    var regex = /^(\d*)(\,\d{1,2})?$/; //99999,99
    return regex.test(value);
}
