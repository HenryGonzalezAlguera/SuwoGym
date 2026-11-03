using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SuwoGym_Web.Filters
{
    /// <summary>
    /// Protege las rutas MVC con los datos de sesión creados al iniciar sesión.
    /// La API continúa siendo la autoridad final para validar JWT y permisos.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly int _rolRequerido;

        // Sin argumento: cualquier usuario autenticado.
        // Con 1, 2 o 3: Administrador, Entrenador o Miembro respectivamente.
        public SessionAuthorizeAttribute(int rolRequerido = 0)
        {
            if (rolRequerido is < 0 or > 3)
                throw new ArgumentOutOfRangeException(nameof(rolRequerido));

            _rolRequerido = rolRequerido;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sesion = context.HttpContext.Session;
            var autenticado = sesion.GetString("Autenticado") == "1";
            var idUsuario = sesion.GetInt32("IdUsuario");
            var idRol = sesion.GetInt32("IdRol");
            var token = sesion.GetString("Token");

            // Una sesión parcial o antigua no autoriza el acceso.
            if (!autenticado || idUsuario is null or <= 0 ||
                idRol is null or < 1 or > 3 || string.IsNullOrWhiteSpace(token))
            {
                context.Result = new RedirectToActionResult(
                    "Index", "Home", null);
                return;
            }

            // Mantiene el comportamiento previo: vuelve al panel principal.
            if (_rolRequerido != 0 && idRol != _rolRequerido)
            {
                context.Result = new RedirectToActionResult(
                    "Principal", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
