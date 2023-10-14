using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.ERPServices.EmailService.interfaces;
using OPUSERP.SCM.Data.Entity.Matrix;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OPUSERP.ERPServices.EmailService
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly ERPDbContext _context;

        public EmailSenderService(IConfiguration configuration, ERPDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<IEnumerable<EmailStatusViewModel>> GetEmailSendingStatusByPeriodId(int periodId)
        {
            return await _context.emailStatusViewModels.FromSql($"sp_GetEmailSendingStatusByPeriodId {periodId}").AsNoTracking().ToListAsync();
        }

        public async Task<SendEmailLogStatus> GetEmailSendingLogById(int id)
        {
            var data = await _context.SendEmailLogStatus.Include(x => x.employee).AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }

        public async Task SendEmail(string mailTo, string subject, string message)
        {
            string userName = _configuration["Email:Email"];
            string password = _configuration["Email:Password"];
            string host = _configuration["Email:Host"];
            int port = int.Parse(_configuration["Email:Port"]);
            string mailFrom = _configuration["Email:Email"];
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName =userName,
                    Password = password
                };

                client.Credentials = credential;
                client.Host = host;
                client.Port = port;
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(mailTo));
                    emailMessage.From = new MailAddress(mailFrom);
                    emailMessage.Subject = subject;
                    emailMessage.Body = message;
                    emailMessage.IsBodyHtml = true;
                    client.Send(emailMessage);
                }
            }
            await Task.CompletedTask;
        }

        //Asad Added On 08.08.2023
		public async Task<int> DeleteEmailLogByEmployeeId(int periodId, int? employeeId)
		{
			var data = await _context.SendEmailLogStatus.Where(x => x.PeriodId == periodId && x.employeeId == employeeId).AsNoTracking().ToListAsync();
			_context.SendEmailLogStatus.RemoveRange(data);
			return await _context.SaveChangesAsync();
		}

		//Asad Added On 08.08.2023
		public async Task<int> DeleteEmailLogByDesignationId(int periodId, int? designationId)
		{
			var data = await _context.SendEmailLogStatus.Where(x => x.PeriodId == periodId && x.DesignationId == designationId).AsNoTracking().ToListAsync();
			_context.SendEmailLogStatus.RemoveRange(data);
			return await _context.SaveChangesAsync();
		}
		//Asad Added On 08.08.2023
		public async Task<int> DeleteEmailLogByBranchId(int periodId, int? branchId)
		{
			var data = await _context.SendEmailLogStatus.Where(x => x.PeriodId == periodId && x.BranchId == branchId).AsNoTracking().ToListAsync();
			_context.SendEmailLogStatus.RemoveRange(data);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteAllEmailLogByPeriodId(int periodId)
		{
			var data = await _context.SendEmailLogStatus.Where(x => x.isDelete == periodId).AsNoTracking().ToListAsync();
			_context.SendEmailLogStatus.RemoveRange(data);
			return await _context.SaveChangesAsync();
		}

        //Asad Added On 10.08.2023
        public async Task<SendEmailLogStatus> GetEmailLogStatusByEmpId(int empId, int periodId)
        {
            var data = await _context.SendEmailLogStatus.Where(x => x.employeeId == empId && x.PeriodId == periodId).FirstOrDefaultAsync();
            return data;
        }
        public async Task<SendEmailLogStatus> getAttatchmentUrlByEmpId(int empId, int periodId)
        {
            var data = await _context.SendEmailLogStatus.Where(x => x.employeeId == empId && x.isDelete == periodId).FirstOrDefaultAsync();
            return data;
        }


        public async Task SendEmailWithAttatchment(string to, string subject, string body, string path)
		{
			string fromMail = _configuration["Email:Email"];
			string password = _configuration["Email:Password"];
			string host = _configuration["Email:Host"];
			int port = int.Parse(_configuration["Email:Port"]);
			string mailFrom = _configuration["Email:Email"];


			Attachment attach = new Attachment(path);

			MailMessage msg = new MailMessage();



			msg.From = new MailAddress(fromMail);

			msg.To.Add(to);

			msg.Subject = subject;

			msg.Body = body;
			msg.IsBodyHtml = true;

			msg.Attachments.Add(attach);



			SmtpClient smtp = new SmtpClient();

			smtp.Host = host;

			NetworkCredential credential = new NetworkCredential();

			credential.UserName = fromMail;

			credential.Password = password;

			smtp.Credentials = credential;

			smtp.EnableSsl = true;

			smtp.Port = port;

            try
            {
                smtp.Send(msg);
            }
            catch (System.Exception ex)
            {

                throw;
            }

			await Task.CompletedTask;
		}


        public async Task SendEmailWithFrom(string mailTo, string name, string subject, string message)
        {
            try
            {
                string userName = _configuration["Email:Email"];
                string password = _configuration["Email:Password"];
                string host = _configuration["Email:Host"];
                int port = int.Parse(_configuration["Email:Port"]);
                string mailFrom = _configuration["Email:Email"];
                using (var client = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = userName,
                        Password = password
                    };

                    client.Credentials = credential;
                    client.Host = host;
                    client.Port = port;
                    client.EnableSsl = true;

                    using (var emailMessage = new MailMessage())
                    {
                        emailMessage.To.Add(new MailAddress(mailTo));
                        emailMessage.From = new MailAddress(mailFrom, name);
                        emailMessage.Subject = subject;
                        emailMessage.Body = message;
                        emailMessage.IsBodyHtml = true;
                        client.Send(emailMessage);
                    }
                }
                await Task.CompletedTask;
            }
            catch (System.Exception ex)
            {

            }
        }

        public async Task<bool> SaveMailLog(MailLog mailLog)
        {
            if (mailLog.Id != 0)
                _context.MailLogs.Update(mailLog);
            else
                _context.MailLogs.Add(mailLog);

            return 1 == await _context.SaveChangesAsync();
        }
    }
}
