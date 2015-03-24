window.googleMapsScriptLoaded = function () {
    $(window).trigger('googleMapsScriptLoaded');
};

$(function () {

    /*Show map with radius in results*/

    var apiScriptLoaded    = false,
		apiScriptLoading   = false,
        $window            = $(window),
        $body              = $('body'),
        apprLatitude       = Number($('#Latitude').val()),
        apprLongitude      = Number($('#Longitude').val()),
        apprMiles          = Number($('#loc-within').val()),
        resultsPage        = $('#results-per-page').val(),
        numberOfResults    = $('.vacancy-link').length,
        distanceOfLast     = $('.search-results__item:last-child .distance-value').html(),
        firstLat           = $('.vacancy-link:first-of-type').attr('data-lat'),
        firstLon           = $('.vacancy-link:first-of-type').attr('data-lon'),
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
        bounds,
        originLocation,
        latLngList         = [],
        theLatLon          = apprLatitude + ',' + apprLongitude,
        markerIcon,
        selectedIcon,
        vacanciesSame      = true;

    $window.on('googleMapsScriptLoaded', function () {
        apiScriptLoaded = true;
        bounds = new google.maps.LatLngBounds();
        originLocation = new google.maps.LatLng(apprLatitude, apprLongitude);
        markerIcon = new google.maps.MarkerImage('/Content/_assets/img/icon-location.png', null, null, null, new google.maps.Size(20, 32));
        selectedIcon = new google.maps.MarkerImage('/Content/_assets/img/icon-location-selected.png', null, null, null, new google.maps.Size(20, 32));

        lazyLoadMaps();
        setGoogDirectionsServices();
        initialize();
        loadScript();
    });

    $('.map-links').each(function(){
        var $this = $(this),
        aHref = $this.attr('href');

        $this.attr('href', aHref.replace('LocationLatLon', theLatLon));
    });

    function setGoogDirectionsServices() {
        for (var i = 0; i < vacancyLinks.length; i++) {
            directionsDisplay[i] = new google.maps.DirectionsRenderer({ suppressMarkers: true });
            directionsService[i] = new google.maps.DirectionsService();
        }
    }

    for (var i = 0; i < vacancyLinks.length; i++) {
        var lat = $(vacancyLinks[i]).attr('data-lat'),
            longi = $(vacancyLinks[i]).attr('data-lon'),
            title = $(vacancyLinks[i]).html(),
            id = $(vacancyLinks[i]).attr('data-vacancy-id');

        vacancies[i] = [lat, longi, title, id];

        if (lat != firstLat && longi != firstLon) {
            vacanciesSame = false;
        }
    }

    if (apprLatitude == 0 || apprLongitude == 0) {
        $('#map-canvas').parent().hide();
    }

    function initialize() {

        if (!apiScriptLoaded && !apiScriptLoading) {
            $body.append('<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&callback=googleMapsScriptLoaded&client=gme-skillsfundingagency' + '"></script>');
            apiScriptLoading = true;
        }

        if (!apiScriptLoaded) return true;

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

        if (!vacanciesSame && $('#LocationType').val() == 'NonNational') {
            map.fitBounds(bounds);
        } else if (apprMiles > 0 && $('#LocationType').val() == 'NonNational') {
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

        if (apprMiles > 0 && $('#LocationType').val() == 'National') {
            map.fitBounds(radiusBounds);
        }

    }

    function setMarkers(map, locations) {

        for (var i = 0; i < locations.length; i++) {
            var appship = locations[i];
            var myLatLng = new google.maps.LatLng(appship[0], appship[1]);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                animation: google.maps.Animation.DROP,
                icon: markerIcon,
                title: appship[2]
            });

            theMarkers.push(marker);
            latLngList.push(myLatLng);

            bounds.extend(latLngList[i]);

            var vacancyID = appship[3];

            bindMarkerClick(marker, map, vacancyID, markerIcon, selectedIcon);

            itemHover(markerIcon, selectedIcon);

        }

    }

    function bindMarkerClick(marker, map, vacancyID, markerIcon, selectedIcon) {
        google.maps.event.addListener(marker, 'mouseover', function () {
            marker.setIcon(selectedIcon);
            marker.setZIndex(1000);
        });

        google.maps.event.addListener(marker, 'click', function () {
            marker.setIcon(selectedIcon);
            marker.setZIndex(1000);
            $('[data-vacancy-id="' + vacancyID + '"]').closest('.search-results__item')[0].scrollIntoView();
        });

        google.maps.event.addListener(marker, 'mouseout', function () {
            marker.setIcon(markerIcon);
            marker.setZIndex(0);
        });
    }

    function itemHover(markerIcon, selectedIcon) {
        $('.search-results__item').mouseover(function () {
            var thisPosition = $(this).index();
            theMarkers[thisPosition].setIcon(selectedIcon);
            theMarkers[thisPosition].setZIndex(1000);

        });

        $('.search-results__item').mouseleave(function () {
            var thisPosition = $(this).index();
            theMarkers[thisPosition].setIcon(markerIcon);
            theMarkers[thisPosition].setZIndex(0);
        });

    }

    function lazyLoadMaps() {
        if ($('.content-container').css('font-size') !== '16px' && apiScriptLoaded) {
            (function ($, window, document) {
                $('.map').lazyLoadGoogleMaps(
                    {
                        api_key: 'gme-skillsfundingagency',
                        callback: function (container, lazyMap) {
                            var $container = $(container),
                                vacancyLink = $container.closest('.search-results__item').find('.vacancy-link'),
                                vacancyLat = vacancyLink.attr('data-lat'),
                                vacancyLon = vacancyLink.attr('data-lon'),
                                latlng = new google.maps.LatLng(vacancyLat, vacancyLon);

                            var lazyOptions = {
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

                            lazyMap.setOptions(lazyOptions);

                            new google.maps.Marker({ position: latlng, map: lazyMap, icon: markerIcon });

                            theMaps.push(lazyMap);

                        }
                    });
            })(jQuery, window, document);
        }
    }

    function loadScript() {
        if (!apiScriptLoaded && !apiScriptLoading) {
            $body.append('<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&callback=googleMapsScriptLoaded&client=gme-skillsfundingagency' + '"></script>');
            apiScriptLoading = true;
        }

        if (!apiScriptLoaded) return true;
    }

    $('.mob-map-trigger.map-closed').on('click', function () {
        var $this = $(this);

        loadScript();

        $window.on('googleMapsScriptLoaded', function () {
            showHideMaps($this);
        });

        if (apiScriptLoaded) {
            showHideMaps($this);
        }

    });

    function showHideMaps(that) {
        var $this = that,
            mobVacancyLink = $this.closest('.search-results__item').find('.vacancy-link'),
            mobVacancyLat = mobVacancyLink.attr('data-lat'),
            mobVacancyLon = mobVacancyLink.attr('data-lon'),
            mobLatlng = new google.maps.LatLng(mobVacancyLat, mobVacancyLon),
            mapContainer = $this.closest('.search-results__item').find('.map-container'),
            mobMap = mapContainer.find('.map')[0],
            $mapNumber = $this.closest('.search-results__item').index();

        $this.toggleClass('map-closed');
        mapContainer.toggle();

        var mobMapOptions = {
            zoom: 10,
            center: mobLatlng,
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

        var theMobMap = new google.maps.Map(mobMap, mobMapOptions);

        new google.maps.Marker({ position: mobLatlng, map: theMobMap, icon: markerIcon });

        theMaps[$mapNumber] = theMobMap;

        setTimeout(function () {
            google.maps.event.trigger(theMobMap, 'resize');
            theMobMap.setCenter(mobLatlng);
        }, 300);
    }

    function calcRoute(transportMode, thisLat, thisLong, journeyTime, mapNumber) {

        directionsDisplay[mapNumber].setMap(theMaps[mapNumber]);

        if (!apiScriptLoaded && !apiScriptLoading) {
            $body.append('<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&callback=googleMapsScriptLoaded&client=gme-skillsfundingagency' + '"></script>');
            apiScriptLoading = true;
        }

        if (!apiScriptLoaded) return true;

        var request = {
            origin: originLocation,
            destination: new google.maps.LatLng(thisLat, thisLong),
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
            $durationElement = $this.next('.journey-time'),
            $mapNumber = $this.closest('.search-results__item').index();

        loadScript();

        $window.on('googleMapsScriptLoaded', function () {
            calcRoute($thisVal, $thisLat, $thisLong, $durationElement, $mapNumber);
        });

        if (apiScriptLoaded) {
            calcRoute($thisVal, $thisLat, $thisLong, $durationElement, $mapNumber);
        }
    });

    $('.journey-trigger').on('click', function (originLocation) {
        var $this = $(this),
            $thisVal = $this.next('.detail-content').find('.select-mode option:selected').val(),
            $thisVacLink = $this.closest('.search-results__item').find('.vacancy-link'),
            $thisMap = $this.closest('.search-results__item').find('.map'),
            $thisLat = $thisVacLink.attr('data-lat'),
            $thisLong = $thisVacLink.attr('data-lon'),
            $durationElement = $this.next('.detail-content').find('.journey-time'),
            $mapNumber = $this.closest('.search-results__item').index();

        loadScript();

        $window.on('googleMapsScriptLoaded', function () {
            calcRoute($thisVal, $thisLat, $thisLong, $durationElement, $mapNumber);
        });

        if (apiScriptLoaded) {
            calcRoute($thisVal, $thisLat, $thisLong, $durationElement, $mapNumber);
        }

    });

    if ($('#editSearchPanel').css('display') == 'block') {
        initialize();
    }

    $('#editSearchToggle').on('click', function () {
        initialize();
    });

});