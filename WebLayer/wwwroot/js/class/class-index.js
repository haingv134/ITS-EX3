var datatable;

$(document).ready(function () {
  window.addEventListener('resize', function (e) {
    datatable.draw();
  }, true)
})

$(document).ready(function () {
  $('#btnAddClass').click(function () {
    OpenPopup(urlAddClass());
  })
  $('#btnAddSubjectToClass').click(function () {
    let rowData = datatable.rows({ selected: true }).data();
    let classIdData = [];
    for (let index = 0; index < rowData.length; index++) {
      classIdData[index] = rowData[index].classId;
    };
    if (classIdData.length > 0) {
      OpenPopup(urlRegisterSubject(), classIdData);
    }
    else {
      notify('Please select class first', 'error');
    }
  })
})
function renderClass(data, type, row, meta) {
  if (String(data).length !== 0) {
    return '<a href="#" onclick=OpenPopup("' + urlEditClass(row['classId']) + '") style="text-decoration: none;">' + data + ' </a>';
  }
}

function renderPresidentStudent(data, type, row, meta) {
  if (String(data).length !== 0) {
    return '<a href="#" onclick=OpenPopup("' + urlEditStudent(row["persidentId"]) + '") style="text-decoration: none;">' + data + ' </a>';
  }
  return data;
}

function renderSecretaryStudent(data, type, row, meta) {
  if (String(data).length !== 0) {
    return '<a href="#" onclick=OpenPopup("' + urlEditStudent(row["secretaryId"]) + '") style="text-decoration: none;">' + data + ' </a>';
  }
  return data;
}

function renderAction(data, type, row, meta) {
  return '<a class="btn btn-success btn-sm" onclick=OpenPopup("' + urlEditClass(data) + '")> <i class="ti-pencil"></i>  Edit </a> | <a class="btn btn-danger btn-sm" onclick=DoAction("' + urlDeleteClass(data) + '")> <i class="ti-trash"></i> Delete </a> ';
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

  // reload datatable after ajax request completed
  let datatableAjaxResponse = {};
  ajaxResponseStatus = new Proxy(datatableAjaxResponse, {
    set: () => {
      datatable.ajax.reload();
    }
  })

  // default option for datatables
  $.extend(true, $.fn.dataTable.defaults, {
    processing: false, // showing 'processing' message while ajax is executing
    serverSide: true,  
    searching: false, 
    ordering: true,
    paging: true, 
    filter: false, // enable searching on input search
    stateSave: true, // save state of last table (lost if accessing with orther url or no session)
    autoWidth: false,
    // scrollY: "200px",
    // crollCollapse: true
    pagingType: "full_numbers",
    responsive: true
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
    columnDefs: [
      { orderable: false, className: 'select-checkbox', targets: 0 },
      { width: "10%", targets: [1, 2, 3, 4] }
    ],

    select: {
      style: 'os',
      selector: 'td:first-child'
    },
    columns: [
      { data: 'null', "defaultContent": "", width: '5%' },
      { data: 'className', name: 'ClassName', title: 'Class', render: renderClass },
      { data: 'quantity', name: 'Quantity', title: 'Quantity' },
      { data: 'boyQuantity', name: 'BoyQuantity', title: 'Boy Quantity' },
      { data: 'girlQuantity', name: 'GirlQuantity', title: 'Girl Quantity' },
      {
        data: 'classId', name: 'ClassId', title: 'Actions', render: renderAction,
        orderable: false,
        visiable: false,
        width: '15%'
      }
    ],

    order: [[1, 'asc']],
    language: {
      emptyTable: 'No record found, please <b> Add New Class </b> to show detail', // no data
      infoEmpty: 'No records avaiable',
      zeroRecords: 'Humm.... No result founded'
    }
  });

  var classIdData = [];
  // delete many class when select row in datatable
  datatable.on('deselect', DoRowSelectChange);
  datatable.on('select', DoRowSelectChange);

  function DoRowSelectChange(e, dt, type, indexes) {
    if (type === 'row') {
      let rowData = datatable.rows({ selected: true }).data();
      if (rowData.length >= 2) {
        let btnDelete = document.querySelector('#btnDelete');
        btnDelete.removeEventListener('click', DoRmoveRange);
        btnDelete.style.display = 'inline-block';
        classIdData = [];
        for (let index = 0; index < rowData.length; index++) {
          classIdData[index] = rowData[index].classId;
        };
        btnDelete.addEventListener('click', DoRmoveRange);
      } else {
        btnDelete.style.display = 'none';
      }
    }
  }
  function DoRmoveRange() {
    DoAction(urlDeleteClassWithRange(), classIdData);
    btnDelete.style.display = 'none';
  }
});