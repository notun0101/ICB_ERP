using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Programs.Data.Entity
{
    [Table("ProgramOutPutProgres")]
    public class ProgramOutPutProgres : Base
    {
      

        public int? programOutPutId { get; set; }
        public ProgramOutPut programOutPut { get; set; }
        public int? programStatusId { get; set; }
        public ProgramStatus programStatus { get; set; }
    }
}
