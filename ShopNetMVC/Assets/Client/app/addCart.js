var controller = {
    init: function () {
        controller.registerEvents();
    },
    registerEvents: function () {
        $("#btn-order").off("click").on("click",
        function(e) {
            var quantity = $('.qty').val();
            var prodID = $(this).data('id');
            $.ajax({
                type: 'POST',
                url: '/cart/addtocart',
                data: { productId: +prodID, amount: +quantity },
                dataType: 'json',
                success: function (response) {
                    if (response.result === true) {
                        bootbox.alert({
                            message: "Thêm sản phẩm vào giỏ hàng thành công.",
                            size: 'small'
                        });
                    } else {
                        bootbox.confirm({
                            message: "Thêm sản phẩm vào giỏ hàng thất bại",
                            size: 'small',
                            title: 'Thông báo',
                            callback: function () { }
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

