using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSAttendence.Models;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSReport.Models;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using Microsoft.EntityFrameworkCore;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Areas.HRPMSDisciplineInvestigation.Models;
using OPUSERP.Helpers;

namespace OPUSERP.HRPMS.Services.Report
{
    public class ReportsService : IReports
    {
        private readonly ERPDbContext _context;

        public ReportsService(ERPDbContext context)
        {
            _context = context;
        }


        public IEnumerable<IndividualAttendanceReportViewModel> GetJobCardReport(string empCode, string fromDate, string toDate)
        {
            //return _context.JobCardReportViewModels.FromSql("IndividualAttendanceReport @p0,@p1,@p2",empCode, fromDate, toDate).ToList();  
            //return _context.JobCardReportViewModels.FromSql(@"call IndividualAttendanceReport ({0},{1},{2})", empCode, fromDate, toDate).ToList();

            return _context.individualAttendanceReportViewModels.FromSql(@"SP_GET_Attendance @p0,@p1,@p2", empCode, fromDate, toDate).ToList();
        }

        //Here We GetQuery Result
        public async Task<IEnumerable<EmployeeReport>> GetEmployeeInfoAsQueryAble(string queryString, string org)
        {
            IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Include(x => x.lastDesignation).Where(x => x.orgType == org);

            #region Filtering...

            string[] Tokens = queryString.Split("|");
            foreach (string token in Tokens)
            {
                string[] SepToken = token.Split("=");
                if (SepToken.Length > 1)
                {
                    if (SepToken[0] == "Gender") queryData = queryData.Where(x => x.gender == SepToken[1]);
                    else if (SepToken[0] == "HomeDistrict") queryData = queryData.Where(x => x.homeDistrict == SepToken[1]);
                    else if (SepToken[0] == "Disability") queryData = queryData.Where(x => x.disability == SepToken[1]);
                    else if (SepToken[0] == "MaritalStatus") queryData = queryData.Where(x => x.maritalStatus == SepToken[1]);
                    else if (SepToken[0] == "Religion") queryData = queryData.Where(x => x.religionId == Int32.Parse(SepToken[1]));
                    else if (SepToken[0] == "BloodGroup") queryData = queryData.Where(x => x.bloodGroup == SepToken[1]);
                    else if (SepToken[0] == "EmployeePosition") queryData = queryData.Where(x => x.employeeTypeId == Int32.Parse(SepToken[1]));
                    else if (SepToken[0] == "FreedomFighter") queryData = queryData.Where(x => x.freedomFighter == (SepToken[1] == "Yes" ? true : false));
                    else if (SepToken[0] == "NatureRecrutement") queryData = queryData.Where(x => x.natureOfRequitment == SepToken[1]);
                    else if (SepToken[0] == "joiningDesignation") queryData = queryData.Where(x => x.joiningDesignation == SepToken[1]);
                    else if (SepToken[0] == "CurrentDesignation") queryData = queryData.Where(x => x.lastDesignation.designationName == SepToken[1]);
                    else if (SepToken[0] == "SBU") queryData = queryData.Where(x => x.branchId == Int32.Parse(SepToken[1]));
                    else if (SepToken[0] == "PNS") queryData = queryData.Where(x => x.pNSId == Int32.Parse(SepToken[1]));

                    else if (SepToken[0] == "Division")
                    {
                        List<int> Ids = await _context.addresses.Where(x => x.divisionId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "District")
                    {
                        List<int> Ids = await _context.addresses.Where(x => x.districtId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Thana")
                    {
                        List<int> Ids = await _context.addresses.Where(x => x.thanaId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Degree")
                    {
                        List<int> Ids = await _context.educationalQualifications.Where(x => x.degreeId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Group")
                    {
                        List<int> Ids = await _context.educationalQualifications.Where(x => x.reldegreesubjectId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "University")
                    {
                        List<int> Ids = await _context.educationalQualifications.Where(x => x.organizationId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "SpouseHomeDistrict")
                    {
                        List<int> Ids = await _context.addresses.Where(x => x.districtId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Language")
                    {
                        List<int> Ids = await _context.employeeLanguages.Where(x => x.languageId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "AdverseComment")
                    {
                        List<int> Ids = await _context.acrInfos.Where(x => x.advanceComment == SepToken[1]).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "TravelCountry")
                    {
                        List<int> Ids = await _context.traveInfos.Where(x => x.countryId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "IBASS")
                    {
                        List<int> Ids = await _context.bankInfos.Where(x => x.ibus == SepToken[1]).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "LicenseCategory")
                    {
                        List<int> Ids = await _context.drivingLicenses.Where(x => x.category == SepToken[1]).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "LeaveType")
                    {
                        List<int> Ids = await _context.leaveLogs.Where(x => x.Id == Int32.Parse(SepToken[1])).Select(x => (int)x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Project")
                    {
                        List<int> Ids = await _context.employeeProjectActivities.Where(x => x.hRProjectId == Int32.Parse(SepToken[1])).Select(x => (int)x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Doner")
                    {
                        List<int> Ids = await _context.employeeProjectActivities.Where(x => x.hRDonerId == Int32.Parse(SepToken[1])).Select(x => (int)x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Activity")
                    {
                        List<int> Ids = await _context.employeeProjectActivities.Where(x => x.hRActivityId == Int32.Parse(SepToken[1])).Select(x => (int)x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "DateOfBirth") queryData = queryData.Where(x => (x.dateOfBirth >= DateTime.Parse(SepToken[1]) && x.dateOfBirth <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "DateofJoining") queryData = queryData.Where(x => (x.joiningDateGovtService >= DateTime.Parse(SepToken[1]) && x.joiningDateGovtService <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "LPRDate") queryData = queryData.Where(x => (x.LPRDate >= DateTime.Parse(SepToken[1]) && x.LPRDate <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "PRLStartDate") queryData = queryData.Where(x => (x.PRLStartDate >= DateTime.Parse(SepToken[1]) && x.PRLStartDate <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "PRLEndDate") queryData = queryData.Where(x => (x.PRLEndDate >= DateTime.Parse(SepToken[1]) && x.PRLEndDate <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "DateofRegularity") queryData = queryData.Where(x => (x.dateofregularity >= DateTime.Parse(SepToken[1]) && x.dateofregularity <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "DateofConfirmation") queryData = queryData.Where(x => (x.dateOfPermanent >= DateTime.Parse(SepToken[1]) && x.dateOfPermanent <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "ACRStartDate")
                    {
                        //List<int> Ids = await _context.acrInfos.Where(x => (DateTime.Parse(x.startDate) >= DateTime.Parse(SepToken[1]) && DateTime.Parse(x.startDate) <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        List<int> Ids = await _context.acrInfos.Where(x => (x.startDate >= DateTime.Parse(SepToken[1]) && x.startDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ACREndDate")
                    {
                        //List<int> Ids = await _context.acrInfos.Where(x => (DateTime.Parse(x.endDate) >= DateTime.Parse(SepToken[1]) && DateTime.Parse(x.endDate) <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        List<int> Ids = await _context.acrInfos.Where(x => (x.endDate >= DateTime.Parse(SepToken[1]) && x.endDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "PassportIssueDate")
                    {
                        List<int> Ids = await _context.passportDetails.Where(x => (x.dateOfIssue >= DateTime.Parse(SepToken[1]) && x.dateOfIssue <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "PassportExpiryDate")
                    {
                        List<int> Ids = await _context.passportDetails.Where(x => (x.dateOfExpair >= DateTime.Parse(SepToken[1]) && x.dateOfExpair <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "TravelStartDate")
                    {
                        List<int> Ids = await _context.traveInfos.Where(x => (x.startDate >= DateTime.Parse(SepToken[1]) && x.startDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "TravelEndDate")
                    {
                        List<int> Ids = await _context.traveInfos.Where(x => (x.endDate >= DateTime.Parse(SepToken[1]) && x.endDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "LicenseIssueDate")
                    {
                        List<int> Ids = await _context.drivingLicenses.Where(x => (x.dateOfIssue >= DateTime.Parse(SepToken[1]) && x.dateOfIssue <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "LicenseExpiryDate")
                    {
                        List<int> Ids = await _context.drivingLicenses.Where(x => (x.dateOfExpair >= DateTime.Parse(SepToken[1]) && x.dateOfExpair <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ServiceFromDate")
                    {
                        List<int> Ids = await _context.transferLogs.Where(x => (x.from >= DateTime.Parse(SepToken[1]) && x.from <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ServiceToDate")
                    {
                        List<int> Ids = await _context.transferLogs.Where(x => (x.to >= DateTime.Parse(SepToken[1]) && x.to <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "BookBorrowDate")
                    {
                        List<int> Ids = await _context.borrowerInfos.Where(x => (x.dateOfBorrow >= DateTime.Parse(SepToken[1]) && x.dateOfBorrow <= DateTime.Parse(SepToken[2]))).Select(x => x.borrowerId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "BookReturnDate")
                    {
                        List<int> Ids = await _context.borrowerInfos.Where(x => (x.dateOfReturn >= DateTime.Parse(SepToken[1]) && x.dateOfReturn <= DateTime.Parse(SepToken[2]))).Select(x => x.borrowerId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "LeaveFromDate")
                    {
                        List<int?> Ids = await _context.leaveLogs.Where(x => (x.leaveFrom >= DateTime.Parse(SepToken[1]) && x.leaveFrom <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "LeaveToDate")
                    {
                        List<int?> Ids = await _context.leaveLogs.Where(x => (x.leaveTo >= DateTime.Parse(SepToken[1]) && x.leaveTo <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "TrainingFromDate")
                    {
                        List<int?> Ids = await _context.enrolledTrainees.Include(x => x.trainingInfoNewId).Where(x => (x.trainingInfoNew.startDateActual >= DateTime.Parse(SepToken[1]) && x.trainingInfoNew.startDateActual <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "TrainingToDate")
                    {
                        List<int?> Ids = await _context.enrolledTrainees.Include(x => x.trainingInfoNewId).Where(x => (x.trainingInfoNew.endDateActual >= DateTime.Parse(SepToken[1]) && x.trainingInfoNew.endDateActual <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ContractStartDate")
                    {
                        List<int?> Ids = await _context.EmployeeContractInfos.Where(x => (x.contractStartDate >= DateTime.Parse(SepToken[1]) && x.contractStartDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ContractEndDate")
                    {
                        List<int?> Ids = await _context.EmployeeContractInfos.Where(x => (x.contractEndDate >= DateTime.Parse(SepToken[1]) && x.contractEndDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                }
            }
            #endregion



            IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
            List<EmployeeReport> filteredData = new List<EmployeeReport>();
            foreach (EmployeeInfo employeeInfo in data)
            {
                var path = "";
                var imgUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).FirstOrDefaultAsync();
                if (imgUrl != null)
                {
                    path = $"<img src = '/../../" + imgUrl + "' height = '50' width = '50' style = 'border-radius: 50%' alt = 'profile Image'/>";
                }
                else
                {
                    path = $"<img src = '/../../EmpImages/default-avatar.jpg' height = '50' width = '50' style = 'border-radius: 50%' alt = 'profile Image'/>";
                }

                filteredData.Add(new EmployeeReport
                {
                    id = employeeInfo.employeeCode,
                    name = employeeInfo.nameEnglish,
                    currentDepartment = _context.departments.Where(x => x.Id == employeeInfo.departmentId).Select(x => x.deptName).FirstOrDefault(),
                    //currentDesignation = employeeInfo.lastDesignation.designationName,
                    currentDesignation = _context.designations.Where(x => x.Id == employeeInfo.lastDesignationId).Select(x => x.designationName).FirstOrDefault(),
                    email = employeeInfo.emailAddress,
                    mobile = employeeInfo.mobileNumberPersonal,
                    imageUrl = path,

                });
            }

            return filteredData;
        }


        //Here We GetQuery Result
        public async Task<IEnumerable<EmployeeReport>> GetWagesAsQueryAble(string queryString, string org)
        {
            IQueryable<WagesEmployeeInfo> queryData = _context.wagesEmployeeInfos.Where(x => x.orgType == org);

            #region Filtering...

            string[] Tokens = queryString.Split("|");
            foreach (string token in Tokens)
            {
                string[] SepToken = token.Split("=");
                if (SepToken.Length > 1)
                {
                    if (SepToken[0] == "Gender") queryData = queryData.Where(x => x.gender == SepToken[1]);
                    else if (SepToken[0] == "HomeDistrict") queryData = queryData.Where(x => x.homeDistrict == SepToken[1]);
                    else if (SepToken[0] == "ActivityStatus") queryData = queryData.Where(x => x.activityStatus == Int32.Parse(SepToken[1]));
                    else if (SepToken[0] == "Disability") queryData = queryData.Where(x => x.disability == SepToken[1]);
                    else if (SepToken[0] == "MaritalStatus") queryData = queryData.Where(x => x.maritalStatus == SepToken[1]);
                    else if (SepToken[0] == "Religion") queryData = queryData.Where(x => x.religionId == Int32.Parse(SepToken[1]));
                    else if (SepToken[0] == "BloodGroup") queryData = queryData.Where(x => x.bloodGroup == SepToken[1]);
                    else if (SepToken[0] == "EmployeePosition") queryData = queryData.Where(x => x.employeeTypeId == Int32.Parse(SepToken[1]));
                    else if (SepToken[0] == "FreedomFighter") queryData = queryData.Where(x => x.freedomFighter == (SepToken[1] == "Yes" ? true : false));
                    else if (SepToken[0] == "NatureRecrutement") queryData = queryData.Where(x => x.natureOfRequitment == SepToken[1]);
                    else if (SepToken[0] == "joiningDesignation") queryData = queryData.Where(x => x.joiningDesignation == SepToken[1]);
                    else if (SepToken[0] == "CurrentDesignation") queryData = queryData.Where(x => x.designation == SepToken[1]);
                    else if (SepToken[0] == "SBU") queryData = queryData.Where(x => x.branchId == Int32.Parse(SepToken[1]));
                    else if (SepToken[0] == "PNS") queryData = queryData.Where(x => x.pNSId == Int32.Parse(SepToken[1]));

                    else if (SepToken[0] == "Division")
                    {
                        List<int> Ids = await _context.addresses.Where(x => x.divisionId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "District")
                    {
                        List<int> Ids = await _context.addresses.Where(x => x.districtId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Thana")
                    {
                        List<int> Ids = await _context.addresses.Where(x => x.thanaId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Degree")
                    {
                        List<int> Ids = await _context.educationalQualifications.Where(x => x.degreeId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Group")
                    {
                        List<int> Ids = await _context.educationalQualifications.Where(x => x.reldegreesubjectId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "University")
                    {
                        List<int> Ids = await _context.educationalQualifications.Where(x => x.organizationId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "SpouseHomeDistrict")
                    {
                        List<int> Ids = await _context.addresses.Where(x => x.districtId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Language")
                    {
                        List<int> Ids = await _context.employeeLanguages.Where(x => x.languageId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "AdverseComment")
                    {
                        List<int> Ids = await _context.acrInfos.Where(x => x.advanceComment == SepToken[1]).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "TravelCountry")
                    {
                        List<int> Ids = await _context.traveInfos.Where(x => x.countryId == Int32.Parse(SepToken[1])).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "IBASS")
                    {
                        List<int> Ids = await _context.bankInfos.Where(x => x.ibus == SepToken[1]).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "LicenseCategory")
                    {
                        List<int> Ids = await _context.drivingLicenses.Where(x => x.category == SepToken[1]).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "LeaveType")
                    {
                        List<int> Ids = await _context.leaveLogs.Where(x => x.Id == Int32.Parse(SepToken[1])).Select(x => (int)x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Project")
                    {
                        List<int> Ids = await _context.employeeProjectActivities.Where(x => x.hRProjectId == Int32.Parse(SepToken[1])).Select(x => (int)x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Doner")
                    {
                        List<int> Ids = await _context.employeeProjectActivities.Where(x => x.hRDonerId == Int32.Parse(SepToken[1])).Select(x => (int)x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }
                    else if (SepToken[0] == "Activity")
                    {
                        List<int> Ids = await _context.employeeProjectActivities.Where(x => x.hRActivityId == Int32.Parse(SepToken[1])).Select(x => (int)x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "DateOfBirth") queryData = queryData.Where(x => (x.dateOfBirth >= DateTime.Parse(SepToken[1]) && x.dateOfBirth <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "DateofJoining") queryData = queryData.Where(x => (x.joiningDateGovtService >= DateTime.Parse(SepToken[1]) && x.joiningDateGovtService <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "LPRDate") queryData = queryData.Where(x => (DateTime.Parse(x.LPRDate) >= DateTime.Parse(SepToken[1]) && DateTime.Parse(x.LPRDate) <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "PRLStartDate") queryData = queryData.Where(x => (DateTime.Parse(x.PRLStartDate) >= DateTime.Parse(SepToken[1]) && DateTime.Parse(x.PRLStartDate) <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "PRLEndDate") queryData = queryData.Where(x => (DateTime.Parse(x.PRLEndDate) >= DateTime.Parse(SepToken[1]) && DateTime.Parse(x.PRLEndDate) <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "DateofRegularity") queryData = queryData.Where(x => (x.dateofregularity >= DateTime.Parse(SepToken[1]) && x.dateofregularity <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "DateofConfirmation") queryData = queryData.Where(x => (x.dateOfPermanent >= DateTime.Parse(SepToken[1]) && x.dateOfPermanent <= DateTime.Parse(SepToken[2])));

                    else if (SepToken[0] == "ACRStartDate")
                    {
                        //List<int> Ids = await _context.acrInfos.Where(x => (DateTime.Parse(x.startDate) >= DateTime.Parse(SepToken[1]) && DateTime.Parse(x.startDate) <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        List<int> Ids = await _context.acrInfos.Where(x => (x.startDate >= DateTime.Parse(SepToken[1]) && x.startDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ACREndDate")
                    {
                        List<int> Ids = await _context.acrInfos.Where(x => (x.endDate >= DateTime.Parse(SepToken[1]) && x.endDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "PassportIssueDate")
                    {
                        List<int> Ids = await _context.passportDetails.Where(x => (x.dateOfIssue >= DateTime.Parse(SepToken[1]) && x.dateOfIssue <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "PassportExpiryDate")
                    {
                        List<int> Ids = await _context.passportDetails.Where(x => (x.dateOfExpair >= DateTime.Parse(SepToken[1]) && x.dateOfExpair <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "TravelStartDate")
                    {
                        List<int> Ids = await _context.traveInfos.Where(x => (x.startDate >= DateTime.Parse(SepToken[1]) && x.startDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "TravelEndDate")
                    {
                        List<int> Ids = await _context.traveInfos.Where(x => (x.endDate >= DateTime.Parse(SepToken[1]) && x.endDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "LicenseIssueDate")
                    {
                        List<int> Ids = await _context.drivingLicenses.Where(x => (x.dateOfIssue >= DateTime.Parse(SepToken[1]) && x.dateOfIssue <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "LicenseExpiryDate")
                    {
                        List<int> Ids = await _context.drivingLicenses.Where(x => (x.dateOfExpair >= DateTime.Parse(SepToken[1]) && x.dateOfExpair <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ServiceFromDate")
                    {
                        List<int> Ids = await _context.transferLogs.Where(x => (x.from >= DateTime.Parse(SepToken[1]) && x.from <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ServiceToDate")
                    {
                        List<int> Ids = await _context.transferLogs.Where(x => (x.to >= DateTime.Parse(SepToken[1]) && x.to <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "BookBorrowDate")
                    {
                        List<int> Ids = await _context.borrowerInfos.Where(x => (x.dateOfBorrow >= DateTime.Parse(SepToken[1]) && x.dateOfBorrow <= DateTime.Parse(SepToken[2]))).Select(x => x.borrowerId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "BookReturnDate")
                    {
                        List<int> Ids = await _context.borrowerInfos.Where(x => (x.dateOfReturn >= DateTime.Parse(SepToken[1]) && x.dateOfReturn <= DateTime.Parse(SepToken[2]))).Select(x => x.borrowerId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "LeaveFromDate")
                    {
                        List<int?> Ids = await _context.leaveLogs.Where(x => (x.leaveFrom >= DateTime.Parse(SepToken[1]) && x.leaveFrom <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "LeaveToDate")
                    {
                        List<int?> Ids = await _context.leaveLogs.Where(x => (x.leaveTo >= DateTime.Parse(SepToken[1]) && x.leaveTo <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "TrainingFromDate")
                    {
                        List<int?> Ids = await _context.enrolledTrainees.Include(x => x.trainingInfoNewId).Where(x => (x.trainingInfoNew.startDateActual >= DateTime.Parse(SepToken[1]) && x.trainingInfoNew.startDateActual <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "TrainingToDate")
                    {
                        List<int?> Ids = await _context.enrolledTrainees.Include(x => x.trainingInfoNewId).Where(x => (x.trainingInfoNew.endDateActual >= DateTime.Parse(SepToken[1]) && x.trainingInfoNew.endDateActual <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ContractStartDate")
                    {
                        List<int?> Ids = await _context.EmployeeContractInfos.Where(x => (x.contractStartDate >= DateTime.Parse(SepToken[1]) && x.contractStartDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                    else if (SepToken[0] == "ContractEndDate")
                    {
                        List<int?> Ids = await _context.EmployeeContractInfos.Where(x => (x.contractEndDate >= DateTime.Parse(SepToken[1]) && x.contractEndDate <= DateTime.Parse(SepToken[2]))).Select(x => x.employeeId).ToListAsync();
                        queryData = queryData.Where(x => Ids.Contains(x.Id));
                    }

                }
            }
            #endregion

            return await queryData.Select(x => new EmployeeReport
            {
                id = x.employeeCode,
                name = x.nameEnglish,
                currentDesignation = x.designation,
                currentDepartment = _context.departments.Where(y => y.Id == x.departmentId).Select(y => y.deptName).FirstOrDefault(),
                email = x.emailAddress,
                mobile = x.mobileNumberPersonal
            }).ToListAsync();
        }

        public async Task<IEnumerable<HrReportViewModel>> GetHrDataByDesig(int? desigId, int? deptId, string bloodGroup, int? sbuId)
        {
            var bGroup = "";
            if (bloodGroup == "Op")
            {
                bGroup = "O+";
            }

            else if (bloodGroup == "On")
            {
                bGroup = "O-";
            }

            else if (bloodGroup == "An")
            {
                bGroup = "A-";
            }
            else if (bloodGroup == "Ap")
            {
                bGroup = "A+";
            }
            else if (bloodGroup == "Bn")
            {
                bGroup = "B-";
            }
            else if (bloodGroup == "Bp")
            {
                bGroup = "B+";
            }
            else if (bloodGroup == "ABn")
            {
                bGroup = "AB-";
            }
            else if (bloodGroup == "ABp")
            {
                bGroup = "AB+";
            }
            else
            {
                bGroup = "0";
            }

            return await _context.hrReportViewModels.FromSql($"SP_RPT_HR {desigId},{deptId},{bGroup},{sbuId}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<HrEducationReportViewModel>> GetHrDataByEducation(int? subjectId, int? universityId, int? loeId)
        {
            return await _context.hrEducationReportViewModels.FromSql($"SP_RPT_HR_Education {subjectId},{universityId},{loeId}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<HrTrainingReportViewModel>> GetHrDataByTrCourse(int? courseId)
        {
            return await _context.hrTrainingReportViewModels.FromSql($"SP_RPT_HR_Training {courseId}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<HrBelongingReportViewModel>> GetHrDataByBelongingItem(int? belongingId)
        {
            return await _context.hrBelongingReportViewModels.FromSql($"SP_RPT_HR_Belonging {belongingId}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<HrSummaryReportViewModel>> GetHrSummaryData(string callName)
        {
            return await _context.hrSummaryReportViewModels.FromSql($"SP_RPT_HRSUMMARY {callName}").AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SignatureReportViewModel>> GetSignatureLists()
        {
            return await _context.signatureReports.FromSql($"sp_GetSignatureLists").AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<HrSummaryReportViewModel>> GetHrSummaryData1(string toDate)
        {
            return await _context.hrSummaryReportViewModels.FromSql($"SP_RPT_HRSUMMARYAge {toDate}").AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<GManViewModel>> GetHrGenderSummaryData1(string toDate)
        {
            var table = new List<GManViewModel>();
            var employees = await _context.employeeInfos.Where(x => (x.activityStatus == 1 || x.activityStatus == 3) && Convert.ToDateTime(x.joiningDateGovtService).Date <= Convert.ToDateTime(toDate).Date).ToListAsync();

            //var data = new GenderManpowerViewModel
            //{
            //    male = employees.Where(x => x.gender == "Male").Count(),
            //    maleRegular = employees.Where(x => x.gender == "Male" && x.employeeTypeId == 1).Count(),
            //    maleContructual = employees.Where(x => x.gender == "Male" && x.employeeTypeId == 2).Count(),
            //    female = employees.Where(x => x.gender == "Female").Count(),
            //    femaleRegular = employees.Where(x => x.gender == "Female" && x.employeeTypeId == 1).Count(),
            //    femaleContructual = employees.Where(x => x.gender == "Female" && x.employeeTypeId == 2).Count()
            //};
            table.Add(new GManViewModel { title = "Male", count = employees.Where(x => x.gender == "Male").Count() });
            table.Add(new GManViewModel { title = "Regular", count = employees.Where(x => x.gender == "Male" && x.employeeTypeId == 1).Count() });
            table.Add(new GManViewModel { title = "Contructual Male", count = employees.Where(x => x.gender == "Male" && x.employeeTypeId == 2).Count() });

            table.Add(new GManViewModel { title = "Female", count = employees.Where(x => x.gender == "Female").Count() });
            table.Add(new GManViewModel { title = "Regular", count = employees.Where(x => x.gender == "Female" && x.employeeTypeId == 1).Count() });
            table.Add(new GManViewModel { title = "Contructual Female", count = employees.Where(x => x.gender == "Female" && x.employeeTypeId == 2).Count() });

            table.Add(new GManViewModel { title = "Grand Total", count = employees.Count() });

            return table;
        }


        public async Task<IEnumerable<EmployeeInfo>> GetReligionWiseEmployee(int id)
        {
            var data = await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.religionId == id && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetDivisionWiseEmployee(int id)
        {
            var data = await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).Include(x => x.Division).Include(x => x.hrDivision).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.DivisionId == id && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetDistrictWiseEmployee(int id)
        {
            var data = await _context.employeeInfos.Include(x => x.religion).Include(x => x.lastDesignation).Include(x => x.department).Include(x => x.branch).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where( x=> x.lastDesignationId != null && x.homeDistrict == _context.Districts.Where(y => y.Id == id).Select(y => y.districtName).FirstOrDefault()).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetBranchWiseEmployee(int id)
        {
            var data = await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).Include(x => x.location).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.locationId == id && x.lastDesignationId != null).ToListAsync();
            return data;
        }


        public async Task<IEnumerable<DesignationSeniorityViewModel>> GetSeniorityDesigByJoiningDate(DateTime joiningDate)
        {
            var data = new List<DesignationSeniorityViewModel>();
            var desig = await _context.designations.Where(x => Convert.ToInt32(x.designationCode) <= 10).OrderBy(x => Convert.ToInt32(x.designationCode)).ToListAsync();
            foreach (var item in desig)
            {
                data.Add(new DesignationSeniorityViewModel
                {
                    designation = item,
                    designationCount = _context.employeeInfos.Where(x => x.lastDesignationId == item.Id && x.seniorityLevel != null && Convert.ToDateTime(x.joiningDateGovtService).Date < Convert.ToDateTime(joiningDate).Date).AsNoTracking().Count(),
                    employees = await _context.seniorityListViewModelNews.FromSql($"sp_GetSeniorityWiseEmployee").Where(x => x.designationId == item.Id && Convert.ToDateTime(x.joiningDate) < Convert.ToDateTime(joiningDate)).ToListAsync()
					//employeeInfos = _context.employeeInfos.Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignationId == item.Id && x.seniorityLevel != null && Convert.ToDateTime(x.joiningDateGovtService).Date < Convert.ToDateTime(joiningDate).Date && x.lastDesignationId != null).AsNoTracking().ToList()
				});
            }

            return data;
        }

		public async Task<IEnumerable<SeniorityListViewModelNew>> GetAllSeniorityList()
		{
			var data = await _context.seniorityListViewModelNews.FromSql($"sp_GetSeniorityWiseEmployee").ToListAsync();
			return data;
		}

		public async Task<IEnumerable<SeniorityListViewModelNew>> GetAllByJoiningDateSeniorityList(DateTime? joiningDate)
		{
			var data = await _context.seniorityListViewModelNews.FromSql($"sp_GetSeniorityWiseEmployee").ToListAsync();
			return data.Where(x => Convert.ToDateTime(x.joiningDate) <= Convert.ToDateTime(joiningDate));
		}



		public async Task<IEnumerable<DesignationSeniorityViewModel>> GetSeniorityDesigBylastPromotionDate(DateTime lastPromotionDate)
        {
            var data = new List<DesignationSeniorityViewModel>();
            var desig = await _context.designations.OrderBy(x => Convert.ToInt32(x.designationCode)).ToListAsync();
            foreach (var item in desig)
            {
                data.Add(new DesignationSeniorityViewModel
                {
                    designation = item,
                    designationCount = _context.employeeInfos.Where(x => x.lastDesignationId == item.Id).AsNoTracking().Count(),

                    employeeInfos = _context.employeeInfos.Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignationId == item.Id && Convert.ToDateTime(x.lastPromotionDate).Date <= Convert.ToDateTime(lastPromotionDate).Date && x.lastPromotionDate != null).AsNoTracking().ToList()
                });
            }

            return data;
        }

        public async Task<IEnumerable<DesignationSeniorityViewModel>> GetSeniorityDepByDate(DateTime postingDate)
        {
            var data = new List<DesignationSeniorityViewModel>();
            // var desig = await _context.designations.ToListAsync();
            var dep = await _context.departments.ToListAsync();
            foreach (var item in dep)
            {
                data.Add(new DesignationSeniorityViewModel
                {
                    // designation = item,

                    department = item,
                    employeeInfos = _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.department).Where(x => x.departmentId == item.Id && Convert.ToDateTime(x.postingDate).Date <= Convert.ToDateTime(postingDate).Date && x.postingDate != null).OrderBy(x => x.seniorityLevel).AsNoTracking().ToList(),
                    designationCount = _context.employeeInfos.Where(x => x.lastDesignationId == item.Id).AsNoTracking().Count(),
                });
            }

            return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetSeniorityByPRLDate(DateTime prlDate)
        {
            //var data = await _context.employeeInfos.Include(x => x.lastDesignation).Where(x => x.prlDate != null && Convert.ToDateTime(x.prlDate) <= Convert.ToDateTime(prlDate)).OrderBy(x => x.seniorityLevel).ToListAsync();
            var data = await _context.employeeInfos.Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => Convert.ToDateTime(x.DateOfRetirement) <= Convert.ToDateTime(prlDate) && x.lastDesignationId != null).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetSeniorityByEqAndDate(DateTime joiningDate, string qualification)
        {

            var data = await _context.employeeInfos.Include(x => x.lastDesignation).Where(x => x.joiningDateGovtService != null && Convert.ToDateTime(x.joiningDateGovtService) <= Convert.ToDateTime(joiningDate) && x.qualification == qualification).OrderBy(x => x.seniorityLevel).ToListAsync();


            return data;
        }

        public async Task<IEnumerable<EducationalQualification>> GetSeniorityByEqAndDate1(DateTime joiningDate, int qualification)
        {
            var data = await _context.educationalQualifications.Include(x => x.reldegreesubject.subject).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.degree).Where(x => x.employee.joiningDateGovtService != null && Convert.ToDateTime(x.employee.joiningDateGovtService) <= Convert.ToDateTime(joiningDate) && x.degreeId == qualification).OrderBy(x => x.employee.seniorityLevel).ToListAsync();

            //var data = await _context.employeeInfos.Include(x => x.lastDesignation).Where(x => x.joiningDateGovtService != null && Convert.ToDateTime(x.joiningDateGovtService) <= Convert.ToDateTime(joiningDate) && x.qualification == qualification).OrderBy(x => x.seniorityLevel).ToListAsync();


            return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeeByBranch(DateTime joiningDate, int Id)
        {
			var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.department).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.joiningDateGovtService != null && Convert.ToDateTime(x.joiningDateGovtService) <= Convert.ToDateTime(joiningDate) && x.hrBranchId == Id && x.lastDesignationId != null).ToListAsync();

			if (Id == 0)
			{
				data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.department).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.joiningDateGovtService != null && Convert.ToDateTime(x.joiningDateGovtService) <= Convert.ToDateTime(joiningDate) && x.lastDesignationId != null).ToListAsync();

			}

			return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeeByDivision(DateTime joiningDate, int divisionId)
        {
            var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.department).Where(x => x.joiningDateGovtService != null && Convert.ToDateTime(x.joiningDateGovtService) <= Convert.ToDateTime(joiningDate) && x.hrDivisionId == divisionId).OrderBy(x => x.seniorityLevel).ToListAsync();

            return data;
        }



        public async Task<Location> GetLocationById(int Id)
        {
            return await _context.Locations.Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<HrBranch> GetBranchNameById(int Id)
        {
            return await _context.hrBranches.Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<FunctionInfo> GetOfficeNameById(int Id)
        {
            return await _context.FunctionInfos.Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<HrDivision> GetDivisionNameById(int divisionId)
        {
            return await _context.hrDivisions.Where(x => x.Id == divisionId).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<Department> GetDepNameById(int Id)
        {
            return await _context.departments.Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<Designation> GetDegNameById(int Id)
        {

            return await _context.designations.Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<EducationalQualification> GetEduCationalSubjectNameById(int subjectId)
        {
            return await _context.educationalQualifications.Where(x => x.Id == subjectId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Designation> GetDesignationById(int Id)
        {
            return await _context.designations.FindAsync(Id);
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByPostingDate(DateTime date, int deptId)
        {

            var data = await (from e in _context.employeeInfos
                              join p in _context.employeePostings
                              on e.Id equals p.employeeId
                              join d in _context.designations
                              on e.lastDesignationId equals d.Id
                              where p.startDate != null && Convert.ToDateTime(p.endDate).Date <= Convert.ToDateTime(date).Date && e.departmentId == deptId
                              select e).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.departmentId == deptId && x.lastDesignationId != null).ToListAsync();
            return data;

        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesDivisionByPostingDate(DateTime date, int divId)
        {
            //var data = await (from e in _context.employeeInfos
            //                  join p in _context.employeePostings
            //                  on e.Id equals p.employeeId
            //                  where p.startDate != null && Convert.ToDateTime(p.endDate).Date <= Convert.ToDateTime(date).Date && e.hrDivisionId == divId
            //                  select e).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.hrDivisionId == divId).ToListAsync();

            var data = await _context.employeeInfos.AsNoTracking().Include(x => x.lastDesignation).Include(x => x.hrDivision).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignationId != null && x.hrDivisionId == divId).ToListAsync();
            return data;
        }
         public async Task<IEnumerable<EmployeeInfo>> GetEmployeesDivisionByPostingDateNew(DateTime date, int divId)
        {
            var data = await (from e in _context.employeeInfos
                              join p in _context.employeePostings
                              on e.Id equals p.employeeId
                              where p.startDate != null && Convert.ToDateTime(p.endDate).Date <= Convert.ToDateTime(date).Date && e.hrDivisionId == divId
                              select e).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).Where(x => x.hrDivisionId == divId).ToListAsync();

            return data;
        }


        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesBranchByPostingDate(DateTime date, int branchId)
        {
            var data = await (from e in _context.employeeInfos
                              join p in _context.employeePostings
                              on e.Id equals p.employeeId
                              //join d in _context.designations
                              //on e.lastDesignationId equals d.Id
                              where p.startDate != null && Convert.ToDateTime(p.endDate).Date <= Convert.ToDateTime(date).Date && e.hrBranchId == branchId 
                              select e).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).Where(x => x.hrBranchId == branchId && x.lastDesignationId != null).ToListAsync();

           // var data = await _context.employeeInfos.AsNoTracking().Include(x => x.lastDesignation).Include(x => x.department).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.hrUnit).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignationId != null).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesDepartmentByPostingDate(DateTime date, int DepartmentId)
        {
            var data = await (from e in _context.employeeInfos
                              join p in _context.employeePostings
                              on e.Id equals p.employeeId
                              where p.startDate != null && Convert.ToDateTime(p.endDate).Date <= Convert.ToDateTime(date).Date && e.departmentId == DepartmentId
                              select e).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel))
                                 .GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).Where(x => x.departmentId == DepartmentId && x.lastDesignationId != null).ToListAsync();
            return data;

            //var data = await _context.employeeInfos.AsNoTracking().Include(x => x.lastDesignation).Include(x => x.department).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.hrUnit).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignationId != null).ToListAsync();
            //return data;
        }




        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesOfficeByPostingDate(DateTime date, int officeId)
        {
            //var data = await (from e in _context.employeeInfos
            //                  join p in _context.employeePostings
            //                  on e.Id equals p.employeeId
            //                  //join d in _context.designations
            //                  //on e.lastDesignationId equals d.Id
            //                  where p.startDate != null && Convert.ToDateTime(p.endDate).Date <= Convert.ToDateTime(date).Date && e.functionInfoId == officeId
            //                  select e).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).Where(x => x.functionInfoId == officeId).ToListAsync();
            var data = await _context.employeeInfos.AsNoTracking().Include(x => x.lastDesignation).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignationId != null && x.functionInfoId == officeId).ToListAsync();
            return data;
        }

         public async Task<IEnumerable<EmployeeInfo>> GetEmployeesOfficeByPostingDateNew(DateTime date, int officeId)
        {
            var data = await (from e in _context.employeeInfos
                              join p in _context.employeePostings
                              on e.Id equals p.employeeId
                              //join d in _context.designations
                              //on e.lastDesignationId equals d.Id
                              where p.startDate != null && Convert.ToDateTime(p.endDate).Date <= Convert.ToDateTime(date).Date && e.functionInfoId == officeId
                              select e).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).Where(x => x.functionInfoId == officeId && x.lastDesignationId != null).ToListAsync();
            return data;
        }






        public async Task<IEnumerable<EmployeePostingPlaceViewModel>> GetPostingsByEmployeeIdAndDate(int id, DateTime date)
        {
            var model = new List<EmployeePostingPlaceViewModel>();
            var data = await _context.employeePostings.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.department).Include(x => x.hrBranch).Where(x => Convert.ToDateTime(x.endDate).Date <= Convert.ToDateTime(date).Date && x.employeeId == id && x.type == 1 && x.status == 1).ToListAsync();
            foreach (var item in data)
            {
                model.Add(new EmployeePostingPlaceViewModel
                {
                    posting = item,
                    duration = item.endDate != null ? CalculateYearsByDays((Convert.ToDateTime(item.endDate).Date - Convert.ToDateTime(item.startDate).Date).Days) : CalculateYearsByDays((Convert.ToDateTime(date).Date - Convert.ToDateTime(item.startDate).Date).Days)
                });
            }
            return model;
        }

        public async Task<IEnumerable<EmployeePostingPlaceViewModel>> GetPostingsByEmployeeIdAndDate1(int id, DateTime date)
        {
            var model = new List<EmployeePostingPlaceViewModel>();
            var data = await _context.employeePostings.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.department).Include(x => x.hrBranch).Where(x => Convert.ToDateTime(x.endDate).Date <= Convert.ToDateTime(date).Date && x.employeeId == id).ToListAsync();

            //var data = await _context.employeeInfos.AsNoTracking().Include(x => x.lastDesignation).Include(x => x.department).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.hrUnit).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignationId != null).ToListAsync();

            foreach (var item in data)
            {
                model.Add(new EmployeePostingPlaceViewModel
                {
                    posting = item,
                    duration = item.endDate != null ? EnglishToBanglaNumber.DaysToBanglaYears((Convert.ToDateTime(item.endDate).Date - Convert.ToDateTime(item.startDate).Date).Days) : EnglishToBanglaNumber.DaysToBanglaYears((Convert.ToDateTime(date).Date - Convert.ToDateTime(item.startDate).Date).Days)
                });
            }
            return model;
        }




        public async Task<EmployeePostingPlace> GetPostingsById(int id, DateTime date)
        {
            return await _context.employeePostings.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.department).Include(x => x.hrBranch).Where(x => Convert.ToDateTime(x.endDate).Date <= Convert.ToDateTime(date).Date && x.employeeId == id).AsNoTracking().FirstOrDefaultAsync();


        }


        public async Task<IEnumerable<EmployeeWithYearsTransfer>> GetSeniorityBy3YAndAbove(DateTime transferDate, int years)
        {
            var model = new List<EmployeeWithYearsTransfer>();
            var data = await _context.employeeInfos.Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastTransferDate != null && x.activityStatus == 1 && (Convert.ToDateTime(transferDate).Date - Convert.ToDateTime(x.lastTransferDate).Date).Days >= (years * 365) && x.lastDesignationId != null).ToListAsync();

            foreach (var item in data)
            {
                model.Add(new EmployeeWithYearsTransfer
                {
                    employee = item,
                    duration = CalculateYearsByDays((Convert.ToDateTime(transferDate).Date - Convert.ToDateTime(item.lastTransferDate).Date).Days)
                });
            }
            return model;
        }


        public string CalculateYearsByDays(int days)
        {
            int y = days / 365;
            int m = (days - (y * 365)) / 30;

            return y + "Y-" + m + "M";
        }


        public async Task<IEnumerable<EmployeeInfo>> GetGenderWiseEmployee(string gender)
        {
            return await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).OrderBy(x=>Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x=>Convert.ToInt32(x.seniorityLevel)).Where(x => x.gender == gender && x.lastDesignationId !=null).ToListAsync();
        }
        public async Task<Religion> GetReligionById(int id)
        {
            return await _context.religions.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<Religion> GetGenderByName(int id)
        {
            return await _context.religions.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<string> GetDeisignationNameById(int id)
        {
            return await _context.designations.Where(x => x.Id == id).Select(x => x.designationName).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<string> GetDeisignationNameById1(int id)
        {
            return await _context.designations.Where(x => x.Id == id).Select(x => x.designationNameBN).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<string> GetHrBranchNameById(int id)
        {
            return await _context.hrBranches.Where(x => x.Id == id).Select(x => x.branchName).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<string> GetHrBranchNameById1(int id)
        {
            return await _context.hrBranches.Where(x => x.Id == id).Select(x => x.branchNameBn).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<string> GetDeisignationShortNameById(int id)
        {
            return await _context.designations.Where(x => x.Id == id).Select(x => x.shortName).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeeByJoiningDateRange(DateTime? from, DateTime? to)
        {
            return await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.joiningDateGovtService >= from && x.joiningDateGovtService <= to && x.lastDesignationId != null).ToListAsync();
        }
        public async Task<IEnumerable<EmployeeInfo>> GetDegWiseEmpLisr(int Id, int depId)
        {
            return await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).Where(x => x.lastDesignationId == Id && x.departmentId == depId).OrderBy(x => x.seniorityLevel).ToListAsync();
        }
        public async Task<IEnumerable<EmployeeInfo>> GetOfficeWiseEmpLisr(int Id, int OfficeId)
        {
            return await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).Where(x => x.lastDesignationId == Id && x.functionInfoId == OfficeId).OrderBy(x => x.seniorityLevel).ToListAsync();
        }
        public async Task<IEnumerable<EmployeeInfo>> GetDivWiseEmpLisr(int Id, int DivId)
        {
            return await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).Where(x => x.lastDesignationId == Id && x.hrDivisionId == DivId).OrderBy(x => x.seniorityLevel).ToListAsync();
        }
       


        public async Task<int> GetDegWiseEmpLisrCount(int Id, int depId)
        {
            return await _context.employeeInfos.Where(x => x.lastDesignationId == Id && x.departmentId == depId).CountAsync();
        }



        public async Task<IEnumerable<CustomRole>> GetAllRoles()
        {
            return await _context.customRoles.OrderBy(x => x.isDelete).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<EmployeeInfo>> GetEmployeeByConfirmationDateRange(DateTime? from, DateTime? to)
        {
            return await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.dateOfPermanent >= from && x.dateOfPermanent <= to && x.lastDesignationId != null).ToListAsync();
        }
        public async Task<IEnumerable<EmployeeInfo>> GetEmployeeByRetirementDateRange(DateTime? from, DateTime? to)
        {
            return await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.DateOfRetirement >= from && x.DateOfRetirement <= to && x.lastDesignationId != null).ToListAsync();
        }
        public async Task<IEnumerable<EmployeeInfo>> GetEmployeeByResignationDateRange(DateTime? from, DateTime? to)
        {
            return await _context.employeeInfos.Include(x => x.religion).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.branch).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastWorkingDate >= from && x.lastWorkingDate <= to && x.lastDesignationId != null).ToListAsync();
        }
        public async Task<IEnumerable<EmpSuspensionViewModel>> GetEmployeeBySuspensionDateRange(DateTime? fromdate, DateTime? todate)
        {
            var data = await (from e in _context.employeeInfos
                              join s in _context.suspensions on e.Id equals s.employeeId
                              join de in _context.designations
                              on e.lastDesignationId equals de.Id
                              where s.effectiveForm >= fromdate && s.effectiveForm <= todate && e.lastDesignationId != null
                              select new EmpSuspensionViewModel
                              {
                                  employeeInfo = e,
                                  suspension = s,
                                  designation = de
                              }).OrderBy(x => Convert.ToInt32(x.designation.designationCode)).ThenBy(x => Convert.ToInt32(x.employeeInfo.seniorityLevel)).ToListAsync();

            return data;
        }
        public async Task<IEnumerable<Award>> GetAllAwardList()
        {
            return await _context.awards.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<EmployeeInfo>> Degconut(int id) //awardId
        {
            var data = await (from e in _context.employeeInfos
                              join ea in _context.employeeAwards on e.Id equals ea.employeeId
                              //join a in _context.awards 
                              //on ea.awardId equals a.Id
                              join de in _context.designations
                              on e.lastDesignationId equals de.Id
                              where ea.awardId == id
                              select e).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeByAwardId1(int id) //awardId
        {
            var data = await (from e in _context.employeeInfos
                              join ea in _context.employeeAwards on e.Id equals ea.employeeId
                              //join a in _context.awards 
                              //on ea.awardId equals a.Id
                              join de in _context.designations
                              on e.lastDesignationId equals de.Id
                              where ea.awardId == id
                              select e).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignationId != null).ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeeByAwardId(int id) //awardId
        {
            var data = await (from e in _context.employeeInfos
                              join d in _context.employeeAwards
                              on e.Id equals d.employeeId
                              join de in _context.designations
                              on e.lastDesignationId equals de.Id
                              where d.Id == id
                              select new EmpDiplomaViewModel
                              {
                                  employeeInfo = e,
                                  employeeAward = d,
                                  designation = de,
                              }).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeesByPart(string part)
        {
            var data = await (from e in _context.employeeInfos
                              join d in _context.bankingDiplomas
                              on e.Id equals d.employeeId
                              join de in _context.designations
                              on e.lastDesignationId equals de.Id
                              where d.diplomaPart == part
                              select new EmpDiplomaViewModel
                              {
                                  employeeInfo = e,
                                  bankingDiploma = d,
                                  designation = de,
                              }).OrderBy(x => Convert.ToInt32(x.designation.designationCode)).ThenBy(x => Convert.ToInt32(x.employeeInfo.seniorityLevel)).Where(x => x.employeeInfo.activityStatus != 0 && x.employeeInfo.lastDesignationId != null).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<EmpDiplomaVm>> GetAllEmployeesByDiploma(string part) {
            return await _context.empDiplomaVms.FromSql($"GetAllEmployeesByDiploma {part}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeesByBoth()
        {
            var data = await (from e in _context.employeeInfos
                              join d in _context.bankingDiplomas
                              on e.Id equals d.employeeId
                              join de in _context.designations
                              on e.lastDesignationId equals de.Id
                             
                              select new EmpDiplomaViewModel
                              {
                                  employeeInfo = e,
                                  bankingDiploma = d,
                                  designation = de,
                              }).OrderBy(x => Convert.ToInt32(x.designation.designationCode)).ThenBy(x => Convert.ToInt32(x.employeeInfo.seniorityLevel)).Where(x=>x.employeeInfo.lastDesignationId != null).ToListAsync();
            return data;
        }



        public async Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeesByPart()
        {
            var data = await (from e in _context.employeeInfos
                              join d in _context.bankingDiplomas
                              on e.Id equals d.employeeId
                              join de in _context.designations
                              on e.lastDesignationId equals de.Id
                              join r in _context.customRoles
                              on e.customRoleId equals r.Id
                              select new EmpDiplomaViewModel
                              {
                                  employeeInfo = e,
                                  bankingDiploma = d,
                                  designation = de,
                                  customRole = r,
                              }).ToListAsync();
            return data;
        }

        //public async Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeesByid()
        //{
        //    var data = await (from e in _context.employeeInfos
        //                      join d in _context.bankingDiplomas
        //                      on e.Id equals d.employeeId
        //                      join de in _context.designations
        //                      on e.lastDesignationId equals de.Id
        //                      join r in _context.customRoles
        //                      on e.customRoleId equals r.Id
        //                      select new EmpDiplomaViewModel {
        //                          employeeInfo = e,
        //                          bankingDiploma = d,
        //                          designation = de,
        //                          customRole = r,
        //                      }).ToListAsync();
        //    return data;
        //}

        public async Task<int> GetOfficerCountByOfficeId(int id, DateTime date)
        {
            return await _context.employeeInfos.Where(x => x.functionInfoId == id && x.lastDesignation.customRoleId == 1 && Convert.ToDateTime(x.postingDate).Date <= Convert.ToDateTime(date).Date).CountAsync();
        }
        public async Task<int> GetStaffCountByOfficeId(int id, DateTime date)
        {
            return await _context.employeeInfos.Where(x => x.functionInfoId == id && x.lastDesignation.customRoleId == 3 && Convert.ToDateTime(x.postingDate).Date <= Convert.ToDateTime(date).Date).CountAsync();
        }
        public async Task<int> GetTotalOfficerAndStaffCountByOfficeId(int id, DateTime date)
        {
            return await _context.employeeInfos.Where(x => x.functionInfoId == id && (x.lastDesignation.customRoleId == 3 || x.lastDesignation.customRoleId == 1) && Convert.ToDateTime(x.postingDate).Date <= Convert.ToDateTime(date).Date).CountAsync();
        }

        public async Task<int> GetOfficerCountByBranchId(int id, DateTime date)
        {
            return await _context.employeeInfos.Where(x => x.hrBranchId == id && x.lastDesignation.customRoleId == 1 && Convert.ToDateTime(x.postingDate).Date <= Convert.ToDateTime(date).Date).CountAsync();
        }
        public async Task<int> GetStaffCountByBranchId(int id, DateTime date)
        {
            return await _context.employeeInfos.Where(x => x.hrBranchId == id && x.lastDesignation.customRoleId == 3 && Convert.ToDateTime(x.postingDate).Date <= Convert.ToDateTime(date).Date).CountAsync();
        }
        public async Task<int> GetTotalOfficerAndStaffCountByBranchId(int id, DateTime date)
        {
            return await _context.employeeInfos.Where(x => x.hrBranchId == id && (x.lastDesignation.customRoleId == 3 || x.lastDesignation.customRoleId == 1) && Convert.ToDateTime(x.postingDate).Date <= Convert.ToDateTime(date).Date).CountAsync();
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployees()
        {
            var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.department).ToListAsync();
            return data;
        }
       
        public async Task<IEnumerable<EducationManpowerViewModel>> GetHrEducationSummaryData1(string todate)
        {
            var data = new List<EducationManpowerViewModel>();
            var experties = await _context.empExpertises.AsNoTracking().ToListAsync();
            foreach (var item in experties)
            {
                data.Add(new EducationManpowerViewModel
                {
                    title = item.nameEn,
                    count = await _context.employeeInfos.Where(x => x.expertiseId == item.Id && Convert.ToDateTime(x.joiningDateGovtService).Date <= Convert.ToDateTime(todate).Date).CountAsync()
                });
            }
            return data;
        }

        public async Task<IEnumerable<BranchManagerViewModel>> GetBranchManagerByBranchIdDate(int hrBranchId, string fromDate, string toDate)
        {
            var data= await _context.branchManagerVM.FromSql($"sp_GetBranchManagersByDate {hrBranchId},{fromDate},{toDate}").AsNoTracking().ToListAsync();
            return data;
        }
    }
}
