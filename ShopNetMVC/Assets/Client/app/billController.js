var controller = {
    config: {
        page: 1,
        pageSize: 10
    },
    init: function () {
        controller.getBills();
        controller.registerEvents();
    },
    getBills: function () {
        $.ajax({
            url: '/bill/getbills',
            type: 'GET',
            data: {
                pageIndex: controller.config.page,
                pageSize: controller.config.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    if (response.totalRows === 0) {
                        html = `<tr>
                                <td colspan="6" style="text-align: center">
                                    Chưa có đơn hàng!
                                </td>
                            </tr>`;
                    } else {
                        $.each(data, function (i, item) {
                            html += Mustache.render(template, {
                                BillID: item.BillID,
                                CustomerName: item.CustomerName,
                                TotalPrice: main.formatPrice(item.TotalPrice),
                                DeliveryAddress: item.DeliveryAddress,
                                CreatedAt: item.CreatedAt,
                                Status: item.Status === true ? 'Hoàn thành' : 'Đang xử lý',
                                Class: item.Status === true ? 'btn-success' : 'btn-danger'
                            });
                        });

                        main.paging(response.totalRows, response.totalPages, function () {
                            controller.getBills();
                        });
                    }
                    $('#table-data').html(html);
                    controller.registerEvents();

                }

            },
            error: function (error) {
                console.log(error);
            }
        });
    },
    registerEvents: function () {
        $('.btn-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            bootbox.confirm({
                size: 'small',
                message: 'Bạn muốn hoàn đổi trạng thái đơn hàng này?',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            url: '/bill/ChangeStatus',
                            type: 'POST',
                            data: {
                                billId: id
                            },
                            dataType: 'json',
                            success: function (response) {
                                controller.getBills();
                            }
                        });
                    }

                }
            });
        });
    }
};
controller.init();