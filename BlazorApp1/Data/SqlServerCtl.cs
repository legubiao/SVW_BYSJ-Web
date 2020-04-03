using BlazorApp1.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{  
    public static class SqlServerCtl
    {
        public static void AddEngineer(Engineer engineer)
        {
            string command = "INSERT INTO 维护工程师(ID,姓名,车间,组别) VALUES("+engineer.ID+",'"+engineer.Name+"','"+engineer.Workshop+"','"+engineer.Group+"')";
            insetData(command);
        }

        public static int newEngineerID()
        {
            string command = "select Max(ID) ID from 维护工程师";
            SqlDataReader reader = readData(command);
            reader.Read();
            int ID = (int)reader["ID"]+1;
            return ID;
        }

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
