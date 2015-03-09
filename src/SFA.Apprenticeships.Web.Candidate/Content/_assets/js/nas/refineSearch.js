// moves html to correct location for refining search without the need
// to make large changes to the control library
$(document).ready(function() {

    $("#keyword-label").append($("#refine-search"));
    $("#refine-controls").insertAfter($("#keyword-label")).removeClass("hidden").addClass('toggle-content');
    $("#refine-search").removeClass("hidden");

    if ($("input[name='SearchField']:checked").val() != "All") {
        $("#refine-controls").toggle();
        $("#keyword-hint").toggle();
    }

    $("#refine-search").click(function() {
        $("#refine-controls").toggle();
        $("#keyword-hint").toggle();
    });
});