using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class ResourcePersonViewModel
    {

        public int ResourcePersonId { get; set; }

     

        public IEnumerable<ResourcePerson> resourcePersons { get; set; }
        public string name { get; set; }

        public string designation { get; set; }

        public string workPlace { get; set; }

        public string specialization { get; set; }
        public string type { get; set; } //In House/External

        public string contactNumber { get; set; }

        public string performance { get; set; }

        public string email { get; set; }

        public string address { get; set; }

        public string remarks { get; set; }

        public string url { get; set; }
        public IFormFile personPhoto { get; set; }


        //Other For Future
        public string orgType { get; set; }
		public string photoUrl { get; set; } 
		public string postingPlace { get; set; }
		public int? employeeInfoId { get; set; }
	}
}
