using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Employee;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface ISpouseChildrenService
    {
        Task<string> GetSpouseImgUrlById(int id);
        Task<string> GetChildrenImgUrlById(int id);
        Task<string> GetSpouseMarriageImgUrlById(int id);
        Task<int> SaveSpouse(Spouse spouse);
        Task<IEnumerable<Spouse>> GetSpouse();
        Task<Spouse> GetSpouseById(int id);
        Task<bool> DeleteSpouseById(int id);
        Task<IEnumerable<Spouse>> GetSpouseByEmpId(int empId);

        Task<int> SaveChildren(Children children);
        Task<IEnumerable<Children>> GetChildren();
        Task<Children> GetChildrenById(int id);
        Task<bool> DeleteChildrenById(int id);
        Task<IEnumerable<Children>> GetChildrenByEmpId(int empId);
        Task<Spouse> GetSpouseByEmpId1(int empId);
        Task<Children> GetchildByEmpId1(int empId);

        Task<int> SaveChildrenEducation(ChildrenEducation childrenEdu);

        Task<bool> SaveEmployeeHobby(EmployeeHobby employeeHobby);
        Task<IEnumerable<ChildrenEducation>> GetChildEduById(int id);
        Task<int> DeleteChildrenEducationById(int id);

        Task<Spouse> GetSpouseByEmpIdForSingle(int id);
        Task<IEnumerable<Data.Entity.Master.Degree>> GetDegreesByLoeId(int id);
    }
}
