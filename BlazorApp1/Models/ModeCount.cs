using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SVW_BYSJ_WEB.Models
{
    public class ModeCount
    {
        public string FailureMode { get; set; }                   //失效模式
        public string location { get; set; }                      //图片存放地址
        public int finishedCount { get; set; }                    //已完成维修单数量
        public int totalCount { get; set; }                       //总维修单数量
    }
}
