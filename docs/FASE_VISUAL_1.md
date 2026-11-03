# Fase Visual 1 — Sistema de diseño SUWÓ GYM

## Alcance

Esta fase introduce una **base visual compartida** para las vistas Razor sin
reescribir el tema original ni cambiar endpoints, controladores, procedimientos
almacenados o autenticación.

La aplicación sigue utilizando el mismo CSS original:
`wwwroot/css/main.css` (bundle del tema),
`wwwroot/css/colores.css` (colores del equipo) y
`wwwroot/js/main.js` (Bootstrap, menú lateral, componentes).

Los nuevos archivos, cargados después de los estilos originales, son:

- `wwwroot/css/suwo-polish.css`: variables de marca y estilos compatibles.
- `wwwroot/js/suwo-polish.js`: selección activa del menú mediante rutas MVC.
  Los eventos de colapsar/abrir el sidebar siguen en el bundle original.

## Pantallas y componentes cubiertos

- Layout interno (`_Layout.cshtml`): topbar con identidad de usuario,
  sidebar con secciones y navegación consistente, footer actualizado y
  mayor espacio para contenidos.
- Layout externo (`_LayoutExterno.cshtml`): tarjeta de acceso y registro con
  fondo suave, formulario y tarjeta de planes con foco visible.
- Layout vacío (`_LayoutVacio.cshtml`): estilos base sin duplicar el bundle JS.
- Componentes comunes de todos los roles: tarjetas, botones, tablas,
  formularios, alertas, badges, títulos, etiquetas y campos informativos.
- `Home/Principal`: bienvenida diferenciada, tarjetas de información,
  accesos rápidos por rol. No cambia enlaces ni condiciones de rol.
- `Miembro/Historial_Asistencias`: tarjetas de indicadores y mensaje de
  estado vacío con mayor jerarquía.

La fase **no afirma que se haya revisado visualmente cada pantalla**: las
vistas no modificadas heredan estilos base. Los módulos extensos de
Administrador y Entrenador se pulirán individualmente en próximas fases,
una vez completada la prueba manual de esta base.

## Guía rápida para probar en Visual Studio (en español)

1. Detén API y WEB con **Depurar → Detener depuración**.
2. Desde la solución local `SuwoGym.slnx`, tras sincronizar los cambios de
   `main`, selecciona **Compilar → Recompilar solución**.
3. En **Configurar proyectos de inicio...**, establece
   `SuwoGym_API` y `SuwoGym_Web` como **Iniciar** con perfil HTTPS.
4. Ejecuta **F5** y abre `https://localhost:7035/Home/Principal`.
5. En el navegador, usa **Ctrl + F5** para invalidar la caché de CSS.
6. Comprueba las siguientes rutas con cuentas de desarrollo de cada rol:

| Área | Qué revisar |
| --- | --- |
| `Home/Principal` | Bienvenida, columnas, acceso a cada módulo |
| `Miembro/Historial_Asistencias` | Indicadores y estado sin registros |
| `Miembro/Perfil` | Campos y tarjetas, sin desbordamientos |
| `Miembro/Pagos_Lista` | Tablas, botones y mensajes |
| `Admin/Dashboard` | KPIs, gráficas y tablas |
| `Entrenador/Clientes_Lista` | Listado y acciones |
| `Home/Index`, `Home/Registro` | Formulario, confirmación y radio de membresías |

### Validación transversal

- Escritorio amplio (1440 px), portátil (1024 px) y móvil (390 px).
- Abrir/cerrar menú lateral y overlay móvil.
- Enlaces activos según la ruta de cada controlador.
- Botones con hover y foco visible usando teclado.
- Tablas anchas con desplazamiento horizontal cuando corresponda.
- Formularios y validaciones intactos, incluida protección antifalsificación.
- No se debe publicar información sensible en capturas.

## Limitaciones y continuidad

- GitHub Actions valida compilación y las pruebas automatizadas existentes;
  **no sustituye las pruebas visuales ni E2E** con un navegador y SQL Server.
- El ajuste es intencionadamente no destructivo; no se modifica el bundle del
  tema de más de 800 KB ni se reemplazan los layouts o las vistas del equipo.
- No se crean imágenes, nuevos assets externos o dependencias.
- La continuidad recomendada es **Fase Visual 2: vistas de Miembro**,
  seguida de Administrador, Entrenador y la portada pública.
