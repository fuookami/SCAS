using System;
using System.Collections.Generic;
using System.Text;

namespace SSUtils
{
    public class ChineseNumber
    {
        public static String ToChineseNumber(UInt64 num)
        {
            String[] intArr = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", };
            String[] strArr = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", };
            String[] Chinese = { "", "十", "百", "千", "万", "十", "百", "千", "亿" };
            char[] tmpArr = num.ToString().ToCharArray();
            String tmpVal = "";
            for (int i = 0; i < tmpArr.Length; i++)
            {
                tmpVal += strArr[tmpArr[i] - 48];
                tmpVal += Chinese[tmpArr.Length - 1 - i];
            }
            return tmpVal;

        }
    }
}
