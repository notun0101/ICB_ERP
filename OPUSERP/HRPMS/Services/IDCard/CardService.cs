using OPUSERP.Data;
using OPUSERP.HRPMS.Services.IDCard.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.IDCard
{
	public class CardService:IIdCard
	{
		private readonly ERPDbContext _context;

		public CardService(ERPDbContext context)
		{
			_context = context;
		}


	}
}
