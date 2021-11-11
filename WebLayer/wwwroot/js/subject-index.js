$('#btnAddSubject').click(function () {
	OpenPopup(urlAddSubject());
})

var subjectList = ajaxLoadSubject(0, 6);

function ajaxLoadSubject(skip, take) {

	$.ajax({
		type: 'POST',
		url: urlListSubject(),
		data: 'skip=' + skip + '&take=' + take,
		success: function (response) {
			loadList(response, skip, take);
		}
	});
};
function requestHander(skip, take) {
	ajaxLoadSubject(skip, take);
}

function loadList(response, skip, take) {

	var parentNode = document.querySelector('#parentNode');
	// need only one child element to add list before
	while (parentNode.childElementCount) {
		parentNode.removeChild(parentNode.childNodes[0]);
	}
	var pagination = document.querySelector('#previous-page').parentNode;
	while (pagination.childElementCount > 2) {
		pagination.removeChild(pagination.childNodes[1]);
	}

	// create subject information and action element
	response.data.forEach(element => {
		let row = document.createElement('div');
		row.innerText = element.name;
		row.classList.add('list-group-item', 'col', 'col-10', 'border-0');
		parentNode.appendChild(row);

		let action = document.createElement('div');
		action.classList.add('list-group-item', 'col', 'col-2', 'border-0', 'text-end');

		let spanDelete = document.createElement('span');
		spanDelete.classList.add('badge', 'bg-danger', 'btn', 'rounded-pill');
		spanDelete.setAttribute('onclick', 'DoAction(' + urlDeleteSubject(element.subjectId) + ')');
		spanDelete.innerHTML = 'Delete ';

		let spanUpdate = document.createElement('span');
		spanUpdate.classList.add('badge', 'bg-primary', 'btn', 'rounded-pill');
		spanUpdate.setAttribute('onclick', 'OpenPopup(' + urlEditSubject(element.subjectId) + ')');
		spanUpdate.innerHTML = 'Update';
		action.appendChild(spanDelete);
		action.appendChild(spanUpdate);

		parentNode.appendChild(action);
	});
	let currentPage = Math.ceil(skip / take);
	let totalPages = Math.ceil(response.totalRecords / take);
	setPagination(totalPages, currentPage, take);
	// show error elert if no subject
	if (response.data.length === 0) alertError();
}

// create pagination
function setPagination(totalPages, currentPage, take) {

	var nextPage = document.querySelector('#next-page');
	for (let index = 1; index <= totalPages; index++) {
		// create li with page-item class
		let pageItem = document.createElement('li');
		pageItem.classList.add('page-item');
		let pageLink = document.createElement('a');
		pageLink.classList.add('page-link');
		pageLink.setAttribute('href', '#');
		pageLink.innerText = index;
		pageItem.appendChild(pageLink);
		// handler page clicked		
		let skip = (index - 1) * take;
		//pageItem.addEventListener('click',requestHander(skip, take));
		pageItem.setAttribute('onclick', 'requestHander(' + skip + ',' + take + ')');
		// insert li to ol
		nextPage.parentNode.insertBefore(pageItem, nextPage);
	}
}
function alertError() {
	let al = document.createElement('div');
	al.innerHTML = 'Currently no subject here!';
	al.classList.add('alert', 'alert-danger');
	parentNode.appendChild(al);
}