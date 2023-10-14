using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.MasterData;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.SCM.Data.Entity.MasterData;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("UploadedVoucherData", Schema = "ACCOUNT")]
    public class UploadedVoucherData : Base
    {

        [Column(TypeName = "nvarchar(150)")]
        public string refNo { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string voucherNo { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? voucherDate { get; set; }
        
        [Column(TypeName = "nvarchar(200)")]
        public string accountName { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string accountCode { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string description { get; set; }


        [Column(TypeName = "nvarchar(200)")]
        public string sbuName { get; set; }


        public int? specialBranchUnitId { get; set; }
        public SpecialBranchUnit specialBranchUnit { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string projectName { get; set; }
        // public Project project { get; set; }

        public int? projectId { get; set; }
        public Project project { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? amount { get; set; }


        [Column(TypeName = "nvarchar(150)")]
        public string chequeNo { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? chequeDate { get; set; }

        public int? transectionModeId { get; set; }
        public TransectionMode transectionMode { get; set; }

        public int? voucherTypeId { get; set; }
        public virtual VoucherType voucherType { get; set; }

        public int? fiscalYearId { get; set; }
        public virtual SalaryYear fiscalYear { get; set; }

        public int? taxYearId { get; set; }
        public virtual TaxYear taxYear { get; set; }

        public int? companyId { get; set; }

        public int? fundSourceId { get; set; }
        public virtual FundSource fundSource { get; set; }

        public int? ledgerRelationId { get; set; }
        public LedgerRelation ledgerRelation { get; set; }


        public int? hasWrongData { get; set; }
        public int? isProcessed { get; set; }
        public string remarks { get; set; }
        public string isActive { get; set; }
    }
}
