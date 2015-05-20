$(function () {

    if (Modernizr.history) {
        history.replaceState({ searchUrl: location.href }, location.href);
    };

    $("#Location").locationMatch({
        url: locationUrl,
        longitude: '#Longitude',
        latitude: '#Latitude',
        latlonhash: '#Hash'
    });

    $('.no-history #sort-results').change(function () {
        $('#SearchAction').val("Sort");
        $("form").submit();
    });

    $(document).on('change', '.history #sort-results', function () {
        $('#SearchAction').val("Sort");
        var searchQueryUrl = searchUrl + "?" + $('form').serialize() + "&" + GetSearchResultsDetailsValues();
        loadResults(searchQueryUrl);
    });

    $('.no-history #results-per-page').change(function () {
        $('#SearchAction').val("Sort");
        $("form").submit();
    });

    $(document).on('change', '.history #results-per-page', function () {
        $('#SearchAction').val("Sort");
        var searchQueryUrl = searchUrl + "?" + $('form').serialize() + "&" + GetSearchResultsDetailsValues();
        loadResults(searchQueryUrl);
    });

    $(document).on('click', '.history .page-navigation__btn', function (e) {
        e.preventDefault();
        var searchQueryUrl = $(this).attr('href');
        loadResults(searchQueryUrl, true);
    });

    $(window).on("popstate", function (e) {
        if (e.originalEvent.state !== null && e.originalEvent.state.searchUrl) {
            loadResults(e.originalEvent.state.searchUrl, true);
        }
    });

    $(document).on('click', '.no-history #search-button', function () {
        $('#LocationType').val("NonNational");
    });

    $(document).on('click', '.history #search-button', function () {
        $('#LocationType').val("NonNational");
        var searchQueryUrl = searchUrl + "?" + $('form').serialize() + "&" + GetSearchResultsDetailsValues();
        loadResults(searchQueryUrl, true);
    });

    function loadResults(searchQueryUrl, scrollTop) {

        $('.search-results').addClass('disabled');
        $('#ajaxLoading').show();

        $.ajax({
            url: searchQueryUrl,
            method: 'GET'
        }).done(function (response) {
            $("#pagedList").empty();
            $("#pagedList").html(response);
            $(window).trigger('googleMapsScriptLoaded');
            history.pushState({ searchUrl: searchQueryUrl }, '', searchQueryUrl);

            $('.search-results').removeClass('disabled');
            $('#ajaxLoading').hide();

            if (scrollTop) {
                $(document).scrollTop(0);
            }

            //TODO: Need to add matraxis multi track here to log page
        }).fail(function () {
        });
    }

    $(document).on('click', '#receiveSaveSearchAlert', function () {
        var $this = $(this),
            $href = $this.attr('href');

        //Append current results detail view settings to query string so they are saved.
        $href = $href + '&' + GetSearchResultsDetailsValues();
        $this.attr('href', $href);
    });

    $('#chooseDetails input').each(function () {
        var $this = $(this),
            $thisId = $this.attr('id');

        var $value = GetSearchResultsDetailsValue($thisId);

        if ($value != null) {
            var $currentlyChecked = $this.is(':checked');
            $this.prop("checked", $value);
            if ($currentlyChecked !== $value) {
                $('[data-show="' + $thisId + '"]').toggle();
            }
        }
    });

    //Write the new, complete cookie with the current view of the display settings
    SetSearchResultsDetailsCookieValue();

    $(document).on('change', '#chooseDetails input', function () {
        var $this = $(this),
            $thisId = $this.attr('id');

        $('[data-show="' + $thisId + '"]').toggle();

        //Write the new, complete cookie with the current view of the display settings
        SetSearchResultsDetailsCookieValue();
    });
});

jQuery.extend({
    getQueryParameters: function (str) {
        return (str || document.location.search).replace(/(^\?)/, '').split("&").map(function (n) { return n = n.split("="), this[n[0]] = n[1], this }.bind({}))[0];
    }
});

jQuery.extend({
    getCookieValues: function (str) {
        var cookieValues = {};

        var cookie = $.cookie(str);

        if (typeof cookie !== 'undefined' && cookie != null) {
            var cookieComponents = cookie.split("&");
            $.each(cookieComponents, function (index, value) {
                var cookieComponent = value.split('=');
                cookieValues[cookieComponent[0]] = cookieComponent[1];
            });
        }

        return cookieValues;
    }
});

function gup(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return null;
    else
        return results[1];
}

if (gup('FromSubmitted') == "true") {
    var categoryType = gup('Category').toLowerCase(),
        subCatType = gup('SubCategories'),
        $scrollingPanel = $('#sub-category-' + categoryType + '-list'),
        $subCatLi = $('#sub-category-' + subCatType).parent();

    $scrollingPanel.scrollTop($subCatLi.position().top);

}

function GetSearchResultsDetailsValue(searchResultDetail) {
    //Get URL param first
    var queryParams = $.getQueryParameters();
    var queryValue = queryParams[searchResultDetail];
    if (typeof queryValue !== 'undefined' && queryValue != null) {
        return queryValue.toLowerCase() === 'true';
    }

    //If that fails use cookie value
    var cookieValues = $.getCookieValues('NAS.SearchResultsDetails');
    var cookieValue = cookieValues[searchResultDetail];
    if (typeof cookieValue !== 'undefined' && cookieValue != null) {
        return cookieValue.toLowerCase() === 'true';
    }

    //Otherwise use defaults
    return null;
}

function GetSearchResultsDetailsValues() {
    //Assemble query string or cookie compatible value from inputs
    var detailsValue = $('#chooseDetails input').toArray().map(function (value) {
        return $(value).attr('id') + "=" + $(value).is(':checked');
    }).join("&");

    return detailsValue;
}

function SetSearchResultsDetailsCookieValue() {
    //Assemble cookie value from inputs
    var cookieValue = GetSearchResultsDetailsValues();

    //Break early if the cookie was empty. This happens if there are no results.
    //Saving and empty cookie would reset the user's settings
    if (!cookieValue)
        return;

    //Need the cookie not to be encoded so it's compatible with MVC
    $.cookie.raw = true;

    //Write the new, complete cookie with the current view of the display settings
    $.cookie('NAS.SearchResultsDetails', cookieValue);
}