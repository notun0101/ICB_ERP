using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("Designation")]
    public class Designation:Base
    {
        [Required]
        public string designationCode { get; set; }

        [Required]
        public string designationName { get; set; }

        public string designationNameBN { get; set; }
  
        public string shortName { get; set; }

        public string designationType { get; set; }

		public int? customRoleId { get; set; }
		public CustomRole customRole { get; set; }

        public int? summaryRole { get; set; } //1=DGM - Above, 2=1st Class - Above, 3=2nd Class

		public int? isActive { get; set; } = 1;
	}
}
