using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Data.Entity.MasterData;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.Lead
{
    [Table("ActivityTeam")]
    public class ActivityTeam : Base
    {

        public int? activityMasterId { get; set; }
        public ActivityMaster activityMaster { get; set; }

        public int? teamId { get; set; }
        public OPUSERP.Data.Entity.MasterData.Team team { get; set; } 
      


    }
}
