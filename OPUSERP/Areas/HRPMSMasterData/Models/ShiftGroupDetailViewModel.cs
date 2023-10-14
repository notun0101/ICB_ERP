using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class ShiftGroupDetailViewModel
    {
        public int shiftMasterId { get; set; }
        [Required]
        public string weekDay { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }

        public string holiday { get; set; }
        public string holidayType { get; set; }
        
        public int shiftGroupMasterId { get; set; }

        public ShiftGroupDetailsLn fLang { get; set; }

        public IEnumerable<ShiftGroupDetail> shiftGroupDetailslist { get; set; }

        public IEnumerable<ShiftGroupMaster> shiftGroupMasterslist { get; set; }
		public IEnumerable<ShiftGroupWithDetails> shiftGroupWithDetails { get; set; }
	}
	public class ShiftGroupWithDetails
	{
		public ShiftGroupMaster shiftGroupMaster { get; set; }
		public bool isAssigned { get; set; }
		public IEnumerable<ShiftGroupDetail> shiftGroupDetail { get; set; }
	}
}
