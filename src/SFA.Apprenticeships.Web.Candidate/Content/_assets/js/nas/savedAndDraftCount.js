function UpdateSavedAndDraftCount(savedAndDraftCount) {
    var savedSearchTitle = $("#savedapplications-link");
    if (savedAndDraftCount != 0) {
        savedSearchTitle.text(savedAndDraftCount + " Saved");
    }
    savedSearchTitle.attr("saved-count", savedAndDraftCount);
}

function IncrementSavedAndDraftCount() {
    var savedSearchTitle = $("#savedapplications-link");
    var currentCount = parseInt(savedSearchTitle.attr("saved-count")) + 1;
    savedSearchTitle.text(currentCount + " Saved");
    savedSearchTitle.attr("saved-count", currentCount);
}

function DecrementSavedAndDraftCount() {
    var savedSearchTitle = $("#savedapplications-link");
    var currentCount = parseInt(savedSearchTitle.attr("saved-count")) - 1;

    if (currentCount == 0) {
        savedSearchTitle.text("");
    } else {
        savedSearchTitle.text(currentCount + " Saved");
    }
    savedSearchTitle.attr("saved-count", currentCount);
}