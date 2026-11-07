# Fase 7B — Pruebas de integración con SQL Server real

## Propósito y protección de los datos

Esta etapa comprueba los procedimientos almacenados **reales** del proyecto
SUWÓ GYM sobre una base **separada, vacía y exclusivamente ficticia**.

**La base original `SuwoGym` no debe usarse.** El ejecutor impide el uso
de cualquier base con otro nombre, exige una marca de aislamiento y
requiere una confirmación explícita mediante variable de entorno.
GitHub Actions solo lo **compila**: no se conecta a SQL Server y no ejecuta
ninguna prueba SQL real.

Se trabaja con `tools/SuwoGym.SqlIntegration`, un ejecutable .NET 10
independiente de la solución principal. Usa Dapper, SqlClient y BCrypt.

## Matriz de pruebas disponibles

| Escenario real | Procedimiento o tabla | Verificación |
|---|---|---|
| Dos miembros intentan reservar un único cupo simultáneamente | `spReservarClaseMiembro` | Exactamente una reserva, `CupoActual = 1` |
| Cancelación antes del inicio | `spCancelarReservaClaseMiembro` | Reserva inactiva y `CupoActual = 0` |
| Registro de pago de prueba | `spRegistrarPagoMembresia` | ₡10.000 + 13 % IVA = ₡11.300 |
| Pago duplicado | `spRegistrarPagoMembresia` | El segundo intento se rechaza |
| Restablecimiento de contraseña | `spRecuperacionSolicitar`, `spRecuperacionValidar`, `spRecuperacionConsumir` | Hash actualizado, `VersionJwt` rotada y segundo uso rechazado |
| Token expirado | `RecuperacionContrasennaToken` y SP de validación/consumo | No acepta token ni modifica el hash |
| Desactivación y reactivación | Trigger `trg_Usuario_RevocarJwt` | VersionJwt diferente en cada transición |

El ejecutor crea un entrenador, dos miembros, un plan, una clase,
dos membresías, una programación y los registros necesarios, todos
**ficticios y temporales**. No envía correos ni abre la interfaz WEB.
Al finalizar intenta borrar exclusivamente las filas que él creó.
Si se cancela abruptamente, puede quedar basura de pruebas en la base
aislada: el ejecutor muestra el error y los correos terminan en
`@example.invalid` con prefijo `t7b-`.

## Paso 1 — Crear la base de desarrollo AISLADA (SSMS)

1. Confirmar que la base de origen `SuwoGym` está respaldada.
2. En SSMS, crear una **nueva base vacía** llamada exactamente
   `SuwoGym_Test_7B` usando **Bases de datos → Nueva base de datos**.
3. Sobre `SuwoGym`, seleccionar **Tareas → Generar scripts** y elegir
   únicamente el **esquema completo**: tablas, claves, defaults,
   procedimientos almacenados, triggers e índices. En las opciones
   avanzadas elegir **Tipos de datos para script: Solo esquema**.
   **NO copiar datos ni cuentas de usuarios**.
4. Guardar el script generado en un archivo temporal fuera del
   repositorio. **Antes de ejecutarlo**, comprobar las sentencias de
   `CREATE DATABASE`, `USE`, `ALTER DATABASE`, `DROP`,
   rutas físicas y nombres de base: NO debe contener una operación
   que cambie o borre `SuwoGym`. Retirar encabezados de
   `CREATE DATABASE` si se genera el origen; fijar explícitamente
   `USE [SuwoGym_Test_7B]`.
5. Ejecutar el script de **solo esquema** desde una ventana de
   consulta conectada a `SuwoGym_Test_7B`.
6. La base debe contener los SP y el trigger de las fases 6B-2 y
   6C (equivalentes a scripts 07 y 08). **NO ejecutar los scripts
   07 y 08 originales sin adaptarlos**: contienen `USE [SuwoGym]`
   y actuarían sobre la base original. El esquema generado desde
   la base ya migrada debe incluir estos objetos.
7. **NO ejecutar `02_SuwoGym_DatosIniciales.sql`** en la base de
   pruebas: además de catálogos, incluye usuarios iniciales.
   El script de marcado crea únicamente los roles necesarios.

