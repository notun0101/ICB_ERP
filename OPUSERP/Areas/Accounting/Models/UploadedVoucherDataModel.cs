using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Areas.Accounting.Models
{
  
    public class UploadedVoucherDataModel
    {
        public int sl { get; set; }
        public string refNo { get; set; }
        public string voucherNo { get; set; }
        public string voucherDate { get; set; }
        public string accountName { get; set; }
        public string accountCode { get; set; }
        public string description { get; set; }
        public string sbuName { get; set; }
        public string sbuMessage { get; set; }
        public int? specialBranchUnitId { get; set; }
        public string projectName { get; set; }
        public string proMessage { get; set; }
        public int? projectId { get; set; }
        public string accMessage { get; set; }
        public string amount { get; set; }
        public string chequeNo { get; set; }
        public string chequeDate { get; set; }

        public int? transectionModeId { get; set; }
        public string transectionMode { get; set; }

        public int? voucherTypeId { get; set; }
        public string voucherType { get; set; }

        public int? fiscalYearId { get; set; }
        public string fiscalYear { get; set; }

        public int? taxYearId { get; set; }
        public string taxYear { get; set; }

        public int? companyId { get; set; }

        public int? fundSourceId { get; set; }
        public string fundSource { get; set; }

        public int? ledgerRelationId { get; set; }
        public string ledgerRelation { get; set; }

        public string isProcessedMessage { get; set; }
        public int? isProcessed { get; set; }

        public string hasAmountMismatched { get; set; }
    }
}
