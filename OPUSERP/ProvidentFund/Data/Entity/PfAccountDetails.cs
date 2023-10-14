using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PfAccountDetails")]
    public class PfAccountDetails : Base
    {
        public string pfMemberCode { get; set; }
        public int? pfId { get; set; }
        public PFMemberInfo pf { get; set; }

        public string employeeCode { get; set; }
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string nameEnglish { get; set; }
        public DateTime? TrDate { get; set; }
        public decimal? pfOwn { get; set; }
        public decimal? pfBank { get; set; }
        public decimal? pfLoan { get; set; }
        public decimal? pfInterest { get; set; }
        public decimal? pfInterestBank { get; set; }
    }
}
