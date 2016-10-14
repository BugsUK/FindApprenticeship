$(function () {

    if (Modernizr.history) {
        history.replaceState({ searchUrl: location.href }, location.href);
    };

    $(document).on('click', '#single-offline-application-url-button', function (e) {
        e.preventDefault();
        var form = $('form');
        var input = $("<input>").attr("type", "hidden").attr("name", "CreateVacancy").val("SingleOfflineApplicationUrl");
        form.append(input);
        loadResults(searchUrl, false, false, 'POST', form.serialize());
    });

    $(document).on('click', '#multiple-offline-application-urls-button', function (e) {
        e.preventDefault();
        var form = $('form');
        var input = $("<input>").attr("type", "hidden").attr("name", "CreateVacancy").val("MultipleOfflineApplicationUrls");
        form.append(input);
        loadResults(searchUrl, false, false, 'POST', form.serialize());
    });

    function loadResults(searchQueryUrl, addHistory, scrollTop, method, data) {

        $('#ajaxLoading').show();

        $.ajax({
            url: searchQueryUrl,
            method: method,
            data: data
        }).done(function (response) {

            var main = $(response).find("#main");
            $("#main").html(main.html());
            if (addHistory) {
                history.pushState({ searchUrl: searchQueryUrl }, '', searchQueryUrl);
            }
            if (scrollTop) {
                $(document).scrollTop(0);
            }
        }).fail(function () {
        });
    }
});