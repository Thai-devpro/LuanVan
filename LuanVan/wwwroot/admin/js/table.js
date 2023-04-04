$(document).ready(function () {
    $('#datatable-chiendich').dataTable({
        "columnDefs": [{
            "targets": 4, // chỉ định cột số 0
            "render": function (data, type, row) {
                if (type === 'display' && data.length > 50) {
                    return data.substr(0, 50) + '...';
                } else {
                    return data;
                }
            }
        }]
    });
    //$('#datatable-chiendich').dataTable({

    //});
    //$('#dataTableTang').dataTable({

    //});
    //$('#dataTableLoaiPhong').dataTable({

    //});

    //$('#dataTableDichVu').dataTable({

    //});
    //$('#dataTableDichVuPhong').dataTable({

    //});
});
