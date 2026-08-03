using Microsoft.AspNetCore.Mvc;
using SuwoGym_Web.Models;
using SuwoGym_Web.Models.SuwoGym_Web.Models;
using System.Collections.Generic;

namespace HomePage.Controllers
{
    public class HomePageController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomePageModel
            {
                InfoGeneral = new InfoGeneral
                {
                    NombreGimnasio = "SUWO GYM",
                    Direccion = "Av. Principal #123, San Jose, Costa rica",
                    Telefono = "+506 22606269",
                    Email = "suwo@gmail.com",
                    Descripcion = "El mejor lugar para transformar tu cuerpo y mente. Contamos con instalaciones de primer nivel y entrenadores certificados.",
                    HorarioAtencion = "Lun - Vie: 6:00 AM - 10:00 PM | Sáb: 8:00 AM - 6:00 PM",
                    RedesSociales = "@suwogym"
                },
                Metodologias = new List<MetodologiaEntrenamiento>
                {
                    new MetodologiaEntrenamiento
                    {
                        Id = 1,
                        Nombre = "Funcional",
                        Descripcion = "Entrenamiento basado en movimientos naturales que mejoran tu fuerza, estabilidad y rendimiento para el día a día.",
                        Icono = "fa-solid fa-dumbbell",
                        Color = "#ff5500",
                        Beneficios = new List<string>
                        {
                            "Mejora la movilidad articular",
                            "Aumenta la fuerza funcional",
                            "Previene lesiones",
                            "Entrenamiento adaptado a todos los niveles"
                        },
                        ImagenUrl = "https://images.unsplash.com/photo-1518611012118-696072aa579a?w=500"
                    },
                    new MetodologiaEntrenamiento
                    {
                        Id = 2,
                        Nombre = "CrossFit",
                        Descripcion = "Entrenamiento de alta intensidad que combina levantamiento de pesas, cardio y gimnasia para resultados extremos.",
                        Icono = "fa-solid fa-bolt",
                        Color = "#007bff",
                        Beneficios = new List<string>
                        {
                            "Quema grasa rápidamente",
                            "Desarrolla resistencia extrema",
                            "Fortalece todo el cuerpo",
                            "Comunidad motivadora"
                        },
                        ImagenUrl = "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=500"
                    },
                    new MetodologiaEntrenamiento
                    {
                        Id = 3,
                        Nombre = "Baile",
                        Descripcion = "Diviértete mientras quemas calorías con nuestras clases de baile llenas de energía y ritmo.",
                        Icono = "fa-solid fa-music",
                        Color = "#ff6b6b",
                        Beneficios = new List<string>
                        {
                            "Cardiovascular divertido",
                            "Mejora la coordinación",
                            "Reduce el estrés",
                            "Ritmos variados: Salsa, Zumba, Urbano"
                        },
                        ImagenUrl = "https://images.unsplash.com/photo-1518834107818-4b0c989b2b7c?w=500"
                    }
                },
                Membresias = new List<Membresia>
                {
                    new Membresia
                    {
                        Id = 1,
                        Tipo = "Mensual",
                        Precio = 45000,
                        Duracion = "mes",
                        EsPopular = false,
                        Color = "#293039",
                        Descripcion = "Perfecto para comenzar tu transformación",
                        Beneficios = new List<string>
                        {
                            "Acceso ilimitado al gimnasio",
                            "2 clases grupales por semana",
                            "Plan nutricional básico",
                            "Aplicación móvil incluida"
                        }
                    },
                    new Membresia
                    {
                        Id = 2,
                        Tipo = "Trimestral",
                        Precio = 120000,
                        Duracion = "trimestre",
                        EsPopular = true,
                        Color = "#ff5500",
                        Descripcion = "¡La favorita de nuestros miembros!",
                        Beneficios = new List<string>
                        {
                            "Acceso ilimitado al gimnasio",
                            "4 clases grupales por semana",
                            "Plan nutricional premium",
                            "Evaluación física mensual",
                            "Camiseta SUWO GYM"
                        }
                    },
                    new Membresia
                    {
                        Id = 3,
                        Tipo = "Anual",
                        Precio = 420000,
                        Duracion = "año",
                        EsPopular = false,
                        Color = "#293039",
                        Descripcion = "El compromiso total con tu salud",
                        Beneficios = new List<string>
                        {
                            "Acceso ilimitado al gimnasio",
                            "Clases grupales ilimitadas",
                            "Plan nutricional premium",
                            "Evaluación física mensual",
                            "Kit completo SUWO GYM",
                            "2 meses gratis"
                        }
                    }
                },
                Horarios = new CronogramaSemanal
                {
                    Dias = new List<DiaHorario>
                    {
                        new DiaHorario
                        {
                            Dia = "Lunes",
                            DiaSemana = "lunes",
                            Clases = new List<Clase>
                            {
                                new Clase { HoraInicio = "07:00", HoraFin = "08:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Principiante", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "09:00", HoraFin = "10:00", Actividad = "CrossFit", Instructor = "María González", Nivel = "Intermedio", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "11:00", HoraFin = "12:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Todos", ColorTag = "#ff6b6b" },
                                new Clase { HoraInicio = "15:00", HoraFin = "16:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Intermedio", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "17:00", HoraFin = "18:00", Actividad = "CrossFit", Instructor = "María González", Nivel = "Avanzado", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "19:00", HoraFin = "20:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Todos", ColorTag = "#ff6b6b" }
                            }
                        },
                        new DiaHorario
                        {
                            Dia = "Martes",
                            DiaSemana = "martes",
                            Clases = new List<Clase>
                            {
                                new Clase { HoraInicio = "08:00", HoraFin = "09:00", Actividad = "CrossFit", Instructor = "Andrés Rojas", Nivel = "Principiante", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "10:00", HoraFin = "11:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Todos", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "12:00", HoraFin = "13:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Intermedio", ColorTag = "#ff6b6b" },
                                new Clase { HoraInicio = "16:00", HoraFin = "17:00", Actividad = "CrossFit", Instructor = "Andrés Rojas", Nivel = "Intermedio", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "18:00", HoraFin = "19:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Avanzado", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "20:00", HoraFin = "21:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Todos", ColorTag = "#ff6b6b" }
                            }
                        },
                        new DiaHorario
                        {
                            Dia = "Miércoles",
                            DiaSemana = "miercoles",
                            Clases = new List<Clase>
                            {
                                new Clase { HoraInicio = "07:00", HoraFin = "08:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Principiante", ColorTag = "#ff6b6b" },
                                new Clase { HoraInicio = "09:00", HoraFin = "10:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Intermedio", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "11:00", HoraFin = "12:00", Actividad = "CrossFit", Instructor = "María González", Nivel = "Todos", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "14:00", HoraFin = "15:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Intermedio", ColorTag = "#ff6b6b" },
                                new Clase { HoraInicio = "16:00", HoraFin = "17:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Avanzado", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "18:00", HoraFin = "19:00", Actividad = "CrossFit", Instructor = "María González", Nivel = "Avanzado", ColorTag = "#007bff" }
                            }
                        },
                        new DiaHorario
                        {
                            Dia = "Jueves",
                            DiaSemana = "jueves",
                            Clases = new List<Clase>
                            {
                                new Clase { HoraInicio = "08:00", HoraFin = "09:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Principiante", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "10:00", HoraFin = "11:00", Actividad = "CrossFit", Instructor = "Andrés Rojas", Nivel = "Todos", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "12:00", HoraFin = "13:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Intermedio", ColorTag = "#ff6b6b" },
                                new Clase { HoraInicio = "15:00", HoraFin = "16:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Intermedio", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "17:00", HoraFin = "18:00", Actividad = "CrossFit", Instructor = "Andrés Rojas", Nivel = "Avanzado", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "19:00", HoraFin = "20:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Todos", ColorTag = "#ff6b6b" }
                            }
                        },
                        new DiaHorario
                        {
                            Dia = "Viernes",
                            DiaSemana = "viernes",
                            Clases = new List<Clase>
                            {
                                new Clase { HoraInicio = "07:00", HoraFin = "08:00", Actividad = "CrossFit", Instructor = "María González", Nivel = "Principiante", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "09:00", HoraFin = "10:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Intermedio", ColorTag = "#ff6b6b" },
                                new Clase { HoraInicio = "11:00", HoraFin = "12:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Todos", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "14:00", HoraFin = "15:00", Actividad = "CrossFit", Instructor = "María González", Nivel = "Intermedio", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "16:00", HoraFin = "17:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Avanzado", ColorTag = "#ff6b6b" },
                                new Clase { HoraInicio = "18:00", HoraFin = "19:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Avanzado", ColorTag = "#ff5500" }
                            }
                        },
                        new DiaHorario
                        {
                            Dia = "Sábado",
                            DiaSemana = "sabado",
                            Clases = new List<Clase>
                            {
                                new Clase { HoraInicio = "09:00", HoraFin = "10:00", Actividad = "Funcional", Instructor = "Carlos Pérez", Nivel = "Todos", ColorTag = "#ff5500" },
                                new Clase { HoraInicio = "10:30", HoraFin = "11:30", Actividad = "CrossFit", Instructor = "Andrés Rojas", Nivel = "Todos", ColorTag = "#007bff" },
                                new Clase { HoraInicio = "12:00", HoraFin = "13:00", Actividad = "Baile", Instructor = "Laura Martínez", Nivel = "Todos", ColorTag = "#ff6b6b" }
                            }
                        }
                    }
                }
            };

            return View(model);
        }

       
    }
}