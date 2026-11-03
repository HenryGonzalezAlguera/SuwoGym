# Fase 3D — Recuperar acceso de Entrenador (solo desarrollo)

## Problema detectado

El alta de empleados existente `POST /api/Admin/EmpleadoAgregar` genera
una contraseña temporal, almacena únicamente un hash BCrypt y envía un email
a la cuenta creada. Si `Correos:AppPassword` no está configurado,
`UtilesService.EnviarCorreoAsync` omite el envío: el entrenador queda creado
sin que el Administrador conozca la contraseña. La contraseña **no es
recuperable** a partir del hash en `dbo.Usuario`.

El objetivo es continuar la prueba funcional con el Entrenador ya creado,
**sin eliminar al empleado, sus horarios ni las reservas de los miembros**.

## Recuperación administrativa — entorno local Development exclusivamente

En Visual Studio:

1. Detener depuración (**Mayús + F5**), sincronizar `main` mediante
   **Git → Extraer** y **Compilar → Recompilar solución**.
2. Iniciar `SuwoGym_API` y `SuwoGym_Web` con perfil **https** en Visual
   Studio (**F5**). Ambos deben ejecutarse en entorno `Development`.
3. Ingresar como **Administrador**, abrir **Empleados** y editar la cuenta
   del entrenador de desarrollo.
4. En la tarjeta «Datos laborales», localizar «Recuperar acceso del
   entrenador» y pulsar **Generar contraseña temporal**.
5. Copiar la nueva clave en privado. **No tomar capturas que la muestren,
   no compartirla en chats, no subirla a GitHub ni guardarla en SQL.**
6. Seleccionar **Cerrar sesión para probar entrenador**.
7. Iniciar sesión con el correo registrado del Entrenador y la clave temporal
   recién emitida. El sistema debe redirigir automáticamente a
   `/Usuario/Configuracion` porque `DebeCambiarContrasenna = 1`.
8. Escribir una contraseña privada nueva, confirmarla y guardarla.
   La aplicación cerrará sesión; vuelve a ingresar con la nueva contraseña.
9. Abrir **Mi cronograma**, **Asistencias** y **Participantes**. La
   clase reservada por el Miembro debe figurar con estado **Pendiente**.
   No intentes marcar asistencia antes de la hora real de inicio.

## Controles incluidos

- WEB usa `[SessionAuthorize(1)]`; API exige JWT con
  `[Authorize(Roles="1")]`.
- La opción solo aparece y el endpoint solo responde en ambiente
  **Development**. No está disponible en producción.
- Solo se permiten cuentas de entrenador **activas, IdRol=2**.
  No restablece Administradores, Miembros ni empleados inactivos.
- El botón envía POST con token antifalsificación.
- La clave es aleatoria (24 caracteres, `RandomNumberGenerator`), su
  hash se genera con BCrypt en la API.
- Se reutiliza `dbo.spActualizarContrasenna` **que ya existe** en la
  base: no hace falta ejecutar ningún SQL nuevo.
- El procedimiento marca `DebeCambiarContrasenna = 1`.
- La contraseña temporal se muestra únicamente en la respuesta inmediata
  de la operación autenticada, con encabezados para evitar caché. No se
  coloca en `TempData`, query strings, cookies o logs del código.
- La pantalla de edición de empleados sigue sin mostrar hashes ni
  contraseñas existentes.

## Límites y próximos pendientes de seguridad

Este mecanismo está pensado para desbloquear **pruebas locales de
desarrollo**. Antes de ofrecer recuperación en producción habrá que
implementar un flujo de invitación/restablecimiento basado en enlaces
de un solo uso con expiración, confirmación de identidad, auditoría,
protección ante abuso, rotación de sesiones activas y correo confiable.

El flujo general de recuperación por correo existente también requiere
revisión: actualmente actualiza la contraseña antes de confirmar la
entrega SMTP. **No usar «Olvidó su contraseña» en el entorno local sin
verificar antes SMTP y el acceso a la bandeja de destino.**

Los tests xUnit comprueban el generador de claves y la presencia de
protección por rol/antifalsificación. Las pruebas no contactan SMTP
ni un SQL Server real; el inicio de sesión final se verifica manualmente.
