$('#hobbyName6').click(function () {
	$(this).val('').focus();
})

$("#approverDiv").click(function () {
	$("#AddNew2").hide();
	$("#hobbyName6").show();
	$('#hobbyName6').focus();
	$("#AddNewLevelModalHobby").show();
});


$(".add1").click(function () {
	$("#AddNew2").show();
	$("#hobbyName6").hide();
	$("#AddNewLevelModalHobby").hide();
	var v = $('#hobbyName6').val();
	var i = Math.floor(Math.random() * 90000) + 10000;
	$('.no-gutters').append(`<div class="col-md-3">
										<div class="card hobbyVal active cc aa" style="width:250px!important; border-radius: 50%; border: 1px solid #3af3e4e8; cursor:pointer">
											<div class="pt-3 outer-approve" style="height:250px;text-align:center;" id="a1" onclick="openModal('1')">
															<div style="margin-top: 41px;">
																<i class="fa fa-magic fa-3x" aria-hidden="true"></i>
															</div>
															<input class="pt-2 input1 inp" value="${v}" name="hobbyName" id="hobbyName7" />
														<button id="del2" type="button" class="close" data-dismiss="modal" aria-label="Close">
													<span id="del3" class="btn btn-danger btn-sm aaa" aria-hidden="true">&times;</span>
												</button>
												<input type="hidden" class="result23" id="EmployeeHobbyID" name="EmployeeHobbyID" value="" />
												</div>
										</div><br/>
									</div> `);

	$('#a' + i).children().find('.addF1').hide();
	$('#a' + i).find('.addF1').show();
	$('#hobbyName6').val('Add New');
});
function OpenAddModal() {
	$('#exampleModal').modal('show');
}

function EditFighter(number, Id, sectorNo, owner, remarks, employeeID, relationship) {
	$("#FighterModal").modal('show');
	$("#ffNo").val(number);
	$("#FFID").val(Id);
	$("#sectorNoF").val(sectorNo);
	$("#ownerF").val(owner);
	$("#remarkF").val(remarks);
	$("#FreedomemployeeID").val(employeeID);
	$("#relationShipF").val(relationship);
}

function Edit37(Id, subjectId, competencyLevel, training, diploma, remarks) {
	$("#ComputerLiteracyModal").modal('show');
	$("#LiteracyId").val(Id);
	$("#subjectId37").val(subjectId);
	$("#competencyLevel").val(competencyLevel);
	$("#training").val(training);
	$("#diploma").val(diploma);
	$("#remarks37").val(remarks);
}
function Edit36(Id, examType, centerNo, date, candidateNo, listeningScore, readingScore, writingScore, speakingScore, overallScore, cefrScore, attachment) {
	$("#IeltsInfoModal").modal('show');
	$("#ieltsId").val(Id);
	$("#examTypes").val(examType);
	$("#center").val(centerNo);
	$("#dates").val(date);
	$("#candidates").val(candidateNo);
	$("#listening").val(listeningScore);
	$("#reading").val(readingScore);
	$("#writting").val(writingScore);
	$("#speaking").val(speakingScore);
	$("#overall").val(overallScore);
	$("#cefr").val(cefrScore);
	//if (attachment.indexOf(".pdf") >= 0) {

	//    $('#user_img1').attr("src", "/EmpAttachment/pdfIcon.png");
	//    $('#userFile').val("/EmpAttachment/" + attachment);
	//}
	//else {
	//    $('#user_img1').attr("src", "/EmpAttachment/" + attachment);
	//}

}

function Edit35(Id, salaryGradeId, effectiveDate) {
	$("#EmployeeGradeModal").modal('show');
	$("#EmployeeOtherid35").val(Id);
	$("#salaryGradeId").val(salaryGradeId);
	$("#effectiveDate35").val(effectiveDate);
}
function Edit34(Id, costCentreId, costCentreDate) {
	$("#CostCentreModal").modal('show');
	$("#EmployeeOtherid34").val(Id);
	$("#costCentreId").val(costCentreId);
	$("#costCentreDate").val(costCentreDate);
	Common.Ajax('GET', '/HRPMSEmployee/EmployeeProjectActivity/GetcostCentreById/?id=' + costCentreId, [], 'json', ajaxGetcostCentreById);
}
function ajaxGetcostCentreById(response) {
	$("#centreCode").val(response.centreCode);
}
function Edit33(Id, simsId, busArea, type, hRFacilityId) {
	$("#EmployeeOtherInfoModal").modal('show');
	$("#EmployeeOtherid").val(Id);
	$("#simsId").val(simsId);
	$("#busArea").val(busArea);
	$("#type33").val(type);
	$("#hRFacilityId33").val(hRFacilityId);

	Common.Ajax('GET', '/HRPMSEmployee/EmployeeProjectActivity/GetHRFacilityById/?id=' + hRFacilityId, [], 'json', ajaxGetHRFacilityById);
}


function ajaxGetHRFacilityById(response) {
	$("#facilityCode").val(response.facilityCode);
}

function Edit32(Id, projectId, isActive, employeeId) {
	$("#ProjectAssignModal").modal('show');
	$("#ProjectActivityid32").val(Id);
	$("#projectId32").val(projectId);
	$("#isActive32").val(isActive);
	$("#ProjectActivityemployeeId").val(employeeId);
}


function Edit30(Id, fncode) {
	$("#FinanceCodeModal").modal('show');
	$("#fnCodeId").val(Id);
	$("#fncode").val(fncode);
}

function Edit29(Id, hRProjectId, hRDonerId, hRActivityId, isActive) {
	$("#ProjectActivityModal").modal('show');
	$("#ProjectActivityid").val(Id);
	$("#hRProjectId").val(hRProjectId);
	$("#hRDonerId").val(hRDonerId);
	$("#hRActivityId").val(hRActivityId);
	$("#isActive29").val(isActive);
}

function Edit28(Id, contractStartDate, contractEndDate, isDelete) {
	$("#EmployeeContractModal").modal('show');
	$('#ContractRef').val(Id);
	$('#ContractStartDate').val(contractStartDate);
	$('#ContractEndDate').val(contractEndDate);
	$('#isDelete28').val(isDelete);
}


function Edit27(Id, floorNo, roomNo, deskNo) {
	$("#OfficeAssignModal").modal('show');
	$("#officeAssignID").val(Id);
	$("#floorNo").val(floorNo);
	$("#roomNo").val(roomNo);
	$("#deskNo").val(deskNo);

}


function Edit26(Id, name, organization, designation, email, contact) {
	$("#ReferenceModal").modal('show');
	$("#refID25").val(Id);
	$("#refName25").val(name);
	$("#refOrganization25").val(organization);
	$("#refDesignation25").val(designation);
	$("#refEmail25").val(email);
	$("#refContact25").val(contact);
}
function Edit24(Id, companyName, organizationTypeId, position, department, startDate, endDate, companyBusiness, companyLocation, responsibilities) {
	$("#PreviousEmploymentModal").modal('show');
	$("#previousEmploymentID").val(Id);
	$("#companyName").val(companyName);
	$("#organizationType").val(organizationTypeId);
	$("#PEorganizationType").val(organizationTypeId);
	$("#position").val(position);
	$("#TLdepartmentId2").val(department);
	$("#PreviousstartDate").val(startDate);
	$("#PreviousendDate").val(endDate);
	$("#companyBusiness").val(companyBusiness);
	$("#companyLocation").html(companyLocation);
	$("#responsibilities").html(responsibilities);
}

function Edit23(Id, itemId, assetNo, description, issueDate, returnDate) {
	$("#BelongingsModal").modal('show');
	$("#belongingsItemID").val(Id);
	$("#belongingId").val(itemId);
	$("#assetNo").val(assetNo);
	$("#issueDate").val(issueDate);
	$("#remarks23").val(description);
	$("#returnDate").val(returnDate);
}


function Edit22(Id, bankId, branchName, accountNumber, walletTypeId, walletNumber) {
	$("#BankInfoModal").modal('show');
	$("#bankInfoId").val(Id);
	$("#bankId").val(bankId);
	$("#branchName").val(branchName);
	$("#accountNumber").val(accountNumber);
	$("#walletTypeId").val(walletTypeId);
	$("#walletNumber").val(walletNumber);
	if (walletTypeId == '') {
		$('#walletNumber').prop("disabled", true);
	}
	else {
		$('#walletNumber').prop("disabled", false);
	}
}

function Edit21(Id, activityTypeId, activityNameId, description) {

	$("#OtherActivityModal").modal('show');
	$("#activityID").val(Id);
	$("#activityType").val(activityTypeId);
	Common.Ajax('GET', '/global/api/getActivityNameByType/' + activityTypeId, [], 'json', ajaxActivityName, false);
	$("#activityName").val(activityNameId);
	$("#description21").html(description);
}
function ajaxActivityName(response) {
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.name + '</option>';
	});
	$('#activityName').empty();
	$('#activityName').append(options);
}

function Edit20(Id, languageId, reading, writing, speaking) {
	$("#LanguageModal").modal('show');
	$("#languageId").val(Id);
	$("#txtLanguage").val(languageId);
	if (reading == "reading") $('#Reading').prop('checked', true);
	else $('#Reading').prop('checked', false);

	if (writing == "writing") $('#Writing').prop('checked', true);
	else $('#Writing').prop('checked', false);

	if (speaking == "speaking") $('#Speaking').prop('checked', true);
	else $('#Speaking').prop('checked', false);

	// $("#txtProficiency").val(proficiency);
}


function Edit19(Id, publicationName, publicationType, publicationDate, publicationPlace) {
	$("#PublicationModal").modal('show');
	$("#publicationId").val(Id);
	$("#publicationName").val(publicationName);
	$("#publicationType").val(publicationType);
	$("#publicationPlace").val(publicationPlace);
	$("#txtPubDate").val(publicationDate);
}


function Edit18(Id, awardName, purpose, awardDate, url) {
	$("#AwardModal").modal('show');
	$("#awardId").val(Id);
	$("#awardName").val(awardName);
	$("#perpose18").val(purpose);
	$("#txtAwardDate").val(awardDate);
	//if (url.indexOf(".pdf") >= 0) {
	//    alert(url)
	//    $('#user_img1').attr("src", "/EmpAttachment/pdfIcon.png");
	//    $('#userFile').val("/EmpAttachment/" + url);
	//}
	//else {
	//    $('#user_img1').attr("src", "/EmpAttachment/" + url);
	//}
}

function Edit17(Id, membershipId, nameOrganization, remarks, membershipNo) {
	$("#MembershipModal").modal('show');
	$("#membershipId").val(Id);
	$("#membershipType").val(membershipId);
	$("#nameOrganization").val(nameOrganization);
	$("#remarks17").val(remarks);
	$("#membershipNo").val(membershipNo);
}

function Edit16(Id, travelPurposeId, titleName, location, countryId, sponsoringAgency, startDate, endDate, goNumber, goDate, remarks, titleName, accountCode, hrProgramId, projectId, leaveStartDate, leaveEndDate, purpose, file, titleOfFile, leaveStartDate, leaveEndDate) {
	$("#TravelModal").modal('show');
	$('#travelId').val(Id);
	$('#type16').val(travelPurposeId);
	$('#titleName16').val(titleName);
	$('#location16').val(location);
	$('#country16').val(countryId);
	$('#sponsoringAgency16').val(sponsoringAgency);
	$('#TravelstartDate').val(startDate);
	$('#TravelendDate').val(endDate);
	$('#leaveStartDate').val(leaveStartDate);
	$('#leaveEndDate').val(leaveEndDate);
	$('#goNumber16').val(goNumber);
	$('#goDateTraval').val(goDate);
	$('#remarks16').val(remarks);
	$('#accountCode16').val(accountCode);
	$('#hrProgramId16').val(hrProgramId);
	$('#projectId16se').val(purpose);
	$('#file').val(file);
	$('#titleOfFile16').val(titleOfFile);
}

