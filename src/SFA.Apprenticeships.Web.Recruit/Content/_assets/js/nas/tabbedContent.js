$('.tabbed-tab').attr('href', "#");

$('.tabbed-tab').on('click', function () {
    tabSelected($(this), true);
    return false;
});

$(window).on("popstate", function (e) {
    if (e.originalEvent.state !== null && e.originalEvent.state.tab) {
        tabSelected($('[tab="' + e.originalEvent.state.tab + '"]'), false, false);
    }
});

if (window.location.hash !== "") {
    tabSelected($('[tab="' + window.location.hash + '"]'), false, false);
} else {
    tabSelected($(".tabbed-tab.active").first(), false, true);
}

function tabSelected(tab, pushState, replaceState) {
    var tabId = tab.attr('tab');

    tab.addClass('active');
    $('.tabbed-tab').not($('[tab="' + tabId + '"]')).removeClass('active');

    if ($(tabId).length) {
        $(tabId).show();
        $('.tabbed-content').not(tabId).hide();
    } else {
        var tabClass = '.' + tabId.substr(1);
        $('.tabbed-element' + tabClass).show();
        $('.tabbed-element').not(tabClass).hide();
    }

    //Now do history.
    if (Modernizr.history && pushState) {
        history.pushState({ tab: tabId }, '', window.location.pathname + tabId);
    }

    if (Modernizr.history && replaceState) {
        history.replaceState({ tab: tabId }, '', window.location.pathname);
    }

    $(window).trigger("tabchanged", { tab: tabId });
}