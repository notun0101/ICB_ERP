using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.TrainingNew
{
    public class TrainingResourcePersonService: ITrainingResourcePersonService
    {
        private readonly ERPDbContext _context;

        public TrainingResourcePersonService(ERPDbContext context)
        {
            _context = context;
        }

        //TrainingResourcePerson
        public async Task<bool> DeleteTrainingResourcePersonById(int id)
        {
            _context.trainingResourcePersons.Remove(_context.trainingResourcePersons.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrainingResourcePerson>> GetTrainingResourcePerson()
        {
            return await _context.trainingResourcePersons.AsNoTracking().ToListAsync();
        }

        public async Task<TrainingResourcePerson> GetTrainingResourcePersonById(int id)
        {
            return await _context.trainingResourcePersons.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TrainingResourcePerson>> GetTrainingResourcePersonByTrainingId(int trainingId)
        {
            var data= await _context.trainingResourcePersons.Where(x => x.trainingInfoNewId == trainingId).Include(x => x.resourcePerson).Include(x => x.trainingInfoNew).AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<ResourcePerson> GetResourcePersonById(int id)
        {
            return await _context.resourcePersons.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<string>> GetTrainingResourcePersonByTraining(int id)
        {
            return await _context.trainingResourcePersons.Where(x => x.trainingInfoNewId == id).Include(x => x.resourcePerson).Include(x => x.trainingInfoNew).Select(x => x.resourcePerson.name).AsNoTracking().ToListAsync();
        }

        public async Task<int> SaveTrainingResourcePerson(TrainingResourcePerson trainingResourcePerson)
        {
            if (trainingResourcePerson.Id != 0)
                _context.trainingResourcePersons.Update(trainingResourcePerson);
            else
                _context.trainingResourcePersons.Add(trainingResourcePerson);
            await _context.SaveChangesAsync();
            return trainingResourcePerson.Id;
        }

        public async Task<IEnumerable<TrainingTaDa>> GetTrainingTaDaList()
        {
            return await _context.trainingTaDas.Include(x=>x.training).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewList() 
        {
            return await _context.trainingInfoNews.AsNoTracking().ToListAsync();
        }

        public async Task<bool> SaveTrainingTaDa(TrainingTaDa trainingTaDa)
        {
            if (trainingTaDa.Id != 0)
            {
                _context.trainingTaDas.Update(trainingTaDa);
            }
            else
            {
                _context.trainingTaDas.Add(trainingTaDa);
            }
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteTrainingTAdaById(int id)
        {
            _context.trainingTaDas.Remove(_context.trainingTaDas.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
