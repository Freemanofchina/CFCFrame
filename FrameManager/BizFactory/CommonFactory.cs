using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.MetadataServices;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Web.Script.Serialization;

using Biz;
using BizExectute;
using Common;
using Entitys.ComonEnti;
using FrameCommon;

namespace BizFactory
{
    public class CommonFactory
    {
        public CommonBiz Create(string bizType, SysEnvironmentSerialize _envirObj)
        {
            if (bizType == "one")
            {
                //return new BizExectuteCommon();
                return AOPFactory.Create<BizExectuteCommon>(new BizExectuteCommon(_envirObj));
            }
            return null;
        }
    }

    /// <summary>
    /// ���͹���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonFactory<T>
    {
        public T Create(string bizType, SysEnvironmentSerialize _envirObj)
        {
            return (T)this.GetBiz(bizType, _envirObj);
        }

        public object GetBiz(string bizType, SysEnvironmentSerialize _envirObj)
        {
            if (bizType == "one")
            {                
                return AOPFactory.Create<BizExectuteCommon>(new BizExectuteCommon(_envirObj));
            }
            return null;
        }
    }   

    #region AOP��ش���

    public class DueWithAOP<T> : RealProxy
    {
        public string logpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("logpathForDebug", "");  //������־·�� 
        public string isLogpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("isLogpathForDebug", "");  //�Ƿ��¼������־

        public T Target
        {
            get; internal set;
        }

        public DueWithAOP(T target) : base(typeof(T))
        {
            this.Target = target;
        }

