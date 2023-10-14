function ajaxFunctiGroup(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.groupName}</option>`
	});
	$('#atttachmentGroupId').empty();
	$('#atttachmentGroupId').append(`<option value="">Select </option>`);
	$('#atttachmentGroupId').append(option);
}

function ajaxFunctiGroupModal(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.groupName}</option>`
	});
	$('#atttachmentGroupIdModal').empty();
	$('#atttachmentGroupIdModal').append(`<option value="">Select </option>`);
	$('#atttachmentGroupIdModal').append(option);
}

function ajaxGetAllAttachmentCategoryByGroupId(response) {
	var options = '<option value="">Select Category</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.categoryName + '</option>';
	});
	$('#atttachmentCategoryId').empty();
	$('#atttachmentCategoryId').append(options);
}

function ajaxFunctiRelation2(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.organizationName}">${item.organizationName}</option>`
	});
	$('#nameOrganization').empty();
	$('#nameOrganization').append(`<option value="">Select </option>`);
	$('#nameOrganization').append(option);
}
function ajaxGetSupervisors(response) {
	var GeEmployeesList = [];
	$.each(response, function (id, option) {
		var obj = new Object();
		obj.key = option.Id;
		obj.value = option.employeeCode;
		obj.designation = option.designation;
		obj.employeeCode = option.employeeCode;
		obj.nameEnglish = option.nameEnglish;
		GeEmployeesList[GeEmployeesList.length] = obj;
	});
	$('#supervisorCode').autocomplete({
		source: GeEmployeesList,
		select: function (event, ui) {
			$("#supervisorCode").val(ui.item.value);
			$("#supervisorDesignation").val(ui.item.designation);
			$("#supervisorName").val(ui.item.nameEnglish);
		}
	});
}

function ajaxGetApprovers(response) {
	var GeEmployeesList = [];
	$.each(response, function (id, option) {
		var obj = new Object();
		obj.key = option.Id;
		obj.value = option.employeeCode;
		obj.designation = option.designation;
		obj.employeeCode = option.employeeCode;
		obj.nameEnglish = option.nameEnglish;
		GeEmployeesList[GeEmployeesList.length] = obj;
	});
	$('#signatoryCode').autocomplete({
		source: GeEmployeesList,
		select: function (event, ui) {
			$("#signatoryCode").val(ui.item.value);
			$("#signatoryDesignation").val(ui.item.designation);
			$("#signatoryName").val(ui.item.nameEnglish);
		}
	});
}

function ajaxGetAllEmployee(response) {
	var GetEmployeeList = [];
	$.each(response, function (id, option) {
		var obj = new Object();
		obj.key = option.Id;
		obj.value = option.nameEnglish + ' (' + option.employeeCode + ')';
		GetEmployeeList[GetEmployeeList.length] = obj;
	});
	$('#employeeName').autocomplete({
		source: GetEmployeeList,
		select: function (event, ui) {
			$("#employeeInfoId").val(ui.item.key);

		}
	});

}

