document.addEventListener('DOMContentLoaded', () => {
    function setActiveSidebarLink() {
        const currentPath = window.location.pathname;
        const homePath = "/";

        const sidebarLinks = document.querySelectorAll('.sidebar nav a');

        sidebarLinks.forEach(link => {
            const linkPath = link.getAttribute('href');
            link.classList.remove('active');

            if (linkPath !== homePath && currentPath.toLowerCase().startsWith(linkPath.toLowerCase())) {
                link.classList.add('active');
            }
        });

        const homeLink = document.querySelector('.sidebar nav a[href="/"]');
        if (currentPath === homePath && homeLink) {
            homeLink.classList.add('active');
        }
    }

    setActiveSidebarLink();
});