using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Web;
using System.Configuration;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

namespace Common
{
    public class Security
    {                
        #region ������Ϣ   
        
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="input">ԭʼ��Ϣ</param>      
        /// <param name="encryStr">����ʹ�õ���Կ</param>      
        /// <param name="index">��Կ����</param>
        /// <returns>���ܺ����Ϣ</returns>
        public static string EncryptInfo(string input, string encryStr, int index)
        {            
            StringBuilder tmpstr = new StringBuilder();
            int iRandNum = 0;
            Random rnd = new Random();
            for (int i = 0; i < input.Length; i++)
            {
                tmpstr.Append(input.Substring(i, 1));
                for (int j = 0; j < index * 2; j++)
                {
                    iRandNum = rnd.Next(encryStr.Length - 1);
                    tmpstr.Append(encryStr.Substring(iRandNum, 1));
                }
            }

            return tmpstr.ToString();
        }      


        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="encryInput">������Ϣ</param>      
        /// <param name="encryStr">����ʹ�õ���Կ</param>
        /// <param name="index">��Կ����</param>
        /// <param name="IsEncry">�Ƿ����</param>
        /// <returns>���ܺ����Ϣ</returns>
        public static string DeEncryptInfo(string encryInput, string encryStr, int index, string IsEncry)
        {
            StringBuilder tmpstr = new StringBuilder();
            if (IsEncry.Equals("Y"))
            {
                for (int i = 0; i < encryInput.Length; i++)
                {
                    tmpstr.Append(encryInput.Substring(i, 1));
                    i = i + index * 2;
                }
            }
            else
            {
                tmpstr.Append(encryInput);
            }

            return tmpstr.ToString();
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="input">ԭʼ��Ϣ</param>
        /// <param name="flag">true��ǿ���ܣ�false��ͨ����</param>
        /// <returns>���ܺ����Ϣ</returns>
        public static byte[] EncryptInfo(string input, bool flag)
        {
            byte[] sha1Pwd;
            SHA1 sha1 = SHA1.Create();
            string highCode = "128167213105241091992541172169014025413312010521667211123";
            if (flag)
            {
                sha1Pwd = sha1.ComputeHash(Encoding.Unicode.GetBytes(input + highCode));
            }
            else
            {
                sha1Pwd = sha1.ComputeHash(Encoding.Unicode.GetBytes(input));
            }
            sha1.Clear();

            return sha1Pwd;
        }

        /// <summary>
        /// ���ܺ��ϣֵת��Ϊ�ַ���
        /// </summary>
        /// <param name="hsv">���ܺ�Ĺ�ϣֵ</param>
        /// <returns>���ܺ�Ĺ�ϣֵ���ַ���</returns>
        public static string EncryptInfoToString(byte[] hsv)
        {
            string hsvStr = string.Empty;
            for (int i = 0; i < hsv.Length; i++)
            {
                hsvStr = hsvStr + hsv[i].ToString();
                
            }
            return hsvStr;
        }    
        
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="pwdchars">���ɵ�������봮����ʹ����Щ�ַ�</param>
        /// <param name="pwdlen">���ɵ�������봮�ĳ���</param>
        /// <returns>�����������</returns>
        public static string MakeLightPassword(string pwdchars, int pwdlen)
        {
            StringBuilder tmpstr = new StringBuilder();
            int iRandNum;
            Random rnd = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                iRandNum = rnd.Next(pwdchars.Length);
                tmpstr.Append(pwdchars[iRandNum]);
            }
            return tmpstr.ToString();
        }

        #endregion

        #region �Ƚ������ֽ�����
        /// <summary>
        /// �Ƚ������ֽ�����
        /// </summary>
        /// <param name="array1">����1</param>
        /// <param name="array2">����2</param>
        /// <returns>�Ƿ����</returns>
        public static bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }
        #endregion

