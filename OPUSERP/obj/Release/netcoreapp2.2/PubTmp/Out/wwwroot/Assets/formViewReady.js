$(document).ready(function () {

    $('#queryString').val(pathname);
    $('#Vegiterian').change(function () {
        if ($(this).is(":checked")) {
            $(this).val(1);
        }
        else {
            $(this).val(0);
        }
    })

    $('#NonVegiterian').change(function () {
        if ($(this).is(":checked")) {
            $(this).val(1);
        }
        else {
            $(this).val(0);
        }
    })

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
	

	$("#hobbyName6").hide();
	$("#AddNewLevelModalHobby").hide();

	$("#personal").addClass("active");

	
	$(document).on('click', '#del2', function () {
		$(this).closest('.bb').toggleClass('strike').fadeOut('slow', function () {
			$(this).remove();
		});
	});

	$(document).on('click', '#del3', function () {
		$(this).closest('.cc').toggleClass('strike').fadeOut('slow', function () {
			$(this).parent('.col-md-3').remove();
		});
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

	

	$("#Spousesubmit").click(function () {
		swal({
			title: 'Are you sure?', text: 'Do you want to submit this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#3085d6', cancelButtonColor: '#d33', confirmButtonText: 'Yes, submit it!'
		}).then(function () {
			$("#Spousesubmitbtn").click();
		});
	});

	$('#hobbyName6').click(function () {
		$(this).val('').focus();
	})

	$("#approverDiv").click(function () {
		$("#AddNew2").hide();
		$("#hobbyName6").show();
		$('#hobbyName6').focus();
		$("#AddNewLevelModalHobby").show();
	});

	$("#submit").click(function () {
		swal({
			title: 'Are you sure?', text: 'Do you want to submit this record!', type: 'warning', showCancelButton: true, confirmButtonColor: '#3085d6', cancelButtonColor: '#d33', confirmButtonText: 'Yes, submit it!'
		}).then(function () {
			$("#submitbtn").click();
		});
	});

	



	//Select2
	$(".select2").select2();

	$("#homeDistrict").select2();
	$("#fatherOccupationId").select2();
	$("#occupation3").select2();
	$("#motherOccupationId").select2();
	$("#joiningDesignation").select2();
	$("#locationId").select2();
	$("#functionInfoId").select2();
	$("#hrUnitId").select2();
	$("#hrBranchId").select2();
	$("#hrProgramId").select2();



	//datepicker
	$('.datepicker').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });

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
	$('#nomineeDate').datepicker({ dateFormat: "dd-M-yy", showAnim: "scale", changeMonth: true, changeYear: true, yearRange: "1940:2030" });
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

	Common.Ajax('GET', '/global/api/GetAllUserEmployeeInfo', [], 'json', ajaxGetAllEmployee);
	Common.Ajax('GET', '/global/api/GetAllUserEmployeeInfo', [], 'json', ajaxGetAllEmployee1);
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
	Common.Ajax('GET', '/HRPMSEmployee/OtherQualifications/GetAllOtherQualificationHead', [], 'json', ajaxFunctionOtherqualification, false);
	Common.Ajax('GET', '/HRPMSEmployee/ProfessionalQualifications/GetAllQualificationHead', [], 'json', ajaxFunctionQualifications, false);
	//training
	Common.Ajax('GET', '/HRPMSEmployee/Training/GetAllTrainingCategories', [], 'json', ajaxTrainingCategories, false);
	Common.Ajax('GET', '/HRPMSEmployee/Training/GetAllTrainingInstitute', [], 'json', ajaxTrainingInstitute, false);
	Common.Ajax('GET', '/HRPMSEmployee/EmployeeAttachment/GetAllGroup', [], 'json', ajaxFunctiGroup, false);
	Common.Ajax('GET', '/HRPMSEmployee/Membership/GetMembershipOrganization', [], 'json', ajaxFunctiRelation2, false);
	Common.Ajax('GET', '/global/api/GetActiveAllEmployeeInfo', [], 'json', ajaxGetSupervisors);
	Common.Ajax('GET', '/global/api/GetActiveAllEmployeeInfo', [], 'json', ajaxGetApprovers);
	//address
	Common.Ajax('GET', '/global/api/divisions/1', [], 'json', ajaxDivision, false);
	Common.Ajax('GET', '/global/api/divisions/1', [], 'json', ajaxPermanentDivision, false);
	//Nominee
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationN, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationS, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllOccupation', [], 'json', ajaxFunctiOccupationC, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation3, false);
	Common.Ajax('GET', '/HRPMSEmployee/Nominee/GetAllRelation', [], 'json', ajaxFunctiRelation5, false);
	//Education
	Common.Ajax('GET', '/global/api/levelOfEducations', [], 'json', ajaxLevelOfEducation, false);
	Common.Ajax('GET', '/global/api/organizations', [], 'json', ajaxOrganization, false);
	Common.Ajax('GET', '/global/api/results', [], 'json', ajaxResult, false);


	$("#dateOfBirth").change(function () {
		var dob = $("#dateOfBirth").val();
		//$("#dateOfLPR").val(dob);
	});

	$("#SpouceimagePath1").change(function () {
		readURL1Spouce(this);
	});

	$("#childrenimagePath1").change(function () {
		readURL1(this);
	});

	$("#NomineeimagePath1").change(function () {
		readURL1Nominee(this);
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

	$("#levelOfEducation").change(function () {
		var id = $(this).val();
		Common.Ajax('GET', '/global/api/degrees/' + id, [], 'json', ajaxDegrees);
	});

	$("#degreeId").change(function () {
		var id = $(this).val();
		Common.Ajax('GET', '/global/api/relDegreeSubjects/' + id, [], 'json', ajaxRelDegreeSubjects);
	});

	$("#hRFacilityId33").change(function () {
		var hRFacilityId = $('#hRFacilityId33').val();
		Common.Ajax('GET', '/HRPMSEmployee/EmployeeProjectActivity/GetHRFacilityById/?id=' + hRFacilityId, [], 'json', ajaxGetHRFacilityById);
	});

	$("#atttachmentGroupId").change(function () {
		var atttachmentGroupId = $('#atttachmentGroupId').val();
		Common.Ajax('GET', '/HRPMSEmployee/EmployeeAttachment/GetAllAttachmentCategoryByGroupId/?masterId=' + atttachmentGroupId, [], 'json', ajaxGetAllAttachmentCategoryByGroupId);
	});

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
	$('#favouriteColor').change(function () {
		$('#fillColor').css('background-color', $(this).val())
	})
	

	

})