        public override IMessage Invoke(IMessage msg)
        {            
            #region ��־׼��

            List<string> opObjPerporty = UtilitysForT<SSY_LOGENTITY>.GetAllColumns(new SSY_LOGENTITY()); //Ҫ������������
            List<string> opWherePerporty = new List<string>(); //where���������� 
            opWherePerporty.Add("LOGID");
            List<string> mainProperty = new List<string>(); //���������� 
            mainProperty.Add("LOGID");

            //string errStr = string.Empty;
            List<string> errStr = new List<string>();
            List<SSY_LOGENTITY> opList = new List<SSY_LOGENTITY>();
            SSY_LOGENTITY logenti = null;  

            BizExectuteCommon recordLog = new BizExectuteCommon(ManagerSysEnvironment.GetSysEnvironmentSerialize()); //����������¼��־Ҳ���øù�������

            //��־�̶�����
            string USERNAMES = string.Empty;
            if (FrameCommon.SysEnvironment.SysUserDict != null)
            {
                if (FrameCommon.SysEnvironment.SysUserDict.USERNAME != null)
                {
                    USERNAMES = FrameCommon.SysEnvironment.SysUserDict.USERNAME.ToString();
                }
            }
            string IPS = string.Empty;
            if(!string.IsNullOrEmpty(FrameCommon.SysEnvironment.Ips))
            {
                IPS = FrameCommon.SysEnvironment.Ips;
            }
            string SYSTEMNAME = string.Empty;
            if (!string.IsNullOrEmpty(FrameCommon.SysEnvironment.distManagerParam.DistributeDataNodes[0].Systemname))
            {
                SYSTEMNAME = FrameCommon.SysEnvironment.distManagerParam.DistributeDataNodes[0].Systemname;
            }

            #endregion

            IMethodCallMessage mcall = (IMethodCallMessage)msg; //�ٳַ�����׼��ִ��
            var resResult = new ReturnMessage(new Exception(), mcall);

            #region ��ȡ��Ҫ����

            //distributeActionIden  �ֲ�ʽ����ʶ��, �������
            //distributeDataNodes �ֲ�ʽ���ݽڵ㼯�ϣ� �������
            //distributeDataNode �ֲ�ʽ���ݽڵ������ �������   
            //distriActionSql �ֲ�ʽ����sql���ϣ�������ڣ�����sql���ĺͲ���
            //ddnmParams

            //singleActionList  �������ʧ�ܼ���, out���� �Ǳ�����ڣ�����յĲ�������  

            //TODO ������Ĳ������������ڲ�����ִ��ҵ�񷽷�������ִ���쳣

            //��ȡ�ֲ�ʽ�������
            DistributeDataNodeManagerParams distManagerParam = new  DistributeDataNodeManagerParams();
            for (int i = 0; i < mcall.InArgs.Length; i++)
            {
                if (mcall.GetInArgName(i).ToUpper() == "ddnmParams".ToUpper())
                {
                    distManagerParam = ((DistributeDataNodeManagerParams)mcall.GetInArg(i));
                    //SYSTEMNAME = distManagerParam.DistributeDataNodes[0].Systemname;
                    break;
                }
            }           

            //��ȡ�ֲ�ʽ����ʶ�����
            DistributeActionIden distBAC = distManagerParam.DistributeActionIden;

            //�������ݽڵ㼯�ϣ�Ȼ����ݽڵ��������ֲ�ʽ����ʶ���ʼ���ֲ�ʽ���ݽڵ㼰�ֲ�ʽ������
            //���ݽڵ㼯���ɷ��񷽷�����            
            //��ȡ���ݽڵ㼯�ϲ���
            List<SSY_DATANODE_ADDR> dataNodes = distManagerParam.DistributeDataNodes;

            //��ȡ���ݽڵ����
            DistributeDataNode ddn = distManagerParam.DistributeDataNode;

            //�������ʧ�ܼ��ϣ����Ҫ������ڵ����ģ�out����
            bool permitSingleDataOperation = false; //�Ƿ�֧�ֵ������ʧ�ܺ���б���
            List<SSY_DATA_ACTION_TASK> data_action_task = new List<SSY_DATA_ACTION_TASK>();
            for (int i = 0; i < mcall.InArgs.Length; i++)
            {
                if (mcall.GetInArgName(i).ToUpper() == "singleActionList".ToUpper())
                {
                    permitSingleDataOperation = true;
                    data_action_task = mcall.GetInArg(i) as List<SSY_DATA_ACTION_TASK>;
                    break;
                }
            }

            #endregion

            if (distBAC == DistributeActionIden.Query)
            {
                //�������ݽڵ�
                //distManagerParam.DistributeDataNode
                distManagerParam.DistributeDataNode.Connectionstring = string.Format(dataNodes[0].Data_conn, dataNodes[0].Url_addr,
                    dataNodes[0].Data_user, dataNodes[0].Data_password);
                distManagerParam.DistributeDataNode.DbSchema = dataNodes[0].Data_schema;

                //ִֻ��һ�μ���
                #region ִ��ҵ�񷽷�

                try
                {
                    object objRv = mcall.MethodBase.Invoke(this.Target, mcall.Args);

                    #region ��¼ҵ����־

                    //ִ�з������¼����ҵ����־���������Է���ListBizLog����
                    //��Ҫ��¼��־��Ҫ��÷������봫��ò����������ֱ���ΪListBizLog������ΪҪ��¼��ҵ����־����
                    SSY_LOGENTITY tempLog = null;
                    for (int i = 0; i < mcall.InArgs.Length; i++)
                    {
                        if (mcall.GetInArgName(i).ToUpper() == "ListBizLog".ToUpper())
                        {
                            List<SSY_LOGENTITY> dictBizLog = mcall.GetInArg(i) as List<SSY_LOGENTITY>;

                            for (int j = 0; j < dictBizLog.Count; j++)
                            {
                                //������¼ҵ����־
                                tempLog = dictBizLog[j] as SSY_LOGENTITY;

                                //��ȡ��־����,ȷ���Ƿ��¼������־
                                if (recordLog.CheckIsRecord(tempLog.DOMAINNAME.ToString(), tempLog.OPTIONNAME.ToString(), distManagerParam))
                                {
                                    tempLog.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                                    //ҵ��ֱ��ʹ��ҵ����ύ�����ݣ����ڶ�ȡ��ܻ�����������Ϊ��¼ʱ�ⲿ�������ͺ󣬵��²��ܼ�����־
                                    //tempLog.USERNAMES = USERNAMES;
                                    //tempLog.IPS = IPS;
                                    //tempLog.SYSTEMNAME = SYSTEMNAME;

                                    opList.Add(tempLog);
                                }
                            }
                            if(opList.Count > 0)
                            {
                                //��¼��־
                                bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                            }                            
                            break;
                        }
                    }

                    #endregion

                    resResult = new ReturnMessage(objRv, mcall.Args, mcall.Args.Length, mcall.LogicalCallContext, mcall);
                }
                catch (Exception ex)
                {
                    Common.Utility.RecordLog("���� Query ģʽ�����쳣��ԭ��" + ex.Message + ex.Source, this.logpathForDebug, this.isLogpathForDebug);

                    #region ��¼�쳣��־

                    if (ex.InnerException != null)
                    {
                        //��ȡ��־����,ȷ���Ƿ��¼������־
                        if (recordLog.CheckIsRecord("ExceptionErr", "ExceptionErr", distManagerParam))
                        {
                            //�����쳣�������Ϣ
                            string CLASSNAME = mcall.TypeName;
                            string METHORDNAME = mcall.MethodName;                           

                            //�쳣ʱ�ⲿ�ֿ�û������
                            string TABLENAME = "";
                            string RECORDIDENCOLS = "";
                            string RECORDIDENCOLSVALUES = "";
                            string FUNCTIONNAME = "";

                            logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.ExceptionErr, LogLevelOption.ExecptionErr, recordLog.GetSystemDateTime(distManagerParam), 
                                CLASSNAME, METHORDNAME, LogAction.ExecptionErr, TABLENAME, RECORDIDENCOLS, RECORDIDENCOLSVALUES, USERNAMES, IPS, FUNCTIONNAME, 
                                ex.InnerException.Message, SYSTEMNAME, "");
                            logenti.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                            opList.Add(logenti);

                            if (opList.Count > 0)
                            {
                                //��¼��־
                                bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                            }                                
                        }

                        resResult = new ReturnMessage(ex.InnerException, mcall);
                    }

                    #endregion

                    resResult = new ReturnMessage(ex, mcall);
                }

                #endregion
            }
            else if (distBAC == DistributeActionIden.SingleAction)
            {
                //���ݽڵ��м���ִ�м��Σ������ύ������ִ���쳣�����쳣������ڵ����ģ�����ִ�У�ֱ�����
                for (int m = 0; m < dataNodes.Count; m++)
                {
                    //ddn.DbFactoryName  ���ݿ⹤��ȡ�����ļ���Ŀǰ������ͬ����ͬ��������ݿ�
                    distManagerParam.DistributeDataNode.Connectionstring = string.Format(dataNodes[m].Data_conn, dataNodes[m].Url_addr, dataNodes[m].Data_user,
                        dataNodes[m].Data_password);
                    distManagerParam.DistributeDataNode.DbSchema = dataNodes[m].Data_schema;

                    #region ִ��ҵ�񷽷�

                    try
                    {
                        object objRv = mcall.MethodBase.Invoke(this.Target, mcall.Args);

                        #region ��¼ҵ����־

                        //ִ�з������¼����ҵ����־���������Է���ListBizLog����
                        //��Ҫ��¼��־��Ҫ��÷������봫��ò����������ֱ���ΪListBizLog������ΪҪ��¼��ҵ����־����
                        SSY_LOGENTITY tempLog = null;
                        for (int i = 0; i < mcall.InArgs.Length; i++)
                        {
                            if (mcall.GetInArgName(i).ToUpper() == "ListBizLog".ToUpper())
                            {
                                List<SSY_LOGENTITY> dictBizLog = mcall.GetInArg(i) as List<SSY_LOGENTITY>;

                                for (int j = 0; j < dictBizLog.Count; j++)
                                {
                                    //������¼ҵ����־
                                    tempLog = dictBizLog[j] as SSY_LOGENTITY;

                                    //��ȡ��־����,ȷ���Ƿ��¼������־
                                    if (recordLog.CheckIsRecord(tempLog.DOMAINNAME.ToString(), tempLog.OPTIONNAME.ToString(), distManagerParam))
                                    {
                                        tempLog.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                                        //ҵ��ֱ��ʹ��ҵ����ύ�����ݣ����ڶ�ȡ��ܻ�����������Ϊ��¼ʱ�ⲿ�������ͺ󣬵��²��ܼ�����־
                                        //tempLog.USERNAMES = USERNAMES;
                                        //tempLog.IPS = IPS;
                                        //tempLog.SYSTEMNAME = SYSTEMNAME;                                      

                                        opList.Add(tempLog);
                                    }
                                }
                                if(opList.Count > 0)
                                {
                                    //��¼��־
                                    bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                                }
                                break;
                            }
                        }

                        #endregion

                        resResult = new ReturnMessage(objRv, mcall.Args, mcall.Args.Length, mcall.LogicalCallContext, mcall);
                    }
                    catch (Exception ex)
                    {
                        SSY_DATA_ACTION_TASK tempDataTask = null;

                        #region ��¼�쳣��־

                        if (ex.InnerException != null)
                        {
                            //��ȡ��־����,ȷ���Ƿ��¼������־
                            if (recordLog.CheckIsRecord("ExceptionErr", "ExceptionErr", distManagerParam))
                            {
                                //�����쳣�������Ϣ
                                string CLASSNAME = mcall.TypeName;
                                string METHORDNAME = mcall.MethodName;

                                //�쳣ʱ�ⲿ�ֿ�û������
                                string TABLENAME = "";
                                string RECORDIDENCOLS = "";
                                string RECORDIDENCOLSVALUES = "";
                                string FUNCTIONNAME = "";

                                logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.ExceptionErr, LogLevelOption.ExecptionErr, recordLog.GetSystemDateTime(distManagerParam),
                                    CLASSNAME, METHORDNAME, LogAction.ExecptionErr, TABLENAME, RECORDIDENCOLS, RECORDIDENCOLSVALUES, USERNAMES, IPS, FUNCTIONNAME, 
                                    ex.InnerException.Message, SYSTEMNAME, "");
                                logenti.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                                opList.Add(logenti);

                                if(opList.Count > 0)
                                {
                                    //��¼��־
                                    bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                                }
                            }
                            if (permitSingleDataOperation)
                            {
                                #region ��ȡʧ�ܲ���sql

                                //��ȡʧ�ܼ�¼���Ա㽫���񱨸���ڵ�����
                                //��ȡ����sql  List<DistActionSql> DistriActionSqlParams

                                for (int task = 0; task < distManagerParam.DistriActionSqlParams.Count; task++)
                                {
                                    tempDataTask = new SSY_DATA_ACTION_TASK();
                                    tempDataTask.Action_sql = distManagerParam.DistriActionSqlParams[task].ActionSqlText;
                                    string tempSqlParamSeq = string.Empty;
                                    bool temddddd = JsonSerializer.Serialize(distManagerParam.DistriActionSqlParams[task].ActionSqlTextParams, out tempSqlParamSeq);
                                    //����sql�������л����
                                    tempDataTask.Action_sql_params = tempSqlParamSeq;
                                    tempDataTask.Data_real_conn = ddn.Connectionstring;
                                    data_action_task.Add(tempDataTask);
                                }
                                //ִ����Ϻ�������ε�sql��¼
                                distManagerParam.DistriActionSqlParams.Clear();

                                //TODO ���浥���쳣���ڵ����ģ���ʱ��֧�֣�������չ
                                #endregion
                            }
                        }

                        #endregion

                        continue; //����ִ��
                    }

                    #endregion
                }                
            }
            else if (distBAC == DistributeActionIden.TransAction)
            {
                try
                {
                    //�ֲ�ʽ����ִ�У� �����ݽڵ�����ִ�У�ͬ���ύ
                    using (var ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, TimeSpan.FromHours(1)))
                    {
                        for (int m = 0; m < dataNodes.Count; m++)
                        {
                            //ddn.DbFactoryName  ���ݿ⹤��ȡ�����ļ���Ŀǰ������ͬ����ͬ��������ݿ�
                            distManagerParam.DistributeDataNode.Connectionstring = string.Format(dataNodes[m].Data_conn, dataNodes[m].Url_addr, dataNodes[m].Data_user,
                            dataNodes[m].Data_password);
                            distManagerParam.DistributeDataNode.DbSchema = dataNodes[m].Data_schema;

                            #region ִ��ҵ�񷽷�

                            object objRv = mcall.MethodBase.Invoke(this.Target, mcall.Args);

                            #region ��¼ҵ����־

                            //ִ�з������¼����ҵ����־���������Է���ListBizLog����
                            //��Ҫ��¼��־��Ҫ��÷������봫��ò����������ֱ���ΪListBizLog������ΪҪ��¼��ҵ����־����
                            SSY_LOGENTITY tempLog = null;
                            for (int i = 0; i < mcall.InArgs.Length; i++)
                            {
                                if (mcall.GetInArgName(i).ToUpper() == "ListBizLog".ToUpper())
                                {
                                    List<SSY_LOGENTITY> dictBizLog = mcall.GetInArg(i) as List<SSY_LOGENTITY>;

                                    for (int j = 0; j < dictBizLog.Count; j++)
                                    {
                                        //������¼ҵ����־
                                        tempLog = dictBizLog[j] as SSY_LOGENTITY;

                                        //��ȡ��־����,ȷ���Ƿ��¼������־
                                        if (recordLog.CheckIsRecord(tempLog.DOMAINNAME.ToString(), tempLog.OPTIONNAME.ToString(), distManagerParam))
                                        {
                                            tempLog.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                                            //ҵ��ֱ��ʹ��ҵ����ύ�����ݣ����ڶ�ȡ��ܻ�����������Ϊ��¼ʱ�ⲿ�������ͺ󣬵��²��ܼ�����־
                                            //tempLog.USERNAMES = USERNAMES;
                                            //tempLog.IPS = IPS;
                                            //tempLog.SYSTEMNAME = SYSTEMNAME;                                           

                                            opList.Add(tempLog);
                                        }
                                    }
                                    //�������ﲻ��ͬʱ��¼��־����Ҫ�ŵ�ҵ�񷽷��ύ��ɺ󵥶���¼��־
                                    if (opList.Count > 0)
                                    {
                                        //��¼��־
                                        bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                                    }
                                    break;
                                }
                            }

                            #endregion

                            resResult = new ReturnMessage(objRv, mcall.Args, mcall.Args.Length, mcall.LogicalCallContext, mcall);

                            #endregion
                        }

                        ts.Complete();
                        ts.Dispose();
                    }

                    //ͬʱ��¼��־����Ϊ��־��¼ȥ�����﷽ʽ
                    ////�ָ�����Ĭ�ϱ�ʶ
                    //distManagerParam.DistributeActionIden = DistributeActionIden.Query;
                    ////����������־��Ҫ������¼�����ܺ�ҵ���������һ��������
                    //for (int m = 0; m < dataNodes.Count; m++)
                    //{
                    //    //ddn.DbFactoryName  ���ݿ⹤��ȡ�����ļ���Ŀǰ������ͬ����ͬ��������ݿ�
                    //    distManagerParam.DistributeDataNode.Connectionstring = string.Format(dataNodes[m].Data_conn, dataNodes[m].Url_addr, dataNodes[m].Data_user,
                    //    dataNodes[m].Data_password);
                    //    distManagerParam.DistributeDataNode.DbSchema = dataNodes[m].Data_schema;

                    //    if (opList.Count > 0)
                    //    {
                    //        //��¼��־
                    //        bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);

                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    Common.Utility.RecordLog("���� TransAction ģʽ�����쳣��ԭ��" + ex.Message + ex.Source, this.logpathForDebug, this.isLogpathForDebug);
                }                              

            }            

            //���շ��ؽ��,ѭ���������ݽڵ�ʱ��ֻ�������һ��ִ�гɹ������ݽڵ��ִ�����
            return resResult;           
        }
    }

    public static class AOPFactory
    {
        public static T Create<T>(T target)
        {
            DueWithAOP<T> dwAOP = new DueWithAOP<T>(target);
            return (T)(dwAOP.GetTransparentProxy());
        }
    }

    #endregion
}
