
$(function () {

    function geoFindMe() {
        var output = document.getElementById("Location"),
            latVal,
            longVal;  

        if (!navigator.geolocation) {
            output.placeholder = "Geolocation is not supported by your browser";
            return;
        }

        function success(position) {
            var latVal = position.coords.latitude,
                longVal = position.coords.longitude,
                url = "https://api.postcodes.io/postcodes?lon=" + longVal + "&lat=" + latVal + "&wideSearch=true",
                json;

            $.get(url)
            .done(function (data) {
                json = data;

                if (json.status == 200 && json.result !== null) {
                    output.value = json.result[0].postcode;
                    output.placeholder = "";
                } else {
                    output.value = "Unable to retrieve postcode"
                }

            })
            .fail(function () {
                output.value = "Unable to retrieve postcode"
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

