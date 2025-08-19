document.addEventListener('DOMContentLoaded', () => {

    // --- 1. Funcionalidade da Barra Lateral (Sidebar) com Hover ---
    const sidebarTrigger = document.getElementById('sidebar-trigger');
    const sidebar = document.querySelector('.sidebar');
    const mainContent = document.querySelector('.main-content');

    if (sidebarTrigger && sidebar && mainContent) {
        sidebarTrigger.addEventListener('mouseenter', () => {
            sidebar.classList.add('visible');
            mainContent.classList.add('shifted');
        });

        sidebar.addEventListener('mouseleave', () => {
            sidebar.classList.remove('visible');
            mainContent.classList.remove('shifted');
        });
    }


    // --- 2. Funcionalidade de Link Ativo na Barra Lateral ---
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