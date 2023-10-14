using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
    [Table("RebateSlabType")]
    public class RebateSlabType : Base
    {
       
        public string slabTypeName { get; set; }    
        public decimal? minValue { get; set; }
        public decimal? maxValue { get; set; }
    }
}
