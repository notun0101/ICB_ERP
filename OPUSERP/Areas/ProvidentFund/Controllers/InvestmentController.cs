using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.ProvidentFund.Models;
using Microsoft.AspNetCore.Authorization;
using OPUSERP.ProvidentFund.Service.Interface;
using OPUSERP.ProvidentFund.Data.Entity;

namespace OPUSERP.Areas.ProvidentFund.Controllers
{
    [Authorize]
    [Area("ProvidentFund")]
    public class InvestmentController : Controller
    {
        private readonly IPFInvestmentService pFInvestmentService;

        public InvestmentController(IPFInvestmentService pFInvestmentService)
        {
            this.pFInvestmentService = pFInvestmentService;
        }

        public async Task<IActionResult> InvestmentOverView()
        {
            var totalInvestment = await pFInvestmentService.GetTotalInvestment();
            var totalNewYearInvestment = await pFInvestmentService.GetTotalNewYearInvestment();
            PFInvestmentViewModel model = new PFInvestmentViewModel
            {
                pFInvestments = await pFInvestmentService.GetAllPFInvestment(),
                totalInvestment=totalInvestment,
                totalNewYearInvestment=totalNewYearInvestment
            };
            return View(model);
        }

        public async Task<IActionResult> NewInvestment()
        {
            return View();
        }
        [HttpPost]
         public async Task<IActionResult> NewInvestment([FromForm] PFInvestmentViewModel model)
        {
            try
            {
                PFInvestment data = new PFInvestment
                {
                    Id = model.investId,
                    investmentName = model.investmentName,
                    investmentAccount = model.investmentAccount,
                    interestRate = model.interestRate,
                    investmentDate = model.investmentDate,
                    maturityDate = model.maturityDate,
                    transactionDate = model.transactionDate,
                    investmentAmount = model.investmentAmount,
                    interestPeriod = model.interestPeriod,
                    modeOfPayment = model.modeOfPayment
                };
                await pFInvestmentService.SavePFInvestment(data);
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<IActionResult> InvestmentList()
        {
            PFInvestmentViewModel model = new PFInvestmentViewModel
            {
                pFInvestments = await pFInvestmentService.GetAllPFInvestment()
            };
            return View(model);
        }

        public async Task<IActionResult> InvestmentInterest()
        {
            return View();
        }

         public async Task<IActionResult> InvestmentEncashment()
        {
            return View();
        }
         public async Task<IActionResult> PreviewInvestment()
        {
            return View();
        }
         public async Task<IActionResult> ProcessEncashment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessEncashment([FromForm] EncashmentViewModel model)
        {
            EncashmentViewModel data = new EncashmentViewModel
            {
                encashDate=model.encashDate,
                encashAccount=model.encashAccount,
                encashAmount=model.encashAmount,
                adjustAccount=model.adjustAccount,
                adjustAmount=model.adjustAmount
            };
            return View(data);
        }
         public async Task<IActionResult> ProcessInterest()
        {
            return View();
        }

        public async Task<IActionResult> DeleteInvestment(int id)
        {
            //await stockService.DeleteStockMasterById(id);
            if (id > 0)
            {

                try
                {
                    await pFInvestmentService.DeletePFInvestmentById(id);
                    return Json(new { Success = true, Message = "Deleted Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = "Unable to delete! please try again.. ", exception = ex.Message });
                }
            }

            return Json("ok");
        }
    }
}