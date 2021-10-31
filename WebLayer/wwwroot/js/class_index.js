var datatable;
$(document).on('resize', function () {
  $.debounce(500, function () {
    datatable.columns.adjust();
  })
})

function renderClass(data, type, row, meta) {
  if (String(data).length !== 0) {
    return '<a href="#" onclick=OpenPopup(' + urlEditClass(row['classId']) + ') style="text-decoration: none;">' + data + ' </a>';
  }
}

function renderPresidentStudent(data, type, row, meta) {
  if (String(data).length !== 0) {
    return '<a href="#" onclick=OpenPopup(' + urlEditStudent(row["persidentId"]) + ') style="text-decoration: none;">' + data + ' </a>';
  }
  return data;
}

function renderSecretaryStudent(data, type, row, meta) {
  if (String(data).length !== 0) {
    return '<a href="#" onclick=OpenPopup(' + urlEditStudent(row["secretaryId"]) + ') style="text-decoration: none;">' + data + ' </a>';
  }
  return data;
}

function renderAction(data, type, row, meta) {
  return '<a class="btn btn-success btn-sm" onclick=OpenPopup(' + urlEditClass(data) + ')> <i class="ti-pencil"></i>  Edit </a> | <a class="btn btn-danger btn-sm" onclick=DoAction(' + urlDeleteClass(data) + ')> <i class="ti-trash"></i> Delete </a> ';
}

$('#Reset').click(function () {
  if ($("#minValue").val().length > 0 && $("#maxValue").val().length > 0 || $("#SearchBar").val().length > 0) {
    $('#minValue, #maxValue, #SearchBar').val('');
    datatable.draw();
  }
})

$('#Search').click(function () {
  if ($('#minValue').val().length > 0 && $('#maxValue').val().length > 0 || $('#SearchBar').val().length > 0) {
    datatable.search($(this).val()).draw();
  }
})

$(document).ready(function () {
  // default option for datatables
  $.extend(true, $.fn.dataTable.defaults, {
    processing: false, // showing 'processing' message while ajax is executing
    serverSide: true,  // handing request on server
    searching: false, //
    ordering: true, // allow order
    paging: true, // phan trang
    filter: false, // enable searching on input search
    stateSave: true, // save state of last table (lost if accessing with orther url or no session)
    autoWidth: true
  });

  datatable = $('#example').DataTable({
    ajax: {
      url: urlIndexClass(),
      type: 'POST',
      'data': function (d) {
        return $.extend({}, d, {
          'quantityType': $('#quanityType').val(),
          'minValue': $('#minValue').val(),
          'maxValue': $('#maxValue').val(),
          'search': {
            'value': $('#SearchBar').val(),
            'regex': false
          }
        })
      }
    },

    columns: [
      { data: 'className', name: 'ClassName', title: 'Class Name', render: renderClass },
      { data: 'persidentName', name: 'PersidentName', title: 'Persident Name', render: renderPresidentStudent },
      { data: 'secretaryName', name: 'SecretaryName', title: 'Secretary Name', render: renderSecretaryStudent },
      { data: 'quantity', name: 'Quantity', title: 'Quantity' },
      { data: 'boyQuantity', name: 'BoyQuantity', title: 'Boy Quantity' },
      { data: 'girlQuantity', name: 'GirlQuantity', title: 'Girl Quantity' },
      { data: 'subjects', name: 'Subjects', title: 'Subjects', orderable: false },
      {
        data: 'classId', name: 'ClassId', title: 'Actions', render: renderAction,
        orderable: false,
        visiable: false,
        width: '200px'
      }
    ],

    order: [[0, 'asc'], [1, 'asc']],
    language: {
      emptyTable: 'No record found, please <b> Add New Class </b> to show detail', // no data
      infoEmpty: 'No records avaiable',
      zeroRecords: 'Humm.... No result founded'
    }
  })
});