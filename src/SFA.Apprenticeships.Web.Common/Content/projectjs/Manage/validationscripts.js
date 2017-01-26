﻿$(document).ready(function () {

    $("form").each(function () {
        var validator = $.data(this, 'validator');

        if (validator === undefined) return;

        var settings = validator.settings;
        var oldErrorFunction = settings.errorPlacement;
        var oldSuccessFunction = settings.success;
        settings.errorPlacement = function (error, element) {
            $(element).removeClass('error');

            if ($(element).parents('.form-date').length === 0)
                $(element).parent('.form-group').addClass("error");
            else
                $(element).parents('form-group').last().addClass('error');

            oldErrorFunction(error, element);
        };
        settings.success = function (label, element) {
            $(element).parent('.form-group').removeClass("error");
            oldSuccessFunction(label, element);

        };
        settings.showErrors = function (errorMap, errorList) {
            //See http://stackoverflow.com/questions/7935568/jquery-validation-show-validation-summary-during-eager-validation
            this.defaultShowErrors();
        };
    });

    $('.inline-fixed').not('#qualifications-panel .inline-fixed, #workexperience-panel .inline-fixed, #training-history-panel .inline-fixed').on('blur keyup', '.form-control', function () {
        var $this = $(this),
            $thisParent = $this.closest('.inline-fixed');

        setTimeout(function () {
            if ($thisParent.find('.error-message').length > 0) {
                $thisParent.addClass('error');
            } else {
                $thisParent.removeClass('error');
            }
        }, 10);
    });

    $('.inline-fixed').find('[data-valmsg-for]').each(function () {
        var $this = $(this);

        $this.appendTo($this.closest('.inline-fixed'));
    });

    $('[data-valmsg-for]').each(function () {
        $(this).attr('aria-live', 'polite');
    });
});

/*
 * This is a fix to support client side validation of checkboxes being 
 * specific values, see links below for more details
 * http://stackoverflow.com/questions/9808794/validate-checkbox-on-the-client-with-fluentvalidation-mvc-3
 * http://pastebin.com/7uzUJz71
 */
(function ($) {
    if ($.validator === undefined) return;

    $.validator.unobtrusive.adapters.add('equaltovalue', ['valuetocompare'], function (options) {
        options.rules['equaltovalue'] = options.params;
        if (options.message != null) {
            options.messages['equaltovalue'] = options.message;
        }
    });

    $.validator.addMethod('equaltovalue', function (value, element, params) {
        if ($(element).is(':checkbox')) {
            if ($(element).is(':checked')) {
                return params.valuetocompare.toLowerCase() === 'true';
            } else {
                return params.valuetocompare.toLowerCase() === 'false';
            }
        }
        return params.valuetocompare.toLowerCase() === value.toLowerCase();
    });
})(jQuery);

$(document).ready(function () {
    // -- Password strength indicator - this was taken out of the minified scripts.js so that id etc can be changed.

    $("#Password").keyup(function () {
        initializeStrengthMeter();
    });

    function initializeStrengthMeter() {
        $("#pass_meter").pwStrengthManager({
            password: $("#Password").val(),
            minChars: "8",
            advancedStrength: true
        });
    }

    $('button, input[type="submit"], a.button').not('#qualifications-panel .button, #workexperience-panel .button, #addTrainingCourseBtn, .no-handler').on('click', function () {
        var $this = $(this),
            $thisText = $this.text();

        if ($this.is('#save-button')) {
            $this.text('Saving').addClass('disabled');
        } else {
            $this.text('Loading').addClass('disabled');
        }

        setTimeout(function () {
            if ($('.form-group.error').length > 0) {
                $this.text($thisText).removeClass('disabled');
            } else if ($('.block-label.error').length > 0) {
                $this.text($thisText).removeClass('disabled');
            }
            $this.attr('disabled');
        }, 50);
    });

    $('.grid-wrapper').on('click', '.delete-draft', function () {
        var $this = $(this),
            delDraft = $this[0];

        $(delDraft).addClass('disabled').blur();
    });

    // -- Sort out styling issues on details page

    $('[itemscope]').find('[style]').not('iframe').removeAttr('style');

    // -- Remove unnecessary breaks from details page

    $('[itemscope] p').contents().each(function () {
        if (this.nodeType === 3 && $.trim(this.nodeValue).length) {
            $(this).wrap('<span class="temp_span">')
        }

    });

    $('p .temp_span').each(function () {
        var $br = $(this).nextUntil('.temp_span');
        $br.filter('br:gt(0)').remove()
    }).replaceWith(function () {
        return $(this).text()
    });
});