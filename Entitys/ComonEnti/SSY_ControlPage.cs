using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    [DataContract]
    public  class SSY_ControlPage
    {
        #region ҳ��С
        [DataMember]
        /// <summary>
        /// ҳ��С
        /// <summary>
        private int _pageSize = 0;
        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                this._pageSize = value;
            }
        }
        #endregion

        #region ��¼����
        [DataMember]
        /// <summary>
        /// ��¼����
        /// <summary>
        private int _inRecordCnt = 0;
        public int InRecordCnt
        {
            get
            {
                return this._inRecordCnt;
            }
            set
            {
                this._inRecordCnt = value;
            }
        }
        #endregion

        #region ��ǰҳ��
        [DataMember]
        /// <summary>
        /// ��ǰҳ��
        /// <summary>
        private int _pageIndex = 0;
        public int PageIndex
        {
            get
            {
                return this._pageIndex;
            }
            set
            {
                this._pageIndex = value;
            }
        }
        #endregion       


    }
}
