using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Patient.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Data.Entity.Hospital
{
	public class Patient:Base
	{
		public string resourceName { get; set; }
		[MaxLength(150)]
		public string phone { get; set; }
		[MaxLength(150)]
		public string mobile { get; set; }
		[MaxLength(200)]
		public string email { get; set; }
		[MaxLength(150)]
		public string otherPhone { get; set; }
		public int? age { get; set; }
		[MaxLength(30)]
		public string gender { get; set; }
		//fax use as occupation in patient
		[MaxLength(100)]
		public string fax { get; set; }

		public int? departmentsId { get; set; }
		//departments use as reson for seenig doc in patient
		public Department departments { get; set; }

		public int? designationsId { get; set; }
		public Designation designations { get; set; }

		public int? crmdepartmentsId { get; set; }
		public CRMDepartment crmdepartments { get; set; }

		public int? crmdesignationsId { get; set; }
		public CRMDesignation crmdesignations { get; set; }

		public string imagePath { get; set; }
		[MaxLength(200)]
		//skypeId us as Patients Height in Patient
		public string skypeId { get; set; }
		//linkedInId us as Patients Weightatient
		[MaxLength(200)]
		public string linkedInId { get; set; }
		[MaxLength(500)]
		public string nameEnglish { get; set; }
		[MaxLength(500)]
		public string organizationName { get; set; }
		[MaxLength(100)]

		public string LicenseNumber { get; set; }
		[MaxLength(100)]
		public string eTinNumber { get; set; }
		[MaxLength(100)]
		public string binNumber { get; set; }
		[MaxLength(100)]
		public string VATNumber { get; set; }
		[MaxLength(200)]
		public string alternativeEmail { get; set; }

		public int? ageInMonths { get; set; }
		public int? ageInDays { get; set; }
		[MaxLength(200)]
		public string fatherName { get; set; }
		[MaxLength(200)]
		public string motherName { get; set; }
		[MaxLength(10)]
		public string bloodGroup { get; set; }
		[MaxLength(30)]
		public string nationalId { get; set; }
		[MaxLength(15)]
		public string maritalStatus { get; set; }
		[MaxLength(50)]

		public string contactRelation { get; set; }
		public int? professionTypeId { get; set; }
		public ProfessionType professionType { get; set; }
	}
}
