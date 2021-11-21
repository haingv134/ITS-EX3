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
				if (response.totalRecords == 0) {
					alertError(true);
				}
				else {
					LoadTable(response);
					setPagination(response);
					alertError(false);
				}
			}
		});
	}
});

callAjaxRequest(1);

function callAjaxRequest(index) {
	skip = (index - 1) * take;
	ajaxResponseStatus.change = 'start';
}

function LoadTable(response) {
	let table = document.querySelector('#subject-index-list');
	table.innerHTML = '';
	for (let index = 0; index < response.data.length; index++) {
		const res = response.data[index];

		let row = createHTMLElement('tr', '', '');

		let checkBox = createHTMLElement('input', '', 'form-check-input');
		checkBox.setAttribute('type', 'checkbox');
		checkBox.setAttribute('value', String(index));
		row.appendChild(checkBox);

		let text = [res.name, res.subjectCode, res.startTime, res.endTime];
		text.forEach(value => {
			let col = createHTMLElement('td', value, '');
			row.appendChild(col);
		})

		let action = document.createElement('td');
		let deleteAction = createHTMLElement('span', 'Delete', 'badge', 'bg-danger', 'btn', 'rounded-pill');
		let updateAction = createHTMLElement('span', 'Update', 'badge', 'bg-primary', 'btn', 'rounded-pill');
		deleteAction.setAttribute('onclick', 'DoAction("' + urlDeleteSubject(res.subjectId) + '")');
		updateAction.setAttribute('onclick', 'OpenPopup("' + urlEditSubject(res.subjectId) + '")');
		action.appendChild(updateAction);
		action.appendChild(deleteAction);
		row.appendChild(action);		
		table.appendChild(row);
	}
}

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
function alertError(status) {
	let noSubject = document.querySelector('#index-no-subject');
	let table = document.querySelector('#table-subject-index-list');
	let pagination = document.querySelector('#subject-pagination');
	if (status) {
		noSubject.classList.add('alert', 'alert-danger');
		noSubject.innerText = "Currently, No subject here!";
		noSubject.style.display = "block";
		table.style.display = 'none';
		pagination.innerHTML = '';
	} else {
		noSubject.style.display = "none";
		table.style.display = '';

	}
}

function createHTMLElement(tag, text, ...classes) {
	let element = document.createElement(String(tag));
	if (classes[0].length) {
		element.classList.add(...classes);
	}
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