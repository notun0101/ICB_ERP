using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
   public interface INomineeService
    {
        #region Nominee

        Task<int> SaveNominee(Nominee nominee);
        Task<IEnumerable<Nominee>> GetNominee();
        Task<Nominee> GetNomineeById(int id);
        Task<bool> DeleteNomineeById(int id);
        Task<IEnumerable<Nominee>> GetNomineeByEmpId(int empId);
        Task<Nominee> GetNomineeByEmpId1(int empId);
        //occupation,relation
        Task<IEnumerable<Occupation>> GetOccupation();
        Task<IEnumerable<Degree>> GetDegree();

        Task<IEnumerable<Relation>> GetRelation();

         Task<int> SaveRelation(Relation relation);
        Task<int> SaveOccupation(Occupation occupation);

        #endregion

        #region Employee Insurance

        Task<int> SaveEmployeeInsurance(EmployeeInsurance employeeInsurance);         
        Task<bool> DeleteEmployeeInsuranceById(int id);
        Task<IEnumerable<EmployeeInsurance>> GetEmployeeInsuranceByEmpId(int empId);

        #endregion

        Task<IEnumerable<Occupation>> GetFatherOccupation();
        Task<IEnumerable<Occupation>> GetMotherOccupation();
        Task<IEnumerable<LevelofEducation>> GetLevelofEducation();
        Task<string> GetNomineeImgUrlById(int id);
        Task<EmployeeAward> GetAwardById(int id);
        Task<EmployeeInsurance> GetInsuranceById(int id);
        Task<PassportDetails> GetPassportById(int id);
        Task<HRPMSAttachment> GetAttachmentById(int id);
        Task<IeltsInfo> GetIeltsById(int id);
        Task<TraveInfo> GetTravelById(int id);
    }
}
