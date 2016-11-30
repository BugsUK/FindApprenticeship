$(function () {

    if (Modernizr.history) {
        history.replaceState({ searchUrl: location.href }, location.href);
    };

    $(document).on('change', '#provider-site', function () {
        var form = $('form[name="provider-site-form"]');
        var input = $("<input>").attr("type", "hidden").attr("name", "ChangeProviderSiteAction").val("ChangeProviderSite");
        form.append(input);
        loadResults(searchUrl, true, false, 'POST', form.serialize());
    });

    $(document).on('change', '#page-size', function () {
        var form = $('form[name="vacancy-search-form"]');
        var input = $("<input>").attr("type", "hidden").attr("name", "SearchVacanciesAction").val("SearchVacancies");
        form.append(input);
        loadResults(searchUrl, true, false, 'POST', form.serialize());
    });

    $(document).on('click', '#search-vacancies-button', function (e) {
        e.preventDefault();
        var form = $('form[name="vacancy-search-form"]');
        var input = $("<input>").attr("type", "hidden").attr("name", "SearchVacanciesAction").val("SearchVacancies");
        form.append(input);
        loadResults(searchUrl, true, false, 'POST', form.serialize());
    });

    $(document).on('click', '.page-navigation__btn:not(#page-size-container)', function (e) {
        e.preventDefault();
        var searchQueryUrl = $(this).attr('href');
        loadResults(searchQueryUrl, true, false, 'GET');
    });

    $(document).on('click', '.vacancy-filter', function (e) {
        e.preventDefault();
        var searchQueryUrl = $(this).attr('href');
        loadResults(searchQueryUrl, true, false, 'GET');
    });

    $(document).on('click', '.tabbed-tab', function (e) {
        e.preventDefault();
        var searchQueryUrl = $(this).attr('href');
        loadResults(searchQueryUrl, true, false, 'GET');
    });

    function loadResults(searchQueryUrl, addHistory, scrollTop, method, data) {

        $('.search-results').addClass('disabled');
        $('#ajaxLoading').show();

        $.ajax({
            url: searchQueryUrl,
            method: method,
            data: data
        }).done(function (response) {

            var main = $(response).find("#content");
            $("#content").html(main.html());
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