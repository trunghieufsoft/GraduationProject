var controller = {
    init: function () {
        controller.registerEvents();
    },
    registerEvents: function () {
        $('#btnRegister').off('click').on('click', function () {
            var data = main.toJsonObject('registerForm');
            controller.register(data);
        });
    },
    register: function (data) {
        $.ajax({
            url: '/user/register',
            data: data,
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (response.success) {
                    window.location.replace('/');
                } else {
                    $('#validation').text(response.message);
                }
            }
        });
    }
};

controller.init();