$(document).ready(function () {
	console.log("Formview.js Loaded 1")

	$('#institute').on('change', function () {
		if ($(this).val() == 0) {
			$('#instituteNameBox').show();
		}
		else {
			$('#instituteNameBox').hide();
		}
	})
})

//Common Remove Function
//Common Remove Function
//Common Remove Function
function RemoveData(Id, employeeId, action) {
	swal({
		title: 'Are you sure?', text: 'Do you want to delete this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#3085d6', cancelButtonColor: '#d33', confirmButtonText: 'Yes, delete it!'
	}).then(function () {
		$.ajax({
			url: "/HRPMSEmployee/formview/" + action + "?id=" + Id + "&empId=" + employeeId,
			type: 'GET',
			dataType: 'json',
			success: function (res) {
				swal('', 'Deleted Successfully!', 'success').then(function () {
					if (res == 'tax') {
						LoadTaxInfo(employeeId);
					}
					else if (res == 'computerLit') {
						LoadComputer(employeeId);
					}
					else if (res == 'membership') {
						LoadMembership(employeeId);
					}
                    else if (res == 'extracurricular') {
						LoadExtraCurricular(employeeId);
					}
                    else if (res == 'freedom') {
						LoadFreedomFighter(employeeId);
					}
                    else if (res == 'ref') {
						LoadReference(employeeId);
					}
                    else if (res == 'attatch') {
						LoadAttatchments(employeeId);
                    }
                    else if (res == 'travel') {
						LoadTravelInfo(employeeId);
                    }

                    else if (res == 'passport') {
                        LoadPassportInfo(employeeId);
                    }
                    else if (res == 'license') {
                        LoadLicensInfo(employeeId);
                    }

                    else if (res == 'IeltsInfos') {
                        LoadIeltsInfo(employeeId);
                    }

                    else if (res == 'language') {
                        LoadLanguageInfo(employeeId);
                    }
                    else if (res == 'emergency') {
                        LoadEmergencyContactInfo(employeeId);
                    }
                    else if (res == 'Acc') {
                        LoadAccountsInfo(employeeId);
                    }

                    else if (res == 'displinary') {
                        LoadDisciplinaryInfo(employeeId);
                    }
                    else if (res == 'award') {
                        LoadAwardInfo(employeeId);
                    }

                    else if (res == 'promotion') {
                        LoadPromotionInfo(employeeId);
                    }

                    else if (res == 'training') {
                        LoadTrainingInfo(employeeId);
                    }

                    else if (res == 'BankingDp') {
                        LoadBankingDiplomaInfo(employeeId);
                    }
                    else if (res == 'cer') {
                        LoadCertificationInfo(employeeId);
                    }
                           else if (res == 'edu') {
                        LoadEducationalQualification(employeeId);
                    }
                    else if (res == 'nominee') {
                        LoadNomineeInfo(employeeId);
                    }
                    else if (res == 'upchild') {
                        LoadChildrenInfo(employeeId);
                    }
                    else if (res == 'spouse') {
                        LoadSpouseInfo(employeeId);
                    }

					else {

					}
				})
			}
		});
	});
}






//Load Functions

function LoadTaxInfo(employeeId) {
	Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllTaxByEmployeeId?empId=' + employeeId, [], 'json', ajaxLoadTaxInfo);
}

function ajaxLoadTaxInfo(response) {
	var option = '';
	$.each(response, function (i, item) {
		option += `<tr>
						<td>${item.taxZone}</td>
						<td>${item.taxCircle}</td>
						<td>${item.eTin}</td>
						<td>
							<a class="btn btn-success" onclick="EditTax(${item.Id},'${item.employeeId}','${item.taxZone}','${item.taxCircle}', '${item.eTin}')" href="#heading35"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteTAX')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
	})

	$('#TaxTable tbody').empty();
	$('#TaxTable tbody').append(option);

	$('#taxZone').val('')
	$('#taxID').val(0)
	$('#taxCircle').val('')
	$('#etinTax').val('')
}

function LoadComputer(employeeId) {
	Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllComputerLiteracyByEmployeeId?empId=' + employeeId, [], 'json', ajaxLoadComputerLiteracy);
}
function ajaxLoadComputerLiteracy(response) {
	var option = '';
	$.each(response, function (i, item) {
		option += `<tr>
							<td>${item.subject}</td>
							<td>${item.competencyLevel}</td>
							<td>${item.training}</td>
							<td>${item.diploma == null ? '' : item.diploma}</td>
							<td>${item.remarks == null ? '' : item.remarks}</td>
							<td>
								<a class="btn btn-success" onclick="EditComputer(${item.Id}, '${item.subject}','${item.competencyLevel}','${item.training}','${item.diploma}','${item.remarks}')" href="#heading33"><i class="fa fa-edit"></i></a>&nbsp;&nbsp;
								<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteComputerJson')"><i class="fa fa-trash-alt"></i></a>
							</td>
						</tr>`
	})
	$('#relDegreeSubjectTable tbody').empty();
	$('#relDegreeSubjectTable tbody').append(option);

	$('#subjectIdlit').val(2)
	$('#LiteracyId').val(0)
	$('#competencyLevel').val('')
	$('#traininglt').val('')
	$('#diplomalt').val('')
	$('#remarkslt').val('')
}

function LoadMembership(employeeId) {
	Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllMembership?empId=' + employeeId, [], 'json', ajaxLoadMembership);
}
function ajaxLoadMembership(response) {
	var option = '';
	$.each(response, function (i, item) {
		option += `<tr>
						<td>${item.membership?.organizationName}</td>
						<td>${item.membershipNo}</td>
						<td>${item.remarks}</td>
						<td>
							<a class="btn btn-success" onclick="EditMembership(${item.Id}, '${item.employeeId}','${item.membershipId}' , '${item.remarks}', '${item.membershipNo}')" href="#heading18"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteMembership')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
	})
	$('#membershipTable tbody').empty();
	$('#membershipTable tbody').append(option);

	$('#nameOrganization').val('')
	$('#employeeMembershipID').val(0)
	$('#membershipNo').val('')
	$('#remarksmm').val('')
}


