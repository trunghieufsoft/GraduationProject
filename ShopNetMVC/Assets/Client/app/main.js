var main = {
    config: {
        pageIndex: 1,
        pageSize: 8
    },
    formatPrice: function (price) {
        var priceString = String(price);
        const comma = ',';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(priceString)) {
            priceString = priceString.replace(rgx, '$1' + comma + '$2');
        }
        return priceString + ' đồng';
    },
    formatPriceBill: function (price) {
        var priceString = String(price);
        const comma = ',';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(priceString)) {
            priceString = priceString.replace(rgx, '$1' + comma + '$2');
        }
        return priceString + ' đ';
    },
    formatDay: function (date) {
        if (date == '')
            return 'Chưa cập nhật';
        date = new Date(date);
        var dateString = date.toLocaleDateString('vi-vi', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            timeZone: 'Asia/Ho_Chi_Minh'
        });
        return dateString;
    },
    formatTime: function (date) {
        if (date == '')
            return 'Chưa cập nhật';
        date = new Date(date);
        var timeString = date.toLocaleTimeString('en-vi', {
            timeZone: 'Asia/Ho_Chi_Minh'
        });
        return timeString;
    },
    formatDayTime: function (date) {
        if (date == '')
            return 'Chưa có chỉnh sửa';
        date = new Date(date);
        var dateString = date.toLocaleDateString('vi-vi', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            timeZone: 'Asia/Ho_Chi_Minh'
        });
        var timeString = date.toLocaleTimeString('en-vi', {
            timeZone: 'Asia/Ho_Chi_Minh'
        });
        return dateString + ' ' + timeString;
    },
    testNumber: function (value) {
        var rgx = /^[0-9]+$/g;
        if (!rgx.test(value)) {
            value = value.toString().substring(0, value.length - 1);
            value = value;
        }
        return value;
    },
    validateEmail: function (value) {
        var regex = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
        var rgx = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        return regex.test(value) && rgx.test(value);
    },
    paging: function (totalRows,totalPages, callback) {
        if (totalPages !== 1) {
            $('#pagination').twbsPagination({
                totalPages: totalPages,
                visiblePages: 5,
                first: 'Đầu',
                prev: 'Trước',
                last: 'Cuối',
                next: 'Tiếp',
                onPageClick: function (event, page) {
                    main.config.pageIndex = page;
                    setTimeout(callback, 200);
                }
            });
        }
        var posStart = (main.config.pageIndex - 1) * main.config.pageSize + 1;
        var posEnd = posStart + main.config.pageSize - 1;
        if (posEnd > totalRows)
            posEnd = totalRows;
        var html = 'Hiển thị ' + posStart + ' đến ' + posEnd + ' trong tổng số ' + totalRows + ' mục';
        $('#table-info').html(html);
    }
};