**Advertencia crítica:** si aparece algún usuario real al consultar
`dbo.Usuario` en la base de test, detenerse y crear de nuevo una
base de **solo esquema**, sin continuar con el marcado.

## Paso 2 — Marcar la base de pruebas

En SSMS, abrir y ejecutar **únicamente** sobre la base de pruebas
`Database/09_7B_Marcar_Base_Ficticia.sql`.

Confirma el nombre de base y la ausencia de usuarios ajenos a las
pruebas, agrega roles 1, 2, 3 si faltan y crea la propiedad de base:

- Nombre: `SuwoGym7BTestOnly`
- Valor: `YES`

El resultado debe ser:

| BaseDePruebas | MarcaAislamiento |
|---|---|
| SuwoGym_Test_7B | YES |

**No ejecutar `09` contra `SuwoGym`.** El script aborta si
la base no tiene el nombre esperado.

## Paso 3 — Ejecutar el programa de pruebas en Windows

Desde Visual Studio, abrir **Ver → Terminal** o **Herramientas →
Línea de comandos → PowerShell para desarrolladores** en la carpeta
del repositorio, donde está `SuwoGym.slnx`.

En PowerShell, configurar una conexión **local de desarrollo** con
autenticación de Windows (ajustar tu instancia SIN publicarla):

```powershell
$env:SUWOGYM_7B_CONFIRM = "YES_TEST_DATABASE_ONLY"
$env:SUWOGYM_7B_CONNECTION = "Server=MI_SERVIDOR_LOCAL;Database=SuwoGym_Test_7B;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"

dotnet run --project .\tools\SuwoGym.SqlIntegration\SuwoGym.SqlIntegration.csproj --configuration Release

Remove-Item Env:\SUWOGYM_7B_CONNECTION
Remove-Item Env:\SUWOGYM_7B_CONFIRM
```

**No compartir capturas de cadenas de conexión que incluyan claves**
ni introducir contraseñas de SQL en el proyecto.

El ejecutor también exige:

- `Database=SuwoGym_Test_7B` en la cadena de conexión.
- `DB_NAME() = SuwoGym_Test_7B` comprobado por SQL Server.
- La propiedad `SuwoGym7BTestOnly = YES` en esa base.
- Esquema completo con SP y migraciones 07/08, y roles 2 y 3.
- La variable exacta `SUWOGYM_7B_CONFIRM`.

Si falta cualquier requisito, **se detiene antes de insertar usuarios
o crear reservas y pagos**.

## Paso 4 — Revisar los resultados

Se esperan seis grupos con `[OK]`, una línea final de limpieza y:

```text
Resultado: 6 aprobadas, 0 fallidas.
```

Si alguna prueba falla, debe conservarse el error en el registro
**sin pegar datos sensibles en chats públicos**. El runner evita
imprimir claves, correos y tokens.

Los test de concurrencia pueden descubrir defectos reales; en caso
de fallo no se modificará ningún SP automáticamente. Se analizará
el procedimiento exacto antes de proponer una corrección.

## Validación del proyecto (independiente)

La suite anterior de **85 pruebas xUnit** continúa ejecutándose
normalmente desde `Prueba → Explorador de pruebas`. La fase 7B **no
incrementa ese contador**: es un ejecutor de integración externa,
aislado y de activación manual.

GitHub Actions compila ambos proyectos, ejecuta las 85 pruebas unitarias
y HTTP, y **compila** el ejecutor SQL, pero NO ejecuta sus pruebas porque
no dispone de una base ficticia SQL Server.

## Límites pendientes antes del despliegue

- La suite SQL trabaja directamente con SP, no prueba interfaz
  completa WEB→API→SQL ni la entrega real SMTP.
- Deben verificarse las operaciones financieras con más casos
  extremos, cancelaciones concurrentes y concurrencia entre dos
  solicitudes sobre el mismo usuario.
- Revisar además que los scripts de configuración, HTTPS y secretos
  funcionan en un entorno de preproducción.
- No conectar jamás las pruebas 7B a cuentas productivas.
