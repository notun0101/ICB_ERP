﻿using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface IFunctionsInfoService
    {
        Task<bool> SaveFunctionInfo(FunctionInfo functionInfo);
        Task<IEnumerable<FunctionInfo>> GetFunctionInfo();
        Task<FunctionInfo> GetFunctionInfoById(int id);
        
        Task<bool> DeleteFunctionInfoById(int id);
        Task<IEnumerable<Company>> GetAllCompany();
        Task<EmployeePostingPlace> GetpresentPosting(int empid);
    }
}
