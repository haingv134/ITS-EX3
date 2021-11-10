$(document).ready(function () {

    // select2
    $('#StudentListId').select2({
        dropdownParent: $('.modal'),
        width: '100%',
        theme: "classic"
    });
    $('#SubjectListId').select2({
        dropdownParent: $('.modal'),
        width: '100%',
        theme: "classic"
    });

    let setRole = document.querySelector('#set-student-role');
    let persident = document.querySelector('#select-persident');
    let secretary = document.querySelector('#select-secretary');

    StudentRoleHandler();

    $("#StudentListId").change(StudentRoleHandler);
    
    function StudentRoleHandler(){
        let options = document.querySelector('#StudentListId').selectedOptions;
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
    }        
})