// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    // Get full URL
    var url = window.location.href;

    // Passes every "a" tag
    $("#categories a").each(function () {
        // Checks address bar for match
        if (url == (this.href)) {
            $(this).closest("a").addClass("active");
        }
    });
});