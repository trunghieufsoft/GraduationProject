var controller = {
    init: function () {
        controller.registerEvents();
    },
    validateInfo: function (controlIndex) {
        var control = controlIndex === 1 ? $('#validator-name') : controlIndex === 2 ? $('#validator-phone') : controlIndex === 3 ? $('#validator-email') : $('#validator-content');
        if (!control.hasClass("validator")) {
            control.addClass("validator");
        }
    },
    registerEvents: function () {
        $('input#first').focusout(function () { controller.validateInfo(1) });
        $('input#input-phone').focusout(function () { controller.validateInfo(2) });
        $('input#input-phone').off('click').on('click', function (e) {
            $(this).val(main.testNumber($(this).val()));
        });
        $('input#input-email').focusout(function () { controller.validateInfo(3) });
        $('textarea#areatext-content').focusout(function () { controller.validateInfo(0) });
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
                $('#first').focus();
                return false;
            }
            if (dataText.email === '' || !main.validateEmail(dataText.email)) {
                $('#validator-email').removeClass('validator');
                $('#input-email').focus();
                return false;
            }
            if (dataText.phone === '') {
                $('#validator-phone').removeClass('validator');
                $('#validator-phone label').text('Vui lòng điền vào trường này.');
                $('#input-phone').focus();
                return false;
            }
            if (dataText.phone.length !== 10) {
                $('#validator-phone').removeClass('validator');
                $('#validator-phone label').text('Vui lòng nhập đúng format 10 số.');
                $('#input-phone').focus();
                return false;
            } else if (dataText.content === '') {
                $('#validator-content').removeClass('validator');
                $('#areatext-content').focus();
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
    }
};

controller.init();

