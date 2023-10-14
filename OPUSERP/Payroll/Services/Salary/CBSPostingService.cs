using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.Models;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.Salary
{
	public class CBSPostingService : ICBSPostingService
	{
		private readonly ERPDbContext _context;

		public CBSPostingService(ERPDbContext context)
		{
			_context = context;
		}

		public async Task<int> ProcessAllForIndividual(int periodId)
		{
			var officeId = 11;
			var yearname = await _context.salaryPeriods.Where(x => x.Id == periodId).Select(x => x.salaryYear.yearName).AsNoTracking().FirstOrDefaultAsync();
			var monthname = await _context.salaryPeriods.Where(x => x.Id == periodId).Select(x => x.monthName).AsNoTracking().FirstOrDefaultAsync();

			var data = await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalaryVoucherInd {periodId}, {officeId}").AsNoTracking().ToListAsync();
			var data2 = await _context.cBSVoucherXMLDatas.FromSql($"sp_GenerateVoucherXMLInd {periodId}").AsNoTracking().ToListAsync();
			var data1 = await _context.cBSProcessSps.FromSql($"sp_CBSProcessSalarySheetInd {periodId}").AsNoTracking().ToListAsync();
			var data5 = await _context.cBSVoucherXMLDatas.FromSql($"sp_GenerateSalaryXMLInd {periodId}").AsNoTracking().ToListAsync();
			var data3 = await _context.cBSXMLDataViewModels.FromSql($"sp_PostVoucherByBranchOfficerUnionInd {yearname}, {monthname}").AsNoTracking().ToListAsync();
			var data4 = await _context.cBSXMLDataViewModels.FromSql($"sp_GenerateUnionXMLInd {periodId}").AsNoTracking().ToListAsync();
			return 1;
		}

		public async Task<string> checkSalaryPostedLogByKey(int periodId, int branchId, string type, string uniqueKey)
		{
			string data = await _context.salaryPostingLogs.Where(x => x.key == uniqueKey && x.periodId == periodId && x.hrBranchId == branchId && x.type == type).Select(x => x.refCode).FirstOrDefaultAsync();
			return data;
		}
		public async Task<string> checkSalaryPostedLog(int hrBranchId, int salaryPeriodId, string stype)
		{
			string data = "";
			if (stype == "INDVOUCHER" || stype == "INDSALARY" || stype == "INDUNION")
			{
				data = await _context.salaryPostingLogs.Where(x => x.officeId == hrBranchId && x.periodId == salaryPeriodId && x.type == stype && x.successStatus == "Y").Select(x => x.refCode).FirstOrDefaultAsync();
			}
			else
			{
				data = await _context.salaryPostingLogs.Where(x => x.periodId == salaryPeriodId && x.type == stype && x.successStatus == "Y" && (x.hrBranchId == hrBranchId || x.zoneId == hrBranchId)).Select(x => x.refCode).FirstOrDefaultAsync();
			}
			return data;
		}
		public async Task<string> checkSalaryPostedLogInd(int officeId, int salaryPeriodId, string stype)
		{
			var data = await _context.salaryPostingLogs.Where(x => x.officeId == officeId && x.periodId == salaryPeriodId && x.type == stype && x.successStatus == "Y").Select(x => x.refCode).FirstOrDefaultAsync();
			return data;
		}
		public async Task<IEnumerable<CBSXMLDataViewModel>> GetCBSXMLData(string year, string month, string sbuname, string empCode, string branchCode)
		{
			return await _context.cBSXMLDataViewModels.FromSql($"sp_GetNetPayByYearMonthAndSBUCBSPosting {year}, {month}, {sbuname}, {empCode}, {branchCode}").AsNoTracking().ToListAsync();
		}
		public async Task<CBSVoucherViewModels> GetVoucherXMLById(string unique)
		{
			return await _context.cBSVoucherViewModels.FromSql($"sp_GetVoucherXMLById {unique}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSVoucherViewModels> GetVoucherXMLByKeyAndId(string unique, int? periodId)
		{
			return await _context.cBSVoucherViewModels.FromSql($"sp_GetVoucherXMLByKeyAndId {unique}, {periodId}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSVoucherViewModels> GetSalaryXMLById(string unique)
		{
			return await _context.cBSVoucherViewModels.FromSql($"sp_GetSalaryXMLById {unique}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSVoucherViewModels> GetUnionXMLById(string unique)
		{
			return await _context.cBSVoucherViewModels.FromSql($"sp_GetUnionXMLById {unique}").AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<CBSVoucherXMLDataVm>> GetAllCBSVoucherXmls(int periodId)
		{
			var period = await _context.salaryPeriods.Where(x => x.Id == periodId).FirstOrDefaultAsync();
			if (period.salaryTypeId == 1)
			{
				return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetAllVoucherXmls {periodId}").AsNoTracking().ToListAsync();
			}
			else
			{
				return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetAllVoucherXmlsBonus {periodId}").AsNoTracking().ToListAsync();
			}
		}
		public async Task<IEnumerable<CBSVoucherXMLDataVm>> GetAllCBSSalaryXmls(int periodId)
		{
			var period = await _context.salaryPeriods.Where(x => x.Id == periodId).FirstOrDefaultAsync();
			if (period.salaryTypeId == 1)
			{
				return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetAllSalaryXmls {periodId}").AsNoTracking().ToListAsync();
			}
			else
			{
				return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetAllSalaryXmls {periodId}").AsNoTracking().ToListAsync();
			}
		}
		public async Task<CBSVoucherXMLDataVm> GetVoucherXmlInd(int periodId)
		{
			return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetVoucherXmlInd {periodId}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSVoucherXMLDataVm> GetSalaryXmlInd(int periodId)
		{
			return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetSalaryXmlInd {periodId}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<CBSPostingLogViewModel>> GetCBSPostingLogs(int periodId, string type, string status)
		{
			return await _context.cBSPostingLogViewModels.FromSql($"sp_GetPostingLogs {periodId}, {type}, {status}").AsNoTracking().ToListAsync();
		}
		public async Task<CBSVoucherXMLDataVm> GetUnionXmlInd(int periodId)
		{
			return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetUnionXmlInd {periodId}").AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<CBSVoucherXMLDataVm>> GetAllCBSUnionXmls(int periodId)
		{
			return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetAllUnionXmls {periodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSVoucherXMLDataVm>> GetCBSPayOrderXmls(int periodId)
		{
			return await _context.cBSVoucherXMLDataVms.FromSql($"sp_GetCBSPayOrderXmls {periodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSVoucherXMLData>> GenerateVoucherXML(int periodId)
		{
			var period = await _context.salaryPeriods.Where(x => x.Id == periodId).FirstOrDefaultAsync();
			if (period.salaryTypeId == 2)
			{
				return await _context.cBSVoucherXMLDatas.FromSql($"sp_GenerateAllVoucherXMLBonus {periodId}").AsNoTracking().ToListAsync();
			}
			else
			{
				return await _context.cBSVoucherXMLDatas.FromSql($"sp_GenerateAllVoucherXML {periodId}").AsNoTracking().ToListAsync();
			}
		}
		public async Task<IEnumerable<CBSVoucherXMLData>> GenerateHeadOfficePayOrderXML(int periodId)
		{
			return await _context.cBSVoucherXMLDatas.FromSql($"sp_PostHeadOfficePayOrder {periodId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<CBSVoucherXMLData>> GenerateSalaryXML(int periodId)
		{
			var period = await _context.salaryPeriods.Where(x => x.Id == periodId).FirstOrDefaultAsync();
			if (period.salaryTypeId == 2)
			{
				return await _context.cBSVoucherXMLDatas.FromSql($"sp_GenerateAllSalaryXMLBonus {periodId}").AsNoTracking().ToListAsync();
			}
			else
			{
				return await _context.cBSVoucherXMLDatas.FromSql($"sp_GenerateAllSalaryXML {periodId}").AsNoTracking().ToListAsync();
			}
		}

		public async Task<IEnumerable<CBSVoucherXMLData>> GenerateUnionXML(int periodId)
		{
			var period = await _context.salaryPeriods.Where(x => x.Id == periodId).FirstOrDefaultAsync();
			if (period.salaryTypeId == 1)
			{
				return await _context.cBSVoucherXMLDatas.FromSql($"sp_GenerateAllUnionXML {periodId}").AsNoTracking().ToListAsync();
			}
			else
			{
				return new List<CBSVoucherXMLData>();
			}
		}


		public async Task<IEnumerable<CBSXMLDataViewModel>> GetCBSXMLDataHeadOffice(string year, string month, string sbuname, string empCode, string branchCode)
		{
			return await _context.cBSXMLDataViewModels.FromSql($"sp_GetNetPayByYearMonthAndSBUCBSPostingHeadOffice {year}, {month}, {sbuname}, {empCode}, {branchCode}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<CBSPostedLogViewModel>> GetAllPostedVoucher(int periodId)
		{
			return await _context.cBSPostedLogViewModels.FromSql($"sp_GetAllPostedVoucher {periodId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<CBSPostedLogViewModel>> GetAllPostedSalary(int periodId)
		{
			return await _context.cBSPostedLogViewModels.FromSql($"sp_GetAllPostedSalary {periodId}").AsNoTracking().ToListAsync();
		}
		public async Task<int> GetTypeByPeriodId(int id)
		{
			var data = await _context.salaryPeriods.Where(x => x.Id == id).Select(x => x.salaryTypeId).FirstOrDefaultAsync();
			return data;
		}
		public async Task<IEnumerable<CBSPostedLogViewModel>> GetAllPostedUnion(int periodId)
		{
			return await _context.cBSPostedLogViewModels.FromSql($"sp_GetAllPostedUnion {periodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSPostedLogViewModel>> GetAllPostedPayOrder(int periodId)
		{
			return await _context.cBSPostedLogViewModels.FromSql($"sp_GetAllPostedPayOrder {periodId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<ProcessedVouchersVm>> GetProcessedVoucher(int periodId)
		{
			return await _context.processedVouchersVms.FromSql($"sp_GetProcessedVoucher {periodId}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<ProcessedSalaryVm>> GetProcessedSalary(int periodId)
		{
			return await _context.processedSalaryVms.FromSql($"sp_GetProcessedSalary {periodId}").AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<CBSXMLDataViewModel>> GetSalaryAndVoucherCBSXMLData(string year, string month, string branchCode)
		{
			return await _context.cBSXMLDataViewModels.FromSql($"sp_PostVoucherByBranchTogether {year}, {month}, {branchCode}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSXMLDataViewModel>> GetCBSVoucherXMLData(string year, string month, string branchCode)
		{
			return await _context.cBSXMLDataViewModels.FromSql($"sp_PostVoucherByBranch {year}, {month}, {branchCode}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSXMLDataViewModel>> GetIndCBSVoucherXMLData(string year, string month, string empCode)
		{
			return await _context.cBSXMLDataViewModels.FromSql($"sp_PostIndVoucher {year}, {month}, {empCode}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSXMLDataViewModel>> GetCBSOfficerUninonVoucherXMLData(string year, string month, string branchCode)
		{
			return await _context.cBSXMLDataViewModels.FromSql($"sp_PostVoucherByBranchOfficerUnion {year}, {month}, {branchCode}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSXMLDataViewModel>> GetCBSLoanXMLData(string year, string month, string branchCode)
		{
			return await _context.cBSXMLDataViewModels.FromSql($"sp_PostLoanByBranch {year}, {month}, {branchCode}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSSalarySheetViewModel>> GetCBSProcessData(int periodId, int branchId, string status)
		{
			return await _context.cBSSalarySheets.FromSql($"sp_CBSSalarySheets {periodId}, {branchId}, {status}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSVoucherViewModel>> GetCBSVoucherProcessData(int periodId, int branchId, string status)
		{
			return await _context.cBSVouchers.FromSql($"sp_CBSVouchers {periodId}, {branchId}, {status}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<CBSLoanViewModel>> GetCBSLoanProcessData(int periodId, int branchId, string status)
		{
			return await _context.cBSLoans.FromSql($"sp_CBSLoans {periodId}, {branchId}, {status}").AsNoTracking().ToListAsync();
		}

		public async Task<CBSProcessSp> UpdateCBSSalaryStatus(string uniqueKey, string status)
		{
			return await _context.cBSProcessSps.FromSql($"sp_UpdateCBSSalaryStatus {uniqueKey}, {status}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSProcessSp> UpdateCBSVoucherStatus(string uniqueKey, string status)
		{
			return await _context.cBSProcessSps.FromSql($"sp_UpdateCBSVoucherStatus {uniqueKey}, {status}").AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<CBSProcessSp> UpdateCBSVoucherStatusNew(string uniqueKey, string status, string type)
		{
			return await _context.cBSProcessSps.FromSql($"UpdateCBSVoucherStatusNew {uniqueKey}, {status}, {type}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSProcessSp> SaveCBSPostingLog(string createdBy, int? periodId, int? hrBranchId, int? zoneId, int? officeId, string code, string type, string key, string successStatus, string refCode, string message)
		{
			return await _context.cBSProcessSps.FromSql($"sp_SaveCBSPostingLog {createdBy}, {periodId}, {hrBranchId}, {zoneId}, {officeId}, {code}, {type}, {key}, {successStatus}, {refCode}, {message}").AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<CBSProcessSp> UpdateCBSLoanStatus(string uniqueKey, string status)
		{
			return await _context.cBSProcessSps.FromSql($"sp_UpdateCBSLoanStatus {uniqueKey}, {status}").AsNoTracking().FirstOrDefaultAsync();
		}
	}
}
