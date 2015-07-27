// TODO: get 'postcode' messages from C#

$(document).ready(function () {

    $(document).on("change", ".address-item", function() {
        $("#Address_Uprn").val("");
    });
});

// provides the matching addresses from postcode
(function ($) {

    var searchContext = "",
        key = "RH59-EY94-RA78-NZ89";

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
                            value: "",
                            data: suggestion
                        }
                    }));
                }
            });
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
        autoFocus: true,
        minLength: 1,
        delay: 100
    }).focus(function () {
        searchContext = "";
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

    }

    //TODO: Aria message when locations are found

    

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