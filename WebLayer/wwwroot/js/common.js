// store response satatus of ajax each time its completed
var ajaxResponseStatus = false;

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
// convert url param to data
function ArgHandler(url, ...args) {
    let splits = String(url).split('?');
    let dataTransfer;

    if (args.length > 0) {
        dataTransfer = {
            'args': args
        }
    } else {
        dataTransfer = splits.shift();
    }
    alert(dataTransfer);
    
    return dataTransfer;
}

function DoAction(url, ...args) {
    var dataTransfer = ArgHandler(url, args);
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

function AjaxRequest(url, callback, ...args) {
    var dataTransfer = ArgHandler(args);
    $.ajax({
        type: 'POST',
        url: url,
        traditional: true,
        data: dataTransfer,
        success: function (response) {
            callback(response);
        },
    })
}


function HanderAjaxResponse(data) {
    // Call reload ajax request in each page using this
    ajaxResponseStatus.change = data.success;
    if (data.success) {
        notify(data.message, 'success');
    } else if (!data.success) {
        notify(data.message, 'error');
    }
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
            showSwal = true;
            description.style.display = 'none';
            status.style.display = 'none';
        } else if (showSwal) {
            description.style.display = '';
            status.style.display = '';
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

// back-to-top options 
$(document).ready(function () {

    $(window).scroll(function (e) {
        if ($(this).scrollTop() - 200 > 0) {
            $('#back-to-top').slideDown('fast');
        } else {
            $('#back-to-top').slideUp('fast');
        }
    });
    $('#back-to-top').click(function (e) {
        $('html, body').animate({
            scrollTop: 0
        }, 200);
    });
});


$(document).ready(function () {
    var ajaxStartTime, ajaxEndTime;

    $(document).ajaxStart(function () {
        ajaxStartTime = new Date().getTime();
    });

    $(document).ajaxComplete(function () {
        ajaxEndTime = new Date().getTime();
        console.log("Ajax request take: " + (ajaxEndTime - ajaxStartTime) + 'ms');
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