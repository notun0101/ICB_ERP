using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
    public class TrainingWithAttendanceDetails
    {
        public IEnumerable<ResourcePerson> trainers { get; set; }
        public IEnumerable<TrainingInfoNew> trainingInfoNews { get; set; }

        public int trainingId { get; set; }
		public int[] traineeId { get; set; }
		public int[] isPresent { get; set; }
		public int[] isPresentStatus { get; set; }
		public int[] marks { get; set; }

		public TrainingDetails trainingDetails { get; set; }
		public TrainingWithTrainees trainingWithTrainees { get; set; }
        public IEnumerable<TrainingSubject> trainingSubjects { get; set; }
        public IEnumerable<CourseTitle> courseTitles { get; set; }
        public IEnumerable<TrainingInfoNew> AlltrainingInfoNews { get; set; }
        public IEnumerable<EnrolledTrainee> AllEnrollTrainee { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfoList { get; set; }


	}
    public class TrainingDetails
    {
        public TrainingInfoNew trainingInfoNew { get; set; }
        public IEnumerable<TrainingWithMarks> enrolledTrainees { get; set; }
    }
    public class TrainingWithMarks
    {
        public EnrolledTrainee enrolledTrainee { get; set; }
        public TrainingAttendance attendance { get; set; }
        public TrainingMarks trainingMarks { get; set; }
    }
	public class TrainingWithTrainees
    {
        public TrainingInfoNew training { get; set; }
        public IEnumerable<EnrolledTrainee> trainees { get; set; }
    }
}
