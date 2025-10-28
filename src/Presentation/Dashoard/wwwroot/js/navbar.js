window.navbarHandler = {
    init: function () {
        document.addEventListener("DOMContentLoaded", function () {
            var navbarCollapse = document.getElementById("navbarSupportedContent");
            if (!navbarCollapse) return;

            var navList = navbarCollapse.querySelector("ul.navbar-nav");

            navbarCollapse.addEventListener('show.bs.collapse', function () {
                if (navList) {
                    navList.style.display = "none";
                }
            });

            navbarCollapse.addEventListener('hide.bs.collapse', function () {
                if (navList) {
                    navList.style.display = "";
                }
            });
        });
    }
};
