using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class RegExpress
    {
        /// <summary>
        /// ��ĸ������
        /// </summary>
        public const string charactersAndNumber = "^[0-9a-zA-Z]";

        /// <summary>
        /// ���Ҹ�ʽ
        /// </summary>
        public const string currencys = "^-?\\d+(.\\d{2})?$";
        /// <summary>
        /// ����
        /// </summary>
        public const string numbers = "^[0-9]";

        /// <summary>
        /// ��ĸ
        /// </summary>
        public const string characters = "^[a-zA-Z]";

        /// <summary>
        /// email��ʽ
        /// </summary>
        public const string emailReg = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

        /// <summary>
        /// ��ҳ��ַ��ʽ
        /// </summary>
        public const string homePage = "http://([\\w-]+\\.)+[\\w-]+(/[\\w-./?%&=]*)?";

        /// <summary>
        /// �����ַ�
        /// </summary>
        public const string allCharacter = "^[0-9a-zA-Z\u4e00-\u9fa5]";

        /// <summary>
        /// ����(yyyy_MM_dd)
        /// </summary>
        public const string yyyy_MM_dd = "^\\d{4}-\\d{2}-\\d{2}$";

        /// <summary>
        /// ����
        /// </summary>
        public const string zNum = "(^\\+?|^\\d?)\\d*\\.?\\d+$";
 
        /// <summary>
        /// ����
        /// </summary>
        public const string negative = "^-\\d*\\.?\\d+$";


        /// <summary>
        /// ����
        /// </summary>
        public const string integer = "(^-?|^\\+?|\\d)\\d+$";
        
        /// <summary>
        /// ������
        /// </summary>
        public const string floats = "(^-?|^\\+?|^\\d?)\\d*\\.\\d+$";
        
    }
}
