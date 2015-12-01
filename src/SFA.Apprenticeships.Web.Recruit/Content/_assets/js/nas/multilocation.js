(function () {
    // Validation messages
    var validationMessageNumberOfPositionsRequired = "You must enter at least 1 position";

    ko.validation.registerExtenders();

    var locationAddressItemModel = function (itemAddressLine1, itemAddressLine2, itemAddressLine3, itemAddressLine4, itemPostcode, itemNumberOfPositions, itemUprn) {

        var self = this;

        self.itemFriendlyAddress = ko.computed(function () {
            var address = itemAddressLine1;
            if (itemAddressLine2) {
                address += "<br />" + itemAddressLine2;
            }
            address += "<br />" + itemAddressLine4 + " " + itemPostcode;
            return address;
        }, self);

        self.itemAddressLine1 = ko.observable(itemAddressLine1);
        self.itemAddressLine2 = ko.observable(itemAddressLine2);
        self.itemAddressLine3 = ko.observable(itemAddressLine3);
        self.itemAddressLine4 = ko.observable(itemAddressLine4);
        
        self.itemPostcode = ko.observable(itemPostcode);
        self.itemUprn = ko.observable(itemUprn);
        self.itemNumberOfPositions = ko.observable(itemNumberOfPositions);
        //    .extend({
        //        required: { message: validationMessageNumberOfPositionsRequired }
        //});

        self.itemErrors = ko.validation.group(self);
    };

    var locationAddressesViewModel = function () {
        var self = this;
        
        self.locationAddresses = ko.observableArray();

        self.showLocationAddresses = ko.observable(false);

        self.locationAddressesStatus = ko.computed(function () {
            return self.showLocationAddresses() ? "block" : "none";
        }, self);
        
        self.errors = ko.validation.group(self);

        self.addLocationAddress = function (locationAddressItem) {
            self.locationAddresses.push(locationAddressItem);
            self.errors.showAllMessages(false);
            self.showLocationAddresses(true);
        };

        self.addLocationAddressByField = function (addressLine1, addressLine2, addressLine3, addressLine4, postcode, numberOfPositions, uprn) {
            var locationAddressItem = new locationAddressItemModel(addressLine1, addressLine2, addressLine3, addressLine4, postcode, numberOfPositions, uprn);

            var found = self.locationAddresses().some(function (el) {
                return el.itemFriendlyAddress() === locationAddressItem.itemFriendlyAddress();
            });
            if (!found) {
                self.locationAddresses.push(locationAddressItem);
                self.showLocationAddresses(true);
            }
            
            self.errors.showAllMessages(false);
        };

        self.removeLocationAddress = function (locationAddress) {
            self.locationAddresses.remove(locationAddress);
            if (self.locationAddresses().length === 0) {
                self.showLocationAddresses(false);
            }
        };
        
        self.getLocationAddresses = function (data) {
            $(data).each(function (index, item) {
                var locationAddressItem = new locationAddressItemModel(item.Address.AddressLine1, item.Address.AddressLine2, item.Address.AddressLine3, item.Address.AddressLine4, item.Address.Postcode, item.NumberOfPositions);
                self.addLocationAddress(locationAddressItem);
            });
        };

        self.saveAndContinue = function () {
            self.itemErrors = ko.validation.group(self);
            if (self.errors().length === 0) {
            } else {
                self.errors.showAllMessages();
            }
        }

    };

    $(function () {
        //override default knockout validation - insert validation message
        ko.validation.insertValidationMessage = function (element) {

            var span = document.createElement('span');

            span.className = "field-validation-error";
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
            errorClass: 'input-validation-error',
            grouping: {
                deep: true
            },
            errorElementClass: 'input-validation-error'
        });

        var locationsViewModel = new locationAddressesViewModel();

        if (window.getLocationAddressesData) {
            locationsViewModel.getLocationAddresses(window.getLocationAddressesData());
        }

        ko.applyBindings(locationsViewModel, document.getElementById('locationAddressesTable'));

        //expose ViewModel globally (needed for lookup service)
        window.locationAddressesViewModel = locationsViewModel;
    });
}());