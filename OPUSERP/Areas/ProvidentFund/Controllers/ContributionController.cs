using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.ProvidentFund.Models;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;

namespace OPUSERP.Areas.ProvidentFund.Controllers
{
    //[Authorize]
    [Area("ProvidentFund")]

    public class ContributionController : Controller
    {
        private readonly IPFContributionService pFContributionService;
        private readonly IEmployeeInfoService employeeInfoService;

        public ContributionController(IPFContributionService pFContributionService, IEmployeeInfoService employeeInfoService)
        {
            this.pFContributionService = pFContributionService;
            this.employeeInfoService = employeeInfoService;
        }
        public async Task<ActionResult> ContributionOverView()
        {
            var totalEmployeeContribution = await pFContributionService.GetTotalEmployeeContribution();
            var totalCompanyContribution = await pFContributionService.GetTotalEmployeeContribution();
            var totalContribution = totalEmployeeContribution + totalCompanyContribution;
            ContributionViewModel model = new ContributionViewModel
            {
                pFContributions = await pFContributionService.GetAllPFContribution(),
                //totalEmployeeContribution= totalEmployeeContribution,
                //totalCompanyContribution= totalCompanyContribution,
                totalContribution=totalContribution
            };
            return View(model);

        }


        public async Task<ActionResult> NewContribution()
        {
            ContributionViewModel model = new ContributionViewModel
            {

            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> NewContribution([FromForm] ContributionViewModel model)
        {
            PFContribution data = new PFContribution
            {
                Id=model.contributionId,
                pfmemberId=model.pfmemberId,
                EmployeeInfoId = model.employeeInfoId,
                transectionDate=model.transectionDate,
               // contributionMonth=model.contributionMonth,
                companyContribution=model.companyContribution,
                employeeContribution=model.employeeContribution,
                credit=model.companyContribution + model.employeeContribution,
                narration =model.narration

            };
            await pFContributionService.SavePFContribution(data);
            return RedirectToAction(nameof(NewContribution));
        }

        public async Task<ActionResult> ContributionList()
        {
            ContributionViewModel model = new ContributionViewModel
            {
            };
            return View(model);
        }
        public async Task<IActionResult> DeleteContribution(int id)
        {
            //await stockService.DeleteStockMasterById(id);
            if (id > 0)
            {

                try
                {
                    await pFContributionService.DeletePFContributionById(id);
                    return Json(new { Success = true, Message = "Deleted Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = "Unable to delete! please try again.. ", exception = ex.Message });
                }
            }

            return Json("ok");
        }

        public async Task<IActionResult> EditContribution(int id)
        {
            try
            {
                var data = await pFContributionService.GetPFContributionById(id);
                ContributionViewModel model = new ContributionViewModel
                {
                    contributionId=data.Id,
                    pfmemberId=data.pfmemberId,
                    transectionDate = data.transectionDate,
                  //  contributionMonth=data.contributionMonth,
                    companyContribution=(int)data.companyContribution,
                    employeeContribution=(int)data.employeeContribution
                };

                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        [HttpPost]
        public async Task<IActionResult> EditContribution([FromForm] ContributionViewModel model)
        {

            PFContribution data = new PFContribution
            {
                Id = model.contributionId,
                pfmemberId = model.pfmemberId,
                transectionDate = model.transectionDate,
               // contributionMonth = model.contributionMonth,
                companyContribution = model.companyContribution,
                employeeContribution = model.employeeContribution

            };
            await pFContributionService.SavePFContribution(data);
            return RedirectToAction(nameof(ContributionList));
        }

        [HttpGet]
        [Route("global/api/GetContributionAmountByMemberId/{Id}")]
        public async Task<IActionResult> GetContributionAmountByMemberId(int id)
        {
            return Json(await pFContributionService.GetContributionAmountByMemberId(id));
        }

        public async Task<IActionResult> GetTotalContribution()
        {
            return Json(await pFContributionService.GetTotalContribution());
        }

        public async Task<IActionResult> PfInterestDistribution(int periodId, decimal? interest)
        {

            var data = await pFContributionService.PFInterestDistribution(HttpContext.User.Identity.Name, periodId, interest);
            return Json("ok");

        }
    }
}