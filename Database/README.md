# Base de datos SUWÓ Gym

## Archivos públicos para GitHub

1. `01_SuwoGym_Estructura.sql`
   - Crea la base `SuwoGym`.
   - Crea tablas, restricciones, índices y procedimientos almacenados.

2. `02_SuwoGym_DatosIniciales.sql`
   - Inserta roles, clases y planes de membresía.

3. `03_SuwoGym_AdministradorInicial_Plantilla.sql`
   - Plantilla para crear el primer administrador.
   - Debe configurarse con un correo, identificación y hash BCrypt propios.
   - Nunca almacene contraseñas en texto plano.

## Orden de instalación

1. Ejecutar `01_SuwoGym_Estructura.sql`.
2. Ejecutar `02_SuwoGym_DatosIniciales.sql`.
3. Configurar y ejecutar `03_SuwoGym_AdministradorInicial_Plantilla.sql`.
4. Configurar la cadena de conexión de `SuwoGym_API`.
5. Iniciar `SuwoGym_API` y `SuwoGym_Web`.

## Archivos que no deben subirse

- Copias `.bak`.
- Archivos `.mdf` y `.ldf`.
- `03_SuwoGym_AdministradorInicial_LOCAL.sql`.
- Archivos con claves JWT, credenciales de correo o contraseñas.
- Datos personales o transacciones de prueba.

## Exclusiones recomendadas

```gitignore
*.bak
*.mdf
*.ldf
03_SuwoGym_AdministradorInicial_LOCAL.sql
```
