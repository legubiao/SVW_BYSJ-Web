using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SVW_BYSJ_WEB.Models
{
    public class Engineer
    {
        public int ID { get; set; }                 //ID
        public string Name { get; set; }            //姓名
        public workshop Workshop { get; set; }      //车间
        public group Group { get; set; }            //工厂
        public bool onWork { get; set; }            //是否在职
        public int RepairTimes { get; set; }        //维护次数
        public int MaintainTimes { get; set; }      //保养次数


        public override string ToString()
        {
            return $"ID：{ID}， 姓名：{Name}，车间：{Workshop}，工厂：{Group}";
        }


    }
    public enum workshop
    {
        TC3B1,
        TC3B2,
        TC3B,
        PQ
    }

    public enum group
    {
        TC3B
    }
}