function Edit15(Id, nameInPassport, passportNumber, placeOfIssue, dateOfIssue, dateOfExpair, attachmentUrl) {
	$("#PassportModal").modal('show');
	$("#passportId").val(Id);
	$("#nameInPassport").val(nameInPassport);
	$("#passPortNumber").val(passportNumber);
	$("#place15").val(placeOfIssue);
	$("#dateOfIssuePass").val(dateOfIssue);
	$("#dateOfExpairPass").val(dateOfExpair);
	//if (attachmentUrl.indexOf(".pdf") >= 0) {
	//    //alert(attachmentUrl)
	//    $('#user_img1').attr("src", "/EmpAttachment/pdfIcon.png");
	//    $('#userFile').val("/EmpAttachment/" + attachmentUrl);
	//}
	//else {
	//    $('#user_img1').attr("src", "/EmpAttachment/" + attachmentUrl);
	//}

}


function Edit14(Id, licenseNumber, placeOfIssue, dateOfIssue, dateOfExpair, category) {
	$("#DrivingModal").modal('show');
	$("#licenseId").val(Id);
	$("#licenseNumber").val(licenseNumber);
	$("#place14").val(placeOfIssue);
	$("#dateOfIssue").val(dateOfIssue);
	$("#dateOfExpair").val(dateOfExpair);
	$("#licenseCategory14").val(category);
}

function EditApprovalTable(Id, approvalTypeId, employeeInfoId, nameEnglish, desig, dept) {
	$("#ApprovalModal").modal('show');
	$("#approvalMasterId").val(Id);
	$("#approvalTypeId").val(approvalTypeId);
	$("#employeeInfoId").val(employeeInfoId);
	$("#employeeName").val(nameEnglish);

	Common.Ajax('GET', '/HRPMSMasterData/Approval/GetApprovalDetailByApprovalMasterId/?ApprovalMasterId=' + Id, [], 'json', ajaxGetApprovalDetailByApprovalMasterId);

}

function Edit13(Id, OffenseId, naturalPunishmentId, goNumberWithDate, punishmentDate, startingDate, endDate, remarks) {
	$("#DisciplinaryModal").modal('show');
	$("#disciplinaryId").val(Id);
	$("#offenseId").val(OffenseId);
	$("#punishmentId").val(naturalPunishmentId);
	$("#punishmentDate").val(punishmentDate);
	$("#goNo13").val(goNumberWithDate);
	$("#startFromD").val(startingDate);
	$("#endToD").val(endDate);
	$("#remarks13").val(remarks);
}

function Edit12(Id, startDate, endDate, supervisorName, supervisorDesg, signatoryName, signatoryDesg, remarks, year, score, advanceComment, supervisorCode, signatoryCode, finalStatus, effectiveDate) {
	$("#PMSHistoryModal").modal('show');
	$('#ACRLogID').val(Id);
	$('#startDateLog').val(startDate);
	$('#endDateLOG').val(endDate);
	$('#supervisorName').val(supervisorName);
	$('#supervisorDesignation').val(supervisorDesg);
	$('#signatoryName').val(signatoryName);
	$('#signatoryDesignation').val(signatoryDesg);
	$('#remarks12').val(remarks);
	$('#year12').val(year);
	$('#score12').val(score);
	$('#advanceComment').val(advanceComment);
	$('#supervisorCode').val(supervisorCode);
	$('#signatoryCode').val(signatoryCode);
	$('#finalStatus').val(finalStatus);
	$('#effectiveDateACR').val(effectiveDate);
}

function Edit11(Id, employeeId, date, payScale, remark, designationOldId, designationNewId, goNumber, goDate) {
	$("#PromotionModal").modal('show');
	$("#promotionId").val(Id);
	$("#PLogEmployeeID").val(employeeId);
	$("#designationOldId").val(designationOldId);
	$("#designationNewId").val(designationNewId);
	$("#Promotiondate").val(date);
	$("#remark11").val(remark);
	$("#payScale11").val(payScale);
	$("#goNumber").val(goNumber);
	$("#PromotiongoDate").val(goDate);

}

function Edit10(Id, workStation, from, to, salaryGradeId, departmentId, designatioId) {
	$("#ServiceModal").modal('show');
	$("#transfarID").val(Id);
	$("#workStation").val(workStation);
	$("#TLfromDate").val(from);
	$("#TLtoDate").val(to);
	$("#TLgrade").val(salaryGradeId);
	$("#TLdesignationId").val(designatioId);
	$("#TLdepartmentId").val(departmentId);
}


function Edit9(Id, trainingCategoryId, trainingInstituteId, trainingTitle, countryId, fromDate, toDate, sponsoringAgency, remarks) {
	$("#TrainingModal").modal('show');
	$('#trainingLogID').val(Id);
	$('#trainingTitle').val(trainingTitle);
	$('#category9').val(trainingCategoryId);
	// $('#country9').val(countryId);
	$('#institute9').val(trainingInstituteId);
	$('#sponsoringAgency').val(sponsoringAgency);
	$('#TfromDate').val(fromDate);
	$('#TtoDate').val(toDate);
	$('#remarks9').val(remarks);
}

function Edit8(Id, otherQualificationHeadId, instituteName, passingYear, subject, resultId, markGrade) {
	$("#OtherQualificationsModal").modal('show');
	$("#OtherqualificationID").val(Id);
	$("#OtherqualificationHeadId").val(otherQualificationHeadId);
	$("#subject8").val(subject);
	$("#result8").val(resultId);
	$("#instituteName8").val(instituteName);
	$("#PropassingYear8").val(passingYear);
	$("#markGrade8").val(markGrade);
}


function Edit7(Id, qualificationHeadId, instituteName, passingYear, subject, resultId, markGrade) {
	$("#QualificationModal").modal('show');
	$("#qualificationID").val(Id);
	$("#qualificationHeadId").val(qualificationHeadId);
	$("#subject7").val(subject);
	$("#result7").val(resultId);
	$("#instituteName7").val(instituteName);
	$("#PropassingYear").val(passingYear);
	$("#markGrade7").val(markGrade);
}

function Edit6(Id, levelofeducationId, degreeId, reldegreesubjectId, organizationId, institution, resultId, grade, passingYear) {
	$("#EducationModal").modal('show');
	$('#educationId').val(Id);
	$("#levelOfEducation").val(levelofeducationId);

	Common.Ajax('GET', '/global/api/degrees/' + levelofeducationId, [], 'json', ajaxDegrees, false);
	$('#degreeId').val(degreeId);

	Common.Ajax('GET', '/global/api/relDegreeSubjects/' + degreeId, [], 'json', ajaxRelDegreeSubjects, false);
	$('#reldegreesubjectId').val(reldegreesubjectId);

	$('#organizationId').val(organizationId);
	$('#instritution').val(institution);
	$('#resultId').val(resultId);
	$('#grade').val(grade);
	$('#passingYear6').val(passingYear);
}



function Edit5(Id, name, relation, gender, NID, dateOfBirth, insuranceDate) {
	$("#InsuranceModal").modal('show');
	$("#insuranceID").val(Id);
	$("#name5").val(name);
	$("#relation5").val(relation);
	$("#gender5").val(gender);
	$("#NID5").val(NID);
	$("#IdateOfBirth").val(dateOfBirth);
	$("#insuranceDate").val(insuranceDate);

	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetInsurancePhotoById/?Id=' + Id, [], 'json', ajaxGetInsurancePhotoById);
}

function ajaxGetInsurancePhotoById(response) {
	var baseUrl = 'http://' + location.host + '/Upload/Photo/' + response.fileName;
	var image = document.getElementById('user_img_Challan');
	image.setAttribute('src', baseUrl);
}


function Edit4(Id, name, relation, contact, address, guardianName, witnessName, nomineeDate, imgUrl, designation, organization, occupation, email, NID, BRN, witnessPhone) {
	$("#NomineeModal").modal('show');
	$("#nomineeID").val(Id);
	$("#name4").val(name);
	$("#Nomineerelation").val(relation);
	$("#Nomineerelation").trigger('change');
	$("#contact4").val(contact);
	$("#refDesignation4").val(designation);
	$("#refOrganization4").val(organization);
	//  $("#occupation4").val(occupation);
	//  $('#occupation4').trigger('change');
	$("#emailAddress4").val(email);
	$("#addressa4").val(address);
	$("#guardianName").val(guardianName);
	$("#witnessName").val(witnessName);
	$("#witnessPhone").val(witnessPhone);
	$("#nomineeDate").val(nomineeDate);
	$("#NID4").val(NID);
	$("#BRN4").val(BRN);
	$("#Nomineeoccupation").val(occupation);
	if (imgUrl.indexOf(".pdf") >= 0) {
		//alert(attachmentUrl)
		$('#user_img1').attr("src", "/EmpImages/pdfIcon.png");
		$('#userFile').val("/EmpImages/" + imgUrl);
	}
	else {
		$('#user_img1').attr("src", "/EmpImages/" + imgUrl);
	}

	Common.Ajax('GET', '/global/api/getNomineeDetailByNomineeId/' + Id, [], 'json', ajaxNomineeDetail);
}
//Emergency Contact
function Edit3(Id, name, relation, organization, designation, email, contact, occupation, OfficeAddress, HomeAddress) {
	$("#EmergencyContactModal").modal('show');
	$("#refID3").val(Id);
	$("#refName").val(name);
	$("#relation3").val(relation);
	$("#relation3").trigger('change');
	$("#refOrganization3").val(organization);
	$("#refDesignation3").val(designation);
	$("#refEmail3").val(email);
	$("#refContact3").val(contact);
	$("#occupation3").val(occupation);
	$("#OfficeAddress3").val(OfficeAddress);
	$("#HomeAddress3").val(HomeAddress);
	$('#occupation3').trigger('change');
}

//Children

function Edit2(Id, childName, childrenEmployeeID, childNameBN, dateOfBirth, emailAddressPersonal, gender, occupation, relationship, organization, bin, nid, bloodGroup, disability, disablityType, imgUrl, nationality) {
	$("#ChildrenModal").modal('show');
	$('#childrenEmployeeID').val(childrenEmployeeID);
	$('#childrenID').val(Id);
	$('#childName').val(childName);
	$('#childNameBN').val(childNameBN);
	$('#CdateOfBirth').val(dateOfBirth);
	$('#bin2').val(bin);
	$('#gender2').val(gender);
	$('#Coccupation').val(occupation);
	$('#organization2').val(organization);
	$('#nid2').val(nid);
	$('#bloodGroup2').val(bloodGroup);
	$('#disability2').val(disability);
	$('#disablityType2').val(disablityType);
	$('#CemailAddressPersonal').val(emailAddressPersonal);
	$('#relationship2').val(relationship);
	$('#Cnationality2').val(nationality);
	//$('#contact2').val(contact);
	//$('#user_img1_children').attr("src", "/EmpImages/" + imgUrl);
	if ($("#disability2").val() == '1') {
		$("#disablityType2").attr('readonly', false);
	}
	else {
		$("#disablityType2").attr('readonly', true);
		$("#disablityType2").val('');
	}


	if (imgUrl.indexOf("application/pdf") >= 0) {

		$('#user_img1_children').attr("src", "/images/defaultperson.png");
	}
	else {
		$('#user_img1_children').attr("src", "/EmpImages/" + imgUrl);
	}

}


