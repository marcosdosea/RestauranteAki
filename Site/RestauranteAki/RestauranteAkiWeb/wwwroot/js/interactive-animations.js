document.addEventListener('DOMContentLoaded', function () {
    document.body.classList.add('bg-animated');
    AOS.init({
        once: true,      
        duration: 700,  
        easing: 'ease-out-quad' 
    });

});
document.querySelectorAll('.premium-action-btn').forEach(button => {
    button.addEventListener('mousemove', e => {
        const rect = button.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;
        button.style.setProperty('--mouse-x', `${x}px`);
        button.style.setProperty('--mouse-y', `${y}px`);
    });
});