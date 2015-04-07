// moves html to correct location for refining search without the need
// to make large changes to the control library
$(document).ready(function () {
    var $keywordsInput = $("#Keywords");

    $keywordsInput.wrap('<div class="input-withlink input-withlink--all-select"></div>');

    $("#SearchField").insertBefore($keywordsInput).removeClass('hidden');

    function setSelectControl(that) {
        var $this = that,
            $container = $this.closest('.input-withlink--all-select');

        if ($this.val() != "All") {
            $container.addClass('auto-width');
            $container.css('padding-left', $this.outerWidth() + 'px');
            $container.find('input').focus();
        } else {
            $container.removeClass('auto-width');
            $container.css('padding-left', $this.outerWidth() + 'px');
            $container.find('input').focus();
        }
    }

    $('.all-select').on('change', function () {
        var $this = $(this);

        setSelectControl($this);
    });

    if ($('#getLocation').length > 0) {
        setSelectControl($('#SearchField'));
    }
});