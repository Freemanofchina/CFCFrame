using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Caching;

using Entitys.ComonEnti;
using Common;
using FrameCommon;

namespace Biz
{
    public abstract class CommonBiz : System.MarshalByRefObject
    {
        public DataTable i18nCommonCurrLang = new DataTable(); //ͨ�����԰�
        public DataTable i18nFrameSecurityi18nLang = new DataTable(); //��ܰ�ȫ���԰�        
        public DataTable i18nFrameManageri18nLang = new DataTable(); //��ܹ������԰�
        public SysEnvironmentSerialize envirObj = null; //���ݿ�ܻ���
        public string permitMaxLoginFailtCnt = ""; //�����������¼����

        public string logpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("logpathForDebug", "");  //������־·�� 
        public string isLogpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("isLogpathForDebug", "");  //�Ƿ��¼������־



        //������������Ҫ�ۼ�

        public CommonBiz(SysEnvironmentSerialize _envirObj)
        {
            string _currlang = _envirObj.I18nCurrLang;
            System.Web.Caching.Cache currCache = HttpRuntime.Cache; //��ǰ����
            string defaultlang = APPConfig.GetAPPConfig().GetConfigValue("currlang", "");  //Ĭ������ 
            this.envirObj = _envirObj;
            this.permitMaxLoginFailtCnt = APPConfig.GetAPPConfig().GetConfigValue("permitMaxLoginFailtCnt", "5");  //�����������¼����, Ĭ��5��

            #region ͨ�����԰�

            DataTable comlangtmp = (DataTable)currCache.Get("i18nCommonCurrLang");
            if (comlangtmp != null)
            {
                if (defaultlang == _currlang)
                {
                    i18nCommonCurrLang = comlangtmp;
                }
                else
                {
                    string commoni18nLangPath = string.Format(APPConfig.GetAPPConfig().GetConfigValue("Commoni18nLang", ""), _currlang);
                    i18nCommonCurrLang = BaseServiceUtility.GetI18nLang(commoni18nLangPath);
                }
            }
            else
            {
                string commoni18nLangPath = string.Format(APPConfig.GetAPPConfig().GetConfigValue("Commoni18nLang", ""), _currlang);
                i18nCommonCurrLang = BaseServiceUtility.GetI18nLang(commoni18nLangPath);
            }

            #endregion

            #region ��ܰ�ȫ���԰�

            DataTable servFrameSecuriylangtmp = (DataTable)currCache.Get("i18nFrameSecurityi18nLang");
            if (servFrameSecuriylangtmp != null)
            {
                if (defaultlang == _currlang)
                {
                    i18nFrameSecurityi18nLang = servFrameSecuriylangtmp;
                }
                else
                {
                    string FrameSecurityi18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameSecurityi18nLang", ""), _currlang);
                    i18nFrameSecurityi18nLang = BaseServiceUtility.GetI18nLang(FrameSecurityi18nLang);
                }
            }
            else
            {
                string FrameSecurityi18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameSecurityi18nLang", ""), _currlang);
                i18nFrameSecurityi18nLang = BaseServiceUtility.GetI18nLang(FrameSecurityi18nLang);
            }

            #endregion

            #region ��ܹ������԰�

            DataTable servFrameManagerlangtmp = (DataTable)currCache.Get("i18nFrameManageri18nLang");
            if (servFrameManagerlangtmp != null)
            {
                if (defaultlang == _currlang)
                {
                    i18nFrameManageri18nLang = servFrameManagerlangtmp;
                }
                else
                {
                    string FrameManageri18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameManageri18nLang", ""), _currlang);
                    i18nFrameManageri18nLang = BaseServiceUtility.GetI18nLang(FrameManageri18nLang);
                }
            }
            else
            {
                string FrameManageri18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameManageri18nLang", ""), _currlang);
                i18nFrameManageri18nLang = BaseServiceUtility.GetI18nLang(FrameManageri18nLang);
            }

            #endregion
        }

        #region ϵͳȨ�����

        /// <summary>
        /// ��ȡ�û���
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams">�ֲ�ʽ�����������������п�ܸ�ֵ</param>
        /// <param name="ListBizLog">��¼��־���ݲ�����������¼��־���Բ�����</param>
        /// <returns></returns>
        public abstract DataTable GetUserForLogin(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams,
            List<SSY_LOGENTITY> ListBizLog);