//spouse
function Edit1(Id, SpouseEmployeeID, spouseName, spouseNameBN, email, dateOfBirth, gender, occupation, designation, organization, bin, nid, bloodGroup, contact, highestEducation, homeDistrict, imgUrl, relation) {
	// alert(relation)

	$("#SpouseModal").modal('show');
	$('#SpouseEmployeeID').val(SpouseEmployeeID);
	$('#spouseID').val(Id);
	$('#spouseName').val(spouseName);
	$('#spouseNameBN').val(spouseNameBN);
	$('#emailAddress1').val(email);
	$('#SdateOfBirth').val(dateOfBirth);
	$('#gender1').val(gender);
	$('#Soccupation').val(occupation);
	$('#organization1').val(organization);
	// $('#user_img1_Spouce').attr("src", "/EmpImages/" + imgUrl);
	$('#nid1').val(nid);
	$('#bloodGroup1').val(bloodGroup);
	$('#contact1').val(contact);
	$('#relationship1').val(relation);
	//if ($("#user_img1_Spouce").val() == "") {
	//    $('#user_img1_Spouce').attr("src", "/images/defaultperson.png");
	//}
	//else {
	//    $('#user_img1_Spouce').attr("src", "/EmpImages/" + imgUrl);
	//}
	if (imgUrl.indexOf("application/pdf") >= 0) {

		$('#user_img1_Spouce').attr("src", "/images/defaultperson.png");
	}
	else {
		$('#user_img1_Spouce').attr("src", "/EmpImages/" + imgUrl);
	}

}

