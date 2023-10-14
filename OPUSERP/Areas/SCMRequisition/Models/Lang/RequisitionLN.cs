﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMRequisition.Models.Lang
{
    public class RequisitionLN
    {
        public string reqNo { get; set; }
        public string reqDate { get; set; }
        public string targetDate { get; set; }
        public string justification { get; set; }
        public string bracjustification { get; set; }

        public string singleSource { get; set; }
        public string multipleSource { get; set; }
        public string subject { get; set; }
        public string bracsubject { get; set; }
        public string reqDept { get; set; }
        public string reqType { get; set; }
        public string supplier { get; set; }
        public string isPostPR { get; set; }
        public string statusInfo { get; set; }

        public string itemDetails { get; set; }
        public string itemId { get; set; }
        public string itemSpecificationId { get; set; }
        public string lastPrice { get; set; }
        public string reqQty { get; set; }
        public string reqRate { get; set; }
        public string totalAmount { get; set; }
        public string itemDetailsList { get; set; }
        public string unit { get; set; }
        public string projectName { get; set; }

        public string title { get; set; }
        public string listTitle { get; set; }
        public string action { get; set; }

        public string attachment { get; set; }
        public string filename { get; set; }
        public string file { get; set; }
        public string choosefile { get; set; }
        public string preferableVendor { get; set; }
        public string approvalPanel { get; set; }
        public string fundsource { get; set; }

        public string createdAt { get; set; }
        public string createdBy { get; set; }

        public string empName { get; set; }
        public string nextEmpName { get; set; }
        public string Status { get; set; }
        public string remarks { get; set; }
    }
}
