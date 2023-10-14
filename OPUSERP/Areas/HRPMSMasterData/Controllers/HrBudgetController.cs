using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.HRPMS.Data.Entity.HrBudget;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.HrBudget.Interface;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
	[Authorize]
	[Area("HRPMSMasterData")]
	public class HrBudgetController : Controller
    {
		private readonly IHrBudget budgetService;
		private readonly IClientServeLost clientServeLost;

		public HrBudgetController(IHostingEnvironment hostingEnvironment, IHrBudget budgetService, IClientServeLost clientServeLost)
		{
			this.budgetService = budgetService;
            this.clientServeLost = clientServeLost;
		}

		public async Task<IActionResult> Organogram()
		{
			return View();
		}

		public async Task<IActionResult> Index()
        {
			var deptInHrBudget = await budgetService.GetAllDepartments();
			var data = new List<DepartmentWithBudget>();

			foreach (var item in deptInHrBudget)
			{
				data.Add(new DepartmentWithBudget
				{
					department = item.department,
					hrBudgetDpts = await budgetService.GetHrBudgetsByDeptId(item.departmentId)
				});
			}
			var model = new HrBudgetViewModel
			{
				allDesignation = await budgetService.GetAllHrDesignations(),
				allDepartment = await budgetService.GetAllHrDepartments(),
				AllBranch = await budgetService.GetAllHrBranch(),
				departmentWithBudgets = data
			};

			return View(model);
        }

		public async Task<IActionResult> Create()
		{
			var data = new HrBudgetViewModel
			{
				allDesignation = await budgetService.GetAllHrDesignations(),
				allDepartment = await budgetService.GetAllHrDepartments(),
				AllBranch = await budgetService.GetAllHrBranch()
			};
			return View(data);
		}

		[HttpPost]
		public async Task<IActionResult> CreateApi(HrBudgetViewModel model)
		{
			var data = new HrBudgetDpt
			{
				Id = model.hrbudgetId,
				departmentId = model.departmentId,
				designationId = model.designationId,
				branchId = model.branchId,
				person = model.person,
				isActive = 1
			};
			await budgetService.SaveHrBudget(data);

			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> Create(HrBudgetViewModel model)
		{
			var data = new HrBudgetDpt
			{
				Id = model.hrbudgetId,
				departmentId = model.departmentId,
				designationId = model.designationId,
				branchId = model.branchId,
				person = model.person,
				isActive = 1
			};
			await budgetService.SaveHrBudget(data);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			await budgetService.DeleteHrBudgetById(id);
			return Json("ok");
		}
        // GET: ClientServeLost
        public async Task<IActionResult> ClientServeLostLogs()
        {
            ClientServeLostViewModel model = new ClientServeLostViewModel
            {

                clientServeLosts = await clientServeLost.GetClientServeLost(),


            };

            return View(model);
        }

        // POST: ClientServeLost/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientServeLostLogs([FromForm] ClientServeLostViewModel model)
        {


            if (!ModelState.IsValid)
            {
                model.clientServeLosts = await clientServeLost.GetClientServeLost();

                return View(model);
            }
            ClientServeLost data = new ClientServeLost
            {

                Id = model.ClientServeLostId,
                year = model.year,
                clientServe = model.clientServe,
                clientLost = model.clientLost,
                businessTarget = model.businessTarget,
                businessGrowth = model.businessGrowth,
                profit = model.profit,
                dividend = model.dividend,

            };

            await clientServeLost.SaveClientServeLost(data);

            return RedirectToAction(nameof(ClientServeLostLogs));
        }

        public async Task<IActionResult> DeleteClientServeLost(int id)
        {
            await clientServeLost.DeleteClientServeLostById(id);
            return RedirectToAction(nameof(ClientServeLostLogs));
        }
    }
}