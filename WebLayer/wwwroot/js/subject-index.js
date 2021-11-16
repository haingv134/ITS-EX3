/*
	iinitialization:
	The ajax request will resend each time ajasResponseStatus changed
*/

var subjectResponseStatus = {}, skip = 0, take = 6;

ajaxResponseStatus = new Proxy(subjectResponseStatus, {
	set: () => {
		$.ajax({
			type: 'POST',
			url: urlListSubject(),
			data: 'skip=' + skip + '&take=' + take,
			success: function (response) {
				loadList(response);
			}
		});
	}
});

callAjaxRequest(1);

function callAjaxRequest(index) {
	skip = (index - 1) * take;
	ajaxResponseStatus.change = 'start';
}

function loadList(response) {
	var parentNode = document.querySelector('#parentNode');
	parentNode.innerHTML = '';

	if (response.totalRecords == 0) {
		alertError();
	}
	else {
		// create subject information and action element
		for (let index = 0; index < response.data.length; index++) {
			const res = response.data[index];

			let rowLeft = createHTMLElement('div', '', 'list-group-item', 'col', 'col-10', 'border-0');
			let checkBox = createHTMLElement('input', '', 'form-check-input');
			let span = createHTMLElement('span', res.name, 'fw-2', 'ms-2');
			let rowRight = createHTMLElement('div', '', 'list-group-item', 'col', 'col-2', 'border-0', 'text-end')
			let deleteAction = createHTMLElement('span', 'Delete', 'badge', 'bg-danger', 'btn', 'rounded-pill');
			let updateAction = createHTMLElement('span', 'Update', 'badge', 'bg-primary', 'btn', 'rounded-pill');

			checkBox.setAttribute('type', 'checkbox');
			checkBox.setAttribute('value', String(index));
			deleteAction.setAttribute('onclick', 'DoAction("' + urlDeleteSubject(res.subjectId) + '")');
			updateAction.setAttribute('onclick', 'OpenPopup("' + urlEditSubject(res.subjectId) + '")');

			rowLeft.appendChild(checkBox);
			rowLeft.appendChild(span);		
			rowRight.appendChild(deleteAction);
			rowRight.appendChild(updateAction);

			parentNode.appendChild(rowLeft);
			parentNode.appendChild(rowRight);
		}
		setPagination(response);
	}
}

// create pagination
function setPagination(response) {

	let pagination = document.querySelector('#subject-pagination');
	pagination.innerHTML = '';

	let totalPages = Math.ceil(response.totalRecords / take);
	let currentPage = Math.ceil(skip / take + 1);

	let previous = createHTMLListItemElement('Previous', 'onclick', 'callAjaxRequest(' + (currentPage - 1) + ')');
	if (currentPage == 1) previous.classList.add('disabled');
	pagination.appendChild(previous);

	for (let index = 1; index <= totalPages; index++) {
		let item = createHTMLElement('li', '', 'page-item');
		let link = createHTMLElement('a', String(index), 'page-link');
		item.appendChild(link);

		link.setAttribute('href', '#');
		link.setAttribute('onclick', 'callAjaxRequest(' + index + ')');
		if (currentPage == index) {
			item.classList.add('active');
		}
		pagination.appendChild(item);
	}
	let next = createHTMLListItemElement('Next', 'onclick', 'callAjaxRequest(' + (currentPage + 1) + ')');
	if (currentPage == totalPages) next.classList.add('disabled');
	pagination.appendChild(next);
}
function alertError() {
	let nontification = createHTMLElement('div', 'Currently no subject here!', 'alert', 'alert-danger')
	document.querySelector('#subject-container').appendChild(nontification);
}
function createHTMLElement(tag, text, ...classes) {
	let element = document.createElement(String(tag));
	element.classList.add(...classes);
	element.innerText = text;
	return element;
}
function createHTMLListItemElement(name, action, execute) {
	let item = createHTMLElement('li', '', 'page-item');
	let link = createHTMLElement('a', name, 'page-link');
	link.setAttribute('href', '#');
	link.setAttribute(action, execute);

	item.appendChild(link);
	return item;
}
document.querySelector('#btnAddSubject').addEventListener('click', () => {
	OpenPopup(urlAddSubject());
})
document.querySelector('#btnDeleteSubject').addEventListener('click', () => {
	OpenPopup(urlAddSubject());
})