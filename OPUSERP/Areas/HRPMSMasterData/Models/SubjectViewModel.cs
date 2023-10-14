using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class SubjectViewModel
    {
        public int subjectId { get; set; }
        [Required]
        public string subjectName { get; set; }
        public string subjectNameBn { get; set; }
        
        public string subjectShortName { get; set; }
        public int? degreeId { get; set; }

        public SubjectInfoLn fLang { get; set; }

        public IEnumerable<Subject> subjects { get; set; }
        public IEnumerable<SubjectVm> subjectvm { get; set; }
        public IEnumerable<Degree> degrees { get; set; }
    }

	public class SubjectVm
	{
		public int? Id { get; set; }
		public string subjectName { get; set; }
		public string subjectNameBn { get; set; }
		public string subjectShortName { get; set; }
		public int? degreeId { get; set; }
		public string degreeName { get; set; }
	}
}
