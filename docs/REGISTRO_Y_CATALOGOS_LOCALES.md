# SUWÓ GYM — Recuperación del registro local

## Diagnóstico de noviembre de 2026

El formulario `Home/Registro` no mostraba `confirmarContrasenna`,
aunque `UsuarioModel` declaraba ese campo requerido. También utilizaba dos
entradas con el mismo nombre de `PlanMembresia` (radio y texto oculto).
El proceso original transformaba la contraseña en BCrypt antes de comprobar
validez y descartaba el error útil al redirigir.

**La API actual sí envía `@PlanMembresia` a
`dbo.spRegistrarUsuario`**, y el procedimiento de `Database/01_SuwoGym_Estructura.sql`
sí requiere ese parámetro. El mensaje «esperaba @PlanMembresia» indica que
el proceso API realmente ejecutado podría tener un binario anterior; recuerda
que anteriormente el puerto `7129` estaba ocupado por otro proceso. No se
debe cambiar el SP sin comprobar la versión real que está funcionando.

## Cambios de código

- ViewModel específico `RegistroUsuarioViewModel`, con confirmación y plan obligatorio.
- Formulario con confirmación y un único grupo de radio `PlanMembresia`.
- El POST comprueba validaciones y catálogo antes de invocar la API. Solo envía
  campos de registro y hash BCrypt, nunca la confirmación.
- Se conserva el formulario y se muestra un mensaje general ante respuesta de
  error, sin publicar el texto interno de las excepciones SQL.
- El POST utiliza `[ValidateAntiForgeryToken]`, compatible con los formularios
  Razor generados con tag helpers.
- Pruebas unitarias de validación, envío de campos y rechazo de planes ausentes.

## Procedimiento recomendado en SSMS

1. Comprueba que la conexión de SSMS es la misma instancia que
   `ConnectionStringsApi:DefaultConnection` de la API.
2. Ejecuta **solo lectura** `Database/03_Diagnostico_Registro.sql`.
   Confirma nombre de la base, roles, planes y los parámetros de
   `dbo.spRegistrarUsuario`. Debe mostrar `@PlanMembresia`.
3. Si faltan roles o planes, revisa y ejecuta voluntariamente
   `Database/04_Catalogos_Registro_Sin_Usuarios.sql`. Este script solo
   **inserta roles 1–3 y planes 1–3 cuando faltan**; no modifica contraseñas,
   cuentas, SP ni datos existentes.
4. **No ejecutes `Database/02_SuwoGym_DatosIniciales.sql` para crear accesos:**
   contiene usuarios con hashes versionados públicamente, por lo que no
   constituyen credenciales seguras.
5. **No ejecutes `SuwoGym_API/Scripts/CreateFullDatabase.sql`:**
   contiene `DROP DATABASE SuwoGym` y destruiría la base existente.
   Los archivos SQL de `SuwoGym_API/Scripts/` incluyen procedimientos
   históricos incompatibles con el contrato de la API actual.

## Reiniciar procesos y comprobar la versión ejecutada

Detén API y WEB con Ctrl+C o desde Visual Studio antes de probar.
En PowerShell, verifica el propietario del puerto HTTPS de API:

```powershell
Get-NetTCPConnection -LocalPort 7129 -State Listen -ErrorAction SilentlyContinue |
    Select-Object LocalAddress, LocalPort, OwningProcess
```

Si sigue ocupado, identifica el proceso antes de cerrarlo; puede ser un
`SuwoGym_API` antiguo. Desde la raíz del repositorio actualizado:

```powershell
dotnet restore .\SuwoGym.slnx
dotnet build .\SuwoGym.slnx -c Release --no-restore
dotnet test .\SuwoGym.Tests\SuwoGym.Tests.csproj -c Release --no-restore
dotnet run --project .\SuwoGym_API\SuwoGym_API.csproj --launch-profile https
```

En **otra terminal**, ejecutar
`dotnet run --project .\SuwoGym_Web\SuwoGym_Web.csproj --launch-profile https`.
Abrir `https://localhost:7035/Home/Registro`. Deben aparecer los planes,
dos campos de contraseña y el botón de registro.

Una cuenta creada desde el registro recibe el **rol Miembro (3)** según
`dbo.spRegistrarUsuario`. La creación o elevación controlada de una cuenta
Administrador de demostración es un paso separado que debe realizar el dueño
de la base y requiere verificar primero que el miembro se haya registrado con éxito.
No subir cuentas ni contraseñas reales a GitHub.

## Verificación de datos después de registrar

```sql
USE SuwoGym;
SELECT IdUsuario, CorreoElectronico, IdRol, Estado
FROM dbo.Usuario ORDER BY IdUsuario DESC;

SELECT MU.IdUsuario, MU.IdPlan, MU.FechaVencimiento
FROM dbo.MembresiaUsuario AS MU
ORDER BY MU.IdMembresia DESC;
```

No compartas contraseñas, `ContrasennaHash`, tokens JWT ni datos personales.
