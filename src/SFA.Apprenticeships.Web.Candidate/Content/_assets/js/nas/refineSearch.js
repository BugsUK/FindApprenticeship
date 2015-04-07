// moves html to correct location for refining search without the need
// to make large changes to the control library
$(document).ready(function () {
    var $keywordsInput = $("#Keywords"),
        $searchField = $("#SearchField");

    if ($('#searchHome').length > 0) {
        $keywordsInput.wrap('<div class="input-withlink input-withlink--all-select"></div>');

        $searchField.insertBefore($keywordsInput).removeClass('hidden');

        setSelectControl($('#SearchField'));
    } else {
        $searchField.insertBefore($keywordsInput).removeClass('hidden');

        $searchField.find('option:last-of-type').attr('hidden', 'hidden');
    }

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

});