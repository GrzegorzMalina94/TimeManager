using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TimeManager.DB
{
    class Access
    {
        private static Access _instance;
        private SQLiteDataAdapter m_oDataAdapter = null;
        private DataSet m_oDataSet = null;
        private DataTable m_oDataTable = null;

        private Access()
        {

        }

        public static Access GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Access();
            }
            return _instance;
        }

        public void SaveData(string[] values)
        {
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "delete from Quarters where id >= 0";
            using (SQLiteConnection connection = new SQLiteConnection(
               connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

                for (int i = 0; i < values.Length; i++)
                {
                    queryString = "Insert into Quarters (id, Activity) values (" + i.ToString() + " ,'" + values[i] + "')";
                    command.CommandText = queryString;
                    command.ExecuteNonQuery();
                }
            }

        }

        public string[] ReadData()
        {
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "SELECT * FROM Quarters WHERE id <= 671";
            SQLiteDataReader dataReader;
            string[] obtainedData = new string[672];

            using (SQLiteConnection connection = new SQLiteConnection(
               connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                command.Connection.Open();
                dataReader = command.ExecuteReader();

                for(int i = 0;  dataReader.Read(); i++)
                {
                    obtainedData[i] = dataReader.GetString(1);
                }
            }

            return obtainedData;
        }

        public void On_Window_Closing()
        {
            if (null != m_oDataAdapter)
            {
                m_oDataAdapter.Dispose();
                m_oDataAdapter = null;
            }
        }
    }
}
