using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IPhotographService
    {
        Task<int> SavePhotograph(Photograph photograph);
        Task<IEnumerable<Photograph>> GetPhotographs();
        Task<Photograph> GetPhotographById(int id);
        Task<bool> DeletePhotographById(int id);
        Task<Photograph> GetPhotographByEmpIdAndType(int empId, string type);
        int UpdatePostingPlace(int empId, DateTime? fromDate);
     //   Task<bool> SaveEmployeeSignature(EmployeeSignature photograph);
        Task<IEnumerable<EmployeeSignature>> GetEmployeeSignature();
        Task<EmployeeSignature> GetEmployeeSignatureById(int id);
        Task<bool> DeleteEmployeeSignatureById(int id);
        Task<EmployeeSignature> GetEmployeeSignatureByEmpId(int empId);
        Task<EmployeeSignature> GetEmpSignatureByEmployeeId(int id);
        Task<EmployeeSignature> GetEmployeeSignatureByEmpCode(string code);
        Task<Photograph> GetPhotographByEmpCodeAndType(string code, string type);
        Task<int> SaveTransferAttachment(TransferLog transferLog);
        Task<TransferLog> GetTransferAttachmentByEmpId(int id);
        Task<Photograph> GetPhotographByEmpId(int empId);
        Task<int> SaveEmployeeSignature(EmployeeSignature photograph);
        Task<EmployeeSignature> GetEmpSignatureById(int id);


        Task<Photograph> GetPhotographByEmpIdNew(int empId);
    }
}
