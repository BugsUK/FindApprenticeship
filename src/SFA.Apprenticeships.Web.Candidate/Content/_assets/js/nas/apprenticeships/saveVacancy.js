function initSavedVacancies (options) {
    var applicationStatuses = {
        Indeterminate: { },
        Unsaved: { unsaved: true, },
        Saved: { saved: true, },
        Draft: { draft: true },
        Submitting: { applied: true },
        Submitted: { applied: true },
        InProgress: { applied: true },
        Successful: { applied: true },
        Unsuccessful: { applied: true }
    };

    function updateSaveVacancyIcon($saveLink, vacancyStatus) {
        var $icon = $saveLink.children("i");

        $icon.toggleClass("fa-spinner fa-spin", !(vacancyStatus.unsaved || vacancyStatus.saved));
        $icon.toggleClass("fa-star", !vacancyStatus.unsaved);
        $icon.toggleClass("fa-star-o", !vacancyStatus.saved);
    }

    function updateSaveVacancyView (applicationStatus) {
        var $saveLink = $(this);
        var $resumeLink = $saveLink.siblings(".resume-link");
        var $appliedLabel = $saveLink.siblings(".applied-label");

        if (applicationStatus) {
            $saveLink.data("application-status", applicationStatus);
        } else {
            applicationStatus = $saveLink.data("application-status");
        }

        var vacancyStatus = applicationStatuses[applicationStatus];

        if (!vacancyStatus) {
            return;
        }

        $saveLink.toggleClass("hidden", !(vacancyStatus.unsaved || vacancyStatus.saved));
        $resumeLink.toggleClass("hidden", !vacancyStatus.draft);
        $appliedLabel.toggleClass("hidden", !vacancyStatus.applied);

        if (vacancyStatus.applied) {
            return;
        }

        updateSaveVacancyIcon($saveLink, vacancyStatus);

        var text = vacancyStatus.saved ? "Remove from saved" : "Save for later";

        if (options.title) {
            $saveLink.attr("title", text);
        } else {
            var $label = $saveLink.children(".save-vacancy-link-text");
            $label.text(text);
        }
    };

    $(".save-vacancy-link").on("click", function (e) {
        e.preventDefault();

        var $saveLink = $(this);
        var applicationStatus = $saveLink.data("application-status");
        var vacancyStatus = applicationStatuses[applicationStatus];

        if (!vacancyStatus) {
            return;
        }

        var vacancyId = parseInt($saveLink.data("vacancy-id"));

        var ajaxOptions = {
            type: vacancyStatus.unsaved ? "POST" : "DELETE",
            url: vacancyStatus.unsaved ? options.saveUrl : options.deleteUrl,
            data: {
                id: vacancyId
            }
        };

        updateSaveVacancyIcon($saveLink, applicationStatuses.Indeterminate);

        $.ajax(ajaxOptions)
            .done(function (result) {
                updateSaveVacancyView.call($saveLink, result.applicationStatus);
                if (vacancyStatus.unsaved) {
                    IncrementSavedAndDraftCount();
                    Webtrends.multiTrack({ argsa: ['DCS.dcsuri', '/apprenticeship/saved/' + vacancyId, 'WT.dl', '99', 'WT.ti', 'Apprenticeship Saved for Later'] });
                } else {
                    DecrementSavedAndDraftCount();
                    Webtrends.multiTrack({ argsa: ['DCS.dcsuri', '/apprenticeship/unsaved/' + vacancyId, 'WT.dl', '99', 'WT.ti', 'Apprenticeship UnSaved for Later'] });
                }
            })
            .fail(function () {
                // Best efforts: revert application status.
                updateSaveVacancyView.call($saveLink, applicationStatus);
            });

        $saveLink.blur();
    });

    // Initialise saved vacancy / resume application links.
    $(".save-vacancy-link").each(function () {
        updateSaveVacancyView.call(this);
    });
};