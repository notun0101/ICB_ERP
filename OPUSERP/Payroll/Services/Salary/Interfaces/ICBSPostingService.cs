using OPUSERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.Salary.Interfaces
{
	public interface ICBSPostingService
	{
		Task<string> checkSalaryPostedLog(int hrBranchId, int salaryPeriodId, string stype);
		Task<string> checkSalaryPostedLogByKey(int periodId, int branchId, string type, string uniqueKey);
		Task<IEnumerable<CBSXMLDataViewModel>> GetCBSXMLData(string year, string month, string sbuname, string empCode, string branchCode);
		Task<IEnumerable<CBSXMLDataViewModel>> GetCBSXMLDataHeadOffice(string year, string month, string sbuname, string empCode, string branchCode);
		Task<IEnumerable<CBSXMLDataViewModel>> GetCBSVoucherXMLData(string year, string month, string branchCode);
		Task<IEnumerable<CBSXMLDataViewModel>> GetSalaryAndVoucherCBSXMLData(string year, string month, string branchCode);
		Task<IEnumerable<CBSSalarySheetViewModel>> GetCBSProcessData(int periodId, int branchId, string status);
		Task<IEnumerable<CBSVoucherViewModel>> GetCBSVoucherProcessData(int periodId, int branchId, string status);
		Task<CBSProcessSp> UpdateCBSSalaryStatus(string uniqueKey, string status);
		Task<CBSProcessSp> UpdateCBSVoucherStatus(string uniqueKey, string status);
		Task<CBSProcessSp> UpdateCBSVoucherStatusNew(string uniqueKey, string status, string type);
		Task<IEnumerable<CBSXMLDataViewModel>> GetCBSLoanXMLData(string year, string month, string branchCode);
		Task<CBSProcessSp> UpdateCBSLoanStatus(string uniqueKey, string status);
        Task<CBSProcessSp> SaveCBSPostingLog(string createdBy, int? periodId, int? hrBranchId, int? zoneId, int? officeId, string code, string type, string key, string successStatus, string refCode, string message);
		Task<IEnumerable<CBSLoanViewModel>> GetCBSLoanProcessData(int periodId, int branchId, string status);
		Task<IEnumerable<CBSXMLDataViewModel>> GetCBSOfficerUninonVoucherXMLData(string year, string month, string branchCode);
        Task<IEnumerable<CBSPostedLogViewModel>> GetAllPostedVoucher(int periodId);
		Task<IEnumerable<CBSPostedLogViewModel>> GetAllPostedSalary(int periodId);
		Task<int> GetTypeByPeriodId(int id);
		Task<IEnumerable<CBSPostedLogViewModel>> GetAllPostedUnion(int periodId);
		Task<IEnumerable<CBSVoucherXMLData>> GenerateVoucherXML(int periodId);
		Task<IEnumerable<CBSVoucherXMLData>> GenerateSalaryXML(int periodId);
		Task<IEnumerable<CBSVoucherXMLData>> GenerateUnionXML(int periodId);
		Task<IEnumerable<CBSVoucherXMLDataVm>> GetAllCBSVoucherXmls(int periodId);
		Task<IEnumerable<CBSVoucherXMLDataVm>> GetAllCBSSalaryXmls(int periodId);
		Task<IEnumerable<CBSVoucherXMLDataVm>> GetAllCBSUnionXmls(int periodId);
		Task<IEnumerable<CBSVoucherXMLData>> GenerateHeadOfficePayOrderXML(int periodId);
		Task<IEnumerable<CBSVoucherXMLDataVm>> GetCBSPayOrderXmls(int periodId);
		Task<IEnumerable<CBSPostedLogViewModel>> GetAllPostedPayOrder(int periodId);
        Task<IEnumerable<CBSXMLDataViewModel>> GetIndCBSVoucherXMLData(string year, string month, string empCode);
		Task<CBSVoucherXMLDataVm> GetVoucherXmlInd(int periodId);
		Task<CBSVoucherXMLDataVm> GetSalaryXmlInd(int periodId);
		Task<CBSVoucherXMLDataVm> GetUnionXmlInd(int periodId);
		Task<string> checkSalaryPostedLogInd(int officeId, int salaryPeriodId, string stype);
		Task<int> ProcessAllForIndividual(int periodId);
        Task<IEnumerable<CBSPostingLogViewModel>> GetCBSPostingLogs(int periodId, string type, string status);
		Task<IEnumerable<ProcessedVouchersVm>> GetProcessedVoucher(int periodId);
		Task<IEnumerable<ProcessedSalaryVm>> GetProcessedSalary(int periodId);
		Task<CBSVoucherViewModels> GetVoucherXMLById(string unique);
		Task<CBSVoucherViewModels> GetSalaryXMLById(string unique);
		Task<CBSVoucherViewModels> GetUnionXMLById(string unique);
		Task<CBSVoucherViewModels> GetVoucherXMLByKeyAndId(string unique, int? periodId);
	}
}
