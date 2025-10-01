function initDataTable(selector, orderIndex = 1) {
    $(selector).DataTable({
        columnDefs: [
            { orderable: false, searchable: false, targets: 0 } // Kolom No
        ],
        order: [[orderIndex, 'asc']]
    }).on('order.dt search.dt', function () {
        let t = $(selector).DataTable();
        t.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
    }).draw();
}