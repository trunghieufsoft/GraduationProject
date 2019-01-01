var controller = {
    init: function () {
        controller.registerEvents();
    },
    registerEvents: function () {
        $('input#first').off('click').on('click', function (e) { main.validateInfo(e, 1) });
        $('input#input-phone').off('click').on('click', function (e) { main.validateInfo(e, 2) });
        $('input#input-phone').off('keyup').on('keyup', function (e) {
            $(this).val(main.testNumber($(this).val()));
        });
        $('input#input-email').off('click').on('click', function (e) { main.validateInfo(e, 3) });
        $('textarea#areatext-content').off('click').on('click', function (e) { main.validateInfo(e, 0) });
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
            if (dataText.email === '' || !main.validateEmail(dataText.email)) {
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
    }
};

controller.init();

