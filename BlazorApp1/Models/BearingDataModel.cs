using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SVW_BYSJ_WEB.Models
{
    public class BearingDataModel
    {
        public double ID { get; set; }
        public double label { get; set; }
        public double mean { get; set; }
        public double std { get; set; }
        public double var { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double median { get; set; }
        public double argmax { get; set; }
        public double skew { get; set; }
        public double kuri { get; set; }
    }
}
