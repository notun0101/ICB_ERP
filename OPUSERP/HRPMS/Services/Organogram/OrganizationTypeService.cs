using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Organogram;
using OPUSERP.HRPMS.Services.Organogram.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Organogram
{
    public class OrganizationTypeService : IOrganizationTypeService
    {
        private readonly ERPDbContext _context;

        public OrganizationTypeService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveOrganizationType(OrganizationType organizationType)
        {
            if (organizationType.Id != 0)
                _context.organizationTypes.Update(organizationType);
            else
                _context.organizationTypes.Add(organizationType);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrganizationType>> GetAllOrganizationType()
        {
            return await _context.organizationTypes.AsNoTracking().ToListAsync();
        }

        public async Task<OrganizationType> GetOrganizationTypeById(int id)
        {
            return await _context.organizationTypes.FindAsync(id);
        }

        public async Task<bool> DeleteOrganizationTypeById(int id)
        {
            _context.organizationTypes.Remove(_context.organizationTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

		public async Task<IEnumerable<OrganogramRelation>> GetAllOrganogram()
		{
			var data = await _context.organogramRelations.AsNoTracking().ToListAsync();
			return data;
		}
		public IEnumerable<OrganogramRelation> GetAllOrganogramList()
		{
			var data = _context.organogramRelations.Where(x => x.isDelete == 0).AsNoTracking().ToList();
			return data;
		}
		public IEnumerable<OrganogramChild> GetChildrenByOrganogramId(int id)
		{
			var data = _context.organogramChildren.Include(x => x.designation).Where(x => x.OrgRelationId == id).AsNoTracking().ToList();
			return data;
		}
		public string GetPhotoByEmpId(int id)
		{
			var data = (from e in _context.employeeInfos
					   join p in _context.photographs
					   on e.Id equals p.employeeId
						where e.Id == id
					   select p.url).FirstOrDefault();
			return data;
		}

        public async Task<IEnumerable<OrganogramChild>> GetChildrenByOrganogramId1(int id)
        {
            return await _context.organogramChildren.Include(x => x.designation).Include(x => x.OrgRelation).Where(x => x.OrgRelationId == id).ToListAsync();
        }

        public async Task<int> SaveOrganogramRelation(OrganogramRelation organogramRelation)
        {
            if (organogramRelation.Id != 0)
                _context.organogramRelations.Update(organogramRelation);
            else
                _context.organogramRelations.Add(organogramRelation);

             await _context.SaveChangesAsync();
            return organogramRelation.Id;
        }

        public async Task<bool> SaveOrganogramChild(OrganogramChild organogramChild)
        {
            if (organogramChild.Id != 0)
                _context.organogramChildren.Update(organogramChild);
            else
                _context.organogramChildren.Add(organogramChild);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteOrgChildrenById(int id)
        {
            _context.organogramChildren.Remove(_context.organogramChildren.Find(id));
            await _context.SaveChangesAsync();
            return id;
        }
        
       
        public async Task<int> DeleteOrgChildrenByRelationId(int id )
        {

            _context.organogramChildren.RemoveRange(_context.organogramChildren.Where(x => x.OrgRelation.Id == id));
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<int> DeleteOrgRelationById(int id)
        {
            _context.organogramRelations.Remove(_context.organogramRelations.Find(id));
            await _context.SaveChangesAsync();
            return id;
        }
    }
}
