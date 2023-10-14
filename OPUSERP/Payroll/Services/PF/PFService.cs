using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data;
using OPUSERP.Payroll.Data.Entity.carLoan;
using OPUSERP.Payroll.Data.Entity.homeLoan;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.PF.Interfaces;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.PF
{
    public class PFService:IPFService
    {
        private readonly ERPDbContext _context;
        public PFService(ERPDbContext context)
        {
            _context = context;
        }
        #region Pf Transaction
        public async Task<PFMemberInfo> GetPfMemberByEmpId(int empId)
        {
            var data = await _context.pfMemberInfos.Include(x => x.employeeInfo).Where(x => x.employeeInfoId == empId).AsNoTracking().FirstOrDefaultAsync();
            return data;
        }
        public async Task<int> DeletePfTransactionByEmpIdYearAndMonth(int empId, int yearId, string monthName)
        {
            var data = await _context.pfTransactions.Where(x => x.employeeId == empId && x.yearId == yearId && x.month == monthName).FirstOrDefaultAsync();
            if (data != null)
            {
                _context.pfTransactions.Remove(data);
                return await _context.SaveChangesAsync();
            }
            else
            {
                return 1;
            }
        }
        public async Task<int> SavePfTransaction(PfTransaction model)
        {
            if (model.Id != 0)
            {
                _context.pfTransactions.Update(model);
            }
            else
            {
                _context.pfTransactions.Update(model);
            }
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task<decimal> GetPfHeadAmountByEmpIdAndHeadName(int empId, string headName)
        {
            var data = await _context.employeesSalaryStructures.Where(x => x.employeeInfoId == empId && x.salaryHead.salaryHeadName == headName).Select(x => x.amount).FirstOrDefaultAsync();
            return Convert.ToDecimal(data);
        }
        public async Task<PFLoan> GetPfLoanByEmpIdAndPeriodId(int empId, int periodId)
        {
            var data = await _context.PFLoans.Where(x => x.employeeInfoId == empId && x.salaryPeriodId == periodId).FirstOrDefaultAsync();
            return data;
        }
        public async Task<decimal> GetPfHeadAmountByEmpIdAndHeadName1(int empId, int salaryPeriodId, string headName)
        {
            var data = await _context.ProcessEmpSalaryStructures.Where(x => x.employeeInfoId == empId && x.salaryPeriodId == salaryPeriodId && x.salaryHead.salaryHeadName == headName).Select(x => x.amount).FirstOrDefaultAsync();
            return Convert.ToDecimal(data);
        }
        public async Task<SalaryPeriod> GetSalaryPeriodById(int id)
        {
            var data = await _context.salaryPeriods.Include(x => x.salaryYear).Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }
        public async Task<decimal> GetInterestByYear(string year)
        {
            var data = await _context.pfInterests.Where(x => x.year == year).Select(x => x.total).FirstOrDefaultAsync();
            return Convert.ToDecimal(data);
        }
        #endregion

        #region FDR

        public async Task<int> SaveFdrInvestment(FdrInvestment fdrInvestment)
        {
            if (fdrInvestment.Id != 0)
            {
                _context.FdrInvestments.Update(fdrInvestment);
            }
            else
            {
                _context.FdrInvestments.Add(fdrInvestment);
            }
            await _context.SaveChangesAsync();
            return fdrInvestment.Id;
        }

        public async Task<IEnumerable<FdrInvestment>> GetAllFdrInvestment()
        {
            return await _context.FdrInvestments.AsNoTracking().ToListAsync();
        }

        public async Task<FdrInvestment> GetFdrInvestmentById(int id)
        {
            return await _context.FdrInvestments.FindAsync(id);
        }

        public async Task<bool> DeleteFdrInvestmentById(int id)
        {
            _context.FdrInvestments.RemoveRange(_context.FdrInvestments.Where(a => a.Id == id));
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion
        #region PF Advance
		public async Task<int> GetMaxOrderByLoanId(int id)
		{
			var data = await _context.loanLogs.Where(x => x.loanId == id).MaxAsync(x => x.approveOrder);
			return Convert.ToInt32(data);
		}

		public async Task<int> SavePFAdvance(PFLoan pFLoan)
        {
            if (pFLoan.Id != 0)
                _context.PFLoans.Update(pFLoan);
            else
                _context.PFLoans.Add(pFLoan);
            await _context.SaveChangesAsync();
            return pFLoan.Id;
        }


		public async Task<int> SaveLoanLog(LoanLog loanLog)
        {
            if (loanLog.Id != 0)
                _context.loanLogs.Update(loanLog);
            else
                _context.loanLogs.Add(loanLog);
            await _context.SaveChangesAsync();
            return loanLog.Id;
        }
          public async Task<int> SaveCarAdvance(CarLoan carLoan)
        {
            if (carLoan.Id != 0)
                _context.carLoans.Update(carLoan);
            else
                _context.carLoans.Add(carLoan);
            await _context.SaveChangesAsync();
            return carLoan.Id;
        }
          public async Task<int> SaveHomeAdvance(HomeLoan homeLoan)
        {
            if (homeLoan.Id != 0)
                _context.homeLoans.Update(homeLoan);
            else
                _context.homeLoans.Add(homeLoan);
            await _context.SaveChangesAsync();
            return homeLoan.Id;
        }

        public async Task<IEnumerable<PFLoan>> GetAllPFLoan()
        {
            return await _context.PFLoans.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.salaryPeriod).Include(x => x.employeeInfo.currentGrade).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
         public async Task<IEnumerable<CarLoan>> GetAllCarLoan()
        {
            return await _context.carLoans.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.salaryPeriod).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
         public async Task<IEnumerable<HomeLoan>> GetAllHomeLoan()
        {
            return await _context.homeLoans.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.salaryPeriod).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<PFLoan>> GetPFLoanBysalaryPeriodId(int salaryPeriodId)
        {
            return await _context.PFLoans.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId == salaryPeriodId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<PFLoan>> GetPFLoanByemployeeInfoId(int salaryPeriodId, int employeeInfoId)
        {
            return await _context.PFLoans.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId == salaryPeriodId && x.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<PFLoan>> GetPFLoanByemployeeId( int employeeInfoId)
        {
            List<int> lstPF =  _context.PFLoanSchedules.Include(x=>x.pFLoan).Where(x => x.isComplete == 0 && x.pFLoan.employeeInfoId == employeeInfoId).Select(x=>(int)x.pFLoanId).ToList();
            return await _context.PFLoans.Include(x => x.employeeInfo).Include(x => x.salaryPeriod).Where(x =>  x.employeeInfoId == employeeInfoId&&lstPF.Contains(x.Id)).AsNoTracking().ToListAsync();
        }

        public async Task<PFLoan> GetPFLoanById(int id)
        {
            return await _context.PFLoans.FindAsync(id);
        }
        public async Task<PFLoan> GetPFLoanByIdNew(int id)
        {
            return await _context.PFLoans.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo.department).Include(x => x.employeeInfo.currentGrade).Include(x => x.salaryPeriod).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<CarLoan> GetCarLoanById(int id)
        {
            return await _context.carLoans.FindAsync(id);
        }
        public async Task<HomeLoan> GetHomeLoanById(int id)
        {
            return await _context.homeLoans.FindAsync(id);
        }

		public async Task<LoanLog> GetLoanIdAndUserId(int loanId, int userid)
        {
			return await _context.loanLogs.Where(x => x.loanId == loanId && Convert.ToInt32(x.nextapprover) == userid && x.ispassed == 0).OrderBy(x => x.approveOrder).FirstOrDefaultAsync();

        }

        public async Task<bool> DeletePFLoanById(int id)
        {
            _context.PFLoans.Remove(_context.PFLoans.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

		public async Task<IEnumerable<LoansViewModel>> GetOngoingLoanByTypeAndUserId(string type, int userid)
		{
			var data = await (from pl in _context.PFLoans
							  join l in _context.loanLogs
							  on pl.Id equals l.loanId
							  join e in _context.employeeInfos
							  on pl.employeeInfoId equals e.Id
                              //join de in _context.employeeInfos
                              //on de
							  where pl.loanType == type && l.nextapprover == userid && l.ispassed == 0
							  select new LoansViewModel {
								  loan = pl,
								  employee = e
							  }).ToListAsync();
			return data;
		}

        public async Task<IEnumerable<LoansViewModel>> GetMyLoanByTypeAndEmpId(string type, string employeeCode)
		{
			var data = await (from pl in _context.PFLoans
							  join l in _context.loanLogs
							  on pl.Id equals l.loanId
							  join e in _context.employeeInfos
							  on pl.employeeInfoId equals e.Id
                              //join de in _context.employeeInfos
                              //on de
							  where pl.loanType == type && e.employeeCode == employeeCode && l.ispassed == 1
							  select new LoansViewModel {
								  loan = pl,
								  employee = e
							  }).ToListAsync();
			return data;
		}




        public async Task<IEnumerable<LoansViewModel>> GetForwardedLoanByTypeAndUserId(string type, int userid)
        {
            var data = await (from pl in _context.PFLoans
                              join l in _context.loanLogs
                              on pl.Id equals l.loanId
                              join e in _context.employeeInfos
                              on pl.employeeInfoId equals e.Id
                              where pl.loanType == type && pl.forwardedBy == userid
                              select new LoansViewModel
                              {
                                  loan = pl,
                                  employee = e
                              }).ToListAsync();
            return data;
        }
        public async Task<IEnumerable<PFLoan>> GetLoansByTypeAndStatus(string type, int status)
		{
			var data = await _context.PFLoans.Include(x => x.employeeInfo).Include(x=>x.employeeInfo.lastDesignation).Where(x => x.loanType == type && Convert.ToInt32(x.approver) > 0).ToListAsync();
			return data;
		}
        public async Task<IEnumerable<PFLoan>> GetLoansByTypeAndStatus1(string type, int status, int year)
		{
			var data = await _context.PFLoans.Include(x => x.employeeInfo).Include(x=>x.employeeInfo.lastDesignation).Where(x => x.loanType == type && Convert.ToDateTime(x.loanDate).Year == year && Convert.ToInt32(x.approver) > 0).ToListAsync();
			return data;
		}
        public async Task<IEnumerable<PfAccountsSchedule>> GetNonRefundableContribution(int year)
		{
			var data = await _context.pfAccountsSchedules.Include(x => x.employee).Include(x=>x.employee.lastDesignation).Where(x=>x.year==year.ToString()).ToListAsync();
			return data;
		}
        public async Task<IEnumerable<PfAccountsSchedule>> GetFinalSettlementContribution(int year)
        {
            var data = await _context.pfAccountsSchedules.Include(x => x.employee).Include(x=>x.employee.lastDesignation).Where(x=>x.year==year.ToString()).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<CarLoan>> GetCarLoansByTypeAndStatus(string type, int status)
		{
			var data = await _context.carLoans.Include(x => x.employeeInfo).Where(x => x.loanType == type && Convert.ToInt32(x.approver) > 0).ToListAsync();
			return data;
		}
        public async Task<IEnumerable<HomeLoan>> GetHomeLoansByTypeAndStatus(string type, int status)
		{
			var data = await _context.homeLoans.Include(x => x.employeeInfo).Where(x => x.loanType == type && Convert.ToInt32(x.approver) > 0).ToListAsync();
			return data;
		}

		public async Task<LoanLog> GetLoanLogByUserAndLoan(int userid, int loanid)
		{
			var data = await _context.loanLogs.Where(x => x.nextapprover == userid && x.loanId == loanid).FirstOrDefaultAsync();
			return data;
		}

		#endregion
		#region PF Loan Schedule

		public async Task<bool> SavePFLoanSchedule(PFLoanSchedule pFLoanSchedule)
        {
            if (pFLoanSchedule.Id != 0)
                _context.PFLoanSchedules.Update(pFLoanSchedule);
            else
                _context.PFLoanSchedules.Add(pFLoanSchedule);
          return 1 ==  await _context.SaveChangesAsync();
          
        }
         public async Task<bool> SaveCarLoanSchedule(CarLoanSchedule carLoanSchedule)
        {
            if (carLoanSchedule.Id != 0)
                _context.carLoanSchedules.Update(carLoanSchedule);
            else
                _context.carLoanSchedules.Add(carLoanSchedule);
          return 1 ==  await _context.SaveChangesAsync();
          
        }
         public async Task<bool> SaveHomeLoanSchedule(HomeLoanSchedule homeLoanSchedule)
        {
            if (homeLoanSchedule.Id != 0)
                _context.homeLoanSchedules.Update(homeLoanSchedule);
            else
                _context.homeLoanSchedules.Add(homeLoanSchedule);
          return 1 ==  await _context.SaveChangesAsync();
          
        }

        public async Task<IEnumerable<PFLoanSchedule>> GetAllPFLoanSchedule()
        {
            return await _context.PFLoanSchedules.Include(x => x.pFLoan).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<PFLoanSchedule>> GetPFLoanScheduleBySalaryPeriodId(int salaryPeriodId)
        {
            return await _context.PFLoanSchedules.Include(x => x.pFLoan).Where(x => x.salaryPeriodId == salaryPeriodId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<PFLoanSchedule>> GetPFLoanScheduleByemployeeInfoId(int salaryPeriodId, int employeeInfoId)
        {
            return await _context.PFLoanSchedules.Include(x => x.pFLoan.employeeInfo).Where(x => x.salaryPeriodId <= salaryPeriodId && x.pFLoan.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
        }
        public async Task<PFLoanSchedule> GetPFLoanScheduleById(int id)
        {
            return await _context.PFLoanSchedules.FindAsync(id);
        }
        public async Task<IEnumerable<PFLoanScheduleViewModel>> GetLoanScheduleViewModel(int pfLoanId)
        {
            PFLoan pfdata =  _context.PFLoans.Where(x => x.Id == pfLoanId).FirstOrDefault();
            List<PFLoanScheduleViewModel> data = new List<PFLoanScheduleViewModel>();
            List<SalaryPeriod> salaryPeriods = await _context.salaryPeriods.Include(x => x.salaryYear).ToListAsync();
            List<PFLoanSchedule> pFLoanSchedules =await _context.PFLoanSchedules.Include(x=>x.pFLoan.employeeInfo).Where(x => x.pFLoanId == pfLoanId).ToListAsync();
            foreach (PFLoanSchedule d in pFLoanSchedules)
            {
                string periodName = salaryPeriods.Where(x => x.Id == d.salaryPeriodId)?.FirstOrDefault()?.periodName;
                string status = string.Empty;
                if (d.isComplete == 1)
                {
                    status = "Paid";
                }
                else
                {
                    status = "Not Paid";
                }
                if (periodName == null)
                {
                    periodName = "";
                }
                data.Add(new PFLoanScheduleViewModel
                {
                    periodName=periodName,
                    noOfInstallment=d.noOfInstallment,
                    advanceAmount=Convert.ToDecimal(pfdata.advanceAmount),
                    installmentAmount=d.installmentAmount,
                    restAmount=d.restAmount,
                    status=status

                });
            }
            return data;

        }
        public async Task<bool> UpdatePFScheduleByPaymentId(int id)
        {
           IEnumerable<PFLoanSchedule> pFLoanSchedule = await _context.PFLoanSchedules.Where(x=>x.PFLoanPaymentId==id).ToListAsync();
            foreach (PFLoanSchedule data in pFLoanSchedule)
            {
                data.isComplete = 0;
                data.PFLoanPaymentId = null;
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> UpdatePFSchedulePaymentByPaymentId(int id, int installNo, int paymentId)
        {
            int pFLoanScheduleId = await _context.PFLoanSchedules.Where(x => x.pFLoanId == id &&x.isComplete==0).Select(x=>x.Id).FirstOrDefaultAsync();
            for (int i = 0; i < installNo; i++)
            {
                PFLoanSchedule data = await _context.PFLoanSchedules.Where(x => x.Id == pFLoanScheduleId).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.isComplete = 1;
                    data.PFLoanPaymentId = paymentId;
                    await _context.SaveChangesAsync();
                }
                pFLoanScheduleId++;
            }
            return true;
        }
        public async Task<bool> DeletePFLoanScheduleById(int id)
        {
            _context.PFLoanSchedules.Remove(_context.PFLoanSchedules.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteCarLoanScheduleByCarLoanId(int id)
        {
            _context.carLoanSchedules.RemoveRange(_context.carLoanSchedules.Where(x=>x.carLoanID==id).ToList());
            return 1 == await _context.SaveChangesAsync();
        }
         public async Task<bool> DeleteHomeLoanScheduleByHomeLoanId(int id)
        {
            _context.homeLoanSchedules.RemoveRange(_context.homeLoanSchedules.Where(x=>x.homeLoanID==id).ToList());
            return 1 == await _context.SaveChangesAsync();
        }
         public async Task<bool> DeletePFLoanScheduleByPFId(int id)
        {
            _context.PFLoanSchedules.RemoveRange(_context.PFLoanSchedules.Where(x=>x.pFLoanId==id).ToList());
            return 1 == await _context.SaveChangesAsync();
        }


        #endregion
        #region PF Loan Payment

        public async Task<int> SavePFLoanPayment(PFLoanPayment pFLoanPayment)
        {
            if (pFLoanPayment.Id != 0)
                _context.PFLoanPayments.Update(pFLoanPayment);
            else
                _context.PFLoanPayments.Add(pFLoanPayment);
           await _context.SaveChangesAsync();
            return pFLoanPayment.Id;

        }

        public async Task<IEnumerable<PFLoanPayment>> GetAllPFLoanPayment()
        {
            return await _context.PFLoanPayments.Include(x => x.employeeInfo).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        
        public async Task<IEnumerable<PFLoanPayment>> GetPFLoanPaymentByemployeeInfoId(int employeeInfoId)
        {
            return await _context.PFLoanPayments.Include(x => x.employeeInfo).Where(x =>x.employeeInfoId == employeeInfoId).AsNoTracking().ToListAsync();
        }
        
        public async Task<bool> DeletePFLoanPaymentById(int id)
        {
            _context.PFLoanPayments.Remove(_context.PFLoanPayments.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion

        public async Task<bool> SavePFInterest(PfInterest pfInterest)
        {
            if (pfInterest.Id != 0)
                _context.pfInterests.Update(pfInterest);
            else
                _context.pfInterests.Add(pfInterest);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PfInterest>> GetAllPfInterest()
        {
            return await _context.pfInterests.ToListAsync();
        }

        public async Task<bool> DeletePfInterestById(int id)
        {
            _context.pfInterests.Remove(_context.pfInterests.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ForfeitAccount>> GetPfAccountsByYear(int year)
        {
            var data = await _context.forfeitAccounts.Include(x=>x.employee).Include(x=>x.pf).Where(x => Convert.ToDateTime(x.forfeitDate).Year == year).AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<PfFinalSettlement> GetPfAccountByMemberId(int MemberId)
        {
            return await _context.pfFinalSettlements.Where(x => x.pfMemberId == MemberId).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetTotalPfLoanByYearAndPfId(int pfId, int year)
        {
            var data = await _context.pfAccountsSchedules.Where(x => x.pfMemberId == pfId && x.year == year.ToString()).FirstOrDefaultAsync();
			var totalPfOwn = data.januaryOwn + data.februaryOwn + data.marchOwn + data.aprilOwn + data.mayOwn + data.juneOwn + data.julyOwn + data.augustOwn + data.septemberOwn + data.octoberOwn + data.novemberOwn + data.decemberOwn;
			return Convert.ToDecimal(totalPfOwn);
        }
		public async Task<PfAccountsSchedule> GetPfAccountByIdAndYear(int pfId, int year)
        {
            var data = await _context.pfAccountsSchedules.Where(x => x.pfMemberId == pfId && x.year == year.ToString()).FirstOrDefaultAsync(); 
            return data;
        }
		public async Task<PfYearlyStatement> GetPfIndividualYearlyStatement(int pfId, int year)
		{
			var data = await _context.pfYearlyStatements.FromSql($"sp_GetIndividualYearlyStatement {year},{pfId}").AsNoTracking().FirstOrDefaultAsync();
			return data;
		}
		public async Task<PfYearlyStatementWithMonth> GetPfIndividualYearlyStatementWithMonth(int pfId, int year, DateTime startMonth, DateTime endMonth)
		{
			var data = await _context.pfYearlyStatementsWithMonth.FromSql($"sp_GetIndividualYearlyStatementWithMonth {year},{pfId},{startMonth.ToString("ddMMyyyy")},{endMonth.ToString("ddMMyyyy")}").AsNoTracking().FirstOrDefaultAsync();
			return data;
		}
		public async Task<IEnumerable<PFEmployeeDetailViewModel>> GetPfDetailByPfIdAndYear(int pfId, int year)
        {
            var data =  await _context.PFEmployeeDetailViewModels.FromSql($"PFEmpDetails {pfId},{year}").AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<IEnumerable<PFBranchWiseMonthlySummeryViewModel>> GetPfBranchWiseMonthlySummery(int year, string month)
        {
            var data = await _context.PFBranchWiseMonthlySummeryViewModels.FromSql($"PFBranchWiseMonthlySummery {month},{year}").AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<PFMemberInfo> GetPfMemberById(int id)
        {
            var data = await _context.pfMemberInfos.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo.department).Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }
        public async Task<PfAccountsSchedule> GetLastAccountScheduleByMemberId(int memberId)
        {
            var data = await _context.pfAccountsSchedules.Where(x => x.pfMemberId == memberId).LastOrDefaultAsync();
            return data;
        }
        //public async Task<decimal> GetInterestByYear(string year)
        //{
        //    var data = await _context.pfInterests.Where(x => x.year == year).Select(x => x.total).FirstOrDefaultAsync();
        //    return Convert.ToDecimal(data);
        //}
    }
}
