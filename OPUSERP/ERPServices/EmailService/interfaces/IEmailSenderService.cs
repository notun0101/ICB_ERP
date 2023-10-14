using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.SCM.Data.Entity.Matrix;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.ERPServices.EmailService.interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmail(string mailTo,string subject, string message);
        Task SendEmailWithFrom(string mailTo, string name, string subject, string message);
        Task<bool> SaveMailLog(MailLog mailLog);
		Task SendEmailWithAttatchment(string to, string subject, string body, string path);
        Task<SendEmailLogStatus> GetEmailLogStatusByEmpId(int empId, int periodId);
        Task<SendEmailLogStatus> getAttatchmentUrlByEmpId(int empId, int periodId);
        Task<IEnumerable<EmailStatusViewModel>> GetEmailSendingStatusByPeriodId(int periodId);
        Task<SendEmailLogStatus> GetEmailSendingLogById(int id);
		Task<int> DeleteEmailLogByEmployeeId(int periodId, int? employeeId);
		Task<int> DeleteEmailLogByDesignationId(int periodId, int? designationId);

		Task<int> DeleteEmailLogByBranchId(int periodId, int? branchId);
		Task<int> DeleteAllEmailLogByPeriodId(int periodId);

	}
}
