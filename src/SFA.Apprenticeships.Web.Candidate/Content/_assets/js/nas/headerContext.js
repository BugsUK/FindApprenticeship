var blackHeaderHeight = $('.global-header').outerHeight(),
    fixedContainerHeight = $('.fixed-container').outerHeight(),
    heightOfHeader = blackHeaderHeight + fixedContainerHeight,
    lastScrollTop = 0,
    delta = 5;

function UpdateApplicationStatusChangeCount(applicationStatusChangeCount) {
    $("#dashUpdatesNumber").text(" (" + applicationStatusChangeCount + ")");
}

function UpdateSavedAndDraftCount(savedAndDraftCount) {
    var savedSearchTitle = $("#savedapplications-link");
    if (savedAndDraftCount != 0) {
        savedSearchTitle.text(savedAndDraftCount + " Saved");
    }
    savedSearchTitle.attr("saved-count", savedAndDraftCount);
}

function IncrementSavedAndDraftCount() {
    var savedSearchTitle = $("#savedapplications-link");
    var currentCount = parseInt(savedSearchTitle.attr("saved-count") || 0) + 1;
    savedSearchTitle.text(currentCount + " Saved");
    savedSearchTitle.attr("saved-count", currentCount);

    if ($(window).scrollTop() > heightOfHeader) {
        addFixedHeader();
    }
}

function DecrementSavedAndDraftCount() {
    var savedSearchTitle = $("#savedapplications-link");
    var currentCount = Math.max(0, parseInt(savedSearchTitle.attr("saved-count") || 0) - 1);

    if (currentCount == 0) {
        savedSearchTitle.text("");
    } else {
        savedSearchTitle.text(currentCount + " Saved");
        if ($(window).scrollTop() > heightOfHeader) {
            addFixedHeader();
        }
    }
    savedSearchTitle.attr("saved-count", currentCount);
}

$(window).on('scroll', removeFixedHeader);

function showFixedHeader() {
    var nowScrollTop = $(window).scrollTop();
    if (Math.abs(lastScrollTop - nowScrollTop) >= delta) {
        if (nowScrollTop > lastScrollTop) {
            removeFixedHeader();

        } else {

            if (nowScrollTop > heightOfHeader) {
                addFixedHeader();

            } else {
                removeFixedHeader();

            }
        }
        lastScrollTop = nowScrollTop;
    }
}

function removeFixedHeader() {
    $('.fixed-container').removeClass('fixed-header');
    $('.content-container').css('padding-top', '0');
}

function addFixedHeader() {
    $('.fixed-container').addClass('fixed-header');
    $('.content-container').css('padding-top', fixedContainerHeight + 'px');
}

