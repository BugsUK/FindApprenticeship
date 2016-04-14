var autoSave = (function() {
    // settings is composed by:
    //  - formSelector: selector pointing to the form we want 
    //    to serialize and compare
    //  - timeout: timeout to reset the autoSave timer
    //  - postUrl: url to send the form
    var autoSaveTimeout = null,
        initialise = function(settings) {
            // settings.formSelector
            // settings.timeout
            // settings.postUrl

            function timeoutReset() {
                setTimeout(function() {
                    timeoutTrigger();
                }, autoSaveTimeout);
            }

            function timeoutTrigger() {
                $.ajax({
                    type: "POST",
                    url: settings.postUrl,
                    cache: false,
                    timeout: 30000,
                    // data: $("form").serialize()
                    data: $(settings.formSelector).serialize()
                });

                timeoutReset();
            }

            $(function() {
                autoSaveTimeout = settings.timeout;

                setTimeout(function() {
                    timeoutTrigger();
                }, autoSaveTimeout);
            });
        };

    return {
        initialise: initialise
    };
})();