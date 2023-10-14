﻿using OPUSERP.OtherSales.Data.Entity.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.OtherSales.Models.NotMapped
{
    public class PaymentInvoiceWithDue
    {
        public SalesInvoiceMaster salesInvoiceMaster { get; set; }

        public decimal? due { get; set; }
        public int? storeId { get; set; }
    }
}
