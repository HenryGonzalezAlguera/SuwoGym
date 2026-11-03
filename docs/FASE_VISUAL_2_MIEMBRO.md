# Fase Visual 2 — Experiencia del Miembro

## Objetivo

Mejorar la presentación de las **nueve vistas Razor existentes** del módulo
Miembro sin modificar sus datos, rutas MVC, formularios, API o procedimientos
almacenados.

La Fase 1 estableció el sistema de diseño compartido en `suwo-polish.css`.
La Fase 2 añade `wwwroot/css/suwo-member.css`, que se carga después y utiliza
selectores restringidos a `.suwo-member-view` para evitar afectar a los
módulos Administrador y Entrenador.

## Pantallas incluidas

| Vista | Mejora |
| --- | --- |
| `Miembro/Perfil` | Datos personales, membresía y entrenador en secciones más legibles |
| `Miembro/Pagos_Lista` | Encabezado, contador de pagos, tabla y estado vacío |
| `Miembro/Pagos_Ver` | Secciones de detalle, importes y visualización del total |
| `Miembro/Clases_Disponibles` | Contador, tarjetas de clases, disponibilidad y CTA de reserva |
| `Miembro/Reservas_Lista` | Encabezado, contador, tabla y estado vacío |
| `Miembro/Historial_Asistencias` | Encabezado, indicadores, tabla y estado sin registros |
| `Miembro/Rutinas_Lista` | Encabezado, contador y presentación de rutinas |
| `Miembro/Rutinas_Ver` | Presentación coherente del detalle de rutina |
| `Miembro/Rutinas_Ejercicios` | Presentación coherente de ejercicios y estado vacío |

La mayoría de ajustes son clases CSS y contenedores de presentación. Los
contadores usan `Model?.Count ?? 0`, sin consultas adicionales. En la vista
de detalle de pago también se corrigió el atributo HTML `xlass` a `class`
para que el separador reciba el estilo.

## Ajuste de estados vacíos (PR de seguimiento)

Cuando una lista está vacía, la API devuelve `404 NotFound` con un texto
específico. Antes, la WEB representaba ese texto como alerta informativa
**además** de mostrar el estado vacío diseñado para la página.

En las vistas de clases, reservas, asistencia, pagos y rutinas se evita esa
alerta redundante **solo si el mensaje recibido coincide exactamente** (sin
distinguir mayúsculas/minúsculas) con el texto esperado de ausencia de datos.
Una respuesta inesperada seguirá siendo visible como alerta. Los mensajes de
éxito y error de operaciones (`TempData`) permanecen intactos. No cambian
las respuestas API ni los contratos de las acciones MVC.

## Contratos preservados

- Todas las acciones `asp-action` y `asp-controller` originales.
- Formulario POST de reservar clase y formulario POST de cancelar reserva.
- Métodos de pago, clases, reserva, asistencia y rutinas siguen usando sus
  modelos existentes.
- No se modificaron controladores, entidades, servicios, SQL ni endpoints API.
- El menú y la autenticación conservan la estructura validada en Fase Visual 1.

## Revisión visual manual desde Visual Studio

1. Abrir solución `SuwoGym.slnx`, detener depuración y sincronizar `main`.
2. **Compilar → Recompilar solución**, ejecutar API y WEB con perfiles HTTPS.
3. Probar navegador en zoom **100 %** (Ctrl+0), luego usar Ctrl+F5 para
   actualizar estilos.
4. Entrar con una cuenta de Miembro de desarrollo y recorrer las nueve vistas.
5. Probar en desktop, portátil y móvil; verificar que tablas anchas se desplacen
   horizontalmente y que los botones de acción no se oculten.
6. Probar una cuenta sin pagos/reservas/asistencias y otra con datos de prueba.
7. Reservar y cancelar una clase ficticia para confirmar el flujo; no usar
   datos reales ni compartir capturas con datos sensibles.
8. Si se observan regresiones, documentar la ruta exacta y captura para
   introducir una corrección puntual en GitHub.

## Alcance de las comprobaciones automatizadas

GitHub Actions verifica compilación de `SuwoGym_API`,
`SuwoGym_Web` y `SuwoGym.Tests`; esas pruebas no verifican
el aspecto real en Edge ni la interacción contra SQL Server.

La siguiente fase recomendada es la mejora individual de pantallas de
Administrador, seguida de Entrenador, conservando los estilos del equipo.
