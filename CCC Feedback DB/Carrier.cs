using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCC_Feedback_DB
{
    public class Carrier
    {
        #region vars
        public int carrierID { get; set; }
        public string name { get; set; }
        #endregion

        #region init
        public Carrier()
        {
            carrierID = 0; // identifies new carrier
            name = "";
        }
        #endregion

        #region instance methods
         public void Save()
        {
            if(carrierID == 0)
            {
                carrierID = Insert();
            } else
            {
                Update();
            }
        }

        private int Insert()
        {
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            return dbAccess.ExecuteNonQuery(String.Format(DatabaseStrings.CARRIER_INSERT, name));
        }

        private int Update()
        {
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            return dbAccess.ExecuteNonQuery(String.Format(DatabaseStrings.CARRIER_UPDATE, name));
        }
        #endregion

        #region static methods
        public static List<Carrier> GetEntries()
         {
            List<Carrier> carriers = new List<Carrier>();
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            OleDbCommand dbCmd = dbAccess.GetCommand(DatabaseStrings.CARRIER_SELECT_ALL);

            using (OleDbDataReader reader = dbCmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    Carrier carrier = new Carrier();
                    carrier.carrierID = (int) reader[DatabaseStrings.CARRIER_ID];
                    carrier.name = reader[DatabaseStrings.CARRIER_NAME].ToString();
                    carriers.Add(carrier);
                }
            }

            return carriers;
         }
         
         public static DataSet GetCarriersAsDataSet()
        {
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            OleDbConnection dbConn = dbAccess.GetConnection();
            OleDbDataAdapter adapter = new OleDbDataAdapter(DatabaseStrings.CARRIER_SELECT_ALL, dbConn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            return ds;
        }

        public static bool CarrierExists(String carrierName)
        {
            List<Carrier> carriers = GetEntries();
            foreach(Carrier carrier in carriers)
            {
                if(String.Equals(carrierName, carrier.name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        public static Carrier GetCarrier(int id)
        {
            string query = String.Format(DatabaseStrings.CARRIER_SELECT_ONE, id);
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            OleDbCommand dbCmd = dbAccess.GetCommand(query);

            Carrier carrier = null;

            OleDbDataReader reader = dbCmd.ExecuteReader();
            if (reader.Read())
            {
                carrier = new Carrier();

                carrier.carrierID = (int)reader[DatabaseStrings.CARRIER_ID];
                carrier.name = reader[DatabaseStrings.CARRIER_NAME].ToString();
            }

            return carrier;
        }
        #endregion
    }
}
