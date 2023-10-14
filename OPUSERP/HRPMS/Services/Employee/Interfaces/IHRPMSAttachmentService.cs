using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
   public interface IHRPMSAttachmentService
    {
        Task<int> SaveHRPMSAttachment(HRPMSAttachment hRPMSAttachment);
        Task<IEnumerable<HRPMSAttachment>> GetHRPMSAttachment();
        Task<HRPMSAttachment> GetHRPMSAttachmentById(int id);
        Task<bool> DeleteHRPMSAttachmentById(int id);
        Task<IEnumerable<HRPMSAttachment>> GetHRPMSAttachmentByEmpId(int empId);
        Task<string> GetAttachmentUrlById(int id);
        Task<string> GetAttachmentUrlByIdcv(int id);
        Task<IEnumerable<HRPMSAttachment>> GetHRPMSAttachmentByFile();

    }
}
