////https://stackoverflow.com/questions/16034986/knockout-combining-2-custom-bindings-for-digits-financial-data

//// Formatting Functions
//function formatWithComma(x, precision, seperator) {
//    if (x == undefined)
//        return null;
    
//    if ((x + "").split(".").length == 2)
//        precision = (x + "").split(".")[1].length;

//    var options = {
//        //precision: precision || 2,
//        //precision: precision || (x + "").split(".")[1].length,
//        precision: precision,
//        seperator: seperator || ','
//    }
//    //var formatted = parseFloat(x, 10).toFixed(options.precision);
//    var formatted = parseFloat(x).toFixed(options.precision);
//    var regex = new RegExp(
//            '^(\\d+)[^\\d](\\d{' + options.precision + '})$');
//    formatted = formatted.replace(
//        regex, '$1' + options.seperator + '$2');
//    return formatted;
//}

//function reverseFormat(x, precision, seperator) {
//    var options = {
//        precision: precision || 2,
//        seperator: seperator || ','
//    }
//    var regex = new RegExp(
//        '^(\\d+)[^\\d](\\d+)$');
//    var formatted = x.replace(regex, '$1.$2');
//    return parseFloat(formatted);
//}
//// END: Formatting Functions

///*

//Explanation of the RegExp used above:

//^(\\d+)[^\\d](\\d{' + options.precision + '})$$

//    ^        = pattern must match from the start of the string

//    (\\d+)   = 1 or more digits
//               - the brackets capture the matched string
//                 to be used in the replace function as $1

//    [^\\d]   = any character but a digit
//               - this includes letters, whitespace and
//                 punctutation (any character but 0-9)

//    (\\d{X}) = X number of digits (above we use options.precision)
//               - the brackets capture the matched string
//                 to be used in the replace function as $2

//    $        = pattern must match to the end of the string
//               - here we've used ^...$ so the pattern must
//                 match the entire string
//*/

//// Custom Binding - place this in a seperate .js file and reference it in your html
//ko.bindingHandlers.commaDecimalFormatter = {
//    init: function (element, valueAccessor) {

//        var observable = valueAccessor();

//        var interceptor = ko.computed({
//            read: function () {
//                return formatWithComma(observable());
//            },
//            write: function (newValue) {
//                observable(reverseFormat(newValue));
//            }
//        });

//        if (element.tagName == 'INPUT')
//            ko.applyBindingsToNode(element, {
//                value: interceptor
//            });
//        else
//            ko.applyBindingsToNode(element, {
//                text: interceptor
//            });
//        //ko.bindingHandlers.coreValidation.init(element, valueAccessor, allBindingsAccessor);
//    }
//}

//ko.bindingHandlers.numericGeneral = {
//    init: function (element, valueAccessor) {
//        $(element).on("keydown", function (event) {
//            // Allow: backspace, delete, tab, escape, and enter
//            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
//                // Allow: Ctrl+A
//                (event.keyCode == 65 && event.ctrlKey === true) ||
//                // Allow: . ,
//                (event.keyCode == 188 || event.keyCode == 190 || event.keyCode == 110) ||
//                // Allow: home, end, left, right
//                (event.keyCode >= 35 && event.keyCode <= 39)) {
//                // let it happen, don't do anything
//                return;
//            }
//            else {
//                // Ensure that it is a number and stop the keypress
//                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
//                    event.preventDefault();
//                }
//            }
//        });
//    }
//}