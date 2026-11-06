# Fase 5 — Sistema visual del Administrador

## Objetivo

Modernizar las 22 vistas Razor actuales de `SuwoGym_Web/Views/Admin/` sin cambiar lógica de negocios, formularios, rutas, API, SQL Server ni sesiones.

La hoja `SuwoGym_Web/wwwroot/css/suwo-admin.css` se carga **después** de los estilos anteriores y todas sus reglas se limitan a `body.suwo-app.suwo-admin`. La clase se asigna en `Views/Shared/_Layout.cshtml` únicamente cuando el rol de sesión es `Administrador`. No altera las páginas de Miembro o Entrenador.

## Cobertura de pantallas

| Área | Archivos Razor |
| --- | --- |
| Dashboard | `Dashboard` |
| Empleados | `Empleados_Lista`, `Empleados_Agregar`, `Empleados_Ver`, `Empleados_Editar` |
| Clientes | `Clientes_Lista`, `Clientes_Ver`, `Clientes_Editar`, `Clientes_AsignarMembresia`, `Clientes_AsignarEntrenador` |
| Planes | `Planes_Lista`, `Planes_Ver`, `Planes_Modificar` |
| Clases | `Clases_Lista`, `Clases_Agregar` |
| Horarios | `Horarios_Lista`, `Horarios_Agregar`, `Horarios_Ver`, `Horarios_Editar` |
| Pagos | `Pagos_Registrar`, `Clientes_Pagos`, `Pagos_Ver` |

Todas las pantallas mantienen sus modelos Razor y sus acciones MVC. Se añadió/normalizó el título de pestaña, un rótulo contextual y estilos de encabezados, tarjetas, tablas, fichas, formularios, badges y mensajes de estado vacío. En Dashboard, las 4 tarjetas principales y los 6 indicadores secundarios tienen presentación específica; los valores siguen usando las propiedades del modelo existente.

## Restricciones y riesgos

- Sin cambios en `AdminController`, `SuwoGym_API`, base de datos, procedimientos almacenados ni credenciales.
- No se cambian los formularios `POST`, atributos `asp-action`, `asp-controller` o `asp-route-*`, ni nombres de campos.
- El restablecimiento temporal para un Entrenador sigue disponible **solo en Development**, tal como se implementó en fase 3D.
- No se requiere ejecutar ningún SQL nuevo.
- El build automatizado detecta errores de compilación, **no** demuestra que la UI se vea bien ni que SQL Server y el correo funcionen.

## Checklist manual en Visual Studio (Windows, entorno Development)

1. En la rama de prueba, **Compilar → Recompilar solución**. Ejecutar `SuwoGym_API` y `SuwoGym_Web` con sus perfiles HTTPS habituales.
2. Entrar con un usuario **Administrador de pruebas**, confirmar nombre y rol y comprobar navegación lateral.
3. **Dashboard:** revisar 4 KPI principales, 6 secundarios, pagos recientes y próximas clases con datos reales y listas vacías.
4. **Empleados:** listar, registrar un empleado de prueba, ver detalles y editar; verificar validaciones de formulario. En Development, comprobar la opción de acceso temporal de Entrenador sin compartir credenciales en capturas.
5. **Clientes:** listado y detalle; editar información, asignar membresía y entrenador. Comprobar que la asignación se refleja tras guardar.
6. **Planes:** listado, detalle y modificación; verificar precio, duración y estados sin cambiar los datos productivos.
7. **Clases:** catálogo y registro. **Horarios:** crear una programación de prueba, revisar y editar la clase.
8. **Pagos:** registrar un pago de prueba, abrir su detalle y consultar historial del cliente. Usar una base de desarrollo.
9. Comprobar mensajes de error y validación en cada formulario. Revisar estados vacíos y tablas con varias filas.
10. Probar tamaños de **1440 px, 1024 px y 390 px**, navegación con teclado y zoom del navegador al **200 %**. En móvil las tablas deben permitir desplazamiento horizontal sin ocultar acciones.
11. Iniciar sesión como **Entrenador** y **Miembro**: sus páginas deben conservar el diseño previo y los flujos de fase 4.
12. Ejecutar `dotnet test .\SuwoGym.slnx -c Release` y verificar el estado de GitHub Actions.

## Criterios de aceptación

- Las 22 vistas del Administrador compilan y muestran la capa visual prevista.
- Se conservan las acciones de alta, edición, asignación, pago y programación.
- Ninguna vista de otro rol hereda estilos del Administrador.
- No se difunden secretos, contraseñas ni datos personales en capturas.
- La rama se integra en `main` **solo tras** comprobar CI y el checklist visual/funcional en entorno local.

## Siguientes fases

- Fase 6: endurecimiento de seguridad, tokens y recuperación de acceso, auditoría y pruebas de integración.
- Fase 7: capturas anonimizadas, guía reproducible y presentación profesional GitHub/LinkedIn.
