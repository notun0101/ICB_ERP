using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("BankBranch")]
    public class BankBranch : Base
    {   
        [MaxLength(250)]
        public string branchName { get; set; }    
        
        public string routingNumber { get; set; }
    }
}
