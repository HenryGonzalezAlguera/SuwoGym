$(function () {

    $.validator.addMethod(
        "caracterEspecial",
        function (value, element) {

            return this.optional(element) ||
                /[!@#$%^&*(),.?":{}|<>]/.test(value);

        },
        ""
    );

    $("#SeguridadForm").validate({

        rules: {

            contrasenna: {
                required: true,
                minlength: 6,
                caracterEspecial: true
            },

            confirmarContrasenna: {
                required: true,
                equalTo: "#contrasenna"
            }

        },

        messages: {

            contrasenna: {
                required: "Campo obligatorio",
                minlength: "Mínimo 6 caracteres",
                caracterEspecial:
                    "Al menos 1 carácter especial"
            },

            confirmarContrasenna: {
                required: "Campo obligatorio",
                equalTo: "Las contraseñas no coinciden"
            }

        },

        errorElement: "span",

        errorPlacement: function (error, element) {

            error.addClass(
                "text-danger small d-block");

            element
                .closest(".form-group")
                .after(error);
        },

        highlight: function (element) {

            $(element).addClass(
                "is-invalid");
        },

        unhighlight: function (element) {

            $(element)
                .removeClass("is-invalid")
                .addClass("is-valid");
        },

        submitHandler: function (form) {

            form.submit();
        }

    });

});