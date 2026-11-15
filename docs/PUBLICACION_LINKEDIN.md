# SUWÓ GYM — Kit de publicación para LinkedIn

**Estado: demo pública de staging.** El código contiene módulos de miembro, entrenador y administrador, pero solo un subconjunto de flujos se verificó manualmente en Azure. No se debe afirmar que pagos o reservas funcionan de extremo a extremo en staging sin probarlos.

## Publicación recomendada (English)

🏋️ **Building beyond CRUD: SUWÓ GYM — a .NET 10 gym management platform.**

I'm sharing **SUWÓ GYM**, a full-stack application I built to practice modular backend design, user authentication, and cloud deployment.

🛠️ **Stack:** C# · .NET 10 · ASP.NET Core MVC · REST API · Dapper · SQL Server · Azure SQL · JWT · GitHub Actions.

What I built:
- Role-based areas for **members, trainers, and administrators**.
- Modules for memberships, gym classes, reservations, training, attendance and payments.
- **JWT-based authentication**, BCrypt hashing, CSRF protection and recovery-token flows.
- MVC front end communicating with an API; database access through **Dapper and stored procedures**.
- CI tests and controlled **Azure staging deployment with GitHub Actions / OIDC**.

🔎 **What has been validated in Azure:** app connectivity, health endpoints, member registration and login. Other management workflows still require end-to-end staging verification.

🌐 **Public staging demo:** https://suwogym-stg-web-hga-emfxdjcfccabdfh2.centralus-01.azurewebsites.net/

💻 **Code and architecture:** https://github.com/HenryGonzalezAlguera/SuwoGym

The demo uses a free hosting tier and may take time to resume after inactivity. Please use fictional test data only.

This project strengthened my skills in ASP.NET Core, authentication, relational data access, testing and cloud delivery.

#DotNet #ASPNetCore #CSharp #Azure #BackendDevelopment

## Versión opcional en español

🏋️ **Comparto SUWÓ GYM**, una plataforma de gestión de gimnasios desarrollada con **C#, .NET 10, ASP.NET Core MVC, API REST, Dapper y SQL Server**.

Implementé módulos por rol para miembros, entrenadores y administradores; autenticación con JWT y BCrypt; acceso a datos mediante procedimientos almacenados; pruebas automáticas y despliegue controlado en Azure con GitHub Actions.

**En staging ya validé** la conectividad, salud de los servicios, registro e inicio de sesión de miembros. Otros flujos siguen pendientes de pruebas completas.

🌐 Demo: https://suwogym-stg-web-hga-emfxdjcfccabdfh2.centralus-01.azurewebsites.net/

💻 GitHub: https://github.com/HenryGonzalezAlguera/SuwoGym

Demo gratuita: puede reactivarse lentamente. Solo datos ficticios.

#DotNet #CSharp #Azure #SQLServer #SoftwareDevelopment

## Material visual

**Material que SÍ está versionado:**
- [Logotipo oficial del proyecto](../SuwoGym_Web/wwwroot/images/logoSuwo_logotype.png)
- [Imagen de marca](../SuwoGym_Web/wwwroot/images/logoSuwo.png)
- [Diagrama del proyecto](../IEEE/diagrama_suwo_gym.png)

**Material pendiente (no inventar):** capturas reales de inicio, pantalla de miembro y, más adelante, pantallas de administrador y entrenador en Azure o en local con datos de prueba.

Guía para obtener capturas: [docs/screenshots/README.md](screenshots/README.md).

**Presentación en LinkedIn:** mientras no existan capturas reales, comparte el **logo** y el enlace de demo o una composición rotulada «portada del proyecto»; no presentes una maqueta como una pantalla real. Examina el material visual antes de publicarlo.

## Destacados / LinkedIn Projects

**Título:** SUWÓ GYM | Full-Stack .NET 10 & Azure

**Resumen:** Gym management platform with MVC + REST API, Dapper, Azure SQL and JWT authentication. Public Azure staging with validated member registration and login.

**Aptitudes:** C#, ASP.NET Core, SQL Server, Microsoft Azure, GitHub Actions.

**GitHub:** https://github.com/HenryGonzalezAlguera/SuwoGym

**Demo:** https://suwogym-stg-web-hga-emfxdjcfccabdfh2.centralus-01.azurewebsites.net/

## Antes de hacer clic en «Publicar»

- [ ] Abrir demo y enlaces desde navegador privado; **no se confirmó disponibilidad en tiempo real al redactar este documento**.
- [ ] Comprobar que la imagen no muestra correos, información personal, cookies ni credenciales.
- [ ] Evitar afirmaciones sobre pagos, reservas o administración de staging todavía no validadas.
- [ ] Adjuntar material real disponible, escoger versión en inglés o español y revisar vista previa.
- [ ] No utilizar datos reales en el staging público.
