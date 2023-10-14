using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSDisciplineInvestigation.Models
{
	public class AllegationViewModel
	{
        public int? allegationID { get; set; }
        public string allegationDetail { get; set; }
		public IFormFile allegationUrl { get; set; }
		public string clarification { get; set; }
		public IFormFile clarificationUrl { get; set; }
		public string management { get; set; }
		public IFormFile managementUrl { get; set; }

		public int? isActive { get; set; }
		public int? status { get; set; }
        public IEnumerable<Allegation> allegations { get; set; }

        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public int? employeeId { get; set; }
        public string employeeNameCode { get; set; }

        public Allegation allegationDetails { get; set; }
    }
}
