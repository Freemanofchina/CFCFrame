using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;

namespace Common
{
    public class ClassBuilder
    {
        /// <summary>
        /// ����������������
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static object CreateObject(string className)
        {
            object instance = null ;

            string[] s = className.Split(',');
            if (s.Length >= 2)
            {
                string strAssemblyName = s[1];
                if (strAssemblyName.IndexOf(':') < 0)   //������·��
                {
                    string path = "";
                    if (System.Environment.CurrentDirectory + "\\" == AppDomain.CurrentDomain.BaseDirectory)//WindowsӦ�ó��������
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory;
                    }                    
                    else
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + "bin\\"; //֧��web����
                    }
                    strAssemblyName = path  + s[1];
                }

                if (File.Exists(strAssemblyName))
                {
                    Assembly assembly = GetAssemblyFromAppDomain(strAssemblyName);
                    if (assembly == null)
                        assembly = Assembly.LoadFrom(strAssemblyName);

                    instance = assembly.CreateInstance(s[0]);
                }
                else
                {
                    throw new InvalidDataException("�����ļ�����ȷ���Ҳ�����̬���ļ�:" + strAssemblyName + " ��");
                }
            }
            else
            {
                instance = Assembly.GetExecutingAssembly().CreateInstance(className);
                if (instance == null)
                {
                    instance = Assembly.GetCallingAssembly().CreateInstance(className);
                }
                if (instance == null)
                {
                    IEnumerator ie = AppDomain.CurrentDomain.GetAssemblies().GetEnumerator();
                    while (ie.MoveNext())
                    {
                        Assembly ab = ie.Current as Assembly;
                        if (ab != null)
                        {
                            instance = ab.CreateInstance(className);
                            if (instance != null)
                                break;
                        }
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// �Ӷ�̬���д���һ�����ʵ����
        /// </summary>
        /// <param name="strClassName">����</param>
        /// <param name="strAssemblyName">��̬����</param>
        /// <returns></returns>
        public static object CreateObject(string strClassName, string strAssemblyName)
        {

            if (strAssemblyName.IndexOf(":") < 1)  //�������̷�,�����addin
            {
                strAssemblyName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
                     +  strAssemblyName;
            }

            object newInstance = null;
            //�˴�Ӧ���жϷ����
            if (File.Exists(strAssemblyName))
            {
                Assembly assembly = GetAssemblyFromAppDomain(strAssemblyName);
                if (assembly == null)
                    assembly = Assembly.LoadFrom(strAssemblyName);
            
                newInstance = assembly.CreateInstance(strClassName);
            }
            if (newInstance == null)
            {
                throw new CreateObjectException(strClassName);               
            }
            return newInstance;
        }

        /// <summary>
        /// �ж�ĳ�����Ƿ��Ѿ����أ�������أ��ӵ�ǰ�������з��ظù�����
        /// </summary>
        /// <param name="assemblyName">Ҫ�жϵĹ���ȫ��������·�����ļ�����</param>
        /// <returns></returns>
        private static Assembly GetAssemblyFromAppDomain(string assemblyName)
        {
            IEnumerator ie = AppDomain.CurrentDomain.GetAssemblies().GetEnumerator();
            while (ie.MoveNext())
            {
                Assembly ab = ie.Current as Assembly;
                try
                {
                    if (ab != null && ab.Location.ToUpper() == assemblyName.ToUpper()) return ab;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// �����Ѿ�����ʱ�׳������⡣
    /// </summary>
    public class ObjectExistException : Exception
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        public ObjectExistException()
            : base("�����Ѿ�������")
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }
    }

    /// <summary>
    /// �ļ�������ʱ�׳������⡣
    /// </summary>
    public class FileNotExistException : Exception
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="FileName">�����ڵ��ļ���</param>
        public FileNotExistException(string FileName)
            : base("�ļ�" + FileName + "�����ڣ�")
        {
        }
    }
    /// <summary>
    /// ����������������ʧ��ʱ�׳�������
    /// </summary>
    public class CreateObjectException : Exception
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="ObjectClassName">����ʧ�ܵ�����</param>
        public CreateObjectException(string ObjectClassName)
            : base("���ܴ�����" + ObjectClassName + "��ʵ����")
        {
        }
    }
}
