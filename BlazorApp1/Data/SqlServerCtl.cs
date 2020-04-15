using BlazorApp1.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorApp1.Models.Engineer;
using static BlazorApp1.Models.SparePart;

namespace BlazorApp1.Data
{  
    public static class SqlServerCtl
    {
        //获取新建工程师的ID
        public static int newEngineerID()
        {
            string command = "select Max(ID) ID from 维护工程师";
            SqlDataReader reader = readData(command);
            reader.Read();
            int ID = (int)reader["ID"] + 1;
            return ID;
        }
        //列出工程师
        public static IList<Engineer> ListEngineer( bool isOn)
        {
            IList<Engineer> onListEngineer = new List<Engineer>();
            string command;
            if (isOn){
                command = "select * from 在职维护工程师";
            }
            else
            {
                command = "select * from 离职维护工程师";
            }

            SqlDataReader reader = readData(command);
            while (reader.Read())
            {
                Engineer engineer = new Engineer();
                engineer.ID = (int)reader["ID"];
                engineer.Name = (string)reader["姓名"];
                engineer.Workshop = (workshop)Enum.Parse(typeof(workshop),(string)reader["车间"]);
                engineer.Group = (group)Enum.Parse(typeof(group), (string)reader["工厂"]);
                onListEngineer.Add(engineer);
            }
            return onListEngineer;
        }       
        //添加工程师
        public static void AddEngineer(Engineer engineer)
        {
            string command = "INSERT INTO 维护工程师(ID,姓名,车间,工厂,是否在职) " +
                "VALUES("+engineer.ID+",'"+engineer.Name+"','"+engineer.Workshop+"','"+engineer.Group+ "','" + 1 + "')";
            ManipulateData(command);
        }

        //搜寻备件记录
        public static IList<SparePart> SearchParts(string keyword)
        {
            List<SparePart> spareParts = new List<SparePart>();
            string command = "select * from 备件信息表 where [中/英文描述/规格型] like '%" + keyword + "%'";
            SqlDataReader reader = readData(command);
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
                sparePart.PartProperty = (string)reader["物料属性"];
                sparePart.IsSafety = (bool)reader["物料安全标识"];
                sparePart.ProducingArea = (producingArea)Enum.Parse(typeof(producingArea), (string)reader["产地"]);

                spareParts.Add(sparePart);
            }
            return spareParts;
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

            SqlDataReader reader = readData(command);
            while (reader.Read())
            {
                repairRecord record = new repairRecord();
                record.orderNumber = (string)reader["维修单号"];
                record.maintainTime = (DateTime)reader["维护日期"];
                record.Group = (group)Enum.Parse(typeof(group),(string)reader["工厂"]);
                record.Workshop = (workshop)Enum.Parse(typeof(workshop),(string)reader["车间"]);
                record.Area = (string)reader["区域"];
                record.Station=(string)reader["工位"];
                record.FailureMode=(failureMode)Enum.Parse(typeof(failureMode),(string)reader["故障类型"]);
                record.FailureDetail=(string)reader["故障内容"];
                record.RepairMeasures=(string)reader["修理措施"];
                record.LongtimeMeasures=(string)reader["长期措施"];
                record.MaintanenceTime=(int)reader["故障修理时间"];
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

        //通用：读取数据
        static SqlDataReader readData(string command)
        {
            SqlConnection sqlConn = new SqlConnection(getConnectionString());
            sqlConn.Open();
            SqlCommand sqlComm = new SqlCommand(command, sqlConn);
            return sqlComm.ExecuteReader();
        }
        //通用：更改数据
        static void ManipulateData(string command)
        {
            SqlConnection sqlConn = new SqlConnection(getConnectionString());
            sqlConn.Open();
            SqlCommand sqlComm = new SqlCommand(command, sqlConn);
            sqlComm.ExecuteNonQuery();
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
