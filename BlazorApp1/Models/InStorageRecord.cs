using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SVW_BYSJ_WEB.Models
{
    public class InStorageRecord
    {
        public int InStorageNumber { get; set; }        //入库单号
        public string SVWNumber { get; set; }           //备件的SVW物料号
        public DateTime InStorageTime { get; set; }     //备件的入库日期
        public int InStorageCount { get; set; }         //备件的入库数量
        public string writtenby { get; set; }           //填写人
        public DateTime writtenTime { get; set; }       //填写时间
        public string remark { get; set; }              //备注
    }
}
