var cartController = {
    config: {
        page: 1,
        pageSize: 5
    },
    init: function () {
        cartController.loadData();
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/cart/getOrders',
            type: 'GET',
            data: {
                page: cartController.config.page,
                pageSize: cartController.config.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.result) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            Stt: i + ((response.page - 1) * response.pageSize) + 1,
                            Code: item.Product.Code,
                            ProdName: item.Product.ProdName,
                            Cost: main.formatPrice(item.Product.Cost),
                            ImageUrl: item.Product.ImageUrl,
                            Amount: item.Count,
                            ProdId: item.Product.ProdID
                        });
                    });
                    if (response.totalPrice < 1) {
                        html = `<tr>
                                <td colspan="7" style="text-align: center">
                                    Chưa có sản phẩm nào được chọn!
                                </td>
                            </tr>`;
                    }
                    $('#table-data').html(html);
                    $.ajax({
                        url: '/cart/payOrders',
                        type: 'GET',
                        dataType: 'json',
                        success: function (result) {
                        },
                        error: function (error) {
                            $('#bill-orders').html(error.responseText);
                        }
                    });
                    //paging
                    if (response.totalPages !== 0) {
                        cartController.paging(response.totalRows, changePageSize, function () {
                            cartController.loadData();
                        });
                    }

                    cartController.registerEvents();
                }
            },
            error: function (error) {
                bootbox.alert({
                    message: 'Không có sản phẩm nào trong giỏ hàng',
                    size: 'small'
                });
                console.log(error.message);
            }
        });
    },
    registerEvents: function () {
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var productId = btn.data('productid');
            var productName = btn.data('productname');
            bootbox.confirm({
                message: 'Xóa sản phẩm ' + productName +
                    ' khỏi giỏ hàng?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: 'POST',
                            data: {
                                id: productId
                            },
                            url: '/cart/deleteorder',
                            dataType: 'json',
                            success: function (response) {
                                if (response.result) {
                                    cartController.loadData(true);
                                } else {
                                    bootbox.alert({
                                        title: 'Thông báo',
                                        size: 'small',
                                        message: 'Đã xảy ra lỗi!'
                                    });
                                    console.log(response.message);
                                }
                            },
                            error: function (response) {
                                bootbox.alert({
                                    title: 'Thông báo',
                                    size: 'small',
                                    message: 'Đã xảy ra lỗi!'
                                });
                                console.log(response.message);
                            }
                        });
                    }
                }
            });
        });

        $('.btn-edit').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var id = btn.data('productid');

            var amount = btn.data('amount');

            $.ajax({
                url: '/product/getproduct',
                data: {
                    id: id
                },
                type: 'GET',
                dataType: 'json',
                success: function (response) {
                    var data = response.data;
                    if (response.status) {
                        var dialog = bootbox.dialog({
                            title: 'Thông tin đặt hàng',
                            message: `
                            <div class ="row">
	                            <div class ="col-md-6 product_img">
		                            <img style="height: 250px;width: 100%;" src= "` + data.ImageUrl + `" class ="img-responsive">
	                            </div>
	                            <div class ="col-md-6 product_content" style="font-weight: 400;color: #965c2c;font-weight: bold;">
		                            <p>Tên sản phẩm: <span class ="text-info" style="color: #e0a22f;"> ` + data.ProdName + ` </span></p>
		                            <p>Mã sản phẩm: ` + data.Code + ` </p>
		                            <p class ="cost">Giá sản phẩm: ` + main.formatPrice(data.Cost) + ` </p>
		                            <p>Mô tả sản phẩm: </p>
		                            <p>Sản phẩm nước giải khát được phân phối chính thức bởi công ty.</p>
		                            <div class ="row">
			                            <div class ="col-md-6 col-sm-6" style="vertical-align: middle;margin: 10px 0;">Số lượng: </div>
			                            <div class ="col-md-4 col-sm-4">
				                            <input type="number" min="0" value= "` + amount + `" name="amount" id="amount" class ="form-control" style="padding: 7px;">
			                            </div>
			                            <!--end col-->
			                            </div>
		                            <p class ="text-danger" id="amount-validation" name="amount-validation"></p>
	                            </div>
                            </div>
                        `,
                            buttons: {
                                cancel: {
                                    label: 'Hủy',
                                    className: 'default'
                                },
                                noclose: {
                                    label: 'Cập nhật',
                                    className: 'btn-info',
                                    callback: function () {
                                        var amount = $('#amount').val();
                                        if (+amount < 0 || amount === '') {
                                            $('#amount-validation').text('Số lượng đặt hàng lớn hơn 0');
                                        } else {
                                            $.ajax({
                                                type: 'POST',
                                                url: '/cart/updateorder',
                                                data: {
                                                    id: id,
                                                    count: +amount
                                                },
                                                dataType: 'json',
                                                success: function (response) {
                                                    if (response.result === true) {
                                                        dialog.modal('hide');
                                                        bootbox.alert({
                                                            message: "Cập nhật giỏ hàng thành công",
                                                            size: 'small'
                                                        });
                                                        cartController.loadData(true);
                                                    } else {
                                                        bootbox.alert({
                                                            message: "Cập nhật giỏ hàng thất bại",
                                                            size: 'small'
                                                        });
                                                    }
                                                },
                                                error: function (response) {
                                                    bootbox.alert({
                                                        message: 'Đã xảy ra lỗi',
                                                        size: 'small'
                                                    });
                                                    console.log(response.message);
                                                }
                                            });
                                        }
                                        return false;
                                    }
                                }
                            }
                        });
                    } else {
                        bootbox.alert({
                            title: 'Thông báo',
                            message: 'Đã xảy ra lỗi!',
                            size: 'small'
                        });
                        console.log(data.message);
                    }
                },
                error: function (response) {
                    bootbox.alert({
                        title: 'Thông báo',
                        message: 'Đã xảy ra lỗi!',
                        size: 'small'
                    });
                    console.log(response);
                }
            });
        });

        $('#add-bill').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var totalpay = btn.data('totalpay');
            if (totalpay > 0) {
                $.ajax({
                    url: '/bill/getaccountlogin',
                    type: 'POST',
                    dataType: 'json',
                    success: function (response) {
                        if (response.status) {
                            var info = response.info;
                            var fullName = info !== null ? info.FullName : '';
                            var phone = info !== null ? info.Phone : '';
                            var address = info !== null ? info.Address : '';

                            var dialog = bootbox.dialog({
                                title: 'Thông tin thanh toán',
                                message: `
                                <div class ="row contact-all">
                                    <form id="form-add-bill">
                                        <div class ="text-fields">
                                            <div class ="float-input tag-input">
                                                <input type="text" name="cusName" style="text-transform: capitalize;" id="cusName" class ="input-info" placeholder="họ tên khách hàng" value= "` + fullName + `" onfocusout="lostFocus('validator-cusname')">
                                                <span><i class ="fa fa-user tag-i"></i></span>
                                                <div id="validator-cusname" class ="validator tooltips-validator"><label>Vui lòng điền vào trường này.</label></div>
                                            </div>
                                        </div>
                                        <div class ="submit-area">
                                            <div class ="float-input tag-input">
                                                <input type="text" name="phone" id="phone" class ="input-info" placeholder="số điện thoại liên hệ" value= "` + phone + `" onkeyup="testNumber()" onfocusout="lostFocus('validator-phone')">
                                                <span><i class ="fa fa-phone tag-i"></i></span>
                                                <div id="validator-phone" class ="validator tooltips-validator"><label></label></div>
                                            </div>
                                        </div>
                                        <div class ="address-area">
                                            <div class ="float-input tag-input">
                                                <textarea name="address" cols="60" placeholder="địa chỉ giao hàng" id="address" class ="input-info" rows="7" onfocusout="lostFocus('validator-address')"> ` + address + ` </textarea>
                                                <span><i class ="fa fa-map-marker tag-i"></i></span>
                                                <div id="validator-address" class ="validator tooltips-validator"><label>Vui lòng điền vào trường này.</label></div>
                                            </div>
                                            <div class ="float-input tag-input">
                                                <textarea name="note" cols="60" placeholder="ghi chú" id="note" class ="input-info" rows="7"></textarea>
                                                <span><i class ="fa fa-comment tag-i"></i></span>
                                            </div>
                                        </div>
                                    </form>
                                    <script>
                                        function testNumber() {
                                            var rgx = /^[0-9]+$/g;
                                            var text = document.getElementById("phone").value;
                                            if (!rgx.test(text)) {
                                                text = text.substring(0, text.length -1);
                                                document.getElementById("phone").value = text;
                                            }
                                        };
                                        function lostFocus(attrid) {
                                            var name = "validator";
                                            var element = document.getElementById(attrid);
                                            var arr = element.className.split(" ");
                                            if (arr.indexOf(name) == -1) {
                                                element.className += " " +name;
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
                                        label: 'Đồng ý',
                                        className: 'btn-info',
                                        callback: function () {
                                            var data = { 
                                                userid: info !== null ? info.UserID : '',
                                                cusName: $('#cusName').val(),
                                                address: $('#address').val(),
                                                phone: $('#phone').val(),
                                                note: $('#note').val(),
                                                pay: totalpay
                                            };
                                            if (data.cusName === "") {
                                                $('#validator-cusname').removeClass('validator');
                                                $('#cusName').focus();
                                                return false;
                                            }
                                            if (data.phone === "") {
                                                $('#validator-phone').removeClass('validator');
                                                $('#validator-phone label').text('Vui lòng điền vào trường này.');
                                                $('#phone').focus();
                                                return false;
                                            }
                                            if (data.phone.length !== 10) {
                                                $('#validator-phone').removeClass('validator');
                                                $('#validator-phone label').text('Vui lòng nhập đúng format 10 số.');
                                                $('#phone').focus();
                                            } else if (data.address === "") {
                                                $('#validator-address').removeClass('validator');
                                                $('#address').focus();
                                            } else {
                                                $.ajax({
                                                    type: 'POST',
                                                    url: '/bill/addToBill',
                                                    data: data,
                                                    dataType: 'json',
                                                    success: function (response) {
                                                        if (response.result === true) {
                                                            dialog.modal('hide');
                                                            // ajax add order
                                                            $.ajax({
                                                                type: 'POST',
                                                                url: '/bill/addToOrder',
                                                                data: {
                                                                    request: response.billID
                                                                },
                                                                dataType: 'json',
                                                                success: function (response) {
                                                                    bootbox.alert({
                                                                        message: "Thêm đơn đặt hàng thành công",
                                                                        size: 'small'
                                                                    });
                                                                    cartController.loadData(true);
                                                                },
                                                                error: function (response) {
                                                                    bootbox.alert({
                                                                        message: 'Đã xảy ra lỗi khi add orders',
                                                                        size: 'small'
                                                                    });
                                                                    console.log(response.message);
                                                                }
                                                            });
                                                        } else {
                                                            bootbox.alert({
                                                                message: "Thêm đơn đặt hàng thất bại",
                                                                size: 'small'
                                                            });
                                                        }
                                                    },
                                                    error: function (response) {
                                                        bootbox.alert({
                                                            message: 'Đã xảy ra lỗi',
                                                            size: 'small'
                                                        });
                                                        console.log(response.message);
                                                    }
                                                });
                                            }
                                            return false;
                                        }
                                    }
                                }
                            });
                        } else {
                            bootbox.alert({
                                title: 'Thông báo',
                                message: 'Đã xảy ra lỗi!',
                                size: 'small'
                            });
                            console.log(data.message);
                        }
                    },
                    error: function (response) {
                        bootbox.alert({
                            title: 'Thông báo',
                            message: 'Đã xảy ra lỗi!',
                            size: 'small'
                        });
                        console.log(response);
                    }
                });
            } else {
                bootbox.alert({
                    title: 'Thông báo',
                    message: `
                        <div>
                            <label style="margin-bottom: 15px"> Chưa có sản phẩm nào trong giỏ hàng, </label> <br/ >
                            <label> Chọn sản phẩm mua hàng sau đó quay lại nhé!.</label>
                        </div>
                    `,
                });
            }
        });
    },
    paging: function (totalRows, changePageSize, callback) {
        var totalPages = Math.ceil(totalRows / cartController.config.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }
        if (totalPages > 1) {
            $('#pagination').twbsPagination({
                totalPages: totalPages,
                visiblePages: 5,
                first: 'Đầu',
                prev: 'Trước',
                last: 'Cuối',
                next: 'Tiếp',
                onPageClick: function (event, page) {
                    cartController.config.page = page;
                    setTimeout(callback, 10);
                }
            });
        }
        var posStart = (cartController.config.page - 1) * cartController.config.pageSize + 1;
        var posEnd = posStart + (cartController.config.pageSize - 1);
        if (posEnd > totalRows) {
            posEnd = totalRows;
        }

        var html = 'Hiển thị ' + posStart + ' đến ' + posEnd + ' trong tổng số ' + totalRows + ' sản phẩm';
        if (totalRows < 2) {
            html = "";
        }
        $('#dataTables_info').html(html);
    },
    getUserInfo: function () {
        $.ajax({
            url: '/user/getuserinfo',
            type: 'GET',
            data: {},
            dataType: 'json',
            success: function (response) {
                $('#customerName').val(response.user.UserID);
                $('#phone').val(response.user.Phone);
                $('#deliveryAddress').val(response.user.Address);
            },
            error: function (response) {
                bootbox.alert({
                    size: 'small',
                    message: 'Vui lòng đăng nhập!'
                });
                console.error(response.message);
            }
        });
    }
};

cartController.init();