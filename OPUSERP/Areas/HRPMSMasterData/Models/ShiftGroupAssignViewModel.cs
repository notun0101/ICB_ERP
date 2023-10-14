﻿using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class ShiftGroupAssignViewModel
    {
        public string ShiftType { get; set; }
        public int? employeeInfoId { get; set; }
        public int? shiftGroup { get; set; }
        public int? department { get; set; }
        public int? sbu { get; set; }
        public string visualEmpCodeName { get; set; }

        public ShiftGroupLn fLang { get; set; }

        public IEnumerable<ShiftGroupMaster> shiftGroupMasterlist { get; set; }
        public IEnumerable<ShiftGroupDetail> shiftGroupDetailslist { get; set; }
        public IEnumerable<SpecialBranchUnit> specialBranchUnits { get; set; }
        public IEnumerable<Department> departments { get; set; }
		public IEnumerable<EmployeeInfo> employeeWithShiftGroups { get; set; }

        public IEnumerable<ShiftGroupDetail> shiftGroupWithDetails { get; set; }
      //  public IEnumerable<ShiftGroupWithDetails> shiftGroups { get; set; }


    }
	//public class EmployeeWithShiftGroup
	//{
	//	public int MyProperty { get; set; }
	//}
}
