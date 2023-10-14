using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Loan
{
    [Table("UploadExcelLog", Schema = "Payroll")]
    public class UploadExcelLog:Base
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
        public DateTime? uploadDate { get; set; }
        public int? totalData { get; set; }
        public string type { get; set; }
    }
}
