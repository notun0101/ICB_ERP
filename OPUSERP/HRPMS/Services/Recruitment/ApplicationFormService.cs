using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Recruitment;
using OPUSERP.HRPMS.Services.Recruitment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Payroll.Data.Entity.Salary;

namespace OPUSERP.HRPMS.Services.Recruitment
{
    public class ApplicationFormService : IApplicationFormService
    {
        private readonly ERPDbContext _context;

        public ApplicationFormService(ERPDbContext context)
        {
            _context = context;
        }

        //ApplicationForm
        public async Task<bool> DeleteApplicationFormById(int id)
        {
            _context.applicationForms.Remove(_context.applicationForms.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationForm>> GetApplicationForm()
        {
            return await _context.applicationForms.Include(x => x.newBranch).Include(x => x.newDesignation).Include(x => x.salaryGrade).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ApplicantAddress>> GetApplicantAddress()
        {
            return await _context.applicantAddresses.AsNoTracking().ToListAsync();
        }
		public async Task<IEnumerable<ApplicationForm>> GetAllApplicationForm()
        {
            return await _context.applicationForms.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SalaryGrade>> GetAllSalaryGrade()
        {
            return await _context.salaryGrades.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ApplicationEdu>> GetApplicationEdu()
        {
            return await _context.applicationEdus.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ApplicationExp>> GetApplicationExp()
        {
            return await _context.applicationExps.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ApplicationQuota>> GetApplicationQuota()
        {
            return await _context.applicationQuotas.AsNoTracking().ToListAsync();
        }

        public async Task<ApplicationForm> GetApplicationFormById(int id)
        {
            return await _context.applicationForms.Where(x => x.Id == id).Include(x => x.religion).FirstAsync();
        }

        public async Task<IEnumerable<ApplicationForm>> GetApplicationFormByStatus(int status)
        {
         
           return await _context.applicationForms.Where(x => x.status == status).Include(x => x.religion).Include(x => x.newDesignation).AsNoTracking().ToListAsync();
        }

        public async Task<int> SaveApplicationForm(ApplicationForm applicationForm)
        {
            if (applicationForm.Id != 0)
                _context.applicationForms.Update(applicationForm);
            else
                _context.applicationForms.Add(applicationForm);
            await _context.SaveChangesAsync();
            return applicationForm.Id;
        }
         public async Task<int> SaveApplicantAddress(ApplicantAddress applicantAddress)
        {
            if (applicantAddress.Id != 0)
                _context.applicantAddresses.Update(applicantAddress);
            else
                _context.applicantAddresses.Add(applicantAddress);
            await _context.SaveChangesAsync();
            return applicantAddress.Id;
        }
          public async Task<int> SaveApplicationEdu(ApplicationEdu applicationEdu)
        {
            if (applicationEdu.Id != 0)
                _context.applicationEdus.Update(applicationEdu);
            else
                _context.applicationEdus.Add(applicationEdu);
            await _context.SaveChangesAsync();
            return applicationEdu.Id;
        }
          public async Task<int> SaveApplicationExp(ApplicationExp applicationExp)
        {
            if (applicationExp.Id != 0)
                _context.applicationExps.Update(applicationExp);
            else
                _context.applicationExps.Add(applicationExp);
            await _context.SaveChangesAsync();
            return applicationExp.Id;
        }
          public async Task<int> SaveApplicationQuota(ApplicationQuota applicationQuota)
        {
            if (applicationQuota.Id != 0)
                _context.applicationQuotas.Update(applicationQuota);
            else
                _context.applicationQuotas.Add(applicationQuota);
            await _context.SaveChangesAsync();
            return applicationQuota.Id;
        }
         

        //JobCircular
        public async Task<bool> DeleteCreateJobCircularById(int id)
        {
            _context.jobCirculars.Remove(_context.jobCirculars.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobCircular>> GetCreateJobCircular(string status)
        {
            return await _context.jobCirculars.Where(x => x.status == status).AsNoTracking().ToListAsync();
        }

        public async Task<JobCircular> GetCreateJobCircularById(int id)
        {
            return await _context.jobCirculars.Where(x => x.Id == id).FirstAsync();
        }

        public async Task<int> SaveCreateJobCircular(JobCircular jobCircular)
        {
            if (jobCircular.Id != 0)
                _context.jobCirculars.Update(jobCircular);
            else
                _context.jobCirculars.Add(jobCircular);
            await _context.SaveChangesAsync();
            return jobCircular.Id;
        }

        public async Task<bool> UpdateJobCircular(int Id, string Type)
        {
            JobCircular data =  await _context.jobCirculars.FindAsync(Id);
            if(data != null)
            {
                data.status = Type;
                return 1 == await _context.SaveChangesAsync();
            }
            return false;
        }

        public async Task<ApplicantAddress> GetAddressByTypeAndapplicationFormId(int FormId, string type)
        {
            return await _context.applicantAddresses.Where(x => x.applicationFormId == FormId && x.type == type).Include(x => x.division).Include(x => x.district).Include(x => x.thana).Include(x => x.applicationForm).FirstOrDefaultAsync();
        }
         public async Task<ApplicationEdu> GetApplicationEduByapplicationFormId(int FormId)
        {
            return await _context.applicationEdus.Where(x => x.applicationFormId == FormId ).Include(x => x.applicationForm).FirstOrDefaultAsync();
        }
         public async Task<ApplicationExp> GetApplicationExpByapplicationFormId(int FormId)
        {
            return await _context.applicationExps.Where(x => x.applicationFormId == FormId ).Include(x => x.applicationForm).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ApplicationExp>> GetApplicationExpByapplicationFormId1(int FormId)
        {
            return await _context.applicationExps.Where(x => x.applicationFormId == FormId ).Include(x => x.applicationForm).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ApplicationEdu>> GetEduByapplicationFormId(int FormId)
        {
            return await _context.applicationEdus.Where(x => x.applicationFormId == FormId ).Include(x => x.applicationForm).AsNoTracking().ToListAsync();
        }
         public async Task<ApplicationQuota> GetApplicationQuotaByapplicationFormId(int FormId)
        {
            return await _context.applicationQuotas.Where(x => x.applicationFormId == FormId).Include(x => x.applicationForm).FirstOrDefaultAsync();
        }


        public async Task<string> GetApplicationFormpayDocById(int id)
        {
            return await _context.applicationForms.Where(x => x.Id == id).Select(x => x.payDoc).FirstOrDefaultAsync();
        }
        public async Task<string> GetApplicationFormapplicantPhotoById(int id)
        {
            return await _context.applicationForms.Where(x => x.Id == id).Select(x => x.applicantPhoto).FirstOrDefaultAsync();
        }
        public async Task<string> GetApplicationFormSignatureById(int id)
        {
            return await _context.applicationForms.Where(x => x.Id == id).Select(x => x.applicantSignature).FirstOrDefaultAsync();
        }
        public async Task<string> GetApplicationQuotaDocumentById(int id)
        {
            return await _context.applicationQuotas.Where(x => x.Id == id).Select(x => x.document).FirstOrDefaultAsync();
        }

        public async Task<string> GetJoiningAttachmentUrlById(int id)
        {
            return await _context.applicationForms.Where(x => x.Id == id).Select(x => x.attachmentUrl).FirstOrDefaultAsync();
        }
        public async Task<int> SaveJoininigAttachment(ApplicationForm applicationForm)
        {

            if (applicationForm.Id != 0)
                _context.applicationForms.Update(applicationForm);
            else
                _context.applicationForms.Add(applicationForm);
            await _context.SaveChangesAsync();
            return applicationForm.Id;



        }

    }
}
