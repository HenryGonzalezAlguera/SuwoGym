

$("input[name='planMembresia']").on("change", function () {
    var idPlan = $(this).val();
    $("#plan").val(idPlan);
});


$.validator.addMethod(
    "caracterEspecial",
    function (value, element) {
        return this.optional(element) ||
            /[!@#$%^&*(),.?":{}|<>]/.test(value);
    },
    ""
);


// Restricciones de forma

$(document).ready(function () {
    $("#RegistroUsuarioFrom").validate({
        rules: {
            nombre: {
                required: true
            },
            apellido: {
                required: true
            },
            correoElectronico: {
                required: true,
                email: true
            },
            identificacion: {
                required: true
            },
            contrasenna: {
                required: true,
                minlength: 6,
                caracterEspecial: true
            },
            PlanMembresia: {
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
            correoElectronico: {
                required: "Campo obligatorio.",
                email: "Formato no válido."
            },
            identificacion: {
                required: "Campo obligatorio."
            },
            contrasenna: {
                required: "Campo obligatorio.",
                minlength: "Mínimo 6 caracteres",
                caracterEspecial:
                    "Al menos 1 carácter especial"
            },
            PlanMembresia: {
                required: "Debe seleccionar una membresía"
            }
        },
        errorElement: "span",
        errorPlacement: function (error, element) {

            error.addClass("text-danger small");
            error.insertAfter(element);
          
        },
        highlight: function (element) {
            $(element)
                .addClass("is-invalid")
                .removeClass("is-valid"); 
        },
        unhighlight: function (element) {
            $(element)
                .removeClass("is-invalid")
                .addClass("is-valid");
        }
    });
});