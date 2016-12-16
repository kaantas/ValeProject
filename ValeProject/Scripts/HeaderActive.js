$(function () {
    var getHref = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);
    var Href = "/Home/" + getHref;
    $(".nav>li>a")
        .each(function () {
            if ($(this).attr("href") == Href || $(this).attr("href") == '') {
                var id = ".nav>#" + getHref;
                $(id).addClass("active");
            }
        });
});