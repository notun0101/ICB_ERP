function PostData(formId, area, controller, action) {
	var serializeData = $('#' + formId).serialize();
	var message = '';
	$.ajax({
		url: '/' + area + '/'+controller + '/' + action,
		method: "POST",
		data: serializeData,
		contentType: "application/json",
		success: function (data) {
			console.log(JSON.stringify(data));
			message = 'success';
		},
		error: function (errMsg) {
			console.log(JSON.stringify(errMsg));
			message = 'failed';
		}
	});
	return message;
}

function DeleteData(id, area, controller, action) {
	$.ajax({
		url: '/' + area + '/' + controller + '/' + action + '/',
		type: 'GET',
		dataType: 'json', // added data type
		success: function (res) {
			console.log(res);
			alert(res);
		}
	});
}