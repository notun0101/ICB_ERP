using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Payroll.Data.Entity.carLoan;
using OPUSERP.Payroll.Data.Entity.homeLoan;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.PF.Interfaces
{
    public interface IPFService
    {
        #region FDR
        Task<int> SaveFdrInvestment(FdrInvestment fdrInvestment);
        Task<IEnumerable<FdrInvestment>> GetAllFdrInvestment();
        Task<FdrInvestment> GetFdrInvestmentById(int id);
        Task<bool> DeleteFdrInvestmentById(int id);
        #endregion
        #region PF Advance
        Task<int> SavePFAdvance(PFLoan pFLoan);
        Task<IEnumerable<PFLoan>> GetAllPFLoan();
        Task<IEnumerable<PFLoan>> GetPFLoanBysalaryPeriodId(int salaryPeriodId);
        Task<IEnumerable<PFLoan>> GetPFLoanByemployeeInfoId(int salaryPeriodId, int employeeInfoId);
        Task<IEnumerable<PFLoan>> GetPFLoanByemployeeId(int employeeInfoId);
        Task<PFLoan> GetPFLoanById(int id);
        Task<bool> DeletePFLoanById(int id);

        #endregion
        #region PF Loan Schedule
        Task<bool> SavePFLoanSchedule(PFLoanSchedule pFLoanSchedule);
        Task<IEnumerable<PFLoanSchedule>> GetAllPFLoanSchedule();
        Task<IEnumerable<PFLoanSchedule>> GetPFLoanScheduleBySalaryPeriodId(int salaryPeriodId);
        Task<IEnumerable<PFLoanSchedule>> GetPFLoanScheduleByemployeeInfoId(int salaryPeriodId, int employeeInfoId);
        Task<PFLoanSchedule> GetPFLoanScheduleById(int id);
        Task<bool> DeletePFLoanScheduleById(int id);
        Task<bool> DeletePFLoanScheduleByPFId(int id);
        Task<IEnumerable<PFLoanScheduleViewModel>> GetLoanScheduleViewModel(int pfLoanId);
        Task<bool> UpdatePFScheduleByPaymentId(int id);
        Task<bool> UpdatePFSchedulePaymentByPaymentId(int id,int installNo,int PaymentId);

        #endregion
        #region PF Loan Payment
        Task<int> SavePFLoanPayment(PFLoanPayment pFLoanPayment);
        Task<IEnumerable<PFLoanPayment>> GetAllPFLoanPayment();
        Task<IEnumerable<PFLoanPayment>> GetPFLoanPaymentByemployeeInfoId(int employeeInfoId);
        Task<bool> DeletePFLoanPaymentById(int id);

		#endregion

		Task<int> GetMaxOrderByLoanId(int id);
		Task<int> SaveLoanLog(LoanLog loanLog);
		Task<IEnumerable<LoansViewModel>> GetOngoingLoanByTypeAndUserId(string type, int userid);
		Task<IEnumerable<PFLoan>> GetLoansByTypeAndStatus(string type, int status);
		Task<LoanLog> GetLoanIdAndUserId(int loanId, int userid);
		Task<LoanLog> GetLoanLogByUserAndLoan(int userid, int loanid);
        Task<PFLoan> GetPFLoanByIdNew(int id);
        Task<IEnumerable<LoansViewModel>> GetForwardedLoanByTypeAndUserId(string type, int userid);

        Task<IEnumerable<LoansViewModel>> GetMyLoanByTypeAndEmpId(string type, string userid);
        Task<IEnumerable<CarLoan>> GetAllCarLoan();
        Task<IEnumerable<HomeLoan>> GetAllHomeLoan();

        Task<int> SaveHomeAdvance(HomeLoan homeLoan);
        Task<int> SaveCarAdvance(CarLoan carLoan);
        Task<bool> DeleteHomeLoanScheduleByHomeLoanId(int id);
        Task<bool> DeleteCarLoanScheduleByCarLoanId(int id);
        Task<bool> SaveCarLoanSchedule(CarLoanSchedule carLoanSchedule);
        Task<bool> SaveHomeLoanSchedule(HomeLoanSchedule homeLoanSchedule);
        Task<IEnumerable<HomeLoan>> GetHomeLoansByTypeAndStatus(string type, int status);
        Task<IEnumerable<CarLoan>> GetCarLoansByTypeAndStatus(string type, int status);
        Task<HomeLoan> GetHomeLoanById(int id);
        Task<CarLoan> GetCarLoanById(int id);

        Task<PFMemberInfo> GetPfMemberByEmpId(int empId);
        Task<int> DeletePfTransactionByEmpIdYearAndMonth(int empId, int yearId, string monthName);
        Task<int> SavePfTransaction(PfTransaction model);
        Task<decimal> GetPfHeadAmountByEmpIdAndHeadName(int empId, string headName);
        Task<PFLoan> GetPfLoanByEmpIdAndPeriodId(int empId, int periodId);
        Task<decimal> GetPfHeadAmountByEmpIdAndHeadName1(int empId, int salaryPeriodId, string headName);
        Task<SalaryPeriod> GetSalaryPeriodById(int id);
        Task<decimal> GetInterestByYear(string year);
        Task<bool> SavePFInterest(PfInterest pfInterest);
        Task<IEnumerable<PfInterest>> GetAllPfInterest();
        Task<bool> DeletePfInterestById(int id);
        Task<IEnumerable<PFLoan>> GetLoansByTypeAndStatus1(string type, int status, int year);
        Task<IEnumerable<ForfeitAccount>> GetPfAccountsByYear(int year);
        Task<IEnumerable<PfAccountsSchedule>> GetNonRefundableContribution(int year);
        Task<IEnumerable<PfAccountsSchedule>> GetFinalSettlementContribution(int year);
        Task<PFMemberInfo> GetPfMemberById(int id);
        Task<PfAccountsSchedule> GetPfAccountByIdAndYear(int pfId, int year);
        Task<IEnumerable<PFEmployeeDetailViewModel>> GetPfDetailByPfIdAndYear(int pfId, int year);
        Task<decimal> GetTotalPfLoanByYearAndPfId(int pfId, int year);
		Task<PfYearlyStatement> GetPfIndividualYearlyStatement(int pfId, int year);
		Task<PfYearlyStatementWithMonth> GetPfIndividualYearlyStatementWithMonth(int pfId, int year, DateTime startMonth, DateTime endMonth);

		Task<PfAccountsSchedule> GetLastAccountScheduleByMemberId(int memberId);
        Task<PfFinalSettlement> GetPfAccountByMemberId(int MemberId);
        Task<IEnumerable<PFBranchWiseMonthlySummeryViewModel>> GetPfBranchWiseMonthlySummery(int year, string month);
        //Task<decimal> GetInterestByYear(string year);
    }
}
