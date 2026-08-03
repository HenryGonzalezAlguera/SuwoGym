$(document).ready(function () {

    $("#PlanModificarForm").validate({

        rules: {

            NombrePlan: {
                required: true,
                maxlength: 100
            },

            DuracionDias: {
                required: true,
                min: 1
            },

            Precio: {
                required: true,
                min: 0.01
            },

            Descripcion: {
                maxlength: 300
            }
        },

        messages: {

            NombrePlan: {
                required:
                    "El nombre del plan es obligatorio.",

                maxlength:
                    "El nombre no puede superar los 100 caracteres."
            },

            DuracionDias: {
                required:
                    "La duración es obligatoria.",

                min:
                    "La duración debe ser mayor que cero."
            },

            Precio: {
                required:
                    "El precio es obligatorio.",

                min:
                    "El precio debe ser mayor que cero."
            },

            Descripcion: {
                maxlength:
                    "La descripción no puede superar los 300 caracteres."
            }
        },

        errorElement: "span",

        errorClass:
            "text-danger"
    });

});