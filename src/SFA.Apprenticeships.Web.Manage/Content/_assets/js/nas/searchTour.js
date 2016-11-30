$(function () {

    $('#keywords-tab-control').on('click', function () {
        if ($('.joyride-content-wrapper').is(':visible')) {
            $("#browseTour").joyride('destroy');
            $("#savedSearchTour").joyride('destroy');

            $("#searchTour").joyride({
                'autoStart': true,
                'nextButton': true,
                'tipAnimation': 'pop'
            });
        }
    });

    $('#categories-tab-control').on('click', function () {
        if ($('.joyride-content-wrapper').is(':visible')) {
            $("#searchTour").joyride('destroy');
            $("#savedSearchTour").joyride('destroy');

            $("#browseTour").joyride({
                'autoStart': true,
                'nextButton': true,
                'tipAnimation': 'pop'
            });
        }
    });

    $('#saved-searches-tab-control').on('click', function () {
        if ($('.joyride-content-wrapper').is(':visible')) {
            $("#searchTour").joyride('destroy');
            $("#browseTour").joyride('destroy');

            $("#savedSearchTour").joyride({
                'autoStart': true,
                'nextButton': true,
                'tipAnimation': 'pop'
            });
        }
    });

    $('#runSearchHelp').on('click', function () {
        var joyrideAttached = false;

        setTimeout(function () {
            $('html').find('.joyride-close-tip').each(function () {
                $(this).attr('title', "Close tour")
            });
        }, 100);

        if ($('.joyride-tip-guide').css('visibility') == 'visible') {
            joyrideAttached = true;
        }

        if ($('#keywords-tab-control').hasClass('active')) {

            if (joyrideAttached) {
                $("#browseTour").joyride('destroy');
                $("#savedSearchTour").joyride('destroy');
            }

            $("#searchTour").joyride({
                'autoStart': true,
                'nextButton': true,
                'tipAnimation': 'pop'
            });

        } else if ($('#categories-tab-control').hasClass('active')) {

            if (joyrideAttached) {
                $("#searchTour").joyride('destroy');
                $("#savedSearchTour").joyride('destroy');
            }

            $("#browseTour").joyride({
                'autoStart': true,
                'nextButton': true,
                'tipAnimation': 'pop'
            });

        } else if ($('#saved-searches-tab-control').hasClass('active')) {

            if (joyrideAttached) {
                $("#searchTour").joyride('destroy');
                $("#browseTour").joyride('destroy');
            }

            $("#savedSearchTour").joyride({
                'autoStart': true,
                'nextButton': true,
                'tipAnimation': 'pop'
            });

        }

        return false;
    });
});