//https://stackoverflow.com/questions/48066208/mvc-jquery-validation-does-not-accept-comma-as-decimal-separator
var originalNumber = $.validator.methods.number;
var wrappedNumber = function (value, element) {
    var fixedValue = parseFloat(value.toString().replace(",", "."));
    return originalNumber.call($.validator.prototype, fixedValue, element);     // Call function as if "this" is the original caller
};
$.validator.methods.number = wrappedNumber;


$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}

//$.validator.methods.number = function (value, element) {
//    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
//}

////(function ($) {
////    $.validator.methods.range = function (value, element, param) {
////        var globalizedValue = value.replace(",", ".");
////        return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
////    }

////    $.validator.methods.number = function (value, element) {
////        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
////    }

////    // deprecated, use $.validator.format instead
////    $.format = $.validator.format;
////}(jQuery));