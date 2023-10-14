using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMMatrix.Models;
using OPUSERP.Areas.SCMMatrix.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using OPUSERP.SCM.Services.Matrix.Interfaces;
using OPUSERP.SCM.Services.Requisition.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Matrix.Controllers
{
    [Area("SCMMatrix")]
    public class ApprovalMatrixController : Controller
    {
        private readonly IApprovalMatrixService approvalMatrixService;
        private readonly IApprovalMatrixlogService approvalMatrixlogService;
        private readonly IApproverTypeService approverTypeService;
        private readonly IMatrixTypeService matrixTypeService;
        private readonly IUserInfoes userInfoes;
        private readonly IProjectService projectService;
        private readonly IApprovalLogService approverLogService;
        private readonly LangGenerate<ApprovalMatrixLN> _lang;
        private readonly IRequisitionService requisitionService;
        private UserManager<ApplicationUser> _userManager;

        public ApprovalMatrixController(IHostingEnvironment hostingEnvironment, IRequisitionService requisitionService, IApprovalMatrixlogService approvalMatrixlogService, IApprovalMatrixService approvalMatrixService, IApproverTypeService approverTypeService, IMatrixTypeService matrixTypeService, IUserInfoes userInfoes, IProjectService projectService, IApprovalLogService approverLogService)
        {
            this.approverLogService = approverLogService;
            this.approvalMatrixService = approvalMatrixService;
            this.approverTypeService = approverTypeService;
            this.matrixTypeService = matrixTypeService;
            this.userInfoes = userInfoes;
            this.projectService = projectService;
            this.requisitionService = requisitionService;
            this.approvalMatrixlogService = approvalMatrixlogService;

            _lang = new LangGenerate<ApprovalMatrixLN>(hostingEnvironment.ContentRootPath);
        }

        public async Task<IActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;
            var lstMatrix = await approvalMatrixService.GetAllMatrixInformation(userName);
            var user = await userInfoes.GetAspnetuserByUser(User.Identity.Name);
            var sbuId = user.specialBranchUnitId;
            if (lstMatrix == null)
            {
                lstMatrix = new List<MatrixInformationVM>();
            }

            var llstApproverType = await approverTypeService.GetApproverTypeList();
            if (llstApproverType == null)
            {
                llstApproverType = new List<ApproverType>();
            }

            var lstMatrixType = await matrixTypeService.GetMatrixTypeList();
            if (lstMatrixType == null)
            {
                lstMatrixType = new List<MatrixType>();
            }

            var lstProject = await projectService.GetProjectList();
            if (lstProject == null)
            {
                lstProject = new List<Project>();
            }
            var model = new ApprovalMatrixViewModel
            {
                budgetBranchId = sbuId,
                matrixInformation = lstMatrix,
                matrixTypes = lstMatrixType,
                projects = lstProject,
                approverTypes = llstApproverType,
                aspNetUsersViews = await userInfoes.GetUsersByEmployeeInfo(),
                flang = _lang.PerseLang("Matrix/ApprovalMatrixEN.json", "Matrix/ApprovalMatrixBN.json", Request.Cookies["lang"]),
            };
            return View(model);
        }


        //public async Task<IActionResult> NewApprovalMatrix()
        //{
        //    string userName = HttpContext.User.Identity.Name;
        //    //var user = await _userManager.FindByNameAsync(userName);

        //    //var data = await approvalMatrixService.GetApprovalMatrixs();
        //    List<ApprovalMatrixsVM1> pro = new List<ApprovalMatrixsVM1>();
        //    var raisers = await approvalMatrixService.GetAllProjectsByMatrixType("Purchase Requisition");
        //    foreach (var item in raisers)
        //    {
        //        var data = new ApprovalMatrixsVM1
        //        {
        //            projectName = item.projectName,
        //            raiserName = item.Raiser,
        //            raiserPhoto = item.Photo,
        //            Approvars = await approvalMatrixService.GetApprovarsByUserIdAndProjectId(item.userid, item.projectId)
        //        };
        //        pro.Add(data);
        //    }
        //    return View(pro);
        //}

        [HttpGet]
        public async Task<IActionResult> NewApprovalMatrix()
        {
            string userName = HttpContext.User.Identity.Name;

            //var user = await _userManager.FindByNameAsync(userName);
            //var data = await approvalMatrixService.GetApprovalMatrixs();

            var lstMatrixType = await matrixTypeService.GetMatrixTypeList();
            List<ApprovalMatrixsVM1> pro = new List<ApprovalMatrixsVM1>();

            //pro.FirstOrDefault().matrixTypes = lstMatrixType;
            //var raisers = await approvalMatrixService.GetAllProjectsByMatrixType("Purchase Requisition");
            var raisers = await approvalMatrixService.GetAllProjectsByMatrixType("");

            foreach (var item in raisers)
            {
                var data = new ApprovalMatrixsVM1
                {
                    raiserName = item.Raiser,
                    projectId = item.projectId,
                    projectName = item.projectName,
                    matrixTypeName = item.matrixTypeName,
                    matrixTypeId=item.matrixTypeId,
                    raiserId = item.userid,
                    raiserPhoto = item.Photo,
                    Approvars = await approvalMatrixService.GetApprovarsByUserIdAndProjectId(item.userid, item.projectId, item.matrixTypeName)
                };
                pro.Add(data);
            }

            ApprovalMatrixListViewModel model = new ApprovalMatrixListViewModel
            {
                approvalMatricess = pro,
                matrixTypes = lstMatrixType
            };
            return View(model);

        }
        public async Task<IActionResult> IndexShow()
        {
            string userName = HttpContext.User.Identity.Name;


            var model = new ApprovalMatrixViewModel
            {
                requisitionMasters = await requisitionService.GetAllRequisitionMasterList(),
                aspNetUsersViews = await userInfoes.GetUsersByEmployeeInfo(),
                aspNetUsersApproverViews = await userInfoes.GetUsersApproverByEmployeeInfo(),
                changeOfDoas = await approverLogService.Getchangedoas(),
                changeDoaViewModels = await approverLogService.Getchangedoasp(),

                flang = _lang.PerseLang("Matrix/ApprovalMatrixEN.json", "Matrix/ApprovalMatrixBN.json", Request.Cookies["lang"]),
            };
            return View(model);
        }
        public async Task<IActionResult> IndexShowFS()
        {
            string userName = HttpContext.User.Identity.Name;


            var model = new ApprovalMatrixViewModel
            {
                // requisitionMasters = await requisitionService.GetAllRequisitionMasterList(),
                aspNetUsersViews = await userInfoes.GetUsersByEmployeeInfo(),
                aspNetUsersApproverViews = await userInfoes.GetUsersApproverByEmployeeInfo(),
                matrixChangeHistories = await approverLogService.GetForceShiftLogList(),
                //  changeOfDoas = await approverLogService.Getchangedoas(),
                //changeDoaViewModels = await approverLogService.Getchangedoasp(),

                flang = _lang.PerseLang("Matrix/ApprovalMatrixEN.json", "Matrix/ApprovalMatrixBN.json", Request.Cookies["lang"]),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] ApprovalMatrixVM lstModel)
        {
            string userName = HttpContext.User.Identity.Name;
            var totalLen = lstModel.approverTypeId;

            //ApprovalMatrix raiser = new ApprovalMatrix
            //{
            //    projectId = lstModel.projectId[0],
            //    matrixTypeId = lstModel.matrixTypeId[0],
            //    userId = lstModel.userId[0],
            //    nextApproverId = lstModel.userId[0],
            //    approverTypeId = 0,
            //    sequenceNo = 0,
            //    isActive = 1,
            //    createdBy = userName,
            //    isDelete = 0
            //};
            //await approvalMatrixService.SaveApprovalMatrix(raiser);

            for (int i = 0; i < totalLen.Count(); i++)
            {
                ApprovalMatrix data = new ApprovalMatrix
                {
                    projectId = lstModel.projectId[i],
                    matrixTypeId = lstModel.matrixTypeId[i],
                    userId = lstModel.userId[i],
                    nextApproverId = lstModel.nextApproverId[i],
                    approverTypeId = lstModel.approverTypeId[i],
                    sequenceNo = lstModel.sequenceNo[i],
                    isActive = (int)lstModel.isActive[i],
                    createdBy = userName,
                    isDelete = 0
                };
                await approvalMatrixService.SaveApprovalMatrix(data);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRaiser(int projectId, int matrixTypeId, int newUserId, int oldUserId)
        {
            string userName = HttpContext.User.Identity.Name;

            var approvaldata = await approvalMatrixService.GetApprovalMatrixByUserId(oldUserId);

            await approvalMatrixService.UpdateApprovalMatrix(projectId, matrixTypeId, newUserId, oldUserId);
            ApprovalMatrixlog data = new ApprovalMatrixlog
            {
                projectId = projectId,
                matrixTypeId = matrixTypeId,
                userId = oldUserId,
                nextApproverId = approvaldata.FirstOrDefault().nextApproverId,
                approverTypeId = approvaldata.FirstOrDefault().approverTypeId,
                sequenceNo = approvaldata.FirstOrDefault().sequenceNo,
                //isActive = lstModel.isActive[i]==null?0: lstModel.isActive[i],
                isActive = approvaldata.FirstOrDefault().isActive,
                createdBy = userName,
                isDelete = 0,
                remarks = "Raiser Change " + oldUserId + " " + newUserId
            };
            await approvalMatrixlogService.SaveApproverMatrixLog(data);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> ChangeApprover(int Id)
        {
            string userName = HttpContext.User.Identity.Name;

            // var approvaldata = await approverLogService.GetApproverLogById(Id);

            await approverLogService.UpdateApproverLog(Id, userName);


            return RedirectToAction(nameof(IndexShowFS));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeDOA(int Id, DateTime? fromDate, DateTime? toDate)
        {
            //  string userName = HttpContext.User.Identity.Name;

            // var approvaldata = await approverLogService.GetApproverLogById(Id);

            await approverLogService.UpdateDOALog(Id, fromDate, toDate);


            return RedirectToAction(nameof(IndexShow));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeReDOA(int Id)
        {
            string userName = HttpContext.User.Identity.Name;

            // var approvaldata = await approverLogService.GetApproverLogById(Id);

            await approverLogService.UpdateReDOALog(Id);


            return RedirectToAction(nameof(IndexShow));
        }
        [HttpGet]
        [Route("/api/matrix/UserInfoes/")]
        public async Task<JsonResult> GetPlaceOfPostingWithUser()
        {
            var result = await userInfoes.GetUsersByEmployeeInfo();
            return Json(result);
        }

        [HttpGet]
        [Route("/api/matrix/MatrixByRaiser/{projectId}/{matrixTypeId}/{raiserId}")]
        public async Task<JsonResult> GetMatrixByRaiser(int projectId, int matrixTypeId, int raiserId)
        {
            var result = await approvalMatrixService.GetApprovalMatrixByRaiserId(projectId, matrixTypeId, raiserId);
            return Json(result);
        }

        [HttpGet]
        [Route("/api/matrix/ApproverPanelList/{id}/{projectId}")]
        public async Task<JsonResult> GetPRApproverPanel(int id, int projectId)
        {
            string userName = HttpContext.User.Identity.Name;
            var result = await approvalMatrixService.GetPRApproverPanelList(userName, id, projectId);
            return Json(result);
        }

        [HttpGet]
        [Route("/api/matrix/GetPRApproverPanelListFromApprovalLogs/{id}/{reqMasterId}")]
        public async Task<JsonResult> GetPRApproverPanelListFromApprovalLogs(int id, int reqMasterId)
        {
            string userName = HttpContext.User.Identity.Name;
            var result = await approvalMatrixService.GetPRApproverPanelListFromApprovalLogs(userName, id, reqMasterId);
            return Json(result);
        }
        [HttpGet]
        [Route("/api/matrix/GetPRApproverPanelFListFromApprovalLogs/{id}/{reqMasterId}")]
        public async Task<JsonResult> GetPRApproverPanelFListFromApprovalLogs(int id, int reqMasterId)
        {
            string userName = HttpContext.User.Identity.Name;
            var result = await approvalMatrixService.GetPRApproverPanelFListFromApprovalLogs(userName, id, reqMasterId);
            return Json(result);
        }
        [HttpGet]
        [Route("/api/matrix/GetPRApproverPanelListbyreq/{reqMasterId}")]
        public async Task<JsonResult> GetPRApproverPanelListbyreq(int reqMasterId)
        {

            var result = await approvalMatrixService.GetApprovalMatrixVMR(reqMasterId);
            return Json(result);
        }

        [HttpGet]
        [Route("/api/matrix/AllApproverInfo/{id}/{matrixTypeId}")]
        public async Task<System.Object> GetAllApproveInfo(int id, int matrixTypeId)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var nextAppInfo = await approverLogService.GetNextApproverInfo(userName, id, matrixTypeId);
                var panelList = await approverLogService.GetApprovedPanelList(userName, id, matrixTypeId);
                return Ok(new { nextAppInfo, panelList });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        //[Route("/api/matrix/UserInfoes/{mTypeName}")]
        public async Task<JsonResult> MatrixTypeList(string mTypeName)
        {
            var result = await approvalMatrixService.GetAllProjectsByMatrixTypeByRaisar(mTypeName);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNewApprovalMatrix()
        {
            var lstMatrixType = await matrixTypeService.GetMatrixTypeList();

            if (lstMatrixType == null)
            {
                lstMatrixType = new List<MatrixType>();
            }

            var model = new ApprovalMatrixViewModel
            {
                projects = await projectService.GetActiveProjectList(),
                matrixTypes = lstMatrixType
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditNewApprovalMatrix(int? projectId, int? raiserId,int matrixTypeId)
        {
			var user = await userInfoes.GetUserInfoByUserId(raiserId);

			ViewBag.raiserId = raiserId;
			ViewBag.raiserName = user.EmpName;
			ViewBag.projectId = projectId;

            var lstMatrixType = await matrixTypeService.GetMatrixTypeList();
            if (lstMatrixType == null)
            {
                lstMatrixType = new List<MatrixType>();
            }

            var model = new ApprovalMatrixViewModel
            {
                projects = await projectService.GetActiveProjectList(),
                matrixTypes = lstMatrixType,
                matrixTypeId=matrixTypeId,
                approvalMatrixForEdit = await approvalMatrixService.GetApprovalMatrixByProjectAndRaiserId(projectId, raiserId,matrixTypeId)
            };
            return View(model);
        }
		public async Task<IActionResult> DeleteNewApprovalMatrix(int? projectId, int? raiserId,int matrixTypeId)
        {
			await approvalMatrixService.DeleteApprovalMatrixByProjectRaiserMTypeId(Convert.ToInt32(projectId), Convert.ToInt32(raiserId), Convert.ToInt32(matrixTypeId));
			return RedirectToAction("NewApprovalMatrix");
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewApprovalMatrix([FromForm] ApprovalMatrixViewModel model)
        {
			//if (model.matrixId != 0 || model.matrixId != null)
			//{
			//    await approvalMatrixService.DeleteApprovalMatrixByprojectuserId(Convert.ToInt32(model.nProjectId), Convert.ToInt32(model.nRaiserId));
			//}

			var delete = await approvalMatrixService.DeleteApprovalMatrixByProjectRaiserMTypeId(Convert.ToInt32(model.nProjectId), Convert.ToInt32(model.nRaiserId), Convert.ToInt32(model.matrixTypeId));

			var i = 1;
			if (model.nextApproversId[0] != null)
			{
				foreach (var item in model.nextApproversId)
				{
					var dataA = new ApprovalMatrix
					{
						Id = 0,
						projectId = model.nProjectId,
						matrixTypeId = model.matrixTypeId,
						userId = model.nRaiserId,
						nextApproverId = item,
						approverTypeId = 1, // Approver
						isActive = 1,
						sequenceNo = i
					};
					var ids = await approvalMatrixService.SaveApprovalMatrix(dataA);
					//if (item != null)
					//{
					//	var dataA = new ApprovalMatrix
					//	{
					//		Id = 0,
					//		projectId = model.nProjectId,
					//		matrixTypeId = model.matrixTypeId,
					//		userId = model.nRaiserId,
					//		nextApproverId = item,
					//		approverTypeId = 1, // Approver
					//		isActive = 1,
					//		sequenceNo = i
					//	};
					//	var ids = await approvalMatrixService.SaveApprovalMatrix(dataA);
					//}
					//else
					//{
					//	var dataA = new ApprovalMatrix
					//	{
					//		Id = 0,
					//		projectId = model.nProjectId,
					//		matrixTypeId = model.matrixTypeId,
					//		userId = model.nRaiserId,
					//		nextApproverId = model.nRaiserId,
					//		approverTypeId = 1, // Approver
					//		isActive = 1,
					//		sequenceNo = i
					//	};
					//	var ids = await approvalMatrixService.SaveApprovalMatrix(dataA);
					//}
					i++;
				}
			}


			var dataF = new ApprovalMatrix
			{
				Id = 0,
				projectId = model.nProjectId,
				matrixTypeId = model.matrixTypeId,
				userId = model.nRaiserId,
				nextApproverId = model.nFinalId,
				approverTypeId = 4, // Final Approver
				isActive = 1,
				sequenceNo = i
			};
			var ids1 = await approvalMatrixService.SaveApprovalMatrix(dataF);
			return RedirectToAction("NewApprovalMatrix");
		}

    }
}