using SVW_BYSJ_WEB.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SVW_BYSJ_WEB.Models.Engineer;
using static SVW_BYSJ_WEB.Models.SparePart;

namespace SVW_BYSJ_WEB.Data
{  
    public static class SqlServerCtl
    {
        //获取新建工程师的ID
        public static int newEngineerID()
        {
            string command = "select Max(ID) ID from 维护工程师";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                reader.Read();
                int ID = (int)reader["ID"] + 1;
                return ID;
            }            
        }
        //列出工程师
        public static IList<Engineer> ListEngineer( bool isOn)
        {
            IList<Engineer> onListEngineer = new List<Engineer>();
            string command;
            if (isOn){
                command = "select * from 工程师详细表 where 是否在职='True'";
            }
            else
            {
                command = "select * from 工程师详细表 where 是否在职='False'";
            }

            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    Engineer engineer = new Engineer();
                    engineer.ID = (int)reader["ID"];
                    engineer.Name = (string)reader["姓名"];
                    engineer.Workshop = (workshop)Enum.Parse(typeof(workshop), (string)reader["车间"]);
                    engineer.Group = (group)Enum.Parse(typeof(group), (string)reader["工厂"]);
                    engineer.onWork = (bool)reader["是否在职"];
                    engineer.RepairTimes = (int)reader["维护次数"];
                    engineer.MaintainTimes = (int)reader["保养次数"];
                    onListEngineer.Add(engineer);
                }
                return onListEngineer;
            }            
        }       
        //列出可用工程师的名单
        public static List<string> ListEngineerName(bool MaintainOrRepair)
        {
            List<string> nameList = new List<string>();
            string command;
            if (MaintainOrRepair)
            {
                command = "select 姓名 from 工程师详细表 where 是否在职='True' AND 维护次数 > 0";
            }
            else
            {
                command = "select 姓名 from 工程师详细表 where 是否在职='True' AND 保养次数 > 0";
            }

            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    nameList.Add((string)reader["姓名"]);
                }
                return nameList;
            }
            
        }
        //添加工程师
        public static void AddEngineer(Engineer engineer)
        {
            string command = "INSERT INTO 维护工程师(ID,姓名,车间,工厂,是否在职) " +
                "VALUES("+engineer.ID+",'"+engineer.Name+"','"+engineer.Workshop+"','"+engineer.Group+ "','" + 1 + "')";
            ManipulateData(command);
        }

        //搜寻备件记录
        public static IList<SparePart> SearchParts(string keyword,bool isDescription)
        {
            List<SparePart> spareParts = new List<SparePart>();
            string command;
            if (isDescription)
            {
                command = "select * from 备件信息与库存表 where [中/英文描述/规格型] like '%" + keyword + "%'";
            }
            else
            {
                command = "select * from 备件信息与库存表 where SVW物料号 like '%" + keyword + "%'";
            }
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    SparePart sparePart = new SparePart();
                    sparePart.CreatDate = (DateTime)reader["物料生成日期"];
                    sparePart.SVWNumber = (string)reader["SVW物料号"];
                    sparePart.DCNumber = (string)reader["DC物料号"];
                    sparePart.Description = (string)reader["中/英文描述/规格型"];
                    sparePart.PartProducer = (string)reader["备件制造商"];
                    sparePart.DeviceProducer = (string)reader["设备制造商"];
                    sparePart.MachineNumber = (string)reader["设备编号（机器号）"];
                    sparePart.MapNumber = (string)reader["图号"];
                    sparePart.CountUnit = (partUnit)Enum.Parse(typeof(partUnit), (string)reader["计量单位"]);
                    sparePart.ABCNumber = (ABCnumber)Enum.Parse(typeof(ABCnumber), (string)reader["ABC码"]);
                    sparePart.PlannerNumber = (string)reader["计划员码"];
                    sparePart.price = (double)reader["参考单价"];
                    sparePart.MinSafetyStock = (int)reader["最小安全库存"];
                    sparePart.MaxSafetyStock = (int)reader["最大安全库存"];
                    sparePart.OrderCycle = (int)reader["采购周期"];
                    sparePart.PartProperty = (partProperty)Enum.Parse(typeof(partProperty), (string)reader["物料属性"]);
                    sparePart.PartType = (partType)Enum.Parse(typeof(partType), (string)reader["物料类型"]);
                    sparePart.PartStatus = (partStatus)Enum.Parse(typeof(partStatus), (string)reader["物料状态"]);
                    sparePart.IsSafety = (bool)reader["物料安全标识"];
                    sparePart.ProducingArea = (producingArea)Enum.Parse(typeof(producingArea), (string)reader["产地"]);
                    sparePart.SparePartNo= (int)reader["备件库存"];
                    sparePart.ReWorkNo = (int)reader["返修件库存"];
                    spareParts.Add(sparePart);
                }
                return spareParts;
            }
            
        }
        //添加备件记录
        public static void AddPartInfo(SparePart sparePart)
        {
            string command = "Insert into 备件信息表(物料生成日期,DC物料号,SVW物料号,[中/英文描述/规格型]," +
                "备件制造商,设备制造商,[设备编号（机器号）],图号,计量单位,ABC码,计划员码,最小安全库存," +
                "最大安全库存,采购周期,物料属性,物料安全标识,产地,参考单价)Values('"
                + DateTime.Now + "','" + sparePart.DCNumber + "','" + sparePart.SVWNumber + "','"
                + sparePart.Description + "','" + sparePart.PartProducer + "','" + sparePart.DeviceProducer + "','"
                + sparePart.MachineNumber + "','" + sparePart.MapNumber + "','" + sparePart.CountUnit + "','"
                + sparePart.ABCNumber + "','" + sparePart.PlannerNumber + "','" + sparePart.MinSafetyStock + "','"
                + sparePart.MaxSafetyStock + "','" + sparePart.OrderCycle + "','" + sparePart.PartProperty + "','"
                + sparePart.IsSafety + "','" + sparePart.ProducingArea +"','"+sparePart.price+ "')";
            ManipulateData(command);
        }
        //删除备件记录
        public static void DeletePartInfo(String SVWNumber)
        {
            string command = "Delete from 备件信息表 where SVW物料号 = '" + SVWNumber + "'";
            ManipulateData(command);
        }
        //修改备件的部分数据
        public static void UpdatePartInfo(SparePart sparePart)
        {
            string command = "UPDATE 备件信息表 SET [中/英文描述/规格型] = '" + sparePart.Description+
                "',设备制造商='"+sparePart.DeviceProducer+
                "',备件制造商='"+sparePart.PartProducer+
                "',[设备编号（机器号）]='" + sparePart.MachineNumber+
                "',图号='"+sparePart.MapNumber+
                "',计划员码='" + sparePart.PlannerNumber +
                "',参考单价='" + sparePart.price +
                "',最小安全库存='" + sparePart.MinSafetyStock +
                "',最大安全库存='" + sparePart.MaxSafetyStock +
                "',采购周期='" + sparePart.OrderCycle +
                "',物料属性='" + sparePart.PartProperty +
                "' WHERE SVW物料号 = '" + sparePart.SVWNumber + "'";
            ManipulateData(command);
        }
        //切换备件的安全或不安全状态
        public static void SwitchPartSafety(SparePart sparePart)
        {
            string command;
            if (sparePart.IsSafety)
            {
                command = "UPDATE 备件信息表 SET 物料安全标识 = 'False' WHERE SVW物料号 = '" + sparePart.SVWNumber + "'";
            }
            else
            {
                command = "UPDATE 备件信息表 SET 物料安全标识 = 'True' WHERE SVW物料号 = '" + sparePart.SVWNumber + "'";
            }
            ManipulateData(command);
        }
        public static Dictionary<string, int> GetPartTypeIndex()
        {
            Dictionary<string, int> partTypeIndex = new Dictionary<string, int>();
            string command = "select 物料类型,count(物料类型)as 次数 from 备件信息表 group by 物料类型";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    partTypeIndex.Add((string)reader["物料类型"], (int)reader["次数"]);
                }
            }
            return partTypeIndex;
        }
        public static Dictionary<string, int> GetPartPropertyIndex()
        {
            Dictionary<string, int> partTypeIndex = new Dictionary<string, int>();
            string command = "select 物料属性,count(物料属性)as 次数 from 备件信息表 group by 物料属性";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    partTypeIndex.Add((string)reader["物料属性"], (int)reader["次数"]);
                }
            }
            return partTypeIndex;
        }
        public static Dictionary<string, int> GetPartStatusIndex()
        {
            Dictionary<string, int> partTypeIndex = new Dictionary<string, int>();
            string command = "select 物料状态,count(物料状态)as 次数 from 备件信息表 group by 物料状态";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    partTypeIndex.Add((string)reader["物料状态"], (int)reader["次数"]);
                }
            }
            return partTypeIndex;
        }
        //列出已完成维修单的计数表
        public static List<ModeCount> GetRecordCount()
        {
            List<ModeCount> summaryCount = new List<ModeCount>();
            string command = "select * from 故障单计数";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {               
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    ModeCount modeCount = new ModeCount();
                    modeCount.FailureMode = (String)reader["故障类型"];
                    modeCount.finishedCount = (int)reader["已完成故障单次数"];
                    modeCount.totalCount = (int)reader["总故障发生次数"];
                    string picture;
                    switch (modeCount.FailureMode)
                    {
                        case "PLC故障": picture = "PLC"; break;
                        case "传输系统故障": picture = "Trans"; break;
                        case "切割设备": picture = "Cutter"; break;
                        case "夹具故障": picture = "Fixer"; break;
                        case "定位设备": picture = "Locate"; break;
                        case "机器人故障": picture = "Robot"; break;
                        case "机械断裂": picture = "Machine"; break;
                        case "涂胶设备": picture = "Glu"; break;
                        case "激光设备故障": picture = "Laser"; break;
                        case "焊接设备": picture = "Weld"; break;
                        default: picture = "Others"; break;
                    }
                    modeCount.location = "../Image/"+picture+".jpg";
                    summaryCount.Add(modeCount);
                }
                return summaryCount;
            }    
        }
        //列出维修单
        public static IList<repairRecord> ListRepairRecord(bool isOn)
        {
            IList<repairRecord> recordList = new List<repairRecord>();
            string command;
            if (isOn)
            {
                command = "Select * from 维护记录 where 故障单完成情况 = 1";   //已完成
            }
            else
            {
                command = "Select * from 维护记录 where 故障单完成情况 = 0";   //未完成
            }

            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    repairRecord record = new repairRecord();
                    record.orderNumber = (string)reader["维修单号"];
                    record.maintainTime = (DateTime)reader["维护日期"];
                    record.Group = (group)Enum.Parse(typeof(group), (string)reader["工厂"]);
                    record.Workshop = (workshop)Enum.Parse(typeof(workshop), (string)reader["车间"]);
                    record.Area = (string)reader["区域"];
                    record.Station = (string)reader["工位"];
                    record.FailureMode = (failureMode)Enum.Parse(typeof(failureMode), (string)reader["故障类型"]);
                    record.FailureDetail = (string)reader["故障内容"];
                    record.RepairMeasures = (string)reader["修理措施"];
                    record.LongtimeMeasures = (string)reader["长期措施"];
                    record.MaintanenceTime = (int)reader["故障修理时间"];
                    record.RepairPeople = (string)reader["维修责任人"];
                    record.MaintanencePeople = (string)reader["保养责任人"];
                    record.ShutdownTime = (int)reader["影响主线时间"];
                    record.writtenby = (string)reader["填写人"];
                    record.writtenTime = (DateTime)reader["填写时间"];
                    record.SVWNumber = (string)reader["SVW物料号"];
                    record.SparePartNo = (int)reader["备件消耗数量"];
                    record.ReworkNo = (int)reader["返修件消耗数量"];
                    record.isFinished = (bool)reader["故障单完成情况"];
                    record.remark = (string)reader["备注"];
                    recordList.Add(record);
                }
                return recordList;
            }
            
        }        
        //添加维修单
        public static void AddRepairRecord(repairRecord record)
        {
            string command = "Insert into 维护记录(维修单号,维护日期,工厂,车间,区域,工位,故障类型,故障内容," +
                "修理措施,长期措施,故障修理时间,维修责任人,保养责任人,影响主线时间,填写人,填写时间,SVW物料号," +
                "备件消耗数量,返修件消耗数量,故障单完成情况,备注)Values('"
               + record.orderNumber + "','"
               + record.maintainTime + "','"
               + record.Group + "','"
               + record.Workshop + "','"
               + record.Area + "','"
               + record.Station + "','"
               + record.FailureMode + "','"
               + record.FailureDetail + "','"
               + record.RepairMeasures + "','"
               + record.LongtimeMeasures + "','"
               + record.MaintanenceTime + "','"
               + record.RepairPeople + "','"
               + record.MaintanencePeople + "','"
               + record.ShutdownTime + "','"
               + record.writtenby + "','"
               + DateTime.Now + "','"
               + record.SVWNumber + "','"
               + record.SparePartNo + "','"
               + record.ReworkNo + "','"
               + false + "','"
               + record.remark + "')";
            ManipulateData(command);
        }
        //切换维修单的完成与未完成状态
        public static void SwitchRecordStatus(repairRecord record)
        {
            string command;
            if (record.isFinished)
            {
                command = "UPDATE 维护记录 SET 故障单完成情况 = 'False' WHERE 维修单号 = '" + record.orderNumber + "'";
            }
            else
            {
                command = "UPDATE 维护记录 SET 故障单完成情况 = 'True' WHERE 维修单号 = '" + record.orderNumber + "'";
            }
            ManipulateData(command);
        }
        //列出特定失效模式的Summary
        public static IList<Strategy> ListStrategy(string FailureMode)
        {
            IList<Strategy> strategies = new List<Strategy>();
            string command1 = "select 故障内容,count(故障内容)as 记录条数 from 维护记录 where 故障类型 = '" + FailureMode + "' group by 故障内容";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command1, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    Strategy strategy = new Strategy();
                    strategy.FailureDetail = (string)reader["故障内容"];
                    strategy.strategyCount = (int)reader["记录条数"];
                    strategies.Add(strategy);
                }
                return strategies;
            }
        }
        public static IList<repairRecord> GetRecordBySummary(string FailureMode,string FailureDetail)
        {
            IList<repairRecord> recordList = new List<repairRecord>();
            string command = "select * from 维护记录 where 故障类型 = '" + FailureMode + "' AND 故障内容='" + FailureDetail + "'";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    repairRecord record = new repairRecord();
                    record.orderNumber = (string)reader["维修单号"];
                    record.maintainTime = (DateTime)reader["维护日期"];
                    record.Group = (group)Enum.Parse(typeof(group), (string)reader["工厂"]);
                    record.Workshop = (workshop)Enum.Parse(typeof(workshop), (string)reader["车间"]);
                    record.Area = (string)reader["区域"];
                    record.Station = (string)reader["工位"];
                    record.FailureMode = (failureMode)Enum.Parse(typeof(failureMode), (string)reader["故障类型"]);
                    record.FailureDetail = (string)reader["故障内容"];
                    record.RepairMeasures = (string)reader["修理措施"];
                    record.LongtimeMeasures = (string)reader["长期措施"];
                    record.MaintanenceTime = (int)reader["故障修理时间"];
                    record.RepairPeople = (string)reader["维修责任人"];
                    record.MaintanencePeople = (string)reader["保养责任人"];
                    record.ShutdownTime = (int)reader["影响主线时间"];
                    record.writtenby = (string)reader["填写人"];
                    record.writtenTime = (DateTime)reader["填写时间"];
                    record.SVWNumber = (string)reader["SVW物料号"];
                    record.SparePartNo = (int)reader["备件消耗数量"];
                    record.ReworkNo = (int)reader["返修件消耗数量"];
                    record.isFinished = (bool)reader["故障单完成情况"];
                    record.remark = (string)reader["备注"];
                    recordList.Add(record);
                }
                return recordList;
            }
        }
        public static string newRepairRecordNumber()
        {
            string command = "SELECT count(*)as count from 维护记录";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                reader.Read();
                int ID = (int)reader["count"] + 1000001;
                return "MD"+ID;
            }
        }


        public static void AddInStorageRecord(InStorageRecord record)
        {
            string command = "INSERT INTO 补货记录(入库单号,SVW物料号,入库日期,入库数量,填写人,填写日期,备注) " +
                "VALUES(" + record.InStorageNumber + ",'" 
                + record.SVWNumber + "','" 
                + record.InStorageTime + "','" 
                + record.InStorageCount + "','"
                + record.writtenby + "','"
                + DateTime.Now + "','"
                + record.remark +  "')";
            ManipulateData(command);
        }
        public static int newInStorageNumber()
        {
            string command = "select Max(入库单号) 入库单号 from 补货记录";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                reader.Read();
                int ID = (int)reader["入库单号"] + 1;
                return ID;
            }
        }

        public static void AddReWorkRecord(ReWorkRecord record)
        {
            string command = "INSERT INTO 返修件表(编号,工厂,SVW物料号,存放位置,补充数量,补充日期,填写人,填写日期,备注) " +
                "VALUES(" + record.ReWorkNumber + ",'"
                + record.Group + "','"
                + record.SVWNumber + "','"
                + record.Location + "','"
                + record.ReWorkCount + "','"
                + record.ReWorkTime + "','"
                + record.writtenby + "','"
                + DateTime.Now + "','"
                + record.remark + "')";
            ManipulateData(command);
        }
        public static int newReWorkNumber()
        {
            string command = "select Max(编号) 编号 from 返修件表";
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                SqlDataReader reader = sqlComm.ExecuteReader();
                reader.Read();
                int ID = (int)reader["编号"] + 1;
                return ID;
            }
        }


        //通用：更改数据
        static void ManipulateData(string command)
        {
            using (SqlConnection sqlConn = new SqlConnection(getConnectionString()))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand(command, sqlConn);
                sqlComm.ExecuteNonQuery();
            }
        }
        //通用：连接数据库
        private static string getConnectionString()
        {
            SqlConnectionStringBuilder sqlStr = new SqlConnectionStringBuilder();
            sqlStr.DataSource = "49.232.143.134";
            sqlStr.InitialCatalog = "SVW_bysj";
            sqlStr.UserID = "SA";
            sqlStr.Password = "ZslNB2020";
            return sqlStr.ToString();
        }
    }
}