function LoadMembership(employeeId) {
	Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllMembership?empId=' + employeeId, [], 'json', ajaxLoadMembership);
}
function ajaxLoadMembership(response) {
	var option = '';
	$.each(response, function (i, item) {
		option += `<tr>
						<td>${item.membership?.organizationName}</td>
						<td>${item.membershipNo}</td>
						<td>${item.remarks}</td>
						<td>
							<a class="btn btn-success" onclick="EditMembership(${item.Id}, '${item.employeeId}','${item.membershipId}' , '${item.remarks}', '${item.membershipNo}')" href="#heading18"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteMembership')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
	})
	$('#membershipTable tbody').empty();
	$('#membershipTable tbody').append(option);

	$('#nameOrganization').val('')
	$('#employeeMembershipID').val(0)
	$('#membershipNo').val('')
	$('#remarksmm').val('')
}


function LoadExtraCurricular(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllExtraCurricular?empId=' + employeeId, [], 'json', ajaxLoadExtraCurricular);
}
function ajaxLoadExtraCurricular(response) {
	var option = '';
	$.each(response, function (i, item) {
		console.log(item)
		var skillLevel = item.skillLevel;
		var description = item.description;
		if (skillLevel == null || skillLevel == '') {
			skillLevel = '';
		}
		if (description == null || description == '') {
			description = '';
		}
        option += `<tr>
						<td>${item.extraCurricularType?.name}</td>
						<td>${skillLevel}</td>
						<td>${description}</td>
						<td>
							<a class="btn btn-success" onclick="EditExtraCurricular(${item.Id}, '${item.skillLevel}', '${item.skillType}','${item.description}','${item.extraCurricularTypeId}')" href="#heading39"><i class="fa fa-edit"></i></a>
                            <a class="btn btn-danger" href ="javascript:void(0)" onclick ="RemoveData(${item.Id} ,${item.employeeId}, 'DeleteExtraCurricular')"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#EmployeeExtraCurricular tbody').empty();
    $('#EmployeeExtraCurricular tbody').append(option);

    $('#ExtraCurricularId').val(0)
    $('#extraCurricularTypeId').val('')
    $('#skillLevel').val('')
    $('#skillType').val('')
    $('#description').val('')
}


function LoadFreedomFighter(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllFreedomFighter?empId=' + employeeId, [], 'json', ajaxLoadFreedomFighter);
}
function ajaxLoadFreedomFighter(response) {
    console.log(response);
    var option = '';
    $.each(response, function (i, item) {
        option += `<tr>
						<td>${item.number}</td>
						<td>${item.sectorNo}</td>
						<td>${item.owner}</td>
						<td>${item.remarks}</td>
						<td>
							<a class="btn btn-success" onclick="EditFreedom('${item.number}',${item.Id},'${item.sectorNo}','${item.owner}','${item.remarks}')" href="#heading23"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeID},'DeleteFreedom')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#ffTable tbody').empty();
    $('#ffTable tbody').append(option);

    $('#FFID').val(0)
    $('#ffNo').val('')
    $('#sectorNo').val('')
    $('#owner').val('')
    $('#remarkff').val('')
}



function LoadReference(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllReference?empId=' + employeeId, [], 'json', ajaxLoadReference);
}
function ajaxLoadReference(response) {
   // console.log(response);
    var option = '';
    $.each(response, function (i, item) {
        option += `<tr>
						<td>${item.name}</td>
						<td>${item.organization}</td>
						<td>${item.designation}</td>
						<td>${item.email}</td>
						<td>${item.contact}</td>
						<td>
							<a class="btn btn-success" onclick="Editreference(${item.Id}, '${item.name}', '${item.organization}', '${item.designation}','${item.email}','${item.contact}')" href="#heading24"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id}, ${item.employeeID}, 'DeleteReference')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#referenceTable tbody').empty();
    $('#referenceTable tbody').append(option);

    $('#refIDR').val(0)
    $('#refNameR').val('')
    $('#refDesignationR').val('')
    $('#refOrganizationR').val('')
    $('#refContactR').val('')
    $('#refEmailR').val('')
}



function LoadTravelInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllTravel?empId=' + employeeId, [], 'json', ajaxLoadTravelInfo,false);
}
function ajaxLoadTravelInfo(response) {
    console.log(response);
    var option = '';
    var z = 1;
    //var goDates = formatDatefix(response.item?.goDate);

    $.each(response, function (i, item) {
       
        option += `<tr>

						<td>${z}</td>
						<td>${item.location}</td>
						<td>${item.titleName}</td>
						<td>${item.sponsoringAgency}</td>
						<td>${formatDatefix(item?.goDate)}</td>
						
                         <td> <a href="/EmpImages/${item.file}" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a></td>

						<td>
							<a class="btn btn-success" onclick="EditTravel(${item.Id}, ${item.travelPurposeId}, '${item.titleName}', '${item.location}','${item.countryId}','${item.sponsoringAgency}','${formatDatefix(item.startDate)}','${formatDatefix(item.endDate)}','${item.goNumber}','${formatDatefix(item.goDate)}','${item.remarks}','${item.accountCode}','${item.hrProgramId}','${formatDatefix(item.leaveStartDate)}','${formatDatefix(item.leaveEndDate)}','${item.purpose}','${item.titleOfFile}','${item.file}')" href="#heading17"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id}, ${item.employeeId}, 'DeleteTravel')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
        z++;
    })
    $('#travelTable tbody').empty();
    $('#travelTable tbody').append(option);

    $('#type').val(0)
    $('#Purpose1').val('')
    $('#hrProgramId').val('')
    $('#location').val('')
    $('#sponsoringAgencytr').val('')
    $('#startDate').val('')
    $('#leaveEndDate').val('')
    $('#goDatetr').val('')
    $('#titleOfFile').val('')

    $('#titleName').val('')
    $('#accountCode').val('')
    $('#country').val('')
    $('#goNumbertr').val('')
    $('#endDate').val('')
    $('#leaveStartDate').val('')
    $('#remarkstr').val('')
    $('#imagePathtravel').val('')
}



function LoadAttatchments(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllAttatchments?empId=' + employeeId, [], 'json', ajaxLoadAttatchments,false);
}
function ajaxLoadAttatchments(response) {
    //console.log(response);
    var option = '';
    //var attachDate = formatDatefix(response.item?.attachmentDate);

    $.each(response, function (i, item) {
        option += `<tr>
						<td>${item?.atttachmentCategory?.atttachmentGroup?.groupName}</td>
						<td>${item?.atttachmentCategory?.categoryName}</td>
						<td>${item?.fileTitle}</td>
						<td>${formatDatefix(item?.attachmentDate)}</td>
					    <td> <a href="/EmpImages/${item?.fileUrl}" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a></td>
						<td>
							 <a class="btn btn-success" onclick="EditAttachment(${item.Id},  '${item.atttachmentCategory?.Id}','${item.atttachmentCategoryId}','${item.fileTitle}', '${item.fileUrl}', '${formatDatefix(item?.attachmentDate)}', '${item.remarks}')" href="#heading28"><i class="fa fa-edit"></i></a>
                             <a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteAttachment')"  href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#attachmentTable tbody').empty();
    $('#attachmentTable tbody').append(option);

    $('#atttachmentGroupId').val('')
    $('#atttachmentCategoryId').val('')
    $('#fileTitle').val('')
    $('#attachmentDate').val('')
    $('#remarksat').val('')
    $('#user_imgAttachment').val()
}



function LoadSocialMedia(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetSocialMedia?empId=' + employeeId, [], 'json', ajaxLoadSocialMedia);
}
function ajaxLoadSocialMedia(response) {
    console.log(response);
    
    $('#skypeId').val(response.skypeId)
    $('#fbId').val(response.facebookId)
    $('#linkedId').val(response.linkedinId)
    $('#twitterId').val(response.twitterId)
    $('#intaId').val(response.instagramId)
    $('#WhatsAppId').val(response.whatsappId)
}


function LoadPassportInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllPassport?empId=' + employeeId, [], 'json', ajaxLoadPassportInfo,false);
}
function ajaxLoadPassportInfo(response) {
    var option = '';
    //var dateIssue = formatDatefix(response.item?.dateOfIssue);
    //var dateExpair = formatDatefix(response.item?.dateOfExpair);
    $.each(response, function (i, item) {
        option += `<tr>
						<td>${item.nameInPassport}</td>
						<td>${item.passportNumber}</td>
						<td>${item.placeOfIssue}</td>
						<td>${formatDatefix(item?.dateOfIssue)}</td>
						<td>${formatDatefix(item?.dateOfExpair)}</td>
						<td> <a href="/EmpImages/${item.attachmentUrl}" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a></td>
						<td>
							<a class="btn btn-success" onclick="EditPassport(${item.Id}, '${item.nameInPassport}','${item.passportNumber}' , '${item.placeOfIssue}', '${formatDatefix(item.dateOfIssue)}', '${formatDatefix(item.dateOfExpair)}', '${item.attachmentUrl}')" href="#heading16"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeletePassport')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#passportTable tbody').empty();
    $('#passportTable tbody').append(option);

    $('#nameInPassport').val('')
    $('#passPortNumber').val('')
    $('#dateOfIssuepp').val('')
    $('#dateOfExpairpp').val('')
    $('#imagePathPassport').val('')
    $('#placepp').val('')
}


function LoadLicensInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllLicense?empId=' + employeeId, [], 'json', ajaxLoadLicensInfo,false);
}
function ajaxLoadLicensInfo(response) {
    var option = '';

    //var dateIssue = formatDatefix(response.item?.dateOfIssue);
    //var dateExpair = formatDatefix(response.item?.dateOfExpair);

    $.each(response, function (i, item) {
        option += `<tr>
                                              
						<td>${item.licenseNumber}</td>
						<td>${item.placeOfIssue}</td>
						<td>${item.category}</td>
						<td>${formatDatefix(item?.dateOfIssue)}</td>
						<td>${formatDatefix(item?.dateOfExpair)}</td>
						<td>
							<a class="btn btn-success" onclick="EditLicense(${item.Id}, '${item.licenseNumber}','${item.placeOfIssue}', '${formatDatefix(item.dateOfIssue)}', '${formatDatefix(item.dateOfExpair)}','${item.category}', '${item.url}')" href="#heading15"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteLicense')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#licenseTable tbody').empty();
    $('#licenseTable tbody').append(option);

    $('#licenseNumber').val('')
    $('#licenseCategory').val('')
    $('#dateOfIssue').val('')
    $('#dateOfExpair').val('')
    $('#place').val('')
    $('#drivingUrl').val('')
}

function LoadIeltsInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllIeltsInfo?empId=' + employeeId, [], 'json', ajaxLoadIeltsInfo,false);
}
function ajaxLoadIeltsInfo(response) {
    var option = '';
    //var dates = formatDatefix(response.item?.date);
    $.each(response, function (i, item) {
        option += `<tr>
						<td>${item.examType}</td>
						<td>${item.centerNo}</td>
						<td>${formatDatefix(item?.date)}</td>
						<td>${item.candidateNo}</td>
						<td>${item.overallScore}</td>
						<td> <a href="~/EmpImages/${item.attachment}" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a></td>
						<td>
							<a class="btn btn-success" onclick="EditIelts(${item.Id}, '${item.examType}','${item.centerNo}' , '${item.date}', '${item.candidateNo}', '${item.listeningScore}', '${item.readingScore}', '${item.writingScore}', '${item.speakingScore}', '${item.overallScore}', '${item.cefrScore}', '${item.attachment}')" href="#heading31"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteIelts')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#ieltsTable tbody').empty();
    $('#ieltsTable tbody').append(option);

    $('#examTypes').val('')
    $('#center').val('')
    $('#dates').val('')
    $('#candidates').val('')
    $('#listeningI').val('')
    $('#readingI').val('')
    $('#writtingI').val('')
    $('#speakingI').val('')
    $('#overall').val('')

    $('#cefr').val('')
    $('#imagePathielts').val('')
}


function LoadLanguageInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllLanguage?empId=' + employeeId, [], 'json', ajaxLoadLanguageInfo);
}
function ajaxLoadLanguageInfo(response) {
    var option = '';
	$.each(response, function (i, item) {
		var reading = 'fa-times';
		var writing = 'fa-times';
		var speaking = 'fa-times';
		var listening = 'fa-times';
		if (item.reading == 'reading') {
			reading = 'fa-check-circle';
		}
		if (item.writing == 'writing') {
			writing = 'fa-check-circle';
		}
		if (item.speaking == 'speaking') {
			speaking = 'fa-check-circle';
		}
		if (item.listening == 'listening') {
			listening = 'fa-check-circle';
		}

        option += `<tr>
						<td>${item.language?.languageName}</td>
						<td>
                                <i class="fa ${reading}"></i>
                        </td>
						<td>
                              <i class="fa ${writing}"></i>
                        </td>
						<td>
                              <i class="fa ${speaking}"></i>
                        </td>
                          <td>
                              <i class="fa ${listening}"></i>
                        </td>
						
						<td>
							<a class="btn btn-success" onclick="EditLanguage(${item.Id}, '${item.languageId}','${item.reading}' , '${item.writing}', '${item.speaking}', '${item.listening}')" href="#heading21"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteLanguage')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#languageTable tbody').empty();
    $('#languageTable tbody').append(option);

    $('#txtLanguage').val('')
    $('#Reading').val('')
    $('#Writing').val('')
    $('#Speaking').val('')
    $('#listening').val('')
  
}




function LoadEmergencyContactInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllEmergencyContact?empId=' + employeeId, [], 'json', ajaxLoadEmergencyContactInfo);
}
function ajaxLoadEmergencyContactInfo(response) {
    var option = '';
     $.each(response, function (i, item) {
         option += `<tr>
						<td>${item.name}</td>
						<td>${item.HomeAddress}</td>
						<td>${item.relation}</td>
						<td>${item.email}</td>
						<td>${item.contact}</td>
						<td>
							<a class="btn btn-success" onclick="EditEmergency(${item.Id}, '${item.name}','${item.relation}' , '${item.organization}', '${item.designation}', '${item.email}', '${item.contact}', '${item.Occupation}', '${item.OfficeAddress}', '${item.HomeAddress}')" href="#collapseSeven"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteEmergencyContact')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#EmergencyContactTable tbody').empty();
    $('#EmergencyContactTable tbody').append(option);

    $('#refName').val('')
    $('#EMOccupation').val('')
    $('#HomeAddress').val('')
    $('#OfficeAddress').val('')
	$('#relation').val(0).trigger("change");
    $('#refOrganization').val('')

    $('#refContact').val('')
    $('#refEmail').val('')
    $('#refDesignation').val('')
}


function LoadAccountsInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllAccountsInfo?empId=' + employeeId, [], 'json', ajaxLoadAccountsInfo);
}

function ajaxLoadAccountsInfo(response) {
    var option = '';
    var ii = 1;
	$.each(response, function (i, item) {
		console.log(item)
        option += `<tr>
						<td>${ii}</td>
						<td>${item ?.accountType}</td>
						<td>${item.accountNumber}</td>
						<td>
							<a class="btn btn-success" onclick="EditAccounts(${item.Id},'${item.bankId}','${item.accountNumber}','${item.accountType}')" href="#heading22"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteAccounts')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
        ii++;
    })

    $('#accountInfoTable tbody').empty();
    $('#accountInfoTable tbody').append(option);

    $('#bankId').val(0)
    $('#walletTypeId').val(0)
    $('#accountNumber').val('')
    $('#branchName').val('')
    $('#walletNumber').val('')
}

function LoadDisciplinaryInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllDisplinaryInfo?empId=' + employeeId, [], 'json', ajaxLoadDisciplinaryInfo,false);
}
function ajaxLoadDisciplinaryInfo(response) {
    console.log("dis");
    console.log(response);
    var option = '';
    //var goNumberDate = formatDatefix(response.item?.goNumberWithDate);
    //var punishDates = formatDatefix(response.item?.punishmentDate);

    $.each(response, function (i, item) {
        option += `<tr>
						<td>${item?.employee?.nameEnglish}</td>
						<td>${item ?.offenceName}</td>
						<td>${item?.naturalPunishmentName}</td>
						<td>${formatDatefix(item?.goNumberWithDate)}</td>
						<td>${formatDatefix(item?.punishmentDate)}</td>
						<td>
							 <a class="btn btn-success" onclick="EditDisciplinary(${item.Id},'${item.offenceName}','${item.naturalPunishmentName}',  '${item.goNumberWithDate}','${formatDatefix(item.punishmentDate)}','${formatDatefix(item.startingDate)}','${formatDatefix(item.endDate)}','${item.remarks}', '${item.goFileURL}')" href="#heading14"><i class="fa fa-edit"></i></a>
                             <a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteDisciplinary')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#disciplinaryTable tbody').empty();
    $('#disciplinaryTable tbody').append(option);

    $('#offenceName').val('')
    $('#naturalPunishmentName').val('')
    $('#punishmentDate').val('')
    $('#goNo').val('')
    $('#startFrom').val('')
    $('#remarkDisc').val('')
    $('#endTo').val('')
    $('#goAttachment').val('')
}

function LoadSignatures(employeeId) {
	Common.Ajax('GET', '/HRPMSEmployee/formview/GetSignaturesByEmpId?empId=' + employeeId, [], 'json', ajaxLoadSignatures, false);
}
function ajaxLoadSignatures(response) {
	console.log(response);
	$('#user_img_current_e').attr('src', '/EmpImages/'+response.url);
	$('#user_img_current_b').attr('src', '/EmpImages/'+response.banglaSign);
}
function LoadAwardInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllAwardInfo?empId=' + employeeId, [], 'json', ajaxLoadAwardInfo, false);
}
function ajaxLoadAwardInfo(response) {
    console.log("award")
    console.log(response)
    var option = '';
    //var awardDates = formatDatefix(response.item?.awardDate)
	$.each(response, function (i, item) {
		console.log(item)
		var awardDate = item.awardDate;
		var purpose = item.purpose;
		if (awardDate == null || awardDate == '') {
			awardDate = '';
		}
		else {
			awardDate = formatDatefix(item.awardDate);
		}
		if (purpose == null) {
			purpose = '';
		}
        option += `<tr>
							<td>${item.award?.awardName}</td>
							<td>${purpose}</td>
							<td>${awardDate}</td>
							<td><a href="/EmpImages/${item.url}" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a></td>
							
							<td>
								<a class="btn btn-success" onclick="EditAward(${item.Id}, '${item.award?.Id}','${item.purpose}','${formatDatefix(item.awardDate)}','${item.url}')" href="#heading19"><i class="fa fa-edit"></i></a>&nbsp;&nbsp;
								<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteAward')"  href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
							</td>
						</tr>`
    })
    $('#awardTable tbody').empty();
    $('#awardTable tbody').append(option);

    $('#awardd').val('')
    $('#perpose').val('')
    $('#txtAwardDate').val('')
    $('#user_imgAward').attr("src", "/images/file_imge.jpg");
   
}


function LoadPromotionInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllPromotionInfo?empId=' + employeeId, [], 'json', ajaxLoadPromotionInfo);
}
function ajaxLoadPromotionInfo(response) {
    var option = '';
    $.each(response, function (i, item) {
        option += `<tr>
							<td>${item.designationNew?.designationName}</td>
							<td>${item.designationOld?.designationName}</td>
							<td>${formatDatefix(item.date)}</td>
							<td>${item.goNumber}</td>
							<td>${formatDatefix(item.goDate)}</td>
							<td>
								<a class="btn btn-success" onclick="EditPromotion(${item.Id}, ${item.employeeId},'${formatDatefix(item.date)}','${item.payScaleId}','${item.remark}','${item.designationOldId}','${item.designationNewId}','${item.goNumber}','${formatDatefix(item.goDate)}')" href="#heading13"><i class="fa fa-edit"></i></a>&nbsp;&nbsp;
								<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeletePromotion')"><i class="fa fa-trash-alt"></i></a>
							</td>
						</tr>`
    })
    $('#promotionTable tbody').empty();
    $('#promotionTable tbody').append(option);

    $('#designationNewId').val(0)
    $('#designationOldId').val(0)
    $('#date').val('')
    $('#payScale').val('')
    $('#goNumber').val('')
    $('#goDate').val('')
    $('#remark').val('')
}

function LoadTrainingInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllTrainingInfo?empId=' + employeeId, [], 'json', ajaxLoadTrainingInfo);
}
function ajaxLoadTrainingInfo(response) {
    var option = '';
	$.each(response, function (i, item) {
		console.log(item)
		var trainingInstitute = '';
		var remarks = '';
		if (item.trainingInstituteId != null) {
			trainingInstitute = item.trainingInstitute ?.trainingInstituteName;
		}
		else if (item.trainingInstituteName != null) {
			trainingInstitute = item.trainingInstituteName;
		}
		else {
			trainingInstitute = '';
		}
		if (item.remarks != null) {
			remarks = item.remarks;
		}

        option += `<tr>
						<td>${item.trainingCategory?.trainingCategoryName}</td>
						<td>${item.trainingTitle}</td>
						<td>${trainingInstitute}</td>
						<td>${formatDatefix(item.fromDate)}</td>
						<td>${formatDatefix(item.toDate)}</td>
						<td>${remarks}</td>
						<td>
							<a class="btn btn-success" onclick="EditTraining(${item.Id},'${item.trainingTitle}', ${item.trainingCategoryId} , '${formatDatefix(item.fromDate)}', '${formatDatefix(item.toDate)}', ${item.trainingInstituteId}, '${item.remarks}', '${item.trainingInstituteName}')" href="#collapse11"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteTraining')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
    })
    $('#trainingTable tbody').empty();
    $('#trainingTable tbody').append(option);

    $('#trainingTitle').val('')
    $('#category').val(0)
    $('#fromDate').val('')
    $('#toDate').val('')
   
    $('#institute').val('')
    $('#instituteName').val('')
    $('#remarks').val('')
}


function LoadBankingDiplomaInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllBankingDiplomaInfo?empId=' + employeeId, [], 'json', ajaxLoadBankingDiplomaInfo);
}
function ajaxLoadBankingDiplomaInfo(response) {
    var option = '';
    var p = 0;
    $.each(response, function (i, item) {
        option += `<tr>
						<td>${p}</td>
						<td>${item.diplomaPart}</td>
						<td>${item.result}</td>
						<td>${item.passingYear}</td>
						<td>${item.session}</td>
						
						<td>
							<a class="btn btn-success" onclick="EditDiploma(${item.Id}, '${item.diplomaPart}','${item.result}' , '${item.passingYear}', '${item.session}')" href="#heading34"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteDiploma')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
        p++;
    })
    $('#bankInfoTable tbody').empty();
    $('#bankInfoTable tbody').append(option);

    $('#diplomaPart').val('')
    $('#resultdp').val('')
    $('#passingYeardp').val('')
    $('#session').val('')
}

function LoadCertificationInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllCertificationInfo?empId=' + employeeId, [], 'json', ajaxLoadCertificationInfo);
}
function ajaxLoadCertificationInfo(response) {
    var option = '';
    $.each(response, function (i, item) {
        option += `<tr>
							<td>${item.certificationName}</td>
							<td>${item.instituteName}</td>
							<td>${item.passingYear}</td>
							<td>${item.subject}</td>
							<td>${item.markGrade}</td>
							<td>
								<a class="btn btn-success" onclick="EditCertification(${item.Id}, '${item.certificationName}','${item.instituteName}','${item.passingYear}','${item.subject}','${item.resultId}','${item.markGrade}')" href="#headingTenCer"><i class="fa fa-edit"></i></a>&nbsp;&nbsp;
								<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeID}, 'DeleteCertification')"  href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
							</td>
						</tr>`
    })
    $('#certificationTable tbody').empty();
    $('#certificationTable tbody').append(option);

    $('#certificationNameCer').val('')
    $('#subjectCer').val('')
    $('#passingYearCer').val('')
    $('#markGradeCer').val('')
    $('#instituteNameCer').val('')
  
}


function LoadEducationalQualification(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllEducationalQualification?empId=' + employeeId, [], 'json', ajaxLoadEducationalQualification);
}
function ajaxLoadEducationalQualification(response) {
   
    var option = '';
    //var awardDates = formatDatefix(response.item?.awardDate)
    $.each(response, function (i, item) {
        option += `<tr>
							<td>${item.degree?.degreeName}</td>
							<td>${item.organizationName}</td>
							<td>${item.passingYear}</td>
							<td>${item.grade}</td>
							
							<td>
								<a class="btn btn-success" onclick="EditEducationq(${item.Id}, '${item?.degree?.levelofeducationId}','${item.degreeId}','${item.reldegreesubjectId}','${item.organizationId}','${item.organizationName}','${item.institution}','${item.resultId}','${item.grade}','${item.passingYear}','${item.duration}')" href="#heading10"><i class="fa fa-edit"></i></a>&nbsp;&nbsp;
								<a class="btn btn-danger" onclick="RemoveData(${item.Id},${item.employeeId}, 'DeleteEducationQualification')"   href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
							</td>
						</tr>`
    })
    $('#educationalTable tbody').empty();
    $('#educationalTable tbody').append(option);

    $('#levelOfEducation').val('')
    $('#degreeIdedu').val('')
    $('#reldegreesubjectId').val('')
    $('#organizationId').val('')
    $('#duration').val(0)
    $('#organizationName').val('')
    $('#institutionedu').val('')
    $('#resultId').val(0)
    $('#grade').val('')
    $('#passingYearedu').val('')

}


function LoadNomineeInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllNomineeInfo?empId=' + employeeId, [], 'json', ajaxLoadNomineeInfo,false);
}
function ajaxLoadNomineeInfo(response) {
    console.log(response);
    var option = '';
    //var z = 1;
    //var goDates = formatDatefix(response.item?.goDate);

    $.each(response, function (i, item) {
        var imageUrl = '';
        if (item.imageUrl != null && item.imageUrl != '') {
            imageUrl = '<a href="/EmpImages/' + item.imageUrl + '" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a>';
        }
        option += `<tr>

						
						<td>${item.name == null ? '' : item.name}</td>
						<td>${item.relation == null ? '' : item.relation}</td>
						<td>${item.contact == null ? '' : item.contact}</td>
						<td>${item.Designation == null ? '' : item.Designation}</td>
						<td>${item.Organization == null ? '' : item.Organization}</td>
						<td>${item.Email == null ? '' : item.Email}</td>
						<td>${item.address == null ? '' : item.address}</td>
						<td>${imageUrl}</td>
						

						<td>
							<a class="btn btn-success" onclick="EditNominee(${item.Id}, '${item.name}', '${item.relation}', '${item.address}','${item.contact}','${item.Email}','${item.NID}','${item.BRN}','${formatDatefix(item.nomineeDate)}','${item.Occupation}','${item.Organization}','${item.Designation}','${item.guardianName}','${item.witnessName}','${item.witnessPhone}','${item.imageUrl}')" href="#headingEight"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id}, ${item.employeeID}, 'DeleteNominee')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
       
    })
    $('#NomineeTable tbody').empty();
    $('#NomineeTable tbody').append(option);

    $('#nameNomi').val('')
    $('#relationNomi').val('')
    $('#addressa').val('')
    $('#contactNomi').val('')
    $('#emailAddressNomi').val('')
    $('#NIDNomi').val('')
    $('#BRN').val('')
    $('#nomineeDate').val('')
    $('#occupationNomi').val(0)

    $('#refOrganizationNomi').val('')
    $('#refDesignationNomi').val('')
    $('#guardianName').val('')
    $('#witnessName').val('')
    $('#witnessPhone').val('')
    $('#imagePathnominee').val('')
   
   
}


function LoadChildrenInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllChildrenInfo?empId=' + employeeId, [], 'json', ajaxLoadChildrenInfo,false);
}
function ajaxLoadChildrenInfo(response) {
    console.log(response);
    var option = '';
    //var z = 1;
    //var goDates = formatDatefix(response.item?.goDate);

    $.each(response, function (i, item) {

        option += `<tr>

						<td>${item.childName}</td>
						<td>${formatDatefix(item.dateOfBirth)}</td>
						<td>${item.gender}</td>
						<td>${item.occupation?.occupationName}</td>
						<td>${item.bloodGroup}</td>
						<td>${item?.presentEducation}</td>
						 <td> <a href="/EmpImages/${item.imageUrl}" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a></td>


						<td>
                            <a class="btn btn-info" onclick="ViewEdu(${item.Id})" href="javascript:void(0)"><i class="fas fa-graduation-cap"></i></a>

							<a class="btn btn-success" onclick="EditChildren(${item.Id}, '${item.childName}', '${item.employeeId}', '${item.childNameBN}','${formatDatefix(item.dateOfBirth)}','${item.education}','${item.gender}','${item.occupation?.Id}','${item.designation}','${item.organization}','${item.bin}','${item.nid}','${item.bloodGroup}','${item.presentEducation}','${item.disability}','${item.disablityType}','${item.imageUrl}','${item.relationship}','${item.emailAddressPersonal}','${item.phone}','${item.childNo}','${item.maritalstatus}')" href="#heading6"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id}, ${item.employeeId}, 'DeleteChildren')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`
       
    })
    $('#childrenTable tbody').empty();
    $('#childrenTable tbody').append(option);

    $('#childName').val('')
    $('#bloodGroupcd').val('')
    $('#childNameBN').val('')
    $('#contactcd').val('')
    $('#nationalitycd').val('')
    $('#relationshipcd').val('')

    $('#emailAddressPersonalcd').val('')
    $('#gendercd').val('')
    $('#disabilitycd').val('')

    $('#dateOfBirthcd').val('')
    $('#disablityTypecd').val('')
    $('#nidcd').val('')
    $('#occupationcd').val(0)
    $('#bincd').val('')
    $('#organizationcd').val('')
    $('#presentEducation').val('')
    $('#childNo').val('')
    $('#userFile1').val('')
    $('#maritalstatuscd').val('')
}

