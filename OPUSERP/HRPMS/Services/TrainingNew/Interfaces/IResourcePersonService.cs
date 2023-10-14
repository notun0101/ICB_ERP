﻿using OPUSERP.HRPMS.Data.Entity.Suspensions;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.TrainingNew.Interfaces
{
   public interface IResourcePersonService
    {
        Task<int> SaveResourcePersonInfo(ResourcePerson resourcePerson);
        Task<IEnumerable<ResourcePerson>> GetResourcePersonInfo();
        Task<IEnumerable<ResourcePerson>> GetResourcePersonInfoByOrg(string org);
        Task<ResourcePerson> GetResourcePersonInfoById(int id);
        Task<bool> DeleteResourcePersonInfoById(int id);
        Task<bool> UpdateResourcePersonInfoById(ResourcePerson resourcePerson);
        Task<bool> DeleteTrainerById(int id);
        Task<string> GetresourcePersonImgUrlById(int id);
        Task<IEnumerable<ResourcePerson>> GetResourcePersonInfoByOrgNew();





        Task<IEnumerable<EnrolledTrainee>> GetAllEnrollTrainee();
        Task<TrainingInfoNew> GetTrainingCoOrdinatorById(int id);


    }
}
