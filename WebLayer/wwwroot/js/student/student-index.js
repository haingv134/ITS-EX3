var datatable;
$(document).ready(function () {
    window.addEventListener('resize', function (e) {
        datatable.draw();
    }, true)
})

$('#Reset').click(function () {
    if ($('#SearchBar').val().length > 0 || $('#ClassId').val().length > 0 || $('#Gender').val().length > 0) {
        $('#SearchBar').val('');
        $('#ClassId').val('0');
        $('#Gender').val('');
        datatable.draw();
    }
})
$('#Search').click(function () {
    if ($('#SearchBar').val().length > 0) {
        datatable.search($(this).val()).draw();
    }
})
$('#ClassId').on('change', function () {
    datatable.draw();
});
$('#Gender').on('change', function () {
    datatable.draw();
});

$(document).ready(function () {

    function renderGender(data, type, row, meta) {
        if (data === true) return 'Male';
        else return 'Female';
    }
    function renderDate(data, type, row, meta) {
        let date = new Date(data);
        return date.toDateString('dd/MM/yyyy');
    }
    function renderAction(data, type, row, meta) {
        return '<a class="btn btn-success btn-sm" onclick=OpenPopup(' + urlEditStudent(data) + ')> <i class="ti-pencil"></i>  Edit </a> | <a class="btn btn-danger btn-sm" onclick=DoAction(' + urlDeleteStudent(data) + ')> <i class="ti-trash"></i> Delete </a> ';
    }
    // default option for datatables
    $.extend(true, $.fn.dataTable.defaults, {
        processing: false, // showing 'processing' message while ajax is executing
        serverSide: true,  // handing request on server
        searching: false, //
        ordering: true, // allow order
        paging: true, // phan trang
        filter: false, // enable searching on input search
        stateSave: true, // save state of last table (lost if accessing with orther url or no session)
        autoWidth: true,
        pagingType: "full_numbers",
        responsive: true
    });

    datatable = $('#StudentTable').DataTable({

        ajax:
        {
            url: urlIndexStudent(),
            type: 'POST',
            data: function (d) {
                return $.extend({}, d, {
                    'classid': $('#ClassId').val(),
                    'gender': $('#Gender').val(),
                    'search': {
                        'value': $('#SearchBar').val(),
                        'regex': false
                    }
                });
            }
        },
        columnDefs: [
            { orderable: false, targets: [3, 5] },
            { width: "14%", targets: [0, 1, 2, 3, 4, 5] }
        ],
        columns: [
            { data: 'name', name: 'Name', title: 'Student Name' },
            { data: 'studentCode', name: 'StudentCode', title: 'Student Code' },
            { data: 'birthday', name: 'Birthday', title: 'Birthday', render: renderDate },
            { data: 'gender', name: 'Gender', title: 'Gender ', render: renderGender },
            { data: 'className', name: 'ClassName', title: 'Class Name' },
            { data: 'subjects', name: 'Subjects', title: 'Subjects' },
            {
                data: 'studentId', name: 'StudentId', title: 'Actions', render: renderAction,
                orderable: false,
                visiable: false,
                width: '15%'
            }
        ],

        order: [[0, 'asc'], [1, 'asc']],
        language: {
            emptyTable: 'No student found, please <b> Add Student </b> to show detail', // no data
            infoEmpty: 'No records avaiable',
            zeroRecords: 'Humm.... No result founded'
        }
    });
})
