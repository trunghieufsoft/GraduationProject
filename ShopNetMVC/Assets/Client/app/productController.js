var controller = {
    config: {
        page: 1,
        pageSize: 5
    },
    init: function () {
        var prodId = $('#commentInput').data('prodid');
        controller.loadRatings(prodId);
        controller.loadComments();
        controller.loadFeedbacks(prodId);
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
                        controller.removeComment(comId);
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
                        controller.removeReplies(comId, repNo);
                    }
                }
            });
        });

        // add rating to customer
        $('#add-rating').off('click').on('click', function () {
            var ri = $('.bigstars #rateit_star');
            controller.addRating($('#ratingContent').val(), ri.data('id'), ri.rateit('value'));
        });

        // edit comment
        $('i#edit-comment').off('click').on('click', function () {
            var btn = $(this);
            controller.handleEditClick(btn, true);
        });

        // edit reply
        $('i#edit-reply').off('click').on('click', function () {
            var btn = $(this);
            controller.handleEditClick(btn, false);
        });

        // handle lost focus input comment
        $('input#cmet').focusout(function () {
            var input = $(this);
            controller.handleFocusOut(input, true);
        });

        // handle lost focus input rep
        $('input#rep').focusout(function () {
            var input = $(this);
            controller.handleFocusOut(input, false);
        });

        // handle enter key
        $('input#cmet').on('keypress', function (e) {
            var inputs = $(this);
            controller.handleEnterKey(e, input, true);
        });

        // handle enter key
        $('input#rep').on('keypress', function (e) {
            var inputs = $(this);
            controller.handleEnterKey(e, input, false);
        });
    },
    removeComment: function (comId) {
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
    },
    removeReplies: function (comId, repNo) {
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
    },
    loadComments: function () {
        var prodId = $('#commentInput').data('prodid');
        $.ajax({
            url: '/comment/getcomments',
            dataType: 'json',
            type: 'Get',
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
                                    RepNo: elm.reply.RepNo,
                                    ComID: elm.reply.ComID,
                                    CreatedAt: main.formatDayTime(elm.reply.CreatedAt),
                                    isDelete: elm.reply.UserID === item.userID ? '<p><i id="trash-reply" data-comid="' + elm.reply.ComID + '" data-repno="' + elm.reply.RepNo + '" class="fa fa-trash-o" aria-hidden="true"></i></p>' : '',
                                    isEdit: elm.reply.UserID === item.userID ? '<p><i id="edit-reply" class="fa fa-pencil" aria-hidden="true"></i></p>' : ''
                                });
                                count++;
                            });
                        }
                        commentsHtml += Mustache.render(commentTemplate, {
                            ComID: item.comment.ComID,
                            Img: item.userGrant === 1 ? 'user' : 'custommer',
                            UserName: item.userName,
                            Content: item.comment.Content,
                            CreatedAt: main.formatDayTime(item.comment.CreatedAt),
                            Replies: repliesHtml,
                            isDelete: item.comment.UserID === item.userID ? '<p><i id="trash-comment" data-comid="' + item.comment.ComID + '" class="fa fa-trash-o" aria-hidden="true"></i></p>' : '',
                            isEdit: item.comment.UserID === item.userID ? '<p><i id="edit-comment" class="fa fa-pencil" aria-hidden="true"></i></p>' : ''
                        });
                        
                    });
                    $('#comments-list').html(commentsHtml);
                    controller.registerEvents();
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    },
    loadRatings: function (code) {
        $.ajax({
            url: '/rating/evaluationChart?code=' + code,
            type: 'Get',
            dataType: 'json',
            error: function (error) {
                $('#evaluation-chart').html(error.responseText);
                var value = $('#start-medium-val').val();
                $('#rateit_star0').rateit('value', +value);
            }
        });
        controller.registerEvents();
    },
    loadFeedbacks: function (code) {
        $.ajax({
            url: '/rating/getFeedback',
            data: {
                product: code,
                page: controller.config.page,
                pageSize: controller.config.pageSize
            },
            type: 'Post',
            dataType: 'json',
            success: function (response) {
                if (response.totalRows > 0) {
                    var template = $('#rating-template').html();
                    var html = '';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            Img: 'user',
                            UserName: response.Uname[i],
                            CreatedAt: main.formatDayTime(item.CreatedAt),
                            Content: item.Content,
                            Level: item.Level,
                            Level_width: 80 - (5 - item.Level) * 16
                        });
                    });
                    $('#feedback-main').html(html);
                    // paging
                    if (response.totalPages !== 1) {
                        $('#pagination').twbsPagination({
                            totalPages: response.totalPages,
                            visiblePages: 5,
                            first: 'Đầu',
                            prev: 'Trước',
                            last: 'Cuối',
                            next: 'Tiếp',
                            onPageClick: function (event, page) {
                                controller.config.page = page;
                                controller.loadRatings();
                            }
                        });
                    }
                }
            },
            error: function (error) {
                console.log(error);
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
    addRating: function (content, product, rating) {
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
                    controller.loadRatings(product);
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
    },
    handleEditClick: function (btn, isComment) {
        var parent = btn.parent().parent().parent();
        var content = parent.children('.comment-content');
        var p = content.children('p');
        var input = content.children(isComment ? 'input#cmet' : 'input#rep');
        input.attr('value', p.text());
        if (input.hasClass('hidden')) {
            input.removeClass('hidden');
            p.addClass('hidden');
        }
        // focus
        input.focus();
    },
    handleEnterKey: function (e, input, isComment) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            // Enter pressed... do anything here...
            var p = input.parent().children('p');
            if (p.hasClass('hidden')) {
                p.removeClass('hidden');
                input.addClass('hidden');
                controller.changeRepOrComment(isComment, input.data('comid'), !isComment ? input.data('repno') : null, input.val());
                p.text(input.val());
            }
        };
    },
    handleFocusOut: function (input, isComment) {
        var p = input.parent().children('p');
        if (p.hasClass('hidden')) {
            p.removeClass('hidden');
            input.addClass('hidden');
            controller.changeRepOrComment(isComment, input.data('comid'), !isComment ? input.data('repno') : null, input.val());
            p.text(input.val());
        }
    },
    changeRepOrComment: function (isComment, comment, rep, text) {
        var url = isComment ? '/comment/changeComment' : '/comment/changeReply';
        $.ajax({
            type: 'POST',
            data: {
                comId: comment,
                repNo: rep,
                value: text
            },
            url: url,
            dataType: 'json',
            success: function (reponse) {
                if (reponse.result == null) {
                    controller.loadComments();
                    console.log('change success!');
                }
                else {
                    console.log(reponse.result);
                }
            },
            error: function (error) {
                console.log(error.message);
            }
        });
    }
};

controller.init();