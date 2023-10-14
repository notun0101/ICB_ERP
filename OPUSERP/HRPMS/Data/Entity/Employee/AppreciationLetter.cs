﻿using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("AppreciationLetter", Schema = "HR")]
    public class AppreciationLetter:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string letterNo { get; set; }
        public DateTime? date { get; set; }
        public string fileUrl { get; set; }
        public int? status { get; set; }
        public string remarks { get; set; }
    }
}