function RemoveFighter(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteFighter/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove1(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteSpouse/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove2(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteChildren/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove3(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/EmergencyContactDelete/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove4(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteNominee/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove5(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteInsurance/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove6(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteEducation/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove7(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteProfessionalQualifications/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}


function Remove8(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteOtherQualification/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove9(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteTraningHistory/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove10(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteServiceHistory/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}


function Remove11(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeletePromotionLog/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove12(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeletePMSHistory/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove13(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteDisciplinary/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove14(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteDrivingLicense/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove15(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeletePassportInfo/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}


function Remove16(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteTraveInfo/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove17(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteMembershipInfo/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove18(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteAward/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}


function Remove19(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeletePublication/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove20(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteLanguage/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove21(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteOtherActivity/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove22(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteBankInfo/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove23(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteBelongings/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove24(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeletePriviousEmployment/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}


function Remove25(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteFreedomFighter/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove26(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteReference/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove27(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteOfficeAssign/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}


function Remove28(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteContractInfo/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove29(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteProjectActivity/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove30(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteFinanceCode/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove31(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteAttachment/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}


function Remove32(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteProjectAssign/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove33(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		//return true;
		location.replace("/HRPMSEmployee/Info/DeleteOtherInfo/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}

function Remove34(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteCostCentre/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove35(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteEmployeeGrade/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove36(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteIeltsFormView/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}
function Remove37(Id, employeeId) {
	chk = confirm('Do you want to delete this?');
	if (chk) {
		location.replace("/HRPMSEmployee/Info/DeleteComputerLiteracy/" + Id + "?empId=" + employeeId);
	} else {
		return false;
	}
}




var posts = {};
$(document).ready(function () {

	Common.Ajax('GET', '/global/api/GetAllUserEmployeeInfo', [], 'json', ajaxGetAllEmployee);
	Common.Ajax('GET', '/global/api/GetAllUserEmployeeInfo', [], 'json', ajaxGetAllEmployee1);
	$('#ffTable').DataTable();
	$('#computerLiteracyTable').DataTable();
	$('#ieltsTable').DataTable();
	$('#EmployeeGradeTable').DataTable();
	$('#CostCentreTable').DataTable();
	$('#EmployeeOtherInfoTable').DataTable();
	$('#projectAssignTable').DataTable();
	$('#attachmentTable').DataTable();
	$('#financeCodeTable').DataTable();
	$('#bankInfoTable').DataTable();
	$('#contractTable').DataTable();
	$('#OfficeAssignTable').DataTable();
	$('#referenceTable').DataTable();
	$('#previousEmploymentTable').DataTable();
	$('#belongingsTable').DataTable();
	$('#bankInfoTable').DataTable();
	$('#otherActivitiesTable').DataTable();
	$('#languageTable').DataTable();
	$('#publicationTable').DataTable();
	$('#awardTable').DataTable();
	$('#membershipTable').DataTable();
	$('#travelTable').DataTable();
	$('#passportTable').DataTable();
	$('#disciplinaryTable').DataTable();
	$('#ApprovalTable').DataTable();
	$('#licenseTable').DataTable();
	$('#acrLogTable').DataTable();
	$('#promotionTable').DataTable();
	$('#ServiceTable').DataTable();
	$('#trainingTable').DataTable();
	$('#otherQualificationsTable').DataTable();
	$('#educationalTable').DataTable();
	$('#professionalQualificationsTable').DataTable();
	$('#NomineeTable').DataTable();
	$('#InsuranceTable').DataTable();
	$('#spouseTable').DataTable();
	$('#childrenTable').DataTable();
	$('#emergencyTable').DataTable();
	//$("#Coccupation").select2();
	//$("#Soccupation").select2();
	$("#relation3").select2();
	$("#occupation3").select2();
	$("#TLdesignationId").select2();
	$("#TLdepartmentId").select2();
	$("#designationNewId").select2();
	$("#designationOldId").select2();
	$('#favouriteColor').change(function () {
		$('#fillColor').css('background-color', $(this).val())
	})

	$('#favouriteColor').val('@Model.employeeInfo?.favouriteColor').trigger('change');


	$("#hobbyName6").hide();
	$("#AddNewLevelModalHobby").hide();


	$("#hRFacilityId33").change(function () {
		var hRFacilityId = $('#hRFacilityId33').val();
		Common.Ajax('GET', '/HRPMSEmployee/EmployeeProjectActivity/GetHRFacilityById/?id=' + hRFacilityId, [], 'json', ajaxGetHRFacilityById);
	});
	$(document).on('click', '#del2', function () {
		$(this).closest('.bb').toggleClass('strike').fadeOut('slow', function () { $(this).remove(); });
	});


	$(document).on('click', '#del3', function () {
		$(this).closest('.cc').toggleClass('strike').fadeOut('slow', function () { $(this).parent('.col-md-3').remove(); });
	});


	$('.hobbyVal').click(function () {
		//alert('ok')
		if ($(this).hasClass('active')) {
			$(this).removeClass('active');
			$(this).find('.inp').removeAttr('name');
		}
		else {
			$(this).addClass('active')
			$(this).find('.inp').attr('name', 'hobbyName');
		}
	})

	$('#submitbtn').click(function () {
		var formData = $('#FormId').serialize();

		swal({
			title: 'Are you sure?', text: 'Do you want to save this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Save it!'
		}).then(function () {
			$.ajax({
				url: '@Url.Action("SaveHobby", "Info")',
				type: 'Post',
				data: formData
			})
				.done(function (res) {
					swal('Success', 'Saved Successfully!', 'success').then(function () {
						window.location.reload();
					});
				})
				.fail(function (res) {
					alert("Unable to Save. Please Try Again");
				});
		});
	})
	$("#fatherMobile").val('@Model.employeeInfo?.fatherMobile');
	$("#motherMobile").val('@Model.employeeInfo?.motherMobile');
	$('#fatherPassportIssueDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#motherPassportIssueDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#fatherPassportExDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#motherPassportExDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#lastTransferDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#lastPromotionDate1').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#prlDate1').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#lastWorkingDate1').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#probationStart1').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#probationEnd1').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#bondDateStart1').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#bondDateEnd11').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#lastTransferDate1').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });


	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupation, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupation1, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupation2, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllJoiningDesignation', [], 'json', ajaxFunctijoiningDesignation, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllLocation', [], 'json', ajaxFuncLocation, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllFunction', [], 'json', ajaxFunctiFunction, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllUnit', [], 'json', ajaxFunctiUnit, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllDepartment', [], 'json', ajaxFunctiDepartment, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllBranch', [], 'json', ajaxFunctiBranch, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllProgram', [], 'json', ajaxFunctiProgram, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllBranchType', [], 'json', ajaxFunctiBranchType, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllCompany', [], 'json', ajaxFunctiCompany, false);
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllCompany', [], 'json', ajaxFunctiCompany1, false);



	$("#Spousesubmit").click(function () {
		swal({
			title: 'Are you sure?', text: 'Do you want to submit this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#3085d6', cancelButtonColor: '#d33', confirmButtonText: 'Yes, submit it!'
		}).then(function () {
			$("#Spousesubmitbtn").click();
		});
	});
	Common.Ajax('GET', '/HRPMSEmployee/OtherQualifications/GetAllOtherQualificationHead', [], 'json', ajaxFunctionOtherqualification, false);

	Common.Ajax('GET', '/HRPMSEmployee/ProfessionalQualifications/GetAllQualificationHead', [], 'json', ajaxFunctionQualifications, false);
	//training
	Common.Ajax('GET', '/HRPMSEmployee/Training/GetAllTrainingCategories', [], 'json', ajaxTrainingCategories, false);
	Common.Ajax('GET', '/HRPMSEmployee/Training/GetAllTrainingInstitute', [], 'json', ajaxTrainingInstitute, false);
	$('#TfromDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#TLfromDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#TtoDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#TLtoDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#Promotiondate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#PromotiongoDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#effectiveDateACR').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#endDateLOG').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#startDateLog').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#punishmentDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#startFromD').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#endToD').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#dateOfIssue').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#dateOfExpair').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#dateOfIssuePass').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#dateOfExpairPass').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#goDateTraval').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#TravelstartDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#TravelendDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#leaveStartDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#leaveEndDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#txtAwardDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#txtPubDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#issueDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#returnDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#PreviousstartDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#PreviousendDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#IdateOfBirth').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#insuranceDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#CdateOfBirth').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#SdateOfBirth').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
	$('#dateOfBirth').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#joiningDateofPresentPost').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#joiningDatePresentWorkstation').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#joiningDateGovtService').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#dateofregularity').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#dateOfPermanent').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#dateOfRetirement').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#ContractStartDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$('#ContractEndDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1900:2100" });
	$("#attachmentDate").datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "2010:2050" }).datepicker("setDate", new Date());
	$("#dates").datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "2010:2050" }).datepicker("setDate", new Date());
	$("#costCentreDate").datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "2010:2050" }).datepicker("setDate", new Date());
	$("#effectiveDate35").datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "2010:2050" }).datepicker("setDate", new Date());



	$("#atttachmentGroupId").change(function () {
		var atttachmentGroupId = $('#atttachmentGroupId').val();
		Common.Ajax('GET', '/HRPMSEmployee/EmployeeAttachment/GetAllAttachmentCategoryByGroupId/?masterId=' + atttachmentGroupId, [], 'json', ajaxGetAllAttachmentCategoryByGroupId);
	});
	Common.Ajax('GET', '/HRPMSEmployee/EmployeeAttachment/GetAllGroup', [], 'json', ajaxFunctiGroup, false);


	Common.Ajax('GET', '/HRPMSEmployee/Membership/GetMembershipOrganization', [], 'json', ajaxFunctiRelation2, false);

	Common.Ajax('GET', '/global/api/GetActiveAllEmployeeInfo', [], 'json', ajaxGetSupervisors);
	Common.Ajax('GET', '/global/api/GetActiveAllEmployeeInfo', [], 'json', ajaxGetApprovers);
	//address

	Common.Ajax('GET', '/global/api/divisions/1', [], 'json', ajaxDivision, false);
	Common.Ajax('GET', '/global/api/divisions/1', [], 'json', ajaxPermanentDivision, false);

	$("#presentDivision").change(function () {
		var id = $(this).val();
		Common.Ajax('GET', '/global/api/districts/' + id, [], 'json', ajaxDistrict, false);
	});

	$("#permanentDivision").change(function () {
		var id = $(this).val();
		Common.Ajax('GET', '/global/api/districts/' + id, [], 'json', ajaxPermanentDistrict, false);
	});

	$("#presentDistrict").change(function () {
		var id = $(this).val();
		Common.Ajax('GET', '/global/api/thanas/' + id, [], 'json', ajaxThana, false);
	});

	$("#permanentDistrict").change(function () {
		var id = $(this).val();
		Common.Ajax('GET', '/global/api/thanas/' + id, [], 'json', ajaxPermanentThana, false);
	});

	//Nominee

	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationN, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationS, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationC, false);


	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation3, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation5, false);
	$('#nomineeDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });

	$("#fundType").change(function () {
		Common.Ajax('GET', '/global/api/getNomineeRemainingFunByEmpIdAndFundId/' + @Model.employeeID + '/' + $("#fundType").val(), [], 'json', ajaxNomineeDistributedPercent);
});


//Education
Common.Ajax('GET', '/global/api/levelOfEducations', [], 'json', ajaxLevelOfEducation, false);
Common.Ajax('GET', '/global/api/organizations', [], 'json', ajaxOrganization, false);

Common.Ajax('GET', '/global/api/results', [], 'json', ajaxResult, false);

$("#levelOfEducation").change(function () {
	var id = $(this).val();
	Common.Ajax('GET', '/global/api/degrees/' + id, [], 'json', ajaxDegrees);
});

$("#degreeId").change(function () {
	var id = $(this).val();
	Common.Ajax('GET', '/global/api/relDegreeSubjects/' + id, [], 'json', ajaxRelDegreeSubjects);
});

$("#submit").click(function () {
	swal({
		title: 'Are you sure?', text: 'Do you want to submit this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#3085d6', cancelButtonColor: '#d33', confirmButtonText: 'Yes, submit it!'
	}).then(function () {
		$("#submitbtn").click();
	});
});



$("#personal").addClass("active");


$("#dateOfBirth").change(function () {
	var dob = $("#dateOfBirth").val();
	//$("#dateOfLPR").val(dob);
});

$("#disability2").change(function () {
	if ($("#disability2").val() == '1') {
		$("#disablityType2").attr('readonly', false);
	}
	else {
		$("#disablityType2").attr('readonly', true);
		$("#disablityType2").val('');
	}
});

//$("#currentDivision").change(function () { Common.Ajax('GET', '/global/api/allAvailablePosts/' + $(this).val(), [], 'json', ajaxPosts); });

$("#designation").change(function () { $("#post").val(posts['"' + $(this).val() + '"']); });

$("#sbu").change(function () {
	if ($(this).val() != '') {
		Common.Ajax('GET', '/global/api/getRlnSBUPNSBySBUId/' + $(this).val(), [], 'json', ajaxPNS);
	}
	else {
		var options = '<option value="">Select</option>';
		$('#pns').empty();
		$('#pns').append(options);
	}
});
        });


$('#GroupAddBtn').click(function () {
	var form = $('#GroupAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Group", "EmployeeAttachment")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/EmployeeAttachment/GetAllGroup', [], 'json', ajaxFunctiGroup, false);
					$('#groupName').val('');
					$('#exampleModalGroup').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})


$('#CategoryAddBtn2').click(function () {
	var form = $('#CategoryAddForm2');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Category", "EmployeeAttachment")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					var atttachmentGroupId = $('#atttachmentGroupId').val();
					Common.Ajax('GET', '/HRPMSEmployee/EmployeeAttachment/GetAllAttachmentCategoryByGroupId/?masterId=' + atttachmentGroupId, [], 'json', ajaxGetAllAttachmentCategoryByGroupId);
					$('#categoryName').val('');
					$('#exampleModalCategory2').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})


function ajaxFunctiGroup(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.groupName}</option>`
	});
	$('#atttachmentGroupId').empty();
	$('#atttachmentGroupId').append(`<option value="">Select </option>`);
	$('#atttachmentGroupId').append(option);
}

function ajaxFunctiGroupModal(response) {
	console.log(response)
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


function AddNewGroupModal() {
	$('#exampleModalGroup').modal('show');
}

function AddNewCategoryModal2() {
	Common.Ajax('GET', '/HRPMSEmployee/EmployeeAttachment/GetAllGroup', [], 'json', ajaxFunctiGroupModal, false);

	$('#exampleModalCategory2').modal('show');
}

function AddNewRelationModal2() {
	$('#exampleModalRelation2').modal('show');
}
$('#RelationAddBtn2').click(function () {
	var form = $('#RelationAddForm2');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("MembershipOrganization", "Membership")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Membership/GetMembershipOrganization', [], 'json', ajaxFunctiRelation2, false);
					$('#organizationType2').val('');
					$('#organizationName2').val('');
					$('#exampleModalRelation2').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})


function ajaxFunctiRelation2(response) {
	console.log(response)
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



function saveEmpInfo() {
	var formData = $('#empInfoForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateEmpPersonalInfo", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveAddress() {
	var formData = $('#AddressForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateAddress", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveSpouce() {
	if ($("#spouseName").val() == null || $("#spouseName").val() == '') {
		alert('Spouse Name required');
		return false;
	}

	if ($("#Soccupation").val() == null || $("#Soccupation").val() == '') {
		alert('Occupation required');
		return false;
	}
	//e.preventDefault()
	var formData = $('#spouceForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateSpouce", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#SpouseModal').modal('hide');
				$('#SpouseModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});


}


$("#SpouceimagePath1").change(function () {
	readURL1Spouce(this);
});

function readURL1Spouce(input) {
	if (input.files && input.files[0]) {
		var filerdr = new FileReader();
		filerdr.onload = function (e) {
			console.log(e);

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

function saveSpoucePhoto(id) {
	var fileData = new FormData();
	var fileUpload = $("#SpouceimagePath1").get(0);
	var files = fileUpload.files;

	// Looping over all files and add it to FormData object
	for (var i = 0; i < files.length; i++) {
		fileData.append('childrenPhoto', files[i]);
	}
	fileData.append('id', id);

	$.ajax({
		url: '@Url.Action("UpdateSpouseFile", "Info")',
		data: fileData,
		type: 'Post',
		dataType: 'json',
		contentType: false,
		processData: false,
	})
		.done(function () {
			swal('Success', 'Updated Successfully!', 'success');
			$('#SpouseModal').modal('hide');
			$('#SpouseModal').find("input[type=text], textarea").val("");
		})
		.fail(function () {
			alert("Unable to Save. Please Try Again");
		});
}
function saveSpouceWithoutPhoto() {
	if ($("#spouseName").val() == null || $("#spouseName").val() == '') {
		alert('Spouse Name required');
		return false;
	}

	if ($("#Soccupation").val() == null || $("#Soccupation").val() == '') {
		alert('Occupation required');
		return false;
	}
	var formData = $('#spouceForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateSpouce", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function (res) {
				if (res > 0) {
					saveSpoucePhoto(res);
				}
				$('#spouceForm')[0].reset();
				$('#user_img1_Spouce').attr('src', '/images/defaultperson.png');
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}






$("#childrenimagePath1").change(function () {
	readURL1(this);
});

function readURL1(input) {
	if (input.files && input.files[0]) {
		var filerdr = new FileReader();
		filerdr.onload = function (e) {
			console.log(e);

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

function saveChildrenPhoto(id) {
	var fileData = new FormData();
	var fileUpload = $("#childrenimagePath1").get(0);
	var files = fileUpload.files;

	// Looping over all files and add it to FormData object
	for (var i = 0; i < files.length; i++) {
		fileData.append('childrenPhoto', files[i]);
	}
	fileData.append('id', id);

	$.ajax({
		url: '@Url.Action("UpdateChildrenFile", "Info")',
		data: fileData,
		type: 'Post',
		dataType: 'json',
		contentType: false,
		processData: false,
	})
		.done(function () {
			swal('Success', 'Updated Successfully!', 'success');
			$('#ChildrenModal').modal('hide');
			$('#ChildrenModal').find("input[type=text], textarea").val("");
			location.reload();
		})
		.fail(function () {
			alert("Unable to Save. Please Try Again");
		});
}
function saveChildrenWithoutPhoto() {
	if ($("#childName").val() == null || $("#childName").val() == '') {
		alert('Children Name required');
		return false;
	}

	if ($("#Coccupation").val() == null || $("#Coccupation").val() == '') {
		alert('Occupation required');
		return false;
	}

	if ($("#CdateOfBirth").val() == null || $("#CdateOfBirth").val() == '') {
		alert('Date Of Birth required');
		return false;
	}
	var formData = $('#childrenForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateChildren", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function (res) {
				if (res > 0) {
					saveChildrenPhoto(res);
				}
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}





function ajaxGetAllEmployee(response) {
	var GetEmployeeList = [];
	$.each(response, function (id, option) {
		console.log(option)
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

//function ajaxGetAllEmployee1(response) {
//    var GetEmployeeList = [];
//    $.each(response, function (id, option) {
//        console.log(option)
//        var obj = new Object();
//        obj.key = option.Id;
//        obj.value = option.nameEnglish + ' (' + option.employeeCode + ')';
//        GetEmployeeList[GetEmployeeList.length] = obj;
//    });

//    $('#approverName').autocomplete({
//        source: GetEmployeeList,
//        select: function (event, ui) {
//            $("#approverIds").val(ui.item.key);
//            $("#approverName").val(ui.item.value);

//        }
//    });
//}
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


function saveChildren() {

	if ($("#childName").val() == null || $("#childName").val() == '') {
		alert('Children Name required');
		return false;
	}

	if ($("#Coccupation").val() == null || $("#Coccupation").val() == '') {
		alert('Occupation required');
		return false;
	}

	if ($("#CdateOfBirth").val() == null || $("#CdateOfBirth").val() == '') {
		alert('Date Of Birth required');
		return false;
	}
	var formData = $('#childrenForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateChildren", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#ChildrenModal').modal('hide');
				$('#ChildrenModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}







function saveEContact() {
	if ($("#refName").val() == null || $("#refName").val() == '') {
		alert('Contact Name required');
		return false;
	}

	if ($("#relation3").val() == null || $("#relation3").val() == '') {
		alert('Relationship required');
		return false;
	}

	if ($("#refContact3").val() == null || $("#refContact3").val() == '') {
		alert('Mobile (Personal) required');
		return false;
	}
	var formData = $('#emergencyContactForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateEmergencyContact", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#EmergencyContactModal').modal('hide');
				$('#EmergencyContactModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}




$("#NomineeimagePath1").change(function () {
	readURL1Nominee(this);
});

function readURL1Nominee(input) {
	if (input.files && input.files[0]) {
		var filerdr = new FileReader();
		filerdr.onload = function (e) {
			console.log(e);

			var srcUrl = e.target.result;
			if (srcUrl.indexOf("application/pdf") > 0) {
				$('#user_img1_Nominee').attr('src', "/EmpAttachment/pdfIcon.png");
				$('#userFileNominee').val(e.target.result);
			}
			else {
				$('#user_img1_Nominee').attr('src', e.target.result);
			}

		}
		filerdr.readAsDataURL(input.files[0]);
	}
}

function saveNomineePhoto(id) {
	var fileData = new FormData();
	var fileUpload = $("#NomineeimagePath1").get(0);
	var files = fileUpload.files;

	// Looping over all files and add it to FormData object
	for (var i = 0; i < files.length; i++) {
		fileData.append('childrenPhoto', files[i]);
	}
	fileData.append('id', id);

	$.ajax({
		url: '@Url.Action("UpdateNomineeFile", "Info")',
		data: fileData,
		type: 'Post',
		dataType: 'json',
		contentType: false,
		processData: false,
	})
		.done(function () {
			swal('Success', 'Updated Successfully!', 'success');
			$('#NomineeModal').modal('hide');
			$('#NomineeModal').find("input[type=text], textarea").val("");
		})
		.fail(function () {
			alert("Unable to Save. Please Try Again");
		});
}
function saveNomineeWithoutPhoto() {
	if ($("#name4").val() == null || $("#name4").val() == '') {
		alert('Name required');
		return false;
	}

	if ($("#Nomineerelation").val() == null || $("#Nomineerelation").val() == '') {
		alert('Relationship required');
		return false;
	}

	if ($("#contact4").val() == null || $("#contact4").val() == '') {
		alert('Phone required');
		return false;
	}
	if ($("#fundType").val() == null || $("#fundType").val() == '') {
		alert('Insurance Type required');
		return false;
	}
	var formData = $('#NomineeForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateNominee", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function (res) {
				if (res > 0) {
					saveNomineePhoto(res);
				}
				$('#NomineeForm')[0].reset();
				$('#user_img1_Nominee').attr('src', '/images/file_imge.jpg');
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}








function saveNominee() {

	if ($("#name4").val() == null || $("#name4").val() == '') {
		alert('Name required');
		return false;
	}

	if ($("#Nomineerelation").val() == null || $("#Nomineerelation").val() == '') {
		alert('Relationship required');
		return false;
	}

	if ($("#contact4").val() == null || $("#contact4").val() == '') {
		alert('Phone required');
		return false;
	}
	//if ($("#fundType").val() == null || $("#fundType").val() == '') {
	//    alert('Insurance Type required');
	//    return false;
	//}
	var totaleRow = $('#fundValueListTable tr').length - 1
	if (totaleRow == 0) {
		swal('warning', 'No Insurance Type selected', 'warning');
	}
	var formData = $('#NomineeForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateNominee", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#NomineeModal').modal('hide');
				$('#NomineeModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveInsurance() {

	if ($("#name5").val() == null || $("#name5").val() == '') {
		alert('Name required');
		return false;
	}

	if ($("#relation5").val() == null || $("#relation5").val() == '') {
		alert('Relation required');
		return false;
	}

	if ($("#IdateOfBirth").val() == null || $("#IdateOfBirth").val() == '') {
		alert('Date Of Birth required');
		return false;
	}

	var formData = $('#InsuranceForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateInsurance", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#InsuranceModal').modal('hide');
				$('#InsuranceModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveEducation() {

	if ($("#levelOfEducation").val() == null || $("#levelOfEducation").val() == '') {
		alert('level Of Education required');
		return false;
	}

	if ($("#degreeId").val() == null || $("#degreeId").val() == '') {
		alert('Certificate/Degree required');
		return false;
	}

	if ($("#passingYear6").val() == null || $("#passingYear6").val() == '') {
		alert('Passing Year required');
		return false;
	}

	var formData = $('#EducationForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateEducation", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#EducationModal').modal('hide');
				$('#EducationModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveQualification() {
	if ($("#qualificationHeadId").val() == null || $("#qualificationHeadId").val() == '') {
		alert('Qualification required');
		return false;
	}

	if ($("#result7").val() == null || $("#result7").val() == '') {
		alert('result required');
		return false;
	}

	var formData = $('#QualificationForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateQualifications", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#QualificationModal').modal('hide');
				$('#QualificationModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveOtherQualification() {
	if ($("#OtherqualificationHeadId").val() == null || $("#OtherqualificationHeadId").val() == '') {
		alert('Qualification required');
		return false;
	}

	if ($("#result8").val() == null || $("#result8").val() == '') {
		alert('result required');
		return false;
	}
	var formData = $('#OtherQualificationnAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateOtherQualifications", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#OtherQualificationsModal').modal('hide');
				$('#OtherQualificationsModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveTraining() {

	if ($("#trainingTitle").val() == null || $("#trainingTitle").val() == '') {
		alert('Training Title required');
		return false;
	}

	if ($("#TfromDate").val() == null || $("#TfromDate").val() == '') {
		alert('From Date required');
		return false;
	}

	if ($("#institute9").val() == null || $("#institute9").val() == '') {
		alert('Institute required');
		return false;
	}

	if ($("#category9").val() == null || $("#category9").val() == '') {
		alert('Training Type  required');
		return false;
	}
	if ($("#TtoDate").val() == null || $("#TtoDate").val() == '') {
		alert('To Date required');
		return false;
	}
	if ($("#remarks9").val() == null || $("#remarks9").val() == '') {
		alert('Remarks required');
		return false;
	}
	var formData = $('#TrainingAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateTraining", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#TrainingModal').modal('hide');
				$('#TrainingModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveService() {

	if ($("#workStation").val() == null || $("#workStation").val() == '') {
		alert('Work Station required');
		return false;
	}

	if ($("#TLfromDate").val() == null || $("#TLfromDate").val() == '') {
		alert('From Date required');
		return false;
	}

	var formData = $('#ServiceAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateService", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#ServiceModal').modal('hide');
				$('#ServiceModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function savePromotion() {

	if ($("#designationNewId").val() == null || $("#designationNewId").val() == '') {
		alert('New Designation required');
		return false;
	}

	if ($("#Promotiondate").val() == null || $("#Promotiondate").val() == '') {
		alert('Date of Promotion required');
		return false;
	}

	if ($("#designationNewId").val() == null || $("#designationNewId").val() == '') {
		alert('New Designation required');
		return false;
	}

	if ($("#payScale11").val() == null || $("#payScale11").val() == '') {
		alert('Grade required');
		return false;
	}
	var formData = $('#PromotionAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdatePromotion", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#PromotionModal').modal('hide');
				$('#PromotionModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveDisciplinary() {
	if ($("#offenseId").val() == null || $("#offenseId").val() == '') {
		alert('Offense required');
		return false;
	}

	if ($("#punishmentId").val() == null || $("#punishmentId").val() == '') {
		alert('Natural of Punishment required');
		return false;
	}

	if ($("#punishmentDate").val() == null || $("#punishmentDate").val() == '') {
		alert('Ref. Date required');
		return false;
	}

	if ($("#goNo13").val() == null || $("#goNo13").val() == '') {
		alert('Ref. No required');
		return false;
	}

	var formData = $('#DisciplinaryAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateDisciplinary", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#DisciplinaryModal').modal('hide');
				$('#DisciplinaryModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveApproval() {
	if ($("#approvalTypeId").val() == null || $("#approvalTypeId").val() == '') {
		alert('Approval Type required');
		return false;
	}

	if ($("#sortOrders").val() == null || $("#sortOrders").val() == '') {
		alert('Sort Order required');
		return false;
	}

	if ($("#approverName").val() == null || $("#approverName").val() == '') {
		alert('Approver Name required');
		return false;
	}

	var formData = $('#ApprovalAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateApproval", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#ApprovalModal').modal('hide');
				$('#ApprovalModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveLicense() {
	if ($("#licenseNumber").val() == null || $("#licenseNumber").val() == '') {
		alert('License Number required');
		return false;
	}

	if ($("#licenseCategory14").val() == null || $("#licenseCategory14").val() == '') {
		alert('License Category required');
		return false;
	}

	if ($("#dateOfIssue").val() == null || $("#dateOfIssue").val() == '') {
		alert('Date of Issue required');
		return false;
	}
	if ($("#place14").val() == null || $("#place14").val() == '') {
		alert('Place of Issu required');
		return false;
	}

	if ($("#dateOfExpair").val() == null || $("#dateOfExpair").val() == '') {
		alert('Date of Expiry required');
		return false;
	}
	var formData = $('#LicenseAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateLicense", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#DrivingModal').modal('hide');
				$('#DrivingModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function savePMSHistory() {

	if ($("#year12").val() == null || $("#year12").val() == '') {
		alert('Year required');
		return false;
	}

	if ($("#score12").val() == null || $("#score12").val() == '') {
		alert('Score required');
		return false;
	}

	if ($("#startDateLog").val() == null || $("#startDateLog").val() == '') {
		alert('Start Date required');
		return false;
	}

	if ($("#endDateLOG").val() == null || $("#endDateLOG").val() == '') {
		alert('End Date required');
		return false;
	}

	if ($("#supervisorName").val() == null || $("#supervisorName").val() == '') {
		alert('Supervisor Name required');
		return false;
	}

	if ($("#signatoryName").val() == null || $("#signatoryName").val() == '') {
		alert('Signatory Name required');
		return false;
	}
	var formData = $('#PMSHistoryAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdatePMSHistory", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#PMSHistoryModal').modal('hide');
				$('#PMSHistoryModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function savePassport() {
	if ($("#nameInPassport").val() == null || $("#nameInPassport").val() == '') {
		alert('Name In Passport(English) required');
		return false;
	}

	if ($("#dateOfIssuePass").val() == null || $("#dateOfIssuePass").val() == '') {
		alert('Date of Issue required');
		return false;
	}

	if ($("#dateOfExpairPass").val() == null || $("#dateOfExpairPass").val() == '') {
		alert('Date Of Expair required');
		return false;
	}
	var formData = $('#PassportAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdatePassport", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#PassportModal').modal('hide');
				$('#PassportModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveTravel() {
	if ($("#type16").val() == null || $("#type16").val() == '') {
		alert('Travel Type required');
		return false;
	}

	if ($("#titleName16").val() == null || $("#titleName16").val() == '') {
		alert('Title of Training/Seminar required');
		return false;
	}

	if ($("#hrProgramId16").val() == null || $("#hrProgramId16").val() == '') {
		alert('Program required');
		return false;
	}
	if ($("#country16").val() == null || $("#country16").val() == '') {
		alert('Country required');
		return false;
	}

	if ($("#TravelstartDate").val() == null || $("#TravelstartDate").val() == '') {
		alert('Start Date required');
		return false;
	}

	if ($("#leaveStartDate").val() == null || $("#leaveStartDate").val() == '') {
		alert('Leave Taken From required');
		return false;
	}
	if ($("#TravelendDate").val() == null || $("#TravelendDate").val() == '') {
		alert('End Date required');
		return false;
	}

	if ($("#leaveEndDate").val() == null || $("#leaveEndDate").val() == '') {
		alert('Leave Taken To required');
		return false;
	}
	var formData = $('#TravelAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateTravel", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#TravelModal').modal('hide');
				$('#TravelModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveMembership() {
	if ($("#nameOrganization").val() == null || $("#nameOrganization").val() == '') {
		alert('Organization Name required');
		return false;
	}
	if ($("#membershipType").val() == null || $("#membershipType").val() == '') {
		alert('Membership Type required');
		return false;
	}

	var formData = $('#MembershipAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateMembership", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#MembershipModal').modal('hide');
				$('#MembershipModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveAward() {
	if ($("#awardName").val() == null || $("#awardName").val() == '') {
		alert('Award Name required');
		return false;
	}
	if ($("#perpose18").val() == null || $("#perpose18").val() == '') {
		alert('Event  required');
		return false;
	}
	if ($("#txtAwardDate").val() == null || $("#txtAwardDate").val() == '') {
		alert('Date required');
		return false;
	}
	var formData = $('#AwardAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateAward", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#AwardModal').modal('hide');
				$('#AwardModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function savePublication() {
	if ($("#publicationName").val() == null || $("#publicationName").val() == '') {
		alert('Publication Name  required');
		return false;
	}
	if ($("#publicationPlace").val() == null || $("#publicationPlace").val() == '') {
		alert('Pblication Place required');
		return false;
	}
	if ($("#publicationType").val() == null || $("#publicationType").val() == '') {
		alert('Publication Type required');
		return false;
	}
	if ($("#txtPubDate").val() == null || $("#txtPubDate").val() == '') {
		alert('Publication Date required');
		return false;
	}
	var formData = $('#PublicationAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdatePublication", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#PublicationModal').modal('hide');
				$('#PublicationModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveLanguage() {
	if ($("#txtLanguage").val() == null || $("#txtLanguage").val() == '') {
		alert('Language (English) required');
		return false;
	}
	var formData = $('#LanguageAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateLanguage", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#LanguageModal').modal('hide');
				$('#LanguageModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveOtherActivity() {
	if ($("#activityType").val() == null || $("#activityType").val() == '') {
		alert('Activity Type required');
		return false;
	}
	if ($("#activityName").val() == null || $("#activityName").val() == '') {
		alert('Activity Name required');
		return false;
	}

	var formData = $('#OtherActivityAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateOtherActivity", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#OtherActivityModal').modal('hide');
				$('#OtherActivityModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveBankInfo() {
	if ($("#bankId").val() == null || $("#bankId").val() == '') {
		alert('Bank Name required');
		return false;
	}
	if ($("#accountNumber").val() == null || $("#accountNumber").val() == '') {
		alert('Account Number required');
		return false;
	}
	var formData = $('#BankInfoAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateBankInfo", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#BankInfoModal').modal('hide');
				$('#BankInfoModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveBelongings() {
	//if ($("#belongingId").val() == null || $("#belongingId").val() == '') {
	//    alert('Belongings Item required');
	//    return false;
	//}
	if ($("#issueDate").val() == null || $("#issueDate").val() == '') {
		alert('Issue Date required');
		return false;
	}
	var formData = $('#BelongingsAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateBelongings", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#BelongingsModal').modal('hide');
				$('#BelongingsModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function savePreviousEmployment() {
	if ($("#companyName").val() == null || $("#companyName").val() == '') {
		alert('Company Name required');
		return false;
	}
	if ($("#PEorganizationType").val() == null || $("#PEorganizationType").val() == '') {
		alert('Organization Type required');
		return false;
	}
	if ($("#position").val() == null || $("#position").val() == '') {
		alert('Position required');
		return false;
	}
	if ($("#TLdepartmentId2").val() == null || $("#TLdepartmentId2").val() == '') {
		alert('Department required');
		return false;
	}
	if ($("#PreviousstartDate").val() == null || $("#PreviousstartDate").val() == '') {
		alert('Start Date required');
		return false;
	}
	if ($("#PreviousendDate").val() == null || $("#PreviousendDate").val() == '') {
		alert('End Date  required');
		return false;
	}
	var formData = $('#PreviousEmploymentAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdatePreviousEmployment", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#PreviousEmploymentModal').modal('hide');
				$('#PreviousEmploymentModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveFreedomFighter() {
	if ($("#ffNo").val() == null || $("#ffNo").val() == '') {
		alert('F.F No required');
		return false;
	}

	var formData = $('#FighterAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateFreedomFighter", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#FighterModal').modal('hide');
				$('#FighterModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}
function saveReference() {
	if ($("#refName25").val() == null || $("#refName25").val() == '') {
		alert('Ref. Name required');
		return false;
	}
	if ($("#refDesignation25").val() == null || $("#refDesignation25").val() == '') {
		alert('Ref. Designation required');
		return false;
	}
	if ($("#refOrganization25").val() == null || $("#refOrganization25").val() == '') {
		alert('Ref. Organization required');
		return false;
	}
	if ($("#refContact25").val() == null || $("#refContact25").val() == '') {
		alert('Ref. Contact required');
		return false;
	}
	var formData = $('#ReferenceAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateReference", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#ReferenceModal').modal('hide');
				$('#ReferenceModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveOfficeAssign() {

	if ($("#floorNo").val() == null || $("#floorNo").val() == '') {
		alert('Floor No  required');
		return false;
	}
	if ($("#roomNo").val() == null || $("#roomNo").val() == '') {
		alert('Room No required');
		return false;
	}
	var formData = $('#OfficeAssignAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateOfficeAssign", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#OfficeAssignModal').modal('hide');
				$('#OfficeAssignModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveEmployeeContract() {
	if ($("#ContractStartDate").val() == null || $("#ContractStartDate").val() == '') {
		alert('Contract Start Date required');
		return false;
	}
	if ($("#ContractEndDate").val() == null || $("#ContractEndDate").val() == '') {
		alert('Contract End Date required');
		return false;
	}
	var formData = $('#EmployeeContractAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateEmployeeContract", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#EmployeeContractModal').modal('hide');
				$('#EmployeeContractModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveProjectActivity() {
	if ($("#hRProjectId").val() == null || $("#hRProjectId").val() == '') {
		alert('Project required');
		return false;
	}
	if ($("#hRDonerId").val() == null || $("#hRDonerId").val() == '') {
		alert('Doner required');
		return false;
	}
	if ($("#hRActivityId").val() == null || $("#hRActivityId").val() == '') {
		alert('Activity required');
		return false;
	}
	if ($("#isActive29").val() == null || $("#isActive29").val() == '') {
		alert('Is Active required');
		return false;
	}
	var formData = $('#ProjectActivityAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateProjectActivity", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#ProjectActivityModal').modal('hide');
				$('#ProjectActivityModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveFinanceCode() {
	if ($("#fncode").val() == null || $("#fncode").val() == '') {
		alert('Finance Code required');
		return false;
	}

	var formData = $('#FinanceCodeAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateFinanceCode", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#FinanceCodeModal').modal('hide');
				$('#FinanceCodeModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveAttachment() {
	if ($("#atttachmentGroupId").val() == null || $("#atttachmentGroupId").val() == '') {
		alert('Attachment Group required');
		return false;
	}
	if ($("#atttachmentCategoryId").val() == null || $("#atttachmentCategoryId").val() == '') {
		alert('Attachment Category Type required');
		return false;
	}

	if ($("#fileTitle").val() == null || $("#fileTitle").val() == '') {
		alert('File Name required');
		return false;
	}
	if ($("#user_img1").val() == null || $("#user_img1").val() == '') {
		alert('Select File required');
		return false;
	}
	var formData = $('#AttachmentAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateAttachment", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#AttachmentModal').modal('hide');
				$('#AttachmentModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveProjectAssign() {
	if ($("#projectId32").val() == null || $("#projectId32").val() == '') {
		alert('Project required');
		return false;
	}
	if ($("#isActive32").val() == null || $("#isActive32").val() == '') {
		alert('Is Active  required');
		return false;
	}

	var formData = $('#ProjectAssignAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateProjectAssign", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#ProjectAssignModal').modal('hide');
				$('#ProjectAssignModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveEmployeeOtherInfo() {
	if ($("#simsId").val() == null || $("#simsId").val() == '') {
		alert('SIMS Id required');
		return false;
	}
	if ($("#hRFacilityId33").val() == null || $("#hRFacilityId33").val() == '') {
		alert('Facility Name required');
		return false;
	}
	if ($("#busArea").val() == null || $("#busArea").val() == '') {
		alert('Bus Area  required');
		return false;
	}
	if ($("#type33").val() == null || $("#type33").val() == '') {
		alert('Type required');
		return false;
	}
	var formData = $('#EmployeeOtherInfoAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateEmployeeOtherInfo", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#EmployeeOtherInfoModal').modal('hide');
				$('#EmployeeOtherInfoModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveCostCentre() {
	if ($("#costCentreId").val() == null || $("#costCentreId").val() == '') {
		alert('Cost Centre required');
		return false;
	}
	var formData = $('#CostCentreAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateCostCentre", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#CostCentreModal').modal('hide');
				$('#CostCentreModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveEmployeeGrade() {
	if ($("#salaryGradeId").val() == null || $("#salaryGradeId").val() == '') {
		alert('Salary Grade required');
		return false;
	}

	var formData = $('#EmployeeGradeAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateEmployeeGrade", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#EmployeeGradeModal').modal('hide');
				$('#EmployeeGradeModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveIeltsInfo() {
	if ($("#examTypes").val() == null || $("#examTypes").val() == '') {
		alert('Exam Type required');
		return false;
	}
	if ($("#dates").val() == null || $("#dates").val() == '') {
		alert('Date required');
		return false;
	}
	if ($("#candidates").val() == null || $("#candidates").val() == '') {
		alert('Candidate No required');
		return false;
	}

	var formData = $('#IeltsInfoAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateIeltsInfo", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#IeltsInfoModal').modal('hide');
				$('#IeltsInfoModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}

function saveComputerLiteracy() {
	if ($("#subjectId37").val() == null || $("#subjectId37").val() == '') {
		alert('Subject required');
		return false;
	}
	if ($("#competencyLevel").val() == null || $("#competencyLevel").val() == '') {
		alert('CompetencyLevel required');
		return false;
	}
	var formData = $('#ComputerLiteracyAddForm').serialize();
	swal({
		title: 'Are you sure?', text: 'Do you want to update this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Update it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("UpdateComputerLiteracy", "Info")',
			type: 'Post',
			data: formData
		})
			.done(function () {
				swal('Success', 'Updated Successfully!', 'success');
				$('#ComputerLiteracyModal').modal('hide');
				$('#ComputerLiteracyModal').find("input[type=text], textarea").val("");
				location.reload();
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
}



function AdditemDetails() {
	var approverId = $('#approverIds').val();

	var ifExistIndex = window.localStorage.getItem("holdIndex1");

	var index = "0";
	if (ifExistIndex == "null" || ifExistIndex == null) {
		index = $("#tblitemDetails tbody>tr").length;
	}
	else {
		index = ifExistIndex;
		tdRID = $("#tblitemDetails tbody >tr:eq(" + ifExistIndex + ") td:eq(0) .clsId").val();
	}

	if ($("#approverName").val() == "") {
		alert("Plaese Enter Approver Name.");
		return false
	}

	if ($("#approverIds").val() == "" || $("#approverIds").val() == "0") {
		alert("Plaese Enter Approver Name.");
		return false
	}
	if ($("#sortOrders").val() == "" || $("#sortOrders").val() == "0") {
		alert("Plaese Enter Sort Order.");
		return false
	}

	let id = parseInt($("#detailsId").val()) ? parseInt($("#detailsId").val()) : 0;
	var approverName = $("#approverName").val();
	var canFinalApprover = $("#canFinalApprover").val();
	var approverId = $("#approverIds").val();
	var sortOrder = $("#sortOrders").val();
	var status = $("#statuses").val();
	var stat = "";
	//alert(canFinalApprover);
	var RowCount = $("#tblitemDetails tbody>tr").length;
	var editOption = parseInt($('#IsEdit').val()) ? parseInt($('#IsEdit').val()) : 0;

	if (canFinalApprover == 1) {
		stat = "Final"
	}
	else {
		stat = "Next"
	}

	for (i = 0; i < RowCount; i++) {
		var _approverId = $("#approverId" + i).val();

		if (_approverId == approverId && editOption == 0) {
			alert('You have already added this Approver!');
			return false
		}
	}

	$.ajax({
		url: '@Url.Action("GetDesDeptOfApprover", "Approval")',
		data: { id: approverId },
		type: 'GET',
	})
		.done(function (res) {
			var dtTable = $("#tblitemDetails");
			var tableBody = dtTable.find('tbody');
			var trHtml = '<tr id=' + index + '>' +
				'<td>' + approverName + '<input type="hidden" name="id"class="clsId" value="' + id + '"><input type="hidden" id="approverId' + index + '" name="approverId" class="clsapproverId" value="' + approverId + '"/></td>' +
				'<td>' + res.employeeInfo.department.deptName + '</td>' +
				'<td>' + res.employeeInfo.designation + '</td>' +
				'<td>' + status + '<input type="hidden" id="status' + index + '" name="status" class="clsstatus" value="' + status + '"/></td>' +
				'<td style="text-align:right;">' + sortOrder + '<input type="hidden" id="sortOrder' + index + '" name="sortOrder" class="clssortOrder" value="' + sortOrder + '"/></td>' +
				'<td style="text-align:right;">' + stat + '<input type="hidden" id="canFinalApprover' + index + '" name="canFinalApprover" class="clssortOrder" value="' + canFinalApprover + '"/></td>' +

				'<td><a href="javascript:void(0)"style="background-color:red;border-color:red" data-toggle="tooltip" title="Delete" class="btn btn-danger btn-xs" onclick="Delete(' + index + ')"><i class="fa fa-trash"></i></a>&nbsp;&nbsp;' +
				'<a onclick="EditTbl(' + index + "," + approverId + ", " + sortOrder + ", `" + approverName + "`,  `" + status + "`,  " + id + ')" href="javascript:void(0)" title="Edit" class="btn btn-success btn-xs" > <i class="fa fa-edit"></i></a >' +


				'</tr>';

			if (ifExistIndex == null || ifExistIndex == "null") {
				tableBody.append(trHtml);
			}
			else {
				var rowUpdate = $("#tblitemDetails tbody >tr:eq(" + ifExistIndex + ")");
				rowUpdate.replaceWith(trHtml);
				window.localStorage.setItem("holdIndex", null);
				ifExistIndex = null;
			}

			Refresh();
		})
		.fail(function () {
			//swal('', 'Unable to Delete!', 'warning');
		});
}


//training
function AddNewCategoryModal() {
	$('#exampleModalCategory').modal('show');
}

function AddNewInstituteModal() {
	$('#exampleModalInstitute').modal('show');
}


$('#CategoryAddBtn').click(function () {
	var form = $('#CategoryAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("TrainingCategory", "Training")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {

					//$('#groupAddForm #companyGroupName').val('');
					Common.Ajax('GET', '/HRPMSEmployee/Training/GetAllTrainingCategories', [], 'json', ajaxTrainingCategories, false);
					$('#trainingCategoryName').val('');
					$('#exampleModalCategory').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

$('#InstituteAddBtn').click(function () {
	var form = $('#InstituteAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("TrainingInstitution", "Training")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {

					//$('#groupAddForm #companyGroupName').val('');
					Common.Ajax('GET', '/HRPMSEmployee/Training/GetAllTrainingInstitute', [], 'json', ajaxTrainingInstitute, false);
					$('#trainingInstituteName').val('');
					$('#exampleModalInstitute').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

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
//other quali

$('#OtherQualiAddBtn').click(function () {
	var form = $('#OtherQualiAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("OtherQualificationHead", "OtherQualifications")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/OtherQualifications/GetAllOtherQualificationHead', [], 'json', ajaxFunctionOtherqualification, false);
					$('#OtherQualificationHeadName').val('');
					$('#exampleModalOtherQuali').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})


function ajaxFunctionOtherqualification(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.name}</option>`
	});
	$('#OtherqualificationHeadId').empty();
	$('#OtherqualificationHeadId').append(`<option value="">Select </option>`);
	$('#OtherqualificationHeadId').append(option);
}


function AddNewOtherQualiModal() {
	$('#exampleModalOtherQuali').modal('show');
}

//qualification
function ajaxFunctionQualifications(response) {
	console.log(response);
	var option = "";
	$('#qualificationHeadId').empty();
	$('#qualificationHeadId').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.name}</option>`
	});

	$('#qualificationHeadId').append(option);

}


$('#QualificationnAddBtn').click(function () {
	var form = $('#QualificationnAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("QualificationHead", "ProfessionalQualifications")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {
					//$('#groupAddForm #Id').val(0);
					//$('#groupAddForm #companyGroupName').val('');
					Common.Ajax('GET', '/HRPMSEmployee/ProfessionalQualifications/GetAllQualificationHead', [], 'json', ajaxFunctionQualifications, false);
					$('#exampleModalQualificationn').modal('hide');
					$('#qualificationHeadName').val('');
				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function AddNewQualificationModal() {
	$('#exampleModalQualificationn').modal('show');
}


//nomeenee
$('#RelationAddBtn').click(function () {
	var form = $('#RelationAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Relation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation, false);
					$('#relationName').val('');
					$('#exampleModalNomineeRelation').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})


$('#RelationAddBtn3').click(function () {
	var form = $('#RelationAddForm3');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Relation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation3, false);
					$('#relationName3').val('');
					$('#exampleModalNomineeRelation3').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})


$('#RelationAddBtn5').click(function () {
	var form = $('#RelationAddForm5');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Relation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation5, false);
					$('#relationName5').val('');
					$('#exampleModalNomineeRelation5').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})


$('#OccupationAddBtn').click(function () {
	var form = $('#OccupationAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Occupation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationN, false);
					$('#occupationName').val('');
					$('#exampleModalNomineeOccupation').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

$('#OccupationAddBtnS').click(function () {
	var form = $('#OccupationAddFormS');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Occupation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationS, false);
					$('#occupationNameS').val('');
					$('#exampleModalNomineeOccupationS').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

$('#OccupationAddBtnC').click(function () {
	var form = $('#OccupationAddFormC');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Occupation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationC, false);
					$('#occupationNameC').val('');
					$('#exampleModalNomineeOccupationC').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})
//education

$('#LevelAddBtn').click(function () {
	var form = $('#LevelAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("LevelEducation", "EducationalQualification")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {

					//$('#groupAddForm #companyGroupName').val('');
					Common.Ajax('GET', '/global/api/levelOfEducations', [], 'json', ajaxLevelOfEducation, false);
					$('#levelofeducationName').val('');
					$('#exampleModalLevel').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

$('#DegreeAddBtn').click(function () {

	var form = $('#DegreeAddForm');

	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Degree", "EducationalQualification")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {

					var id = $("#levelOfEducation").val();
					Common.Ajax('GET', '/global/api/degrees/' + id, [], 'json', ajaxDegrees);
					$('#degreeName').val('');
					$('#levelofeducationId').val('');

					$('#exampleModalDegree').modal('hide');

				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

$('#SubjectAddBtn').click(function () {
	var form = $('#SubjectAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Subject", "EducationalQualification")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {
					//$('#groupAddForm #Id').val(0);
					//$('#groupAddForm #companyGroupName').val('');
					var id = $("#degreeId").val();
					Common.Ajax('GET', '/global/api/relDegreeSubjects/' + id, [], 'json', ajaxRelDegreeSubjects);
					$('#degreeSubjectId').val('');
					$('#subjectId').val('');
					$('#exampleModalSubject').modal('hide');

				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

$('#OrgAddBtn').click(function () {
	var form = $('#OrgAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Organization", "EducationalQualification")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {
					//$('#groupAddForm #Id').val(0);
					//$('#groupAddForm #companyGroupName').val('');
					Common.Ajax('GET', '/global/api/organizations', [], 'json', ajaxOrganization, false);
					$('#organizationType').val('');
					$('#organizationName').val('');
					$('#exampleModalOrg').modal('hide');

				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})


function AddNewLevelModal() {
	$('#exampleModalLevel').modal('show');
}

function AddNewDegreeModal() {

	Common.Ajax('GET', '/HRPMSEmployee/EducationalQualification/GetAllLevelOfEducation', [], 'json', ajaxFunctionLevel, false);

	$('#exampleModalDegree').modal('show');
}

function ajaxFunctionLevel(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.levelofeducationName}</option>`
	});
	$('#levelofeducationId').empty();
	$('#levelofeducationId').append(`<option value="">Select </option>`);
	$('#levelofeducationId').append(option);
}

function AddNewSubjectModal() {
	Common.Ajax('GET', '/HRPMSEmployee/EducationalQualification/GetAllDegree', [], 'json', ajaxFunctionDegree, false);

	$('#exampleModalSubject').modal('show');
}

function ajaxFunctionDegree(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.degreeName}</option>`
	});
	$('#degreeSubjectId').empty();
	$('#degreeSubjectId').append(`<option value="">Select </option>`);
	$('#degreeSubjectId').append(option);
}

function AddNewOrgModal() {
	$('#exampleModalOrg').modal('show');
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
	console.log(response);
	var options = '<option value="">Select </option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.Id + '">' + item.resultName + '</option>';
	});
	$('#resultId').empty();
	$('#resultId').append(options);
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
	console.log('ok')
	console.log(response)
	var option = "";
	$('#reldegreesubjectId').empty();
	$('#reldegreesubjectId').append(`<option value="">Select</option>`);
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.subject.subjectName}</option>`
	});

	$('#reldegreesubjectId').append(option);

}




function spouseFunction() {
	$('#spouceForm')[0].reset();
	// $('#user_img1_Spouce').val('');
	$('#user_img1_Spouce').attr("src", "/images/defaultperson.png");
	$("#SpouseModal").modal('show');
};
function eContactFunction() {
	$('#emergencyContactForm')[0].reset();
	$("#EmergencyContactModal").modal('show');
};
function ChildrenFunction() {
	$('#childrenForm')[0].reset();
	$("#ChildrenModal").modal('show');
	$('#user_img1_children').attr("src", "/images/defaultperson.png");

};
//Nominee
function NomineeFunction() {
	$('#NomineeForm')[0].reset();
	$("#NomineeModal").modal('show');
};
function AddNewNomineeRelationModal() {
	$('#exampleModalNomineeRelation').modal('show');
}
function AddNewRelationModal3() {
	$('#exampleModalNomineeRelation3').modal('show');
}
function AddNewRelationModal5() {
	$('#exampleModalNomineeRelation5').modal('show');
}

function AddNewNomineeOccupationModal() {
	$('#exampleModalNomineeOccupation').modal('show');
}

function AddNewNomineeOccupationModalS() {
	$('#exampleModalNomineeOccupationS').modal('show');
}

function AddNewNomineeOccupationModalC() {
	$('#exampleModalNomineeOccupationC').modal('show');
}

//Insurance
function InsuranceFunction() {
	$('#InsuranceForm')[0].reset();
	$("#InsuranceModal").modal('show');
};
//Education
function EducationalFunction() {
	$('#EducationForm')[0].reset();
	$("#EducationModal").modal('show');
};
function ProfessionalQualificationsFunction() {
	$('#QualificationForm')[0].reset();
	$("#QualificationModal").modal('show');
};
function OtherQualificationsFunction() {
	$('#OtherQualificationnAddForm')[0].reset();
	$("#OtherQualificationsModal").modal('show');
};

function trainingFunction() {
	$('#TrainingAddForm')[0].reset();
	$("#TrainingModal").modal('show');
};
function ServiceFunction() {
	$('#ServiceAddForm')[0].reset();
	$("#ServiceModal").modal('show');
};
function PromotionFunction() {
	$('#PromotionAddForm')[0].reset();
	$("#PromotionModal").modal('show');
};

function PMSHistoryFunction() {
	$('#PMSHistoryAddForm')[0].reset();
	$("#PMSHistoryModal").modal('show');
};

function DisciplinaryFunction() {
	$('#DisciplinaryAddForm')[0].reset();
	$("#DisciplinaryModal").modal('show');
};

function ApprovalFunction() {
	$('#ApprovalAddForm')[0].reset();
	$("#ApprovalModal").modal('show');
};

function DrivingFunction() {
	$('#LicenseAddForm')[0].reset();
	$("#DrivingModal").modal('show');
};
function PassportFunction() {
	$('#PassportAddForm')[0].reset();
	$("#PassportModal").modal('show');
};
function TravelFunction() {
	$('#TravelAddForm')[0].reset();
	$("#TravelModal").modal('show');
};
function MembershipFunction() {
	$('#MembershipAddForm')[0].reset();
	$("#MembershipModal").modal('show');
};
function AwardFunction() {
	$('#AwardAddForm')[0].reset();
	$("#AwardModal").modal('show');
};
function PublicationFunction() {
	$('#PublicationAddForm')[0].reset();
	$("#PublicationModal").modal('show');
};
function LanguageFunction() {
	$('#LanguageAddForm')[0].reset();
	$("#LanguageModal").modal('show');
};




function OtherActivityFunction() {
	$('#OtherActivityAddForm')[0].reset();

	$("#OtherActivityModal").modal('show');
};
function BankInfoFunction() {
	$('#BankInfoAddForm')[0].reset();
	$("#BankInfoModal").modal('show');
};
function BelongingsFunction() {
	$('#BelongingsAddForm')[0].reset();
	$("#BelongingsModal").modal('show');
};
function PreviousEmploymentFunction() {
	$('#PreviousEmploymentAddForm')[0].reset();
	$("#PreviousEmploymentModal").modal('show');
};
function FreedomFighterFunction() {
	$('#FreedomFighterAddForm')[0].reset();
	$("#FreedomFighterModal").modal('show');
};
function ReferenceFunction() {
	$('#ReferenceAddForm')[0].reset();

	$("#ReferenceModal").modal('show');
};
function FighterFunction() {
	$('#FighterAddForm')[0].reset();
	$("#FighterModal").modal('show');
};
function OfficeAssignFunction() {
	$('#OfficeAssignAddForm')[0].reset();
	$("#OfficeAssignModal").modal('show');
};
function EmployeeContractFunction() {
	$('#EmployeeContractAddForm')[0].reset();
	$("#EmployeeContractModal").modal('show');
};
function ProjectActivityFunction() {
	$('#ProjectActivityAddForm')[0].reset();
	$("#ProjectActivityModal").modal('show');
};
function FinanceCodeFunction() {
	$('#FinanceCodeAddForm')[0].reset();

	$("#FinanceCodeModal").modal('show');
};
function AttachmentFunction() {
	$('#AttachmentAddForm')[0].reset();
	$("#AttachmentModal").modal('show');
};
function ProjectAssignFunction() {
	$('#ProjectAssignAddForm')[0].reset();
	$("#ProjectAssignModal").modal('show');
};
function EmployeeOtherInfoFunction() {
	$('#EmployeeOtherInfoAddForm')[0].reset();

	$("#EmployeeOtherInfoModal").modal('show');
};
function CostCentreFunction() {
	$('#CostCentreAddForm')[0].reset();
	$("#CostCentreModal").modal('show');
};
function EmployeeGradeFunction() {
	$('#EmployeeGradeAddForm')[0].reset();

	$("#EmployeeGradeModal").modal('show');
};
function IeltsInfoFunction() {
	$('#IeltsInfoAddForm')[0].reset();
	$("#IeltsInfoModal").modal('show');
};
function ComputerLiteracyFunction() {
	$('#ComputerLiteracyAddForm')[0].reset();
	$("#ComputerLiteracyModal").modal('show');
};




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
	console.log(response);
	var options = '<option value="">Select</option>';
	$.each(response, function (i, item) {
		options += '<option value="' + item.pNS.Id + '">' + item.pNS.name + '</option>';
	});
	$('#pns').empty();
	$('#pns').append(options);
}

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
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.occupationName}">${item.occupationName}</option>`
	});
	$('#Nomineeoccupation').empty();
	$('#Nomineeoccupation').append(`<option value="">Select </option>`);
	$('#Nomineeoccupation').append(option);
}
function ajaxFunctiOccupationS(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#Soccupation').empty();
	$('#Soccupation').append(`<option value="">Select </option>`);
	$('#Soccupation').append(option);
}
function ajaxFunctiOccupationC(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#Coccupation').empty();
	$('#Coccupation').append(`<option value="">Select </option>`);
	$('#Coccupation').append(option);
}

function ajaxFunctiRelation(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.relationName}">${item.relationName}</option>`
	});
	$('#Nomineerelation').empty();
	$('#Nomineerelation').append(`<option value="">Select </option>`);
	$('#Nomineerelation').append(option);
}
function ajaxFunctiRelation3(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.relationName}">${item.relationName}</option>`
	});
	$('#relation3').empty();
	$('#relation3').append(`<option value="">Select </option>`);
	$('#relation3').append(option);
}
function ajaxFunctiRelation5(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.relationName}">${item.relationName}</option>`
	});
	$('#relation5').empty();
	$('#relation5').append(`<option value="">Select </option>`);
	$('#relation5').append(option);
}

