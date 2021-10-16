jQuery.validator.addMethod("CheckFormatOfMobileNumber", function (value, element, param) {
    var otherPropId = $(element).data('val-mobilenumber');
    if (otherPropId) {
        var otherProp = $(otherPropId);
        if (otherProp) {
            var otherVal = otherProp.val();
            if (parseInt(otherVal) > parseInt(value)) {
                return true;
            }
            return false;
        }
    }
    return true;
});
jQuery.validator.unobtrusive.adapters.addBool("lowerthan");