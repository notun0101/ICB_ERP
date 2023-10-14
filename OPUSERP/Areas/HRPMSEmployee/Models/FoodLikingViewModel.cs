using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System.ComponentModel.DataAnnotations;
using Address = OPUSERP.Areas.HRPMSEmployee.Models.Lang.Address;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class FoodLikingViewModel
    {
        public int employeeID { get; set; }

        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public int foodLikingId { get; set; }
        public int? vegiterian { get; set; }
        public int? nonVegiterian { get; set; }

        public FoodLiking foodLiking { get; set; }
    }
}
