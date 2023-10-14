using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OPUSERP.Models
{
	public class CBSXMLDataViewModel
	{
		public string XML_DATA { get; set; }
		public string uniqueKey { get; set; }
	}

    public class CBSPostedLogViewModel
    {
        public int? id { get; set; }
        public int? periodId { get; set; }
        public string periodName { get; set; }
        public string uniqueKey { get; set; }
        public string branchCode { get; set; }
        public string branchName { get; set; }
        public DateTime? postingDate { get; set; }
        public string status { get; set; }
        public string refCode { get; set; }
        public string message { get; set; }
    }

	public class CBSVoucherXMLData
	{
		public int? BranchId { get; set; }
		public DateTime? createdAt { get; set; }
		public DateTime? updatedAt { get; set; }
		public int? periodId { get; set; }
		public string XML_DATA { get; set; }
		public string uniqueKey { get; set; }
		public string type { get; set; }
		public int? status { get; set; }
	}
	public class CBSVoucherXMLDataVm
	{
		public int? BranchId { get; set; }
		public string branchCode { get; set; }
		public string branchName { get; set; }
		public DateTime? createdAt { get; set; }
		public DateTime? updatedAt { get; set; }
		public int? periodId { get; set; }
		public string XML_DATA { get; set; }
		public string uniqueKey { get; set; }
		public string type { get; set; }
		public int? status { get; set; }
	}
	public class CBSVoucherViewModels
	{
		public int? BranchId { get; set; }
		public string branchCode { get; set; }
		public string branchName { get; set; }
		public DateTime? createdAt { get; set; }
		public DateTime? updatedAt { get; set; }
		public int? periodId { get; set; }
		[Column(TypeName = "xml")]
		public string XML_DATA { get; set; }
		public string uniqueKey { get; set; }
		public string type { get; set; }
		public int? status { get; set; }
	}
}
