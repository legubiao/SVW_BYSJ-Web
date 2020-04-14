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
        public static IList<Engineer> ListEngineer( bool isOn)
        {
            IList<Engineer> onListEngineer = new List<Engineer>();
            string command;
            if (isOn){
                command = "select * from 在职维护工程师";
            }
            else
            {
                command = "select* from 离职维护工程师";
            }

            SqlDataReader reader = readData(command);
            while (reader.Read())
            {
                Engineer engineer = new Engineer();
                engineer.ID = (int)reader["ID"];
                engineer.Name = (string)reader["姓名"];
                engineer.Workshop = (workshop)Enum.Parse(typeof(workshop),(string)reader["车间"]);
                engineer.Group = (group)Enum.Parse(typeof(group), (string)reader["组别"]);
                onListEngineer.Add(engineer);
            }
            return onListEngineer;
        }       
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

        //添加工程师
        //对于工程师的“是否在职”，1为在职，0为离职
        public static void AddEngineer(Engineer engineer)
        {
            string command = "INSERT INTO 维护工程师(ID,姓名,车间,组别,是否在职) VALUES("+engineer.ID+",'"+engineer.Name+"','"+engineer.Workshop+"','"+engineer.Group+ "','" + 1 + "')";
            insetData(command);
        }
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
            insetData(command);
        }

        //获取新建工程师的ID
        public static int newEngineerID()
        {
            string command = "select Max(ID) ID from 维护工程师";
            SqlDataReader reader = readData(command);
            reader.Read();
            int ID = (int)reader["ID"]+1;
            return ID;
        }

        //读取数据
        static SqlDataReader readData(string command)
        {
            SqlConnection sqlConn = new SqlConnection(getConnectionString());
            sqlConn.Open();
            SqlCommand sqlComm = new SqlCommand(command, sqlConn);
            return sqlComm.ExecuteReader();
        }

        static void insetData(string command)
        {
            SqlConnection sqlConn = new SqlConnection(getConnectionString());
            sqlConn.Open();
            SqlCommand sqlComm = new SqlCommand(command, sqlConn);
            sqlComm.ExecuteNonQuery();
        }

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
