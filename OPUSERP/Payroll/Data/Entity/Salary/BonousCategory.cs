using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
    [Table("BonousCategory")]
    public class BonousCategory : Base
    {
        [MaxLength(50)]
        public string bonousCategoryName { get; set; }

        public int? isActive { get; set; }
    }
}
