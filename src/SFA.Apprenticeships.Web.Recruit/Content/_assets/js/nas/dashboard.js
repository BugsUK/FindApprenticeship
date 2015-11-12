$(function () {

    $(document).on('change', '#provider-site', function () {
        var form = $('form[name="provider-site-form"]');
        var input = $("<input>").attr("type", "hidden").attr("name", "ChangeProviderSiteAction").val("ChangeProviderSite");
        form.append(input);
        form.submit();
    });

    $(document).on('change', '#page-size', function () {
        var form = $('form[name="vacancy-search-form"]');
        var input = $("<input>").attr("type", "hidden").attr("name", "SearchVacanciesAction").val("SearchVacancies");
        form.append(input);
        form.submit();
    });

});