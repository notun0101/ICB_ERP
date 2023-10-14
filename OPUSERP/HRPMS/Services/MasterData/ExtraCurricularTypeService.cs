using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class ExtraCurricularTypeService : IExtraCurricularTypeService
    {
        private readonly ERPDbContext _context;

        public ExtraCurricularTypeService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveExtraCurricularType(ExtraCurricularType extraCurricularType)
        {
            if (extraCurricularType.Id != 0)
                _context.extraCurricularTypes.Update(extraCurricularType);
            else
                _context.extraCurricularTypes.Add(extraCurricularType);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExtraCurricularType>> GetExtraCurricularType()
        {
            return await _context.extraCurricularTypes.AsNoTracking().ToListAsync();
        }


        public async Task<ExtraCurricularType> GetExtraCurricularTypeId(int id)
        {
            return await _context.extraCurricularTypes.FindAsync(id);
        }

        public async Task<bool> DeleteExtraCurricularTypeId(int id)
        {
            _context.extraCurricularTypes.Remove(_context.extraCurricularTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
