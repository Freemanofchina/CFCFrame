using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace FrameCommon
{
    /// <summary>
    /// �Զ�������IDί��
    /// </summary>
    /// <returns></returns>
    public delegate string MakeCustemTypeID();

    /// <summary>
    /// ����ID��������
    /// </summary>
    public enum MakeIDType
    {
        /// <summary>
        /// ��ȡϵͳʱ��
        /// </summary>
        YMDHMM,
        /// <summary>
        /// GUID��������ѡ��GUID�����Ը�formmat������ʽ
        /// </summary>
        GUID,
        /// <summary>
        /// ��ȡϵͳʱ�䣬ͬʱ����һλ�����
        /// </summary>
        YMDHMS_1,
        /// <summary>
        /// ��ȡϵͳʱ�䣬ͬʱ����2λ�����
        /// </summary>
        YMDHMS_2,
        /// <summary>
        /// ��ȡϵͳʱ�䣬ͬʱ����3λ�����
        /// </summary>
        YMDHMS_3,
        /// <summary>
        /// ��ȡϵͳʱ�䣬ͬʱ����4λ�����
        /// </summary>
        YMDHMS_4,
        /// <summary>
        /// ��ȡϵͳʱ�䣬ͬʱ����5λ�����
        /// </summary>
        YMDHMS_5,
        /// <summary>
        /// ��ȡϵͳʱ�䣬ͬʱ����10λ�����
        /// </summary>
        YMDHMS_10,
        /// <summary>
        /// �Լ������ID����,�ù�����Ҫ�������ɵ�ί�к���event�����ܹ������ַ���ID
        /// </summary>
        CUSTEMTYPE
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// �ڵ����ķ���
        /// </summary>
        NodeCenter = 1,

        /// <summary>
        /// ҵ�����
        /// </summary>
        BizDueWith = 2,

        /// <summary>
        /// ���ݿ����
        /// </summary>
        DataBaseErr = 3
    }

    /// <summary>
    /// �ֵ�����
    /// </summary>
    public enum DictType
    {
        /// <summary>
        /// �����ֵ�
        /// </summary>
        FrameDict = 1,

        /// <summary>
        /// ҵ���ֵ�
        /// </summary>
        BizDict = 2
    }

    public class BizCommon
    {
        /// <summary>
        /// ��ȡ��ǰ����ID����
        /// </summary>
        /// <param name="makeIDTypeStr"></param>
        /// <returns></returns>
        public static MakeIDType GetCurrMakeIDType(string makeIDTypeStr)
        {
            MakeIDType currType = MakeIDType.GUID; 

            switch (makeIDTypeStr)
            {
                case "YMDHMM":
                    currType = MakeIDType.YMDHMM;
                    break;
                case "YMDHMS_1":
                    currType = MakeIDType.YMDHMS_1;
                    break;
                case "YMDHMS_10":
                    currType = MakeIDType.YMDHMS_10;
                    break;
                case "YMDHMS_2":
                    currType = MakeIDType.YMDHMS_2;
                    break;
                case "YMDHMS_3":
                    currType = MakeIDType.YMDHMS_3;
                    break;
                case "YMDHMS_4":
                    currType = MakeIDType.YMDHMS_4;
                    break;
                case "YMDHMS_5":
                    currType = MakeIDType.YMDHMS_5;
                    break;
                case "CUSTEMTYPE":
                    currType = MakeIDType.CUSTEMTYPE;
                    break;
                case "GUID":
                    currType = MakeIDType.GUID;
                    break;
                default:
                    break;
            }           

            return currType;
        }

        /// <summary>
        /// ��ȡ��ǰ����ID��ί��
        /// </summary>
        /// <param name="makeCustemTypeIDStr"></param>
        /// <returns></returns>
        public static MakeCustemTypeID GetMakeCustemTypeID(string makeCustemTypeIDStr)
        {
            //TODO
            return null;
        }

        /// <summary>
        /// ��ȡ��ǰ�ֵ�����
        /// </summary>
        /// <param name="dictTypeStr"></param>
        /// <returns></returns>
        public static DictType GetDictType(string dictTypeStr)
        {
            DictType currType = DictType.BizDict;

            switch (dictTypeStr)
            {
                case "FrameDict":
                    currType = DictType.FrameDict;
                    break;
                case "BizDict":
                    currType = DictType.BizDict;
                    break;
                default:
                    break;
            }

            return currType;
        }

        #region ��������

        /// <summary>
        /// �����ַ������ַ���������
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oriArrStr"></param>
        /// <returns></returns>
        public static bool FindStrFromStrArry(string target, string[] oriArrStr)
        {
            for (int i = 0; i < oriArrStr.Length; i++)
            {
                if (oriArrStr[i] == target)
                {
                    return true;
                }
            }

            return false;
        }
          
        /// <summary>
        /// ת����
        /// </summary>
        /// <param name="inPut"></param>
        /// <param name="inUnit"></param>
        /// <returns></returns>
        public static double ToMeter(double inPut, string inUnit)
        {
            switch (inUnit)
            {
                case "M":
                    return inPut;
                case "KM":
                    return inPut * 1000;
                case "NM":
                    return inPut * 1852;
                case "FT":
                    return inPut * 0.3048;
                case "FL":
                    return inPut * 30.48;
                case "SM": //ʮ��
                    return inPut * 10;
                default:
                    throw new Exception("δ֪�ľ��뵥λ" + inUnit);
            }
        }

        public static double ToFeet(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 3.2808399;
        }

        public static double ToFL(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.032808399;
        }

        public static double ToNM(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.00054;
        }

        public static double ToKM(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.001;
        }

        public static double ToSTDMeter(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit);
        }

        #endregion      

        #region ��������ҵ��

        /// <summary>
        /// ��ȡ�Ա������ļ�����
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="configXmlPath"></param>
        /// <returns></returns>
        public static DataSet GetCompConfig(string objData, string configXmlPath)
        {
            DataTable dtTemp = Common.Utility.GetTableFromXml("id|tablename|colname|iscomp|colnamedesc|isIdentifier",
            "String|String|String|String|String|String",
            configXmlPath);

            //���˵��ǵ�ǰ�������
            DataTable dtOk = dtTemp.Clone();
            dtOk.Rows.Clear();

            DataTable dtAirwayDetail = dtOk.Clone();
            dtAirwayDetail.Clear();

            DataRow dr = null;

            for (int j = 0; j < dtTemp.Rows.Count; j++)
            {
                if (objData == "PA" && dtTemp.Rows[j]["tablename"].ToString() == "AD_HP")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "D " && dtTemp.Rows[j]["tablename"].ToString() == "VOR")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "DBPN" && dtTemp.Rows[j]["tablename"].ToString() == "NDB")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "EAPC" && dtTemp.Rows[j]["tablename"].ToString() == "WAYPOINT")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "ER")
                {
                    if (dtTemp.Rows[j]["tablename"].ToString() == "AIRWAY")
                    {
                        dr = dtOk.NewRow();
                        dr.ItemArray = dtTemp.Rows[j].ItemArray;
                        dtOk.Rows.Add(dr);
                    }
                    if (dtTemp.Rows[j]["tablename"].ToString() == "AIRWAY_DETAIL")
                    {
                        dr = dtAirwayDetail.NewRow();
                        dr.ItemArray = dtTemp.Rows[j].ItemArray;
                        dtAirwayDetail.Rows.Add(dr);
                    }
                }

                //�÷�ֻ֧Ϊ��·��ϸ����ʱ����ȡ��ϸ�仯������Ƚϲ�ʹ��
                if (objData == "ERDetail")
                {
                    if (dtTemp.Rows[j]["tablename"].ToString() == "AIRWAY_DETAIL")
                    {
                        dr = dtAirwayDetail.NewRow();
                        dr.ItemArray = dtTemp.Rows[j].ItemArray;
                        dtAirwayDetail.Rows.Add(dr);
                    }
                }

                if (objData == "PG" && dtTemp.Rows[j]["tablename"].ToString() == "RUNWAY")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "TAKEOFF" && dtTemp.Rows[j]["tablename"].ToString() == "TAKEOFF")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "LANDSTANDARD" && dtTemp.Rows[j]["tablename"].ToString() == "LANDSTANDARD")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "ILS_CAT" && dtTemp.Rows[j]["tablename"].ToString() == "ILS_CAT")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }
                if (objData == "ENROUTE" && dtTemp.Rows[j]["tablename"].ToString() == "COMPANY_ENROUTE")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }
            }

            DataSet dsTemp = new DataSet();
            dtOk.TableName = "basedata";
            dsTemp.Tables.Add(dtOk);

            dtAirwayDetail.TableName = "airwayDetailData";
            dsTemp.Tables.Add(dtAirwayDetail);

            return dsTemp;
        }

        #endregion


    }

   
}
