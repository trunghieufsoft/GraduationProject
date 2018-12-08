var grantConfig = {
    pageSize: 5,
    pageIndex: 1,
    search: null
}

var grantController = {
    init: function () {
        grantController.loadData();
    },
    registerEvent: function () {
        $('.btn-change').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            var text = btn.text() === "Kích hoạt" ? "khóa" : "kích hoạt";

            bootbox.confirm({
                message: 'Bạn muốn thay đổi trạng thái quyền này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    $.ajax({
                        type: 'POST',
                        url: '/admin/grant/changeStatus',
                        data: { id: id },
                        dataType: 'json',
                        success: function (response) {
                            grantController.loadData();
                        },
                        error: function (response) {
                            bootbox.alert({
                                message: response.message,
                                size: 'small'
                            });
                        }
                    });
                }
            });
        });

        $('#search').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                grantConfig.search = $(this).val();
                grantController.loadData(true);
            }
        });
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/admin/grant/loaddata',
            type: 'GET',
            data: {
                page: grantConfig.pageIndex,
                pageSize: grantConfig.pageSize,
                search: grantConfig.search
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.GrantID,
                            Name: item.GrantName,
                            CreatedAt: grantController.formatDate(item.CreatedAt),
                            UpdatedAt: grantController.formatDate(item.UpdatedAt),
                            Status: item.isActive ? "Kích hoạt" : "Khóa",
                            Class: item.isActive ? "label label-success" : "label label-danger",
                        });
                    });

                    $('#table-data').html(html);
                    if (response.total != 0) {
                        grantController.paging(response.total, function () {
                            grantController.loadData();
                        }, changePageSize);
                    }
                    grantController.registerEvent();
                }
            },
            error: function (respone) {

            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / grantConfig.pageSize);

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
                    grantConfig.pageIndex = page;
                    setTimeout(callback, 200);
                }
            });
        }
        var posStart = ((grantConfig.pageIndex - 1) * grantConfig.pageSize) + 1;
        var posEnd = posStart + (grantConfig.pageSize - 1);
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
grantController.init();