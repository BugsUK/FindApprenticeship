function initSavedVacancies (options) {
    var vacancyStatuses = {
        Unsaved: {
            unsaved: true
        },
        Saved: {
            saved: true
        },
        Draft: {
            draft: true
        },
        ExpiredOrWithdrawn: {
            applied: true
        },
        Submitting: {
            applied: true
        },
        Submitted: {
            applied: true
        },
        InProgress: {
            applied: true
        },
        Successful: {
            applied: true
        },
        Unsuccessful: {
            applied: true
        }
    };

    function setSavedVacancyView (applicationStatus) {
        var $saveLink = $(this);
        var $resumeLink = $saveLink.siblings(".resume-link");
        var $appliedLabel = $saveLink.siblings(".applied-label");

        console.log("setSavedVacancyView: applicationStatus", applicationStatus);
        console.log("setSavedVacancyView: data-application-status", $saveLink.data("application-status"));

        if (applicationStatus) {
            $saveLink.data("application-status", applicationStatus);
        } else {
            applicationStatus = $saveLink.data("application-status");
        }

        var vacancyStatus = vacancyStatuses[applicationStatus];

        $saveLink.toggleClass("hidden", !(vacancyStatus.unsaved || vacancyStatus.saved));
        $resumeLink.toggleClass("hidden", !vacancyStatus.draft);
        $appliedLabel.toggleClass("hidden", !vacancyStatus.applied);

        if (vacancyStatus.applied) {
            return;
        }

        var $icon = $saveLink.children("i");

        $icon.toggleClass("fa-star", !vacancyStatus.unsaved);
        $icon.toggleClass("fa-star-o", !vacancyStatus.saved);

        var text = vacancyStatus.saved ? "Remove from saved" : "Save for later";

        if (options.title) {
            $saveLink.attr("title", text);
        } else {
            var label = $saveLink.children(".save-vacancy-link-text");
            
            label.text(text);
        }
    };

    $(".save-vacancy-link").on("click", function (e) {
        e.preventDefault();

        var $self = $(this);
        var save = $self.data("application-status") === "Unsaved";
        var vacancyId = parseInt($self.data("vacancy-id"));

        console.log("setSavedVacancyView: data-application-status", $self.data("application-status"));
        console.log("setSavedVacancyView: vacancyId", vacancyId);

        var ajaxOptions = {
            type: save ? "POST" : "DELETE",
            url: save ? options.saveUrl : options.deleteUrl,
            data: {
                id: vacancyId
            }
        };

        $.ajax(ajaxOptions)
            .done(function (result) {
                console.log("ajax: result.applicationStatus", result.applicationStatus);
                setSavedVacancyView.call($self, result.applicationStatus || "Unsaved");
            })
            .fail(function (error) {
                console.error("Failed to save vacancy:", error);
            });

        $self.blur();
    });

    // Initialise saved vacancy / resume application links.
    $(".save-vacancy-link").each(function () {
        setSavedVacancyView.call(this);
    });
};
