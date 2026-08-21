
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
    $("#editarEmpleadoDPForm").validate({
        rules: {
            nombre: {
                required: true
            },
            apellido: {
                required: true
            },
            identificacion: {
                required: true
            },
            fechaNac: {
                required: true,
                mayorDe15: true
            },
            correoElectronico: {
                required: true,
                email: true
            },
            telefono: {
                required: true
            },
            direccion: {
                required: true
            }
        },
        messages: {
            nombre: {
                required: "Campo obligatorio."
            },
            apellido: {
                required: "Campo obligatorio."
            },
            identificacion: {
                required: "Campo obligatorio."
            },
            fechaNac: {
                required: "Campo obligatorio.",
                mayorDe15: "El empleado debe tener al menos 15 años"
            },
            telefono: {
                required: "Campo obligatorio."
            },
            correoElectronico: {
                required: "Campo obligatorio.",
                email: "Formato no válido."
            },
            direccion: {
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