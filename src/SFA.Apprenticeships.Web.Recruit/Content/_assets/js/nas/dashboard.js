$(function () {

    $(document).on('change', '#page-size', function () {
        var form = $("form");
        var input = $("<input>").attr("type", "hidden").attr("name", "ChangePageSizeAction").val("ChangePageSize");
        form.append(input);
        form.submit();
    });

    $(document).on('change', '#provider-site', function () {
        var form = $("form");
        var input = $("<input>").attr("type", "hidden").attr("name", "ChangeProviderSiteAction").val("ChangeProviderSite");
        form.append(input);
        form.submit();
    });

});