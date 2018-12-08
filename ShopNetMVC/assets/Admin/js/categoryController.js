var config = {
    pageSize: 5,
    pageIndex: 1,
    search: null
}
var categoryController = {
    init: function () {
        categoryController.loadData();
    },
    registerEvent: function () {
        //change status
        $('.btn-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var id = btn.data('id');
            var text = btn.text() === 'Khóa' ? 'Kích hoạt' : 'Khóa';
            bootbox.confirm({
                message: 'Bạn muốn ' + text + ' danh mục sản phẩm này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: "POST",
                            url: '/admin/category/changeStatus',
                            data: { id: id },
                            dataType: 'json',
                            success: function (response) {
                                categoryController.loadData();
                            },
                            error: function (response) {
                                bootbox.alert({
                                    message : response.message,
                                    size : 'small'
                                });
                            }
                        });
                    }
                }
            });
        });

        //delete record
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var id = btn.data('id');

            bootbox.confirm({
                message: 'Bạn muốn xóa danh mục sản phẩm này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: "POST",
                            url: '/admin/category/delete',
                            data: { id: id },
                            dataType: 'json',
                            success: function (respone) {
                                if (respone === true) {
                                    categoryController.loadData(true);
                                }else{
                                    bootbox.alert({
                                        size: 'small',
                                        message: 'Không thể xóa danh mục sản phẩm này!'
                                    });
                                }
                            },
                            error: function (respone) {
                               bootbox.alert({
                                   size: 'small',
                                   message: 'Xóa danh mục sản phẩm không thành công!'
                               });
                            }
                        });
                    }
                }
            });
        });

        $('#search').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                config.search = $(this).val();
                categoryController.loadData(true);
            }
        });
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/admin/category/loadData',
            type: 'GET',
            data: {
                page: config.pageIndex,
                pageSize: config.pageSize,
                search: config.search
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.CateID,
                            Name: item.CateName,
                            Code: item.CodeName,
                            CreatedAt: categoryController.formatDate(item.CreatedAt),
                            UpdatedAt: categoryController.formatDate(item.UpdatedAt),
                            Class: item.isActive === true ? 'label label-success' : 'label label-danger',
                            Text: item.isActive === true ? 'Kích hoạt' : 'Khóa'
                        });
                    });
                    $('#table-data').html(html);
                    if (response.total != 0) {
                        categoryController.paging(response.total, function () {
                            categoryController.loadData();
                        }, changePageSize);
                    }
                    categoryController.registerEvent();
                }
                else {
                    bootbox.alert({
                        size: 'small',
                        message: response.message
                    });
                }
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / config.pageSize);

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
                    config.pageIndex = page;
                    setTimeout(callback, 200);
                }
            });
        }
        var posStart = ((config.pageIndex - 1) * config.pageSize) + 1;
        var posEnd = posStart + (config.pageSize - 1);
        if (posEnd > totalRow)
            posEnd = totalRow;
        var html = 'Hiển thị ' + posStart + ' đến ' + posEnd + ' trong tổng số ' + totalRow + ' mục';
        $('#dataTables_info').html(html);
    },

    formatDate: function (date) {
        if (date == '')
            return 'Chưa có chỉnh sửa';
        date = new Date(date);
        var dateString = date.toLocaleDateString('vi-vi', { year: 'numeric', month: 'long', day: 'numeric', timeZone: 'Asia/Ho_Chi_Minh' });
        var timeString = date.toLocaleTimeString('en-vi', { timeZone: 'Asia/Ho_Chi_Minh' });
        return dateString + ' vào lúc ' + timeString;
    }
};

categoryController.init();