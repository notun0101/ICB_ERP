using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class WagesPhotographService : IWagesPhotographService
    {
        private readonly ERPDbContext _context;
        private readonly OPUSERP.Data.ERPDbContext _context1;

        public WagesPhotographService(ERPDbContext context, OPUSERP.Data.ERPDbContext _context1)
        {
            _context = context;
            _context1 = context;
        }

        public async Task<bool> DeletePhotographById(int id)
        {
            _context.wagesPhotographs.Remove(_context.wagesPhotographs.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<WagesPhotograph> GetPhotographByEmpIdAndType(int empId, string type)
        {
            return await _context.wagesPhotographs.Where(x => x.type == type && x.employeeId == empId).FirstOrDefaultAsync();
        }

        public async Task<WagesPhotograph> GetPhotographById(int id)
        {
            return await _context.wagesPhotographs.FindAsync(id);
        }

        public async Task<IEnumerable<WagesPhotograph>> GetPhotographs()
        {
            return await _context.wagesPhotographs.AsNoTracking().ToListAsync();
        }

        public async Task<bool> SavePhotograph(WagesPhotograph photograph)
        {
            if (photograph.Id != 0)
                _context.wagesPhotographs.Update(photograph);
            else
                _context.wagesPhotographs.Add(photograph);

            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<int> GetEditSecByEmpId(int id, string name)
        {
            if (name == "addresse")
            {
                return await _context.addresses.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "spousee")
            {
                return await _context.spouses.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            //remarks double table data save
            else if (name == "childrene")
            {
                return await _context.childrens.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "emergencye")
            {
                return await _context.emergencyContacts.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            //remarks double table data save
            else if (name == "nomineee")
            {
                return await _context.nominees.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "insurancee")
            {
                return await _context.employeeInsurances.Where(x => x.employeeInfoId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "educatione")
            {
                return await _context.educationalQualifications.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "profqualificatione")
            {
                return await _context.professionalQualifications.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "otherqualificatione")
            {
                return await _context.otherQualifications.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "traininge")
            {
                return await _context.traningLogs.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "servicee")
            {
                return await _context.transferLogs.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "promotione")
            {
                return await _context.promotionLogs.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "pmse")
            {
                return await _context.acrInfos.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "diciplinee")
            {
                return await _context.disciplinaryLogs.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "supervisore")
            {
                return await _context.supervisors.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "drivinge")
            {
                return await _context.drivingLicenses.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "passporte")
            {
                return await _context.passportDetails.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "travele")
            {
                return await _context.traveInfos.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "membere")
            {
                return await _context.employeeMemberships.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "awarde")
            {
                return await _context.employeeAwards.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "publicatione")
            {
                return await _context.publications.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "languagee")
            {
                return await _context.employeeLanguages.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "otheractivitiese")
            {
                return await _context.otherActivities.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            //bankAccounts
            else if (name == "accountse")
            {
                return await _context.bankInfos.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "belongingse")
            {
                return await _context.Belongings.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "prevempe")
            {
                return await _context.priviousEmployments.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "freedome")
            {
                return await _context.freedomFighters.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "refe")
            {
                return await _context.wagesReferences.Where(x => x.employeeID == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "officeassigne")
            {
                return await _context.officeAssigns.Where(x => x.employeeInfoId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "picturee")
            {
                return await _context.photographs.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "signe")
            {
                return await _context.signatures.Where(x => x.employeeInfoId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "contracte")
            {
                return await _context.EmployeeContractInfos.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "projecte")
            {
                return await _context.employeeProjectActivities.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "finantiale")
            {
                return await _context.financeCodes.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "attatchmente")
            {
                return await _context.hRPMSAttachments.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "proassigne")
            {
                return await _context.employeeProjectAssigns.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "otherinfoe")
            {
                return await _context.employeeOtherInfos.Where(x => x.employeeInfoId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "costcentere")
            {
                return await _context.employeeCostCentres.Where(x => x.employeeInfoId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "empgradee")
            {
                return await _context.employeeGrades.Where(x => x.employeeInfoId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            /////id check
            else if (name == "shiftassigne")
            {
                return await _context.shiftGroupDetails.Where(x => x.Id == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "leavestatuse")
            {
                return await _context.addresses.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "sociale")
            {
                return await _context.employeeInfos.Where(x => x.Id == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "ieltse")
            {
                return await _context.ieltsInfos.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            //favouriteColor
            else if (name == "addinfoe")
            {
                return await _context.employeeInfos.Where(x => x.Id == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "emphobbye")
            {
                return await _context.employeeHobbies.Where(x => x.employeeInfoId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "copliteracye")
            {
                return await _context.hrComputerLiteracies.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "diplomae")
            {
                return await _context.bankingDiplomas.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "taxe")
            {
                return await _context.taxes.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "foode")
            {
                return await _context.foodLikings.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "duale")
            {
                return await _context.duelResidences.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "Sport")
            {
                return await _context.employeeSports.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "Rolee")
            {
                return await _context.employeeRoles.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (name == "diseases")
            {
                return await _context.employeeDiseases.Where(x => x.employeeInfoId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
             else if (name == "benifits")
            {
                return await _context.mobileBenifits.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
           
              else if (name == "suspensions")
            {
                return await _context.suspensions.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
           
               else if (name == "allegations")
            {
                return await _context.allegations.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
             else if (name == "employeeExtraCurricular")
            {
                return await _context.employeeExtraCurriculars.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
           


            else
            {
                return await _context.addresses.Where(x => x.employeeId == id && x.isDelete == 1).Select(x => x.Id).FirstOrDefaultAsync();
            }
        }
    }
}
