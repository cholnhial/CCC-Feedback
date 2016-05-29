using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCC_Feedback_DB
{
    public static class DatabaseStrings
    {
        #region globals
        public const string CONNECTION_STRING = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Feedbacks.accdb;Persist Security Info=True";
        public const string AFFECTED_ROW_IDENTITY = "SELECT @@Identity";
        #endregion

        #region Feedbacks
        public const string FEEDBACKS_TABLE = "tblJoirnals";

        public const string FEEDBACK_SELECT_ALL = "SELECT * FROM tblFeedbacks";
        public const string FEEDBACK_SELECT_ONE = "SELECT * FROM tblFeedbacks WHERE FeedbackID = {0} ";
        public const string FEEDBACK_SEARCH = "SELECT * FROM tblFeedbacks WHERE FirstName LIKE \"%{0}%\" AND FirstName <> \"\" AND LastName LIKE \"%{1}%\" AND LastName <> \"\" AND DateOfBirth >= #{2}#  OR CarrierID = {3}";

        public const string FEEDBACK_DELETE_ONE = "DELETE FROM tblFeedbacks WHERE FeedbackID = {0}";

        public const string FEEDBACK_INSERT = "INSERT INTO tblFeedbacks(CarrierID, FirstName, LastName, DateOfBirth, Description) VALUES({0}, \"{1}\", \"{2}\","
                                              +"\"{3}\", \"{4}\");";
                                                  
        public const string FEEDBACK_UPDATE = "UPDATE tblFeedbacks SET CarrierID = {0},  FirstName = \"{1}\", LastName = \"{2}\", "
                                              +" DateOfBirth = \"{3}\", Description = \"{4}\" WHERE FeedbackID = {5}";
             
        public const string FEEDBACK_ID = "FeedbackID";
        public const string FEEDBACK_FIRSTNAME = "FirstName";
        public const string FEEDBACK_LASTNAME = "LastName";
        public const string FEEDBACK_DATE_OF_BIRTH = "DateOfBirth";
        public const string FEEDBACK_DESCRIPTION = "Description";

        #endregion

        #region Carriers
        public const string CARRIERS_TABLE = "tblCarriers";

        public const string CARRIER_SELECT_ALL = "SELECT * FROM tblCarriers";
        public const string CARRIER_SELECT_ONE = "SELECT * FROM tblCarriers WHERE CarrierID = {0}";

        public const string CARRIER_DELETE_ONE = "DELETE FROM tblCarriers WHERE CarrierID = {0}";

        public const string CARRIER_INSERT = "INSERT INTO tblCarriers (Name) VALUES(\"{0}\")";
        public const string CARRIER_UPDATE = "UPDATE tblCarriers SET Name = \"{0}\" WHERE CarrierID = {1}";

        public const string CARRIER_ID = "CarrierID";
        public const string CARRIER_NAME = "Name";

        #endregion
    }
}
