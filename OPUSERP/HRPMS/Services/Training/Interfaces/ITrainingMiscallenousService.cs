﻿using OPUSERP.HRPMS.Data.Entity.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Training.Interfaces
{
    public interface ITrainingMiscallenousService
    {
        Task<bool> SaveTrainingMiscallenous(TrainingMiscallenous trainingMiscallenous);
        Task<IEnumerable<TrainingMiscallenous>> GetAllTrainingMiscallenous();
        Task<TrainingMiscallenous> GetTrainingMiscallenousId(int id);
        Task<bool> DeleteTrainingMiscallenousId(int id);
    }
}
