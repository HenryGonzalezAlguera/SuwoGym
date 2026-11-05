# Fase 4 — Sistema visual del Entrenador

## Objetivo

Unificar el diseño del módulo Entrenador de SUWÓ GYM **sin modificar la
autenticación, controladores, API, stored procedures ni reglas de negocio**.

El sistema visual se implementa en
`SuwoGym_Web/wwwroot/css/suwo-trainer.css`. Su alcance queda restringido
a `body.suwo-app.suwo-trainer`, clase que el layout interno asigna
únicamente cuando `Context.Session.GetString("Rol") == "Entrenador"`.
Las vistas de Administrador y Miembro conservan sus estilos previos.

Se respetan `main.css`, `colores.css`, `suwo-polish.css`, `suwo-member.css`
y el JavaScript del menú.

## Alcance de las 16 vistas existentes

| Área | Vistas | Presentación |
| --- | --- | --- |
| Clientes | `Clientes_Lista`, `Clientes_Ver` | Encabezados, fichas de datos y tablas legibles |
| Ejercicios | `Ejercicios_Lista`, `Ejercicios_Agregar` | Catálogo, formularios y estados vacíos |
| Cronograma | `Horarios_Lista`, `Horarios_Ver` | Estados, horarios y fichas |
| Asistencia | `Asistencias_Lista`, `Asistencias_Participantes` | Indicadores, tabla desplazable y columna Guardar visible al desplazarse |
| Rutinas | `Rutinas_Lista`, `Rutinas_Ver`, `Rutinas_Agregar`, `Rutinas_Editar` | Listados, detalle y formularios |
| Ejercicios de rutina | `Rutinas_Ejercicios`, `Rutinas_EjercicioVer`, `Rutinas_EjercicioAgregar`, `Rutinas_EjercicioEditar` | Detalles, configuraciones y formularios |

En cada vista se agregaron el título de pestaña `ViewData["Title"]`,
un encabezado contextual y una clase de presentación en su contenedor
principal, sin alterar acciones ni la cantidad de formularios.

## Validaciones disponibles

- Compilación de `SuwoGym_API` y `SuwoGym_Web` en .NET 10.
- Suite automatizada `SuwoGym.Tests` para sesiones, registro, catálogo de
  clases y recuperación de acceso.
- Revisión estática de conservación de los atributos
  `asp-action`, `asp-controller`, `asp-route-*` y del número de
  formularios por archivo.

**GitHub Actions no es una prueba visual ni prueba SQL Server real.**
Los controles de asistencia, creación de ejercicios y rutinas deben probarse
en la aplicación local contra la base de datos de desarrollo.

## Revisión manual desde Visual Studio

1. Detén la depuración (**Mayús + F5**) y en `main` usa **Git → Extraer**.
2. **Compilar → Recompilar solución**.
3. Inicia la API y la WEB con los perfiles HTTPS desde **F5**.
4. Ingresa con el usuario Entrenador de desarrollo.
5. Abre las vistas de **Clientes**, **Ejercicios**, **Rutinas**,
   **Mi cronograma** y **Asistencias**.
6. Con datos y sin datos, verifica tablas, badges, botones y formularios.
   El menú debería seguir expandiéndose y mostrar la sección activa.
7. Comprueba la página **Asistencias → Participantes**: en escritorio
   la columna «Guardar» permanece visible mientras se desplaza la tabla
   horizontalmente. La acción guarda únicamente cuando la clase ya inició.
8. Haz pruebas de tamaño a 1440 px, 1024 px y 390 px. En móvil, las tablas
   deben poder desplazarse sin recortar campos ni botones.
9. Prueba foco con teclado y tamaño de zoom del navegador 100 %.
10. Verifica que las vistas de **Miembro** y **Administrador** no hayan
    cambiado visualmente al ser estilos limitados al rol Entrenador.

## Pruebas funcionales registradas en conversación

El usuario validó en su entorno local, mediante capturas y pruebas manuales:
alta de clase y horario desde Administrador, reserva desde Miembro,
consulta de reservas por Entrenador, registro de asistencia y acceso al
catálogo de ejercicios. El usuario también indicó que probó las
funciones de clientes, rutinas y pagos.

Estas verificaciones son evidencia manual aportada por el usuario, **no**
equivalen a pruebas automatizadas de extremo a extremo ni acreditan
seguridad, rendimiento o despliegue productivo.

## Próximos trabajos

- Fase 5: pulido visual de todas las vistas de Administrador.
- Fase 6: seguridad (SMTP, tokens y recuperación de acceso),
  antifalsificación, auditoría y pruebas de integración.
- Fase 7: guía reproducible, capturas anonimizadas y material de
  presentación profesional para GitHub/LinkedIn.
