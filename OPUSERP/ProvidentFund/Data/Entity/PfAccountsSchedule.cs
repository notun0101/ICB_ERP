using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PfAccountsSchedule")]
    public class PfAccountsSchedule:Base
    {
        public int? pfMemberId { get; set; }
        public PFMemberInfo pfMember { get; set; }

        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string year { get; set; }

        public decimal? openingBalance { get; set; } = 0;

        public decimal? januaryOwn { get; set; } = 0;
        public decimal? januaryBank { get; set; } = 0;
        public decimal? january { get; set; } = 0;

        public decimal? februaryOwn { get; set; } = 0;
        public decimal? februaryBank { get; set; } = 0;
        public decimal? february { get; set; } = 0;

        public decimal? marchOwn { get; set; } = 0;
        public decimal? marchBank { get; set; } = 0;
        public decimal? march { get; set; } = 0;

        public decimal? aprilOwn { get; set; } = 0;
        public decimal? aprilBank { get; set; } = 0;
        public decimal? april { get; set; } = 0;

        public decimal? mayOwn { get; set; } = 0;
        public decimal? mayBank { get; set; } = 0;
        public decimal? may { get; set; } = 0;

        public decimal? juneOwn { get; set; } = 0;
        public decimal? juneBank { get; set; } = 0;
        public decimal? june { get; set; } = 0;

        public decimal? julyOwn { get; set; } = 0;
        public decimal? julyBank { get; set; } = 0;
        public decimal? july { get; set; } = 0;

        public decimal? augustOwn { get; set; } = 0;
        public decimal? augustBank { get; set; } = 0;
        public decimal? august { get; set; } = 0;

        public decimal? septemberOwn { get; set; } = 0;
        public decimal? septemberBank { get; set; } = 0;
        public decimal? september { get; set; } = 0;

        public decimal? octoberOwn { get; set; } = 0;
        public decimal? octoberBank { get; set; } = 0;
        public decimal? october { get; set; } = 0;

        public decimal? novemberOwn { get; set; } = 0;
        public decimal? novemberBank { get; set; } = 0;
        public decimal? november { get; set; } = 0;

        public decimal? decemberOwn { get; set; } = 0;
        public decimal? decemberBank { get; set; } = 0;
        public decimal? december { get; set; } = 0;

        public decimal? interest { get; set; } = 0;
        public decimal? total { get; set; }
        public decimal? loan { get; set; }
    }
}
