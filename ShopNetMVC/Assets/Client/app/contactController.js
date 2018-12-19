var controller = {
    init: function () {
        controller.registerEvents();
    },
    registerEvents: function () {
        $('#validator-name').off('click').on('click', controller.validateInfo(1));
        $('#validator-phone').off('click').on('click', controller.validateInfo(2));
        $('#validator-email').off('click').on('click', controller.validateInfo(3));
        $('#validator-content').off('click').on('click', controller.validateInfo(0));
        $("#submit_contact").submit(function (event) {
            event.preventDefault();
            var data = $('form-contact').serialize();
            if (data.name === '') {
                $('#validator-name').removeClass('validator');
                return false;
            }
            if (data.email === '' || !controller.validateEmail(data.email)) {
                $('#validator-email').removeClass('validator');
                return false;
            }
            if (data.content === '') {
                $('#validator-content').removeClass('validator');
                return false;
            }
            $.ajax({
                type: 'POST',
                url: '/contact/sentmail',
                data: data,
                dataType: 'json',
                success: function (response) {
                    if (response.status === true) {
                        bootbox.confirm({
                            message: `<p>` + response.message + `</p>`,
                            size: 'small',
                            title: 'Thông báo',
                            callback: function () {
                                window.location.href = "/";
                            }
                        });
                    } else {
                        bootbox.confirm({
                            message: response.message,
                            size: 'small',
                            title: 'Thông báo'
                        });
                    }
                },
                error: function (response) {
                    console.log(response.message);
                }
            });
        });

    },
    validateEmail: function (value) {
        var regex = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
        var rgx = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        return regex.test(value) && rgx.test(value);
    },
    validateInfo: function (controlIndex) {
        var control = controlIndex === 1 ? $('validator-name') : controlIndex === 2 ? $('validator-phone') : controlIndex === 3 ? $('validator-email') : $('validator-content');
        if (!control.hasClass("validator")) {
            control.addClass("validator");
        }
    }
};

controller.init();

