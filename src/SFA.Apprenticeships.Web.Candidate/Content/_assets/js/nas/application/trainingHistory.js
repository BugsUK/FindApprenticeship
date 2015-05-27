(function () {
    var trainingHistoryMessages = {
        providerRequired: "Please enter provider name",
        providerExceedsFiftyCharacters: "Provider name can't exceed 50 characters",
        providerContainsInvalidCharacters: "Provider name can't contain invalid characters, eg '/'",
        courseTitleRequired: "Please enter course title",
        courseTitleExceedsFiftyCharacters: "Course title can't exceed 50 characters",
        courseTitleContainsInvalidCharacters: "Course title can't contain invalid characters, eg '/'",
        fromMonthRequired: "Please enter month started",
        fromYearRequired: "Please enter year started",
        fromYearMustBeNumeric: "Year started must contain 4 digits, eg 1990",
        fromYearMustNotBeInFuture: "Year started can't be in the future",
        yearMustBeARange: "Year must be 4 digits, between " + (new Date().getFullYear() - 100) + " and " + (new Date().getFullYear()),
        toMonthRequired: "Please enter month you finished",
        toYearRequired: "Please enter year finished",
        toYearMustBeNumeric: "Year finished must contain 4 digits, eg 1990",
        toYearMustBeAfterFromYear: 'Year finished must be after year started',
        yearMustBeAfter: "Year must be 4 digits, and not before 1915",
        dateFinishedMustBeAfterDateStarted: "Date finished must be after date started"
    };

    var trainingHistoryItemModel = function(itemProvider, itemCourseTitle, itemFromMonth, itemFromYear, itemToMonth, itemToYear, itemCurrentYear, itemRegex, itemYearRegexPattern) {
        var self = this;

        self.itemRegexPattern = ko.observable(itemRegex);
        self.itemYearRegexPattern = ko.observable(itemYearRegexPattern);

        self.itemProvider = ko.observable(itemProvider).extend({
            required: { message: trainingHistoryMessages.providerRequired }
        }).extend({
            maxLength: {
                message: trainingHistoryMessages.providerExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: trainingHistoryMessages.providerContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.itemCourseTitle = ko.observable(itemCourseTitle).extend({
            required: { message: trainingHistoryMessages.courseTitleRequired }
        }).extend({
            maxLength: {
                message: trainingHistoryMessages.courseTitleExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: trainingHistoryMessages.courseTitleContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.itemCurrentYear = ko.observable(itemCurrentYear);

        self.itemFromMonth = ko.observable(itemFromMonth).extend({
            required: { message: trainingHistoryMessages.fromMonthRequired }
        });

        self.itemFromYear = ko.observable(itemFromYear).extend({
            required: { message: trainingHistoryMessages.fromYearRequired }
        }).extend({
            number: {
                message: trainingHistoryMessages.fromYearMustBeNumeric
            }
        }).extend({
            min: {
                message: trainingHistoryMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            max: {
                message: trainingHistoryMessages.fromYearMustNotBeInFuture,
                params: self.itemCurrentYear
            }
        });

        self.itemToYear = ko.observable(itemToYear).extend({
            required: { message: trainingHistoryMessages.toYearRequired }
        }).extend({
            number: {
                message: trainingHistoryMessages.toYearMustBeNumeric
            }
        }).extend({
            min: {
                message: trainingHistoryMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            validation: {
                validator: function(val, fromYearValue) {
                    return val >= fromYearValue;
                },
                message: trainingHistoryMessages.toYearMustBeAfterFromYear,
                params: self.fromYear
            }
        });

        self.itemToMonth = ko.observable(itemToMonth).extend({
            required: { message: trainingHistoryMessages.toMonthRequired }
        }).extend({
            validation: {
                validator: function(val, fromMonthValue) {
                    return val >= fromMonthValue;
                },
                message: trainingHistoryMessages.dateFinishedMustBeAfterDateStarted,
                params: self.itemFromMonth,
                onlyIf: function() {
                    return (self.itemFromYear() === self.itemToYear());
                }
            }
        });

        self.readOnly = ko.observable("readonly");
        self.showEditButton = ko.observable(true);
        self.toItemDateReadonly = ko.observable(undefined);
        self.itemErrors = ko.validation.group(self);
    };

    var trainingHistoryViewModel = function () {
        var self = this;

        // TODO: get this from config too.
        self.months = ko.observableArray([
            new window.MonthOfTheYear('Jan', 1),
            new window.MonthOfTheYear('Feb', 2),
            new window.MonthOfTheYear('Mar', 3),
            new window.MonthOfTheYear('Apr', 4),
            new window.MonthOfTheYear('May', 5),
            new window.MonthOfTheYear('June', 6),
            new window.MonthOfTheYear('July', 7),
            new window.MonthOfTheYear('Aug', 8),
            new window.MonthOfTheYear('Sept', 9),
            new window.MonthOfTheYear('Oct', 10),
            new window.MonthOfTheYear('Nov', 11),
            new window.MonthOfTheYear('Dec', 12)
        ]);

        self.hasTrainingHistory = ko.observable(undefined);
        self.hasNoTrainingHistory = ko.observable(undefined);
        self.showTrainingHistory = ko.observable(false);

        self.trainingHistoryStatus = ko.computed(function () {
            return self.showTrainingHistory() ? "block" : "none";
        }, self);

        self.regexPattern = ko.observable();
        self.yearRegexPattern = ko.observable();

        self.provider = ko.observable().extend({
            required: { message: trainingHistoryMessages.providerRequired }
        }).extend({
            maxLength: {
                message: trainingHistoryMessages.providerExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: trainingHistoryMessages.providerContainsInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.courseTitle = ko.observable().extend({
            required: { message: trainingHistoryMessages.courseTitleRequired }
        }).extend({
            maxLength: {
                message: trainingHistoryMessages.courseTitleExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: trainingHistoryMessages.courseTitleContainsInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.currentYear = ko.observable();

        self.fromMonth = ko.observable().extend({
            required: { message: trainingHistoryMessages.fromMonthRequired }
        });

        self.fromYear = ko.observable().extend({
            required: { message: trainingHistoryMessages.fromYearRequired }
        }).extend({
            number: {
                message: trainingHistoryMessages.fromYearMustBeNumeric
            }
        }).extend({
            max: {
                message: trainingHistoryMessages.fromYearMustNotBeInFuture,
                params: self.currentYear
            }
        }).extend({
            min: {
                message: trainingHistoryMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        });

        self.toMonth = ko.observable().extend({
            required: { message: trainingHistoryMessages.toMonthRequired }
        }).extend({
            validation: {
                validator: function (val, fromMonthValue) {
                    return val >= fromMonthValue;
                },
                message: trainingHistoryMessages.dateFinishedMustBeAfterDateStarted,
                params: self.fromMonth,
                onlyIf: function () {
                    return (self.fromYear() === self.toYear());
                }
            }
        });

        self.toYear = ko.observable().extend({
            required: { message: trainingHistoryMessages.toYearRequired }
        }).extend({
            number: { message: trainingHistoryMessages.toYearMustBeNumeric }
        }).extend({
            min: {
                message: trainingHistoryMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            validation: {
                validator: function (val, fromYearValue) {
                    return val >= fromYearValue;
                },
                message: trainingHistoryMessages.toYearMustBeAfterFromYear,
                params: self.fromYear
            }
        });

        self.trainingHistoryItems = ko.observableArray();

        self.errors = ko.validation.group(self);

        self.addTrainingHistory = function () {
            if (self.errors().length === 0) {
                var model = new trainingHistoryItemModel(
                    self.provider(),
                    self.courseTitle(),
                    self.fromMonth(),
                    self.fromYear(),
                    self.toMonth(),
                    self.toYear(),
                    self.currentYear(),
                    self.regexPattern(),
                    self.yearRegexPattern());

                self.trainingHistoryItems.push(model);

                self.provider("");
                self.courseTitle("");
                self.fromYear(null);
                self.toYear(null);

                self.errors.showAllMessages(false);

                $('#trainingHistoryAddConfirmText').text("Training course has been added to table below");

            } else {
                self.errors.showAllMessages();
                $('#trainingHistoryAddConfirmText').text("There has been a problem adding training course, check you've entered all details correctly");
            }

        };

        self.editTrainingHistory = function (trainingHistory) {
            trainingHistory.readOnly(undefined);
            trainingHistory.showEditButton(false);
        };

        self.saveTrainingHistory = function (trainingHistory) {
            if (trainingHistory.itemErrors().length === 0) {
                trainingHistory.readOnly('readonly');
                trainingHistory.showEditButton(true);
            } else {
                trainingHistory.itemErrors.showAllMessages();
            }
        };

        self.removeTrainingHistory = function (trainingHistory) {
            self.trainingHistoryItems.remove(trainingHistory);
        };

        self.checkHasNoTrainingHistory = function () {
            self.showTrainingHistory(false);
            self.hasTrainingHistory(undefined);
            self.hasNoTrainingHistory("checked");
        };

        self.getTrainingHistoryItems = function (data) {
            $(data).each(function (index, item) {
                var model = new trainingHistoryItemModel(
                    item.Provider,
                    item.CourseTitle,
                    item.FromMonth,
                    item.FromYear,
                    item.ToMonth,
                    item.ToYear,
                    self.currentYear(),
                    self.regexPattern(),
                    self.yearRegexPattern());

                self.trainingHistoryItems.push(model);
            });

            if (self.trainingHistoryItems().length > 0) {
                self.showTrainingHistory(true);
                self.hasTrainingHistory("checked");
                self.hasNoTrainingHistory(undefined);
            } else {
                self.checkHasNoTrainingHistory();
            }

        };

    };

    var viewModel = new trainingHistoryViewModel();

    if (window.getCurrentYear()) {
        viewModel.currentYear(window.getCurrentYear());
    }

    if (window.getWhiteListRegex()) {
        viewModel.regexPattern(window.getWhiteListRegex());
    }

    if (window.getYearRegex()) {
        viewModel.yearRegexPattern(window.getYearRegex());
    }

    if (window.getTrainingHistoryData()) {
        viewModel.getTrainingHistoryItems(window.getTrainingHistoryData());
    } else {
        viewModel.checkHasNoTrainingHistory();
    }

    ko.applyBindings(viewModel, document.getElementById('applyTrainingHistory'));

    $('#training-history-apply').on('keyup', '#training-history-provider, #training-history-course-title, #training-history-from-year, #training-history-to-year', function () {
        $('#training-history-apply').removeClass('panel-danger');
        $('#apply-button').text('Save and continue');

        $('#unsavedTrainingHistory').hide();

        if ($('#unsavedQuals').is(':hidden') && $('#unsavedWorkExp').is(':hidden')) {
            $('#unsavedChanges').hide();
        }

        if ($(this).val() !== "") {
            $('#apply-button').addClass('dirtyTrainingHistory');
        } else if ($('#training-history-provider, #training-history-course-title, #training-history-from-year, #training-history-to-year').val() === "") {
            $('#apply-button').removeClass('dirtyTrainingHistory');
        }
    });

    $('.content-container').on('click', '.dirtyTrainingHistory', function (e) {
        $(this).removeClass('disabled dirtyTrainingHistory').text('Continue anyway');
        $('#unsavedChanges, #unsavedTrainingHistory').show();
        $('#training-history-apply').addClass('panel-danger').css({ 'margin-bottom': '0', 'padding-top': '0', 'padding-bottom': '0' });

        e.preventDefault();
    });

    $('#addTrainingHistoryBtn').on('click', function () {
        $('#training-history-apply').removeClass('panel-danger');
        $('#apply-button').removeClass('dirtyTrainingHistory');
        $('#apply-button').removeClass('dirtyTrainingHistory').text('Save and continue');

        $('#unsavedTrainingHistory').hide();

        if ($('#unsavedQuals').is(':hidden') && $('#unsavedWorkExp').is(':hidden')) {
            $('#unsavedChanges').hide();
        }
    });
}());