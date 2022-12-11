// Inschrijvingen Page
$(function highlightButton_InschrPage() {
    // Get full URL
    var url = window.location.href;

    // Pass Every "a" Tag
    $("#categories a").each(function () {
        // Checks address bar for match
        if (url == (this.href)) {
            // Highlights pressed button by adding a coresponding class
            $(this).closest("a").addClass("active");
        }
    })
})

// Disable Login Dropdown if User is on Login or Register Page
$(function disableToast_LoginPage() {
    // Get Full URL
    var url = window.location.href;
    var loginDropdown = document.getElementById("dropdown-login-form");
    if (url.includes("/Account/LoginForm") || url.includes("/Account/Login") || url.includes("/Account/Register")) {
        loginDropdown.classList.add("d-none");
        loginDropdown.classList.remove("d-block");
    }
    else {
        loginDropdown.classList.remove("d-none");
        loginDropdown.classList.add("d-block");
    }
})

// Toast that pops up after Login
$(function showSnackbar() {
    var toast = document.getElementById("snackbar");
    toast.style.opacity = "1";
    
    // After 2 seconds, remove the show class from element
    setTimeout(function () { toast.style.opacity = toast.style.opacity.replace("1", "0"); }, 4000);
})