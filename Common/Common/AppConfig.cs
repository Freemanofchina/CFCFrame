using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Web.Configuration;

namespace Common
{
    /// <summary>
    /// ϵͳ�����ļ���ȡ���ò������ࡣ
    /// </summary>
    public class APPConfig
    {
        private static APPConfig appConfig = null;
        Configuration config = null;

        /// <summary>
        /// ˽�й��캯����
        /// </summary>
        private APPConfig()
        {
            SetConfig();
        }

        void SetConfig()
        {
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch
            {
                config = WebConfigurationManager.OpenWebConfiguration("~");
            }
        }
        public object GetSectionValue(string SectionName)
        {

            return ConfigurationManager.GetSection(SectionName);
        }
        /// <summary>
        /// �õ�һ��Ӧ�����á�
        /// </summary>
        /// <returns>Ӧ������ʵ��</returns>
        public static APPConfig GetAPPConfig()
        {
            if (appConfig == null)
            {
                appConfig = new APPConfig();
            }

            return appConfig;
        }

        /// <summary>
        /// �������õļ������õ���Ӧ��ֵ��
        /// </summary>
        /// <returns>ֵ</returns>
        /// <param name="KeyString">����</param>
        public string GetConfigValue(string KeyString)
        {
            string strVal = null;

            try
            {                
                strVal = config.AppSettings.Settings[KeyString].Value;
            }

            catch (Exception e)
            {
                throw new Exception("�������ļ��ж�ȡ" + KeyString + "����", e);
            }
            return strVal;
        }

        /// <summary>
        /// �������õļ������õ���Ӧ��ֵ��
        /// </summary>
        /// <returns>ֵ</returns>
        /// <param name="KeyString">����</param>
        /// <param name="DefaultValue">���û���ҵ���ֵ�����ô�ȱʡֵ��</param>
        public string GetConfigValue(string KeyString, string DefaultValue)
        {
            string strVal = null;
            if (config.AppSettings.Settings[KeyString] == null)
                return DefaultValue;

            try
            {
                strVal = config.AppSettings.Settings[KeyString].Value;
                if (strVal == null)
                    return DefaultValue;
            }
            catch
            {
                strVal = DefaultValue;
            }
            return strVal;
        }


        public void Save()
        {
            //config.Save();
        }

        /// <summary>
        /// ����ĳSection�������ļ���
        /// </summary>
        /// <param name="xmlNodes"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public bool SetConfigSection(List<XmlElement> xmlNodes, string sectionName)
        {

            //Ϊ�����ļ�����һ��FileInfo����
            System.IO.FileInfo FileInfo = new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            //�������ļ�����XML DOM
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();

            xmlDocument.Load(FileInfo.FullName);

            //�ҵ���ȷ�Ľڵ㲢������ֵ��Ϊ�µ�
            try
            {
                xmlDocument.SelectSingleNode("//" + sectionName).RemoveAll(); //ɾ�����н��

                foreach (XmlElement node in xmlNodes)
                {
                    XmlElement element = xmlDocument.CreateElement(sectionName);

                    foreach (XmlAttribute att in node.Attributes)
                    {
                        element.SetAttribute(att.Name, att.Value);

                    }

                    xmlDocument.SelectSingleNode("//" + sectionName).AppendChild(element);
                }
            }
            catch (Exception err)
            {
                throw new Exception("��������" + sectionName + "����!", err);
            }

            //�����޸Ĺ��������ļ�
            xmlDocument.Save(FileInfo.FullName);

            SetConfig();

            return true;
        }
        /// <summary>
        /// �������á�
        /// </summary>
        /// <param name="KeyString">����</param>
        /// <param name="KeyValue">ֵ</param>
        /// <returns></returns>
        public bool SetConfigValue(string KeyString, object KeyValue)
        {
            //Ϊ�����ļ�����һ��FileInfo����
            System.IO.FileInfo FileInfo = new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            //�������ļ�����XML DOM
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();

            xmlDocument.Load(FileInfo.FullName);

            //�ҵ���ȷ�Ľڵ㲢������ֵ��Ϊ�µ�
            try
            {
                bool isModified = false;
                foreach (System.Collections.IEnumerable ie in xmlDocument.GetElementsByTagName("appSettings"))
                {
                    System.Xml.XmlNode node = ie as System.Xml.XmlNode;

                    foreach (System.Xml.XmlNode Node in node.ChildNodes)
                    {
                        if (Node.Name == "add")
                        {
                            if (Node.Attributes.GetNamedItem("key").Value == KeyString)
                            {
                                Node.Attributes.GetNamedItem("value").Value = KeyValue.ToString();
                                isModified = true;
                                break;
                            }
                        }
                    }
                    if (!isModified)
                    {
                        //���һ�����
                        System.Xml.XmlElement xmlE = xmlDocument.CreateElement("add");
                        xmlE.SetAttribute("key", KeyString);
                        xmlE.SetAttribute("value", KeyValue.ToString());
                        xmlDocument.SelectSingleNode("//appSettings").AppendChild(xmlE);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("��������" + KeyString + "����!", err);
            }

            //�����޸Ĺ��������ļ�
            xmlDocument.Save(FileInfo.FullName);
            SetConfig();

            return true;
        }

    }

}
