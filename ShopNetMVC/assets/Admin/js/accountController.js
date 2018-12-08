var accountConfig = {
    pageSize: 5,
    pageIndex: 1,
    search: null
}

var accountController = {
    init: function () {
        accountController.loadData();
    },
    registerEvents: function () {

        //change status
        $('.btn-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var id = btn.data('id');
            var text = btn.text() === "Kích hoạt"? "khóa" : "kích hoạt";
            bootbox.confirm({
                message: 'Bạn muốn ' + text + ' tài khoản này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: 'POST',
                            url: '/admin/account/changeStatus',
                            data: { id: id },
                            dataType: 'json',
                            success: function (response) {
                                accountController.loadData();
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
                message: 'Bạn muốn xóa tài khoản này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: 'POST',
                            url: '/admin/account/delete',
                            data: { id: id },
                            dataType: 'json',
                            success: function (response) {
                                if (response) {
                                    accountController.loadData(true);
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

        $('#search').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                accountConfig.search = $(this).val();
                accountController.loadData(true);
            }
        });
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/admin/account/loadData',
            type: 'GET',
            data: {
                page: accountConfig.pageIndex,
                pageSize: accountConfig.pageSize,
                search: accountConfig.search
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            UserID: item.UserID,
                            FullName: item.FullName,
                            GrantName: item.GrantName,
                            CreatedAt: accountController.formatDate(item.CreatedAt),
                            UpdatedAt: accountController.formatDate(item.UpdatedAt),
                            Active: item.isActive ? "Kích hoạt" : "Khóa",
                            Class: item.isActive ? "label label-success" : "label label-danger"
                        });
                    });

                    $('#table-data').html(html);
                    if (response.total != 0) {
                        accountController.paging(response.total, function () {
                            accountController.loadData();
                        }, changePageSize);
                    }
                    accountController.registerEvents();
                }
            },
            error: function () { }
        });
    },

    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / accountConfig.pageSize);

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
                    accountConfig.pageIndex = page;
                    setTimeout(callback, 200);
                }
            });
        }
        var posStart = ((accountConfig.pageIndex - 1) * accountConfig.pageSize) + 1;
        var posEnd = posStart + (accountConfig.pageSize - 1);
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

accountController.init();