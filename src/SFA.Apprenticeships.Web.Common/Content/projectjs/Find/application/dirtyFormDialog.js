﻿var dirtyFormDialog = (function () {

    var hasToShowMessage,
        initialFormValue,
        linkClicked = false,
        // settings is composed by:
        //  - formSelector: selector pointing to the form we want 
        //    to serialize and compare
        //  - classToExclude: class of the elements that we don't want to check for dirty
        //  - timeout: timeout to reset the hasToShowMessage variable. When the timeout
        //    finishes, we assume that an event can happen due to session timeout
        //  - confirmationMessage: confirmation message to show when leaving the page
        initialise = function(settings) {
            initialFormValue = $(settings.formSelector).serialize();
            hasToShowMessage = true;
            setTimeout(function() {
                hasToShowMessage = false;
            }, settings.timeout);
            setBeforeUnloadEvent(settings.formSelector, settings.classToExclude, settings.confirmationMessage);
            setKeyUpEvent(settings.formSelector);
            setClickEvent(settings.classToExclude);
        },
        isFormDirty = function(formSelector) {
            var actualFormValue = $(formSelector).serialize();
            return initialFormValue !== actualFormValue;
        },
        resetDirtyForm = function(settings) {
            initialFormValue = $(settings.formSelector).serialize();
        },
        setBeforeUnloadEvent = function(formSelector, classToExclude, confirmationMessage) {
            $(window).on('beforeunload', function (e){
                if (!hasToShowMessage || linkClicked || $(e.target.activeElement).hasClass(classToExclude)) {
                    return;
                } else {
                    //https://developer.mozilla.org/en-US/docs/Web/Events/beforeunload

                    if (isFormDirty(formSelector)) {
                        (e || window.event).returnValue = confirmationMessage; //Gecko + IE
                        return confirmationMessage; //Webkit, Safari, Chrome etc.
                    }
                    return;
                }
            });
        },
        setKeyUpEvent = function(formSelector) {
            $('input').keypress(function(e) {
                if (e.which == 13 && !e.isDefaultPrevented()) {
                    hasToShowMessage = false;
                    $(formSelector).submit();
                    return false;
                }
            });
        },
        setClickEvent = function(classToHandle) {
            $('.' + classToHandle).on('click', function() {
                linkClicked = true;
            });
        };


    return {
        initialise: initialise,
        isFormDirty: isFormDirty,
        resetDirtyForm: resetDirtyForm
    };
})();