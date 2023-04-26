const showmodalCreate = (PartialView1) => {
	var id = '2';
	var options = { "backdrop": true, keyboard: true };
	$.ajax({
		type: "GET",
		url: PartialView1,
		contentType: "application/json; charset=utf-8",
		data: { "Id": id },
		datatype: "json",
		success: function (data) {
			debugger;
			$('#myModalContent').html(data);
			$('#myModal').modal(options);
			$('#myModal').modal('show');

		},
		error: function () {
			alert("Dynamic content load failed.");
		}
	});
}