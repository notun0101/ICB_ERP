using OPUSERP.Data.Entity.Master;
using System.Collections.Generic;

namespace OPUSERP.Areas.Accounting.Models
{
    public class BalanceSheetViewModel
    {
        //public string fsLineName { get; set; }
        //public string natureName { get; set; }
        //public string groupName { get; set; }
        //public string noteName { get; set; }
        //public string noteNo { get; set; }
        //public decimal? currentAmount { get; set; }
        //public decimal? previousAmount { get; set; }

        public int GroupId { get; set; }
        public string AccountName { get; set; }
        public int NatureId { get; set; }
        public decimal? Balance { get; set; }
        public string Status { get; set; }
    }
}
