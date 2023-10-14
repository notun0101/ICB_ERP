using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.MasterData
{
    [Table("Lisence")]
    public class Lisence:Base
    {
        public int? lisenceTypeId { get; set; }
        public LisenceType lisenceType { get; set; }

        public string lisenceNo { get; set; }
        public DateTime? issueDate { get; set; }
        public DateTime? expDate { get; set; }
        public string attatchment { get; set; }

        public int? status { get; set; }
    }
}
