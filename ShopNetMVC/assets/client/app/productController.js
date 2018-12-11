var controller = {
    config: {
       productId: 0,
    },
    init: function () {
        controller.loadComments();
        controller.registerEvents();
    },
    registerEvents: function () {
        //TODO: add to cart event

        // add comment event
        $('#btn-comment').off('click').on('click', function () {
            var comment = $('#commentInput').val();
            var productId = $('#productId').val();
            controller.addComment(comment, productId);           
            
        });

        $('#commentInput').off('keypress').on('keypress', function (e) {
            var btn = $(this);
            if (e.which === 13) {
                var comment = btn.val();
                var productId = $('#productId').val();
                controller.addComment(comment, productId);     
            }
        });


        //add reply event
    },
    loadComments: function () {
        var productId = $('#productId').val();
        $.ajax({
            url: '/comment/getcomments',
            dataType: 'json',
            type: 'GET',
            data: { productId: productId },
            success: function (response) {
                if (response.count > 0) {
                    var comment = $('#comment-template').html();
                    var html = '';
                    $.each(response.model, function (i, item) {
                        html += Mustache.render(comment, {
                            Content: item.Content,
                            UserID: item.UserID,
                            CreatedAt: item.CreatedAt,
                        });
                    });
                    $('#comments-list').html(html);
                }
            }
        });
    },
    addComment: function (comment, productId) {
        $.ajax({
            url: '/comment/addcomment',
            dataType: 'json',
            type: 'POST',
            data: { comment: comment, productId : productId },
            success: function (response) {
                if (!response.status) {
                    bootbox.alert({
                        size: 'small',
                        message: response.message
                    });
                } else {
                    //TODO: reload comment
                    $('#commentInput').val('');
                    controller.loadComments();
                }
            }
        });
    }
};

controller.init();