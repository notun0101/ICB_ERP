namespace OPUSERP.Areas.Payroll.Models
{
    public class PayrollLoanEmiViewModel
    {
        public int? employeeId { get; set; }
        public string employeeCode { get; set; }
        public string employeeName { get; set; }
        public string BranchName { get; set; }
        public string department { get; set; }
        public string lastDesignation { get; set; }
        public string year { get; set; }
        public decimal? Emi { get; set; }
    }
}