function ajaxNomineeDistributedPercent(response) {
	console.log(response);
	var distributed = parseFloat(response);
	$("#remaining").val(100 - distributed);
}

var index = 1;
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

function RemoveFromTable(indx) {
	$('#fundValueListTable tbody #record-' + indx).remove();
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
		console.log("ok")
		console.log(value)
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

//15/06

function AddNewOccupationModal() {
	$('#exampleModalOccupation2').modal('show');
}
function AddNewOccupationModal1() {
	$('#exampleModalOccupation1').modal('show');
}
function AddNewOccupationModal2() {
	$('#exampleModalOccupation2').modal('show');
}

function AddNewjoiningDesignationModal() {
	$('#exampleModaljoiningDesignation').modal('show');
}

function AddNewLocationModal() {
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllCompany', [], 'json', ajaxFunctiCompany, false);
	$('#exampleModalLocation').modal('show');
}

function AddNewFunctionModal() {
	Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllCompany', [], 'json', ajaxFunctiCompany1, false);
	$('#exampleModalFunction').modal('show');
}
function AddNewDepartmentModal() {
	$('#exampleModalDepartment').modal('show');
}
function AddNewUnitModal() {
	$('#exampleModalUnit').modal('show');
}
function AddNewBranchModal() {
	$('#exampleModalBranch').modal('show');
}
function AddNewProgramModal() {
	$('#exampleModalProgram').modal('show');
}

$('#OccupationAddBtn2').click(function () {
	var form = $('#OccupationAddForm2');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Occupation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupation, false);
					$('#Occupation').val('');
					$('#exampleModalOccupation2').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFunctiOccupation(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#fatherOccupationId').empty();
	$('#fatherOccupationId').append(`<option value="">Select </option>`);
	$('#fatherOccupationId').append(option);
}
function ajaxFunctiCompany(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.companyName}</option>`
	});
	$('#companyIdL').empty();
	$('#companyIdL').append(`<option value="">Select </option>`);
	$('#companyIdL').append(option);
}

function ajaxFunctiCompany1(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.companyName}</option>`
	});
	$('#companyIdF').empty();
	$('#companyIdF').append(`<option value="">Select </option>`);
	$('#companyIdF').append(option);
}

