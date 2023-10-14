﻿using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Data.Entity.AttachmentComment;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.Accounting.Models
{
    public class LedgerOpeningBalanceViewModel
    {
        public int ledgerOpeningBalanceId { get; set; }

        public int? ledgerRelationId { get; set; }
        public int? voucherId { get; set; }
        public int? projectId { get; set; }
        public decimal? amount { get; set; }
        public int? transectionModeId { get; set; }
        public int? subledgerRelationId { get; set; }
        public int hiddenLederId { get; set; }
        public string particular { get; set; }
        

        public DateTime? balanceUpTo { get; set; }

        public IEnumerable<OpeningBalance> openingBalances { get; set; }
        public IEnumerable<VoucherDetail> voucherMasters { get; set; }

        public IEnumerable<AccountGroup> accountGroups { get; set; }
       

       
    }
}
