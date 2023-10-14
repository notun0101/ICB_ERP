using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Budget.Models
{
    public class YearlyBudgetReport
    {
        public int? headId { get; set; }
        public string headName { get; set; }
        public int? yearId { get; set; }
        public string yearName { get; set; }
        public decimal? amount { get; set; }
        public decimal? initialAmount { get; set; }
        public decimal? achievementAmount { get; set; }

        public decimal? currentYearApprovedbudgetAmount { get; set; }
        public decimal? currentYearAchievementAmountQ1 { get; set; }
        public decimal? currentYearAchievementAmountQ2 { get; set; }
        public decimal? currentYearAchievementAmountQ3 { get; set; }
        public decimal? currentYearAchievementAmountQ4 { get; set; }
        public decimal? currentYearAchievement { get; set; }
        public decimal? currentYearBalance { get; set; } // currentYearApprovedbudgetAmount-currentYearAchievement

    }

    public class YearlyAchievableReport
    {
        public int? headId { get; set; }
        public string headName { get; set; }
        public int? yearId { get; set; }
        public string yearName { get; set; }
        public decimal? amount { get; set; }
    }

}
