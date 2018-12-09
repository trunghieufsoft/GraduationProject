var controller = {
    init: function () {
        controller.registerEvents();
    },
    registerEvents: function () {
        $("#submitAddCart").off("click").on("click",
        function(e) {
            var quantity = $('.qty').val();
            var prodID = $('.prod').val();
            $.ajax({
                type: 'POST',
                url: '/cart/addtocart',
                data: { productId: +prodID, amount: +quantity },
                dataType: 'json',
                success: function (response) {
                    if (response.result === true) {
                        bootbox.confirm({
                            message: "Thêm sản phẩm vào giỏ hàng thành công",
                            size: 'small',
                            title: 'Thông báo',
                            callback: function (){ }
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
            })
        });
    },
};

controller.init();

