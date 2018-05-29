using System;
using Common;
using FrameCommon;


namespace DataAccessLayer.DataBaseFactory
{
    /// <summary>
    /// ���ݹ�����������
    /// </summary>
    public class DBFactorySingleton
    {
        static DBFactorySingleton dbFactorySingleton = null;

        static string connStr = string.Empty; //���浱ǰ�������Ӵ�

        AbstractDBFactory factory;
        /// <summary>
        /// ���Ψһʵ����
        /// </summary>
        /// <returns></returns>
        public static DBFactorySingleton GetInstance(DistributeDataNode ddn)
        {
            //������Ҫ֧�ֲ�ͬ���ݿ�ĵ�����ԭ�����������ӱ����Ҫ����ʵ������
            if (dbFactorySingleton == null)
            {
                dbFactorySingleton = new DBFactorySingleton(ddn);
                connStr = ddn.Connectionstring;
            }
            else
            {
                if (connStr != ddn.Connectionstring)
                {
                    dbFactorySingleton = new DBFactorySingleton(ddn);
                    connStr = ddn.Connectionstring;
                }
            }

            return dbFactorySingleton;
        }      
       
        /// <summary>
        /// ���ݿ���� ���� ��
        /// </summary>
        public AbstractDBFactory Factory
        {
            get
            {
                return factory;
            }
        }   

        /// <summary>
        /// ���캯����
        /// </summary>
        private DBFactorySingleton(DistributeDataNode ddn)
        {
            //string dbFactoryName = APPConfig.GetAPPConfig().GetConfigValue("DBFactoryName", "");
            string dbFactoryName = ddn.DbFactoryName;
            if (dbFactoryName == "")
            {                
                throw new Exception("no find DBFactoryName in config file!");
                //return;
            }
            factory = (AbstractDBFactory) ClassBuilder.CreateObject(dbFactoryName);
            if(factory==null)
                throw new Exception("DBFactoryName find error in config file!");

            //string connectString = APPConfig.GetAPPConfig().GetConfigValue("ConnectionString", "");
            string connectString = ddn.Connectionstring;
            if (connectString == "")
                throw new Exception("find error for init database connection!");

            //string dbschema = APPConfig.GetAPPConfig().GetConfigValue("Dbschema", "");
            string dbschema = ddn.DbSchema;
            if (dbschema == "")
                throw new Exception("find error for init database schema!");

            try
            {                
                factory.ConnectionString = connectString;
                factory.DbSchema = dbschema;
            }
            catch (Exception e)
            {
                throw new Exception("database connection error!\n" + e.ToString());
            }            
        }

        /// <summary>
        /// �������ݿ���������
        /// �÷�ʽ��֧�ֶ����ݽڵ�Ӧ��
        /// </summary>
        /// <param name="dbFactoryName"></param>
        /// <param name="connectionstring"></param>
        /// <param name="dbSchema"></param>
        /// <returns></returns>
        public AbstractDBFactory BuildFactory(string dbFactoryName, string connectionstring, string dbSchema)
        {
            factory = ClassBuilder.CreateObject(dbFactoryName) as AbstractDBFactory;            
            factory.ConnectionString = connectionstring;
            factory.DbSchema = dbSchema;

            return factory;
        }
    }
}