$("#homeDistrict").select2();
$("#fatherOccupationId").select2();
$('#OccupationAddBtn1').click(function () {
	fatherOccupationId
	var form = $('#OccupationAddForm1');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Occupation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupation1, false);
					$('#Occupation1').val('');
					$('#exampleModalOccupation1').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFunctiOccupation1(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#motherOccupationId').empty();
	$('#motherOccupationId').append(`<option value="">Select </option>`);
	$('#motherOccupationId').append(option);
}

$('#OccupationAddBtn2').click(function () {
	fatherOccupationId
	var form = $('#OccupationAddForm2');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Occupation", "Nominee")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupation2, false);
					$('#Occupation2').val('');
					$('#exampleModalOccupation2').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFunctiOccupation2(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.occupationName}</option>`
	});
	$('#occupation3').empty();
	$('#occupation3').append(`<option value="">Select </option>`);
	$('#occupation3').append(option);
}

$("#occupation3").select2();
$("#motherOccupationId").select2();
$('#joiningDesignationAddBtn').click(function () {
	var form = $('#joiningDesignationAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("JoiningDesignation", "Info")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllJoiningDesignation', [], 'json', ajaxFunctijoiningDesignation, false);
					$('#designationName').val('');
					$('#exampleModaljoiningDesignation').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})



function ajaxFunctijoiningDesignation(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.designationName}">${item.designationName}</option>`
	});
	$('#joiningDesignation').empty();
	$('#joiningDesignation').append(`<option value="">Select </option>`);
	$('#joiningDesignation').append(option);
}
$("#joiningDesignation").select2();

