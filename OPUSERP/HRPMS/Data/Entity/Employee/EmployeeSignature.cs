using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("EmployeeSignature", Schema = "HR")]
    public class EmployeeSignature : Base
    {
        //Foreign Reliation
        public int employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        [Required]
        public string url { get; set; } //english sign
        public string banglaSign { get; set; }  //bangla sign

        public string remarks { get; set; }

    }
}
