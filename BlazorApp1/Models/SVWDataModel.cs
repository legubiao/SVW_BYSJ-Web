using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SVW_BYSJ_WEB.Models
{
    public class SVWDataModel
    {
        public double ID { get; set; }                          //唯一的ID

        public double ProgramNo { get; set; }                   //程序号
        public string result { get; set; }                      //结果
        public double cycleTiem { get; set; }                   //循环时间（ms）
        public double workTime { get; set; }                    //涂胶时间（ms）
        public string volume { get; set; }                      //设定容量（ccm）
        public string realVolume { get; set; }                  //测量的容量（ccm）
        public string volumeBias { get; set; }                  //容量偏差（ccm）
        public double meanPreasure { get; set; }                //平均压力（bar）
        public double maxPressure { get; set; }                 //最高压力（bar）
        public double pressureIndex { get; set; }               //形成预压时的压缩系数（%）
        public double meanMoment { get; set; }                  //平均力矩（%）
    }
}
