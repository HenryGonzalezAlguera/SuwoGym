# SUWÓ GYM — Seguridad y configuración local

## Aviso de seguridad importante

Se identificaron una **contraseña de aplicación de correo** y una **clave de firma JWT** publicadas históricamente en `SuwoGym_API/appsettings.json`. Se quitaron de la versión actual. **Eso no borra los valores de commits anteriores**; no se deben reutilizar.

**Acciones necesarias del propietario:**

1. Revoca la contraseña de aplicación anterior en tu cuenta Google (Seguridad → Contraseñas de aplicaciones) y, si vas a enviar correos, crea una diferente. No publiques la nueva contraseña ni la compartas en chats.
2. Genera una **nueva** clave JWT aleatoria de al menos 32 bytes para esta instalación. El cambio hace que los tokens firmados con la clave anterior dejen de ser válidos una vez que se reinicie la API con la nueva.
3. Antes de usar este repositorio con terceros, revisa el historial Git y otros archivos por si se publicaron más claves. Para retirar valores del historial, será necesario un procedimiento separado de **reescritura de Git** con impacto en todas las copias; no lo hagas sin respaldos y coordinación. **La revocación es prioritaria**, porque la reescritura no garantiza que desaparezcan copias ya descargadas.

## Configurar .NET User Secrets en Visual Studio / PowerShell

El proyecto API ya incluye un `UserSecretsId`. Ejecuta en la carpeta raíz:

```powershell
# Generar un secreto nuevo criptográficamente aleatorio; no reutilizar el anterior.
$nuevaClave = [Convert]::ToBase64String(
    [System.Security.Cryptography.RandomNumberGenerator]::GetBytes(48)
)

dotnet user-secrets set "Jwt:SecretKey" $nuevaClave --project .\SuwoGym_API\SuwoGym_API.csproj
Remove-Variable nuevaClave
```

`dotnet user-secrets` guarda los secretos **fuera del repositorio**, asociados al perfil de usuario. Su almacenamiento local **no está cifrado** y está destinado a desarrollo, no a producción.

Si se usa correo, configura `Correos:Correo` y `Correos:AppPassword` mediante User Secrets o variables de entorno. No los agregues a `appsettings.json` ni a archivos rastreados por Git. Si prefieres no configurar correo por ahora, la API puede iniciarse con JWT, pero **las acciones que envían correo no estarán operativas correctamente**.

La cadena SQL Server se configura en `ConnectionStringsApi:DefaultConnection`. Para entornos locales, puede utilizarse autenticación integrada según tu instalación. Si tu servidor requiere contraseña, guárdala fuera del repositorio. El archivo base solo muestra una cadena sin credenciales explícitas.

## Configurar variables de entorno (alternativa)

ASP.NET Core reconoce dobles guiones bajos para secciones de configuración:

- `Jwt__SecretKey`
- `Correos__Correo`
- `Correos__AppPassword`
- `ConnectionStringsApi__DefaultConnection`

Las variables deben estar disponibles en el **proceso que ejecuta la API**, no solo en otra terminal. En Visual Studio, User Secrets suele ser más sencillo para `Development`.

## Restricciones y siguientes pasos

- La API valida el JWT firmado pero **no** verifica `issuer` y `audience` en la configuración actual; requiere endurecimiento antes de despliegue.
- La web utiliza `SessionAuthorize` para verificar una sesión completa y restringir los controladores por rol (`1`, `2`, `3`); la API mantiene autorización JWT independiente. [Ver fase 2](FASE2_SESIONES_Y_ROLES.md).
- Se incorporaron pruebas automatizadas del filtro MVC; aún faltan pruebas E2E, CSRF y revocación de tokens.
- La recuperación de acceso genera contraseñas temporales; su seguridad y el manejo de errores de correo deben revisarse antes de permitir uso real.
- No publicar el sitio en Internet ni utilizar cuentas reales hasta validar autenticación, roles, integridad y manejo de secretos.

## Comprobar

```powershell
dotnet restore .\SuwoGym.slnx
dotnet build .\SuwoGym.slnx -c Release --no-restore
```

GitHub Actions solo verifica compilación y que el JSON público no contenga credenciales conocidas en los campos destinados a secretos. **No realiza una auditoría completa de seguridad**.
