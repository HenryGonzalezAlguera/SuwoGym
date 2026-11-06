# Fase 6B-2 — Recuperación de contraseñas con enlaces seguros

## Diagnóstico del código y del esquema revisados

Se verificaron en el repositorio `Database/01_SuwoGym_Estructura.sql`,
`dbo.Usuario`, `dbo.spValidarCorreo` y `dbo.spActualizarContrasenna`.
El diseño anterior reemplazaba `ContrasennaHash` por un hash de contraseña
temporal **ANTES** de enviar el correo. Un fallo SMTP podía impedir el
acceso del usuario. Este cambio deja de usar ese flujo en el endpoint
público, **sin cambiar** `spActualizarContrasenna`, que sigue siendo utilizado
por otras partes del sistema.

## Qué se implementa

1. El usuario solicita recuperación en `/Home/RecuperarAcceso`.
2. `POST /api/Home/RecuperarAccesoAPI` responde de forma genérica, exista
   o no la cuenta. Se limita a 4 solicitudes/15 min por IP.
3. El servidor genera 32 bytes con `RandomNumberGenerator`, los codifica
   en Base64URL y guarda **únicamente SHA-256 de esos bytes** en la BD.
4. Se invalida cualquier token anterior para la misma cuenta y se envía
   un enlace absoluto HTTPS configurado en `Recuperacion:UrlBaseWeb`.
5. El enlace puede utilizarse **una vez**, durante **15 minutos**;
   `ValidarEnlaceRecuperacionAPI` no devuelve datos del usuario.
6. El formulario WEB pide contraseña y confirmación (12 a 72 caracteres,
   máximo 72 bytes UTF-8). El POST usa antifalsificación.
7. `spRecuperacionConsumir` consume el token y modifica `ContrasennaHash`
   y `DebeCambiarContrasenna` dentro de una **única transacción SQL**.
   Si no existe, expiró o ya fue utilizado, no cambia la cuenta.
8. Si SMTP falla, se intenta revocar el token emitido. El usuario
   conserva siempre su contraseña anterior hasta finalizar el paso 7.

## SQL incremental: ejecución manual obligatoria

Se preparó `Database/07_Recuperacion_Enlaces_Seguros.sql`.

- **No ejecutar** `01_SuwoGym_Estructura.sql` sobre tu BD actual para esta fase.
- Realiza una copia de seguridad de **SuwoGym** y abre SSMS conectándote
  a tu servidor de desarrollo.
- Revisa todo el script `07`, confirma que `USE [SuwoGym]` apunta a la
  base correcta y ejecútalo una sola vez. Es repetible si la estructura
  creada se mantiene compatible, gracias a `IF OBJECT_ID` y
  `CREATE OR ALTER PROCEDURE`.
- El script crea una **tabla adicional**:
  `dbo.RecuperacionContrasennaToken` (ID, usuario, hash, creación,
  vencimiento y uso). Agrega una FK a `dbo.Usuario`.
- Agrega cuatro SP: `spRecuperacionSolicitar`,
  `spRecuperacionValidar`, `spRecuperacionConsumir` y
  `spRecuperacionRevocar`. **No modifica tablas de usuarios ni contraseñas**.
- La consulta final devuelve cinco identificadores de objetos que deben
  ser distintos de `NULL`. La instalación debe comprobarse también
  con una cuenta ficticia mediante la aplicación.

Sin el script `07`, el nuevo endpoint responde con un error controlado.
**No asumir que GitHub Actions aplica cambios a SQL Server**: solo compila
código y ejecuta pruebas sin BD.

## Configuración local

- La API usa `Recuperacion:UrlBaseWeb = https://localhost:7035`
  en `SuwoGym_API/appsettings.json`. Es una URL pública, no una clave
  secreta. Para desplegar en otro dominio debe configurarse el origen
  HTTPS real; no debe construirse a partir de un encabezado Host enviado
  por el cliente.
- Configura `Correos:Correo` y `Correos:AppPassword` mediante **.NET
  User Secrets** de `SuwoGym_API`, como ya se explica en
  `docs/SEGURIDAD_Y_CONFIGURACION.md`. No los añadas a Git ni al chat.
- La API y WEB deben ejecutarse con los perfiles HTTPS existentes.
- No es necesario cambiar claves JWT, roles ni ningún dato de usuarios.

## Diagnóstico de mensaje "El servicio de recuperación no está disponible"

