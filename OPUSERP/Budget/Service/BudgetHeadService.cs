using Microsoft.EntityFrameworkCore;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Budget.Service
{
    public class BudgetHeadService: IBudgetHeadService
    {
        private readonly ERPDbContext _context;

        public BudgetHeadService(ERPDbContext context)
        {
            _context = context;
        }
        


    }
}
