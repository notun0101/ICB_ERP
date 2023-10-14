using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Helpers
{
	public static class EnglishToBanglaNumber
	{
        public static String ConvertToBnEnDate(DateTime date)
        {
            var bnDate = ConvertEnglishNumToBanglaNum(date.Day.ToString());
            var bnYear = ConvertEnglishNumToBanglaNum(date.Year.ToString());
            var enMonth = date.Month;
            var bnMonth = "";
            switch (enMonth)
            {
                case 1:
                    bnMonth = "জানুয়ারী";
                    break;
                case 2:
                    bnMonth = "ফেব্রুয়ারি";
                    break;
                case 3:
                    bnMonth = "মার্চ";
                    break;
                case 4:
                    bnMonth = "এপ্রিল";
                    break;
                case 5:
                    bnMonth = "মে";
                    break;
                case 6:
                    bnMonth = "জুন";
                    break;
                case 7:
                    bnMonth = "জুলাই";
                    break;
                case 8:
                    bnMonth = "আগস্ট";
                    break;
                case 9:
                    bnMonth = "সেপ্টেম্বর";
                    break;
                case 10:
                    bnMonth = "অক্টোবর";
                    break;
                case 11:
                    bnMonth = "নভেম্বর";
                    break;
                case 12:
                    bnMonth = "ডিসেম্বর";
                    break;
                default:
                    break;
            }
            return bnMonth + " " + bnDate + ", " + bnYear;
        }

        public static String ConvertEnglishToBanglaDate(DateTime date)
        {
            //date = new DateTime(2021, 10, 17);
            var bnYear = ConvertEnglishNumToBanglaNum((date.Year - 593).ToString());
            
            int[] bnCode;
            if (date.Year == 2021)
            {
                bnCode = new int[] { 4, 5, 5, 6, 6, 6, 7, 6, 6, 5, 4, 5 };
            }
            else
            {
                bnCode = new int[] { 4, 5, 5, 6, 6, 6, 7, 6, 6, 5, 4, 5 };
            }
            var month = date.Month;
            var startDate = month < 4 ? Convert.ToInt32(bnCode[month + 8]) + 10 : Convert.ToInt32(bnCode[month - 4]) + 10; ;

            var monthName = "";

            if (startDate > date.Day)
            {
                var monthOrder = month - 1 < 4 ? month - 1 + 8 : month - 1 - 4;
                var prevMonthStartDate = month - 1 < 4 ? Convert.ToInt32(bnCode[month - 1 + 8]) + 10 : Convert.ToInt32(bnCode[month - 1 - 4]) + 10;
                var newMonth = date.Month;
                if (date.Month != 1)
                {
                    newMonth = date.Month - 1;
                }
                monthName = monthOrder == 0 ? GetBnMonthNameByIndexOrder(11) : GetBnMonthNameByIndexOrder(monthOrder);
                var acDate = ConvertEnglishNumToBanglaNum((DateTime.DaysInMonth(date.Year, newMonth) - prevMonthStartDate + date.Day + 1).ToString());
                return monthName + " " + acDate + ", " + bnYear;
            }
            else
            {
                var monthOrder = month < 4 ? month + 8 : month - 4;
                monthName = GetBnMonthNameByIndexOrder(monthOrder);
                var acDate = ConvertEnglishNumToBanglaNum((date.Day - startDate + 1).ToString());
                return monthName + " " + acDate + ", " + bnYear;

            }
        }
        public static String ConvertEnglishNumToBanglaNum(string number)
		{
			var en = "0123456789";
			var bn = "০১২৩৪৫৬৭৮৯";

			var bnNum = "";

			if (number != null)
			{
				for (int i = 0; i < number.Length; i++)
				{
					if (en.IndexOf(number[i]) < 0)
					{
						bnNum += number[i];
					}
					else
					{
						var bnNumber = bn[en.IndexOf(number[i])];
						bnNum += bnNumber;
					}

				}
			}
			return bnNum;
		}

        public static String GetBnMonthNameByIndexOrder(int monthOrder)
        {
            var monthName = "";
            switch (monthOrder)
            {
                case 0:
                    monthName = "বৈশাখ";
                    break;
                case 1:
                    monthName = "জ্যৈষ্ঠ";
                    break;
                case 2:
                    monthName = "আষাঢ়";
                    break;
                case 3:
                    monthName = "শ্রাবণ";
                    break;
                case 4:
                    monthName = "ভাদ্র";
                    break;
                case 5:
                    monthName = "আশ্বিন";
                    break;
                case 6:
                    monthName = "কার্তিক";
                    break;
                case 7:
                    monthName = "অগ্রহায়ণ";
                    break;
                case 8:
                    monthName = "পৌষ";
                    break;
                case 9:
                    monthName = "মাঘ";
                    break;
                case 10:
                    monthName = "ফাল্গুন";
                    break;
                case 11:
                    monthName = "চৈত্র";
                    break;
                default:
                    break;
            }
            return monthName;
        }
        public static string DaysToBanglaYears(int days)
        {
            int years = days / 365;
            int months = (days - (years * 365)) / 30;
            int day = (days - ((years * 365) + (months * 30)));
            return ConvertEnglishNumToBanglaNum(years.ToString()) + " বছর " + ConvertEnglishNumToBanglaNum(months.ToString()) + " মাস " + ConvertEnglishNumToBanglaNum(day.ToString()) + " দিন";
        }
        public static string DaysToEnglishYears(int days)
        {
            int years = days / 365;
            int months = (days - (days % 365)) / 30;
            int day = (days - (years * 365) + (months * 30));
            return years + " years " + months + " months " + day + " days";
        }

		public static string EnglishToBanglaDate(DateTime datetime) //dd/MM/yyyy
		{
			var banglaDate = "";

			var year = datetime.Year;
			var month = datetime.Month;
			var date = datetime.Day;
			//var code = "455666655435";

			var code = new List<int>
			{
				4, 5, 5, 6, 6, 6, 6, 5, 5, 4, 3, 5
			};
			var enMonths = new List<int>
			{
				4, 5,6,7,8,9,10,11,12,1,2,3
				//"4","5","6","7","8","9","10","11","12","1","2","3"
			};
			var bnMonths = new List<int>
			{
				1,2,3,4,5,6,7,8,9,10,11,12
				//"1","2","3","4","5","6","7","8","9","10","11","12"
			};
			var bnMonthNames = new List<string>
			{
				"বৈশাখ","জৈষ্ঠ্য","আষাঢ়","শ্রাবণ","ভাদ্র","আশ্বিন","কার্তিক","অগ্রহায়ণ","পৌষ","মাঘ","ফাল্গুন","চৈত্র"
			};

			var enIndex = enMonths.IndexOf(month);
			var bnNum = bnMonths[enIndex];
			var bnMonthName = bnMonthNames[bnNum - 1];

			var bnCode = code[bnNum - 1];
			var bnDateStart = bnCode + 10;

			var today = 0;
			if (date > bnDateStart)
			{
				today = date - bnDateStart;
			}
			else if (date == bnDateStart)
			{
				today = 1;
			}
			else
			{
				today = 30 - (bnDateStart - date);
				bnMonthName = bnMonthNames[bnMonthNames.IndexOf(bnMonthName) - 1];
			}

			var bnYear = year - 593;

			banglaDate = bnMonthName + " " + today + ", " + bnYear;

			return banglaDate;
		}
	}
}
