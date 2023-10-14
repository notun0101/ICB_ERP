using OPUSERP.HRPMS.Data.Entity.Recruitment;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Recruitment.Interfaces
{
    public interface IApplicationFormService
    {
        // ApplicationForm
        Task<int> SaveApplicationForm(ApplicationForm applicationForm);
        Task<IEnumerable<ApplicationForm>> GetApplicationForm();
        Task<ApplicationForm> GetApplicationFormById(int id);
        Task<bool> DeleteApplicationFormById(int id);
		Task<IEnumerable<ApplicationForm>> GetAllApplicationForm();
        Task<IEnumerable<SalaryGrade>> GetAllSalaryGrade();


        // JobCircular
        Task<int> SaveCreateJobCircular(JobCircular jobCircular);
        Task<IEnumerable<JobCircular>> GetCreateJobCircular(string status);
        Task<JobCircular> GetCreateJobCircularById(int id);
        Task<bool> DeleteCreateJobCircularById(int id);
        Task<bool> UpdateJobCircular(int Id, string Type);

        // 

         Task<int> SaveApplicantAddress(ApplicantAddress applicantAddress);
         Task<IEnumerable<ApplicantAddress>> GetApplicantAddress();

         Task<int> SaveApplicationEdu(ApplicationEdu applicationEdu);
         Task<IEnumerable<ApplicationEdu>> GetApplicationEdu();

         Task<int> SaveApplicationExp(ApplicationExp applicationExp);
         Task<IEnumerable<ApplicationExp>> GetApplicationExp();

         Task<int> SaveApplicationQuota(ApplicationQuota applicationQuota);
         Task<IEnumerable<ApplicationQuota>> GetApplicationQuota();

       Task<ApplicantAddress> GetAddressByTypeAndapplicationFormId(int FormId, string type);
        Task<ApplicationEdu> GetApplicationEduByapplicationFormId(int FormId);
        Task<ApplicationExp> GetApplicationExpByapplicationFormId(int FormId);
        Task<ApplicationQuota> GetApplicationQuotaByapplicationFormId(int FormId);

        Task<string> GetApplicationFormpayDocById(int id);
        Task<string> GetApplicationFormapplicantPhotoById(int id);
        Task<string> GetApplicationFormSignatureById(int id);
        Task<string> GetApplicationQuotaDocumentById(int id);
        Task<IEnumerable<ApplicationExp>> GetApplicationExpByapplicationFormId1(int FormId);
        Task<IEnumerable<ApplicationEdu>> GetEduByapplicationFormId(int FormId);
        Task<string> GetJoiningAttachmentUrlById(int id);
        Task<int> SaveJoininigAttachment(ApplicationForm applicationForm);

        Task<IEnumerable<ApplicationForm>> GetApplicationFormByStatus(int status);
    }
}
