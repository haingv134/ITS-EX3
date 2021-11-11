function OpenPopup(URL, ...args) {
    // call URL and including array data if have
    $.ajax({
        type: 'GET',
        url: URL,
        traditional: true,
        data: { 'args': args }
    }).done(function (response) {
        $('#popup').html(response);
        // if response has model -> show it and processing, if not: push error nontification
        if ($('.modal').length) {
            $('.modal').modal('show');
            var form = $('#popup form');
            if (form.length) {
                $.validator.unobtrusive.parse(form);
            }
            ClosePopup();
        } else {
            $('#popup').hide();
            notify(response, 'error');
        }
    })
}

function ClosePopup() {
    $('.btn-close').on('click', function (e) {
        e.preventDefault();
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success mx-2',
                cancelButton: 'btn btn-danger'
            },
            buttonsStyling: false
        });
        swalWithBootstrapButtons.fire({
            title: 'Are you sure?',
            text: 'You won\'t be able to revert this!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, close it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $('.modal').modal('hide');
            } else {
                $('.modal').modal('show');
            }
        });
    });
}

// add submit handling
function SubmitForm(form) {
    if ($(form).valid()) {
        var formData = new FormData(form);
        $.ajax({
            type: 'POST',
            url: form.action,
            data: formData,

            cache: false,
            contentType: false,
            processData: false,

            success: function (data) {
                HanderAjaxResponse(data);
            },
            error: function (data) {
                notify(data.message, 'error');
            }
        })
    }
    return false;
}

function DoAction(url, ...args) {
    var splits = url.split('?');
    var dataTransfer;
    if (args.length > 0){
        dataTransfer = {
            'args' : args
        }
    } else {
        dataTransfer = splits.shift();   
    }
    $.ajax({
        type: 'POST',
        url: url,
        traditional: true,
        data: dataTransfer,
        success: function (data) {
            HanderAjaxResponse(data);
        },
        error: function (data) {
            notify(data.message, 'error');
        }
    })
}

function HanderAjaxResponse(data) {
    if (data.success) {
        // if form is proccessing to datatable
        if (typeof (datatable) != 'undefined') {
            datatable.ajax.reload();
        }
        // if form inside subject part
        if (typeof (subjectList) != 'undefined') {
            ajaxLoadSubject(0, 6);
        }
        notify(data.message, 'success');
    } else {
        notify(data.message, 'error');
    }
    // if form inside a popup modal
    if ($('.modal').length) {
        $('.modal').modal('hide');
    }
}

// intetnet status (online or offline)
$(document).ready(function () {
    var showSwal = true;
    window.setInterval(function () {
        var description = document.querySelector('#status-description');
        var status = document.querySelector('#internet-status');
        if (window.navigator.onLine) {
            status.style.backgroundColor = 'green';
            status.setAttribute('title', 'Online');
            description.innerText = 'Connnected';
            description.style.color = 'green';
            showSwal = true;
            //Swal.Close();
        } else if (showSwal) {
            status.style.backgroundColor = 'red';
            status.setAttribute('title', 'Offline');
            description.innerText = 'Your internet was disconnected';
            description.style.color = 'red';
            Swal.fire({
                icon: 'error',
                title: 'Opps',
                text: description.innerText,
                showConfirmButton: true
            });
            showSwal = false;
        }
    }, 1000);
})

$(document).ready(function () {

    $(window).scroll(function (e) {
        if ($(this).scrollTop() - 200 > 0) {
            $('#back-to-top').slideDown('fast');
        } else {
            $('#back-to-top').slideUp('fast');
        }
    });
});

$('#back-to-top').click(function (e) {
    $('html, body').animate({
        scrollTop: 0
    }, 200);
});

function notify(msg, typeMsg) {

    var panel = document.querySelector("#nontification-container");
    panel.style.display = 'flex';
    panel.style.backgroundColor = (typeMsg === 'success') ? "rgba(32, 151, 32, 0.349)" : "rgba(255, 0, 0, 0.1)";
    panel.style.color = (typeMsg === 'success') ? "green" : "red";
    panel.innerHTML = msg;

    setTimeout(function () {
        panel.style.display = 'none';
    }, 3000);
}
var ajaxStartTime, ajaxEndTime;
$(document).ready(function () {    
    var loadingStatus = true;
    $(document).ajaxStart(function () {
        loadingStatus = true;
        loadingInterval = setInterval(function () {
            loadingAction();
            if (!loadingStatus) clearInterval(loadingInterval);
        }, 250);
        ajaxStartTime = new Date().getTime();
    });

    $(document).ajaxComplete(function () {
        $('#loading-1, #loading-2, #loading-3').css('display', 'none');
        loadingStatus = false;
        ajaxEndTime = new Date().getTime();        
        document.querySelector('#ajax-time-execute').innerText = 'request took ' + (ajaxEndTime - ajaxStartTime) + 'ms';
    });
})


function loadingAction() {
    let timer = 0;
    //let status = hide ? 'inline-block' : 'none';
    for (let index = 1; index <= 3; index++) {
        setTimeout(function () {
            $('#loading-' + index).css('display', 'inline-block');
        }, timer);
        setTimeout(function () {
            $('#loading-' + index).css('display', 'none');
        }, 200);
        timer += 50;
    }
}

// for searching on navbar
$(document).on('keydown', function (event) {
    if (event.shiftKey && (event.key === 'F' || event.key === 'f')) {
        $('#btnSearch').trigger('select');
    }
})