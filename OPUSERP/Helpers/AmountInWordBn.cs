using System;

namespace OPUSERP.Helpers
{
    public static class AmountInWordBn
    {
        public static String ConvertToWordsBn(String numb)
        {
            numb = Convert.ToDecimal(numb).ToString("0.00");
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            //String endStr = "টাকা মাত্র";
            String endStr = "";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "এবং";// just to separate whole numbers from points/cents  
                        endStr = "পয়সা " + endStr;//Cents  
                        pointStr = ConvertDecimalsBn(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumberBn(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val.Trim();
        }

        public static String ConvertToWordsBangla(String numb)
         {
            numb = Convert.ToDecimal(numb).ToString("0.00");
            String val = "", 
            wholeNo = numb,
            points = "",
            andStr = "",
            pointStr = "";
            String endStr = "";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "এবং";// just to separate whole numbers from points/cents  
                        endStr = "পয়সা " + endStr;//Cents  
                        pointStr = ConvertDecimalsBn(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumberBn(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        public static String ConvertWholeNumberBn(String Number)
        {

            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = Number.StartsWith("0");

                    //Escape Leading Zero By jkm
                    Number = Number.TrimStart(new Char[] { '0' });

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://onesBn' range

                            word = onesBn(Number);
                            isDone = true;
                            break;
                        case 2://tensBn' range
                            word = tensBn(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = "শত ";
                            break;
                        case 4://thousands' range
                        case 5:
                            pos = (numDigits % 4) + 1;
                            place = " হাজার ";
                            break;
                        case 6:
                        case 7:
                            pos = (numDigits % 6) + 1;
                            place = " লক্ষ ";
                            break;
                        case 8://millions' range
                        case 9:
                            pos = (numDigits % 8) + 1;
                            place = " কোটি ";
                            break;
                        case 10://Billions's range
                        case 11:
                        case 12:
                            pos = (numDigits % 10) + 1;
                            place = " বিলিয়ন ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumberBn(Number.Substring(0, pos)) + place + ConvertWholeNumberBn(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumberBn(Number.Substring(0, pos)) + ConvertWholeNumberBn(Number.Substring(pos));
                        }

                        //check for trailing zeros
                        //if (beginsZero) word = " and " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
                //else if (dblAmt==0)
                //{
                //    word = onesBn(Number);
                //}
               
            }
            catch { }
            return word.Trim();
        }

        public static String tensBn(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "দশ";
                    break;
                case 11:
                    name = "এগারো";
                    break;
                case 12:
                    name = "বার";
                    break;
                case 13:
                    name = "তের";
                    break;
                case 14:
                    name = "চৌদ্দ";
                    break;
                case 15:
                    name = "পনের";
                    break;
                case 16:
                    name = "ষোল";
                    break;
                case 17:
                    name = "সতের";
                    break;
                case 18:
                    name = "আঠারো";
                    break;
                case 19:
                    name = "ঊনিশ";
                    break;
                case 20:
                    name = "বিশ";
                    break;
                case 21:
                    name = "একুশ";
                    break;
                case 22:
                    name = "বাইশ";
                    break;
                case 23:
                    name = "তেইশ";
                    break;
                case 24:
                    name = "চব্বিশ";
                    break;
                case 25:
                    name = "পঁচিশ";
                    break;
                case 26:
                    name = "ছাব্বিশ";
                    break;
                case 27:
                    name = "সাতাশ";
                    break;
                case 28:
                    name = "আটাশ";
                    break;
                case 29:
                    name = "ঊনত্রিশ";
                    break;
                case 30:
                    name = "ত্রিশ";
                    break;
                case 31:
                    name = "একত্রিশ";
                    break;
                case 32:
                    name = "বত্রিশ";
                    break;
                case 33:
                    name = "তেত্রিশ";
                    break;
                case 34:
                    name = "চৌত্রিশ";
                    break;
                case 35:
                    name = "পঁয়ত্রিশ";
                    break;
                case 36:
                    name = "ছত্রিশ";
                    break;
                case 37:
                    name = "সাইত্রিশ";
                    break;
                case 38:
                    name = "আটত্রিশ";
                    break;
                case 39:
                    name = "ঊনচল্লিশ";
                    break;
                case 40:
                    name = "চল্লিশ";
                    break;
                case 41:
                    name = "একচল্লিশ";
                    break;
                case 42:
                    name = "বিয়াল্লিশ";
                    break;
                case 43:
                    name = "তেতাল্লিশ";
                    break;
                case 44:
                    name = "চুয়াল্লিশ";
                    break;
                case 45:
                    name = "পঁয়তাল্লিশ";
                    break;
                case 46:
                    name = "ছেচল্লিশ";
                    break;
                case 47:
                    name = "সাতচল্লিশ";
                    break;
                case 48:
                    name = "আটচল্লিশ";
                    break;
                case 49:
                    name = "ঊনপঞ্চাশ";
                    break;
                case 50:
                    name = "পঞ্চাশ";
                    break;
                case 51:
                    name = "একান্ন";
                    break;
                case 52:
                    name = "বাহান্ন";
                    break;
                case 53:
                    name = "তেপ্পান্ন";
                    break;
                case 54:
                    name = "চুয়ান্ন";
                    break;
                case 55:
                    name = "পঞ্চান্ন";
                    break;
                case 56:
                    name = "ছাপ্পান্ন";
                    break;
                case 57:
                    name = "সাতান্ন";
                    break;
                case 58:
                    name = "আটান্ন";
                    break;
                case 59:
                    name = "ঊনষাট";
                    break;
                case 60:
                    name = "ষাট";
                    break;
                case 61:
                    name = "একষট্টি";
                    break;
                case 62:
                    name = "বাষট্টি";
                    break;
                case 63:
                    name = "তেষট্টি";
                    break;
                case 64:
                    name = "চৌষট্টি";
                    break;
                case 65:
                    name = "পঁয়ষট্টি";
                    break;
                case 66:
                    name = "ছেষট্টি";
                    break;
                case 67:
                    name = "সাতষট্টি";
                    break;
                case 68:
                    name = "আটষট্টি";
                    break;
                case 69:
                    name = "ঊনসত্তর";
                    break;
                case 70:
                    name = "সত্তর";
                    break;
                case 71:
                    name = "একাত্তর";
                    break;
                case 72:
                    name = "বাহাত্তর";
                    break;
                case 73:
                    name = "তিয়াত্তর";
                    break;
                case 74:
                    name = "চুয়াত্তর";
                    break;
                case 75:
                    name = "পঁচাত্তর";
                    break;
                case 76:
                    name = "ছিয়াত্তর";
                    break;
                case 77:
                    name = "সাতাত্তর";
                    break;
                case 78:
                    name = "আটাত্তর";
                    break;
                case 79:
                    name = "ঊনআশি";
                    break;
                case 80:
                    name = "আশি";
                    break;
                case 81:
                    name = "একাশি";
                    break;
                case 82:
                    name = "বিরাআশি";
                    break;
                case 83:
                    name = "তিরাআশি";
                    break;
                case 84:
                    name = "চুরাআশি";
                    break;
                case 85:
                    name = "পঁচাশি";
                    break;
                case 86:
                    name = "ছিয়াশি";
                    break;
                case 87:
                    name = "সাতাশি";
                    break;
                case 88:
                    name = "আটাশি";
                    break;
                case 89:
                    name = "ঊননব্বই";
                    break;
                case 90:
                    name = "নব্বই";
                    break;
                case 91:
                    name = "একানব্বই";
                    break;
                case 92:
                    name = "বিরানব্বই";
                    break;
                case 93:
                    name = "তিরানব্বই";
                    break;
                case 94:
                    name = "চুরানব্বই";
                    break;
                case 95:
                    name = "পঁচানব্বই";
                    break;
                case 96:
                    name = "ছিয়ানব্বই";
                    break;
                case 97:
                    name = "সাতানব্বই";
                    break;
                case 98:
                    name = "আটানব্বই";
                    break;
                case 99:
                    name = "নিরানব্বই";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tensBn(Number.Substring(0, 1) + "0") + " " + onesBn(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        public static String onesBn(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {
				case 0:
					name = "শূন্য";
					break;
				case 1:
                    name = "এক";
                    break;
                case 2:
                    name = "দুই";
                    break;
                case 3:
                    name = "তিন";
                    break;
                case 4:
                    name = "চার";
                    break;
                case 5:
                    name = "পাঁচ";
                    break;
                case 6:
                    name = "ছয়";
                    break;
                case 7:
                    name = "সাত";
                    break;
                case 8:
                    name = "আট";
                    break;
                case 9:
                    name = "নয়";
                    break;
            }
            return name;
        }

        public static String ConvertDecimalsBn(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "শূন্য";
                }
                else
                {
                    engOne = onesBn(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
		public static String ConvertEnToBnNumber(string number)
		{
			var data = number.Replace('0', '০')
							.Replace('1', '১')
							.Replace('2', '২')
							.Replace('3', '৩')
							.Replace('4', '৪')
							.Replace('5', '৫')
							.Replace('6', '৬')
							.Replace('7', '৭')
							.Replace('8', '৮')
							.Replace('9', '৯')
							.Replace('/', '/')
							.Replace('-', '-');
			return data;
		}
    }
}
