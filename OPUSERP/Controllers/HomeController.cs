using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Dashboard;
using OPUSERP.CRM.Services.Lead.Interfaces;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Auth;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.AuthService.Interfaces;
using OPUSERP.ERPServices.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Dashboard.interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Models.Dashboard;
using OPUSERP.SCM.Services.IOU.Interface;
using OPUSERP.SCM.Services.PurchaseOrder.Interface;
using OPUSERP.SCM.Services.PurchaseProcess.Interfaces;
using OPUSERP.SCM.Services.Requisition.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserInfoes userInfoes;
        private readonly IIOUService iOUService;
        private readonly IPurchaseProcessService purchaseProcessService;
        private readonly IRequisitionService requisitionService;
        private readonly IPageAssignService pageAssignService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly INavbarService navbarService;
        private readonly IBillCollectionService billCollectionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHrmDashboardService hrmDashboardService;
        private readonly IPurchaseOrderService purchaseOrderService;
        private readonly IPersonalInfoService personalInfoService;
        private ERPDbContext _db;

        public HomeController(IPersonalInfoService personalInfoService, IPurchaseProcessService purchaseProcessService, IPurchaseOrderService purchaseOrderService, IRequisitionService requisitionService, IUserInfoes userInfoes, IPageAssignService pageAssignService, IHrmDashboardService hrmDashboardService, IERPCompanyService eRPCompanyService, INavbarService navbarService, IBillCollectionService billCollectionService, ERPDbContext db, UserManager<ApplicationUser> _userManager, IIOUService iOUService)
        {
            this.requisitionService = requisitionService;
            this.iOUService = iOUService;
            this.pageAssignService = pageAssignService;
            this.purchaseOrderService = purchaseOrderService;
            this.eRPCompanyService = eRPCompanyService;
            this.navbarService = navbarService;
            this.userInfoes = userInfoes;
            this.billCollectionService = billCollectionService;
            this._userManager = _userManager;
            this.hrmDashboardService = hrmDashboardService;
            this.purchaseProcessService = purchaseProcessService;
            this.personalInfoService = personalInfoService;

            _db = db;
        }

        public async Task<ActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("UAT PIMS") && roles.Contains("UAT Payroll"))
            {
                return RedirectToAction("HRDashboard", "Dashboard", new { area = "HRPMSEmployee" });
            }
            else if (roles.Contains("HRAdmin"))
            {
                return RedirectToAction("NewDashBoard");
            }
            else if (roles.Contains("PO Admin"))
            {
                return RedirectToAction("DashboardCS");
            }
            else if (roles.Contains("PO Processor"))
            {
                return RedirectToAction("DashboardCSNewAssign");
            }
            else if (roles.Contains("PO Approval"))
            {
                return RedirectToAction("DashboardCS");
            }
            else
            {
                return View();
            }
            //else
            //{
            //    return RedirectToAction("StoreKeeper", "RequisitionDashbord", new { Area = "SCMRequisition" });
            //}
        }
        public async Task<ActionResult> Index2()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            
            var empID = await personalInfoService.GetEmployeeIdByCode(user.EmpCode);
            //return LocalRedirect("~/HRPMSEmployee/Photograph/EditGrid?id=" + empID);
            return LocalRedirect("~/HRPMSEmployee/InfoView/Index?id=4193");


        }




        #region Privacy

        public IActionResult Privacy()
        {
            return View();
        }

        #endregion

        #region Navigation



        public async Task<IActionResult> Navigation()
        {
            string userName = HttpContext.User.Identity.Name;

            NavbarViewModel model = new NavbarViewModel
            {
                navbars = await navbarService.GetNavigationMenu(userName),

            };
            ViewBag.UserTypeID = 1;

            return PartialView("_NavMenu", model);
        }

        public async Task<IActionResult> AssignPage()
        {
            string userName = HttpContext.User.Identity.Name;
            var userId = _db.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
            List<string> roleids = _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            List<int?> lstmodule = _db.UserAccessPages.Where(x => roleids.Contains(x.applicationRoleId)).Select(x => x.navbarId).ToList();

            List<int> lstparentId = _db.Navbars.Where(x => lstmodule.Contains(x.Id)).Select(x => x.parentID).ToList();
            List<int> lstparentIdF = _db.Navbars.Where(x => lstparentId.Contains(x.Id)).Select(x => x.parentID).ToList();

            var navdata = await pageAssignService.GetNavbars(userName);
            var adminrole = _db.UserRoles.Where(x => x.UserId == userId && x.RoleId == "0583d54e-74a8-46a3-b880-e13698723f69").ToList();
            if (adminrole.Count() == 0)
            {
                navdata = navdata.Where(x => lstmodule.Contains(x.Id) || lstparentId.Contains(x.Id) || lstparentIdF.Contains(x.Id));
            }
            List<int?> modid = navdata.Select(x => x.moduleId).ToList();
            var modules = await pageAssignService.GetERPModules();
            if (adminrole.Count() == 0)
            {
                modules = modules.Where(x => modid.Contains(x.Id));
            }
            NavbarViewModel model = new NavbarViewModel
            {
                navbars = navdata,//await pageAssignService.GetNavbars(userName),
                ERPModules = modules,//await pageAssignService.GetERPModules(),
                userRoles = await _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync()
            };

            ViewBag.UserTypeID = 1;

            return PartialView("_Navbar", model);
        }

        public async Task<IActionResult> GridMenuPage(int moduleId, int perentId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userId = _db.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
            // var data = _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            List<string> roleids = _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            List<int?> lstmodule = _db.UserAccessPages.Where(x => roleids.Contains(x.applicationRoleId)).Select(x => x.navbarId).ToList();

            List<int> lstparentId = _db.Navbars.Where(x => lstmodule.Contains(x.Id)).Select(x => x.parentID).ToList();

            List<Navbar> lstMenu = _db.Navbars.Where(x => x.moduleId == moduleId && x.parentID == perentId && x.status == true).OrderBy(x => x.displayOrder).ToList();
            List<Navbar> lstChieldMenu = new List<Navbar>();
            //foreach(var item in lstMenu)
            //{
            //    lstChieldMenu.AddRange(_db.Navbars.Where(x => x.parentID == item.Id).OrderBy(x => x.displayOrder).ToList());
            //}

            var navdata = await pageAssignService.GetNavbars(userName);
            var adminrole = _db.UserRoles.Where(x => x.UserId == userId && x.RoleId == "0583d54e-74a8-46a3-b880-e13698723f69").ToList();
            if (adminrole.Count() == 0)
            {
                lstChieldMenu = navdata.Where(x => lstmodule.Contains(x.Id)).ToList();
                lstMenu = lstMenu.Where(x => lstparentId.Contains(x.Id)).ToList();
            }
            else
            {
                lstChieldMenu = navdata.ToList();
            }
            List<int?> modid = navdata.Select(x => x.moduleId).ToList();
            var modules = await pageAssignService.GetERPModules();
            if (adminrole.Count() == 0)
            {
                modules = modules.Where(x => modid.Contains(x.Id));
            }

            var model = new NavbarViewModel
            {
                navbars = lstChieldMenu,
                navbarsbyparent = lstMenu,
                ERPModules = modules
            };
            return View(model);
        }
        public async Task<IActionResult> GridMenuButton(int moduleId, int perentId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userId = _db.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
            List<string> roleids = _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            List<int?> lstmodule = _db.UserAccessPages.Where(x => roleids.Contains(x.applicationRoleId)).Select(x => x.navbarId).ToList();

            List<int> lstparentId = _db.Navbars.Where(x => lstmodule.Contains(x.Id)).Select(x => x.parentID).ToList();

            List<Navbar> lstMenu = _db.Navbars.Where(x => x.moduleId == moduleId && x.parentID == perentId && x.status == true).OrderBy(x => x.displayOrder).ToList();
            List<Navbar> lstChieldMenu = new List<Navbar>();


            var navdata = await pageAssignService.GetNavbars(userName);
            var adminrole = _db.UserRoles.Where(x => x.UserId == userId && x.RoleId == "0583d54e-74a8-46a3-b880-e13698723f69").ToList();
            if (adminrole.Count() == 0)
            {
                lstChieldMenu = navdata.Where(x => lstmodule.Contains(x.Id)).ToList();
                lstMenu = lstMenu.Where(x => lstparentId.Contains(x.Id)).ToList();
            }
            else
            {
                lstChieldMenu = navdata.ToList();
            }
            List<int?> modid = navdata.Select(x => x.moduleId).ToList();
            var modules = await pageAssignService.GetERPModules();
            if (adminrole.Count() == 0)
            {
                modules = modules.Where(x => modid.Contains(x.Id));
            }

            var model = new NavbarViewModel
            {
                navbars = lstChieldMenu,
                navbarsbyparent = lstMenu,
                ERPModules = modules
            };
            return View(model);
        }


        public async Task<NavbarViewModel> GetMenuActions(string area, string controller, string action)
        {
            string userName = HttpContext.User.Identity.Name;
            var userId = _db.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
            List<string> roleids = _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            List<int?> lstmodule = _db.UserAccessPages.Where(x => roleids.Contains(x.applicationRoleId)).Select(x => x.navbarId).ToList();

            List<int> lstparentId = _db.Navbars.Where(x => lstmodule.Contains(x.Id)).Select(x => x.parentID).ToList();
            var nabver = _db.Navbars.Where(x => x.area == area && x.controller == controller && x.action == action).FirstOrDefault();
            List<Navbar> lstMenu = _db.Navbars.Where(x => x.moduleId == nabver.moduleId && x.parentID == nabver.parentID && x.status == true).OrderBy(x => x.displayOrder).ToList();
            List<Navbar> lstChieldMenu = new List<Navbar>();


            var navdata = await pageAssignService.GetNavbars(userName);
            var adminrole = _db.UserRoles.Where(x => x.UserId == userId && x.RoleId == "0583d54e-74a8-46a3-b880-e13698723f69").ToList();
            if (adminrole.Count() == 0)
            {
                lstChieldMenu = navdata.Where(x => lstmodule.Contains(x.Id)).ToList();
                lstMenu = lstMenu.Where(x => lstparentId.Contains(x.Id)).ToList();
            }
            else
            {
                lstChieldMenu = navdata.ToList();
            }
            List<int?> modid = navdata.Select(x => x.moduleId).ToList();
            var modules = await pageAssignService.GetERPModules();
            if (adminrole.Count() == 0)
            {
                modules = modules.Where(x => modid.Contains(x.Id));
            }

            var model = new NavbarViewModel
            {
                navbars = lstChieldMenu,
                navbarsbyparent = lstMenu,
                ERPModules = modules
            };
            return model;
            // return View(model);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        [Route("/Home/Error")]
        public IActionResult Error()
        {
            return View();
        }

        #endregion

        #region Dashboard

        public ActionResult CrmDashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCollectionDashboardByDateArea(DateTime? frmDate, DateTime? toDate, int? areaId)
        {

            return Json(await billCollectionService.GetCollectionDashboardByDateArea(frmDate, toDate, areaId));
        }


        #endregion

        public async Task<IActionResult> NewDashBoard()
        {
            MainDashboardViewModel model = new MainDashboardViewModel
            {
                activeEmployeeCountViewModel = await hrmDashboardService.GetActiveEmployeeCountViewModel()
                //maleFemalCount = await hrmDashboardService.GetAllActiveMaleFemaleEmpCount()
            };
            return View(model);
        }
        public async Task<IActionResult> DashboardCS(DashboardCSViewModel model)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userInfo = await userInfoes.GetUserInfoByUser(userName);
                //var aprove = await requisitionService.GetAllRequisitionDhasboard();
                var pendingReq = await requisitionService.GetRequisitionApproveList(userInfo.UserId);
                var PendingCS = await purchaseProcessService.GetCSListForApprove(userInfo.UserId);
                var SinglePendingCS = await purchaseProcessService.GetCSListForApproveSingleSource(userInfo.UserId);
                var SpotPending = await iOUService.GetIOUListForApprove(userInfo.UserId);
                var PoPending = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName, 5);
                var assignRequisition = await requisitionService.GetRequisitionMasterListForAssign();

                var allReq = await requisitionService.GetAllRequisitionMasterList();
                var AllReqDetails = await requisitionService.GetAllRequisitionDetailList();

                var allcs = await purchaseProcessService.GetAllCSMasterList();
                var AllCsDetails = await purchaseProcessService.GetAllCsDetailList();

                var allpo = await purchaseOrderService.GetAllPurchaseOrderMasterList();
                var allsp = await iOUService.GetAllIOUMasterList();


                var data = new DashboardCSViewModel
                {
                    PendingRequisitionCount = pendingReq.Count(),
                    CSPendingCount = PendingCS.Count(),
                    POPendingCount = PoPending.Count(),
                    SpotPendingCount = SpotPending.Count(),
                    SingelPendingCount = SinglePendingCS.Count(),
                    AssignPendingRequisitionCount = assignRequisition.Count(),

                    AllRequisition = allReq,
                    AllRequisitionDetails = AllReqDetails,

                    AllReqAmount = await GetTotalReqAmount("all", "all"),
                    AllApReqAmount = await GetTotalReqAmount("all", "approved"),
                    AllPendReqAmount = await GetTotalReqAmount("all", "pending"),

                    MonthReqAmount = await GetTotalReqAmount("month", "all"),
                    MonthApReqAmount = await GetTotalReqAmount("month", "approved"),
                    MonthPendReqAmount = await GetTotalReqAmount("month", "pending"),

                    YearReqAmount = await GetTotalReqAmount("year", "all"),
                    YearApReqAmount = await GetTotalReqAmount("year", "approved"),
                    YearPendReqAmount = await GetTotalReqAmount("year", "pending"),

                    AllCS = allcs,
                    AllCSDetails = AllCsDetails,

                    AllCsAmount = await GetTotalCsAmount("all", "all"),
                    AllApCsAmount = await GetTotalCsAmount("all", "approved"),
                    AllPendCsAmount = await GetTotalCsAmount("all", "pending"),

                    MonthCsAmount = await GetTotalCsAmount("month", "all"),
                    MonthApCsAmount = await GetTotalCsAmount("month", "approved"),
                    MonthPendCsAmount = await GetTotalCsAmount("month", "pending"),

                    YearCsAmount = await GetTotalCsAmount("year", "all"),
                    YearApCsAmount = await GetTotalCsAmount("year", "approved"),
                    YearPendCsAmount = await GetTotalCsAmount("year", "pending"),

                    AllSpotPurchase = allsp,

                    AllSpotAmount = await GetTotalSpotAmount("all", "all"),
                    AllApSpotAmount = await GetTotalSpotAmount("all", "approved"),
                    AllPendSpotAmount = await GetTotalSpotAmount("all", "pending"),

                    MonthSpotAmount = await GetTotalSpotAmount("month", "all"),
                    MonthApSpotAmount = await GetTotalSpotAmount("month", "approved"),
                    MonthPendSpotAmount = await GetTotalSpotAmount("month", "pending"),

                    YearSpotAmount = await GetTotalSpotAmount("year", "all"),
                    YearApSpotAmount = await GetTotalSpotAmount("year", "approved"),
                    YearPendSpotAmount = await GetTotalSpotAmount("year", "pending"),


                    AllPurchase = allpo,
                    AllPOAmount = await GetTotalPOAmount("all", "all"),
                    AllApPOAmount = await GetTotalPOAmount("all", "approved"),
                    AllPendPOAmount = await GetTotalPOAmount("all", "pending"),

                    MonthPOAmount = await GetTotalPOAmount("month", "all"),
                    MonthApPOAmount = await GetTotalPOAmount("month", "approved"),
                    MonthPendPOAmount = await GetTotalPOAmount("month", "pending"),

                    YearPOAmount = await GetTotalPOAmount("year", "all"),
                    YearApPOAmount = await GetTotalPOAmount("year", "approved"),
                    YearPendPOAmount = await GetTotalPOAmount("year", "pending"),
                };
                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> DashboardCSNewAssign(DashboardCSViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            //var aprove = await requisitionService.GetAllRequisitionDhasboard();
            var pendingReq = await requisitionService.GetRequisitionApproveList(userInfo.UserId);
            var PendingCS = await purchaseProcessService.GetCSListForApprove(userInfo.UserId);
            var SinglePendingCS = await purchaseProcessService.GetCSListForApproveSingleSource(userInfo.UserId);
            var SpotPending = await iOUService.GetIOUListForApprove(userInfo.UserId);
            var PoPending = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName, 1);
            var assignRequisition = await requisitionService.GetRequisitionMasterListForAssign();

            var allReq = await requisitionService.GetAllRequisitionMasterList();
            var AllReqDetails = await requisitionService.GetAllRequisitionDetailList();

            var allcs = await purchaseProcessService.GetAllCSMasterList();
            var AllCsDetails = await purchaseProcessService.GetAllCsDetailList();

            var allpo = await purchaseOrderService.GetAllPurchaseOrderMasterList();
            var allsp = await iOUService.GetAllIOUMasterList();


            var data = new DashboardCSViewModel
            {
                PendingRequisitionCount = pendingReq.Count(),
                CSPendingCount = PendingCS.Count(),
                POPendingCount = PoPending.Count(),
                SpotPendingCount = SpotPending.Count(),
                SingelPendingCount = SinglePendingCS.Count(),
                AssignPendingRequisitionCount = assignRequisition.Count(),

                AllRequisition = allReq,
                AllRequisitionDetails = AllReqDetails,

                AllReqAmount = await GetTotalReqAmount("all", "all"),
                AllApReqAmount = await GetTotalReqAmount("all", "approved"),
                AllPendReqAmount = await GetTotalReqAmount("all", "pending"),

                MonthReqAmount = await GetTotalReqAmount("month", "all"),
                MonthApReqAmount = await GetTotalReqAmount("month", "approved"),
                MonthPendReqAmount = await GetTotalReqAmount("month", "pending"),

                YearReqAmount = await GetTotalReqAmount("year", "all"),
                YearApReqAmount = await GetTotalReqAmount("year", "approved"),
                YearPendReqAmount = await GetTotalReqAmount("year", "pending"),

                AllCS = allcs,
                AllCSDetails = AllCsDetails,

                AllCsAmount = await GetTotalCsAmount("all", "all"),
                AllApCsAmount = await GetTotalCsAmount("all", "approved"),
                AllPendCsAmount = await GetTotalCsAmount("all", "pending"),

                MonthCsAmount = await GetTotalCsAmount("month", "all"),
                MonthApCsAmount = await GetTotalCsAmount("month", "approved"),
                MonthPendCsAmount = await GetTotalCsAmount("month", "pending"),

                YearCsAmount = await GetTotalCsAmount("year", "all"),
                YearApCsAmount = await GetTotalCsAmount("year", "approved"),
                YearPendCsAmount = await GetTotalCsAmount("year", "pending"),

                AllSpotPurchase = allsp,

                AllSpotAmount = await GetTotalSpotAmount("all", "all"),
                AllApSpotAmount = await GetTotalSpotAmount("all", "approved"),
                AllPendSpotAmount = await GetTotalSpotAmount("all", "pending"),

                MonthSpotAmount = await GetTotalSpotAmount("month", "all"),
                MonthApSpotAmount = await GetTotalSpotAmount("month", "approved"),
                MonthPendSpotAmount = await GetTotalSpotAmount("month", "pending"),

                YearSpotAmount = await GetTotalSpotAmount("year", "all"),
                YearApSpotAmount = await GetTotalSpotAmount("year", "approved"),
                YearPendSpotAmount = await GetTotalSpotAmount("year", "pending"),


                AllPurchase = allpo,
                AllPOAmount = await GetTotalPOAmount("all", "all"),
                AllApPOAmount = await GetTotalPOAmount("all", "approved"),
                AllPendPOAmount = await GetTotalPOAmount("all", "pending"),

                MonthPOAmount = await GetTotalPOAmount("month", "all"),
                MonthApPOAmount = await GetTotalPOAmount("month", "approved"),
                MonthPendPOAmount = await GetTotalPOAmount("month", "pending"),

                YearPOAmount = await GetTotalPOAmount("year", "all"),
                YearApPOAmount = await GetTotalPOAmount("year", "approved"),
                YearPendPOAmount = await GetTotalPOAmount("year", "pending"),
            };
            return View(data);
        }

        public async Task<decimal> GetTotalReqAmount(string duration, string type)
        {
            var data = await requisitionService.GetAllRequisitionDetailList();
            var AllReqDetails = data;


            if (type == "approved")
            {
                AllReqDetails = data.Where(x => x.requisitionMaster.statusInfoId == 3);
            }
            else if (type == "pending")
            {
                AllReqDetails = data.Where(x => x.requisitionMaster.statusInfoId != 3 && x.requisitionMaster.reqType == "Final");
            }
            else
            {
                AllReqDetails = data;
            }

            var ReqDetails = AllReqDetails;

            if (duration == "year")
            {
                ReqDetails = AllReqDetails.Where(x => Convert.ToDateTime(x.requisitionMaster.reqDate).Year == DateTime.Now.Year);
            }
            else if (duration == "month")
            {
                ReqDetails = AllReqDetails.Where(x => Convert.ToDateTime(x.requisitionMaster.reqDate).Month == DateTime.Now.Month);
            }
            else
            {
                ReqDetails = AllReqDetails;
            }

            var amount = 0.00m;
            foreach (var item in ReqDetails)
            {
                var amnt = Convert.ToDecimal(item.reqQty) * Convert.ToDecimal(item.reqRate);
                amount += amnt;
            }
            return amount;
        }

        public async Task<decimal> GetTotalCsAmount(string duration, string type)
        {
            var data = await purchaseProcessService.GetAllCsDetailList();
            var AllCsDetails = data;


            if (type == "approved")
            {
                AllCsDetails = data.Where(x => x.cSMaster.csStatus == 3);
            }
            else if (type == "pending")
            {
                AllCsDetails = data.Where(x => x.cSMaster.csStatus != 3);
            }
            else
            {
                AllCsDetails = data;
            }

            var CsDetails = AllCsDetails;

            if (duration == "year")
            {
                CsDetails = AllCsDetails.Where(x => Convert.ToDateTime(x.cSMaster.csDate).Year == DateTime.Now.Year);
            }
            else if (duration == "month")
            {
                CsDetails = AllCsDetails.Where(x => Convert.ToDateTime(x.cSMaster.csDate).Month == DateTime.Now.Month);
            }
            else
            {
                CsDetails = AllCsDetails;
            }

            var amount = 0.00m;
            foreach (var item in CsDetails)
            {
                var amnt = Convert.ToDecimal(item.qty) * Convert.ToDecimal(item.rate);
                amount += amnt;
            }
            return amount;
        }

        public async Task<decimal> GetTotalSpotAmount(string duration, string type)
        {
            var data = await iOUService.GetIOUDetails();
            var AllSpotDetails = data;


            if (type == "approved")
            {
                AllSpotDetails = data.Where(x => x.IOU.iouStatus == 3);
            }
            else if (type == "pending")
            {
                AllSpotDetails = data.Where(x => x.IOU.iouStatus != 3);
            }
            else
            {
                AllSpotDetails = data;
            }

            var SpotDetails = AllSpotDetails;

            if (duration == "year")
            {
                SpotDetails = AllSpotDetails.Where(x => Convert.ToDateTime(x.IOU.iouDate).Year == DateTime.Now.Year);
            }
            else if (duration == "month")
            {
                SpotDetails = AllSpotDetails.Where(x => Convert.ToDateTime(x.IOU.iouDate).Month == DateTime.Now.Month);
            }
            else
            {
                SpotDetails = AllSpotDetails;
            }

            var amount = 0.00m;
            foreach (var item in SpotDetails)
            {
                var amnt = Convert.ToDecimal(item.qty) * Convert.ToDecimal(item.rate);
                amount += amnt;
            }
            return amount;
        }

        public async Task<decimal> GetTotalPOAmount(string duration, string type)
        {
            var data = await purchaseOrderService.GetPurchaseOrderDetails();
            var AllPODetails = data;


            if (type == "approved")
            {
                AllPODetails = data.Where(x => x.purchase.poStatus == 3);
            }
            else if (type == "pending")
            {
                AllPODetails = data.Where(x => x.purchase.poStatus != 3);
            }
            else
            {
                AllPODetails = data;
            }

            var PODetails = AllPODetails;

            if (duration == "year")
            {
                PODetails = AllPODetails.Where(x => Convert.ToDateTime(x.purchase.poDate).Year == DateTime.Now.Year);
            }
            else if (duration == "month")
            {
                PODetails = AllPODetails.Where(x => Convert.ToDateTime(x.purchase.poDate).Month == DateTime.Now.Month);
            }
            else
            {
                PODetails = AllPODetails;
            }

            var amount = 0.00m;
            foreach (var item in PODetails)
            {
                var amnt = Convert.ToDecimal(item.poQty) * Convert.ToDecimal(item.poRate);
                amount += amnt;
            }
            return amount;
        }
    }
}
