var controller = {
    init: function () {
        controller.loadData();
    },
    loadData: function () {
        $.ajax({
            url: '/product/homeproductspaging',
            type: 'get',
            data: {
                page: main.config.pageIndex,
                pageSize: main.config.pageSize
            },
            dataType: 'json',
            success: function (response) {
                var template = $('#template').html();
                var html = '';

                $.each(response.data, function (i, item) {
                    html += Mustache.render(template, {
                        ProdID: item.ProdID,
                        ProdName: item.ProdName,
                        Code: item.Code,
                        Cost: main.formatPrice(item.Cost),
                        Image: item.ImageUrl
                    });
                });

                // paging
                main.paging(response.totalRows, response.totalPages, function () {
                    controller.loadData();
                });

                $('#products').html(html);
            }
        });
        controller.registerEvents();
    },
    registerEvents: function () {
    }
};

controller.init();