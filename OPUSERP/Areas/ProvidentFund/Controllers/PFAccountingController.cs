using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.ProvidentFund.Models;
using Microsoft.AspNetCore.Authorization;


namespace OPUSERP.Areas.ProvidentFund.Controllers
{
	[Authorize]
	[Area("ProvidentFund")]

	public class PFAccountingController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> PFAccOverView()
		{
			return View();
		}

		public async Task<IActionResult> PFAChartAccount()
		{
			return View();
		}
		public async Task<IActionResult> PFInactiveCOA()
		{
			return View();
		}
		public async Task<IActionResult> JournalEntry()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> JournalEntry([FromForm] JournalEntryViewModel model)
		{
			JournalEntryViewModel data = new JournalEntryViewModel
			{
				tranDate = model.tranDate,
				journalNo = model.journalNo,
				description = model.description,
				attachment = model.attachment,
				account = model.account,
				debits = model.debits,
				credits = model.credits,
				tbldescription = model.tbldescription,
				partyRef = model.partyRef,
				projRef = model.projRef
			};

			return View(data);
		}
		public async Task<IActionResult> JournalList()
		{
			return View();
		}
		public async Task<IActionResult> PreviewJournal()
		{
			return View();
		}
		public async Task<IActionResult> EditJournal()
		{
			return View();
		}
		public async Task<IActionResult> ProfitMemberList()
		{
			return View();
		}
		public async Task<IActionResult> DistrbuteProcess()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> DistrbuteProcess([FromForm] ProfitDistributionViewModel model)
		{
			ProfitDistributionViewModel data = new ProfitDistributionViewModel
			{
				profitDistributionId = model.profitDistributionId,
				memberName = model.memberName,
				contribution = model.contribution,
				accumulatedProfit = model.accumulatedProfit,
				surplus = model.surplus,
				profitPeriod = model.profitPeriod
			};

			return View(data);
		}


		public async Task<IActionResult> DistrbuteList()
		{
			return View();
		}

		public async Task<IActionResult> TransactionList()
		{
			return View();
		}
		public async Task<IActionResult> GeneralLedger()
		{
			return View();
		}
		public async Task<IActionResult> MemberLedger()
		{
			return View();
		}
		public async Task<IActionResult> TrialBalance()
		{
			return View();
		}
		public async Task<IActionResult> RevenueStatement()
		{
			return View();
		}
		public async Task<IActionResult> BalanceSheet()
		{
			return View();
		}
		public async Task<IActionResult> ReceiptPayment()
		{
			return View();
		}

		public async Task<IActionResult> ADDcoa()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ADDcoa([FromForm] PFAccountingViewModel model)
		{
			PFAccountingViewModel data = new PFAccountingViewModel
			{
				accountType = model.accountType,
				accountCode = model.accountCode,
				subAccountOf = model.subAccountOf,
				accountName = model.accountName,
				description = model.description
			};
			return View(data);
		}

		public async Task<IActionResult> EditChartAccount()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> EditChartAccount([FromForm] PFAccountingViewModel model)
		{
			return View();
		}

		public async Task<IActionResult> PreviewTransaction()
		{
			return View();
		}
		public async Task<IActionResult> EditTransaction()
		{
			return View();
		}



	}
}