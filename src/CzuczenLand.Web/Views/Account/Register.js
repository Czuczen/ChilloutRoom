(function ($) 
{
    if (!$) 
    {
        return;
    }

    $(function () 
    {
        var $registerForm = $('#RegisterForm');
        
        $.validator.addMethod("customUsername", function (value, element) 
        {
            if (value === $registerForm.find('input[name="EmailAddress"]').val()) 
            {
                return true;
            }

            //Username can not be an email address (except the email address entered)
            return !$.validator.methods.email.apply(this, arguments);
        }, abp.localization.localize("RegisterFormUserNameInvalidMessage", "CzuczenLand"));

        $.validator.addMethod("alphanumeric", function(value, element) {
            return this.optional(element) || /^\w+$/i.test(value);
        }, "Tylko litery, cyfry i podkreślenia");
        
        $registerForm.validate({
            rules: {
                UserName: {
                    required: true,
                    customUsername: true
                },
                RegisterEmail: {
                    alphanumeric: true
                },
                ConfirmPassword: {
                    equalTo: "#Password"
                }
            },

            highlight: function (input) {
                $(input).parents('.form-line').addClass('error');
            },

            unhighlight: function (input) {
                $(input).parents('.form-line').removeClass('error');
            },

            errorPlacement: function (error, element) {
                $(element).parents('.form-group').append(error);
            }
        });
    });
    
    $("#registerEmail").on("input", function ()
    {
        $("#FirstName").val($(this).val());
        $("#UserName").val($(this).val());
        $("#tempRegisterEmail").val($(this).val() + "@chilloutroom.pl")
    });

})(jQuery);
