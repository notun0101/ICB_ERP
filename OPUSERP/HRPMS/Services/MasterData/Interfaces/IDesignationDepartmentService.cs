using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface IDesignationDepartmentService
    {
        Task<bool> SaveDesignation(Designation designation);
        Task<IEnumerable<Designation>> GetDesignations();
        Task<Designation> GetDesignationById(int id);
        Task<Designation> GetDesignationIdByName(string name);
        Task<bool> DeleteDesignationById(int id);
		Task<IEnumerable<Location>> GetAllZones();
        Task<IEnumerable<HrBranch>> GetAllHrBranch();
        Task<IEnumerable<BranchWithManagerViewModel>> GetAllBranchWithManagers();


        Task<bool> SaveDepartment(Department department);
        Task<IEnumerable<Department>> GetDepartment();
        Task<Department> GetDepartmentById(int id);
        Task<Department> GetDepartmentIdByName(string name);
        Task<bool> DeleteDepartmentById(int id);

        Task<bool> SaveDivision(Division division);
        Task<IEnumerable<Division>> GetDivision();
        Task<Division> GetDivisionById(int id);
        Task<Division> GetDivisionIdByName(string name);
        Task<bool> DeleteDivisionById(int id);

        Task<bool> SaveJDType(JDType jDType);
        Task<IEnumerable<JDType>> GetJDType();
        Task<JDType> GetJDTypeById(int id);
        Task<bool> DeleteJDTypeById(int id);

        Task<IEnumerable<HrBranch>> GetBranch();
        Task<bool> SaveBranch(HrBranch hrBranch);
        Task<bool> DeleteBranchById(int id);
        Task<IEnumerable<HrBranchType>> GetAllBranchType();

        Task<IEnumerable<HrSubBranch>> GetSubBranch();
        Task<bool> SaveSubBranch(HrSubBranch hrSubBranch);
        Task<bool> DeleteSubBranchById(int id);

        Task<IEnumerable<JDTaskType>> GetJDTaskTypes(int id);
        Task<IEnumerable<JDType>> GetAllJDTypes(int id);
        Task<IEnumerable<JDTaskList>> GetAllJDTaskList(int id);
        Task<bool> SaveJDTaskType(JDTaskType jDTaskType);
        Task<bool> DeleteJDTaskTypeById(int id);

        Task<IEnumerable<HrDivision>> GetHrdivision();
        Task<HrDivision> GetHrDivisionById(int id);

    }
}
