var subjectList = ajaxLoadSubject();

function ajaxLoadSubject() {
	$.ajax({
		type: 'POST',
		url: urlListSubject(),
		data: null,
		success: function (response) {
			loadList(response.data)
		}
	});
	return true;
};
function loadList(data) {
	var parentNode = document.querySelector('#parentNode');
	// need only one child element to add list before
	while (parentNode.childElementCount) {
		parentNode.removeChild(parentNode.childNodes[0]);
	}

	data.forEach(element => {
		let row = document.createElement('div');
		row.innerText = element.name;
		row.classList.add('list-group-item','col','col-10', 'border-0');
		parentNode.appendChild(row);

		let action = document.createElement('div');
		action.classList.add('list-group-item','col','col-2', 'border-0', 'text-end');

		let spanDelete = document.createElement('span');
		spanDelete.classList.add('badge', 'bg-danger', 'btn', 'rounded-pill');
		spanDelete.setAttribute('onclick', 'DoAction(' + urlDeleteSubject(element.subjectId) +')');
		spanDelete.innerHTML = 'Delete';

		let spanUpdate = document.createElement('span');
		spanUpdate.classList.add('badge', 'bg-primary', 'btn', 'rounded-pill');
		spanUpdate.setAttribute('onclick', 'DoAction(' + urlEditSubject(element.subjectId) +')');
		spanUpdate.innerHTML = 'Update';
		action.appendChild(spanDelete);
		action.appendChild(spanUpdate);
		
		parentNode.appendChild(action);
	});
	// show error elert if no subject
	if (data.length === 0){
		let al = document.createElement('div');
		al.innerHTML = 'Currently no subject here!';
		al.classList.add('alert', 'alert-danger');
		parentNode.appendChild(al);
	}
}