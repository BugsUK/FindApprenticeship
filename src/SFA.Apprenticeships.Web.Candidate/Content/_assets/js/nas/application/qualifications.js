(function() {
    var qualificationMessages = {
        selectType: "Please select qualification type",
        otherRequired: "Please enter other qualification",
        otherContainsInvalidCharacters: "Other qualification can't contain invalid characters, eg '/'",
        yearRequired: "Please enter year of qualification",
        yearMustBeAFourDigitNumber: "Year must be 4 digits, eg 1990",
        yearMustBeAfter: "Year must be 4 digits, and not before " + (new Date().getFullYear() - 100),
        subjectRequired: "Please enter subject",
        subjectContainsInvalidCharacters: "Subject can't contain invalid characters, eg '/'",
        gradeRequired: "Please enter grade",
        gradeContainsInvalidCharacters: "Grade can't contain invalid characters, eg '/'",
        futureYear: "Please select 'Predicted' if you're adding a grade in the future"
    };

    var qualificationTypeModel = function(name) {
        var self = this;

        self.qualificationTypeName = name;
    };

    var groupedQualificationsModel = function(key) {
        var self = this;

        self.groupKey = ko.observable(key);
        self.groupItems = ko.observableArray();
    };

    groupedQualificationsModel.prototype = {
        addItem: function(item) {
            var self = this;
            self.groupItems.push(item);
        },
        removeItem: function(item) {
            var self = this;
            self.groupItems.remove(item);
        }
    };

    var qualificationItemModel = function (itemType, itemOtherType, itemYear, itemSubject, itemGrade, itemPredicted, itemRegexPattern, itemYearRegexPattern) {
        var self = this;
        self.itemIndex = ko.observable(0);
        self.itemRegexPattern = ko.observable(itemRegexPattern);
        self.qualificationPredicted = ko.observable(itemPredicted);
        self.itemYearRegexPattern = ko.observable(itemYearRegexPattern);
        self.qualificationType = ko.observable(itemType).extend({
            required: { message: qualificationMessages.selectType }
        });
        self.otherQualificationType = ko.observable(itemOtherType).extend({
            pattern: {
                message: qualificationMessages.otherContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.qualificationYear = ko.observable(itemYear).extend({
            required: { message: qualificationMessages.yearRequired }
        }).extend({
            number: {
                message: qualificationMessages.yearMustBeAFourDigitNumber
            }
        }).extend({
            max: {
                params: new Date().getFullYear(),
                message: qualificationMessages.futureYear,
                onlyIf: function() { return (self.qualificationPredicted() === false); }
            }
        }).extend({
            min: {
                message: qualificationMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        });

        self.qualificationSubject = ko.observable(itemSubject).extend({
            required: { message: qualificationMessages.subjectRequired }
        }).extend({
            pattern: {
                message: qualificationMessages.subjectContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });
        self.qualificationGrade = ko.observable(itemGrade).extend({
            required: { message: qualificationMessages.gradeRequired }
        }).extend({
            pattern: {
                message: qualificationMessages.gradeContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.readOnly = ko.observable("readonly");
        self.showEditButton = ko.observable(true);

        self.itemErrors = ko.validation.group(self);

        self.gradeDisplay = ko.computed(function() {
            return self.qualificationPredicted() ? self.qualificationGrade() + " (Predicted)" : self.qualificationGrade();
        }, self);
    };

    function addQualification(qualifications, typeSelected, typeOther, year, subject, grade, predicted, regex, yearRegex) {
        var qualificationArrays = qualifications;

        if (grade.indexOf("(Predicted)") > -1) {
            var gradeIndex = grade.indexOf("(Predicted)");

            grade = grade.slice(0, gradeIndex).trim();
        }

        if (typeSelected === "Other") {
            if (!typeOther || 0 === typeOther.length) {
            } else {
                typeSelected = typeOther;
            }
        }

        var qualification = new qualificationItemModel(typeSelected, typeOther, year, subject, grade, predicted, regex, yearRegex);

        var match = ko.utils.arrayFirst(qualificationArrays, function(item) {
            return item.groupKey() === typeSelected;
        });

        if (!match) {
            var group = new groupedQualificationsModel(typeSelected);

            group.addItem(qualification);
            qualificationArrays.push(group);
        } else {
            match.addItem(qualification);
        }

        return qualificationArrays;

    }

    var qualificationViewModel = function() {
        var self = this;

        self.hasQualifications = ko.observable(undefined);
        self.hasNoQualifications = ko.observable(undefined);
        self.showQualifications = ko.observable(false);

        self.qualificationStatus = ko.computed(function() {
            return self.showQualifications() ? "block" : "none";
        }, qualificationViewModel);

        self.regexPattern = ko.observable();
        self.yearRegexPattern = ko.observable();

        // TODO: get values from config.
        self.qualificationTypes = ko.observableArray([
            new qualificationTypeModel("GCSE"),
            new qualificationTypeModel("AS Level"),
            new qualificationTypeModel("A Level"),
            new qualificationTypeModel("BTEC"),
            new qualificationTypeModel("NVQ or SVQ Level 1"),
            new qualificationTypeModel("NVQ or SVQ Level 2"),
            new qualificationTypeModel("NVQ or SVQ Level 3"),
            new qualificationTypeModel("Other")
        ]);

        self.selectedQualification = ko.observable().extend({ required: { message: qualificationMessages.selectType } });

        self.otherQualification = ko.observable().extend({
            required: {
                message: qualificationMessages.otherRequired,
                onlyIf: function() { return (self.selectedQualification() === "Other"); }
            }
        }).extend({
            pattern: {
                message: qualificationMessages.otherContainsInvalidCharacters,
                params: self.regexPattern,
                onlyIf: function() { return (self.selectedQualification() === "Other"); }
            }
        });

        self.showOtherQualification = ko.observable(false);

        self.year = ko.observable().extend({
            required: { message: qualificationMessages.yearRequired }
        }).extend({
            number: {
                message: qualificationMessages.yearMustBeAFourDigitNumber
            }
        }).extend({
            max: {
                params: new Date().getFullYear(),
                message: qualificationMessages.futureYear,
                onlyIf: function() { return (self.predicted() === false && self.yearValidationActivated() === true); }
            }
        }).extend({
            min: {
                message: qualificationMessages.yearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        });

        self.subject = ko.observable().extend({
            required: { message: qualificationMessages.subjectRequired }
        }).extend({
            pattern: {
                message: qualificationMessages.subjectContainsInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.grade = ko.observable().extend({
            required: { message: qualificationMessages.gradeRequired }
        }).extend({
            pattern: {
                message: qualificationMessages.gradeContainsInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.predicted = ko.observable(false);
        self.yearValidationActivated = ko.observable(false);
        self.qualifications = ko.observableArray();
        self.errors = ko.validation.group(self);

        self.selectedQualification.subscribe(function(selectedValue) {
            if (selectedValue === "Other") {
                self.showOtherQualification(true);
            } else {
                self.otherQualification("");
                self.showOtherQualification(false);
            }
        });

        self.removeQualification = function(qualification) {
            var match = ko.utils.arrayFirst(self.qualifications(), function(item) {
                return item.groupKey() === qualification.qualificationType();
            });

            if (match) {
                match.removeItem(qualification);
                self.reIndexQualifications();
                $('#qualAddConfirmText').text('Qualification removed from table');
            }
        };

        self.addQualification = function() {
            self.yearValidationActivated(true);

            if (self.errors().length === 0) {
                var result = addQualification(
                    self.qualifications(),
                    self.selectedQualification(),
                    self.otherQualification(),
                    self.year(),
                    self.subject(),
                    self.grade(),
                    self.predicted(),
                    self.regexPattern(),
                    self.yearRegexPattern());

                self.qualifications(result);

                $('#qualAddConfirmText').text('Qualification has been added to table below');

                self.reIndexQualifications();
                self.subject("");
                self.grade("");
                self.predicted(false);
                self.errors.showAllMessages(false);
                self.yearValidationActivated(false);
            } else {
                self.errors.showAllMessages();
                $('#qualAddConfirmText').text('There has been a problem adding this qualification, check errors above');
            }
        };

        self.editQualification = function(qualification) {
            qualification.readOnly(undefined);
            qualification.showEditButton(false);
        };

        self.saveQualificationItem = function(qualification) {
            if (qualification.itemErrors().length === 0) {
                qualification.readOnly('readonly');
                qualification.showEditButton(true);
            } else {
                qualification.itemErrors.showAllMessages();
            }
        };

        self.checkHasNoQualifications = function() {
            self.showQualifications(false);
            self.hasQualifications(undefined);
            self.hasNoQualifications("checked");
        };

        self.getqualifications = function(data) {
            $(data).each(function(index, item) {
                var result = addQualification(
                    self.qualifications(),
                    item.QualificationType,
                    "",
                    item.Year,
                    item.Subject,
                    item.Grade,
                    item.IsPredicted,
                    self.regexPattern(),
                    self.yearRegexPattern());

                self.qualifications(result);
            });

            if (self.qualifications().length > 0) {
                self.reIndexQualifications();
                self.showQualifications(true);
                self.hasQualifications("checked");
                self.hasNoQualifications(undefined);
                self.errors.showAllMessages(false);
            } else {
                self.checkHasNoQualifications();
            }
        };

        self.reIndexQualifications = ko.computed(function() {
            var index = 0;

            ko.utils.arrayForEach(self.qualifications(), function(qualification) {
                ko.utils.arrayForEach(qualification.groupItems(), function(item) {
                    item.itemIndex(index);
                    index++;
                });
            });

            return index;
        }, self);
    };

    var viewModel = new qualificationViewModel();

    if (window.getWhiteListRegex()) {
        viewModel.regexPattern(window.getWhiteListRegex());
    }

    if (window.getYearRegex()) {
        viewModel.yearRegexPattern(window.getYearRegex());
    }

    if (window.getQualificationData()) {
        viewModel.getqualifications(window.getQualificationData());
    } else {
        viewModel.checkHasNoQualifications();
    }

    ko.applyBindings(viewModel, document.getElementById('applyQualifications'));

    $('#qualifications-panel').on('keyup', '#subject-name, #subject-grade', function() {
        $('#qualEntry').removeClass('panel-danger');
        $('#apply-button').text('Save and continue');

        $('#unsavedQuals').hide();

        if ($('#unsavedWorkExp').is(':hidden') && $('#unsavedTrainingHistory').is(':hidden')) {
            $('#unsavedChanges').hide();
        }

        if ($(this).val() !== "") {
            $('#apply-button').addClass('dirtyQuals');
        } else if ($('#subject-name, #subject-grade').val() === "") {
            $('#apply-button').removeClass('dirtyQuals');
        }
    });

    $('.content-container').on('click', '.dirtyQuals', function(e) {
        $(this).removeClass('disabled dirtyQuals').text('Continue anyway');
        $('#unsavedChanges, #unsavedQuals').show();
        $('#qualEntry').addClass('panel-danger').css({ 'margin-bottom': '0', 'padding-top': '0', 'padding-bottom': '0' });

        e.preventDefault();
    });

    $('#saveQualification').on('click', function() {
        $('#qualEntry').removeClass('panel-danger');
        $('#apply-button').removeClass('dirtyQuals').text('Save and continue');

        $('#unsavedQuals').hide();

        if ($('#unsavedWorkExp').is(':hidden') && $('#unsavedTrainingHistory').is(':hidden')) {
            $('#unsavedChanges').hide();
        }
    });
}());
