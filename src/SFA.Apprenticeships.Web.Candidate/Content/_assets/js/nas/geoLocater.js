window.googleMapsScriptLoaded = function () {
    $(window).trigger('googleMapsScriptLoaded');
};

$(function () {
    var geocoder,
        apiScriptLoaded = false,
		apiScriptLoading = false,
        $window = $(window),
        $body = $('body');

    $window.on('googleMapsScriptLoaded', function () {
        apiScriptLoaded = true;
        geocoder = new google.maps.Geocoder();
        geoFindMe();
    });

    function geoFindMe() {
        var output = document.getElementById("Location"),
            latVal,
            longVal;  

        if (!apiScriptLoaded && !apiScriptLoading) {
            $body.append('<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&callback=googleMapsScriptLoaded&client=gme-skillsfundingagency&channel=findapprenticeship' + '"></script>');
            apiScriptLoading = true;
        }

        if (!apiScriptLoaded) return true;

        if (!navigator.geolocation) {
            output.placeholder = "Geolocation is not supported by your browser";
            return;
        }

        function success(position) {
            var latVal = position.coords.latitude,
                longVal = position.coords.longitude,
                latlng = new google.maps.LatLng(latVal, longVal);


            geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        var myPostcode;

                        for (var i = 0; i < results[1].address_components.length; i++) {
                            for (var j = 0; j < results[1].address_components[i].types.length; j++) {
                                if (results[1].address_components[i].types[j] == "postal_code") {
                                    myPostcode = results[1].address_components[i].long_name;
                                    break;
                                }
                            }
                        }

                        output.value = myPostcode;

                    } else {
                        output.placeholder = 'No location found';
                    }
                } else {
                    output.placeholder = 'No location found';
                }
            });
        };

        function error() {
            output.placeholder = "Unable to retrieve your location";
        };

        output.placeholder = "Locating…";

        navigator.geolocation.getCurrentPosition(success, error, { maximumAge: 600000 });

    }

    $('.geolocation #geoLocateContainer').append(' or <span class="fake-link geolocater inl-block hide-nojs" id="getLocation">use current location</span>');

    $('.geolocation').on('click', '#getLocation', function () {
        geoFindMe();
    });

});

