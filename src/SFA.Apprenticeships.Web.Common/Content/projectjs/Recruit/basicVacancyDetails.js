$(function () {

    $(document).on('click', '#single-offline-application-url-button', function (e) {
        e.preventDefault();
        $('#OfflineVacancyType').val('SingleUrl');
        var form = $('form');
        var input = $("<input>").attr("type", "hidden").attr("name", "CreateVacancy").attr("id", "multiple-offline-application-urls-hidden").val("SingleOfflineApplicationUrl");
        form.append(input);
        postForm(form.serialize(), true);
    });

    $(document).on('click', '#multiple-offline-application-urls-button', function (e) {
        e.preventDefault();
        $('#OfflineVacancyType').val('MultiUrl');
        var form = $('form');
        var input = $("<input>").attr("type", "hidden").attr("name", "CreateVacancy").attr("id", "single-offline-application-url-hidden").val("MultipleOfflineApplicationUrls");
        form.append(input);
        postForm(form.serialize(), false);
    });

    function postForm(data, singleUrl) {

        $('#ajaxLoading').show();

        $.ajax({
            url: postUrl,
            method: 'POST',
            data: data
        }).done(function () {

            var validationSummary = $('#validation-summary-errors');

            var singleUrlPara = $('#single-offline-application-url-para');
            var multiUrlPara = $('#multiple-offline-application-urls-para');
            var singleUrlDiv = $('#single-offline-application-url-div');
            var multiUrlsDiv = $('#multiple-offline-application-urls-div');

            if (singleUrl) {
                validationSummary.find('a[href="#offlineapplicationurl"]').parent().show();
                validationSummary.find('a[href^="#locationaddresses"]').parent().hide();

                $('#multiple-offline-application-urls-hidden').remove();
                $('#multiple-offline-application-urls-button').text(multipleOfflineUrlsButtonText).removeClass('disabled');

                singleUrlPara.hide();
                multiUrlPara.show();
                singleUrlDiv.show();
                multiUrlsDiv.hide();
            }
            else {
                validationSummary.find('a[href="#offlineapplicationurl"]').parent().hide();
                validationSummary.find('a[href^="#locationaddresses"]').parent().show();

                $('#single-offline-application-url-hidden').remove();
                $('#single-offline-application-url-button').text(singleOfflineUrlButtonText).removeClass('disabled');

                singleUrlPara.show();
                multiUrlPara.hide();
                singleUrlDiv.hide();
                multiUrlsDiv.show();
            }

            if (validationSummary.find('a').length > 0) {
                if (validationSummary.find('li:not([style="display: none;"])').length > 0) {
                    validationSummary.show();
                } else {
                    validationSummary.hide();
                }
            }

        }).fail(function () {
            if (singleUrl) {
                $('#single-offline-application-url-hidden').remove();
                $('#single-offline-application-url-button').text(singleOfflineUrlButtonText).removeClass('disabled');
            }
            else {
                $('#multiple-offline-application-urls-hidden').remove();
                $('#multiple-offline-application-urls-button').text(multipleOfflineUrlsButtonText).removeClass('disabled');
            }
        }).always(function () {

        });
    }
});