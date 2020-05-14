using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SVW_BYSJ_WEB.Models
{
    public class ReWorkRecord
    {
        public int ReWorkNumber { get; set; }           //返修单编号
        public string SVWNumber { get; set; }           //返修件的SVW物料号
        public string Location { get; set; }            //存放位置
        public DateTime ReWorkTime { get; set; }        //备件的返修日期
        public int ReWorkCount { get; set; }            //备件的返修数量
        public string writtenby { get; set; }           //填写人
        public DateTime writtenTime { get; set; }       //填写时间
        public group Group { get; set; }                //工厂
        public string remark { get; set; }              //备注
    }
}
