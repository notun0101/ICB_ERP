using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Auth.Models
{
    public class UserListViewModel
    {
		//public IEnumerable<AspNetUsersViewModel> aspNetUsersViewModels { get; set; }
		public IEnumerable<ApplicationRoleViewModel> userRoles { get; set; }
        public IEnumerable<UserBackup> userBackups { get; set; }
        public IEnumerable<UserBackUpViewModel> userBackUpViewModels { get; set; }
		public IEnumerable<GetAllUserViewModel> aspNetUsersViewModels { get; set; }
		public IEnumerable<HrBranch> branches { get; set; }

        public string userId { get; set; }
        public string securityCode { get; set; }
    }
}
