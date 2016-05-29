using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCC_Feedback_DB
{
    public class Feedback
    {
        #region vars
        public int feedbackID { get; set; }
        public int carrierID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Nullable<DateTime> dateOfBirth { get; set; }
        public string description { get; set; }
        #endregion

        #region init
        public Feedback()
        {
            feedbackID = 0; // Indicates new object
            firstName = lastName = description = "";
            dateOfBirth = null; // 1/1/1970
        }
        #endregion 

        #region instance methods
        public void Save()
        {
            if(feedbackID == 0)
            {
                feedbackID = Insert();
            }
        }
        
        public int Delete()
        {
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            return dbAccess.ExecuteNonQuery(String.Format(DatabaseStrings.FEEDBACK_DELETE_ONE, feedbackID));
        }

        private int Insert()
        {
            DatabaseAccess dbAccess = DatabaseAccess.Instance;

            return dbAccess.ExecuteNonQuery(String.Format(DatabaseStrings.FEEDBACK_INSERT, carrierID, firstName,
                lastName, dateOfBirth.ToString(), description));
        }

        private int Update()
        {
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            return dbAccess.ExecuteNonQuery(String.Format(DatabaseStrings.FEEDBACK_UPDATE, carrierID, firstName,
              lastName, dateOfBirth.ToString(), description));
        }
        #endregion

        #region static methods
        public static List<Feedback> GetEntries()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            OleDbCommand dbCmd = dbAccess.GetCommand(DatabaseStrings.FEEDBACK_SELECT_ALL);
            
           using(OleDbDataReader reader = dbCmd.ExecuteReader())
           {
               while(reader.Read())
               {
                   Feedback feedback = new Feedback();
                   feedback.feedbackID = (int)reader[DatabaseStrings.FEEDBACK_ID];
                   feedback.carrierID = (int) reader[DatabaseStrings.CARRIER_ID];
                   feedback.firstName = reader[DatabaseStrings.FEEDBACK_FIRSTNAME].ToString();
                   feedback.lastName = reader[DatabaseStrings.FEEDBACK_LASTNAME].ToString();
                   feedback.dateOfBirth = (DateTime)reader[DatabaseStrings.FEEDBACK_DATE_OF_BIRTH];
                   feedback.description = reader[DatabaseStrings.FEEDBACK_DESCRIPTION].ToString();

                   feedbacks.Add(feedback);

               }

               return feedbacks;
           }
         }

        public static List<Feedback> SearchEntries(string firstName, string lastName, DateTime dateOfBirth, int topicID)
        {
            List<Feedback> feedbacks = new List<Feedback>();
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            string query = String.Format(DatabaseStrings.FEEDBACK_SEARCH, firstName, lastName, dateOfBirth.ToString(), topicID);
            OleDbCommand dbCmd = dbAccess.GetCommand(query);

            using (OleDbDataReader reader = dbCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Feedback feedback = new Feedback();
                    feedback.feedbackID = (int)reader[DatabaseStrings.FEEDBACK_ID];
                    feedback.carrierID = (int)reader[DatabaseStrings.CARRIER_ID];
                    feedback.firstName = reader[DatabaseStrings.FEEDBACK_FIRSTNAME].ToString();
                    feedback.lastName = reader[DatabaseStrings.FEEDBACK_LASTNAME].ToString();
                    feedback.dateOfBirth = (DateTime)reader[DatabaseStrings.FEEDBACK_DATE_OF_BIRTH];
                    feedback.description = reader[DatabaseStrings.FEEDBACK_DESCRIPTION].ToString();

                    feedbacks.Add(feedback);

                }

                return feedbacks;
            }
        }

        #endregion

    }
}
