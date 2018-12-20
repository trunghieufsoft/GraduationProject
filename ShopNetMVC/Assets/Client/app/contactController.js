var controller = {
    init: function () {
        controller.registerEvents();
    },
    registerEvents: function () {
        $('input#first').off('click').on('click', function (e) { controller.validateInfo(e, 1) });
        $('input#input-phone').off('click').on('click', function (e) { controller.validateInfo(e, 2) });
        $('input#input-phone').off('keyup').on('keyup', function (e) {
            $(this).val(controller.testNumber($(this).val()));
        });
        $('input#input-email').off('click').on('click', function (e) { controller.validateInfo(e, 3) });
        $('textarea#areatext-content').off('click').on('click', function (e) { controller.validateInfo(e, 0) });
        $("#form-contact").submit(function (event) {
            event.preventDefault();
            var dataJson = $(this).serialize();
            var dataText = {
                name: $('#first').val(),
                email: $('#input-email').val(),
                phone: $('#input-phone').val(),
                content: $('#areatext-content').val(),
            };
            if (dataText.name === '') {
                $('#validator-name').removeClass('validator');
                return false;
            }
            if (dataText.email === '' || !controller.validateEmail(dataText.email)) {
                $('#validator-email').removeClass('validator');
                return false;
            }
            if (dataText.phone === '') {
                $('#validator-phone').removeClass('validator');
                $('#validator-phone label').text('Vui lòng điền vào trường này.');
                return false;
            }
            if (dataText.phone.length !== 10) {
                $('#validator-phone').removeClass('validator');
                $('#validator-phone label').text('Vui lòng nhập đúng format 10 số.');
                return false;
            } else if (dataText.content === '') {
                $('#validator-content').removeClass('validator');
                return false;
            }
            $.ajax({
                type: 'POST',
                url: '/contact/sentmail',
                data: dataJson,
                dataType: 'json',
                success: function (response) {
                    if (response.alert === true) {
                        bootbox.confirm({
                            message: `<p>` + response.message + `</p>`,
                            size: 'small',
                            title: 'Thông báo',
                            callback: function () {
                                window.location.href = "/";
                            }
                        });
                    } else {
                        bootbox.alert({
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
    testNumber: function (value) {
        var rgx = /^[0-9]+$/g;
        if (!rgx.test(value)) {
            value = value.toString().substring(0, value.length -1);
            value = value;
        }
        return value;
    },
    validateEmail: function (value) {
        var regex = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
        var rgx = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        return regex.test(value) && rgx.test(value);
    },
    validateInfo: function (e, controlIndex) {
        event.preventDefault();
        var control = controlIndex === 1 ? $('#validator-name') : controlIndex === 2 ? $('#validator-phone') : controlIndex === 3 ? $('#validator-email') : $('#validator-content');
        if (!control.hasClass("validator")) {
            control.addClass("validator");
        }
    }
};

controller.init();

