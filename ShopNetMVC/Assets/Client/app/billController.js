var controller = {
    config: {
        page: 1,
        pageSize: 10,
        bills: []
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
                    controller.config.bills = data;
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
                                Stt: i + 1,
                                BillID: item.BillID,
                                CustomerName: item.CustomerName,
                                TotalPrice: main.formatPriceBill(item.TotalPrice),
                                DeliveryAddress: item.DeliveryAddress,
                                CreatedAt: main.formatTime(item.CreatedAt),
                                Status: item.Status === true ? '<i class="fa fa-check-square-o" aria-hidden="true" title="đơn hàng đã được gửi"></i>' : '<i class="fa fa-circle-o" aria-hidden="true" title="đang chờ xác nhận"></i>',
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
                message: 'Bạn muốn thay đổi trạng thái đơn hàng này?',
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
        $('.btn-info').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            var detail = '';
            $.ajax({
                url: '/bill/billdetails',
                type: 'Post',
                data: {
                    billId: id
                },
                dataType: 'json',
                success: function (response) {
                    var data = response.data;
                    var template = $('#detail-template').html();
                    $.each(data, function (i, item) {
                        detail += Mustache.render(template, {
                            Stt: i + 1,
                            ProdId: item.ProdID,
                            ProdName: item.ProdName,
                            Count: response.count[i],
                            Cost: main.formatPriceBill(item.Cost),
                            ImageUrl: item.ImageUrl,
                            Price: main.formatPriceBill(item.Cost * response.count[i]),
                        });
                    });

                    bootbox.dialog({
                        title: 'Chi tiết đơn hàng ' + id,
                        size: 'large',
                        message: `
                  <table class ="table table-striped table-bordered table-hover">
                    <thead>
                        <th style="width: 4%">Stt</th>
                        <th style="width: 20%">Ảnh sản phẩm</th>
                        <th style="width: 21%">Tên sản phẩm</th>
                        <th style="width: 20%">Giá tiền</th>
                        <th style="width: 15%">Số lượng</th>
                        <th style="width: 20%">Thành tiền</th>
                    </thead>
                    <tbody>
                        `+ detail + `
                    </tbody>
                  </table>
                `,
                        buttons: {
                            cancel: {
                                label: "Đóng  ",
                                className: 'btn-default'
                            }
                        }
                    });

                }


            });

        });
    }
};
controller.init();