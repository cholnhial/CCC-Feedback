using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace CCC_Feedback_DB
{
 
    public class DatabaseAccess
    {
        #region vars
        private static DatabaseAccess theInstance = null;
        private OleDbConnection dbConnection;

        public static DatabaseAccess Instance
        {
            get
            {
                if (theInstance == null)
                    theInstance = new DatabaseAccess();
                return theInstance;
            }
        }
        #endregion

        #region init
        private DatabaseAccess()
        {
            dbConnection = new OleDbConnection(DatabaseStrings.CONNECTION_STRING);
            dbConnection.Open();
        }
        #endregion

        #region instance methods
        public OleDbConnection GetConnection()
        {
            return dbConnection;
        }

        public int ExecuteNonQuery(string query)
        {
            OleDbCommand dbCmd = new OleDbCommand(query, dbConnection);
            dbCmd.ExecuteNonQuery();
            dbCmd.CommandText = DatabaseStrings.AFFECTED_ROW_IDENTITY;

            return (int)dbCmd.ExecuteScalar();
        }

        public OleDbCommand GetCommand(string query)
        {
            return new OleDbCommand(query, dbConnection);
        }

        public void Close()
        {
            dbConnection.Close();
        }
        #endregion

    }
}
