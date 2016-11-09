$(function () {

    if (Modernizr.history) {
        history.replaceState({ searchUrl: location.href }, location.href);
    };

    $(document).on('change', '#page-size', function () {
        var form = $('form');
        loadResults(searchUrl, false, false, 'POST', form.serialize());
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

    $(document).on('click', '#search-candidates-button', function (e) {
        e.preventDefault();
        var form = $('form');
        loadResults(searchUrl, false, false, 'POST', form.serialize());
    });

    function loadResults(searchQueryUrl, addHistory, scrollTop, method, data) {

        $('.search-results').addClass('disabled');
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