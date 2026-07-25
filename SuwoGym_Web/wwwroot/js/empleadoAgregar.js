
// Restringir el selector de fecha
$.validator.addMethod(
    "mayorDe15",
    function (value, element) {

        if (!value) return true;

        let fechaNacimiento = new Date(value);
        let hoy = new Date();

        let edad = hoy.getFullYear() - fechaNacimiento.getFullYear();
        let mes = hoy.getMonth() - fechaNacimiento.getMonth();

        if (
            mes < 0 ||
            (mes === 0 && hoy.getDate() < fechaNacimiento.getDate())
        ) {
            edad--;
        }

        return edad >= 15;
    },
    "Debe tener al menos 15 años."
);


// Restricciones de forma

$(document).ready(function () {
    $("#EmpleadoAgregarForm").validate({
        rules: {
            Nombre: {
                required: true
            },
            Apellido: {
                required: true
            },
            Identificacion: {
                required: true
            },
            FechaNac: {
                required: true,
                mayorDe15: true
            },
            CorreoElectronico: {
                required: true,
                email: true
            },
            Telefono: {
                required: true
            },
            Rol: {
                required: true,
            },
            Direccion: {
                required: true
            }
        },
        messages: {
            Nombre: {
                required: "Campo obligatorio."
            },
            Apellido: {
                required: "Campo obligatorio."
            },
            Identificacion: {
                required: "Campo obligatorio."
            },
            FechaNac: {
                required: "Campo obligatorio.",
                mayorDe15: "El empleado debe tener al menos 15 años"
            },
            Telefono: {
                required: "Campo obligatorio."
            },
            CorreoElectronico: {
                required: "Campo obligatorio.",
                email: "Formato no válido."
            },
            Rol: {
                required: "Campo obligatorio."
            },
            Direccion: {
                required: "Campo obligatorio."
            }
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            error.addClass("text-danger small");
            error.insertAfter(element);
        },
        highlight: function (element) {
            $(element).addClass("is-invalid").removeClass("is-valid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid").addClass("is-valid");
        }
    });
});