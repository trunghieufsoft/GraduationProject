var controller = {
    init: function () {
        controller.registerEvents();
    },
    registerEvents: function () {
        $("#LoginUse").off("click").on("click", function (e) {
            var dialog = bootbox.dialog({
                title: 'Thông Tin Đăng Nhập',
                message: `
                <div class ="row">
                    <form id="form-login" class ="col-xs-12" style="padding: 0 10%">
                        <div class ="row form-group">
                            <label class ="col-xs-3 login">Tài khoản</label>
                            <div class ="col-xs-9">
                                <input type="text" id="uname" class ="form-control" name="username" onfocusout="focusout('validator-cusname')" />
                                <div id="validator-cusname" class ="hidden tooltips-validator"><label>Vui lòng điền vào trường này.</label></div>
                            </div>
                        </div>

                        <div class ="row form-group">
                            <label class ="col-xs-3 login">Mật khẩu</label>
                            <div class ="col-xs-9">
                                <input type="password" id="pass" class ="form-control" name="password" onfocusout="focusout('validator-password')" />
                                <div id="validator-password" class ="hidden tooltips-validator"><label>Vui lòng điền vào trường này.</label></div>
                            </div>
                        </div>

                        <div id="valid-login" style="text-align: center">
                            <span class ="field-validation-error text-danger"></span>
                        </div>
                    </form>
                    <script>
                        function focusout(attrid) {
                            element = document.getElementById(attrid);
                            var arr = element.className.split(" ");
                            if (arr.indexOf("hidden") == -1) {
                                element.className += " hidden";
                            }
                        }
                    </script>
                </div>
                `,
                buttons: {
                    cancel: {
                        label: 'Hủy',
                        className: 'default'
                    },
                    noclose: {
                        label: 'Đăng Nhập',
                        className: 'btn-info',
                        callback: function () {
                            var data = {
                                uname: $('#uname').val(),
                                passwd: $('#pass').val()
                            };
                            // validator
                            if (data.uname === "") {
                                $('#validator-cusname').removeClass('hidden');
                                $('input#uname').focus();
                                return false;
                            }
                            if (data.passwd === "") {
                                $('#validator-password').removeClass('hidden');
                                $('input#pass').focus();
                                return false;
                            } else {
                                // handle success
                                $.ajax({
                                    type: 'POST',
                                    url: '/user/login',
                                    data: data,
                                    dataType: 'json',
                                    success: function (response) {
                                        if (response.result === true) {
                                            var href = window.location.href;
                                            window.location.replace(href);
                                        } else {
                                            $('#valid-login span').text(response.message);
                                        }
                                    },
                                    error: function (response) {
                                        console.log(response.message);
                                    }
                                });
                            }
                            return false;
                        }
                    }
                }
            }).find('.modal-content').css({
                'margin-top': function () {
                    var w = $(window).height();
                    var b = $(".modal-dialog").height();
                    // should not be (w-h)/3
                    var h = (w - b) / 3;
                    return h + "px";
                }
            });
        });

        $("#LogoutUse").off("click").on("click", function (e) {
            $.ajax({
                type: 'POST',
                url: '/user/logout',
                dataType: 'json',
                success: function (response) {
                    if (response.result === true) {
                        var alert = bootbox.alert({
                            message: `<div class="text-center"><i class="fa fa-spin fa-spinner"></i> Loading...</div>`,
                            size: 'small'
                        }).find('.modal-content').css({
                            'margin-top': function () {
                                var w = $(window).height();
                                var b = $(".modal-dialog").height();
                                // should not be (w-h)/3
                                var h = (w - b) / 3;
                                return h + "px";
                            }
                        }).find('.modal-footer').css({
                            'display': 'none'
                        });
                        setTimeout(function () {
                            var href = window.location.href;
                            window.location.replace(href);
                        }, 1000);
                    } else {
                        console.log('error cmnr!');
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

