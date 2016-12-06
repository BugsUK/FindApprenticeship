(function () {
    // Validation messages
    var validationMessageNumberOfPositionsRequired = "You must enter at least 1 position";

    function stringStartsWith(string, prefix) {
        return string.slice(0, prefix.length) === prefix;
    }

    ko.validation.registerExtenders();

    var locationAddressItemModel = function (itemVacancyLocationId, itemAddressLine1, itemAddressLine2, itemAddressLine3, itemAddressLine4, itemTown, itemPostcode, itemNumberOfPositions, itemUprn, itemProvinceName) {

        var self = this;

        self.itemFriendlyAddress = ko.computed(function () {
            var address = itemAddressLine1;
            if (itemAddressLine2) {
                address += "<br />" + itemAddressLine2;
            }
            address += "<br />" + itemTown + " " + itemPostcode;
            return address;
        }, self);

        self.itemVacancyLocationId = ko.observable(itemVacancyLocationId);
        self.itemAddressLine1 = ko.observable(itemAddressLine1);
        self.itemAddressLine2 = ko.observable(itemAddressLine2);
        self.itemAddressLine3 = ko.observable(itemAddressLine3);
        self.itemAddressLine4 = ko.observable(itemAddressLine4);
        self.itemTown = ko.observable(itemTown);
        
        self.itemPostcode = ko.observable(itemPostcode);
        self.itemProvinceName = ko.observable(itemProvinceName);
        self.itemUprn = ko.observable(itemUprn);
        self.itemNumberOfPositions = ko.observable(itemNumberOfPositions);

    };

    var locationAddressesViewModel = function () {
        var self = this;
        self.locationAddresses = ko.observableArray();

        self.showLocationAddresses = ko.observable(false);
        self.showAddNewLocationLink = ko.observable(false);

        self.locationAddressesStatus = ko.computed(function () {
            return self.showLocationAddresses() ? "block" : "none";
        }, self);

        self.addModeOn = ko.computed(function () {
            return self.showAddNewLocationLink() === false ? "block" : "none";
        }, self);

        self.addModeOff = ko.computed(function () {
            return self.showAddNewLocationLink() ? "block" : "none";
        }, self);

        self.addNewLocation = function() {
            self.showAddNewLocationLink(false);
        }
        
        self.errors = ko.validation.group(self);

        self.addLocationAddress = function (locationAddressItem) {
            self.locationAddresses.push(locationAddressItem);
            self.errors.showAllMessages(false);
            self.showLocationAddresses(true);
        };

        self.addLocationAddressByField = function (addressLine1, addressLine2, addressLine3, addressLine4, town, postcode, numberOfPositions, uprn, provinceName) {
            var locationAddressItem = new locationAddressItemModel(0, addressLine1, addressLine2, addressLine3, addressLine4, town, postcode, numberOfPositions, uprn, provinceName);

            var found = self.locationAddresses().some(function (el) {
                return el.itemFriendlyAddress() === locationAddressItem.itemFriendlyAddress();
            });
            if (!found) {
                self.locationAddresses.push(locationAddressItem);
                self.showLocationAddresses(true);
                self.showAddNewLocationLink(true);
            }
        };

        self.removeLocationAddress = function (locationAddress) {
            self.locationAddresses.remove(locationAddress);
            if (self.locationAddresses().length === 0) {
                self.showLocationAddresses(false);
            }
        };
        
        self.getLocationAddresses = function (data) {
            $(data).each(function (index, item) {
                var locationAddressItem = new locationAddressItemModel(item.VacancyLocationId, item.Address.AddressLine1, item.Address.AddressLine2, item.Address.AddressLine3, item.Address.AddressLine4, item.Address.Town, item.Address.Postcode, item.NumberOfPositions, item.Address.Uprn, item.Address.County);
                self.addLocationAddress(locationAddressItem);
            });
        };

        self.addErrors = function (data, modelState) {
            $(data).each(function (dataIndex, dataItem) {
                var itemModelState = $(modelState).filter(function (filterIndex) { return stringStartsWith(modelState[filterIndex].Key, "Addresses[" + dataIndex + "]") });
                
                $(itemModelState).each(function (index, item) {
                    var field = item.Key.split(".")[1];
                    if (field === "NumberOfPositions") {
                        $(item.Value).each(function (valueIndex, valueItem) {
                            var name = item.Key.replace("[", "\\[").replace("]", "\\]").replace(".", "\\.") + "_Error";
                            $("#" + name).text(valueItem);
                            $("#" + name).removeClass("field-validation-valid").addClass("error-message").show();
                        });
                    }
                });

            });
        };
    };

    $(function () {
        //override default knockout validation - insert validation message
        ko.validation.insertValidationMessage = function (element) {

            var span = document.createElement('span');

            span.className = "error-message";
            $(span).attr('aria-live', 'polite');

            var inputFormControlParent = $(element).closest(".validation-message-parent").find('.validation-message-container');

            if (inputFormControlParent.length > 0) {
                inputFormControlParent.append(span);

            } else {
                element.parentNode.insertBefore(span, element.nextSibling);
            }
            return span;
        };

        ko.validation.rules.pattern.message = 'Invalid.';
        ko.validation.configure({
            decorateElement: true,
            registerExtenders: true,
            messagesOnModified: true,
            insertMessages: true,
            parseInputAttributes: true,
            errorClass: 'error',
            grouping: {
                deep: true
            },
            errorElementClass: 'error'
        });

        var locationsViewModel = new locationAddressesViewModel();

        if (window.getLocationAddressesData) {
            locationsViewModel.getLocationAddresses(window.getLocationAddressesData());
        }

        ko.applyBindings(locationsViewModel, document.getElementById('locationAddressesTable'));

        if (window.getLocationAddressesData) {
            var modelState = window.getModelState();
            locationsViewModel.addErrors(window.getLocationAddressesData(), modelState);
        }

        //expose ViewModel globally (needed for lookup service)
        window.locationAddressesViewModel = locationsViewModel;
    });
}());