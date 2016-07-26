$(function () {
    var downloadButton = $('#DownloadReport');
    if (downloadButton.length) {
        var validateButton = $('#ValidateReportParameters');
        validateButton.prop("disabled", true);
        downloadButton.click();
        setTimeout(function () {
            validateButton.prop("disabled", false);
        }, 5000);
    }
});