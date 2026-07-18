# Sistema de Autenticación SuwoGym

## 📋 Descripción

Se ha implementado un sistema funcional de inicio de sesión y registro para la aplicación SuwoGym utilizando ASP.NET Core 10.0. El sistema está dividido en dos proyectos:

### **SuwoGym_API** (Backend)
- API REST que gestiona la autenticación
- Conexión a Base de Datos SQL Server
- Endpoints: `/api/auth/login` y `/api/auth/registro`
- Hash seguro de contraseñas (SHA256)
- Entity Framework Core para ORM

### **SuwoGym_Web** (Frontend)
- Aplicación Razor Pages / MVC
- Interfaz de usuario para login y registro
- Manejo de sesiones
- Cliente HTTP para comunicarse con la API

---

## 🛠️ Configuración

### **1. Ejecutar el Script SQL**

Primero, ejecuta el script SQL en tu servidor SQL Server para crear la base de datos:

```sql
USE master;
GO

IF DB_ID('SuwoGym') IS NULL
	CREATE DATABASE SuwoGym;
GO

-- El resto del script ya está incluido en el proyecto
```

### **2. Actualizar Strings de Conexión**

Los archivos `appsettings.json` ya han sido configurados:

**SuwoGym_API/appsettings.json:**
```json
{
  "ConnectionStringsApi": {
	"DefaultConnection": "Server=(localdb)\\TonyDB;Database=SuwoGym;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

**SuwoGym_Web/appsettings.json:**
```json
{
  "ConnectionStringsWeb": {
	"urlApi": "https://localhost:7096"
  }
}
```

### **3. Ejecutar la Aplicación**

#### **Opción 1: Visual Studio**
1. Establece **SuwoGym_API** como proyecto de inicio
2. Presiona **F5** para ejecutar
3. La base de datos se creará automáticamente mediante migraciones
4. Luego ejecuta **SuwoGym_Web** como segundo proyecto de inicio

#### **Opción 2: Línea de Comandos**

```powershell
# Restaurar dependencias
dotnet restore

# Ejecutar API (terminal 1)
cd SuwoGym_API
dotnet run

# Ejecutar Web (terminal 2)
cd SuwoGym_Web
dotnet run
```

---

## 📝 Características Implementadas

### **Autenticación**

✅ **Login**
- Validación de correo y contraseña
- Hash SHA256 para contraseñas
- Registro de último acceso
- Manejo de errores

✅ **Registro**
- Creación de nuevos usuarios
- Validación de contraseñas coincidentes
- Creación automática de perfil de cliente
- Rol por defecto: "Miembro"

✅ **Sesiones**
- Almacenamiento de datos de usuario en sesión
- Timeout de 30 minutos
- Cierre de sesión (Logout)

---

## 🗂️ Estructura de Base de Datos

### **Tabla: Rol**
- IdRol (PK)
- NombreRol (Administrador, Entrenador, Miembro)
- Descripcion
- Estado
- FechaRegistro

### **Tabla: Usuario**
- IdUsuario (PK)
- IdRol (FK)
- Nombre, Apellido
- CorreoElectronico (UNIQUE)
- ContrasennaHash
- DebeCambiarContrasenna
- Estado
- FechaRegistro, FechaUltimoAcceso

### **Tabla: Cliente**
- IdCliente (PK)
- IdUsuario (FK, UNIQUE)
- Identificacion
- Telefono
- FechaNacimiento
- Direccion
- FechaRegistro
- Estado

---

## 🔐 Roles y Permisos

| Rol | Permisos |
|-----|----------|
| **Administrador** | Control total del sistema, gestión de usuarios |
| **Entrenador** | Gestión de clientes y rutinas |
| **Miembro** | Acceso a rutinas personalizadas y progreso |

---

## 🌐 Endpoints API

### **POST /api/auth/login**
```json
Request:
{
  "correoElectronico": "usuario@ejemplo.com",
  "contrasenna": "password123"
}

Response (Success):
{
  "exitoso": true,
  "mensaje": "Inicio de sesión exitoso.",
  "usuario": {
	"idUsuario": 1,
	"nombre": "Juan",
	"apellido": "Pérez",
	"correoElectronico": "usuario@ejemplo.com",
	"nombreRol": "Miembro"
  }
}
```

### **POST /api/auth/registro**
```json
Request:
{
  "nombre": "Juan",
  "apellido": "Pérez",
  "correoElectronico": "juan@ejemplo.com",
  "contrasenna": "password123",
  "confirmarContrasenna": "password123",
  "identificacion": "123456789",
  "telefono": "+34612345678"
}

Response (Success):
{
  "exitoso": true,
  "mensaje": "Registro exitoso. Por favor, inicia sesión.",
  "usuario": { ... }
}
```

---

## 📱 Vistas Implementadas

### **Iniciar Sesión** (`/Home/Index`)
- Formulario con validación
- Enlace a registro
- Opción de recuperar acceso

### **Registrar Usuario** (`/Home/Registro`)
- Validación de contraseña confirmada
- Requisitos de seguridad mínimos
- Feedback en tiempo real

### **Panel Principal** (`/Home/Principal`)
- Dashboard personalizado según rol
- Opciones contextual para cada tipo de usuario
- Botón de cierre de sesión

---

## ⚠️ Validaciones Implementadas

✅ Correo electrónico único
✅ Contraseña mínimo 6 caracteres
✅ Contraseña hash con SHA256
✅ Validación de estado de usuario
✅ Campos requeridos no nulos
✅ Manejo de excepciones

---

## 🚀 Próximas Mejoras Sugeridas

- [ ] Implementar JWT (JSON Web Tokens) para APIs
- [ ] Recuperación de contraseña por email
- [ ] Two-factor authentication (2FA)
- [ ] Audit logging de acciones
- [ ] Rate limiting para intentos de login
- [ ] Integración con OAuth (Google, Microsoft)
- [ ] Dashboard de administrador
- [ ] Gestión de rutinas y entrenamientos

---

## 📚 Tecnologías Utilizadas

- **Framework:** ASP.NET Core 10.0
- **Base de Datos:** SQL Server
- **ORM:** Entity Framework Core 10.0.0
- **Autenticación:** Custom (Hash SHA256)
- **Sesiones:** ASP.NET Core Session Management
- **HTTP Client:** HttpClientFactory

---

## ✅ Estado del Proyecto

- [x] Vulnerabilidad de NuGet corregida (Microsoft.OpenApi)
- [x] Modelos de datos creados
- [x] DbContext configurado
- [x] API de autenticación implementada
- [x] Servicio de autenticación en Web
- [x] Vistas del formulario
- [x] Sistema de sesiones
- [x] Migraciones de base de datos
- [x] Validaciones implementadas
- [x] CORS configurado

---

## 📞 Soporte

Para problemas o consultas, revisa los logs en la consola de Visual Studio o verifica la cadena de conexión a la base de datos.

¡Listo para usar! 🎉
