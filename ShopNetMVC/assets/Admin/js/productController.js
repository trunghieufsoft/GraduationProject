var productConfig = {
    pageSize: 5,
    pageIndex: 1,
    search: null
};
var productController = {
    init: function () {
        productController.loadData();
    },
    registerEvent: function () {
        //change status
        $('.btn-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var id = btn.data('id');
            var text = btn.text() === "Kích hoạt" ? "khóa" : "kích hoạt";
            bootbox.confirm({
                message: 'Bạn muốn ' + text + ' sản phẩm này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: 'POST',
                            url: '/admin/product/changeStatus',
                            data: {
                                id: id
                            },
                            dataType: 'json',
                            success: function (response) {
                                productController.loadData();
                            },
                            error: function (response) {
                                bootbox.alert({
                                    message: response.message,
                                    size: 'small'
                                });
                            }
                        });
                    }
                }
            });
        });

        //delete
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var id = btn.data('id');

            bootbox.confirm({
                message: 'Bạn muốn xóa sản phẩm này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: 'POST',
                            url: '/admin/product/delete',
                            data: {
                                id: id
                            },
                            dataType: 'json',
                            success: function (response) {
                                if (response.status) {
                                    productController.loadData(true);
                                }
                            },
                            error: function (response) {
                                bootbox.alert({
                                    message: response.message,
                                    size: 'small'
                                });
                            }
                        });
                    }
                }
            });
        });

        //change image
        $('.product-image').off('click').on('click', function (e) {
            e.preventDefault();

            var btn = $(this);

            var id = btn.data('id');

            var imageUrl = btn.data('image');

            var dialog = bootbox.dialog({
                title: 'Thay đổi ảnh sản phẩm',
                message: `
                        <div class="form-group">
                            <label>Hình ảnh sản phẩm</label>
                            <img class="center-block img-responsive" id="image" src="` + imageUrl + `" style="height: 300px;" />
                        </div>
                        <div class="form-group">
                            <label>Đường dẫn sản phẩm</label>
                            <input type="text" data-val-required="Ảnh sản phẩm không được rỗng" id="image-url" value="` + imageUrl + `" class="form-control"/>
                            <span class="field-validation-error text-danger" id="validation-image-url"></span>
                            <a id="btnSelectImage" href="#">Chọn Ảnh</a>
                        </div>

                        <script>
                            $('#btnSelectImage').on('click', function (e) {
                                e.preventDefault();
                                var finder = new CKFinder();
                                finder.selectActionFunction = function (url) {
                                    $('#image-url').val(url);
                                    $('#image').attr("src",url);
                                };
                                finder.popup();
                            });
                        </script>
                        `,
                buttons: {
                    cancel: {
                        label: 'Hủy',
                        className: 'default'
                    },
                    noclose: {
                        label: 'Lưu',
                        className: 'btn-info',
                        callback: function () {
                            var imageUrl = $('#image-url').val();
                            if (imageUrl === "") {
                                $('#validation-image-url').text('Ảnh sản phẩm không được rỗng');
                            } else {
                                $.ajax({
                                    type: 'POST',
                                    url: '/admin/product/changeImage',
                                    data: {
                                        id: id,
                                        imageUrl: imageUrl
                                    },
                                    dataType: 'json',
                                    success: function (response) {
                                        if (response.status === true) {
                                            productController.loadData();
                                            dialog.modal('hide');
                                        } else {
                                            bootbox.alert({
                                                message: "Cập nhật hình ảnh thất bại! Ảnh đã có trong Db",
                                                size: 'small'
                                            });
                                        }
                                    },
                                    error: function (message) {
                                        bootbox.alert({
                                            message: message,
                                            size: 'small'
                                        });
                                    }
                                });
                            }
                            return false;
                        }
                    }
                }
            });
        });

        //search
        $('#search').off('keypress').on('keypress', function (e) {
            if (e.which === 13) {
                productConfig.search = $(this).val();
                productController.loadData(true);
            }
        });
    },

    // loadData
    loadData: function (changePageSize) {
        $.ajax({
            url: '/admin/product/loadData',
            type: 'GET',
            data: {
                page: productConfig.pageIndex,
                pageSize: productConfig.pageSize,
                search: productConfig.search
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;

                    var html = '';
                    var template = $('#data-template').html();

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProdID: item.ProdID,
                            ProdName: item.ProdName,
                            Cost: productController.formatPrice(item.Cost),
                            CreatedAt: productController.formatDate(item.CreatedAt),
                            UpdatedAt: productController.formatDate(item.UpdatedAt),
                            Active: item.isActive ? "Kích hoạt" : "Khóa",
                            Class: item.isActive ? "label label-success" : "label label-danger",
                            Image: item.ImageUrl
                        });
                    });

                    $('#table-data').html(html);
                    //paging
                    if (response.total != 0) {
                        productController.paging(response.total, function () {
                            productController.loadData();
                        }, changePageSize);
                    }
                    productController.registerEvent();
                }
            },
            error: function () {}
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / productConfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination li').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }
        if (totalPage != 1) {
            $('#pagination').twbsPagination({
                totalPages: totalPage,
                visiblePages: 5,
                first: 'Đầu',
                prev: 'Trước',
                last: 'Cuối',
                next: 'Tiếp',
                onPageClick: function (event, page) {
                    productConfig.pageIndex = page;
                    setTimeout(callback, 200);
                }
            });
        }
        var posStart = ((productConfig.pageIndex - 1) * productConfig.pageSize) + 1;
        var posEnd = posStart + (productConfig.pageSize - 1);
        if (posEnd > totalRow)
            posEnd = totalRow;
        var html = 'Hiển thị ' + posStart + ' đến ' + posEnd + ' trong tổng số ' + totalRow + ' mục';
        $('#dataTables_info').html(html);
    },

    formatDate: function (date) {
        if (date == '')
            return 'Chưa có chỉnh sửa';
        date = new Date(date);
        var dateString = date.toLocaleDateString('vi-vi', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            timeZone: 'Asia/Ho_Chi_Minh'
        });
        var timeString = date.toLocaleTimeString('en-vi', {
            timeZone: 'Asia/Ho_Chi_Minh'
        });
        return dateString + ' vào lúc ' + timeString;
    },

    formatPrice: function (price) {
        var priceString = String(price);
        const comma = ',';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(priceString)) {
            priceString = priceString.replace(rgx, '$1' + comma + '$2');
        }
        return priceString + ' đồng';
    }
};

productController.init();