using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class SpouseChildrenService : ISpouseChildrenService
    {
        private readonly ERPDbContext _context;

        public SpouseChildrenService(ERPDbContext context)
        {
            _context = context;
        }

        //Spouse
        public async Task<bool> DeleteSpouseById(int id)
        {
            _context.spouses.Remove(_context.spouses.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Spouse>> GetSpouse()
        {
            return await _context.spouses.AsNoTracking().ToListAsync();
        }

        public async Task<Spouse> GetSpouseById(int id)
        {
            var data = await _context.spouses.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }



        public async Task<Spouse> GetSpouseByEmpIdForSingle(int id)
        {
            var data = await _context.spouses.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            return data;
        }
        public async Task<string> GetSpouseImgUrlById(int id)
        {
            return await _context.spouses.Where(x => x.Id == id).Select(x => x.imageUrl).FirstOrDefaultAsync();
        }
        public async Task<string> GetSpouseMarriageImgUrlById(int id)
        {
            return await _context.spouses.Where(x => x.Id == id).Select(x => x.marriageCertificate).FirstOrDefaultAsync();
        }
        public async Task<string> GetChildrenImgUrlById(int id)
        {
            return await _context.childrens.Where(x => x.Id == id).Select(x => x.imageUrl).FirstOrDefaultAsync();
        }

        public async Task<int> SaveSpouse(Spouse spouse)
        {
            if (spouse.Id != 0)
                _context.spouses.Update(spouse);
            else
                _context.spouses.Add(spouse);
            await _context.SaveChangesAsync();
            return spouse.Id;
        }

        public async Task<IEnumerable<Spouse>> GetSpouseByEmpId(int empId)
        {
            return await _context.spouses.Include(x => x.occupation).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
        }
        //ADD Spouse Picture
        public async Task<Spouse> GetSpouseByEmpId1(int empId)
        {
            return await _context.spouses.Where(x => x.employeeId == empId).AsNoTracking().FirstOrDefaultAsync();
        }

        //Children
        public async Task<bool> DeleteChildrenById(int id)
        {
            _context.childrens.Remove(_context.childrens.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<ChildrenEducation>> GetChildEduById(int id)
        {
            return await _context.childrenEducations.Include(x => x.children).Include(x => x.degree).Include(x=> x.levelOfEducation).Where(x => x.childrenId == id).ToListAsync();
        }

        public async Task<IEnumerable<Children>> GetChildren()
        {
            return await _context.childrens.AsNoTracking().ToListAsync();
        }

        public async Task<Children> GetChildrenById(int id)
        {
            return await _context.childrens.FindAsync(id);
        }

        public async Task<int> SaveChildren(Children children)
        {
			try
			{
				if (children.Id != 0)
					_context.childrens.Update(children);
				else
					_context.childrens.Add(children);
				await _context.SaveChangesAsync();
				return children.Id;
			}
			catch (System.Exception ex)
			{

				throw;
			}
        }
        public async Task<int> SaveChildrenEducation(ChildrenEducation childrenEdu)
        {
            try
            {
                if (childrenEdu.Id != 0)
                    _context.childrenEducations.Update(childrenEdu);
                else
                    _context.childrenEducations.Add(childrenEdu);
                await _context.SaveChangesAsync();
                return childrenEdu.Id;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }


        public async Task<IEnumerable<Children>> GetChildrenByEmpId(int empId)
        {
            return await _context.childrens.Include(x => x.occupation).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
        }
        public async Task<Children> GetchildByEmpId1(int empId)
        {
            return await _context.childrens.Where(x => x.employeeId == empId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<bool> SaveEmployeeHobby(EmployeeHobby employeeHobby)
        {
            if (employeeHobby.Id != 0)
                _context.employeeHobbies.Update(employeeHobby);
            else
                _context.employeeHobbies.Add(employeeHobby);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteChildrenEducationById(int id)
        {
            _context.childrenEducations.Remove(_context.childrenEducations.Find(id));
            await _context.SaveChangesAsync();
            return id;
        }
        public async Task<IEnumerable<Data.Entity.Master.Degree>> GetDegreesByLoeId(int id)
        {
            return await _context.degrees.Where(x => x.levelofeducationId == id).ToListAsync();
        }
    }
}
