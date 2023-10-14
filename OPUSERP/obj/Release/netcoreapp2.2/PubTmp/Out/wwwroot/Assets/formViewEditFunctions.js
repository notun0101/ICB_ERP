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
