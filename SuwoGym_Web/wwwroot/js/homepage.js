
// Toggle menú móvil
function toggleMenu() {
    const navLinks = document.getElementById('navLinks');
    navLinks.classList.toggle('open');
}

// Mostrar día del horario
function showDay(dayId) {
    // Ocultar todos los contenidos
    const contents = document.querySelectorAll('.horario-content');
    contents.forEach(content => {
        content.classList.remove('active');
    });

    // Mostrar el contenido seleccionado
    const selectedContent = document.getElementById(dayId);
    if (selectedContent) {
        selectedContent.classList.add('active');
    }

    // Actualizar tabs activos
    const tabs = document.querySelectorAll('.horario-tab');
    tabs.forEach(tab => {
        tab.classList.remove('active');
        // Activar el tab correspondiente
        if (tab.textContent.trim().toLowerCase() ===
            document.querySelector(`#${dayId}`).parentElement
                .querySelector('.section-title')?.textContent.trim().toLowerCase() ||
            tab.getAttribute('onclick')?.includes(dayId)) {
            tab.classList.add('active');
        }
    });

    // Método alternativo: buscar por onclick
    tabs.forEach(tab => {
        const onclickAttr = tab.getAttribute('onclick');
        if (onclickAttr && onclickAttr.includes(dayId)) {
            tab.classList.add('active');
        }
    });
}

// Cerrar menú al hacer click en un enlace (móvil)
document.querySelectorAll('.nav-links a').forEach(link => {
    link.addEventListener('click', () => {
        document.getElementById('navLinks').classList.remove('open');
    });
});

// Cerrar menú al hacer scroll
let scrollTimeout;
window.addEventListener('scroll', () => {
    clearTimeout(scrollTimeout);
    scrollTimeout = setTimeout(() => {
        document.getElementById('navLinks').classList.remove('open');
    }, 100);
});

// Smooth scroll para todos los enlaces internos
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            const navHeight = document.querySelector('.navbar').offsetHeight;
            const targetPosition = target.getBoundingClientRect().top + window.pageYOffset - navHeight;
            window.scrollTo({
                top: targetPosition,
                behavior: 'smooth'
            });
        }
    });
});