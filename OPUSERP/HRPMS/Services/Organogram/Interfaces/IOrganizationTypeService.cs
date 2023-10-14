using OPUSERP.HRPMS.Data.Entity.Organogram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Organogram.Interfaces
{
    public interface IOrganizationTypeService
    {
        Task<bool> SaveOrganizationType(OrganizationType organizationType);
        Task<IEnumerable<OrganizationType>> GetAllOrganizationType();
        Task<OrganizationType> GetOrganizationTypeById(int id);
        Task<bool> DeleteOrganizationTypeById(int id);
		Task<IEnumerable<OrganogramRelation>> GetAllOrganogram();
		IEnumerable<OrganogramRelation> GetAllOrganogramList();
		IEnumerable<OrganogramChild> GetChildrenByOrganogramId(int id);
        Task<int> SaveOrganogramRelation(OrganogramRelation organogramRelation);
        Task<bool> SaveOrganogramChild(OrganogramChild organogramChild);
        Task<IEnumerable<OrganogramChild>> GetChildrenByOrganogramId1(int id);
        Task<int> DeleteOrgChildrenById(int id);
        Task<int> DeleteOrgRelationById(int id);
        Task<int> DeleteOrgChildrenByRelationId(int id);
		string GetPhotoByEmpId(int id);

	}
}
