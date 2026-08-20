

// Restricciones de forma

$(document).ready(function () {
    $("#IniciarSesionForm").validate({
        rules: {
            correoElectronico: {
                required: true,
                email: true
            },
            contrasenna: {
                required: true,
                minlength: 6
            }
        },
        messages: {
            correoElectronico: {
                required: "Campo obligatorio.",
                email: "Formato no válido."
            },
            contrasenna: {
                required: "Campo obligatorio.",
                minlength: "Mínimo 6 caracteres"
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