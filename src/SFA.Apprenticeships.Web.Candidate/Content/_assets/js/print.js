$(function() {
    $('.print-trigger').on('click', function(e) {
        window.print();

        e.preventDefault();
    });
});