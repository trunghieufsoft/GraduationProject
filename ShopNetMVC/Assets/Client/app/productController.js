var controller = {
    config: {

    },
    init: function () {
        controller.loadComments();
    },
    registerEvents: function () {
        // add comment event
        $('#commentInput').off('keypress').on('keypress', function (e) {
            var btn = $(this);
            if (e.which === 13) {
                var comment = btn.val();
                var prodId = btn.data('prodid');
                controller.addComment(comment, prodId);
            }
        });

        // add reply event
        $('.reply-input').off('keypress').on('keypress', function (e) {
            var btn = $(this);
            if (e.which === 13) {
                var comment = btn.val();
                var comId = btn.data('comid');
                controller.addReply(comment, comId);
                btn.val('');
            }
        });

        // delete comment
        $('i#trash-comment').on('click', function (e) {
            var comId = $(this).data('comid');
            bootbox.confirm({
                message: 'Bạn có chắc muốn xóa bình luận này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: 'POST',
                            data: {
                                request: comId
                            },
                            url: '/comment/deletecomment',
                            dataType: 'json',
                            success: function (response) {
                                if (response.result) {
                                    controller.loadComments();
                                } else {
                                    bootbox.alert({
                                        title: 'Thông báo',
                                        size: 'small',
                                        message: 'Đã xảy ra lỗi!'
                                    });
                                    console.log(response.message);
                                }
                            },
                            error: function (response) {
                                bootbox.alert({
                                    title: 'Thông báo',
                                    size: 'small',
                                    message: 'Đã xảy ra lỗi!'
                                });
                                console.log(response.message);
                            }
                        })
                    }
                }
            });
        });

        // delete reply
        $('i#trash-reply').on('click', function (e) {
            var comId = $(this).data('comid');
            var repNo = $(this).data('repno');
            bootbox.confirm({
                message: 'Bạn có chắc muốn xóa phản hồi bình luận này?',
                size: 'small',
                title: 'Thông báo',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: 'POST',
                            data: {
                                comId: comId,
                                repNo: repNo
                            },
                            url: '/comment/deletereply',
                            dataType: 'json',
                            success: function (response) {
                                if (response.result) {
                                    controller.loadComments();
                                } else {
                                    bootbox.alert({
                                        title: 'Thông báo',
                                        size: 'small',
                                        message: 'Đã xảy ra lỗi!'
                                    });
                                    console.log(response.message);
                                }
                            },
                            error: function (response) {
                                bootbox.alert({
                                    title: 'Thông báo',
                                    size: 'small',
                                    message: 'Đã xảy ra lỗi!'
                                });
                                console.log(response.message);
                            }
                        })
                    }
                }
            });
        });

        // add rating to customer
        $('#add-rating').off('click').on('click', function () {
            var ri = $('.bigstars #rateit_star');
            var rating = ri.rateit('value');
            var product = ri.data('id');
            var content = $('#ratingContent').val();
            // handle
            $.ajax({
                url: '/rating/addrating',
                type: 'POST',
                dataType: 'json',
                data: {
                    content: content,
                    product: product,
                    rating: rating
                },
                success: function (response) {
                    if (response.status) {
                        $('#ratingContent').val('');
                        controller.loadComments(product);
                        bootbox.alert({
                            message: 'Bạn đã đánh giá ' + rating + ' sao cho sản phẩm ' + response.prdName,
                            size: 'small'
                        });
                    } else {
                        bootbox.alert({
                            message: response.message,
                            size: 'small'
                        });
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        })
    },
    loadComments: function () {
        var prodId = $('#commentInput').data('prodid');
        $.ajax({
            url: '/comment/getcomments',
            dataType: 'json',
            type: 'GET',
            data: { request: prodId },
            success: function (response) {
                if (response.status && response.comment) {
                    var commentTemplate = $('#comment-template').html();
                    var replyTemplate = $('#reply-template').html();
                    /**
                     * Set data form buiding value
                     * response.comment: list<object> = [ {comment: Comments, userName: string, replies: Replies[]} ]
                     * trong đó: Replies: null | object = {reply: Replies, userName: string}
                     */
                    var commentsHtml = '';
                    $.each(response.comment, function (i, item) {
                        var count = 0;
                        if (item.replies !== undefined && item.replies !== null) {
                            var repliesHtml = '';
                            $.each(item.replies, function (i, elm) {
                                repliesHtml += Mustache.render(replyTemplate, {
                                    Img: elm.userGrant === 1 ? 'user' : 'custommer',
                                    UserName: elm.userName,
                                    Content: elm.reply.Content,
                                    CreatedAt: elm.reply.CreatedAt,
                                    isDelete: elm.reply.UserID === item.userID ? '<p><i id="trash-reply" data-comid="' + elm.reply.ComID + '" data-repno="' + elm.reply.RepNo + '" class="fa fa-trash-o" aria-hidden="true"></i></p>' : ''
                                });
                                count++;
                            });
                        }
                        commentsHtml += Mustache.render(commentTemplate, {
                            ComID: item.comment.ComID,
                            Img: item.userGrant === 1 ? 'user' : 'custommer',
                            UserName: item.userName,
                            Content: item.comment.Content,
                            CreatedAt: item.comment.CreatedAt,
                            Replies: repliesHtml,
                            Height: 93 * count + 54,
                            isDelete: item.comment.UserID === item.userID ? '<p><i id="trash-comment" data-comid="' + item.comment.ComID  + '" class="fa fa-trash-o" aria-hidden="true"></i></p>' : ''
                        });
                        
                    });
                    $('#comments-list').html(commentsHtml);
                    controller.loadRatings(prodId);
                    controller.registerEvents();
                }
            }
        });
    },
    loadRatings: function (code) {
        $.ajax({
            url: '/rating/evaluationChart?code=' + code,
            type: 'GET',
            dataType: 'json',
            success: function () { },
            error: function (error) {
                $('#evaluation-chart').html(error.responseText);
            }
        });
        controller.registerEvents();
    },
    addReply: function (comment, comId) {
        $.ajax({
            url: '/comment/addreply',
            dataType: 'json',
            type: 'POST',
            data: { comment: comment, comId: comId },
            success: function (response) {
                if (!response.status) {
                    bootbox.alert({
                        size: 'small',
                        message: response.message
                    });
                } else {
                    //TODO: reload comment
                    controller.loadComments();
                }
            }
        });
    },
    addComment: function (comment, prodId) {
        $.ajax({
            url: '/comment/addcomment',
            dataType: 'json',
            type: 'POST',
            data: { comment: comment, prodId : prodId },
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
    },

};

controller.init();