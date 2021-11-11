$(document).ready(function () {
    let setRole = document.querySelector('#set-student-role');
    let persident = document.querySelector('#select-persident');
    let secretary = document.querySelector('#select-secretary');
    $("#Add-Class-Student").change(function () {
        let options = document.querySelector('#Add-Class-Student').selectedOptions;
        if (options.length >= 2) {

            persident.innerHTML = "";
            secretary.innerHTML = "";
            setRole.style.display = 'flex';
            for (let index = 0; index < options.length; index++) {
                let opt = document.createElement('option');
                opt.value = options[index].value;
                opt.text = options[index].text;
                let clOpt = opt.cloneNode(true);
                persident.appendChild(opt);
                secretary.appendChild(clOpt);
            }
        } else {
            setRole.style.display = 'none';
        }
    })
    $('#Add-Class-Student').select2({
        dropdownParent: $('.modal'),
        width: '100%',
        theme: "classic"
    });
    $('#Add-Class-Subject').select2({
        dropdownParent: $('.modal'),
        width: '100%',
        theme: "classic"
    });
})