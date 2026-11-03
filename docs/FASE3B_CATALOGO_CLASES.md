# Fase 3B — Crear clases desde Administrador antes de programar horarios

## Problema original

`/Admin/Horarios_Agregar` contiene un desplegable de **Clase** que se carga
desde `/api/Admin/ClasesListaAPI`, a su vez respaldado por
`dbo.spListarClases` y `dbo.Clase`. Al estar vacío el catálogo, no era
posible crear horarios. SUWÓ GYM tenía programación de horarios, pero no una
pantalla para registrar nuevas actividades.

## Nueva funcionalidad

La navegación de Administrador contiene ahora **Clases**.

- `/Admin/Clases_Lista`: muestra actividades y permite ir a crear o programar.
- `/Admin/Clases_Agregar`: formulario con nombre obligatorio (2–50 caracteres)
  y descripción opcional (máximo 200). POST protegido contra CSRF.
- `POST /api/Admin/ClaseCrearAPI`: requiere JWT rol Administrador y ejecuta
  `dbo.spRegistrarClase` con Dapper. No devuelve errores SQL privados.
- `/Admin/Horarios_Agregar`: ofrece enlace a crear actividad si no existe,
  sin cambiar el formulario actual de programación.
- Desde el catálogo se puede pulsar **Programar** y la actividad se
  preselecciona en el formulario.

El catálogo no asigna entrenadores ni fechas: eso sigue ocurriendo en la
programación de horarios. No se alteran reservas, pagos, membresías o usuarios.

## Instalar el procedimiento SQL (necesario una vez)

**La API no crea procedimientos automáticamente.** Para habilitar el nuevo
registro, aplica exclusivamente este script:

`Database/06_Registrar_Clases_Catalogo.sql`

1. Detén la WEB y la API desde Visual Studio (**Mayús + F5**).
2. En Visual Studio, **Git → Extraer**, rama `main`.
3. En SSMS abre el archivo `06_Registrar_Clases_Catalogo.sql` desde
   `C:\Users\Henry\source\repos\SuwoGym\Database`.
4. Confirma la conexión de la **misma instancia SuwoGym** que utiliza la API
   en Secretos de usuario. En el caso local se utiliza la instancia nombrada
   `DESKTOP-ELCGC2E\MSSQLSERVER01`.
5. Revisa el script y ejecútalo con **F5**. Verás
   `ProcedimientoRegistrado` (identificador no nulo), `BaseDatos=SuwoGym`
   y el nombre del servidor.
6. El script es repetible (`CREATE OR ALTER`) y **no inserta clases, no
   elimina tablas y no modifica los datos existentes**. Usa una transacción
   con bloqueo para asignar `IdClase`, porque el esquema original define
   esa columna `INT NOT NULL`, **sin IDENTITY**.

**No ejecutes** los scripts destructivos del directorio
`SuwoGym_API/Scripts` ni vuelvas a correr `01` o `02` sobre los datos
que ya tienes.

## Probar desde Visual Studio

1. **Compilar → Recompilar solución**.
2. Ejecuta API y WEB con **F5**, perfiles HTTPS.
3. Inicia sesión como Administrador.
4. Abre **Clases → Nueva clase** y registra una actividad de prueba,
   por ejemplo «Funcional» y su descripción. No se crea un horario aún.
5. La actividad debe aparecer en el catálogo. Vuelve a intentar guardarla:
   la API debe rechazar un nombre duplicado.
6. En el catálogo, pulsa **Programar** junto a esa actividad.
7. Comprueba que el campo Clase queda seleccionado. Elige el Entrenador
   existente, una fecha futura, hora de finalización posterior y cupos.
8. Guarda y verifica que aparezca en **Horarios**.
9. Cierra sesión e inicia con un usuario Miembro para comprobar que
   aparezca en **Clases disponibles** y probar el flujo de reserva.

### Verificación SQL opcional (solo lectura)

```sql
USE SuwoGym;
SELECT IdClase, NombreClase, Descripcion
FROM dbo.Clase
ORDER BY NombreClase;

SELECT CP.IdClaseProg, C.NombreClase, CP.IdEntrenador,
       CP.FechaHoraInicio, CP.FechaHoraFin, CP.CupoMaximo, CP.Estado
FROM dbo.ClaseProgramada AS CP
JOIN dbo.Clase AS C ON C.IdClase = CP.IdClase
ORDER BY CP.FechaHoraInicio DESC;
```

## Seguridad y límites

- Solo Administrador puede crear y listar estas clases desde sus rutas MVC
  y API; se conserva el control por roles existente.
- El usuario no puede especificar `IdClase`; lo asigna SQL Server.
- Los datos se validan en WEB y API, y SQL impide duplicados con mismo nombre.
- No se añadieron funciones de editar/eliminar tipos de clase porque requieren
  analizar su impacto sobre horarios y reservas existentes; son una fase futura.
- Las pruebas automatizadas verifican DTO, CSRF y compilación. **No**
  ejecutan SQL Server ni demuestran que una reserva real haya funcionado;
  esa prueba debe realizarse desde Visual Studio con datos ficticios.
