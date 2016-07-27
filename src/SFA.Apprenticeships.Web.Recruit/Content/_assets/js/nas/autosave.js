var autoSave = (function() {
    // settings is composed by:
    //  - formSelector: selector pointing to the form we want 
    //    to serialize and compare
    //  - timeout: timeout to reset the autoSave timer
    //  - postUrl: url to send the form
    var autoSaveTimeout = null,
        initialise = function(settings) {

            function timeoutReset() {
                setTimeout(function() {
                    timeoutTrigger();
                }, autoSaveTimeout);
            }

            function saveForm() {
                $.ajax({
                    type: "POST",
                    url: settings.postUrl,
                    cache: false,
                    timeout: 30000,
                    data: $(settings.formSelector).serialize()
                });
            }

            function timeoutTrigger() {
                saveForm();
                timeoutReset();
            }

            $(function() {
                autoSaveTimeout = settings.timeout;
                timeoutReset();
            });

            $(window).on('beforeunload', function (e) {
                saveForm();
                return;
            });
        };

    return {
        initialise: initialise
    };
})();