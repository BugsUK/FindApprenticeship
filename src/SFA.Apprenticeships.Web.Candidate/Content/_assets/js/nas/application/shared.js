(function () {
    var sharedMessages = {
        yearMustBeAfterFromYear: 'Year finished must be after year started'
    };

    // ReSharper disable once UnusedLocals
    window.MonthOfTheYear = function (monthName, monthNo) {
        var self = this;

        self.monthName = monthName;
        self.monthNumber = monthNo;
    };

    ko.validation.rules['mustBeGreaterThanOrEqual'] = {
        validator: function (val, otherVal) {
            return val === otherVal || val > otherVal;
        },
        message: sharedMessages.yearMustBeAfterFromYear
    };

    ko.validation.registerExtenders();

    // Change this to modify where the vertical bar is placed
    ko.bindingHandlers.parentvalElement = {
        // ReSharper disable once UnusedParameter
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var valueIsValid = valueAccessor().isValid();
            if (!valueIsValid && viewModel.isAnyMessageShown()) {
                //adds the vertical bar to the input element when it is invalid
                $(element).addClass("input-validation-error");
            }
            else {
                //removes the vertical bar when valid
                $(element).removeClass("input-validation-error");
            }
        }
    };

    ko.validation.insertValidationMessage = function (element) {
        var span = document.createElement('span');

        span.className = "field-validation-error";
        $(span).attr('aria-live', 'polite');

        var inputFormControlParent = $(element).closest(".form-control").parent();

        if (inputFormControlParent.length > 0) {
            $(span).insertAfter(inputFormControlParent);
        } else {
            element.parentNode.insertBefore(span, element.nextSibling);
        }

        return span;
    };

    ko.validation.rules.pattern.message = 'Invalid.';

    ko.validation.configure({
        decorateElement: true,
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        errorClass: 'input-validation-error',
        errorElementClass: 'input-validation-error'
    });
}());