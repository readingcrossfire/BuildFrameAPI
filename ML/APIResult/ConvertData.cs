using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.APIResult
{
    public class ConvertData
    {
        public static bool IsNullColumn(DataRow objRow, string strColumnName)
        {
            if (!objRow.Table.Columns.Contains(strColumnName))
            {
                return true;
            }

            return Convert.IsDBNull(objRow[strColumnName]);
        }

        public static object IsNull(object objValue, object objOtherValue)
        {
            if (objValue == null || Convert.IsDBNull(objValue))
            {
                return objOtherValue;
            }

            return objValue;
        }

        public static Hashtable ConvertSearchParamListToHashtable(List<SearchParams> lstSearchParamsList)
        {
            Hashtable hashtable = new Hashtable();
            foreach (SearchParams lstSearchParams in lstSearchParamsList)
            {
                hashtable.Add(lstSearchParams.SearchKey, lstSearchParams.SearchValue);
            }

            return hashtable;
        }

        public static string ConvertToLowerFirstChar(string strInput)
        {
            if (strInput.Length == 0)
            {
                return strInput;
            }

            return strInput.Substring(0, 1).ToLower() + strInput.Substring(1, strInput.Length - 1);
        }

        public static string ConvertToUnsignString(string strSource)
        {
            if (strSource.Trim().Length == 0)
            {
                return "";
            }

            string text = "á à ả ã ạ Á À Ả Ã Ạ ă ắ ằ ẳ ẵ ặ Ă Ắ Ằ Ẳ Ẵ Ặ â ấ ầ ẩ ẫ ậ Â Ấ Ầ Ẩ Ẫ Ậ đ Đ é è ẻ ẽ ẹ É È Ẻ Ẽ Ẹ ê ế ề ể ễ ệ Ê Ế Ề Ể Ễ Ệ í ì ỉ ĩ ị Í Ì Ỉ Ĩ Ị ó ò ỏ õ ọ Ó Ò Ỏ Õ Ọ ô ố ồ ổ ỗ ộ Ô Ố Ồ Ổ Ỗ Ộ ơ ớ ờ ở ỡ ợ Ơ Ớ Ờ Ở Ỡ Ợ ú ù ủ ũ ụ Ú Ù Ủ Ũ Ụ ư ứ ừ ử ữ ự Ư Ứ Ừ Ử Ữ Ự ý ỳ ỷ ỹ ỵ Ý Ỳ Ỷ Ỹ Ỵ";
            string text2 = "a a a a a A A A A A a a a a a a A A A A A A a a a a a a A A A A A A d d e e e e e E E E E E e e e e e e E E E E E E i i i i i I I I I I o o o o o O O O O O o o o o o o O O O O O O o o o o o o O O O O O O u u u u u U U U U U u u u u u u U U U U U U y y y y y Y Y Y Y Y";
            string[] array = text.Split(" ".ToCharArray());
            string[] array2 = text2.Split(" ".ToCharArray());
            string text3 = strSource;
            for (int i = 0; i < array.Length; i++)
            {
                text3 = text3.Replace(array[i], array2[i]);
            }

            text = "À Á Â Ã Ä Å Æ Ç È É Ê Ë Ì Í Î Ï Ð Ñ Ò Ó Ô Õ Ö Ø Ù Ú Û Ü Ý Þ ß à á â ã ä å æ ç è é ê ë ì í î ï ð ñ ò ó ô õ ö ø ù ú û ü ý þ ÿ";
            text2 = "A A A A A A Æ Ç E E E E I I I I D N O O O O O Ø U U U U Y Þ ß a a a a a a æ ç e e e e i i i i ð n o o o o o ø u u u u y þ y";
            string[] array3 = text.Split(" ".ToCharArray());
            string[] array4 = text2.Split(" ".ToCharArray());
            for (int j = 0; j < array3.Length; j++)
            {
                text3 = text3.Replace(array3[j], array4[j]);
            }

            return text3.Replace("\0", "");
        }
    }
}