        /// <summary>
        /// �û���ȫ�˳�
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams">�ֲ�ʽ�����������������п�ܸ�ֵ</param>
        /// <param name="ListBizLog">��¼��־���ݲ�����������¼��־���Բ�����</param>
        /// <returns></returns>
        public abstract string QuitUserForLogin(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams,
            List<SSY_LOGENTITY> ListBizLog);

        /// <summary>
        /// ��ȡ�û���
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetUsers(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡ�����û�
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetAllUsers(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataSet GetPages(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡ�û���
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataSet GetGroup(SSY_GROUP_DICT gd, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡ�û���(����ҳ)
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetGroup(SSY_GROUP_DICT gd, DistributeDataNodeManagerParams distributeDataNodeManagerParams,  SSY_PagingParam pager);

        /// <summary>
        /// ��ȡ�û�
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetUserdict(SSY_USER_DICT bizobj, DistributeDataNodeManagerParams distributeDataNodeManagerParams, SSY_PagingParam pager);


        /// <summary>
        /// ��ȡҳ��
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataSet GetPage(SSY_PAGE_DICT bizobj, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡ�û���(����ҳ)
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetPagePager(SSY_PAGE_DICT bizobj, DistributeDataNodeManagerParams distributeDataNodeManagerParams, SSY_PagingParam pager);


        /// <summary>
        /// ��ȡ�û������û�
        /// </summary>
        /// <param name="model"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>        
        public abstract DataTable GetGroupUserPager(SSY_USER_GROUP_DICT model, DistributeDataNodeManagerParams distributeDataNodeManagerParams, SSY_PagingParam pager);


        /// <summary>
        /// ��ȡ��ɫȨ������
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract SSY_PAGE_GROUP_MQT GetGroupPageMgt(SSY_GROUP_PAGE_DICT model, DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// ����Ĭ������
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="ListBizLog"></param>
        /// <returns></returns>
        public abstract bool ResetUserPWD(SSY_USER_DICT model, DistributeDataNodeManagerParams ddnmParams, List<SSY_LOGENTITY> ListBizLog);

        #endregion

        #region ϵͳ������

        /// <summary>
        /// ��ȡϵͳ��ܲ���
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameParam(DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡϵͳ��ܲ�����ϸ
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameParamDetail(DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡϵͳ���ṹ������
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetTreeViewConfig(DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡϵͳ�˵�������
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetMenuConfig(DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        #endregion

        #region ϵͳ��־���

        /// <summary>
        /// ��ȡϵͳ��ṹ
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetDataEntity(string  dataEntName, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// �ж�ĳ����־�Ƿ��¼
        /// </summary>
        /// <param name="logtypeDomain"></param>
        /// <param name="loglevelOption"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract bool CheckIsRecord(string logtypeDomain, string loglevelOption, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ��ȡϵͳ��־
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetLogDataPager(SSY_LOGENTITY model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager);

        #endregion

        #region �ֵ����

        /// <summary>
        /// ��ȡȫ��ϵͳ�ֵ�
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameDictAll(SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// ��ȡϵͳ�ֵ�(ĳ���ֵ��ȫ���ֵ���)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameDictPager(SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager);


        /// <summary>
        /// ��ȡȫ��ҵ���ֵ�
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetBizDictAll(SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// ��ȡϵͳ�ֵ�(ĳ���ֵ��ȫ���ֵ���)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetBizDictPager(SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager);

        /// <summary>
        /// ��ȡĳ���ֵ�
        /// </summary>
        /// <param name="DOMAINNAMEIDEN">�ֵ����</param>
        /// <param name="dicttype">�ֵ�����(���� ҵ��)</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetDicts(string DOMAINNAMEIDEN, DictType dicttype, DistributeDataNodeManagerParams ddnmParams);

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡϵͳID�ַ���
        /// </summary>
        /// <param name="makeIDType"></param>
        /// <param name="formmat"></param>
        /// <param name="makeCustemTypeID"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public virtual string GetID(MakeIDType makeIDType, string formmat, MakeCustemTypeID makeCustemTypeID, DistributeDataNodeManagerParams ddnmParams)
        {
            string retId = string.Empty;

            if (makeIDType == MakeIDType.YMDHMM)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss");
            }
            else if (makeIDType == MakeIDType.GUID)
            {
                if(string.IsNullOrEmpty(formmat))
                {
                    return System.Guid.NewGuid().ToString();
                }
                else
                {
                    System.Guid guid = new System.Guid(formmat);
                    return guid.ToString();
                }
            }
            else if (makeIDType == MakeIDType.YMDHMS_1)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(1).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_2)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(2).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_3)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(3).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_4)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(4).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_5)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(5).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_10)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(10).ToString();
            }
            else if (makeIDType == MakeIDType.CUSTEMTYPE)
            {
                return makeCustemTypeID();
            }

            return retId;
        }

        /// <summary>
        /// ��ȡϵͳʱ��
        /// </summary>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public virtual string GetSystemDateTime(DistributeDataNodeManagerParams ddnmParams)
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// ͳһ����ҵ��ʵ�巺��,��¼��־��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objLists">ҵ��ʵ��List</param>
        /// <param name="opObjPropertyL">Ҫ������ҵ��ʵ���������ƣ���Ӧ�����ݿ��ֶΣ���ȷ�϶���ɾ��ʱ��������ַ���</param>
        /// <param name="wherePropertyL">ҵ��ʵ��where����������ƣ���Ӧ�����ݿ��ʾ�����ֶ�</param>
        /// <param name="mainPropertyL">�����ֶ�˵��</param>
        /// <param name="errStr"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract bool OpBizObjectSingle<T>(List<T> objLists, List<string> opObjPropertyL, List<string> wherePropertyL, List<string> mainPropertyL, List<string> errStr,
            DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// ͳһ����ҵ��ʵ�巺��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objLists">ҵ��ʵ��List</param>
        /// <param name="opObjPropertyL">Ҫ������ҵ��ʵ���������ƣ���Ӧ�����ݿ��ֶΣ���ȷ�϶���ɾ��ʱ��������ַ���</param>
        /// <param name="wherePropertyL">ҵ��ʵ��where����������ƣ���Ӧ�����ݿ��ʾ�����ֶ�</param>
        /// <param name="mainPropertyL">�����ֶ�˵��</param>
        /// <param name="errStr"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="ListBizLog"></param>
        /// <returns></returns>
        public abstract bool OpBizObjectSingle<T>(List<T> objLists, List<string> opObjPropertyL, List<string> wherePropertyL, List<string> mainPropertyL, List<string> errStr,
            DistributeDataNodeManagerParams distributeDataNodeManagerParams, List<SSY_LOGENTITY> ListBizLog);


        /// <summary>
        /// ͨ�ò�ѯ����ʵ���Ƿ�������ݿ���
        /// </summary>
        /// <param name="bizobjectname">���ݿ��еı���</param>
        /// <param name="wherePropertyL">�ֶ�|�ֶ�ֵ,���磺prop1|value1</param>
        /// <param name="splitchar">wherePropertyL�еķָ���ţ�Ĭ��|</param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
        public abstract bool CheckBizObjectRepat(string bizobjectname, List<string> wherePropertyL, string splitchar,
            DistributeDataNodeManagerParams distributeDataNodeManagerParams, List<string> errStr);

        /// <summary>
        /// ͨ�ò�ѯ����ʵ���Ƿ�������ݿ���,������ѯ��֧�ֱַ��ѯһ���ֶΡ�һ���ֶ��Ƿ����,�ɲ�ѯ������¼
        /// </summary>
        /// <param name="bizObjectName">����ʵ�����</param>
        /// <param name="fields">Ҫ��ѯ���ֶ���ϣ�֧�ֶ����ѯ�����磺field1|field2|field3|field4;field5|field6,��ʾ�ķֱ��ѯ�ֶ�1���ֶ�2���ֶ�3���ֶ�6�ֱ𵥶����ظ����ֶ�4���ֶ�5��ϲ��ظ�</param>
        /// <param name="fieldsValue">ÿ���ֶζ�Ӧ������ֵ���ַ�������</param>
        /// <param name="splitChar">Ҫ��ѯ���ظ���Ԫ�ָ����š���Ӧ�����е����� |  ���Զ�����������Ҫȷ�������������ݳ�ͻ</param>
        /// <param name="splitCharSub">Ҫ��ѯ����Ϸָ����š���Ӧ�����еķֺ� ; ���Զ������� ��Ҫȷ�������������ݳ�ͻ</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract List<string> CheckBizObjectsRepat(string bizObjectName, string fields, List<string> fieldsValue, string splitChar, string splitCharSub,
            DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// ͨ�û�ȡ����ʵ����������
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetEntityAllDataForCommon(string tablename, DistributeDataNodeManagerParams ddnmParams);

        #endregion
               


    }

   
}
