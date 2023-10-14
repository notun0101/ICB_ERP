using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Budget.Service
{
    public class BudgetDisbursmentMasterService: IBudgetDisbursmentMasterService
    {
        private readonly ERPDbContext _context;

        public BudgetDisbursmentMasterService(ERPDbContext context)
        {
            _context = context;
        }
       
       


       
    }
}
