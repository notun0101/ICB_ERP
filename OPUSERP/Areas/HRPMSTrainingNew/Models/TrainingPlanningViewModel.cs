using OPUSERP.Areas.HRPMSTrainingNew.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
    public class TrainingPlanningViewModel
    {
        public int? planningId { get; set; }

        public string course { get; set; }

        public string courseObjective { get; set; }

        public string amount { get; set; }

        public DateTime? planeStartDate { get; set; }

        public DateTime? planeEndDate { get; set; }

        public string year { get; set; }

        public int participant { get; set; }

        public int trainingType { get; set; }

        public int? employeeType { get; set; }
        public int[] employeeTypeMultiple { get; set; }

        public string budget { get; set; }

        public string remark { get; set; }

        public int? country { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string durationMinutes { get; set; }

        public string location { get; set; }
        public int? CountTotalTrainingInfo { get; set; }
        public string nameEN { get; set; }

        public int? CoOrdinatorId { get; set; }

        public decimal? coOrdinatorPayment { get; set; }
        //TrainingPerticipants
        public int TrainingPerticipanId { get; set; }
		public int? Id { get; set; } 
		public int? trainingId { get; set; }
        public int? traineeId { get; set; }
        public int? trainerId { get; set; }
        public int? subjectId { get; set; }
        public int? trainingInstituteId { get; set; }
        public int? attendance { get; set; } //0=absent, 1=present
        public int? marking { get; set; }
		public string resourcePersonIdList { get; set; }
		public string coordinatorIdList { get; set; }
		public decimal? receivedMoney { get; set; }
		public decimal? taxPercentage { get; set; }
		public decimal? taxAmount { get; set; }
		public int? session { get; set; }
		public DateTime? effectiveDate { get; set; }
		public int? status { get; set; }
		public int? isPaid { get; set; }
		public int? employeeInfoId { get; set; }


		public TrainingPlanningLn fLang { get; set; }
        public IEnumerable<EmployeeType> employeeTypes { get; set; }
        public IEnumerable<EmployeeInfo> trainee { get; set; }
        public IEnumerable<TrainingPerticipants> trainingPerticipants { get; set; }
        public IEnumerable<TrainingInfoNew> trainingInfoNews { get; set; }
        public IEnumerable<CourseTitle> courseTitles { get; set; }
        public IEnumerable<Year> years { get; set; }
        public IEnumerable<Country> countries { get; set; }
        public IEnumerable<TrainingInstitute> trainingInstitutes { get; set; }
        public IEnumerable<TrainingSubject> trainingSubjects { get; set; }
        public IEnumerable<ResourcePerson> resourcePeople { get; set; }
		public IEnumerable<TrainingHonorarium> trainingHonorariums { get; set; }
		public IEnumerable<TrainingHonorariumViewModel> trainingHonorariumsList { get; set; }
		public IEnumerable<SourceOfFund> sourceOfFundList { get; set; }
		public IEnumerable<CourseType> courseTypeList { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfoList { get; set; } 


		public int? instituteId { get; set; }
        public string subjects { get; set; }
        public string employeeID { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
		public int? sourceOfFundId { get; set; }
		public int? courseTypeId { get; set; }
		public TrainingInfoNew trainingInfoNewsSingle { get; set; }


	}
}
