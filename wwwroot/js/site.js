
// Inschrijvingen Page
$(function () {
    // Get Full URL
    var url = window.location.href;

    // Pass Every "a" Tag
    $("#categories a").each(function () {
        // Checks address bar for match
        if (url == (this.href)) {
            $(this).closest("a").addClass("active");
        }
    });
});

// Prevent toast from popping up again after clicking dismiss,
$(function hideToast() {
    var toast = document.getElementById("toast-dismiss");
    toast.classList.remove("show");;
    toast.classList.add("hide");
})
// Shows toast again for next login
$(function showToast() {
    var toast = document.getElementById("toast-dismiss");
    toast.classList.remove("hide");;
    toast.classList.add("show");
})