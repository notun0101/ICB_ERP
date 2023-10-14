using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class ApprovalController : Controller
    {
       
        private readonly IApprovalService approvalService;
        private readonly IEmployeeInfoService employeeInfoService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;


        public ApprovalController(IHostingEnvironment hostingEnvironment, IEmployeeInfoService employeeInfoService, IApprovalService approvalService, IPersonalInfoService personalInfoService, IPhotographService photographService)
        {
            this.approvalService = approvalService;
            this.personalInfoService = personalInfoService;
            this.employeeInfoService = employeeInfoService;
            this.photographService = photographService;

        }

        public async Task<IActionResult> Index(int? id)
        {
            //var empId = await approvalService.GetApprovalMasterByEmpId(Convert.ToInt32(id));
            //var data = new ApprovarsWithEmp
            //{
            //	masterId = empId.Id,
            //	EmpId = empId.employeeInfo.Id,
            //	EmpCode = empId.employeeInfo.employeeCode,
            //	EmpName = empId.employeeInfo.nameEnglish,
            //	approvarsViewModels = await approvalService.GetApprovalDetailByApprovalMasterId(empId.Id)
            //}
			ApprovalViewModel model = new ApprovalViewModel
            {
				employeeInfo = await approvalService.GetEmployeeInfoById(Convert.ToInt32(id)),
               
                approvalTypes = await approvalService.GetAllApprovalType(),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                photograph = await photographService.GetPhotographByEmpIdAndType(Convert.ToInt32(id), "profile"),
            };
			if (id != null)
			{
				model.approvalMasters = await approvalService.GetApprovalMaster(Convert.ToInt32(id));
			}
			else
			{
				model.approvalMasters = await approvalService.GetAllApprovalMaster();
			}
			return View(model);
        }

        // POST: Approval
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] ApprovalViewModel model)
        {
            var approvalMaster = await approvalService.GetApprovalMasterByEmpId(Convert.ToInt32(model.employeeInfoId));
            var MasterId = 0;
            if (approvalMaster != null)
            {
                MasterId = approvalMaster.Id;
            }
            else
            {
                ApprovalMaster master = new ApprovalMaster
                {
                    Id = Convert.ToInt32(model.approvalMasterId),
                    employeeInfoId = model.employeeInfoId,
                    approvalTypeId = model.approvalTypeId,

                };
                MasterId = await approvalService.SaveApprovalMaster(master);

            }

            if (model.approvalMasterId > 0)
            {
                await approvalService.DeleteapprovalDetailsByApprovalMasterId(Convert.ToInt32(MasterId));
            }
            if (model.approverId != null)
            {
                if (model.approverId.Length > 0)
                {

                    for (int i = 0; i < model.approverId.Length; i++)
                    {
                        ApprovalDetail detail = new ApprovalDetail();

                        detail.Id = 0;
                        detail.approvalMasterId = MasterId;
                        detail.approverId = Convert.ToInt32(model.approverId[i]);
                        detail.sortOrder = model.sortOrder[i];
                        detail.status = model.status[i];
						detail.isDelete = model.canFinalApprover[i];

						await approvalService.SaveApprovalDetail(detail);
                        detail = new ApprovalDetail();
                    }

                }
            }

			return Json(model.employeeInfoId);
		}
        [HttpGet]

        public async Task<IActionResult> GetDesDeptOfApprover(int id)
		{
			var model = new ApprovalViewModel
			{
				employeeInfo = await approvalService.GetEmployeeInfoById(id)
			};
			return Json(model);
		}

        [HttpGet]
        public async Task<IActionResult> GetApprovalMasterByApprovalMasterId(int ApprovalMasterId)
        {
            return Json(await approvalService.GetApprovalMasterByApprovalMasterId(ApprovalMasterId));
        }
        [HttpGet]
        public async Task<IActionResult> GetApprovalDetailByApprovalMasterId(int ApprovalMasterId)
        {
            return Json(await approvalService.GetApprovalDetailByApprovalMasterId(ApprovalMasterId));
        }
        public async Task<IActionResult> ApprovalType()
        {
            ApprovalTypeViewModel model = new ApprovalTypeViewModel
            {
                approvalTypes = await approvalService.GetAllApprovalType(),
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovalType([FromForm] ApprovalTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.approvalTypes = await approvalService.GetAllApprovalType();
                return View(model);
            }

            ApprovalType data = new ApprovalType
            {
                Id = (int)model.approvalTypeId,
                approvalTypeName = model.approvalTypeName
            };

            await approvalService.SaveApprovalType(data);

            return RedirectToAction(nameof(ApprovalType));
        }



        //[HttpPost]
        //public async Task<IActionResult> Delete(int Id)
        //{
        //    await approvalService.DeleteApprovalTypeById(Id);
        //    // return Json(true);
        //    return RedirectToAction(nameof(ApprovalType));
        //}

        public async Task<JsonResult> Delete(int Id)
        {
            await approvalService.DeleteApprovalTypeById(Id);
            return Json(true);
        }

		public IActionResult DeleteApprovalById(int id)
		{
			var data = approvalService.DeleteApprovalMasterForcelyById(id);
			return Json(data);
		}

	}
}