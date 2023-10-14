﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLUB.Data.Entity.Master;

namespace CLUB.Services.MasterData.Interfaces
{
    public interface IDesignationDepartmentService
    {
        Task<bool> SaveDesignation(Designation designation);
        Task<IEnumerable<Designation>> GetDesignations();
        Task<Designation> GetDesignationById(int id);
        Task<bool> DeleteDesignationById(int id);
        //for validation
        Task<int> GetDesignationCheck(int id, string name);

        Task<bool> SaveDepartment(Department department);
        Task<IEnumerable<Department>> GetDepartment();
        Task<Department> GetDepartmentById(int id);
        Task<bool> DeleteDepartmentById(int id);

    }
}
