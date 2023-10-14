function readURL1Spouce(input) {
	if (input.files && input.files[0]) {
		var filerdr = new FileReader();
		filerdr.onload = function (e) {

			var srcUrl = e.target.result;
			if (srcUrl.indexOf("application/pdf") > 0) {
				$('#user_img1_Spouce').attr('src', "/EmpAttachment/pdfIcon.png");
				$('#userFileSpouce').val(e.target.result);
			}
			else {
				$('#user_img1_Spouce').attr('src', e.target.result);
			}

		}
		filerdr.readAsDataURL(input.files[0]);
	}
}
function readURL1(input) {
	if (input.files && input.files[0]) {
		var filerdr = new FileReader();
		filerdr.onload = function (e) {

			var srcUrl = e.target.result;
			if (srcUrl.indexOf("application/pdf") > 0) {
				$('#user_img1_children').attr('src', "/EmpAttachment/pdfIcon.png");
				$('#userFileChildren').val(e.target.result);
			}
			else {
				$('#user_img1_children').attr('src', e.target.result);
			}

		}
		filerdr.readAsDataURL(input.files[0]);
	}
}




//Remove Functions
//address
function CopyAddress() {
	if ($('#copyAs').is(":checked")) {
		$('#permanentRoadNo').val($('#presentRoadNo').val());
		$('#permanentHouseVillage').val($('#presentHouseVillage').val());
		$('#permanentBlockSector').val($('#presentBlockSector').val());
		$('#permanentPostCode').val($('#presentPostCode').val());
		$('#permanentPostOffice').val($('#presentPostOffice').val());
		$('#permanentDivision').val($('#presentDivision').val());

		var divId = $('#presentDivision').val();
		Common.Ajax('GET', '/global/api/districts/' + divId, [], 'json', ajaxPermanentDistrict, false);
		$('#permanentDistrict').val($('#presentDistrict').val());

		var disId = $('#presentDistrict').val();
		Common.Ajax('GET', '/global/api/thanas/' + disId, [], 'json', ajaxPermanentThana, false);
		$('#permanentUpazila').val($('#presentUpazila').val());

		$('#permanentUnion').val($('#presentUnion').val());
	}
	else {
		$('#permanentRoadNo').val($('').val());
		$('#permanentHouseVillage').val($('').val());
		$('#permanentBlockSector').val($('').val());
		$('#permanentPostCode').val($('').val());
		$('#permanentPostOffice').val($('').val());
		$('#permanentDivision').val($('').val());

		var divId = $('#presentDivision').val();
		Common.Ajax('GET', '/global/api/districts/' + divId, [], 'json', ajaxPermanentDistrict, false);
		$('#permanentDistrict').val($('').val());

		var disId = $('#presentDistrict').val();
		Common.Ajax('GET', '/global/api/thanas/' + disId, [], 'json', ajaxPermanentThana, false);
		$('#permanentUpazila').val($('').val());

		$('#permanentUnion').val($('').val());
	}
};
function AddFundValue() {
	if ($("#fundType").val() == "") {
		swal('Attention.!!', 'Please select fund type.', 'warning');
		return false;
	}
	if ($("#value").val() == "" || parseFloat($("#value").val()) <= 0 || isNaN($("#value").val())) {
		swal('Attention.!!', 'Please enter valid amount.', 'warning');
		$("#value").val('');
		return false;
	}
	if (parseFloat($("#value").val()) > parseFloat($("#remaining").val())) {
		swal('Attention.!!', 'Please enter valid amount.', 'warning');
		$("#value").val('');
		return false;
	}

	var options = '<tr id="record-' + index + '"><td>' + $("#fundType option:selected").html() + '<input type="hidden" name = "fundTypeList[]" value =' + "'" + $("#fundType").val() + "'" + '/></td><td>' + $("#value").val() + '<input type="hidden" name = "fundValueList[]" value =' + "'" + $("#value").val() + "'" + '/></td><td><button type="button" class="btn btn-danger btn-sm" onclick="RemoveFromTable(' + index + ')"><i class="fa fa-trash"></i></button></tr>'

	$('#fundValueListTable tbody').append(options);

	index = index + 1;
	$("#fundType").val('');
	$("#value").val('');
}
