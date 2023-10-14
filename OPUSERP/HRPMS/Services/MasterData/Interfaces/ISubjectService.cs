using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface ISubjectService
    {
        Task<bool> SaveSubject(Subject employeeType);
        Task<IEnumerable<Subject>> GetAllSubject();
		Task<IEnumerable<SubjectVm>> GetAllSubjectVm();
		Task<Subject> GetSubjectById(int id);
        Task<bool> DeleteSubjectById(int id);
        Task<int> SaveSubject1(Subject subject);
        Task<int> UpdateSubjectDegreeRelation(int subjectId, int degreeId);

        Task<IEnumerable<ComputerSubject>> GetAllComputerSubject();
        Task<bool> SaveComputerSubject(ComputerSubject computerSubject);
        Task<bool> DeleteComputerSubjectById(int id);
        Task<IEnumerable<Degree>> GetAllDegrees();
        Task<IEnumerable<HrComputerLiteracy>> GetAllcomputerLiteracy();
        Task<bool> SaveHrComputerLiteracy(HrComputerLiteracy hrComputerLiteracy);
        Task<bool> DeleteComputerLiteracyById(int id);
        Task<IEnumerable<HrComputerLiteracy>> GetcomputerLiteracyByEmpId(int empId);
        Task<int> GetDegreeBySubjectId(int id);
    }
}
