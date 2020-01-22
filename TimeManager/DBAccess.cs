using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager
{
    class DBAccess
    {
        private static DBAccess _instance;
        private const int _nmbOfQrtsInWeek = 672;
        private const string _targetTableName = "Quarters";

        private DBAccess()
        {
            int result;
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "SELECT COUNT(*) FROM sqlite_master\n" +
                                 "WHERE type='table' AND name='Quarters'";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        dataReader.Read();
                        result = dataReader.GetInt32(0);
                    }
                }
            }

            if(result==0)
            { 
            queryString = "CREATE TABLE Quarters (id INTEGER, Plan TEXT, Report TEXT, PRIMARY KEY(id))";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                    {
                        command.ExecuteNonQuery();                        
                    }
                }
            }

        }

        public static DBAccess GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DBAccess();
            }
            return _instance;
        }

        public bool CheckWeekInDB(int week)
        {
            int firstQrtOfWeekId = _nmbOfQrtsInWeek * (week - 1);
            int lastQrtOfWeekId = firstQrtOfWeekId + _nmbOfQrtsInWeek - 1;
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "SELECT COUNT(Id)\n" +
                                 "FROM Quarters\n" +
                                 "WHERE Id BETWEEN " + firstQrtOfWeekId + " AND " + lastQrtOfWeekId + ";";
            int recordsNumber;

            using (SQLiteConnection connection = new SQLiteConnection(
               connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(queryString, connection))
                {
                    try
                    {
                        using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                        {
                            dataReader.Read();
                            recordsNumber = dataReader.GetInt32(0);
                        }
                    }
                    catch (SQLiteException)
                    {
                        return false;
                    }
                }
            }
            if (recordsNumber != _nmbOfQrtsInWeek)
                return false;
            else
                return true;
        }

        public int CountActivityAppearences(Activity activity, QrtrsMrkngMode markingKind, int firstQuarter, int lastQuarter)
        {
            int result;
            string connectionString = "Data Source=TimeManagerDB.db";
            string column = markingKind == QrtrsMrkngMode.Planning ? "Plan" : "Report";
            string queryString = "SELECT COUNT(*) FROM " + _targetTableName + "\n" +
                                 "WHERE Id BETWEEN " + firstQuarter + " AND " + lastQuarter + "\n" +
                                 "AND " + column + " = '" + activity.Name + "';";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        dataReader.Read();
                        result = dataReader.GetInt32(0);
                    }
                }
            }
            return result;
        }

        public void PrepareDefaultWeekContnent(int week)
        {
            int firstQrtOfWeekId = _nmbOfQrtsInWeek * (week - 1);
            int lastQrtOfWeekId = firstQrtOfWeekId + _nmbOfQrtsInWeek - 1;
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
                    command.ExecuteNonQuery();
                    for (int i = firstQrtOfWeekId; i <= lastQrtOfWeekId; i++)
                    {
                        command.CommandText = "INSERT INTO " + _targetTableName + "(id, Plan, Report)" +
                                        "VALUES(" + i + ", 'Nothing', 'Nothing');";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch(SQLiteException)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        public void ReadData(int week, string[] plan, string[] report)
        {
            int firstQrtOfWeekId = _nmbOfQrtsInWeek * (week - 1);
            int lastQrtOfWeekId = firstQrtOfWeekId + _nmbOfQrtsInWeek - 1;
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "SELECT * FROM " + _targetTableName + "\n" +
                                 "WHERE Id BETWEEN " + firstQrtOfWeekId + " AND " + lastQrtOfWeekId + ";";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                        for (int i = 0; dataReader.Read(); i++)
                        {
                            if (!dataReader.IsDBNull(1)) plan[i] = dataReader.GetString(1);
                            else plan[i] = "Nothing";
                            if (!dataReader.IsDBNull(2)) report[i] = dataReader.GetString(2);
                            else report[i] = "Nothing";
                        }
                }
            }
        }

        public void SaveData(int week, string[] plan, string[] report)
        {
            int firstQrtOfWeekId = _nmbOfQrtsInWeek * (week - 1);
            int lastQrtOfWeekId = firstQrtOfWeekId + _nmbOfQrtsInWeek - 1;
            string connectionString = "Data Source=TimeManagerDB.db";
            string queryString = "";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
                    int arrayIndex = 0;
                    for (int qrtIndex = firstQrtOfWeekId; qrtIndex <= lastQrtOfWeekId; qrtIndex++)
                    {
                        queryString = "UPDATE " + _targetTableName + "\n" +
                                        "SET Plan = '" + plan[arrayIndex] + "', Report = '" + report[arrayIndex] + "'\n" +
                                        "WHERE id = " + qrtIndex + ";";
                        command.CommandText = queryString;
                        command.ExecuteNonQuery();
                        arrayIndex++;
                    }
                }
            }
        }
    }
}