$('#LocationAddBtn').click(function () {
	var form = $('#LocationAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Location", "Info")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllLocation', [], 'json', ajaxFuncLocation, false);
					$('#LocationName').val('');
					$('#branchCode').val('');
					$('#exampleModalLocation').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFuncLocation(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.branchUnitName}</option>`
	});
	$('#locationId').empty();
	$('#locationId').append(`<option value="">Select </option>`);
	$('#locationId').append(option);
}
$("#locationId").select2();

$('#FunctionAddBtn').click(function () {
	var form = $('#FunctionAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Function", "Info")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllFunction', [], 'json', ajaxFunctiFunction, false);
					$('#LocationNameF').val('');
					$('#branchCodeF').val('');
					$('#exampleModalFunction').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFunctiFunction(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.branchUnitName}</option>`
	});
	$('#functionInfoId').empty();
	$('#functionInfoId').append(`<option value="">Select </option>`);
	$('#functionInfoId').append(option);
}

$("#functionInfoId").select2();
$('#DepartmentAddBtn').click(function () {
	var form = $('#DepartmentAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Department", "Info")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllDepartment', [], 'json', ajaxFunctiDepartment, false);
					$('#LocationNameD').val('');
					$('#branchCodeD').val('');
					$('#exampleModalDepartment').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFunctiDepartment(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.deptName}</option>`
	});
	$('#departmentId').empty();
	$('#departmentId').append(`<option value="">Select </option>`);
	$('#departmentId').append(option);
}

$("#departmentId").select2();
$('#UnitAddBtn').click(function () {
	var form = $('#UnitAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Unit", "Info")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllUnit', [], 'json', ajaxFunctiUnit, false);
					$('#LocationNameU').val('');
					$('#exampleModalUnit').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFunctiUnit(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.unitName}</option>`
	});
	$('#hrUnitId').empty();
	$('#hrUnitId').append(`<option value="">Select </option>`);
	$('#hrUnitId').append(option);
}

