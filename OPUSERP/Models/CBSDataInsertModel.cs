using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Models
{
	public class CBSDataInsertModel
	{
		public string in_user_id { get; set; }
		public string in_Auth_Key { get; set; }
		public string in_ChkSum { get; set; }
		public string in_request_id { get; set; }
		public string in_xml_data { get; set; }
		public string in_Bin_data { get; set; }
		public string in_request_type { get; set; }
		public string in_Add_Param_One { get; set; }
		public string in_Add_Param_Two { get; set; }
	}
}
