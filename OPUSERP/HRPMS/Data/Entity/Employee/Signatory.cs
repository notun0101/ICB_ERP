
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	
	[Table("Signatory", Schema = "HR")]
	public class Signatory: Base
	{
		[Required]
		public int DesignationId { get; set; }
		public Designation Designation { get; set; }
        public int EmployeeInfoId { get; set; }
        public EmployeeInfo EmployeeInfo { get; set; }
        //public string SignatoryCode { get; set; }
		//public string SignatoryName { get; set; }
		//public string SignatoryDesignation { get; set; }
		//public string SignatoryPhone { get; set; }
	}
}
