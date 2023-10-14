using OPUSERP.Data.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ERPServices.AuthService.Interfaces
{
    public interface INavbarService
    {
        Task<bool> DeleteNavbarItemById(int id);
        Task<IEnumerable<Navbar>> GetNavbarItem();
        Task<IEnumerable<Navbar>> GetNavigationMenu(string userName);
        Task<IEnumerable<Navbar>> GetNavbarItemByParent();
        Task<IEnumerable<Navbar>> GetNavbarItemByParentByModule(int moduleId, int isParent);
        Task<Navbar> GetNavbarItemById(int id);
        Task<bool> SaveNavbarItem(Navbar navbar);
        Task<IEnumerable<Navbar>> GetNavbarItemByParentIdByModule(int moduleId, int isParent);
        Task<int> GetNavbarParentIdById(int id);
        Task<IEnumerable<Navbar>> GetNavbarItemByModule(int id);
		Task<bool> SaveModule(ERPModule module);
		Task<IEnumerable<ERPModule>> GetModules();
		Task<bool> SaveMasterMenu(MasterMenu masterMenu);
		Task<IEnumerable<MasterMenu>> GetMasterMenus();
        Task<Navbar> GetNavByAreaControllerAction(string area, string controller, string action);

    }
}
