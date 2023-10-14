using Microsoft.AspNetCore.Http;
using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.SCM.Data.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Models
{
    public class BillCreateViewModel
    {
        public IEnumerable<StockMaster> stockMasters { get; set; }
        public IEnumerable<StockDetails> stockDetails { get; set; }
        public string accountCode { get; set; }
        public LedgerRelation ledgerRelation { get; set; }
        public LedgerRelation supplierLedgerRelation { get; set; }
        public int? stockMasterId { get; set; }
        public int? billCreateMasterId { get; set; }
        public int? voucherMasterId { get; set; }
        public string gRNo { get; set; }
        public string billNo { get; set; }
        public string journalVoucherNo { get; set; }
        public DateTime? gRDate { get; set; }
        public DateTime? billDate { get; set; }
        public DateTime? voucherDate { get; set; }
        public int? supplierId { get; set; }
        public string supplier { get; set; }
        public string supplierDetails { get; set; }
        public string ledgerAccountName { get; set; } //Account Payable ledgerRelation

        public int? ledgerId { get; set; }
        public int? ledgerRelationId { get; set; } //Account Payable ledgerRelation

        public int? subLedgerRelationId { get; set; }
        public string subLedgerName { get; set; }
        public IFormFile[] files { get; set; }
        public IFormFile[] multipleFiles { get; set; }
        

        public int?[] specledgerRelationId { get; set; }
        public int?[] specledgerId { get; set; }
        public string[] specledgerAccountName { get; set; }
        public decimal?[] qty { get; set; }
        public decimal?[] rate { get; set; }
        public decimal?[] amount { get; set; }
        public decimal?[] vatAmount { get; set; }
        public decimal?[] linetotalamount { get; set; }
        public int?[] stockDetailId { get; set; }
        public int?[] attachmentIds { get; set; }
        public decimal? totalAmount { get; set; }
        public string remarks { get; set; }
        public BillCreateItemViewModel stockItems { get; set; }
        public CreateBillVoucherMaster createBillVoucherMaster { get; set; }
        public IEnumerable<CreateBillVoucherMaster> billVoucherMasters { get; set; }
        public IEnumerable<CreateBillVoucherDetail> billVoucherDetails { get; set; }
        public IEnumerable<CreateBillVoucherAttachment> billVoucherAttachments { get; set; }
        //  public  linetotalamount { get; set; }



    }

    public class BillCreateItemViewModel
    {
        public string gRNo { get; set; }
        public DateTime? gRDate { get; set; }
        public int? supplierId { get; set; }
        public string supplier { get; set; }
        public string supplierDetails { get; set; }
        public string poNo { get; set; }
        public int? stockMasterId { get; set; }

        public IEnumerable<BillCreateItemViewModelDetails> billCreateItemViewModelDetails { get; set; }
    }


    public class BillCreateItemViewModelDetails
    {
        public int? ledgerId { get; set; }
        public string ledgerAccountNameWithCode { get; set; }
        public int? ledgerRelationId { get; set; }

        public string itemName { get; set; } //Product/Service
        public int? specId { get; set; }
        public string specName { get; set; } //Description

        public decimal? qty { get; set; }
        public decimal? rate { get; set; }
        public decimal? amount { get; set; } //qty*rate
        public decimal? vatAmount { get; set; }
        public decimal? linetotalamount { get; set; } //(qty*rate)+vatAmount
        
        public int? stockDetailId { get; set; }

    }

}
