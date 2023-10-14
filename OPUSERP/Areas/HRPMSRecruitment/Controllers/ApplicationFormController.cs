using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSRecruitment.Models;
using OPUSERP.HRPMS.Data.Entity.Recruitment;
using OPUSERP.HRPMS.Services.Recruitment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Helpers;
using Microsoft.AspNetCore.Hosting;
using DinkToPdf.Contracts;
using System.IO;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.HRPMSRecruitment.Controllers
{
	[Authorize]
	[Area("HRPMSRecruitment")]
	public class ApplicationFormController : Controller
	{
		private readonly IApplicationFormService applicationFormService;
		private readonly IReligionMunicipalityService religionMunicipalityService;
		private readonly IAddressService addressService;
        private readonly IHRPMSAttachmentService hRPMSAttachmentService;
        private readonly string rootPath;
		private readonly MyPDF myPDF;

		public ApplicationFormController(IHostingEnvironment hostingEnvironment, IConverter converter, IApplicationFormService applicationFormService, IReligionMunicipalityService religionMunicipalityService, IAddressService addressService , IHRPMSAttachmentService hRPMSAttachmentService)
		{
			this.applicationFormService = applicationFormService;
			this.hRPMSAttachmentService = hRPMSAttachmentService;
			this.religionMunicipalityService = religionMunicipalityService;
			this.addressService = addressService;
			myPDF = new MyPDF(hostingEnvironment, converter);
			rootPath = hostingEnvironment.ContentRootPath;
		}

		// GET: ApplicationForm
		public async Task<IActionResult> IndexView(int id)
		{

			ViewBag.employeeID = id.ToString();
			ApplicationFormViewModel model = new ApplicationFormViewModel
			{
				ApplicationId = id,
				applicationForms = await applicationFormService.GetApplicationForm(),
				Religion = await religionMunicipalityService.GetReligions(),
				ApplicantAddress = await applicationFormService.GetApplicantAddress(),
				applicationForm = await applicationFormService.GetApplicationFormById(id),
				present = await applicationFormService.GetAddressByTypeAndapplicationFormId(id, "present"),
				permanent = await applicationFormService.GetAddressByTypeAndapplicationFormId(id, "permanent"),
				applicationEdu = await applicationFormService.GetApplicationEduByapplicationFormId(id),
				applicationExps = await applicationFormService.GetApplicationExpByapplicationFormId(id),
				ApplicationExp = await applicationFormService.GetApplicationExpByapplicationFormId1(id),
				ApplicationEdu = await applicationFormService.GetEduByapplicationFormId(id),
				applicationQuota = await applicationFormService.GetApplicationQuotaByapplicationFormId(id),
			};

			if (model.permanent == null) model.permanent = new ApplicantAddress();
			if (model.present == null) model.present = new ApplicantAddress();
			model.present.divisionId = model.present.divisionId ?? 0;
			model.present.districtId = model.present.districtId ?? 0;

			model.permanent.divisionId = model.permanent.divisionId ?? 0;
			model.permanent.districtId = model.permanent.districtId ?? 0;
			return View(model);
		}

		// GET: ApplicationForm
		public async Task<IActionResult> Index()

		{
			var applicants = await applicationFormService.GetApplicationForm();
			var _applicantsNo = "App-" + applicants.Count() + 1;

			ApplicationFormViewModel model = new ApplicationFormViewModel
			{
				applicationForms = applicants,
				applicantsNo = _applicantsNo,
				Religion = await religionMunicipalityService.GetReligions(),
				Countries = await addressService.GetAllContry(),
				ApplicantAddress = await applicationFormService.GetApplicantAddress(),
				//  present = await applicationFormService.GetAddressByTypeAndapplicationFormId(id, "present"),
				//  permanent = await applicationFormService.GetAddressByTypeAndapplicationFormId(id, "permanent"),
				//  applicationEdu = await applicationFormService.GetApplicationEduByapplicationFormId(id),
				// applicationExps = await applicationFormService.GetApplicationExpByapplicationFormId(id),
				// applicationQuota = await applicationFormService.GetApplicationQuotaByapplicationFormId(id),
			};

			if (model.permanent == null) model.permanent = new ApplicantAddress();
			if (model.present == null) model.present = new ApplicantAddress();
			model.present.divisionId = model.present.divisionId ?? 0;
			model.present.districtId = model.present.districtId ?? 0;

			model.permanent.divisionId = model.permanent.divisionId ?? 0;
			model.permanent.districtId = model.permanent.districtId ?? 0;
			return View(model);
		}

        public async Task<IActionResult> PresentApplicationAddress(int id)
        {

            return Json( await applicationFormService.GetAddressByTypeAndapplicationFormId(id, "present"));
        }

        public async Task<IActionResult> PermanentApplicationAddress(int id)
        {

            return Json( await applicationFormService.GetAddressByTypeAndapplicationFormId(id, "permanent"));
        }


         public async Task<IActionResult> ApplicationFrom(int id)
        {

            return Json( await applicationFormService.GetApplicationFormById(id));
        }




        // POST: CreateJobCircular/Index
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index([FromForm] ApplicationFormViewModel model)
		{
			var applicants = await applicationFormService.GetApplicationForm();
			if (!ModelState.IsValid)
			{
				model.applicationForms = applicants;
				return View(model);
			}
			var _applicantsNo = "App-" + applicants.Count() + 1;
			if (model.ApplicationId > 0)
			{
				_applicantsNo = model.applicantsNo;
			}

			string fileName;
			string message;
			if (model.payDoc != null)
			{
				message = FileSave.SaveImageNew(out fileName, model.payDoc);
			}
			else
			{
				fileName = await applicationFormService.GetApplicationFormpayDocById(model.ApplicationId);
			};

			string fileName1;
			string message1;
			if (model.applicantPhoto != null)
			{
				message1 = FileSave.SaveImageNew(out fileName1, model.applicantPhoto);
			}
			else
			{
				fileName1 = await applicationFormService.GetApplicationFormapplicantPhotoById(model.ApplicationId);
			};

			string fileName2;
			string message2;
			if (model.applicantSignature != null)
			{
				message2 = FileSave.SaveImageNew(out fileName2, model.applicantSignature);
			}
			else
			{
				fileName2 = await applicationFormService.GetApplicationFormSignatureById(model.ApplicationId);
			};


			try
			{


				ApplicationForm data = new ApplicationForm
				{
					Id = model.ApplicationId,
					applicationNo = _applicantsNo,
					nameBN = model.nameBN,
					nameEN = model.nameEN,
					nidNO = model.nidNO,
					binNO = model.binNO,
					birthDate = Convert.ToDateTime(model.birthDate),
					birtPlace = model.birtPlace,
					payRef = model.payRef,
					fNmaeBN = model.fNmaeBN,
					fNmaeEN = model.fNmaeEN,
					mNmaeBN = model.mNmaeBN,
					mNmaeEN = model.mNmaeEN,
					sNameBN = model.sNameBN,
					sNameEN = model.sNameEN,
					mobile = model.mobile,
					email = model.email,
					nationality = model.nationality,
					religionId = model.religionId,
					occupation = model.occupation,
					gender = model.gender,
					payDoc = fileName,
					applicantPhoto = fileName1,
					applicantSignature = fileName2,
					status = 1,
					type = ""
				};

				int applicationFormId = await applicationFormService.SaveApplicationForm(data);

				ApplicantAddress Presentdata = new ApplicantAddress
				{
					Id = model.PresentAdressId,
					countryId = 1,
					divisionId = model.divisionId,
					districtId = model.districtId,
					thanaId = model.thanaId,
					union = model.union,
					postOffice = model.postOffice,
					postCode = model.postCode,
					blockSector = model.blockSector,
					houseVillage = model.houseVillage,
					roadNumber = model.roadNumber,
					applicationFormId = applicationFormId,
					type = "present"

				};

				await applicationFormService.SaveApplicantAddress(Presentdata);

				ApplicantAddress Permanentdata = new ApplicantAddress
				{
					Id = model.PermanentAdressId,
					countryId = 1,
					divisionId = model.divisionIdp,
					districtId = model.districtIdp,
					thanaId = model.thanaIdp,
					union = model.unionp,
					postOffice = model.postOfficep,
					postCode = model.postCodep,
					blockSector = model.blockSectorp,
					houseVillage = model.houseVillagep,
					roadNumber = model.roadNumberp,
					applicationFormId = applicationFormId,
					type = "permanent"

				};

				await applicationFormService.SaveApplicantAddress(Permanentdata);

				if (model.EduId != null)
				{
					for (int i = 0; i < model.EduId.Length; i++)
					{
						var Edu = new ApplicationEdu
						{
							Id = (int)model.EduId[i],
							applicationFormId = applicationFormId,
							institution = model.institution[i],
							degree = model.degree[i],
							boardUniversity = model.boardUniversity[i],
							rollNo = model.rollNo[i],
							country = model.country[i],
							yearOfCertification = model.yearOfCertification[i],
							mainSubject = model.mainSubject[i],
							gpaDivision = model.gpaDivision[i],
							//type = model.type1[i],
						};

						await applicationFormService.SaveApplicationEdu(Edu);

					}
				}

				if (model.ExpId != null)
				{
					for (int i = 0; i < model.ExpId.Length; i++)
					{
						var Exp = new ApplicationExp
						{
							Id = (int)model.ExpId[i],
							applicationFormId = applicationFormId,
							officeName = model.officeName[i],
							position = model.position[i],
							from = model.from[i],
							to = model.to[i],
						};

						await applicationFormService.SaveApplicationExp(Exp);

					}
				}



				string fileName3;
				string message3;
				if (model.document != null)
				{
					message3 = FileSave.SaveImageNew(out fileName3, model.document);
				}
				else
				{
					fileName3 = await applicationFormService.GetApplicationFormSignatureById(model.ApplicationId);
				};

				ApplicationQuota data3 = new ApplicationQuota
				{
					Id = (int)model.QuotaIdr,
					isFredomFighter = model.isFredomFighter,
					relation = model.relation,
					document = fileName3,
					isDisable = model.isDisable,
					isTrible = model.isTrible,
					isAnser = model.isAnser,
					isOther = model.isOther,
					otherQuotaName = model.otherQuotaName,
					otherQuotaDoc = model.otherQuotaDoc,
					applicationFormId = applicationFormId,

				};

				await applicationFormService.SaveApplicationQuota(data3);

			}
			catch (Exception ex)
			{

				throw;
			}
			return RedirectToAction(nameof(Index));
		}



        public async Task<IActionResult> ApplicantsNewList()
        {
            var applicants = await applicationFormService.GetApplicationForm();
            var _applicantsNo = "App-" + applicants.Count() + 1;

            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = applicants,
              
            };
            return View(model);
        }

        public async Task<IActionResult> joiningAttachment(int id)
        {
            var applicants = await applicationFormService.GetApplicationForm();
            var _applicantsNo = "App-" + applicants.Count() + 1;
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = applicants,
              
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> joiningAttachment([FromForm] ApplicationFormViewModel model)
        {


            string fileName;
            string message;
            if (model.JoiningAttachmentUrl != null)
            {
                message = FileSave.SaveEmpAttachmentNew(out fileName, model.JoiningAttachmentUrl);
            }
            else
            {
                fileName = await applicationFormService.GetJoiningAttachmentUrlById(model.ApplicationId);
            };
            
            var data = await applicationFormService.GetApplicationFormById(model.ApplicationId);
            data.attachmentUrl = fileName;
            data.status = 8;
            data.joiningDate = model.joiningDate;

            await applicationFormService.SaveJoininigAttachment(data);
            return RedirectToAction("ApplicantsNewList");
        }



        public async Task<IActionResult> ApplicantsNew()
		{
			var applicants = await applicationFormService.GetApplicationForm();
			var _applicantsNo = "App-" + applicants.Count() + 1;

			ApplicationFormViewModel model = new ApplicationFormViewModel
			{
				applicationForms = applicants,
				applicantsNo = _applicantsNo,
				Religion = await religionMunicipalityService.GetReligions(),
				Countries = await addressService.GetAllContry(),
				ApplicantAddress = await applicationFormService.GetApplicantAddress(),
                salaryGrades = await applicationFormService.GetAllSalaryGrade()

            };
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ApplicantsNew(ApplicationFormViewModel model)
		{
            var fullDate = model.banglaDate + " " + model.banglaMonth + " " + model.banglaYear;
            var data = new ApplicationForm
			{
				Id = model.ApplicationId,
				applicationNo = model.applicantsNo,
				nameEN = model.nameEN,
				nameBN = model.nameBN,
				fNmaeEN = model.fNmaeEN,
				fNmaeBN = model.fNmaeBN,
				mNmaeEN = model.mNmaeEN,
                birthDate=model.birthDate,

                salaryGradeId = model.salaryGradeId,
                payRef = model.payRef,
                nidNO = model.nidNO,
                mNmaeBN = model.mNmaeBN,
                //status = 7,

                addressBangla = model.addressBangla,
				postBangla = model.postBangla,
				mailingAddress = model.mailingAddress,

				mobile = model.mobile,
				email = model.email,
				nationality = model.nationality,
				gender = model.gender,
				termsAndConditions = model.termsAndConditions,
				occupation = model.occupation,
                reportDateBn = fullDate,
                reportDateEn = model.reportDateEn,
            };

        

            var applicationFormId = await applicationFormService.SaveApplicationForm(data);

			ApplicantAddress Presentdata = new ApplicantAddress
			{
				Id = model.PresentAdressId,
				countryId = 1,
				divisionId = model.divisionId,
				districtId = model.districtId,
				thanaId = model.thanaId,
				union = model.union,
				postOffice = model.postOffice,
				postCode = model.postCode,
				blockSector = model.blockSector,
				houseVillage = model.houseVillage,
				roadNumber = model.roadNumber,
				applicationFormId = applicationFormId,
				type = "present"

			};
			await applicationFormService.SaveApplicantAddress(Presentdata);

			ApplicantAddress Permanentdata = new ApplicantAddress
			{
				Id = model.PermanentAdressId,
				countryId = 1,
				divisionId = model.divisionIdp,
				districtId = model.districtIdp,
				thanaId = model.thanaIdp,
				union = model.unionp,
				postOffice = model.postOfficep,
				postCode = model.postCodep,
				blockSector = model.blockSectorp,
				houseVillage = model.houseVillagep,
				roadNumber = model.roadNumberp,
				applicationFormId = applicationFormId,
				type = "permanent"

			};
			await applicationFormService.SaveApplicantAddress(Permanentdata);

			return RedirectToAction("ApplicantsNew");
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> AppointmentLetterView(int ida)
		{
			var model = new ApplicationFormViewModel
			{
				applicationForm = await applicationFormService.GetApplicationFormById(ida),
				present = await applicationFormService.GetAddressByTypeAndapplicationFormId(ida, "present"),
				permanent = await applicationFormService.GetAddressByTypeAndapplicationFormId(ida, "permanent")
			};

            var appForm = await applicationFormService.GetApplicationFormById(ida);
            appForm.status = 7;
            await applicationFormService.SaveApplicationForm(appForm);


            return View(model);
		}

		public IActionResult AppointmentLetterPdf(int ida)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;

			string url = scheme + "://" + host + "/HRPMSRecruitment/ApplicationForm/AppointmentLetterView?ida=" + ida;

			string fileName;
			string status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmationLetterView(int ida)
        {
            var model = new ApplicationFormViewModel
            {
                applicationForm = await applicationFormService.GetApplicationFormById(ida),
                present = await applicationFormService.GetAddressByTypeAndapplicationFormId(ida, "present"),
                permanent = await applicationFormService.GetAddressByTypeAndapplicationFormId(ida, "permanent")
            };

            var appForm = await applicationFormService.GetApplicationFormById(ida);
            appForm.status = 7;
            await applicationFormService.SaveApplicationForm(appForm);


            return View(model);
        }
        [AllowAnonymous]
        public IActionResult ConfirmationLetterPdf(int ida)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;

			string url = scheme + "://" + host + "/HRPMSRecruitment/ApplicationForm/ConfirmationLetterView?ida=" + ida;

			string fileName;
			string status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

        public async Task<IActionResult> LetterList()
        {
          
            return View();
        }

         public async Task<IActionResult>PoliceVerificationPending()
        {
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {

                hRPMSAttachments = await hRPMSAttachmentService.GetHRPMSAttachmentByFile(),
                
            };
            return View(model);

           // return View();
        }



    }
}