console.log("_editUserModal.js obecny");
    
(function ($) 
{
    var _userService = abp.services.app.user;
    var _$modal = $('#UserEditModal');
    var _$form = $('form[name=UserEditForm]');

    $.validator.addMethod("alphanumeric", function(value, element) {
        return this.optional(element) || /^\w+$/i.test(value);
    }, "Tylko litery, cyfry i podkreślenia");

    _$form.validate({
        rules: {
            EditEmail: {
                alphanumeric: true
            },
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
    
    const $editEmail = $("#EditEmail");
    $editEmail.on("input", () => 
    {
        $("#email").val($editEmail.val() + $("#siteEmail").data("site-email"));
        $("#username").val($editEmail.val());
        $("#name").val($editEmail.val());
    });
        
    function save() {

        if (!_$form.valid()) {
            return;
        }

        var user = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js
        user.roleNames = [];
        var _$roleCheckboxes = $("input[name='role']:checked.edit-user");
        if (_$roleCheckboxes) {
            for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                user.roleNames.push(_$roleCheckbox.attr('data-role-name'));
            }
        }

        abp.ui.setBusy(_$form);
        _userService.update(user).done(function () {
            _$modal.modal('hide');
            location.reload(true); //reload page to see edited user!
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    }

    //Handle save button click
    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    //Handle enter key
    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    $.AdminBSB.input.activate(_$form);

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first:visible').focus();
    });
})(jQuery);