function ajaxGetAllEmployee1(response) {
	var lstPOP = [];
	$.each(response, function (i, option) {
		var obj = new Object();
		obj.key = option.employeeCode;
		obj.value = option.nameEnglish + " - " + option.employeeCode;

		lstPOP[lstPOP.length] = obj;
	});
	$('#approverName1').autocomplete({
		source: lstPOP,
		select: function (event, ui) {
			$("#approverName1").val(ui.item.value);
			$("#approverIds").val(ui.item.key);

		}
	});

}
function ajaxTrainingInstitute(response) {

	var option = "";
	$('#institute9').empty();
	$('#institute9').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.trainingInstituteName}</option>`
	});

	$('#institute9').append(option);


}
function ajaxTrainingCategories(response) {
	var option = "";
	$('#category9').empty();
	$('#category9').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.trainingCategoryName}</option>`
	});

	$('#category9').append(option);
}
function ajaxFunctionOtherqualification(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.name}</option>`
	});
	$('#OtherqualificationHeadId').empty();
	$('#OtherqualificationHeadId').append(`<option value="">Select </option>`);
	$('#OtherqualificationHeadId').append(option);
}
function ajaxFunctionQualifications(response) {
	var option = "";
	$('#qualificationHeadId').empty();
	$('#qualificationHeadId').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.name}</option>`
	});

	$('#qualificationHeadId').append(option);

}
function ajaxFunctionLevel(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.levelofeducationName}</option>`
	});
	$('#levelofeducationId').empty();
	$('#levelofeducationId').append(`<option value="">Select </option>`);
	$('#levelofeducationId').append(option);
}
function ajaxLevelOfEducation(response) {

	var option = "";
	$('#levelOfEducation').empty();
	$('#levelOfEducation').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.levelofeducationName}</option>`
	});

	$('#levelOfEducation').append(option);

}
function ajaxOrganization(response) {
	var option = "";
	$('#organizationId').empty();
	$('#organizationId').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.organizationName}</option>`
	});

	$('#organizationId').append(option);

}
function ajaxResult(response) {
	var options = '<option value="">Select </option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.resultName + '</option>';
	});
	$('#resultId').empty();
	$('#resultId').append(options);
}
function ajaxFunctionDegree(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.degreeName}</option>`
	});
	$('#degreeSubjectId').empty();
	$('#degreeSubjectId').append(`<option value="">Select </option>`);
	$('#degreeSubjectId').append(option);
}
function ajaxDegrees(response) {
	var option = "";
	$('#degreeId').empty();
	$('#degreeId').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.degreeName}</option>`
	});

	$('#degreeId').append(option);

}
function ajaxRelDegreeSubjects(response) {
	var option = "";
	$('#reldegreesubjectId').empty();
	$('#reldegreesubjectId').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.subject.subjectName}</option>`
	});

	$('#reldegreesubjectId').append(option);

}
function ajaxPosts(response) {
	posts = {}
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		posts['"' + item.designationName + '"'] = item.postId;
		options += '<option value="' + item.designationName + '">' + item.designationName + '</option>';
	});
	$('#designation').empty();
	//$('#designation').append(options);
}
function ajaxPNS(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.pNS.Id + '">' + item.pNS.name + '</option>';
	});
	$('#pns').empty();
	$('#pns').append(options);
}
function ajaxDivision(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.divisionName + '</option>';
	});
	$('#presentDivision').empty();
	$('#presentDivision').append(options);
}
function ajaxPermanentDivision(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.divisionName + '</option>';
	});
	$('#permanentDivision').empty();
	$('#permanentDivision').append(options);
}
function ajaxDistrict(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.districtName + '</option>';
	});
	$('#presentDistrict').empty();
	$('#presentDistrict').append(options);
}
function ajaxPermanentDistrict(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.districtName + '</option>';
	});
	$('#permanentDistrict').empty();
	$('#permanentDistrict').append(options);
}
function ajaxThana(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.thanaName + '</option>';
	});
	$('#presentUpazila').empty();
	$('#presentUpazila').append(options);
}
function ajaxPermanentThana(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.thanaName + '</option>';
	});
	$('#permanentUpazila').empty();
	$('#permanentUpazila').append(options);
}
//Nominee
function ajaxNomineeDetail(response) {
	var options = '';
	var count = 1;
	$.each(response, function (i, item) {
		options += '<tr id="record-' + count + '"><td>' + item.nomineeFund.name + '<input type="hidden" name = "fundTypeList[]" value =' + "'" + item.nomineeFundId + "'" + '/></td><td>' + item.persentence + '<input type="hidden" name = "fundValueList[]" value =' + "'" + item.persentence + "'" + '/></td><td><button type="button" class="btn btn-danger btn-sm" onclick="RemoveFromTable(' + count + ')"><i class="fa fa-trash"></i></button></tr>'
		count++;
	});
	$('#fundValueListTable tbody').empty();
	$('#fundValueListTable tbody').append(options);
}
function ajaxFunctiOccupationN(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.occupationName}">${item.occupationName}</option>`
	});
	$('#Nomineeoccupation').empty();
	$('#Nomineeoccupation').append(`<option value="">Select </option>`);
	$('#Nomineeoccupation').append(option);
}
function ajaxFunctiOccupationS(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#Soccupation').empty();
	$('#Soccupation').append(`<option value="">Select </option>`);
	$('#Soccupation').append(option);
}
function ajaxFunctiOccupationC(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#Coccupation').empty();
	$('#Coccupation').append(`<option value="">Select </option>`);
	$('#Coccupation').append(option);
}
function ajaxFunctiRelation(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.relationName}">${item.relationName}</option>`
	});
	$('#Nomineerelation').empty();
	$('#Nomineerelation').append(`<option value="">Select </option>`);
	$('#Nomineerelation').append(option);
}
function ajaxFunctiRelation3(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.relationName}">${item.relationName}</option>`
	});
	$('#relation3').empty();
	$('#relation3').append(`<option value="">Select </option>`);
	$('#relation3').append(option);
}
function ajaxFunctiRelation5(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.relationName}">${item.relationName}</option>`
	});
	$('#relation5').empty();
	$('#relation5').append(`<option value="">Select </option>`);
	$('#relation5').append(option);
}
function ajaxNomineeDistributedPercent(response) {
	var distributed = parseFloat(response);
	$("#remaining").val(100 - distributed);
}
function ajaxFunctiOccupation(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#fatherOccupationId').empty();
	$('#fatherOccupationId').append(`<option value="">Select </option>`);
	$('#fatherOccupationId').append(option);
}
function ajaxFunctiCompany(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.companyName}</option>`
	});
	$('#companyIdL').empty();
	$('#companyIdL').append(`<option value="">Select </option>`);
	$('#companyIdL').append(option);
}

function ajaxFunctiCompany1(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.companyName}</option>`
	});
	$('#companyIdF').empty();
	$('#companyIdF').append(`<option value="">Select </option>`);
	$('#companyIdF').append(option);
}
function ajaxFunctiOccupation1(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#motherOccupationId').empty();
	$('#motherOccupationId').append(`<option value="">Select </option>`);
	$('#motherOccupationId').append(option);
}
function ajaxFunctiOccupation2(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#occupation3').empty();
	$('#occupation3').append(`<option value="">Select </option>`);
	$('#occupation3').append(option);
}
function ajaxFunctijoiningDesignation(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.designationName}">${item.designationName}</option>`
	});
	$('#joiningDesignation').empty();
	$('#joiningDesignation').append(`<option value="">Select </option>`);
	$('#joiningDesignation').append(option);
}
function ajaxFuncLocation(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.branchUnitName}</option>`
	});
	$('#locationId').empty();
	$('#locationId').append(`<option value="">Select </option>`);
	$('#locationId').append(option);
}
function ajaxFunctiFunction(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.branchUnitName}</option>`
	});
	$('#functionInfoId').empty();
	$('#functionInfoId').append(`<option value="">Select </option>`);
	$('#functionInfoId').append(option);
}
function ajaxFunctiDepartment(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.deptName}</option>`
	});
	$('#departmentId').empty();
	$('#departmentId').append(`<option value="">Select </option>`);
	$('#departmentId').append(option);
}
function ajaxFunctiUnit(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.unitName}</option>`
	});
	$('#hrUnitId').empty();
	$('#hrUnitId').append(`<option value="">Select </option>`);
	$('#hrUnitId').append(option);
}
function ajaxFunctiBranch(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.branchName}</option>`
	});
	$('#hrBranchId').empty();
	$('#hrBranchId').append(`<option value="">Select </option>`);
	$('#hrBranchId').append(option);
}
function ajaxFunctiBranchType(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.name}</option>`
	});
	$('#branchCodeB').empty();
	$('#branchCodeB').append(`<option value="">Select </option>`);
	$('#branchCodeB').append(option);
}
function ajaxFunctiProgram(response) {
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.programName}</option>`
	});
	$('#hrProgramId').empty();
	$('#hrProgramId').append(`<option value="">Select </option>`);
	$('#hrProgramId').append(option);
}
function ajaxGetApprovalDetailByApprovalMasterId(response) {

	var ifExistIndex = window.localStorage.getItem("holdIndex1");
	var index = "0";
	var status = "";
	if (ifExistIndex == "null" || ifExistIndex == null) {
		index = $("#tblitemDetails tbody>tr").length;
	}
	else {
		index = ifExistIndex;
		tdRID = $("#tblitemDetails tbody >tr:eq(" + ifExistIndex + ") td:eq(0) .clsId").val();
	}
	$("#tblitemDetails tbody>tr").empty();
	$.each(response, function (key, value) {
		if (value.isDelete == 0) {
			status = "Next";
		}
		else {
			status = "Final"
		}

		index = $("#tblitemDetails tbody>tr").length;
		var dtTable = $("#tblitemDetails");
		var tableBody = dtTable.find('tbody');
		var trHtml = '<tr id=' + index + '>' +
			'<td>' + value.approver.nameEnglish + '<input type="hidden" name="id"class="clsId" value="' + value.Id + '"><input type="hidden" id="approverId' + index + '" name="approverId" class="clsapproverId" value="' + value.approverId + '"/></td>' +
			'<td>' + value.approver.designation + '</td>' +
			'<td>' + value.approver.designation + '</td>' +
			'<td>' + value.status + '<input type="hidden" id="status' + index + '" name="status" class="clsstatus" value="' + value.status + '"/></td>' +
			'<td style="text-align:right;">' + value.sortOrder + '<input type="hidden" id="sortOrder' + index + '" name="sortOrder" class="clssortOrder" value="' + value.sortOrder + '"/></td>' +
			'<td style="text-align:right;">' + status + '<input type="hidden" id="isDelete' + index + '" name="canFinalApprover" class="clsstatus" value="' + value.isDelete + '"/></td>' +

			'<td><a href="javascript:void(0)"style="background-color:red;border-color:red" data-toggle="tooltip" title="Delete" class="btn btn-danger btn-xs" onclick="Delete(' + index + ')"><i class="fa fa-trash"></i></a>&nbsp;&nbsp;' +
			'<a onclick="EditTbl(' + index + "," + value.approverId + ", " + value.sortOrder + ", `" + value.approver.nameEnglish + "`,  `" + value.status + "`, " + value.Id + ',' + value.isDelete + ')" href="javascript:void(0)" title="Edit" class="btn btn-success btn-xs" > <i class="fa fa-edit"></i></a >' +


			'</tr>';

		if (ifExistIndex == null || ifExistIndex == "null") {

			tableBody.append(trHtml);
		}
		else {
			var rowUpdate = $("#tblitemDetails tbody >tr:eq(" + ifExistIndex + ")");
			rowUpdate.replaceWith(trHtml);
			window.localStorage.setItem("holdIndex1", null);
			ifExistIndex = null;
		}
	})

	index = index + 1;

}
function ajaxActivityName(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.name + '</option>';
	});
	$('#activityName').empty();
	$('#activityName').append(options);
}
function ajaxGetInsurancePhotoById(response) {
	var baseUrl = 'http://' + location.host + '/Upload/Photo/' + response.fileName;
	var image = document.getElementById('user_img_Challan');
	image.setAttribute('src', baseUrl);
}