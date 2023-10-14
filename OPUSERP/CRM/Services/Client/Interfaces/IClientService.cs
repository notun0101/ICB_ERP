﻿using OPUSERP.Areas.CRMClient.Models;
using OPUSERP.CRM.Data.Entity.Client;
using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.CRM.Data.Entity.MasterData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.CRM.Services.Client.Interfaces
{
    public interface IClientService
    {
        #region Clients
        Task<int> SaveClients(Clients Clients);
        Task<IEnumerable<Clients>> GetAllClients();
        Task<IEnumerable<LeadProgressStatus>> GetLeadProgresses();
        Task<Clients> GetClientsById(int id);
        Task<bool> DeletClientsById(int id);
        Task<IEnumerable<GetClientInfoListViewModel>> GetClientsByOwner(string ownername);
        Task<Clients> GetClientsByLeadId(int id);
        Task<bool> DeletClientsByleadId(int id);
        Task<bool> DeletClientsleadsByleadId(int id);
        Task<bool> UpdateClient(int Id);
        Task<bool> UpdateReClient(int Id);
        Task<IEnumerable<Leads>> GetAllClientsFromLeads();
        Task<IEnumerable<GetClientInfoListViewModel>> GetClientsByOwnerfilter(string ownername, string Teamleader, string FaName, string BD, string LeadId);
        Task<IEnumerable<Leads>> GetAllClientsFromLeadsWithImage();
        #endregion


    }
}
