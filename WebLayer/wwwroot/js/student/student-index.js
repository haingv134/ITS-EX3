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

    // reload datatable after ajax request completed
    let datatableAjaxResponse = {};
    ajaxResponseStatus = new Proxy(datatableAjaxResponse, {
        set: () => {
            datatable.ajax.reload();
        }
    })

    function renderGender(data, type, row, meta) {
        if (data === true) return 'Male';
        else return 'Female';
    }
    function renderUpdate(data, type, row, meta) {
        return '<a class="text-decoration-none" href="#" onclick=OpenPopup(' + urlEditStudent(row.studentId) + ')>' + data + ' </a>';
    }
    function renderDelete(data, type, row, meta) {
        return '<a  href="#" onclick=DoAction(' + urlDeleteStudent(data) + ')> <i class="ti-trash"></i></a>';
    }
    function renderSubject(response) {
        let res = '';
        if (response.data) {
            for (let index = 0; index < response.data.length; index++) {
                const element = response.data[index];
                res += '<a href="#" onclick=OpenPopup("' + urlEditSubject(element.subjectId) + '")>' + element.name + '</a> ';
            }
        }
        document.querySelector('#subject-list-td').innerHTML = res;
    }
    function renderClass(response) {
        if (response.data) {
            document.querySelector('#class-td').innerHTML = '<a href="#" onclick=OpenPopup("' + urlEditClass(response.data.classId) + '")>' + response.data.name + '</a> ';;
        }
    }

    /* Formatting function for row details - modify as you need */
    function format(d) {
        AjaxRequest(urlSubjectListByStudent(d.studentId), renderSubject);
        AjaxRequest(urlGetClassByStudent(d.studentId), renderClass);

        // `d` is the original data object for the row
        return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
            '<td> Year old:</td>' +
            '<td>' + (new Date().getFullYear() - parseInt(d.birthday.split('/')[2])) + ' years old</td>' +
            '</tr>' +
            '<tr>' +
            '<td> Subject Assigned: </td>' +
            '<td id="subject-list-td"> </td>' +
            '</tr>' +
            '<tr>' +
            '<td> Class </td>' +
            '<td  id="class-td"></td>' +
            '</tr>' +
            '<tr>' +
            '<td>Extra info:</td>' +
            '<td>' + d.extraInfor + '</td>' +
            '</tr>' +
            '</table>';
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
            { orderable: false, width: '5%', targets: [0] },
            { orderable: false, targets: [6] },
            { autoWidth: true, targets: [1, 2, 3, 4, 5] }
        ],
        order: [[1, 'asc'], [2, 'asc']],
        columns: [
            { "className": 'dt-control', "orderable": false, "data": null, "defaultContent": '' },
            { data: 'name', name: 'Name', title: 'Name', render: renderUpdate },
            { data: 'studentCode', name: 'StudentCode', title: 'Code' },
            { data: 'birthday', name: 'Birthday', title: 'Birthday' },
            { data: 'gender', name: 'Gender', title: 'Gender ', render: renderGender },
            { data: 'yearOfEnroll', name: 'YearOfEnroll', title: 'Year Of Enroll' },
            { data: 'studentId', name: 'StudentId', defaultContent: '', render: renderDelete, orderable: false, width: "8%" }
        ],


        language: {
            emptyTable: 'No student found, please <b> Add Student </b> to show detail', // no data
            infoEmpty: 'No records avaiable',
            zeroRecords: 'Humm.... No result founded'
        }
    });

    // Add event listener for opening and closing details
    $('#StudentTable tbody').on('click', 'td.dt-control', function () {

        var tr = $(this).closest('tr');
        var row = datatable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });

    $('#StudentTable tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });

    $('#StudentTable tbody').on('mouseover', 'tr', function () {
        var tr = $(this);
        var row = datatable.row(tr).data();
        if (row) tr.attr('title', row.extraInfor);
    });
})
