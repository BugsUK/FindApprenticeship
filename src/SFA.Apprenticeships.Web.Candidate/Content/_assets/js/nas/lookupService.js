﻿// TODO: get 'postcode' messages from C#

$(document).ready(function () {

    $(document).on("change", ".address-item", function() {
        $("#Address_Uprn").val("");
        $("#Address_GeoPoint_Latitude").val("");
        $("#Address_GeoPoint_Longitude").val("");
    });
});

// provides the matching addresses from postcode
(function ($) {

    var searchContext = "",
        key = "RH59-EY94-RA78-NZ89",
        uri = $('form').attr('action'),
        findAddressVal = $("#postcode-search").val();

    $('#enterAddressManually').on('click', function (e) {
        e.preventDefault();

        $('#address-details').removeClass('disabled');
        $('#Address_AddressLine1').focus();
    });

    $('#addressManualWrapper').on('click', function () {
        $('#address-details').removeClass('disabled');
    });

    $("#postcode-search").keyup(function () {
        findAddressVal = $(this).val();
    });

    $("#postcode-search").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "//services.postcodeanywhere.co.uk/CapturePlus/Interactive/Find/v2.10/json3.ws",
                dataType: "jsonp",
                data: {
                    key: key,
                    searchFor: "Residential",
                    country: 'GB',
                    searchTerm: request.term,
                    lastId: searchContext
                },
                success: function (data) {
                    response($.map(data.Items, function (suggestion) {
                        return {
                            label: suggestion.Text,
                            data: suggestion
                        }
                    }));
                }
            });
        },
        messages: {
            noResults: function () {
                return "We can't find an address matching " + findAddressVal;
            },
            results: function (amount) {
                return "We've found " + amount + (amount > 1 ? " addresses" : " address") +
                    " that match " + findAddressVal + ". Use up and down arrow keys to navigate";
            }
        },
        select: function (event, ui) {
            var item = ui.item.data

            if (item.Next == "Retrieve") {
                //retrieve the address
                retrieveAddress(item.Id);
            } else {
                var field = $(this);
                searchContext = item.Id;

                //search again
                window.setTimeout(function () {
                    field.autocomplete("search", item.Id);
                });
            }
        },
        autoFocus: false,
        minLength: 1,
        delay: 100
    });

    function retrieveAddress(id) {
        $('#addressLoading').show();
        $('#enterAddressManually').hide();
        $('#address-details').addClass('disabled');

        $.ajax({
            url: "//services.postcodeanywhere.co.uk/CapturePlus/Interactive/Retrieve/v2.10/json3.ws",
            dataType: "jsonp",
            data: {
                key: key,
                id: id
            },
            success: function (data) {
                if (data.Items.length)
                    populateAddress(data.Items[0]);

                $('#addressLoading').hide();
                $('#enterAddressManually').show();
                $('#address-details').removeClass('disabled');
                $("#postcode-search").val("");
            }
        });
    }

    function populateAddress(address) {
        console.log(address);
        $('#Address_AddressLine1').val(address.Line1);
        $('#Address_AddressLine2').val(address.Line2);
        $('#Address_AddressLine3').val(address.Line3);
        $('#Address_AddressLine4').val(address.City);
        $('#Address_Postcode').val(address.PostalCode);
        $("#Address_Uprn").val(address.DomesticId);

        $('#addressesPopulated').text('Your address has been populated below');

        populateLatLng(address);
        Webtrends.multiTrack({ element: this, argsa: ["DCS.dcsuri", uri + "/findaddress", "WT.dl", "99", "WT.ti", "Settings – Find Address"] });
    }

    function populateLatLng(address) {
        var url = "https://api.postcodes.io/postcodes/" + address.PostalCode,
                json;

        $.get(url)
        .done(function (data) {
            json = data;

            if (json.status == 200 && json.result !== null) {
                $("#Address_GeoPoint_Latitude").val(json.result.latitude);
                $("#Address_GeoPoint_Longitude").val(json.result.longitude);
            } else {
                //console.log("Nope");
            }

        })
        .fail(function () {
            //console.log("failed");
        });
    }

})(jQuery);

// checking for existing email address
(function ($) {

    $.fn.usernameLookup = function (apiurl) {

        var self = this;

        var $emailAvailableMessage = $('#email-available-message');

        var setErrorMessage = function () {
            $emailAvailableMessage.html('<p class="text">Your email address has already been activated. Please try signing in again. If you’ve forgotten your password you can reset it.</p>');
        };

        var cleanErrorMessage = function () {
            $emailAvailableMessage.html('');
        }

        var handleSucess = function (response) {
            if (!response.HasError) {
                if (response.IsUserNameAvailable == false) {
                    setErrorMessage();
                }
                else {
                    cleanErrorMessage();
                }
            }
        };

        var handleError = function (error) {
            //Ignore, could be proxy issues so will work as 
            //non-JS version.
            //console.log(error);
        };

        self.focusout(function () {
            var username = $(this).val().trim();

            if (!username) {
                return;
            }

            cleanErrorMessage();

            $.ajax({
                url: apiurl,
                type: 'GET',
                data: { username: username },
                success: handleSucess,
                error: handleError
            });
        });

        self.focusin(function () {
            $('#display-message').html('');
        });
    };
})(jQuery);