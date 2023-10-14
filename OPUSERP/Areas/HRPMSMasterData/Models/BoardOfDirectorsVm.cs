using Microsoft.AspNetCore.Http;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
	public class BoardOfDirectorsVm
	{
		public string boardId { get; set; }
		public string name { get; set; }
		public string designation { get; set; }
		public string mobile { get; set; }
		public string email { get; set; }
		public string address { get; set; }
		public string bio { get; set; }
		public string photoUrl { get; set; }
		public IFormFile chairmanPhotoUrl { get; set; }

		public string[] dirNames { get; set; }
		public string[] dirDesis { get; set; }
		public string[] dirMobiles { get; set; }
		public string[] dirEmails { get; set; }
		public string[] dirAddresses { get; set; }
		public string[] dirBios { get; set; }
		public IFormFile[] dirPhotos { get; set; }

		public string ceoname { get; set; }
		public string ceodesignation { get; set; }
		public string ceomobile { get; set; }
		public string ceoemail { get; set; }
		public string ceoaddress { get; set; }
		public string ceobio { get; set; }
		public IFormFile ceoPhotos { get; set; }

		public int companyId { get; set; }
		public Company company { get; set; }
		public IEnumerable<Company> companies { get; set; }
		public IEnumerable<Company> companyInfo { get; set; }

		public int? year { get; set; }

		public int? isActive { get; set; }
		public int? status { get; set; }
		public DateTime? date { get; set; }

		public IEnumerable<BODWithCompany> bODWithCompanies { get; set; }
	}
	public class BODWithCompany
	{
		public ComapnyAndYear company { get; set; }
		public ComapnyAndBOD chairmen { get; set; }
		public IEnumerable<ComapnyAndBOD> directors { get; set; }
		public ComapnyAndBOD ceo { get; set; }
	}
	public class ComapnyAndYear
	{
		public int companyId { get; set; }
		public string companyName { get; set; }
		public string managerName { get; set; }
		public int year { get; set; }
	}
	public class ComapnyAndBOD
	{
		public Company company { get; set; }
		public BoardOfDirector boardOfDirector { get; set; }
	}
	public class FilterBODVm
	{
		public IEnumerable<Company> companies { get; set; }
		public List<int> years { get; set; }
	}
}
