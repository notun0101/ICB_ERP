using OPUSERP.CRM.Data.Entity.Client;
using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OPUSERP.Areas.CRMClient.Models
{
    public class ClientViewModel
    {
        public int leadsID { get; set; }
        public int? isconverted { get; set; }
        public int? isactive { get; set; }
        public IEnumerable<GetClientInfoListViewModel> getClientInfoListViewModels { get; set; }
        public IEnumerable<Leads> getClientInfoList { get; set; }
    }
}
