﻿using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Workflow.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Workflow.Models
{
    public class DocWithSgnature
    {
        public Doc doc { get; set; }

        public EmployeeSignature employeeSignature { get; set; }
    }
}
