$(function () {

    if (Modernizr.history) {
        history.replaceState({ searchUrl: location.href }, location.href);
    };

    $(document).on('change', '#role', function () {
        var form = $('form');
        var input = $("<input>").attr("type", "hidden").attr("name", "DashboardAction").val("ChangeRole");
        form.append(input);
        form.submit();
        //loadResults(searchUrl, true, false, 'POST', form.serialize());
    });

    $(document).on('change', '#regional-team', function () {
        var form = $('form');
        var input = $("<input>").attr("type", "hidden").attr("name", "DashboardAction").val("ChangeTeam");
        form.append(input);
        form.submit();
        //loadResults(searchUrl, true, false, 'POST', form.serialize());
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