$("#hrUnitId").select2();

$('#BranchAddBtn').click(function () {
	var form = $('#BranchAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Branch", "Info")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllBranch', [], 'json', ajaxFunctiBranch, false);
					$('#LocationNameB').val('');
					$('#branchCodeB').val('');
					$('#exampleModalBranch').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFunctiBranch(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.branchName}</option>`
	});
	$('#hrBranchId').empty();
	$('#hrBranchId').append(`<option value="">Select </option>`);
	$('#hrBranchId').append(option);
}
function ajaxFunctiBranchType(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.name}</option>`
	});
	$('#branchCodeB').empty();
	$('#branchCodeB').append(`<option value="">Select </option>`);
	$('#branchCodeB').append(option);
}
$("#hrBranchId").select2();

$('#ProgramAddBtn').click(function () {
	var form = $('#ProgramAddForm');
	swal({
		title: 'Are you sure?', text: 'Do you want to add this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#007ACC', cancelButtonColor: '#d33', confirmButtonText: 'Yes, Add it!'
	}).then(function () {
		$.ajax({
			url: '@Url.Action("Program", "Info")',
			type: 'Post',
			data: form.serialize()
		})
			.done(function () {
				swal('Success', 'Saved Successfully!', 'success').then(function () {


					Common.Ajax('GET', '/HRPMSEmployee/Info/GetAllProgram', [], 'json', ajaxFunctiProgram, false);
					$('#LocationNameP').val('');
					$('#exampleModalProgram').modal('hide');


				});
			})
			.fail(function () {
				alert("Unable to Save. Please Try Again");
			});
	});
})

function ajaxFunctiProgram(response) {
	console.log(response)
	var option = "";
	$.each(response, function (i, item) {
		option += `<option value="${item.Id}">${item.programName}</option>`
	});
	$('#hrProgramId').empty();
	$('#hrProgramId').append(`<option value="">Select </option>`);
	$('#hrProgramId').append(option);
}
$("#hrProgramId").select2();