        #region ʹ��������ʽ����û�����
        /// <summary>
        /// ʹ��������ʽ����û�����
        /// </summary>
        /// <param name="reg">ʹ�õ�������ʽ</param>
        /// <param name="input">�û�����</param>
        /// <returns>�Ƿ�Ϸ�</returns>
        public static bool CheckInput(string reg, string input)
        {
            //������ȴ����㣬���ʾ����Ҫ��֤�����ݣ�����ͱ�ʾû����Ҫ��֤������
            if (input != null && input.Length != 0)
            {
                System.Text.RegularExpressions.Regex ex = new System.Text.RegularExpressions.Regex(reg);
                return ex.IsMatch(input);
            }
            return true;
        }
        #endregion

        #region ʹ��������ʽɾ���û������еĽű�����
        /// <summary>
        /// ʹ��������ʽɾ���û������еĽű�����
        /// </summary>
        /// <param name="text">�û�����</param>
        /// <returns>�������ı�</returns>
        public static string ClearScript(string text)
        {
            string pattern;

            if (text.Length == 0)
                return text;

            pattern = @"(?i)<script([^>])*>(\w|\W)*</script([^>])*>";
            text = System.Text.RegularExpressions.Regex.Replace(text, pattern, String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            pattern = @"<script([^>])*>";
            text = System.Text.RegularExpressions.Regex.Replace(text, pattern, String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            pattern = @"</script>";
            text = System.Text.RegularExpressions.Regex.Replace(text, pattern, String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return text;
        }
        #endregion

        #region AES���ܽ���

        /// <summary>
        /// Rijndael����
        /// </summary>
        /// <param name="data">��Ҫ���ܵ��ַ�����</param>
        /// <param name="key">�ܳף����ȿ���Ϊ��64λ(byte[8])��128λ(byte[16])��192λ(byte[24])��256λ(byte[32])</param>
        /// <param name="iv">iv����������Ϊ128��byte[16]��</param>
        /// <returns>���ܺ���ַ�</returns>
        public static string EnRijndael(string data, byte[] key, byte[] iv)
        {
            Rijndael rijndael = Rijndael.Create();
            byte[] tmp = null;
            ICryptoTransform encryptor = rijndael.CreateEncryptor(key, iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(data);
                    writer.Flush();
                }
                tmp = ms.ToArray();
            }
            return Convert.ToBase64String(tmp);
        }

        /// <summary>
        /// Rijndael����
        /// </summary>
        /// <param name="data">��Ҫ���ܵ��ַ�����</param>
        /// <param name="key">�ܳף����ȿ���Ϊ��64λ(byte[8])��128λ(byte[16])��192λ(byte[24])��256λ(byte[32])</param>
        /// <param name="iv">iv����������Ϊ128��byte[16]��</param>
        /// <returns>���ܺ���ַ�</returns>
        public static string DeRijndael(string data, byte[] key, byte[] iv)
        {
            string result = string.Empty;
            Rijndael rijndael = Rijndael.Create();
            ICryptoTransform decryptor = rijndael.CreateDecryptor(key, iv);

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(data)))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    StreamReader reader = new StreamReader(cs);
                    result = reader.ReadLine();
                    reader.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// AES����
        /// </summary>
        /// <param name="data">�����ܵ��ַ�����</param>
        /// <param name="key">�ܳף����ȿ���Ϊ��128λ(byte[16])��192λ(byte[24])��256λ(byte[32])</param>
        /// <param name="iv">iv���������ȱ���Ϊ128λ��byte[16]��</param>
        /// <returns>���ܺ���ַ�</returns>
        public static string EnAES(string data, byte[] key, byte[] iv)
        {
            Aes aes = Aes.Create();
            byte[] tmp = null;

            ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                }
                tmp = ms.ToArray();
            }
            return Convert.ToBase64String(tmp);
        }

        /// <summary>
        /// AES����
        /// </summary>
        /// <param name="data">�����ܵ��ַ�����</param>
        /// <param name="key">�ܳף����ȿ���Ϊ��128λ(byte[16])��192λ(byte[24])��256λ(byte[32])</param>
        /// <param name="iv">iv���������ȱ���Ϊ128λ��byte[16]��</param>
        /// <returns>���ܺ���ַ�</returns>
        public static string DeAES(string data, byte[] key, byte[] iv)
        {
            string result = string.Empty;
            Aes aes = Aes.Create();

            ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(data)))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    StreamReader reader = new StreamReader(cs);
                    result = reader.ReadLine();
                    reader.Close();
                }
            }
            aes.Clear();
            return result;
        }

        /// <summary>
        /// ���ɳ����ַ���1-256
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string CreateKey(int num)
        {
            //��ѡ�ַ�
            string securitycode = "Drn1eAgPMvdUiD63aV9R3dmF8Tb9C1mWpA9L5O34S43wf8V9o13sImOms3iYPdWzq7dj17FTZm5agaC4tdsm0mdeBLp6hMtdD2v49xl43kDkyBuheIFMdFeeDX0rukNX1kv664c8Gd0PuugMk8ds104wxitn91ZNi1am9MHTRzH4Ss54rP8m5T4I9ngoQ2P5NWmru4ImP012t0LxtfP1zR44n4evs4nN4a2dmJnmcbk7c60j241z1WWt8m9uOGieM967bb1a";
            char[] scodes = securitycode.ToCharArray();

            Random rand = new Random();
            StringBuilder strb = new StringBuilder();

            for (int i = 0; i < num; i++)
            {
                int tempcharindex = rand.Next(1, scodes.Length);
                strb.Append(scodes[tempcharindex].ToString());
            }
            return strb.ToString();
        }

        /// <summary>
        /// �����ַ�����byte����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] CreateKeyByte(string str)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(str);
        }
        #endregion

        public static byte[] MakeVerifyCode()
        {
            int codeW = 80;
            int codeH = 30;
            int fontSize = 16;
            string chkCode = string.Empty;
            //��ɫ�б�������֤�롢���ߡ���� 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //�����б�������֤�� 
            string[] font = { "Times New Roman" };
            //��֤����ַ�����ȥ����һЩ���׻������ַ� 
            char[] character = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //������֤���ַ��� 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            //д��Session����֤����� TODO ����md5��������
            //Utility.WriteSession("hqd007_session_verifycode", Md5.md5(chkCode.ToLower(), 16));
            Utility.WriteSession("hqd007sessionverifycode", chkCode.ToLower());
            //��������
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //������ 
            for (int i = 0; i < 3; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //����֤���ַ��� 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18, (float)0);
            }
            //����֤��ͼƬд���ڴ������������� "image/Png" ��ʽ��� 
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }

        /// <summary>
        /// ��ʽ���ı�����ֹSQLע�룩
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Formatstr(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex10 = new System.Text.RegularExpressions.Regex(@"select", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex11 = new System.Text.RegularExpressions.Regex(@"update", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex12 = new System.Text.RegularExpressions.Regex(@"delete", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //����<script></script>���
            html = regex2.Replace(html, ""); //����href=javascript: (<A>) ����
            html = regex3.Replace(html, " _disibledevent="); //���������ؼ���on...�¼�
            html = regex4.Replace(html, ""); //����iframe
            html = regex10.Replace(html, "s_elect");
            html = regex11.Replace(html, "u_pudate");
            html = regex12.Replace(html, "d_elete");
            html = html.Replace("'", "��");
            html = html.Replace("&nbsp;", " ");
            return html;
        }

        /// <summary>
        /// ȥ��HTML���
        /// </summary>
        /// <param name="Htmlstring">����HTML��Դ�� </param>
        /// <returns>�Ѿ�ȥ���������</returns>
        public static string ReplaceHtml(string Htmlstring)
        {
            //ɾ���ű�
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //ɾ��HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&hellip;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&mdash;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;

        }

    }


    /// <summary>
    /// �����࣬��Ҫ���ڼ����û����롣
    /// </summary>
    public class EncryptSingleton
    {
        private static EncryptSingleton encryptSingleton = null;
        private bool isUseEncrypt = true;  //�Ƿ�ʹ�ü���

        SymmetricAlgorithm sAlgorithm;
        const string IV = "vmhcLw99CmQ=";
        const string KEY = "8b+rsILdpPY=";

        /// <summary>
        /// ���캯����
        /// </summary>
        private EncryptSingleton()
        {
            sAlgorithm = new DESCryptoServiceProvider();

        }
        /// <summary>
        /// �Ƿ���ܡ�
        /// </summary>
        public bool UseEncrypt
        {
            set
            {
                isUseEncrypt = value;
            }
        }
        /// <summary>
        /// �õ�Ψһʵ����
        /// </summary>
        /// <returns>EncryptSingletonʵ����</returns>
        public static EncryptSingleton GetInstance()
        {

            if (encryptSingleton == null)
                encryptSingleton = new EncryptSingleton();

            return encryptSingleton;
        }

        /// <summary>
        /// �����ַ�����
        /// </summary>
        /// <param name="txt">Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ�����</returns>
        public string GetEncryptedString(string txt)
        {
            string str;
            if (isUseEncrypt)
                str = HashTextMD5(txt);
            else
                str = txt;

            if (str.Length > 8)
                return str.Substring(0, 8);
            else
                return str;

        }
        /// <summary>
        /// ʹ��md5���ܡ�
        /// </summary>
        /// <param name="TextToHash">Ҫ���ܵ��ַ�����</param>
        /// <returns>���ܽ����</returns>
        private string HashTextMD5(string textToHash)
        {

            MD5CryptoServiceProvider md5;
            Byte[] bytValue;
            Byte[] bytHash;

            //�����µļ��ܷ����ṩ�������
            md5 = new MD5CryptoServiceProvider();

            //��ԭʼ�ַ���ת�����ֽ�����
            bytValue = System.Text.Encoding.UTF8.GetBytes(textToHash);

            // ����ɢ�У�������һ���ֽ�����
            bytHash = md5.ComputeHash(bytValue);

            md5.Clear();

            //����ɢ��ֵ�� Base64 �����ַ���
            return Convert.ToBase64String(bytHash);
        }

        /// <summary>
        /// �������롣
        /// </summary>
        /// <param name="pwd">����</param>
        /// <returns>���ܺ������</returns>
        public string EncryptPWD(string pwd)
        {
            return EncryptSingleton.GetInstance().GetEncryptedString(pwd);
        }

        /// <summary>
        /// �����ַ�����
        /// </summary>
        /// <param name="Value">Ҫ���ܵ��ַ�����</param>
        /// <returns>���ܺ���ַ�����</returns>
        public string EncryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;


            //������ʱ���ܶ�
            //mCSP.GenerateKey();
            //mCSP.GenerateIV();
            //Console.WriteLine(Convert.ToBase64String(  mCSP.IV));
            //Console.WriteLine(Convert.ToBase64String(mCSP.Key));

            sAlgorithm.IV = Convert.FromBase64String(IV);
            sAlgorithm.Key = Convert.FromBase64String(KEY);

            ct = sAlgorithm.CreateEncryptor(sAlgorithm.Key, sAlgorithm.IV);

            byt = Encoding.UTF8.GetBytes(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// �����ַ�����
        /// </summary>
        /// <param name="Value">Ҫ���ܵ��ַ�����</param>
        /// <returns>Դ�ַ�����</returns>
        public string DecryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            sAlgorithm.IV = Convert.FromBase64String(IV);
            sAlgorithm.Key = Convert.FromBase64String(KEY);

            ct = sAlgorithm.CreateDecryptor(sAlgorithm.Key, sAlgorithm.IV);

            byt = Convert.FromBase64String(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

    }
}
