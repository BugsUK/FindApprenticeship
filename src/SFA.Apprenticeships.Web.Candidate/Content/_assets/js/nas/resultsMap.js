$(function () {

    /*Show map with radius in results*/

    var apprLatitude       = Number($('#Latitude').val()),
        apprLongitude      = Number($('#Longitude').val()),
        apprMiles          = Number($('#loc-within').val()),
        resultsPage        = $('#results-per-page').val(),
        numberOfResults    = $('.vacancy-link').length,
        distanceOfLast     = $('.search-results__item:last-child .distance-value').html(),
        sortResultsControl = $('#sort-results').val(),
        apprZoom           = 9,
        radiusCircle,
        vacancyLinks       = $('.vacancy-link').toArray(),
        vacancies          = [],
        vacancy            = [],
        theMarkers         = [],
        theMaps            = [],
        directionsDisplay  = [],
        directionsService  = [],
        mapCenter          = { lat: apprLatitude, lng: apprLongitude },
        bounds             = new google.maps.LatLngBounds(),
        originLocation     = new google.maps.LatLng(apprLatitude, apprLongitude),
        latLngList         = [],
        theLatLon          = apprLatitude + ',' + apprLongitude;

    $('.map-links').each(function(){
        var $this = $(this),
        aHref = $this.attr('href');

        $this.attr('href', aHref.replace('LocationLatLon', theLatLon));
    });

    for (var i = 0; i < vacancyLinks.length; i++) {
        var lat = $(vacancyLinks[i]).attr('data-lat'),
            longi = $(vacancyLinks[i]).attr('data-lon'),
            title = $(vacancyLinks[i]).html(),
            id = $(vacancyLinks[i]).attr('data-vacancy-id');

        vacancies[i] = [lat, longi, title, id];

        directionsDisplay[i] = new google.maps.DirectionsRenderer({ suppressMarkers: true });
        directionsService[i] = new google.maps.DirectionsService();
    }

    if (apprLatitude == 0 || apprLongitude == 0) {
        $('#map-canvas').parent().hide();
    }

    function initialize() {

        var mapOptions = {
            center: mapCenter,
            zoom: apprZoom,
            panControl: false,
            zoomControl: true,
            mapTypeControl: false,
            scaleControl: false,
            streetViewControl: false,
            overviewMapControl: false,
            scrollwheel: false
        };

        var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

        var distanceCircle = {
            strokeColor: '#005ea5',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#005ea5',
            fillOpacity: 0.25,
            map: map,
            center: mapOptions.center,
            radius: apprMiles * 1609.344
        }

        radiusCircle = new google.maps.Circle(distanceCircle);

        var radiusBounds = radiusCircle.getBounds();

        var theZoom = null;

        setMarkers(map, vacancies)

        if (latLngList.length > 1) {
            map.fitBounds(bounds);
        } else if (apprMiles > 0) {
            map.fitBounds(radiusBounds);

            zoomChangeBoundsListener =
                google.maps.event.addListenerOnce(map, 'bounds_changed', function (event) {
                    if (this.getZoom()) {
                        theZoom = this.getZoom();

                        this.setZoom(theZoom + 1);
                    }
                });

            setTimeout(function () { google.maps.event.removeListener(zoomChangeBoundsListener) }, 2000);

        } else if (apprMiles == 0) {
            map.setZoom(5);
        }

    }

    function setMarkers(map, locations) {
        var image1 = new google.maps.MarkerImage(
                        '/Content/_assets/img/icon-location.png',
                        null, /* size is determined at runtime */
                        null, /* origin is 0,0 */
                        null, /* anchor is bottom center of the scaled image */
                        new google.maps.Size(20, 32));
        var image2 = new google.maps.MarkerImage(
                        '/Content/_assets/img/icon-location-selected.png',
                        null, /* size is determined at runtime */
                        null, /* origin is 0,0 */
                        null, /* anchor is bottom center of the scaled image */
                        new google.maps.Size(20, 32));

        for (var i = 0; i < locations.length; i++) {
            var appship = locations[i];
            var myLatLng = new google.maps.LatLng(appship[0], appship[1]);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                animation: google.maps.Animation.DROP,
                icon: image1,
                title: appship[2]
            });

            theMarkers.push(marker);
            latLngList.push(myLatLng);

            bounds.extend(latLngList[i]);

            var vacancyID = appship[3];

            bindMarkerClick(marker, map, vacancyID, image1, image2);

            itemHover(image1, image2);

        }

    }

    function bindMarkerClick(marker, map, vacancyID, image1, image2) {
        google.maps.event.addListener(marker, 'mouseover', function () {
            marker.setIcon(image2);
            marker.setZIndex(1000);
        });

        google.maps.event.addListener(marker, 'click', function () {
            marker.setIcon(image2);
            marker.setZIndex(1000);
            $('[data-vacancy-id="' + vacancyID + '"]').closest('.search-results__item')[0].scrollIntoView();
        });

        google.maps.event.addListener(marker, 'mouseout', function () {
            marker.setIcon(image1);
            marker.setZIndex(0);
        });
    }

    function itemHover(image1, image2) {
        $('.search-results__item').mouseover(function () {
            var thisPosition = $(this).index();
            theMarkers[thisPosition].setIcon(image2);
            theMarkers[thisPosition].setZIndex(1000);

        });

        $('.search-results__item').mouseleave(function () {
            var thisPosition = $(this).index();
            theMarkers[thisPosition].setIcon(image1);
            theMarkers[thisPosition].setZIndex(0);
        });

    }

    $('.vacancy-link').each(function () {

        var vacancyMap = $(this).closest('.search-results__item').find('.map')[0],
            vacancyLat = $(this).attr('data-lat'),
            vacancyLon = $(this).attr('data-lon'),
            latlng = new google.maps.LatLng(vacancyLat, vacancyLon);

        var myOptions = {
            zoom: 10,
            center: latlng,
            mapTypeControl: false,
            overviewMapControl: false,
            panControl: false,
            scaleControl: false,
            scrollwheel: false,
            streetViewControl: false,
            zoomControl: true,
            zoomControlOptions: {
                style: google.maps.ZoomControlStyle.SMALL
            }
        };
        var maps = new google.maps.Map(vacancyMap, myOptions);

        theMaps.push(maps);

        var markerIcon = new google.maps.MarkerImage(
                      '/Content/_assets/img/icon-location.png',
                      null, /* size is determined at runtime */
                      null, /* origin is 0,0 */
                      null, /* anchor is bottom center of the scaled image */
                      new google.maps.Size(20, 32));

        var marker = new google.maps.Marker({
            icon: markerIcon,
            position: latlng,
            map: maps
        });

    });

    function calcRoute(transportMode, latLong, journeyTime, mapNumber) {

        directionsDisplay[mapNumber].setMap(theMaps[mapNumber]);

        var request = {
            origin: originLocation,
            destination: latLong,
            travelMode: google.maps.TravelMode[transportMode]
        };
        directionsService[mapNumber].route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {

                $(journeyTime).text(response.routes[0].legs[0].duration.text);

                directionsDisplay[mapNumber].setDirections(response);
            }
        });
    }

    $('.select-mode').on('change', function () {
        var $this = $(this),
            $thisVal = $this.val(),
            $thisVacLink = $this.closest('.search-results__item').find('.vacancy-link'),
            $thisLat = $thisVacLink.attr('data-lat'),
            $thisLong = $thisVacLink.attr('data-lon'),
            $thisLatLong = new google.maps.LatLng($thisLat, $thisLong),
            $durationElement = $this.next('.journey-time'),
            $mapNumber = $this.closest('.search-results__item').index();

        calcRoute($thisVal, $thisLatLong, $durationElement, $mapNumber);
    });

    $('.search-results__item .summary-style').on('click', function (originLocation) {
        var $this = $(this),
            $thisVal = $this.next('.detail-content').find('.select-mode option:selected').val(),
            $thisVacLink = $this.closest('.search-results__item').find('.vacancy-link'),
            $thisMap = $this.closest('.search-results__item').find('.map'),
            $thisLat = $thisVacLink.attr('data-lat'),
            $thisLong = $thisVacLink.attr('data-lon'),
            $thisLatLong = new google.maps.LatLng($thisLat, $thisLong),
            $durationElement = $this.next('.detail-content').find('.journey-time'),
            $mapNumber = $this.closest('.search-results__item').index();

        calcRoute($thisVal, $thisLatLong, $durationElement, $mapNumber);

    });

    google.maps.event.addDomListener(window, 'load', initialize);

    $('#editSearchToggle').on('click', function () {
        initialize();
    });
});