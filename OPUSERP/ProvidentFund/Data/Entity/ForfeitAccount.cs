using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("ForfeitAccount", Schema = "PF")]
    public class ForfeitAccount:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? pfId { get; set; } //autofill
        public PFMemberInfo pf { get; set; }

        public DateTime? forfeitDate { get; set; }
        public string reason { get; set; }

        public decimal? pfOwn { get; set; }
        public decimal? pfCompany { get; set; }
        public decimal? loan { get; set; }
        public decimal? outstandingLoan { get; set; }
        public decimal? balance { get; set; } //calculated (pfOwn+pfCompany+loan-outstandingLoan=balance)

        //optional fields
        public int? status { get; set; }
        public string remarks { get; set; }
    }
}
