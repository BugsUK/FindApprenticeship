(function () {
    var workExperienceMessages = {
        employerRequired: "Please enter employer name",
        employerExceedsFiftyCharacters: "Employer name can't exceed 50 characters",
        employerContainsInvalidCharacters: "Employer name can't contain invalid characters, eg '/'",
        jobTitleRequired: "Please enter job title",
        jobTitleExceedsFiftyCharacters: "Job title can't exceed 50 characters",
        jobTitleContainsInvalidCharacters: "Job title can't contain invalid characters, eg '/'",
        mainDutiesRequired: "Please provide main duties",
        mainDutiesExceedsTwoHundredCharacters: "Main duties can't exceed 200 characters",
        mainDutiesContainInvalidCharacters: "Main duties can't contain invalid characters, eg '/'",
        fromMonthRequired: "Please enter month started",
        fromYearRequired: "Please enter year started",
        fromYearMustBeNumeric: "Year started must contain 4 digits, eg 1990",
        fromYearMustNotBeInFuture: "Year started can't be in the future",
        yearMustBeARange: "Year must be 4 digits, between " + (new Date().getFullYear() - 100) + " and " + (new Date().getFullYear()),
        toMonthRequired: "Please enter month you finished",
        toYearRequired: "Please enter year finished",
        toYearMustBeNumeric: "Year finished must contain 4 digits, eg 1990",
        toYearMustNotBeInFuture: "Year finished can't be in the future",
        toYearMustBeAfterFromYear: 'Year finished must be after year started',
        yearMustBeAfter: "Year must be 4 digits, and not before 1915",
        dateFinishedMustBeAfterDateStarted: "Date finished must be after date started"
    };

    var workExperienceItemModel = function(itemEmployer, itemJobTitle, itemDuties, itemFromMonth, itemFromYear, itemToMonth, itemToYear, itemIsCurrentEmployment, itemCurrentYear, itemRegex, itemYearRegexPattern) {
        var self = this;

        self.itemRegexPattern = ko.observable(itemRegex);
        self.itemYearRegexPattern = ko.observable(itemYearRegexPattern);

        self.itemEmployer = ko.observable(itemEmployer).extend({
            required: { message: workExperienceMessages.employerRequired }
        }).extend({
            maxLength: {
                message: workExperienceMessages.employerExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: workExperienceMessages.employerContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.itemJobTitle = ko.observable(itemJobTitle).extend({
            required: { message: workExperienceMessages.jobTitleRequired }
        }).extend({
            maxLength: {
                message: workExperienceMessages.jobTitleExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: workExperienceMessages.jobTitleContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.itemMainDuties = ko.observable(itemDuties).extend({
            required: { message: workExperienceMessages.mainDutiesRequired }
        }).extend({
            maxLength: {
                message: workExperienceMessages.mainDutiesExceedsTwoHundredCharacters,
                params: 200
            }
        }).extend({
            pattern: {
                message: workExperienceMessages.mainDutiesContainInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.itemIsCurrentEmployment = ko.observable(itemIsCurrentEmployment);
        self.itemCurrentYear = ko.observable(itemCurrentYear);

        self.itemFromMonth = ko.observable(itemFromMonth).extend({
            required: { message: workExperienceMessages.fromMonthRequired }
        });

        self.itemFromYear = ko.observable(itemFromYear).extend({
            required: { message: workExperienceMessages.fromYearRequired }
        }).extend({
            number: {
                message: workExperienceMessages.fromYearMustBeNumeric
            }
        }).extend({
            min: {
                message: workExperienceMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            max: {
                message: workExperienceMessages.fromYearMustNotBeInFuture,
                params: self.itemCurrentYear
            }
        });

        self.itemToYear = ko.observable(itemToYear).extend({
            required: {
                message: workExperienceMessages.toYearRequired,
                onlyIf: function() { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            number: {
                message: workExperienceMessages.toYearMustBeNumeric,
                onlyIf: function() { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            min: {
                message: workExperienceMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            max: {
                message: workExperienceMessages.toYearMustNotBeInFuture,
                params: self.itemCurrentYear,
                onlyIf: function() {
                    return (self.itemIsCurrentEmployment() === false);
                }
            }
        }).extend({
            validation: {
                validator: function(val, fromYearValue) {
                    return val >= fromYearValue;
                },
                message: workExperienceMessages.toYearMustBeAfterFromYear,
                params: self.fromYear,
                onlyIf: function() {
                    return (self.itemIsCurrentEmployment() === false);
                }
            }
        });

        self.itemToMonth = ko.observable(itemToMonth).extend({
            required: {
                message: workExperienceMessages.toMonthRequired,
                onlyIf: function() { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            validation: {
                validator: function(val, fromMonthValue) {
                    return val >= fromMonthValue;
                },
                message: workExperienceMessages.dateFinishedMustBeAfterDateStarted,
                params: self.itemFromMonth,
                onlyIf: function() {
                    return (self.itemFromYear() === self.itemToYear());
                }
            }
        });

        self.readOnly = ko.observable("readonly");
        self.showEditButton = ko.observable(true);

        self.toItemDateReadonly = ko.observable(undefined);

        self.itemIsCurrentEmployment.subscribe(function(selectedValue) {

            if (selectedValue === true) {
                self.itemToYear(null);
                self.toItemDateReadonly("disabled");
                self.errors.showAllMessages(false);
            } else {
                self.toItemDateReadonly(undefined);
            }
        });

        self.disableToDateIfCurrent = function() {
            if (self.itemIsCurrentEmployment()) {
                self.toItemDateReadonly("disabled");
            }
        };

        self.itemErrors = ko.validation.group(self);
    };

    var workExperienceViewModel = function () {

        var self = this;

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

        self.hasWorkExperience = ko.observable(undefined);
        self.hasNoWorkExperience = ko.observable(undefined);
        self.showWorkExperience = ko.observable(false);

        self.workExperienceStatus = ko.computed(function () {
            return self.showWorkExperience() ? "block" : "none";
        }, self);

        self.regexPattern = ko.observable();
        self.yearRegexPattern = ko.observable();

        self.employer = ko.observable().extend({
            required: { message: workExperienceMessages.employerRequired }
        }).extend({
            maxLength: {
                message: workExperienceMessages.employerExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: workExperienceMessages.employerContainsInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.jobTitle = ko.observable().extend({
            required: { message: workExperienceMessages.jobTitleRequired }
        }).extend({
            maxLength: {
                message: workExperienceMessages.jobTitleExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: workExperienceMessages.jobTitleContainsInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.mainDuties = ko.observable().extend({
            required: { message: workExperienceMessages.mainDutiesRequired }
        }).extend({
            maxLength: {
                message: workExperienceMessages.mainDutiesExceedsTwoHundredCharacters,
                params: 200
            }
        }).extend({
            pattern: {
                message: workExperienceMessages.mainDutiesContainInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.isCurrentEmployment = ko.observable(false);

        self.currentYear = ko.observable();

        self.fromMonth = ko.observable().extend({
            required: { message: workExperienceMessages.fromMonthRequired }
        });

        self.fromYear = ko.observable().extend({
            required: { message: workExperienceMessages.fromYearRequired }
        }).extend({
            number: {
                message: workExperienceMessages.fromYearMustBeNumeric
            }
        }).extend({
            max: {
                message: workExperienceMessages.fromYearMustNotBeInFuture,
                params: self.currentYear
            }
        }).extend({
            min: {
                message: workExperienceMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        });

        self.toMonth = ko.observable().extend({
            required: {
                message: workExperienceMessages.toMonthRequired,
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            validation: {
                validator: function (val, fromMonthValue) {
                    return val >= fromMonthValue;
                },
                message: workExperienceMessages.dateFinishedMustBeAfterDateStarted,
                params: self.fromMonth,
                onlyIf: function () {
                    return (self.fromYear() === self.toYear());
                }
            }
        });

        self.toYear = ko.observable().extend({
            required: {
                message: workExperienceMessages.toYearRequired,
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            number: {
                message: workExperienceMessages.toYearMustBeNumeric,
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            max: {
                message: workExperienceMessages.toYearMustNotBeInFuture,
                params: self.currentYear,
                onlyIf: function () {
                    return (self.isCurrentEmployment() === false);
                }
            }
        }).extend({
            min: {
                message: workExperienceMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            validation: {
                validator: function (val, fromYearValue) {
                    return val >= fromYearValue;
                },
                message: workExperienceMessages.toYearMustBeAfterFromYear,
                params: self.fromYear,
                onlyIf: function () {
                    return (self.isCurrentEmployment() === false);
                }
            }
        });

        self.toDateReadonly = ko.observable(undefined);

        self.isCurrentEmployment.subscribe(function (selectedValue) {
            if (selectedValue === true) {

                self.toYear(null);
                self.toDateReadonly("disabled");
            } else {
                self.toDateReadonly(undefined);
            }
        });

        self.workExperiences = ko.observableArray();

        self.errors = ko.validation.group(self);

        self.addWorkExperience = function () {
            if (self.errors().length === 0) {

                var toMonth = self.toMonth();
                var toYear = self.toYear();

                if (self.isCurrentEmployment()) {
                    toMonth = 0;
                    toYear = 0;
                }

                var workExperience = new workExperienceItemModel(
                    self.employer(),
                    self.jobTitle(),
                    self.mainDuties(),
                    self.fromMonth(),
                    self.fromYear(),
                    toMonth,
                    toYear,
                    self.isCurrentEmployment(),
                    self.currentYear(),
                    self.regexPattern(),
                    self.yearRegexPattern());

                workExperience.disableToDateIfCurrent();
                self.workExperiences.push(workExperience);

                self.employer("");
                self.jobTitle("");
                self.mainDuties("");
                self.fromYear(null);
                self.toYear(null);
                self.isCurrentEmployment(false);
                self.toDateReadonly(undefined);

                self.errors.showAllMessages(false);

                $('#workAddConfirmText').text("Work experience has been added to table below");
            } else {
                self.errors.showAllMessages();
                $('#workAddConfirmText').text("There has been a problem adding work experience, check you've entered all details correctly");
            }

        };

        self.editWorkExperience = function (workExperience) {
            workExperience.readOnly(undefined);
            workExperience.showEditButton(false);
        };

        self.saveWorkExperience = function (workExperience) {
            if (workExperience.itemErrors().length === 0) {
                workExperience.readOnly('readonly');
                workExperience.showEditButton(true);
            } else {
                workExperience.itemErrors.showAllMessages();
            }
        };

        self.removeWorkExperience = function (workExperience) {
            self.workExperiences.remove(workExperience);
        };

        self.checkHasNoWorkExperience = function () {
            self.showWorkExperience(false);
            self.hasWorkExperience(undefined);
            self.hasNoWorkExperience("checked");
        };

        self.getWorkExperiences = function (data) {
            $(data).each(function (index, item) {
                var currentEmployer = false;
                var myToYear = item.ToYear;

                if (myToYear <= 1) {
                    currentEmployer = true;
                    myToYear = null;
                }

                var experienceItemModel = new workExperienceItemModel(
                    item.Employer,
                    item.JobTitle,
                    item.Description,
                    item.FromMonth,
                    item.FromYear,
                    item.ToMonth,
                    myToYear,
                    currentEmployer,
                    self.currentYear(),
                    self.regexPattern(),
                    self.yearRegexPattern());

                experienceItemModel.disableToDateIfCurrent();
                self.workExperiences.push(experienceItemModel);
            });

            if (self.workExperiences().length > 0) {
                self.showWorkExperience(true);
                self.hasWorkExperience("checked");
                self.hasNoWorkExperience(undefined);
            } else {
                self.checkHasNoWorkExperience();
            }

        };

    };

    var viewModel = new workExperienceViewModel();

    if (window.getCurrentYear()) {
        viewModel.currentYear(window.getCurrentYear());
    }

    if (window.getWhiteListRegex()) {
        viewModel.regexPattern(window.getWhiteListRegex());
    }

    if (window.getYearRegex()) {
        viewModel.yearRegexPattern(window.getYearRegex());
    }

    if (window.getWorkExperienceData()) {
        viewModel.getWorkExperiences(window.getWorkExperienceData());
    } else {
        viewModel.checkHasNoWorkExperience();
    }

    ko.applyBindings(viewModel, document.getElementById('applyWorkExperience'));

    $('#workexperience-apply').on('keyup', '#work-employer, #work-title, #work-role, #work-from-year, #work-to-year', function () {
        $('#workexperience-apply').removeClass('panel-danger');
        $('#apply-button').text('Save and continue');

        $('#unsavedWorkExp').hide();

        if ($('#unsavedQuals').is(':hidden') && $('#unsavedTrainingHistory').is(':hidden')) {
            $('#unsavedChanges').hide();
        }

        if ($(this).val() !== "") {
            $('#apply-button').addClass('dirtyWorkExp');
        } else if ($('#work-employer, #work-title, #work-role, #work-from-year, #work-to-year').val() === "") {
            $('#apply-button').removeClass('dirtyWorkExp');
        }
    });

    $('.content-container').on('click', '.dirtyWorkExp', function (e) {
        $(this).removeClass('disabled dirtyWorkExp').text('Continue anyway');
        $('#unsavedChanges, #unsavedWorkExp').show();
        $('#workexperience-apply').addClass('panel-danger').css({ 'margin-bottom': '0', 'padding-top': '0', 'padding-bottom': '0' });

        e.preventDefault();
    });

    $('#addWorkBtn').on('click', function () {
        $('#workexperience-apply').removeClass('panel-danger');
        $('#apply-button').removeClass('dirtyWorkExp');
        $('#apply-button').removeClass('dirtyWorkExp').text('Save and continue');

        $('#unsavedWorkExp').hide();

        if ($('#unsavedQuals').is(':hidden') && $('#unsavedTrainingHistory').is(':hidden')) {
            $('#unsavedChanges').hide();
        }
    });
}());