//spouse
function LoadSpouseInfo(employeeId) {
    Common.Ajax('GET', '/HRPMSEmployee/formview/GetAllSpouseInfo?empId=' + employeeId, [], 'json', ajaxLoadSpouseInfo,false);
}
function ajaxLoadSpouseInfo(response) {
    console.log(response);
    var option = '';
    //var z = 1;
    //var goDates = formatDatefix(response.item?.goDate);

    $.each(response, function (i, item) {
        option += `<tr>

						<td>${item.spouseName}</td>
						<td>${formatDatefix(item.dateOfBirth)}</td>
						<td>${item.gender}</td>
						<td>${item.occupation ?.picture}</td>
						<td><a href="/EmpImages/${item.imageUrl}" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a></td>
						<td>${item.bloodGroup}</td>
						<td><a href="/EmpImages/${item.marriageCertificate}" target="_blank"><i class="fas fa-link"><u>Click Here</u></i></a></td>
						<td>${formatDatefix(item.dateOfMarriage)}</td>
						<td>

							<a class="btn btn-success" onclick="EditSpouse(${item.Id}, '${item.employeeId}', '${item.spouseName}', '${item.nationality}','${item.spouseNameBN}','${item.bloodGroup}','${item.contact}','${item.relationship}','${item.gender}','${item.email}','${formatDatefix(item.dateOfBirth)}','${item.occupationId}','${item.nid}','${item.organization}','${item.maritalStatus}','${item.designation}','${item.imageUrl}','${item.bin}','${item.highestEducation}','${formatDatefix(item.dateOfMarriage)}','${item.marriageCertificate}')" href="#headingFive"><i class="fa fa-edit"></i></a>
							<a class="btn btn-danger" onclick="RemoveData(${item.Id}, ${item.employeeId}, 'DeleteSpouse')" href="javascript:void(0)"><i class="fa fa-trash-alt"></i></a>
						</td>
					</tr>`

    })
    $('#spouseTable tbody').empty();
    $('#spouseTable tbody').append(option);

    $('#spouseName').val('')
    $('#nationalitysp').val('')
    $('#spouseNameBN').val('')
    $('#bloodGroupsp').val('')
    $('#contact').val('')
    $('#dateOfMarriage').val('')

    $('#designationsp').val('')
    $('#relationship').val('')
    $('#gendersp').val('')

    $('#emailAddresssp').val('')
    $('#dateOfBirthsp').val('')
    $('#occupationId').val(0)
    $('#nid').val('')
    $('#organization').val('')
    $('#maritalstatusSpouse').val('')
    $('#imagePathspouse').val('')
    $('#imagePathmarriage').val('')
}

function formatDatefix(date) {
    date = new Date(date);
    var year = date.getFullYear(),
        month = date.getMonth() + 1, // months are zero indexed
        day = date.getDate(),
        hour = date.getHours(),
        minute = date.getMinutes(),
        second = date.getSeconds(),
        hourFormatted = hour % 12 || 12, // hour returned in 24 hour format
        minuteFormatted = minute < 10 ? "0" + minute : minute,
        morning = hour < 12 ? "am" : "pm";

    return day + "/" + + month + "/" + year;
}

