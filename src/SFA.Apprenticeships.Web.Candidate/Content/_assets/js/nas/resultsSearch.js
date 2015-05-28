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

    $(document).on('click', '.history #search-button', function (e) {
        e.preventDefault();
        $('#LocationType').val("NonNational");
        var searchQueryUrl = searchUrl + "?" + $('form').serialize() + "&" + GetSearchResultsDetailsValues();
        loadResults(searchQueryUrl, true);
    });

    $(document).on('click', '.history .update-results', function (e) {
        e.preventDefault();
        loadResults($(this).attr("href"), true);
        var queryStringParams = $.getQueryParameters($(this).attr("href"));
        $("#LocationType").val(queryStringParams.LocationType);
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
            $(window).trigger('resultsReloaded');
            history.pushState({ searchUrl: searchQueryUrl }, '', searchQueryUrl);

            $('.search-results').removeClass('disabled');
            $("#search-button").text('Update results').removeClass('disabled');
            $('#ajaxLoading').hide();

            if (scrollTop) {
                $(document).scrollTop(0);
            }
            Webtrends.multiTrack({ argsa: ["DCS.dcsqry", window.location.search] });
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


    $(document).on("apprenticeshipResultsUpdated", function(event, data) {
        //Any copy changes here also need reflected in the Apprenticeship Application Results.cshtml file.
        var resultMessage, nationalResultsMessage;

        if (data.totalLocalHits == 0)
        {
            resultMessage = "";
        }
        else if (data.totalLocalHits == 1)
        {
            if (data.locationType == "National")
            {
                resultMessage = "We've found <b class=\"bold-medium\">1</b> <a id='localLocationTypeLink' class='update-results' href=" + data.locationTypeLink + ">apprenticeship in your selected area</a>.";
            }
            else
            {
                resultMessage = "We've found <b class=\"bold-medium\">1</b> apprenticeship in your selected area.";
            }
        }
        else
        {
            if (data.locationType == "National")
            {
                resultMessage = "We've found <b class=\"bold-medium\">" + data.totalLocalHits + "</b> <a id='localLocationTypeLink' class='update-results' href=" + data.locationTypeLink + ">apprenticeships in your selected area</a>.";
            }
            else
            {
                resultMessage = "We've found <b class=\"bold-medium\">" + data.totalLocalHits +  "</b> apprenticeships in your selected area.";
            }
        }

        if (data.totalNationalHits == 0)
        {
            nationalResultsMessage = "";
        }
        else
        {
            var nationalResultsMessagePrefix = data.totalLocalHits == 0 ? "We've found" : "We've also found";

            if (data.totalNationalHits == 1)
            {

                if (data.locationType == "NonNational")
                {
                    nationalResultsMessage = nationalResultsMessagePrefix + " <a id='nationwideLocationTypeLink' class='update-results' href=" + data.locationTypeLink + ">1 apprenticeship with positions across England</a>.";
                }
                else
                {
                    nationalResultsMessage = nationalResultsMessagePrefix + " 1 apprenticeship with positions across England.";
                }
            }
            else
            {
                if (data.locationType == "NonNational")
                {
                    nationalResultsMessage = nationalResultsMessagePrefix + " <a id='nationwideLocationTypeLink' class='update-results' href=" + data.locationTypeLink + ">" + data.totalNationalHits + " apprenticeships with positions across England</a>.";
                }
                else
                {
                    nationalResultsMessage = nationalResultsMessagePrefix + " " + data.totalNationalHits + " apprenticeships with positions across England.";
                }
            }
        }
        $("#result-message").html(resultMessage);
        $("#national-results-message").html(nationalResultsMessage);

        if ($("#receiveSaveSearchAlert").length) {
            $("#receiveSaveSearchAlert").attr("href", data.saveSearchUrl);
        }
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