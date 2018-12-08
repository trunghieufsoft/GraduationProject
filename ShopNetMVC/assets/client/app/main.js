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
    },
};
