(function () {
    $(function () {

        var _userService = abp.services.app.user;
        var _$modal = $('#UserCreateModal');
        var _$form = _$modal.find('form');

        $.validator.addMethod("alphanumeric", function(value, element) {
            return this.optional(element) || /^\w+$/i.test(value);
        }, "Tylko litery, cyfry i podkreślenia");
        
        _$form.validate({
            rules: {
                Password: "required",
                ConfirmPassword: {
                    equalTo: "#Password"
                },
                PlayerEmail: {
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

        $('#RefreshButton').click(function () {
            refreshUserList();
        });

        $('.delete-user').click(function () {
            var userId = $(this).attr("data-user-id");
            var userName = $(this).attr('data-user-name');

            deleteUser(userId, userName);
        });

        $('.edit-user').click(function (e) {
            const userId = $(this).attr("data-user-id");
            const $userEditModal = $("#UserEditModal");
            
            abp.ui.setBusy($userEditModal);
            e.preventDefault();
            abp.ajax({
                url: abp.appPath + 'Users/EditUserModal?userId=' + userId,
                type: 'POST',
                dataType: 'html',
                success: function (content) 
                {
                    $('#UserEditModal div.modal-content').html(content);
                    abp.ui.clearBusy($userEditModal);
                },
                error: function (e) 
                {
                    abp.ui.clearBusy($userEditModal);
                }
            });
        });

        _$form.find('button[type="submit"]').click(function (e) 
        {
            e.preventDefault();
            
            const $playerEmail = $("#PlayerEmail");
            $("#FirstName").val($playerEmail.val());
            $("#UserName").val($playerEmail.val());
            $("#tempUserEmail").val($playerEmail.val() + $("#siteEmail").data("site-email"));
            
            if (!_$form.valid())
            {
                return;
            }

            var user = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js
            user.roleNames = [];
            var _$roleCheckboxes = $("input[name='role']:checked.create-user");
            if (_$roleCheckboxes) {
                for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                    var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                    user.roleNames.push(_$roleCheckbox.attr('data-role-name'));
                }
            }

            abp.ui.setBusy(_$modal);
            _userService.create(user).done(function () {
                _$modal.modal('hide');
                location.reload(true); //reload page to see new user!
            }).always(function () {
                abp.ui.clearBusy(_$modal);
            });
        });

        _$modal.on('shown.bs.modal', function () {
            _$modal.find('input:not([type=hidden]):first').focus();
        });

        function refreshUserList() {
            location.reload(true); //reload page to see new user!
        }

        function deleteUser(userId, userName) {
            abp.message.confirm(
                "Usuń użytkownika '" + userName + "'?", "Usunąć!",
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userService.delete({
                            id: userId
                        }).done(function () {
                            refreshUserList();
                        });
                    }
                }
            );
        }
    });
})();
