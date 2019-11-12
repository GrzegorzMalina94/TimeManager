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

        public void SaveData(string[] plan, string[] report)
        {
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "DELETE FROM Quarters WHERE id >= 0";
            using (SQLiteConnection connection = new SQLiteConnection(
               connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

                for (int i = 0; i < plan.Length; i++)
                {
                    queryString = "INSERT INTO Quarters (id, Plan, Report) VALUES (" + i.ToString() + ", '" + plan[i] + "', '" + report[i] +"')";
                    command.CommandText = queryString;
                    command.ExecuteNonQuery();
                }
            }

        }

        public void ReadData(string[] plan, string[] report)
        {
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "SELECT * FROM Quarters WHERE id <= 671";
            SQLiteDataReader dataReader;

            using (SQLiteConnection connection = new SQLiteConnection(
               connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                command.Connection.Open();
                dataReader = command.ExecuteReader();

                for(int i = 0;  dataReader.Read(); i++)
                {
                    if (!dataReader.IsDBNull(1)) plan[i] = dataReader.GetString(1);
                    else plan[i] = "Nothing";
                    if (!dataReader.IsDBNull(2)) report[i] = dataReader.GetString(2);
                    else report[i] = "Nothing";
                }
            }
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
