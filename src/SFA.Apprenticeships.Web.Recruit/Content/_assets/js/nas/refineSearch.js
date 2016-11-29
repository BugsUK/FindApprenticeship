// moves html to correct location for refining search without the need
// to make large changes to the control library
$(document).ready(function () {

    function setRefineSearch() {
        var $keywordsInput = $("#Keywords"),
            $searchField = $("#SearchField");

        if ($('#searchHome').length > 0) {
            $keywordsInput.wrap('<div class="input-withlink input-withlink--all-select"></div>');

            $searchField.insertBefore($keywordsInput).removeClass('hidden');

            setSelectControl($('#SearchField'));
        } else {
            $searchField.insertBefore($keywordsInput).removeClass('hidden');

            if ($searchField.find('option:last-child').text() == "-- Refine search --") {
                $searchField.find('option:last-child').attr('disabled', true);
            }
        }
    }

    function setSelectControl(that) {
        //var $this = that,
        //    $container = $this.closest('.input-withlink--all-select');

        //if ($this.val() != "All") {
        //    $container.addClass('auto-width');
        //    $container.css('padding-left', $this.outerWidth() + 'px');
        //    $container.find('input').focus();
        //} else {
        //    $container.removeClass('auto-width');
        //    $container.css('padding-left', $this.outerWidth() + 'px');
        //    $container.find('input').focus();
        //}
    }

    $(document).on('change', '.all-select', function () {
        var $this = $(this);
        setSelectControl($this);
    });

    setRefineSearch();

    $(document).on("setRefineSearch", function() {
        setRefineSearch();
    });

});