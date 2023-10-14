using Microsoft.EntityFrameworkCore;
using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.NonPoTransaction;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Services.Voucher
{
    public class AccountingReportService : IAccountingReportService
    {
        private readonly ERPDbContext _context;
        private readonly IVoucherService voucherService;

        public AccountingReportService(ERPDbContext context, IVoucherService voucherService)
        {
            this.voucherService = voucherService;
            _context = context;
        }
        public async Task<IEnumerable<LedgerBookReportViewModel>> GetLedgerBookReportDatad(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

            //string Accname = string.Empty;
            //if (subledgerId == 0)
            //{

            //    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
            //    Accname = datax.accountName + "(" + datax.accountCode + ")";
            //}
            //else
            //{
            //    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
            //    var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
            //    Accname = datax.accountName + "(" + datax.accountCode + ")-" + dataxx.accountName + "(" + dataxx.accountCode + ")";
            //}



            var datal = await _context.ledgerBookReportViewModels.FromSql($"JAS_LedgerBooksDetails {ledgerId},{subledgerId},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();
            List<LedgerBookReportViewModel> data = new List<LedgerBookReportViewModel>();
            if (datal.Where(x => x.accountName == "Opening Balance").Count() == 0)
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = 1,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Opening Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = 0
                });
            }
            int i = 0;
            decimal? Tcredit = 0;
            decimal? Tdebit = 0;
            foreach (LedgerBookReportViewModel sp in datal)
            {
                i++;
                var url = "";
                if (sp.voucherType == "Contra")
                {
                    url = "/Accounting/ContraVoucher/ContraVoucherPdfAction?MasterId=";
                }
                else if (sp.voucherType == "Payment")
                {
                    url = "/Accounting/PaymentVoucher/PaymentVoucherPdfAction?MasterId=";
                }
                else if (sp.voucherType == "Journal")
                {
                    url = "/Accounting/JournalVoucher/JournalVoucherPdfAction?MasterId=";
                }
                else if (sp.voucherType == "Received")
                {
                    url = "/Accounting/ReceiptVoucher/ReceiptVoucherPdfAction?MasterId=";
                }
                Tdebit = Tdebit + sp.debit;
                Tcredit = Tcredit + sp.credit;

                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i,
                    voucherNo = sp.voucherNo,
                    voucherDate = sp.voucherDate,
                    accountCode = sp.accountCode,
                    accountName = sp.accountName,
                    subledgerName = sp.subledgerName,
                    voucherType = sp.voucherType,
                    credit = sp.credit,
                    debit = sp.debit,
                    action = $"<a class='btn btn-success' target='_blank' href='" + url + sp.VoucherId + "' style='color:white;'><i class='fa fa-eye'></i></a>"
                });

            }

            data.Add(new LedgerBookReportViewModel
            {
                VoucherId = i + 1,
                voucherNo = "Total",
                voucherDate = "",
                accountCode = "",
                accountName = "",
                subledgerName = "",
                voucherType = "",
                credit = Tcredit,
                debit = Tdebit,
                action = $""
            });

            if (Tcredit > Tdebit)
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i + 2,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Closing Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = Tcredit - Tdebit,
                    debit = 0,
                    action = $""
                });

            }
            else if (Tcredit < Tdebit)
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i + 2,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Closing Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = Tdebit - Tcredit,
                    action = $""
                });
            }
            else
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i + 2,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Closing Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = 0,
                    action = $""
                });
            }

            return data;
        }
        
        public async Task<IEnumerable<LedgerBookReportViewModel>> GetSubLedgerBookReportDatad(int subledgerId, DateTime fromDate, DateTime toDate)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<int> LedgerRelIds = new List<int>();
            LedgerRelIds = await _context.LedgerRelations.Where(x => x.subLedgerId == subledgerId).Select(x => x.Id).ToListAsync();
            List<LedgerBookReportViewModel> data = new List<LedgerBookReportViewModel>();
            decimal OpenBalanceAmount = 0;
            var OpenBalanaceList = _context.OpeningBalances.Where(x => LedgerRelIds.Contains((int)x.ledgerRelationId) && x.balanceUpTo < fromDate);
            decimal OpenBalanceDr = OpenBalanaceList.Where(x => x.transectionModeId == 1).Sum(x => (decimal)x.amount);
            decimal OpenBalanceCr = OpenBalanaceList.Where(x => x.transectionModeId == 2).Sum(x => (decimal)x.amount);
            List<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => LedgerRelIds.Contains((int)x.ledgerRelationId) && x.voucher.isPosted == 2).ToListAsync();
            decimal OpenbalancedetailDr = voucherDetails.Where(x => x.voucher.voucherDate < fromDate && x.transectionModeId == 1).Sum(x => (decimal)x.amount);
            decimal OpenbalancedetailCr = voucherDetails.Where(x => x.voucher.voucherDate < fromDate && x.transectionModeId == 2).Sum(x => (decimal)x.amount);
            OpenBalanceAmount = OpenBalanceDr + OpenbalancedetailDr - OpenBalanceCr - OpenbalancedetailCr;
            int i = 0;
            decimal? Tcredit = 0;
            decimal? Tdebit = 0;
            if (OpenBalanceAmount > 0)
            {
                i++;
                data.Add(new LedgerBookReportViewModel
                {

                    VoucherId = i,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Opening Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = OpenBalanceAmount,
                    action = $""
                });
                Tdebit = OpenBalanceAmount;
            }
            else
            {
                i++;
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i + 1,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Opening Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = -1 * OpenBalanceAmount,
                    debit = 0,
                    action = $""
                });
                Tcredit = -1 * OpenBalanceAmount;
            }
            foreach (VoucherDetail vdata in voucherDetails.Where(x => x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate))
            {
                i++;
                var url = "";
                if (vdata.voucher.voucherTypeId == 1)
                {
                    url = "/Accounting/ContraVoucher/ContraVoucherPdfAction?MasterId=";
                }
                else if (vdata.voucher.voucherTypeId == 6)
                {
                    url = "/Accounting/PaymentVoucher/PaymentVoucherPdfAction?MasterId=";
                }
                else if (vdata.voucher.voucherTypeId == 7)
                {
                    url = "/Accounting/JournalVoucher/JournalVoucherPdfAction?MasterId=";
                }
                else if (vdata.voucher.voucherTypeId == 8)
                {
                    url = "/Accounting/ReceiptVoucher/ReceiptVoucherPdfAction?MasterId=";
                }

                decimal? dramount = 0;
                decimal? cramount = 0;
                if (vdata.transectionModeId == 1)
                {
                    dramount = vdata.amount;
                }
                else
                {
                    cramount = vdata.amount;
                }
                Tdebit = Tdebit + dramount;
                Tcredit = Tcredit + cramount;
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i,
                    voucherNo = vdata?.voucher?.voucherNo,
                    voucherDate = vdata?.voucher?.voucherDate?.ToString("dd-MMM-yyyy"),
                    accountCode = vdata?.ledgerRelation?.ledger?.accountCode,
                    accountName = vdata.ledgerRelation.ledger.accountName,
                    subledgerName = vdata?.ledgerRelation?.subLedger?.accountName,
                    voucherType = vdata?.voucher?.voucherType?.voucherTypeName,
                    credit = cramount,
                    debit = dramount,
                    action = $"<a class='btn btn-success' target='_blank' href='" + url + vdata?.voucher?.Id + "' style='color:white;'><i class='fa fa-eye'></i></a>"
                });

            }
            data.Add(new LedgerBookReportViewModel
            {
                VoucherId = i + 1,
                voucherNo = "Total",
                voucherDate = "",
                accountCode = "",
                accountName = "",
                subledgerName = "",
                voucherType = "",
                credit = Tcredit,
                debit = Tdebit,
                action = $""
            });

            if (Tcredit > Tdebit)
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i + 2,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Closing Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = Tcredit - Tdebit,
                    debit = 0,
                    action = $""
                });

            }
            else if (Tcredit < Tdebit)
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i + 2,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Closing Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = Tdebit - Tcredit,
                    action = $""
                });
            }
            else
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = i + 2,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Closing Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = 0,
                    action = $""
                });
            }

            return data;
        }

        public async Task<IEnumerable<LedgerBookReportViewModel>> GetDayBookReportDatad(int voucherTypeId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<LedgerBookReportViewModel> data = new List<LedgerBookReportViewModel>();
            //  List<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.voucherTypeId == voucherTypeId && x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).ToListAsync();
            List<VoucherDetail> voucherDetails = new List<VoucherDetail>();
            //if (voucherTypeId > 0)
            //{
            //    voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.voucherTypeId == voucherTypeId && x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).ToListAsync();

            //}
            //else
            //{
            //    voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).ToListAsync();
            //}


            if (voucherTypeId > 0)
            {
                if (sbuId == 0)
                {
                    voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.voucherTypeId == voucherTypeId && x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                }
                else
                {
                    if (projectId == 0)
                    {
                        voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.voucherTypeId == voucherTypeId && x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.project.specialBranchUnitId == sbuId).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                    }
                    else
                    {
                        voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.voucherTypeId == voucherTypeId && x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.projectId == projectId).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                    }

                }


            }
            else
            {
                if (sbuId == 0)
                {
                    voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                }
                else
                {
                    if (projectId == 0)
                    {
                        voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.project.specialBranchUnitId == sbuId).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                    }
                    else
                    {
                        voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.projectId == projectId).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                    }
                }
            }
            int i = 0;
            decimal? Tcredit = 0;
            decimal? Tdebit = 0;


            foreach (VoucherDetail vdata in voucherDetails)
            {

                decimal? dramount = 0;
                decimal? cramount = 0;
                if (vdata.transectionModeId == 1)
                {
                    dramount = vdata.amount;
                }
                else
                {
                    cramount = vdata.amount;
                }

                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = vdata?.voucher?.Id,
                    voucherNo = vdata?.voucher?.voucherNo,
                    voucherDate = vdata?.voucher?.voucherDate?.ToString("dd-MMM-yyyy"),
                    accountCode = vdata?.ledgerRelation?.ledger?.accountCode,
                    accountName = vdata?.ledgerRelation?.ledger?.accountName,
                    subledgerName = vdata?.ledgerRelation?.subLedger?.accountName,
                    voucherType = vdata?.voucher?.voucherType?.voucherTypeName,
                    remarks = vdata?.voucher?.remarks,
                    credit = cramount,
                    debit = dramount
                    //action = $"<a class='btn btn - success' target='_blank' href='" + url + vdata?.voucher?.Id + "' style='color:white;'><i class='fa fa-eye'></i></a>"
                });
            }
            List<LedgerBookReportViewModel> lbrv = new List<LedgerBookReportViewModel>();
            int k = 0;
            foreach (LedgerBookReportViewModel trialBalanceViewModel in data)
            {
                i = int.Parse(trialBalanceViewModel.VoucherId.ToString());
                var url = "";
                if (trialBalanceViewModel.voucherType == "Contra")
                {
                    url = "/Accounting/ContraVoucher/ContraVoucherPdfAction?MasterId=";
                }
                else if (trialBalanceViewModel.voucherType == "Payment")
                {
                    url = "/Accounting/PaymentVoucher/PaymentVoucherPdfAction?MasterId=";
                }
                else if (trialBalanceViewModel.voucherType == "Journal")
                {
                    url = "/Accounting/JournalVoucher/JournalVoucherPdfAction?MasterId=";
                }
                else if (trialBalanceViewModel.voucherType == "Received")
                {
                    url = "/Accounting/ReceiptVoucher/ReceiptVoucherPdfAction?MasterId=";
                }
                int count = lbrv.Where(x => x.VoucherId == trialBalanceViewModel.VoucherId).Count();
                int rowCount = data.Where(x => x.VoucherId == trialBalanceViewModel.VoucherId).Count();
                Tdebit = Tdebit + trialBalanceViewModel.debit;
                Tcredit = Tcredit + trialBalanceViewModel.credit;

                if (count == 0)
                {
                    k = 0;
                    k++;
                    lbrv.Add(new LedgerBookReportViewModel
                    {

                        VoucherId = trialBalanceViewModel.VoucherId,//i,
                        voucherNo = trialBalanceViewModel.voucherNo,
                        voucherDate = trialBalanceViewModel.voucherDate,
                        accountCode = trialBalanceViewModel.accountCode,
                        accountName = trialBalanceViewModel.accountName,
                        subledgerName = trialBalanceViewModel.subledgerName,
                        voucherType = trialBalanceViewModel.voucherType,
                        credit = trialBalanceViewModel.credit,
                        debit = trialBalanceViewModel.debit,
                        action = $""//$"<a class='btn btn-success' target='_blank' href='" + url + trialBalanceViewModel.VoucherId + "' style='color:white;'><i class='fa fa-eye'></i></a>"
                    });
                }
                else
                {
                    k++;
                    lbrv.Add(new LedgerBookReportViewModel
                    {
                        VoucherId = trialBalanceViewModel.VoucherId,//i,
                        voucherNo = "-",
                        voucherDate = "-",
                        accountCode = trialBalanceViewModel.accountCode,
                        accountName = trialBalanceViewModel.accountName,
                        subledgerName = trialBalanceViewModel.subledgerName,
                        voucherType = "-",
                        credit = trialBalanceViewModel.credit,
                        debit = trialBalanceViewModel.debit,
                        //action = $"<a class='btn btn - success' target='_blank' href='" + url + trialBalanceViewModel.VoucherId + "' style='color:skyblue;'><i class='fa fa-eye'></i></a>"
                        action = $""
                    });
                }

                if (k == rowCount)
                {
                    lbrv.Add(new LedgerBookReportViewModel
                    {
                        VoucherId = trialBalanceViewModel.VoucherId,
                        voucherNo = "-",
                        voucherDate = "-",
                        accountCode = "-",
                        accountName = "-",
                        subledgerName = trialBalanceViewModel.remarks,
                        voucherType = "-",
                        credit = 0,
                        debit = 0
                    });
                }
            }
            lbrv.Add(new LedgerBookReportViewModel
            {
                VoucherId = i + 1,
                voucherNo = "Total",
                voucherDate = "",
                accountCode = "",
                accountName = "",
                subledgerName = "",
                voucherType = "",
                credit = Tcredit,
                debit = Tdebit,
                action = $""
            });
            return lbrv;
        }

        public async Task<IEnumerable<TrialBalanceViewModel>> GetTrialBalanceReportData(DateTime fromDate, DateTime toDate, int? sbuId, int? projectId, int? reportBy)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<TrialBalanceViewModel> trblnc = new List<TrialBalanceViewModel>();
            List<GroupWiseTrialBalanceViewModel> trblncgr = new List<GroupWiseTrialBalanceViewModel>();

            var dataLedger = await _context.trialBalanceSPViewModels.FromSql($"SP_TRAIL_BALANCE {company.Id},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId},{1}").AsNoTracking().ToListAsync();
            var dataGroup = await _context.trialBalanceSPViewModels.FromSql($"SP_TRAIL_BALANCE {company.Id},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId},{2}").AsNoTracking().ToListAsync();

            decimal? STdebit = 0;
            decimal? STcredit = 0;
            int i = 0;

            foreach (TrialBalanceSPViewModel grp in dataGroup)
            {
                i++;
                STdebit = STdebit + grp?.DRAmount;
                STcredit = STcredit + grp?.CRAmount;
                trblnc.Add(new TrialBalanceViewModel
                {
                    accountId = i,
                    accountCode = grp?.groupCode,
                    accountName = grp?.groupName,

                    debit = grp?.DRAmount,
                    credit = grp?.CRAmount,
                    Balance = "G",
                    closing = 0,
                    Company = company,
                    groupId = grp.GroupID,
                    action = $""//$"<a class='btn btn-success'  data-toggle='modal' data-target='#viewVoucher' onclick='voucherdata(" + sp?.Id + ")' style='color:white;'><i class='fas fa-eye'></i></a>"
                });

                if (reportBy != 1)
                {
                    foreach (TrialBalanceSPViewModel sp in dataLedger.Where(l => l.GroupID == grp.GroupID))
                    {
                        i++;
                        //STdebit = STdebit + sp?.DRAmount;
                        //STcredit = STcredit + sp?.CRAmount;
                        trblnc.Add(new TrialBalanceViewModel
                        {
                            accountId = i,
                            accountCode = sp?.AccountCode,
                            accountName = sp?.AccountName,

                            debit = sp?.DRAmount,
                            credit = sp?.CRAmount,
                            Balance = "L",
                            closing = 0,
                            Company = company,
                            groupId = sp?.GroupID,
                            action = $""//$"<a class='btn btn-success'  data-toggle='modal' data-target='#viewVoucher' onclick='voucherdata(" + sp?.Id + ")' style='color:white;'><i class='fas fa-eye'></i></a>"
                        });
                    }
                }

            }

            //int i = 0;
            //foreach (TrialBalanceSPViewModel sp in dataLedger)
            //{
            //    i++;
            //    STdebit = STdebit + sp?.DRAmount;
            //    STcredit = STcredit + sp?.CRAmount;
            //    trblnc.Add(new TrialBalanceViewModel
            //    {
            //        accountId = i,
            //        accountCode = sp?.AccountCode,
            //        accountName = sp?.AccountName,

            //        debit = sp?.DRAmount,
            //        credit = sp?.CRAmount,
            //        Balance = sp?.Balance,
            //        closing = 0,
            //        Company = company,
            //        groupId = sp?.GroupID,
            //        action = $"<a class='btn btn-success'  data-toggle='modal' data-target='#viewVoucher' onclick='voucherdata(" + sp?.Id + ")' style='color:white;'><i class='fas fa-eye'></i></a>"
            //    });
            //}

            trblnc.Add(new TrialBalanceViewModel
            {
                accountId = i + 1,
                accountCode = "",
                accountName = "Total",
                openingBalance = 0,
                debit = STdebit,
                credit = STcredit,
                Balance = "",
                closing = 0,
                Company = company,
                groupId = 0,
                action = $""
            }
           );

            //foreach (TrialBalanceSPViewModel sp in dataLedger)
            //{
            //    i++;

            //    trblncgr.Add(new GroupWiseTrialBalanceViewModel
            //    {
            //        groupId = sp?.GroupID,
            //        groupCode = sp?.groupCode,
            //        groupName = sp?.groupName,

            //        debit = sp?.DRAmount,
            //        credit = sp?.CRAmount,
            //        Balance = sp?.Balance

            //    });
            //}


            return trblnc.OrderBy(x => x.accountId);
        }

        public async Task<LedgerBookViewModel> GetLedgerBookReportData(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            string Accname = string.Empty;
            if (ledgerId != 0)
            {
                if (subledgerId == 0)
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")";
                }
                else
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")-" + dataxx.accountName + "(" + dataxx.accountCode + ")";
                }
            }
            else
            {
                var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                Accname = dataxx.accountName + "(" + dataxx.accountCode + ")";
            }

            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            var datal = await _context.ledgerBookReportViewModels.FromSql($"JAS_LedgerBooksDetails {ledgerId},{subledgerId},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();
            List<LedgerBookReportViewModel> data = new List<LedgerBookReportViewModel>();
            if (datal.Where(x => x.accountName == "Opening Balance").Count() == 0)
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = 0,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Opening Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = 0
                });
            }
            foreach (LedgerBookReportViewModel sp in datal)
            {
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = sp.VoucherId,
                    voucherNo = sp.voucherNo,
                    voucherDate = sp.voucherDate,
                    accountCode = sp.accountCode,
                    accountName = sp.accountName,
                    subledgerName = sp.subledgerName,
                    voucherType = sp.voucherType,
                    credit = sp.credit,
                    debit = sp.debit
                });
            }

            LedgerBookViewModel result = new LedgerBookViewModel
            {
                //obCredit = obcredit,
                //obDebit = obdebit,
                ledgerBookReportViewModels = data,
                Company = company,
                AccountName = Accname
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }
            return result;
        }



        public async Task<BudgetExpenditureViewModel> GetBudgetExpenditureData(DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            string Accname = string.Empty;
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            var datal = await _context.budgetExpenditureViewModels.FromSql($"SP_GET_Budget_Report {sbuId},{projectId}, {Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")}").AsNoTracking().ToListAsync();
            List<BudgetExpenditureReportViewModel> data = new List<BudgetExpenditureReportViewModel>();

            foreach (BudgetExpenditureReportViewModel sp in datal)
            {
                data.Add(new BudgetExpenditureReportViewModel
                {
                    name = sp.name,
                    code = sp.code,
                    TotalBudget = sp.TotalBudget,
                    TotalActualExpense = sp.TotalActualExpense,
                    CurrentBudgetTotal = sp.CurrentBudgetTotal,
                    ActualAmount = sp.ActualAmount
                });
            }
            BudgetExpenditureViewModel result = new BudgetExpenditureViewModel
            {
                budgetExpenditureReportViewModels = data,
                Company = company
            };
            if (result == null)
            {
                result = new BudgetExpenditureViewModel();
            }
            return result;
        }

        public async Task<LedgerBookViewModel> GetCashLedgerBookReportData(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            //string Accname = string.Empty;
            //if (ledgerId != 0)
            //{
            //    if (subledgerId == 0)
            //    {
            //        var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
            //        Accname = datax.accountName + "(" + datax.accountCode + ")";
            //    }
            //    else
            //    {
            //        var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
            //        var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
            //        Accname = datax.accountName + "(" + datax.accountCode + ")-" + dataxx.accountName + "(" + dataxx.accountCode + ")";
            //    }
            //}
            //else
            //{
            //    var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
            //    Accname = dataxx.accountName + "(" + dataxx.accountCode + ")";
            //}

            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            var datal = await _context.bankBookReportViewModels.FromSql($"JAS_CashBooksDetails {ledgerId},{subledgerId},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();
            List<BankBookReportViewModel> data = new List<BankBookReportViewModel>();
            if (datal.Where(x => x.accountName == "Opening Balance").Count() == 0)
            {
                data.Add(new BankBookReportViewModel
                {
                    VoucherId = 0,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Opening Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = 0,
                    closing = 0
                });
            }
            foreach (BankBookReportViewModel sp in datal)
            {
                data.Add(new BankBookReportViewModel
                {
                    VoucherId = sp.VoucherId,
                    voucherNo = sp.voucherNo,
                    voucherDate = sp.voucherDate,
                    accountCode = sp.accountCode,
                    accountName = sp.accountName,
                    subledgerName = sp.subledgerName,
                    voucherType = sp.voucherType,
                    credit = sp.credit,
                    debit = sp.debit,
                    closing = sp.closing
                });
            }

            LedgerBookViewModel result = new LedgerBookViewModel
            {
                bankBookReportViewModels = data,
                Company = company,
                //AccountName = Accname
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }
            return result;
        }

        public async Task<LedgerBookViewModel> GetBankLedgerBookReportData(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            var datal = await _context.bankBookReportViewModels.FromSql($"JAS_BankBooksDetails {ledgerId},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();
            List<BankBookReportViewModel> data = new List<BankBookReportViewModel>();
            if (datal.Where(x => x.accountName == "Opening Balance").Count() == 0)
            {
                data.Add(new BankBookReportViewModel
                {
                    VoucherId = 0,
                    voucherNo = "",
                    voucherDate = "",
                    accountCode = "",
                    accountName = "Opening Balance",
                    subledgerName = "",
                    voucherType = "",
                    credit = 0,
                    debit = 0,
                    closing = 0
                });
            }
            foreach (BankBookReportViewModel sp in datal)
            {
                data.Add(new BankBookReportViewModel
                {
                    VoucherId = sp.VoucherId,
                    voucherNo = sp.voucherNo,
                    voucherDate = sp.voucherDate,
                    accountCode = sp.accountCode,
                    accountName = sp.accountName,
                    subledgerName = sp.subledgerName,
                    voucherType = sp.voucherType,
                    credit = sp.credit,
                    debit = sp.debit,
                    closing = sp.closing
                });
            }

            LedgerBookViewModel result = new LedgerBookViewModel
            {
                bankBookReportViewModels = data,
                Company = company
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }
            return result;
        }

        public async Task<LedgerBookViewModel> GetSubLedgerBookReportData(int subledgerId, DateTime fromDate, DateTime toDate)
        {
            //Company company = await _context.Companies.Where(x => x.Id == 1).FirstOrDefaultAsync();
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            string Accname = string.Empty;
            var datax = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
            Accname = datax.accountName + "(" + datax.accountCode + ")";
            List<int> LedgerRelIds = new List<int>();
            LedgerRelIds = await _context.LedgerRelations.Where(x => x.subLedgerId == subledgerId).Select(x => x.Id).ToListAsync();
            List<LedgerBookReportViewModel> data = new List<LedgerBookReportViewModel>();
            decimal OpenBalanceAmount = 0;
            var OpenBalanaceList = _context.OpeningBalances.Where(x => LedgerRelIds.Contains((int)x.ledgerRelationId) && x.balanceUpTo < fromDate);
            decimal OpenBalanceDr = OpenBalanaceList.Where(x => x.transectionModeId == 1).Sum(x => (decimal)x.amount);
            decimal OpenBalanceCr = OpenBalanaceList.Where(x => x.transectionModeId == 2).Sum(x => (decimal)x.amount);
            List<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => LedgerRelIds.Contains((int)x.ledgerRelationId) && x.voucher.isPosted == 2).ToListAsync();
            decimal OpenbalancedetailDr = voucherDetails.Where(x => x.voucher.voucherDate < fromDate && x.transectionModeId == 1).Sum(x => (decimal)x.amount);
            decimal OpenbalancedetailCr = voucherDetails.Where(x => x.voucher.voucherDate < fromDate && x.transectionModeId == 2).Sum(x => (decimal)x.amount);
            OpenBalanceAmount = OpenBalanceDr + OpenbalancedetailDr - OpenBalanceCr - OpenbalancedetailCr;

            foreach (VoucherDetail vdata in voucherDetails.Where(x => x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate))
            {
                decimal? dramount = 0;
                decimal? cramount = 0;
                if (vdata.transectionModeId == 1)
                {
                    dramount = vdata.amount;
                }
                else
                {
                    cramount = vdata.amount;
                }
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = vdata?.voucher?.Id,
                    voucherNo = vdata?.voucher?.voucherNo,
                    voucherDate = vdata?.voucher?.voucherDate?.ToString("dd-MMM-yyyy"),
                    accountCode = vdata?.ledgerRelation?.ledger?.accountCode,
                    accountName = vdata.ledgerRelation.ledger.accountName,
                    subledgerName = vdata?.ledgerRelation?.subLedger?.accountName,
                    voucherType = vdata?.voucher?.voucherType?.voucherTypeName,
                    credit = cramount,
                    debit = dramount
                });
            }
            decimal? obdebit = 0;
            decimal? obcredit = 0;
            if (OpenBalanceAmount > 0)
            {
                obdebit = OpenBalanceAmount;
            }
            else
            {
                obcredit = -1 * OpenBalanceAmount;
            }
            LedgerBookViewModel result = new LedgerBookViewModel
            {
                obCredit = obcredit,
                obDebit = obdebit,
                ledgerBookReportViewModels = data,
                Company = company,
                AccountName = Accname
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }



            return result;
        }


        public async Task<LedgerBookViewModel> GetDayBookReportData(int voucherTypeId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

            string VoucherTypeName = string.Empty;
            if (voucherTypeId == 0)
            {
                VoucherTypeName = "All Voucher";
            }
            else
            {
                VoucherTypeName = _context.VoucherTypes.Where(x => x.Id == voucherTypeId).Select(x => x.voucherTypeName).FirstOrDefault();
            }
            List<LedgerBookReportViewModel> data = new List<LedgerBookReportViewModel>();
            List<VoucherDetail> voucherDetails = new List<VoucherDetail>();
            if (voucherTypeId > 0)
            {
                if (sbuId == 0)
                {
                    voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.voucherTypeId == voucherTypeId && x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                }
                else
                {
                    if (projectId == 0)
                    {
                        voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.voucherTypeId == voucherTypeId && x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.project.specialBranchUnitId == sbuId).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                    }
                    else
                    {
                        voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.voucherTypeId == voucherTypeId && x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.projectId == projectId).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                    }

                }


            }
            else
            {
                if (sbuId == 0)
                {
                    voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                }
                else
                {
                    if (projectId == 0)
                    {
                        voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.project.specialBranchUnitId == sbuId).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                    }
                    else
                    {
                        voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.voucher.isPosted == 2 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.projectId == projectId).OrderBy(o => o.voucher.voucherDate).ThenBy(o => o.voucher.Id).ThenBy(o => o.transectionModeId).ToListAsync();
                    }
                }
            }



            foreach (VoucherDetail vdata in voucherDetails)
            {
                decimal? dramount = 0;
                decimal? cramount = 0;
                if (vdata.transectionModeId == 1)
                {
                    dramount = vdata.amount;
                }
                else
                {
                    cramount = vdata.amount;
                }
                data.Add(new LedgerBookReportViewModel
                {
                    VoucherId = vdata?.voucher?.Id,
                    voucherNo = vdata?.voucher?.voucherNo,
                    voucherDate = vdata?.voucher?.voucherDate?.ToString("dd-MMM-yyyy"),
                    accountCode = vdata?.ledgerRelation?.ledger?.accountCode,
                    accountName = vdata?.ledgerRelation?.ledger?.accountName,
                    subledgerName = vdata?.ledgerRelation?.subLedger?.accountName,
                    voucherType = vdata?.voucher?.voucherType?.voucherTypeName,
                    remarks = vdata?.voucher?.remarks,
                    credit = cramount,
                    debit = dramount
                });
            }
            List<LedgerBookReportViewModel> lbrv = new List<LedgerBookReportViewModel>();

            int k = 0;
            foreach (LedgerBookReportViewModel trialBalanceViewModel in data)
            {
                int count = lbrv.Where(x => x.VoucherId == trialBalanceViewModel.VoucherId).Count();
                int rowCount = data.Where(x => x.VoucherId == trialBalanceViewModel.VoucherId).Count();
                if (count == 0)
                {
                    k = 0;
                    k++;
                    lbrv.Add(new LedgerBookReportViewModel
                    {
                        VoucherId = trialBalanceViewModel.VoucherId,
                        voucherNo = trialBalanceViewModel.voucherNo,
                        voucherDate = trialBalanceViewModel.voucherDate,
                        accountCode = trialBalanceViewModel.accountCode,
                        accountName = trialBalanceViewModel.accountName,
                        subledgerName = trialBalanceViewModel.subledgerName,
                        voucherType = trialBalanceViewModel.voucherType,
                        credit = trialBalanceViewModel.credit,
                        debit = trialBalanceViewModel.debit
                    });
                }
                else
                {
                    k++;
                    lbrv.Add(new LedgerBookReportViewModel
                    {
                        VoucherId = trialBalanceViewModel.VoucherId,
                        voucherNo = "-",
                        voucherDate = "-",
                        accountCode = trialBalanceViewModel.accountCode,
                        accountName = trialBalanceViewModel.accountName,
                        subledgerName = trialBalanceViewModel.subledgerName,
                        voucherType = "-",
                        credit = trialBalanceViewModel.credit,
                        debit = trialBalanceViewModel.debit
                    });
                }

                if (k == rowCount)
                {
                    lbrv.Add(new LedgerBookReportViewModel
                    {
                        VoucherId = trialBalanceViewModel.VoucherId,
                        voucherNo = "-",
                        voucherDate = "-",
                        accountCode = "-",
                        accountName = "-",
                        subledgerName = trialBalanceViewModel.remarks,
                        voucherType = "-",
                        credit = 0,
                        debit = 0
                    });
                }

            }

            LedgerBookViewModel result = new LedgerBookViewModel
            {
                ledgerBookReportViewModels = lbrv,
                Company = company,
                VoucherTypeName = VoucherTypeName
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }



            return result;
        }

        public async Task<IEnumerable<TrialBalanceViewModel>> GetTrialBalanceReportDatad(DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            IEnumerable<LedgerRelation> ledgerRelations = await _context.LedgerRelations.Include(x => x.ledger).ToListAsync();

            List<TrialBalanceViewModel> data = new List<TrialBalanceViewModel>();

            IEnumerable<OpeningBalance> OpenBalanaceList = await _context.OpeningBalances.Include(x => x.ledgerRelation.ledger).Where(x => x.balanceUpTo < fromDate).ToListAsync();
            IEnumerable<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.ledgerRelation.ledger).Where(x => x.voucher.isPosted == 2).ToListAsync();
            IEnumerable<VoucherDetail> prevoucherDetails = voucherDetails.Where(x => x.voucher.voucherDate < fromDate).ToList();
            IEnumerable<VoucherDetail> curvoucherDetails = voucherDetails.Where(x => x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).ToList();
            foreach (LedgerRelation ledgerrel in ledgerRelations)
            {
                decimal? obdr = OpenBalanaceList.Where(x => x.ledgerRelationId == ledgerrel.Id && x.transectionModeId == 1).Sum(x => x.amount);
                decimal? obcr = OpenBalanaceList.Where(x => x.ledgerRelationId == ledgerrel.Id && x.transectionModeId == 2).Sum(x => x.amount);
                decimal? predr = prevoucherDetails.Where(x => x.ledgerRelationId == ledgerrel.Id && x.transectionModeId == 1).Sum(x => x.amount);
                decimal? precr = prevoucherDetails.Where(x => x.ledgerRelationId == ledgerrel.Id && x.transectionModeId == 2).Sum(x => x.amount);
                decimal? dr = curvoucherDetails.Where(x => x.ledgerRelationId == ledgerrel.Id && x.transectionModeId == 1).Sum(x => x.amount);
                decimal? cr = curvoucherDetails.Where(x => x.ledgerRelationId == ledgerrel.Id && x.transectionModeId == 2).Sum(x => x.amount);
                decimal? opening = obdr + predr - obcr - precr;
                decimal? closing = obdr + predr + dr - obcr - precr - cr;
                if (closing > 0)
                {
                    data.Add(new TrialBalanceViewModel
                    {
                        accountId = ledgerrel?.ledger?.Id,
                        accountCode = ledgerrel?.ledger?.accountCode,
                        accountName = ledgerrel?.ledger?.accountName,
                        openingBalance = opening,
                        debit = obdr + predr + dr,
                        credit = obcr + precr + cr,
                        closing = closing,
                    });

                }

            }
            List<TrialBalanceViewModel> trblnc = new List<TrialBalanceViewModel>();
            foreach (TrialBalanceViewModel trialBalanceViewModel in data)
            {
                int count = trblnc.Where(x => x.accountCode == trialBalanceViewModel.accountCode).Count();
                if (count == 0)
                {
                    decimal? openingBalance = data.Where(x => x.accountCode == trialBalanceViewModel.accountCode).Sum(x => x.openingBalance);
                    decimal? debit = data.Where(x => x.accountCode == trialBalanceViewModel.accountCode).Sum(x => x.debit);
                    decimal? credit = data.Where(x => x.accountCode == trialBalanceViewModel.accountCode).Sum(x => x.credit);
                    decimal? closing = data.Where(x => x.accountCode == trialBalanceViewModel.accountCode).Sum(x => x.closing);
                    trblnc.Add(new TrialBalanceViewModel
                    {
                        accountId = trialBalanceViewModel?.accountId,
                        accountCode = trialBalanceViewModel?.accountCode,
                        accountName = trialBalanceViewModel?.accountName,
                        openingBalance = openingBalance,
                        debit = debit,
                        credit = credit,
                        closing = closing,
                        Company = company,
                    });
                }
            }
            return trblnc;
        }

        public async Task<ChartOfAccountViewModel> ChartOfAccountViewModels(int? sbuId, int? projectId)
        {
            IEnumerable<GroupNature> groupNatures = await _context.GroupNatures.AsNoTracking().ToListAsync();
            List<AccountNatureViewModel> accountNatureViewModels = new List<AccountNatureViewModel>();
            foreach (GroupNature datan in groupNatures)
            {
                accountNatureViewModels.Add(new AccountNatureViewModel
                {
                    natureId = datan.Id,
                    natureName = datan.natureName
                });
            }
            IEnumerable<AccountGroup> accountGroups = await _context.AccountGroups.OrderBy(o => o.SortOrder).AsNoTracking().ToListAsync();
            List<AccountsGroupViewModel> accountsGroupViewModels = new List<AccountsGroupViewModel>();
            foreach (AccountGroup datan in accountGroups)
            {
                string accType = "";
                string nature = "";
                if (datan.AccType == 1)
                {
                    accType = "Balance Sheet";
                }
                else if (datan.AccType == 2)
                {
                    accType = "Profit & Loss";
                }
                if (datan.natureId == 1)
                {
                    nature = "Asset";
                }
                else if (datan.natureId == 2)
                {
                    nature = "Liabilities";
                }
                else if (datan.natureId == 3)
                {
                    nature = "Income";
                }
                else if (datan.natureId == 4)
                {
                    nature = "Expense";
                }

                accountsGroupViewModels.Add(new AccountsGroupViewModel
                {
                    accountGroupId = datan.Id,
                    accountGroupName = datan.groupName,
                    natureId = datan.natureId,
                    accType = accType,
                    groupCode = datan.groupCode,
                    Nature = nature,
                    parentGroupId = datan.parentGroupId

                });
            }

            IEnumerable<Ledger> ledgers = await _context.Ledgers.Where(x => x.haveSubLedger != 2).AsNoTracking().ToListAsync();
            if (sbuId != 0)
            {
                ledgers = ledgers.Where(x => x.specialBranchUnitId == sbuId);
            }
            List<AccountsLedgerViewModel> accountsLedgerViewModels = new List<AccountsLedgerViewModel>();
            foreach (Ledger datan in ledgers)
            {
                accountsLedgerViewModels.Add(new AccountsLedgerViewModel
                {
                    ledgerId = datan.Id,
                    ledgerName = datan.accountName,
                    ledgerCode = datan.accountCode,
                    groupId = datan.groupId,
                    effectiveDate = datan.effectiveDate,
                    ledgerType = datan.ledgerType
                });

            }
            List<Company> companies = await _context.Companies.ToListAsync();
            ChartOfAccountViewModel data = new ChartOfAccountViewModel
            {
                accountNatureViewModels = accountNatureViewModels,
                accountsGroupViewModels = accountsGroupViewModels,
                accountsLedgerViewModels = accountsLedgerViewModels,
                companies = companies

            };
            return data;

        }

        public async Task<IEnumerable<ChartOfAccountdataViewModel>> ChartOfAccountdataViewModel(int? sbuId, int? projectId)
        {

            IEnumerable<Ledger> ledgers = await _context.Ledgers.Include(x => x.group.nature).Where(x => x.haveSubLedger != 2).AsNoTracking().ToListAsync();
            if (sbuId != 0)
            {
                ledgers = ledgers.Where(x => x.specialBranchUnitId == sbuId);
            }
            List<ChartOfAccountdataViewModel> accountsLedgerViewModels = new List<ChartOfAccountdataViewModel>();
            foreach (Ledger datan in ledgers)
            {
                accountsLedgerViewModels.Add(new ChartOfAccountdataViewModel
                {
                    accountCode = datan.accountCode,
                    accountName = datan.accountName,
                    accountGroup = datan.group.groupName,
                    natureName = datan.group.nature.natureName
                });
            }
            return accountsLedgerViewModels;
        }

        public async Task<IEnumerable<DailyBillReceiveReportViewModel>> GetdailyBillReceive(int supplierId, DateTime fromDate, DateTime toDate)
        {
            List<DailyBillReceive> dailyBillReceives = new List<DailyBillReceive>();
            if (supplierId > 0)
            {
                dailyBillReceives = await _context.DailyBillReceives.Include(x => x.supplier).Where(x => x.supplierId == supplierId && x.BillReceiveDate >= fromDate && x.BillReceiveDate <= toDate).ToListAsync();
            }
            else
            {
                dailyBillReceives = await _context.DailyBillReceives.Include(x => x.supplier).Where(x => x.supplierId == supplierId && x.BillReceiveDate >= fromDate && x.BillReceiveDate <= toDate).ToListAsync();
            }
            List<DailyBillReceiveReportViewModel> dailyBillReceiveReportViews = new List<DailyBillReceiveReportViewModel>();
            string status = "";
            foreach (DailyBillReceive bill in dailyBillReceives)
            {
                var url = "/Accounting/NonPOTransaction/BillReceivePdf?Id=";
                if (bill.BillStatusId == 1)
                {

                    status = "Paid";
                }
                else
                {
                    status = "Bill Pending";
                }
                dailyBillReceiveReportViews.Add(new DailyBillReceiveReportViewModel
                {
                    billReceiveId = bill.Id,
                    ProcessNo = bill.ProcessNo,
                    BillReceiveDate = bill.BillReceiveDate,
                    supplierName = bill.supplier.organizationName,
                    InvoiceNo = bill.InvoiceNo,
                    GBamount = bill.GBamount,
                    Vat = bill.Vat,
                    Total = bill.Total,
                    billStatus = status,
                    action = $"<a class='btn btn-success' target='_blank' href='" + url + bill.Id + "' style='color:white;'><i class='fa fa-eye'></i></a>"
                    //Accounting / NonPOTransaction / BillReceivePdf
                });

            }
            return dailyBillReceiveReportViews;

        }

        public async Task<IEnumerable<CCWiseLedgerReportdataViewModel>> GetCCWiseLedgerReportViewModels(int costcentreId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<CostCentreAllocation> costCentreAllocations = await _context.CostCentreAllocations.Include(x => x.voucherDetail.voucher).Include(x => x.costCentre).Include(x => x.voucherDetail.ledgerRelation.ledger).Where(x => x.voucherDetail.voucher.voucherDate >= fromDate && x.voucherDetail.voucher.voucherDate <= toDate).ToListAsync();
            List<int?> lstIds = costCentreAllocations.Select(x => x.voucherDetail.voucherId).Distinct().ToList();
            //List<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x=>x.voucher).Include(x=>x.ledgerRelation.ledger).Where(x => lstIds.Contains(x.voucherId) && x.isPrincAcc==1).Where(x => x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).ToListAsync();
            List<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x => x.voucher).Include(x => x.ledgerRelation.ledger).Where(x => lstIds.Contains(x.voucherId) && x.isPrincAcc == 1 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).ToListAsync();
            List<CCWiseLedgerReportViewModel> cCWiseLedgerReportViewModels = new List<CCWiseLedgerReportViewModel>();

            decimal? debit = 0;
            decimal? credit = 0;
            foreach (CostCentreAllocation bill in costCentreAllocations)
            {
                debit = 0;
                credit = 0;
                if (bill.voucherDetail.transectionModeId == 1)
                {

                    debit = bill?.amount;
                }
                else
                {
                    credit = bill?.amount;
                }


                cCWiseLedgerReportViewModels.Add(new CCWiseLedgerReportViewModel
                {
                    voucherDate = bill?.voucherDetail?.voucher?.voucherDate,
                    voucherNo = bill?.voucherDetail?.voucher?.voucherNo,
                    //accountCode = voucherDetails.Where(x=>x.voucherId==bill?.voucherDetail?.voucherId).FirstOrDefault().ledgerRelation?.ledger?.accountCode,
                    //particular = voucherDetails.Where(x => x.voucherId == bill?.voucherDetail?.voucherId).FirstOrDefault().ledgerRelation?.ledger?.accountName,
                    accountCode = bill.voucherDetail.ledgerRelation.ledger.accountCode,
                    particular = bill.voucherDetail.ledgerRelation.ledger.accountName,
                    centreName = bill?.costCentre?.centreName,
                    debit = debit,
                    credit = credit,
                    costcentreId = bill?.costCentreId,
                    vmId = bill.voucherDetail.voucherId,

                    action = "",

                });

            }
            List<CCWiseLedgerReportViewModel> newdata = new List<CCWiseLedgerReportViewModel>();
            if (costcentreId > 0)
            {
                newdata = cCWiseLedgerReportViewModels.Where(x => x.costcentreId == costcentreId).ToList();

            }
            else
            {
                newdata = cCWiseLedgerReportViewModels.ToList();
            }

            List<CCWiseLedgerReportdataViewModel> data = new List<CCWiseLedgerReportdataViewModel>();
            data.Add(new CCWiseLedgerReportdataViewModel
            {
                cCWiseLedgerReportViewModels = newdata,
                company = company
            });

            return data;
        }

        public async Task<IEnumerable<CCWiseLedgerReportViewModel>> GetCCWiseLedgerReportViewModelsT(int costcentreId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<CostCentreAllocation> costCentreAllocations = await _context.CostCentreAllocations.Include(x => x.voucherDetail.voucher.project).Include(x => x.costCentre).Include(x => x.voucherDetail.ledgerRelation.ledger).Where(x => x.voucherDetail.voucher.voucherDate >= fromDate && x.voucherDetail.voucher.voucherDate <= toDate).ToListAsync();
            List<int?> lstIds = costCentreAllocations.Select(x => x.voucherDetail.voucherId).Distinct().ToList();
            //List<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x=>x.voucher).Include(x=>x.ledgerRelation.ledger).Where(x => lstIds.Contains(x.voucherId) && x.isPrincAcc==1).Where(x => x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).ToListAsync();
            List<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x => x.voucher.voucherType).Include(x => x.voucher.project).Include(x => x.ledgerRelation.ledger).Where(x => lstIds.Contains(x.voucherId) && x.isPrincAcc == 1 && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate).ToListAsync();
            if (sbuId != 0)
            {
                if (projectId == 0)
                {
                    costCentreAllocations = costCentreAllocations.Where(x => x.voucherDetail.voucher.project.specialBranchUnitId == sbuId).ToList();
                    voucherDetails = voucherDetails.Where(x => x.voucher.project.specialBranchUnitId == sbuId).ToList();
                }
                else
                {
                    costCentreAllocations = costCentreAllocations.Where(x => x.voucherDetail.voucher.projectId == projectId).ToList();
                    voucherDetails = voucherDetails.Where(x => x.voucher.projectId == projectId).ToList();
                }

            }



            List<CCWiseLedgerReportViewModel> cCWiseLedgerReportViewModels = new List<CCWiseLedgerReportViewModel>();

            decimal? debit = 0;
            decimal? credit = 0;
            decimal? Tdebit = 0;
            decimal? Tcredit = 0;
            foreach (CostCentreAllocation bill in costCentreAllocations)
            {
                debit = 0;
                credit = 0;
                if (bill.voucherDetail.transectionModeId == 1)
                {

                    debit = bill?.amount;
                }
                else
                {
                    credit = bill?.amount;
                }
                var url = "";
                if (bill.voucherDetail.voucher.voucherType.voucherTypeName == "Contra")
                {
                    url = "/Accounting/ContraVoucher/ContraVoucherPdfAction?MasterId=";
                }
                else if (bill.voucherDetail.voucher.voucherType.voucherTypeName == "Payment")
                {
                    url = "/Accounting/PaymentVoucher/PaymentVoucherPdfAction?MasterId=";
                }
                else if (bill.voucherDetail.voucher.voucherType.voucherTypeName == "Journal")
                {
                    url = "/Accounting/JournalVoucher/JournalVoucherPdfAction?MasterId=";
                }
                else if (bill.voucherDetail.voucher.voucherType.voucherTypeName == "Received")
                {
                    url = "/Accounting/ReceiptVoucher/ReceiptVoucherPdfAction?MasterId=";
                }
                Tcredit = Tcredit + credit;
                Tdebit = Tdebit + debit;
                cCWiseLedgerReportViewModels.Add(new CCWiseLedgerReportViewModel
                {
                    // voucherDate = bill?.voucherDetail?.voucher?.voucherDate,
                    vDate = Convert.ToDateTime(bill?.voucherDetail?.voucher?.voucherDate).ToString("dd-MMM-yyyy"),
                    voucherNo = bill?.voucherDetail?.voucher?.voucherNo,
                    //accountCode = voucherDetails.Where(x=>x.voucherId==bill?.voucherDetail?.voucherId).FirstOrDefault().ledgerRelation?.ledger?.accountCode,
                    //particular = voucherDetails.Where(x => x.voucherId == bill?.voucherDetail?.voucherId).FirstOrDefault().ledgerRelation?.ledger?.accountName,
                    accountCode = bill.voucherDetail.ledgerRelation.ledger.accountCode,
                    particular = bill.voucherDetail.ledgerRelation.ledger.accountName,
                    centreName = bill?.costCentre?.centreName,
                    debit = debit,
                    credit = credit,
                    //  costcentreId = bill?.costCentreId,
                    //  vmId = bill.voucherDetail.voucherId,

                    action = $"<a class='btn btn-success' target='_blank' href='" + url + bill.voucherDetail.voucherId + "' style='color:white;'><i class='fa fa-eye'></i></a>",

                });

            }
            cCWiseLedgerReportViewModels.Add(new CCWiseLedgerReportViewModel
            {
                // voucherDate = bill?.voucherDetail?.voucher?.voucherDate,
                vDate = "Total",
                voucherNo = "",
                //accountCode = voucherDetails.Where(x=>x.voucherId==bill?.voucherDetail?.voucherId).FirstOrDefault().ledgerRelation?.ledger?.accountCode,
                //particular = voucherDetails.Where(x => x.voucherId == bill?.voucherDetail?.voucherId).FirstOrDefault().ledgerRelation?.ledger?.accountName,
                accountCode = "",
                particular = "",
                centreName = "",
                debit = Tdebit,
                credit = Tcredit,
                //  costcentreId = bill?.costCentreId,
                //  vmId = bill.voucherDetail.voucherId,

                action = $"",

            });

            return cCWiseLedgerReportViewModels;
        }

        public async Task<IEnumerable<LedgerDetailsViewModel>> LedgerDetailsViewModels(int ledgerId, DateTime fromDate, DateTime toDate)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<LedgerDetailsViewModel> ledgerDetailsViewModels = new List<LedgerDetailsViewModel>();
            List<VoucherDetail> voucherDetails = await _context.VoucherDetails.Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x => x.ledgerRelation.ledgerId == ledgerId && x.voucher.voucherDate >= fromDate && x.voucher.voucherDate <= toDate && x.voucher.isPosted == 2).ToListAsync();
            List<Ledger> ledgers = voucherDetails.Where(x => x.ledgerRelation.subLedgerId != null).Select(x => x.ledgerRelation.subLedger).Distinct().ToList();
            decimal? bal = 0;
            decimal? Tbal = 0;
            foreach (Ledger led in ledgers)
            {
                bal = voucherDetails.Where(x => x.ledgerRelation.subLedgerId == led.Id && x.transectionModeId == 1).Sum(x => x.amount) - voucherDetails.Where(x => x.ledgerRelation.subLedgerId == led.Id && x.transectionModeId == 2).Sum(x => x.amount);
                Tbal = Tbal + bal;
                ledgerDetailsViewModels.Add(new LedgerDetailsViewModel
                {
                    accountName = led.accountName,
                    accountCode = led.accountCode,
                    Amount = bal,
                    company = company
                });
            }
            ledgerDetailsViewModels.Add(new LedgerDetailsViewModel
            {
                accountName = "Total",
                accountCode = "",
                Amount = Tbal,
                company = company
            });
            return ledgerDetailsViewModels;
        }

        public async Task<IEnumerable<DailyBillReceiveReportViewModel>> GetdailyBillReceivePdf(int supplierId, DateTime fromDate, DateTime toDate)
        {
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<DailyBillReceive> dailyBillReceives = new List<DailyBillReceive>();
            if (supplierId > 0)
            {
                dailyBillReceives = await _context.DailyBillReceives.Include(x => x.supplier).Where(x => x.supplierId == supplierId && x.BillReceiveDate >= fromDate && x.BillReceiveDate <= toDate).ToListAsync();
            }
            else
            {
                dailyBillReceives = await _context.DailyBillReceives.Include(x => x.supplier).Where(x => x.supplierId == supplierId && x.BillReceiveDate >= fromDate && x.BillReceiveDate <= toDate).ToListAsync();
            }
            //List<DailyBillReceiveReportViewModel> dailyBillReceiveReportViews = new List<DailyBillReceiveReportViewModel>();
            List<DailyBillReceiveReportViewModel> data = new List<DailyBillReceiveReportViewModel>();

            string status = "";
            if (dailyBillReceives.Count() > 0)
            {

                foreach (DailyBillReceive bill in dailyBillReceives)
                {
                    //var url = "/Accounting/NonPOTransaction/BillReceivePdf?Id=";
                    if (bill.BillStatusId == 1)
                    {
                        status = "Paid";
                    }
                    else
                    {
                        status = "Bill Pending";
                    }
                    data.Add(new DailyBillReceiveReportViewModel
                    {
                        billReceiveId = bill?.Id,
                        ProcessNo = bill?.ProcessNo,
                        BillReceiveDate = bill?.BillReceiveDate,
                        supplierName = bill?.supplier?.organizationName,
                        InvoiceNo = bill?.InvoiceNo,
                        GBamount = bill?.GBamount,
                        Vat = bill?.Vat,
                        Total = bill?.Total,
                        billStatus = status
                        //action = $"<a class='btn btn - success' target='_blank' href='" + url + bill.Id + "' style='color:skyblue;'><i class='fa fa-eye'></i></a>"
                        //Accounting / NonPOTransaction / BillReceivePdf
                    });

                }

            }
            return data;



        }

        //public async Task<IEnumerable<ProfitAndLossAccountViewModel>> GetProfitLossACData(string FDate, string TDate, int? sbuId, int? projectId)
        ////public async Task<IEnumerable<ProfitAndLossAccountViewModel>> GetProfitLossACData(int? salaryYearId)
        //{
        //    return await _context.profitAndLossAccountViewModels.FromSql($"spprofitlost {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();
        //    //return await _context.profitAndLossAccountViewModels.FromSql($"spprofitlost {salaryYearId}").AsNoTracking().ToListAsync();

        //}

        public async Task<IEnumerable<ProfitAndLossAccountViewModel>> GetProfitLossACData_Previous(string FDate, string TDate, int? sbuId, int? projectId)
        {
            List<ProfitAndLossAccountViewModel> plLedger = new List<ProfitAndLossAccountViewModel>();
            List<ProfitAndLossAccountViewModel> plGroup = new List<ProfitAndLossAccountViewModel>();
            List<ProfitAndLossAccountViewModel> finalGroup = new List<ProfitAndLossAccountViewModel>();

            plLedger = await _context.profitAndLossAccountViewModels.FromSql($"spprofitlost_Previous_New {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId},{1}").AsNoTracking().ToListAsync();
            plGroup = await _context.profitAndLossAccountViewModels.FromSql($"spprofitlost_Previous_New {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId},{2}").AsNoTracking().ToListAsync();
            foreach (ProfitAndLossAccountViewModel grp in plGroup)
            {
                finalGroup.Add(new ProfitAndLossAccountViewModel
                {
                    GroupId = grp.GroupId,
                    AccountName = grp.AccountName,
                    NatureId = grp.NatureId,
                    Balance = grp.Balance,
                    Status = "G"
                });

                foreach (ProfitAndLossAccountViewModel led in plLedger.Where(l => l.GroupId == grp.GroupId))
                {
                    finalGroup.Add(new ProfitAndLossAccountViewModel
                    {
                        GroupId = led.GroupId,
                        AccountName = led.AccountName,
                        NatureId = led.NatureId,
                        Balance = led.Balance,
                        Status = "L"
                    });
                }

            }

            return finalGroup;

        }
        public async Task<IEnumerable<ProfitAndLossAccountViewModel>> GetProfitLossACData(string FDate, string TDate, int? sbuId, int? projectId)
        {
            List<ProfitAndLossAccountViewModel> plLedger = new List<ProfitAndLossAccountViewModel>();
            List<ProfitAndLossAccountViewModel> plGroup = new List<ProfitAndLossAccountViewModel>();
            List<ProfitAndLossAccountViewModel> finalGroup = new List<ProfitAndLossAccountViewModel>();

            plLedger = await _context.profitAndLossAccountViewModels.FromSql($"spprofitlost_New {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId},{1}").AsNoTracking().ToListAsync();
            plGroup = await _context.profitAndLossAccountViewModels.FromSql($"spprofitlost_New {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId},{2}").AsNoTracking().ToListAsync();
            foreach (ProfitAndLossAccountViewModel grp in plGroup)
            {
                finalGroup.Add(new ProfitAndLossAccountViewModel
                {
                    GroupId = grp.GroupId,
                    AccountName = grp.AccountName,
                    NatureId = grp.NatureId,
                    Balance = grp.Balance,
                    Status = "G"
                });

                foreach (ProfitAndLossAccountViewModel led in plLedger.Where(l => l.GroupId == grp.GroupId))
                {
                    finalGroup.Add(new ProfitAndLossAccountViewModel
                    {
                        GroupId = led.GroupId,
                        AccountName = led.AccountName,
                        NatureId = led.NatureId,
                        Balance = led.Balance,
                        Status = "L"
                    });
                }

            }

            return finalGroup;

            //foreach()


            //return await _context.profitAndLossAccountViewModels.FromSql($"spprofitlost {salaryYearId}").AsNoTracking().ToListAsync();

        }
        //public async Task<IEnumerable<BalanceSheetViewModel>> GetBalanceSheetData(string FDate, string TDate, int? sbuId, int? projectId)
        //{
        //    return await _context.balanceSheetViewModels.FromSql($"spfinancialstatement {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();

        //}

        public async Task<IEnumerable<BalanceSheetViewModel>> GetBalanceSheetData(string FDate, string TDate, int? sbuId, int? projectId)
        {
            //List<BalanceSheetViewModel> 
            List<BalanceSheetViewModel> plLedger = new List<BalanceSheetViewModel>();
            List<BalanceSheetViewModel> plGroup = new List<BalanceSheetViewModel>();
            List<BalanceSheetViewModel> finalGroup = new List<BalanceSheetViewModel>();

            plLedger = await _context.balanceSheetViewModels.FromSql($"spfinancialstatement_new {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId},{1}").AsNoTracking().ToListAsync();
            plGroup = await _context.balanceSheetViewModels.FromSql($"spfinancialstatement_new {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId},{2}").AsNoTracking().ToListAsync();
            foreach (BalanceSheetViewModel grp in plGroup)
            {
                finalGroup.Add(new BalanceSheetViewModel
                {
                    GroupId = grp.GroupId,
                    AccountName = grp.AccountName,
                    NatureId = grp.NatureId,
                    Balance = grp.Balance,
                    Status = "G"
                });

                foreach (BalanceSheetViewModel led in plLedger.Where(l => l.GroupId == grp.GroupId))
                {
                    finalGroup.Add(new BalanceSheetViewModel
                    {
                        GroupId = led.GroupId,
                        AccountName = led.AccountName,
                        NatureId = led.NatureId,
                        Balance = led.Balance,
                        Status = "L"
                    });
                }

            }

            return finalGroup;

            //return await _context.balanceSheetViewModels.FromSql($"spfinancialstatement_new {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();

        }
        public async Task<IEnumerable<CFSMasterViewModel>> GetCFSMasterViewModels(string FDate, string TDate, int? sbuId, int? projectId)
        //public async Task<IEnumerable<ProfitAndLossAccountViewModel>> GetProfitLossACData(int? salaryYearId)
        {
            IEnumerable<CFSDetailBalanceViewModel> cFSDetailBalanceViewModels = await _context.cFSDetailBalanceViewModels.FromSql($"spcashflowbalance {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();

            IEnumerable<CFSDetailViewModel> cFSDetailViewModels = await _context.cFSDetailViews.FromSql($"spcashflowcurrent {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<CFSMasterViewModel> data = new List<CFSMasterViewModel>();
            data.Add(new CFSMasterViewModel
            {
                Company = company,
                Date = Convert.ToDateTime(TDate).Date,
                Amount = cFSDetailBalanceViewModels.FirstOrDefault().Amount,
                cFSDetailViewModels = cFSDetailViewModels
            });
            return data;
        }

        public async Task<IEnumerable<CFSMasterViewModel>> GetCFSIndirectMasterViewModels(string FDate, string TDate, int? sbuId, int? projectId)
        {
            IEnumerable<CFSDetailViewModel> cFSDetailViewModels = await _context.cFSDetailViews.FromSql($"RPT_spcashflowIndirectcurrent {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<CFSMasterViewModel> data = new List<CFSMasterViewModel>();
            data.Add(new CFSMasterViewModel
            {
                Company = company,
                Date = Convert.ToDateTime(TDate).Date,
                cFSDetailViewModels = cFSDetailViewModels
            });
            return data;
        }

        public async Task<IEnumerable<BudgetExpenseMasterViewModel>> GetBudgetExpenseMasterViewModels(string FDate, string TDate, int? sbuId, int? projectId)
        {
            // IEnumerable<CFSDetailBalanceViewModel> cFSDetailBalanceViewModels = await _context.cFSDetailBalanceViewModels.FromSql($"spcashflowbalance {Convert.ToDateTime(FDate).ToString("yyyyMMdd")}").AsNoTracking().ToListAsync();

            IEnumerable<BudgetExpenseDetailViewModel> budgetExpenseDetailViewModels = await _context.budgetExpenseDetailViewModels.FromSql($"getexpenditurebudget {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();
            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<BudgetExpenseMasterViewModel> data = new List<BudgetExpenseMasterViewModel>();
            data.Add(new BudgetExpenseMasterViewModel
            {
                Company = company,
                fromDate = Convert.ToDateTime(FDate).ToString("dd-MM-yyyy"),
                toDate = Convert.ToDateTime(TDate).ToString("dd-MM-yyyy"),

                budgetExpenseDetailViewModels = budgetExpenseDetailViewModels
            });
            return data;
        }
        

        public async Task<IEnumerable<RVMasterViewModel>> GetRVMasterViewModels(string FDate, string TDate, int? sbuId, int? projectId, int? reportBy)
        {
            IEnumerable<RVDetailBalanceViewModel> rVDetailBalanceViewModels = await _context.rVDetailBalanceViewModels.FromSql($"spPaymentReceivebalance {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();

            IEnumerable<RVDetailViewModel> rVDetailViewModels = await _context.rVDetailViewModels.FromSql($"spPaymentReceivecurrent {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId},{reportBy}").AsNoTracking().ToListAsync();

            IEnumerable<RVDetailViewModel> cashOpeningBalance = await _context.rVDetailViewModels.FromSql($"SP_CASH_OPENCLOSING_BALANCE {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();

            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            List<RVMasterViewModel> data = new List<RVMasterViewModel>();
            data.Add(new RVMasterViewModel
            {
                Company = company,
                fromDate = Convert.ToDateTime(FDate).ToString("dd-MM-yyyy"),
                toDate = Convert.ToDateTime(TDate).ToString("dd-MM-yyyy"),
                Amount = rVDetailBalanceViewModels.FirstOrDefault().Amount,
                rVDetailViewModels = rVDetailViewModels,
                cashOpeningBalance = cashOpeningBalance
            });
            return data;
        }


        public async Task<LedgerBookViewModel> GetLedgerBookReportDataWithoutSP(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            string Accname = string.Empty;
            if (ledgerId != 0)
            {
                if (subledgerId == 0)
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")";
                }
                else
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")-" + dataxx.accountName + "(" + dataxx.accountCode + ")";
                }
            }
            else
            {
                var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                Accname = dataxx.accountName + "(" + dataxx.accountCode + ")";
            }

            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

            //var datal = await _context.ledgerBookReportViewModels.FromSql($"JAS_LedgerBooksDetails {ledgerId},{subledgerId},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();

            decimal? OBcr = await _context.OpeningBalances.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 2).Where(x => Convert.ToDateTime(x.balanceUpTo).Date < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? OBdr = await _context.OpeningBalances.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 1).Where(x => Convert.ToDateTime(x.balanceUpTo).Date < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? openingcredit = await _context.VoucherDetails.Where(x => x.voucher.isPosted ==2 && x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 2 ).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? openingdebit = await _context.VoucherDetails.Where(x => x.voucher.isPosted == 2 && x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 1).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            List<LedgerBookViewModelWithoutSP> data = new List<LedgerBookViewModelWithoutSP>();

            data.Add(new LedgerBookViewModelWithoutSP
            {
                VoucherId = 0,
                voucherNo = "",
                voucherDate = "",
                accountCode = "",
                accountName = "Opening Balance",
                subledgerName = "",
                narration = "",
                voucherType = "",
                credit = openingcredit + OBcr,
                debit = openingdebit + OBdr
            });

            var detailsdata = await _context.VoucherDetails.Where(x => x.voucher.isPosted == 2 && x.ledgerRelation.ledgerId == ledgerId).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate >= fromDate.Date && x.voucher.voucherDate <= toDate.Date).Include(x => x.voucher).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Include(x => x.voucher.voucherType).AsNoTracking().ToListAsync();


            foreach (var sp in detailsdata)
            {
                data.Add(new LedgerBookViewModelWithoutSP
                {
                    VoucherId = sp.voucher.Id,
                    voucherNo = sp.voucher.voucherNo,
                    voucherDate = sp.voucher.voucherDate?.ToString("dd/MM/yyyy"),
                    accountCode = sp.ledgerRelation.ledger.accountCode,
                    accountName = sp.accountName,
                    narration = sp.voucher.remarks,
                    subledgerName = sp.ledgerRelation?.subLedger?.accountName,
                    voucherType = sp.voucher?.voucherType?.voucherTypeName,
                    credit = sp.transectionModeId == 1 ? 0 : sp.amount,
                    debit = sp.transectionModeId == 2 ? 0 : sp.amount,
                    voucherDetails = await _context.VoucherDetails.Where(x => x.voucherId == sp.voucherId && x.transectionModeId != sp.transectionModeId).AsNoTracking().ToListAsync()
                });
            }

            LedgerBookViewModel result = new LedgerBookViewModel
            {
                //obCredit = obcredit,
                //obDebit = obdebit,
                //ledgerBookReportViewModels = data,
                ledgerBookViewModelWithoutSPs = data,
                Company = company,
                AccountName = Accname
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }
            return result;
        }
        

        

        public async Task<AccountingDashboardModel> YearlyIncomeExpense(int? sbuId,int year)
        {
            List<string> month = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            List<decimal?> income = new List<decimal?>();
            List<decimal?> expense = new List<decimal?>();
            for (int i = 1; i < 13; i++)
            {
                income.Add(await _context.VoucherDetails.Where(x => Convert.ToDateTime(x.voucher.voucherDate).Month == i && x.ledgerRelation.ledger.group.natureId == 3 && x.specialBranchUnitId==(sbuId==0? x.specialBranchUnitId:sbuId)).Where(x => Convert.ToDateTime(x.voucher.voucherDate).Year== (year == 0 ? Convert.ToDateTime(x.voucher.voucherDate).Year : year)).SumAsync(x => x.amount));

                expense.Add(await _context.VoucherDetails.Where(x => Convert.ToDateTime(x.voucher.voucherDate).Month == i && x.ledgerRelation.ledger.group.natureId == 4 && x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId)).Where(x => Convert.ToDateTime(x.voucher.voucherDate).Year == (year==0? Convert.ToDateTime(x.voucher.voucherDate).Year:year)).SumAsync(x => x.amount));
            }

            AccountingDashboardModel data = new AccountingDashboardModel
            {
                months = month,
                monthWiseIncome = income,
                monthWiseExpense = expense
            };

            return data;
        }

        public async Task<AccountingDashboardModel> AccountGroupWiseVoucher(int? sbuId,int year)
        {
            List<string> month = new List<string> ();
            List<decimal?> item = new List<decimal?>();
            var Group = await _context.GroupNatures.AsNoTracking().ToListAsync();
            foreach(var value in Group)
            {
                item.Add(await _context.VoucherDetails.Where(x =>x.ledgerRelation.ledger.group.natureId == value.Id && x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId)).Where(x => Convert.ToDateTime(x.voucher.voucherDate).Year == (year == 0 ? Convert.ToDateTime(x.voucher.voucherDate).Year : year)).SumAsync(x => x.amount));

                month.Add(value.natureName);
            }

            AccountingDashboardModel data = new AccountingDashboardModel
            {
                accountGroup = month,
                accountGroupWiseVouture = item
            };

            return data;
        }

        public async Task<IEnumerable<CashBankAccountBalanceDashboard>> BankAndCashBalance(int? sbuId, int year)
        {
            List<CashBankAccountBalanceDashboard> data = new List<CashBankAccountBalanceDashboard>();
            var LedgerList = await _context.Ledgers.Where(x => x.ledgerType == "Bank" || x.ledgerType == "Cash").Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId)).ToListAsync();

            foreach(var item in LedgerList)
            {
                var opening = await _context.OpeningBalances.Where(x => x.ledgerRelation.ledgerId == item.Id && Convert.ToDateTime(x.balanceUpTo).Year == (year == 0 ? Convert.ToDateTime(x.balanceUpTo).Year : year)).Where(x => x.transectionModeId == 1).SumAsync(x => x.amount);

                var debit = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == item.Id && Convert.ToDateTime(x.voucher.voucherDate).Year == (year == 0 ? Convert.ToDateTime(x.voucher.voucherDate).Year : year)).Where(x=>x.transectionModeId==1).SumAsync(x => x.amount);

                var credit = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == item.Id && Convert.ToDateTime(x.voucher.voucherDate).Year == (year == 0 ? Convert.ToDateTime(x.voucher.voucherDate).Year : year)).Where(x=>x.transectionModeId==2).SumAsync(x => x.amount);

                data.Add(new CashBankAccountBalanceDashboard {
                    name = item.accountName,
                    code = item.accountCode,
                    type = item.ledgerType,
                    amount = opening + debit - credit
                });
            }

            return data;
        }
        
        public async Task<LedgerBookViewModel> GetLedgerForBankReconcilation(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            string Accname = string.Empty;
            if (ledgerId != 0)
            {
                if (subledgerId == 0)
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")";
                }
                else
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")-" + dataxx.accountName + "(" + dataxx.accountCode + ")";
                }
            }
            else
            {
                var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                Accname = dataxx.accountName + "(" + dataxx.accountCode + ")";
            }

            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

            //var datal = await _context.ledgerBookReportViewModels.FromSql($"JAS_LedgerBooksDetails {ledgerId},{subledgerId},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();

            decimal? OBcr = await _context.OpeningBalances.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 2).Where(x => Convert.ToDateTime(x.balanceUpTo).Date < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? OBdr = await _context.OpeningBalances.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 1).Where(x => Convert.ToDateTime(x.balanceUpTo).Date < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? openingcredit = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 2).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? openingdebit = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 1).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            List<LedgerBookViewModelWithoutSP> data = new List<LedgerBookViewModelWithoutSP>();
            List<LedgerBookViewModelWithoutSP> previousUnReconData = new List<LedgerBookViewModelWithoutSP>();
            List<LedgerBookViewModelWithoutSP> currentUnReconData = new List<LedgerBookViewModelWithoutSP>();
            

            //data.Add(new LedgerBookViewModelWithoutSP
            //{
            //    VoucherId = 0,
            //    voucherNo = "",
            //    voucherDate = "",
            //    accountCode = "",
            //    accountName = "Opening Balance",
            //    subledgerName = "",
            //    narration = "",
            //    voucherType = "",
            //    credit = openingcredit + OBcr,
            //    debit = openingdebit + OBdr
            //});

            //var detailsdata = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == ledgerId).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate >= fromDate.Date && x.voucher.voucherDate <= toDate.Date).Include(x => x.voucher).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Include(x => x.voucher.voucherType).AsNoTracking().ToListAsync();
            var detailsdata = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == ledgerId).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate <= toDate.Date).Include(x => x.voucher).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Include(x => x.voucher.voucherType).AsNoTracking().ToListAsync();

            var bankReconDetails = await voucherService.GetBankReconcileDetail();
             bankReconDetails = bankReconDetails.Where(x=>x.isChecked ==1).ToList();
            int?[] voucherDetailIds = bankReconDetails.Select(x => x.voucherDetailId).ToArray();
            detailsdata = detailsdata.Where(x => !voucherDetailIds.Contains(x.Id)).ToList();

            foreach (var sp in detailsdata)
            {
                data.Add(new LedgerBookViewModelWithoutSP
                {
                    VoucherId = sp.voucher.Id,
                    VoucherDetailId = sp.Id,
                    voucherNo = sp.voucher.voucherNo,
                    chequeNo = sp.voucher.chequeNumber,
                    voucherDate = sp.voucher.voucherDate?.ToString("dd-MMM-yyyy"),
                    vDate = sp.voucher.voucherDate,
                    accountCode = sp.ledgerRelation.ledger.accountCode,
                    accountName = sp.accountName,
                    narration = sp.voucher.remarks,
                    subledgerName = sp.ledgerRelation?.subLedger?.accountName,
                    voucherType = sp.voucher?.voucherType?.voucherTypeName,
                    credit = sp.transectionModeId == 1 ? 0 : sp.amount,
                    debit = sp.transectionModeId == 2 ? 0 : sp.amount,
                    tranMode = sp.transectionModeId,
                    voucherDetails = await _context.VoucherDetails.Where(x => x.voucherId == sp.voucherId && x.transectionModeId != sp.transectionModeId).AsNoTracking().ToListAsync()
                });
                
            }

            previousUnReconData = data.Where(x => x.vDate < fromDate.Date).ToList();
            currentUnReconData = data.Where(x => x.vDate >= fromDate.Date && x.vDate <= toDate.Date).ToList();
            LedgerBookViewModel result = new LedgerBookViewModel
            {
                ledgerBookViewModelWithoutSPs = data,
                ledgerBookPreviousUnRecons = previousUnReconData,
                ledgerBookCurrentUnRecons = currentUnReconData,
                Company = company,
                AccountName = Accname
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }
            return result;
        }

        public async Task<LedgerBookViewModel> GetStateMentsBankReconcilation(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            string Accname = string.Empty;
            if (ledgerId != 0)
            {
                if (subledgerId == 0)
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")";
                }
                else
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")-" + dataxx.accountName + "(" + dataxx.accountCode + ")";
                }
            }
            else
            {
                var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                Accname = dataxx.accountName + "(" + dataxx.accountCode + ")";
            }

            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

            //var datal = await _context.ledgerBookReportViewModels.FromSql($"JAS_LedgerBooksDetails {ledgerId},{subledgerId},{Convert.ToDateTime(fromDate).ToString("yyyyMMdd")},{Convert.ToDateTime(toDate).ToString("yyyyMMdd")},{sbuId},{projectId}").AsNoTracking().ToListAsync();

            decimal? OBcr = await _context.OpeningBalances.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 2).Where(x => Convert.ToDateTime(x.balanceUpTo).Date < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? OBdr = await _context.OpeningBalances.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 1).Where(x => Convert.ToDateTime(x.balanceUpTo).Date < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? openingcredit = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 2).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            decimal? openingdebit = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == ledgerId && x.transectionModeId == 1).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate < fromDate.Date).AsNoTracking().SumAsync(x => x.amount);

            List<LedgerBookViewModelWithoutSP> data = new List<LedgerBookViewModelWithoutSP>();
          
            var detailsdata = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == ledgerId).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate <= toDate.Date).Include(x => x.voucher).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Include(x => x.voucher.voucherType).AsNoTracking().ToListAsync();

            var bankReconDetails = await voucherService.GetBankReconcileDetail();
             bankReconDetails = await voucherService.GetBankReconcileDetail();
            int?[] voucherDetailIds = bankReconDetails.Select(x => x.voucherDetailId).ToArray();
            detailsdata = detailsdata.Where(x => !voucherDetailIds.Contains(x.Id)).ToList();

            foreach (var sp in detailsdata)
            {
                data.Add(new LedgerBookViewModelWithoutSP
                {
                    VoucherId = sp.voucher.Id,
                    VoucherDetailId = sp.Id,
                    voucherNo = sp.voucher.voucherNo,
                    chequeNo = sp.voucher.chequeNumber,
                    voucherDate = sp.voucher.voucherDate?.ToString("dd-MMM-yyyy"),
                    vDate = sp.voucher.voucherDate,
                    accountCode = sp.ledgerRelation.ledger.accountCode,
                    accountName = sp.accountName,
                    narration = sp.voucher.remarks,
                    subledgerName = sp.ledgerRelation?.subLedger?.accountName,
                    voucherType = sp.voucher?.voucherType?.voucherTypeName,
                    credit = sp.transectionModeId == 1 ? 0 : sp.amount,
                    debit = sp.transectionModeId == 2 ? 0 : sp.amount,
                    tranMode = sp.transectionModeId,
                    voucherDetails = await _context.VoucherDetails.Where(x => x.voucherId == sp.voucherId && x.transectionModeId != sp.transectionModeId).AsNoTracking().ToListAsync()
                });

            }

           
            LedgerBookViewModel result = new LedgerBookViewModel
            {
                ledgerBookViewModelWithoutSPs = data,
                Company = company,
                AccountName = Accname
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }
            return result;
        }


        public async Task<LedgerBookViewModel> GetStateMentsBankReconcilationByMasterId(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId, int bankReconMasterId)
        {
            string Accname = string.Empty;
            if (ledgerId != 0)
            {
                if (subledgerId == 0)
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")";
                }
                else
                {
                    var datax = _context.Ledgers.Where(x => x.Id == ledgerId).FirstOrDefault();
                    var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                    Accname = datax.accountName + "(" + datax.accountCode + ")-" + dataxx.accountName + "(" + dataxx.accountCode + ")";
                }
            }
            else
            {
                var dataxx = _context.Ledgers.Where(x => x.Id == subledgerId).FirstOrDefault();
                Accname = dataxx.accountName + "(" + dataxx.accountCode + ")";
            }

            Company company = await _context.Companies.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            
            List<LedgerBookViewModelWithoutSP> data = new List<LedgerBookViewModelWithoutSP>();

            var detailsdata = await _context.VoucherDetails.Where(x => x.ledgerRelation.ledgerId == ledgerId).Where(x => x.specialBranchUnitId == (sbuId == 0 ? x.specialBranchUnitId : sbuId) && x.voucher.projectId == ((projectId == 0 ? x.voucher.projectId : projectId))).Where(x => x.voucher.voucherDate <= toDate.Date).Include(x => x.voucher).Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Include(x => x.voucher.voucherType).AsNoTracking().ToListAsync();

            var bankReconDetails = await voucherService.GetBankReconcileDetailByMasterId(bankReconMasterId);
            //bankReconDetails = bankReconDetails.Where(x=>x.isChecked ==0);
            int?[] voucherDetailIds = bankReconDetails.Where(x => x.isChecked == 0).Select(x => x.voucherDetailId).ToArray();
            detailsdata = detailsdata.Where(x => voucherDetailIds.Contains(x.Id)).ToList();

            foreach (var sp in detailsdata)
            {
                data.Add(new LedgerBookViewModelWithoutSP
                {
                    VoucherId = sp.voucher.Id,
                    VoucherDetailId = sp.Id,
                    voucherNo = sp.voucher.voucherNo,
                    chequeNo = sp.voucher.chequeNumber,
                    voucherDate = sp.voucher.voucherDate?.ToString("dd-MMM-yyyy"),
                    vDate = sp.voucher.voucherDate,
                    accountCode = sp.ledgerRelation.ledger.accountCode,
                    accountName = sp.accountName,
                    narration = sp.voucher.remarks,
                    subledgerName = sp.ledgerRelation?.subLedger?.accountName,
                    voucherType = sp.voucher?.voucherType?.voucherTypeName,
                    credit = sp.transectionModeId == 1 ? 0 : sp.amount,
                    debit = sp.transectionModeId == 2 ? 0 : sp.amount,
                    tranMode = sp.transectionModeId,
                    voucherDetails = await _context.VoucherDetails.Where(x => x.voucherId == sp.voucherId && x.transectionModeId != sp.transectionModeId).AsNoTracking().ToListAsync()
                });

            }


            LedgerBookViewModel result = new LedgerBookViewModel
            {
                ledgerBookViewModelWithoutSPs = data,
                Company = company,
                AccountName = Accname
            };
            if (result == null)
            {
                result = new LedgerBookViewModel();
            }
            return result;
        }
    }
}
