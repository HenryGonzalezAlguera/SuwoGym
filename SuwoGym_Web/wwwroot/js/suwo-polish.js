/*
 * SUWÓ GYM — Fase Visual 1
 * Mejora el estado activo de la navegación con rutas MVC reales.
 * No sustituye los eventos de menú móvil/collapse que define main.js.
 */
(() => {
  const sidebar = document.querySelector(".suwo-app #sidebar");
  if (!sidebar) return;

  const normalize = (pathname) => {
    try {
      return decodeURIComponent(pathname)
        .replace(/\/+$/, "")
        .toLowerCase() || "/";
    } catch {
      return pathname.replace(/\/+$/, "").toLowerCase() || "/";
    }
  };

  const pathname = normalize(window.location.pathname);
  const links = [...sidebar.querySelectorAll("a.nav-link[href]")];
  const active = links.find((link) => {
    const href = new URL(link.href, window.location.origin);
    return href.origin === window.location.origin &&
      normalize(href.pathname) === pathname;
  });

  // El bundle anterior compara únicamente el último segmento del path;
  // limpiamos su estado para que no marque erróneamente otra opción.
  links.forEach((link) => {
    link.classList.remove("active");
    link.removeAttribute("aria-current");
  });

  if (active) {
    active.classList.add("active");
    active.setAttribute("aria-current", "page");
  }
})();
