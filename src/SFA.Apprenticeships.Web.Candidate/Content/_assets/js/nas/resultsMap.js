window.googleMapsScriptLoaded = function () {
    $(window).trigger('googleMapsScriptLoaded');
};

$(function () {

    var apiScriptLoaded = false,
        apiScriptLoading = false,
        $window = $(window),
        $body = $('body'),
        apprLatitude,
        apprLongitude,        
        apprZoom = 9,
        radiusCircle,
        theLatLon,
        originLocation,
        markerIcon,
        selectedIcon,
        resultMaps;

    var ResultMap = function (resultItem) {
        this.resultItem = resultItem[0];
        this.resultItem.resultMap = this;
        this.container = resultItem.find(".map-container");
        this.map = resultItem.find(".map-placeholder");
        if (this.map && this.map.length) {
            this.map[0].resultMap = this;
        }
        this.staticMap = resultItem.find(".static-map");
        this.link = resultItem.find('.vacancy-link');
        this.title = this.link.html();
        this.vacancyId = this.link.attr('data-vacancy-id');
        this.lat = this.link.attr('data-lat');
        this.lon = this.link.attr('data-lon');        
        this.isEmployerAnonymous = this.link.attr('data-is-employer-anonymous');
        this.duration = resultItem.find('.journey-time');
        this.travelMode = resultItem.find('.select-mode');
        this.journeyTime = resultItem.find('.journey-time');
        this.latlng = new google.maps.LatLng(this.lat, this.lon);
        this.directionsDisplay = new google.maps.DirectionsRenderer({ suppressMarkers: true });
        this.directionsService = new google.maps.DirectionsService();
        this.marker = null;

        //TODO: perhaps replace server side
        var mapLinkHref = resultItem.find(".map-links:first");
        if (mapLinkHref.attr("href")) {
            mapLinkHref.attr("href", mapLinkHref.attr("href").replace('LocationLatLon', theLatLon));
        }

        resultItem.mouseover(function () {
            if (this.resultMap != null && this.resultMap.marker != null) {
                this.resultMap.marker.setIcon(selectedIcon);
                this.resultMap.marker.setZIndex(1000);
            }
        }).mouseleave(function () {
            if (this.resultMap != null && this.resultMap.marker != null) {
                this.resultMap.marker.setIcon(markerIcon);
                this.resultMap.marker.setZIndex(0);
            }
        });
    }

    function getResultMap(element) {
        var vacancyId = element.closest(".search-result").find(".vacancy-link").attr('data-vacancy-id');
        return _.find(resultMaps, function(resultMap) {
            return resultMap.vacancyId == vacancyId;
        });
    }

    function allResultsAreSameLocation() {
        if (!resultMaps || resultMaps == null || resultMaps.length == 0) { return false; }
        var matching = _.where(resultMaps, { lat: resultMaps[0].lat, lon: resultMaps[0].lon });
        return matching.length == resultMaps.length;
    }

    function checkGoogleMapsApiScriptLoaded() {
        if (!apiScriptLoaded && !apiScriptLoading) {
            apiScriptLoading = true;
            $body.append('<script src="https://maps.googleapis.com/maps/api/js?v=3&callback=googleMapsScriptLoaded&client=gme-skillsfundingagency&channel=findapprenticeship' + '"></script>');
        }
    }

    $window.on('googleMapsScriptLoaded', function () {

        markerIcon = new google.maps.MarkerImage('/Content/_assets/img/icon-location.png', null, null, null, new google.maps.Size(20, 32));
        selectedIcon = new google.maps.MarkerImage('/Content/_assets/img/icon-location-selected.png', null, null, null, new google.maps.Size(20, 32));

        apiScriptLoaded = true;
        initialize();
    });

    $window.on('resultsReloaded', function () {
        initialize();
    });

    function initialize() {

        checkGoogleMapsApiScriptLoaded();
        if (!apiScriptLoaded) return;

        apprLatitude = Number($('#Latitude').val());
        apprLongitude = Number($('#Longitude').val());
        originLocation = new google.maps.LatLng(apprLatitude, apprLongitude);

        if (apprLatitude == 0 || apprLongitude == 0) {
            $('#map-canvas').parent().hide();
        }

        var apprMiles = Number($('#loc-within').val());
        theLatLon = apprLatitude + ',' + apprLongitude;

        resultMaps = [];
        $(".search-result").each(function() {
             resultMaps.push(new ResultMap($(this)));
        });

        var mapCenter = { lat: apprLatitude, lng: apprLongitude };
        var mapOptions = { center: mapCenter, zoom: apprZoom, panControl: false, zoomControl: true, mapTypeControl: false, scaleControl: false, streetViewControl: false, overviewMapControl: false, scrollwheel: false };
        var summaryMap = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

        var distanceCircle = { strokeColor: '#005ea5', strokeOpacity: 0.8, strokeWeight: 2, fillColor: '#005ea5', fillOpacity: 0.25, map: summaryMap, center: mapOptions.center, radius: apprMiles * 1609.344 };
        radiusCircle = new google.maps.Circle(distanceCircle);

        var radiusBounds = radiusCircle.getBounds();
        var bounds = new google.maps.LatLngBounds();
        _.each(resultMaps, function(resultMap) { bounds.extend(resultMap.latlng); });
        setMarkers(summaryMap, resultMaps);

        if (!allResultsAreSameLocation() && $('#LocationType').val() == 'NonNational') {
            summaryMap.fitBounds(bounds);
        } else if (apprMiles > 0 && $('#LocationType').val() == 'NonNational') {
            summaryMap.fitBounds(radiusBounds);
            var zoomChangeBoundsListener = google.maps.event.addListenerOnce(summaryMap, 'bounds_changed', function() {
                if (this.getZoom()) {
                    this.setZoom(this.getZoom() + 1);
                }
            });
            setTimeout(function () { google.maps.event.removeListener(zoomChangeBoundsListener) }, 2000);
        } else if (apprMiles == 0) {
            summaryMap.setZoom(5);
        }
        
        if (apprMiles > 0 && $('#LocationType').val() == 'National') {
            summaryMap.fitBounds(radiusBounds);
        }
    }

    function setMarkers(summaryMap) {

        for (var i = 0; i < resultMaps.length; i++) {
            var resultMap = resultMaps[i];
            var myLatLng = new google.maps.LatLng(resultMap.lat, resultMap.lon);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: summaryMap,
                animation: google.maps.Animation.DROP,
                icon: markerIcon,
                title: resultMap.title
            });

            marker.resultItem = resultMap.resultItem;
            resultMap.marker = marker;

            google.maps.event.addListener(marker, 'mouseover', function () {
                this.setIcon(selectedIcon);
                this.setZIndex(1000);
            });

            google.maps.event.addListener(marker, 'click', function () {
                this.setIcon(selectedIcon);
                this.setZIndex(1000);
                this.resultItem.scrollIntoView();
            });

            google.maps.event.addListener(marker, 'mouseout', function () {
                this.setIcon(markerIcon);
                this.setZIndex(0);
            });
        }
    }

    function lazyLoadMaps() {
        //If desktop - load script and set lazy load on all result maps.
        if (apiScriptLoaded) {
            _.each(resultMaps, function(resultMap) {
                lazyLoadMap(resultMap);
            });
        }
    }

    function lazyLoadMap(resultMap) {
        //If desktop - load script and set lazy load on all result maps.
        if (apiScriptLoaded) {
            resultMap.staticMap.hide();
            resultMap.map.addClass("map");
            resultMap.map.lazyLoadGoogleMaps(
            {
                api_key: 'gme-skillsfundingagency',
                callback: function (container, lazyMap) {
                    var lazyOptions = {
                        zoom: 12,
                        center: resultMap.latlng,
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
                    new google.maps.Marker({ position: resultMap.latlng, map: lazyMap, icon: markerIcon });
                    resultMap.directionsDisplay.setMap(lazyMap);
                }
            });
        }
    }

    $(document).on('click', '.mob-map-trigger', function () {
        var showMapLink = $(this);
        checkGoogleMapsApiScriptLoaded();

        $window.on('googleMapsScriptLoaded', function () {
            showMobileMap(showMapLink);
        });

        if (apiScriptLoaded) {
            showMobileMap(showMapLink);
        }
    });

    function showMobileMap(showMapLink) {
        var resultMap = getResultMap(showMapLink);
        lazyLoadMap(resultMap);
        var mobLatlng = new google.maps.LatLng(resultMap.lat, resultMap.lon);
        showMapLink.toggleClass('map-closed');
        resultMap.container.toggle();

        var mobMapOptions = {
            zoom: 12,
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

        var theMobMap = new google.maps.Map(resultMap.map[0], mobMapOptions);
        new google.maps.Marker({ position: mobLatlng, map: theMobMap, icon: markerIcon });
        resultMap.directionsDisplay.setMap(theMobMap);

        setTimeout(function () {
            google.maps.event.trigger(theMobMap, 'resize');
            theMobMap.setCenter(mobLatlng);
        }, 300);
    }

    function calcRoute(element, updatedMode) {
        var resultMap = getResultMap(element);
        lazyLoadMap(resultMap);
        var dcsUri = updatedMode ? '/apprenticeships/journeytimechange' : '/apprenticeships/journeytime';

        var request = {
            origin: originLocation,
            destination: new google.maps.LatLng(resultMap.lat, resultMap.lon),
            travelMode: google.maps.TravelMode[resultMap.travelMode.val()]
        };
        resultMap.directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                var responseJourneyTime = response.routes[0].legs[0].duration.text;
                resultMap.journeyTime.text(responseJourneyTime);
                if (resultMap.isEmployerAnonymous !== "True") {
                    resultMap.directionsDisplay.setDirections(response);
                }
                Webtrends.multiTrack({ argsa: ['DCS.dcsuri', dcsUri, 'WT.dl', '99', 'WT.ti', 'Search Results Journey Time Option', 'journeyMethods', resultMap.travelMode.val(), 'journeyTime', responseJourneyTime] });
            }
        });
    }

    $(document).on('change', '.select-mode', function () {

        checkGoogleMapsApiScriptLoaded();

        $window.on('googleMapsScriptLoaded', function () {
            calcRoute($(this), true);
        });

        if (apiScriptLoaded) {
            calcRoute($(this), true);
        }
    });

    $(document).on('click', '.journey-trigger', function () {

        checkGoogleMapsApiScriptLoaded();

        var $this = $(this);

        $window.on('googleMapsScriptLoaded', function () {
            calcRoute($this, false);
        });

        if (apiScriptLoaded) {
            calcRoute($(this), false);
        }
    });

    if ($('#editSearchPanel').css('display') == 'block') {
        initialize();
    }

    $(document).on('click', '#editSearchToggle', function () {
        initialize();
    });
});