La pantalla WEB presenta este texto cuando la API devuelve cualquier respuesta
distinta de HTTP 202 o del límite HTTP 429. La creación de objetos en SQL
no confirma que la API esté apuntando a esa misma instancia ni que lea SMTP.

1. Confirmar que están ejecutándose API y WEB con HTTPS.
2. En Visual Studio, clic derecho en **SuwoGym_API → Administrar secretos del usuario**.
   Comprobar que existen `Correos:Correo` y `Correos:AppPassword`
   **sin publicar valores ni capturas de secretos**. `appsettings.json` contiene
   valores vacíos intencionalmente; deben prevalecer los User Secrets.
3. Confirmar `Recuperacion:UrlBaseWeb = https://localhost:7035` para
   desarrollo y que la WEB esté usando ese puerto.
4. Reiniciar API y WEB, enviar una solicitud de recuperación con una
   **cuenta de prueba** y observar la ventana **Salida → ASP.NET Core Web Server**
   o la consola de la API, no la WEB.
5. La API registra solo el tipo de causa, sin correo ni token:
   `faltan Correos`, `UrlBaseWeb`, `error SQL <número>`,
   o `error de infraestructura <tipo>`. Los fallos SMTP se reportan
   de modo genérico y revocan el token emitido.
6. Si aparece error SQL 2812 (procedimiento inexistente), comprobar
   la cadena de conexión utilizada por **SuwoGym_API**, y ejecutar la
   consulta de comprobación del script `07` en **esa misma base**.
   Nunca publicar la cadena de conexión completa.
7. Si falta SMTP, configurar exclusivamente en User Secrets una contraseña
   de aplicación válida; no usar la contraseña habitual de Gmail.
8. No repetir la migración ni cambiar hashes de usuarios para intentar
   arreglar un HTTP 503.

## Verificación manual con cuenta DE PRUEBA

1. Con BD respaldada y script `07` aplicado, ejecuta API y WEB.
2. Desde `Home/Index`, elige **¿Olvidó su contraseña?** y solicita un
   enlace para una cuenta ficticia con correo real de desarrollo.
3. Debe mostrarse el mensaje genérico de recepción, sin indicar si la
   cuenta existe. Solicita también para `nadie@example.invalid` y
   comprueba que la respuesta sea indistinguible.
4. Comprueba el correo: debe contener **enlace** y **ninguna contraseña**.
   No compartas capturas con el token.
5. Abre el enlace HTTPS antes de 15 minutos. Comprueba los dos campos y
   el mensaje de validación si no coinciden.
6. Crea una contraseña nueva y comprueba que puedes iniciar sesión;
   la anterior ya no debe funcionar.
7. Vuelve al mismo enlace: debe aparecer como utilizado o expirado.
8. Solicita un enlace y deja que pasen 15 minutos: no debe actualizar
   ninguna contraseña al intentar utilizarlo.
9. Solicita dos enlaces para la misma cuenta: solo el último debe
   funcionar. Las solicitudes de recuperación están limitadas por IP.
10. Detén SMTP o quita las credenciales **solo en una instancia de pruebas**:
    la solicitud debe fallar antes de tocar contraseñas.
11. Confirma que Administrador, Entrenador y Miembro siguen entrando y
    que el resto de módulos permanece operativo.
12. Compila la solución y ejecuta `dotnet test .\SuwoGym.slnx -c Release`.

Las pruebas automatizadas verifican generación/hash de token,
restricciones de los modelos, existencia de endpoints limitados y CSRF;
**no sustituyen una prueba de integración real de SQL Server y correo**.

## Riesgos que aún impiden publicar en producción

- Los JWT expedidos **antes** del restablecimiento podrían continuar
  siendo válidos durante su vida restante (hasta 30 minutos); el sistema
  todavía no implementa revocación por versión de sesión.
- La política de límites es por IP y en memoria; un despliegue detrás
  de proxy debe configurar encabezados reenviados y proxies confiables.
- Se requiere monitoreo de errores y política de reintentos/entregabilidad
  de correo sin filtrar direcciones ni tokens en logs.
- Revisar historial de Git y rotación de secretos históricos (la
  reescritura del repositorio no reemplaza revocarlos).
- El enlace aparece en la URL del navegador; se establece
  `Referrer-Policy: no-referrer` y caché `no-store` en la pantalla.
  Evitar analytics y scripts externos en la página de recuperación.

No fusionar esta rama en `main` hasta que la compilación, SQL de
desarrollo, correo y las pruebas de tres roles sean satisfactorias.
