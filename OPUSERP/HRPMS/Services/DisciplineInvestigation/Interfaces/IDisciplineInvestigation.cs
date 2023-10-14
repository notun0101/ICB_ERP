using OPUSERP.HRPMS.Data.Entity.DisciplineInvestigation;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.DisciplineInvestigation.Interfaces
{
    public interface IDisciplineInvestigation
    {
        #region Offense
        Task<int> SaveOffense(Offense offense);
        Task<IEnumerable<Offense>> GetAllOffense();
        Task<Offense> GetOffenseById(int id);
        Task<bool> DeleteOffenseById(int id);
        Task<IEnumerable<Offense>> GetOffenseByempId(int id);

        #endregion

        #region Punishment

        Task<int> SavePunishment(NaturalPunishment punishment);
        Task<IEnumerable<NaturalPunishment>> GetAllPunishment();
        //Task<NaturalPunishment> GetPunishmentById(int id);
        Task<IEnumerable<NaturalPunishment>> GetPunishmentbyId(int id);
        Task<bool> DeletePunishmentById(int id);

        #endregion

        #region Disciplinary Action

        Task<int> SaveDisciplinary(DisciplinaryAction disciplinary);
        Task<IEnumerable<DisciplinaryAction>> GetAllDisciplinary();
        Task<IEnumerable<DisciplinaryAction>> GetAllDisciplinaryByEmpIdandStatus(int id, string status);
        Task<DisciplinaryAction> GetDisciplinaryById(int id);
        Task<bool> DeleteDisciplinaryById(int id);

        Task<IEnumerable<DisciplinaryAction>> GetDisciplinaryByStatus(string status);
        Task<bool> UpdateDisciplinaryStatus(int Id, string Type);

        #endregion

        #region Disciplinary Attachment

        Task<int> SaveDisciplinaryAttachment(DisiciplinaryAttachment disciplinary);
        Task<IEnumerable<DisiciplinaryAttachment>> GetAllDisciplinaryAttachment();
        Task<DisiciplinaryAttachment> GetDisciplinaryAttachmentById(int id);
        Task<bool> DeleteDisciplinaryAttachmentById(int id);

        #endregion

        #region Allegation
        Task<int> SaveAllegation(Allegation allegation);
        Task<IEnumerable<Allegation>> GetAllAllegation();
        Task<Allegation> GetAllegationById(int id);
        Task<bool> DeleteAllegationById(int id);
        Task<string> GetAllegationUrlById(int id);
        Task<string> GetClarificationUrlById(int id);
        Task<string> GetManagementUrlById(int id);

        #endregion
    }
}
