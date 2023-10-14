using OPUSERP.Data.Entity;
using OPUSERP.Payroll.Data.Entity.carLoan;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.PF
{
    [Table("CarLoanSchedule")]
    public class CarLoanSchedule:Base
    {
        public int? carLoanID { get; set; }
        public CarLoan carLoan { get; set; }


        public int salaryPeriodId { get; set; }



        public decimal restAmount { get; set; }
        public decimal installmentAmount { get; set; }
        public int noOfInstallment { get; set; }

        [DefaultValue(0)]
        public int? isComplete { get; set; }

        public int? PFLoanPaymentId { get; set; }
        public PFLoanPayment PFLoanPayment { get; set; }
    }
}
