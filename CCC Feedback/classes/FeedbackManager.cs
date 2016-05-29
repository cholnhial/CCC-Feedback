using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCC_Feedback_DB;
using System.Data;

namespace CCC__Feedback.classes
{   
    public enum FeedbackSortType { CARRIER, FIRST_NAME, LAST_NAME, DOB };

    public class FeedbackManager
    {

        #region vars
        private List<Feedback> feedbacks;
        private DatabaseAccess dbAccess;
        #endregion vars


        #region init
        public FeedbackManager()
        {

            feedbacks = new List<Feedback>(); // Empty feedback list
            dbAccess = DatabaseAccess.Instance;
            LoadFeedbacks();
        }
        #endregion init

        #region methods

        /// <summary>
        /// Returns all the feedbacks
        /// </summary>
        /// <returns></returns>
        public List<Feedback> GetFeedbacks()
        {
            return feedbacks;
        }

        public List<Feedback> SearchFeedbacks(string firstName, string lastName, DateTime dateOfBirth, int topicID)
        {
            return Feedback.SearchEntries(firstName, lastName, dateOfBirth, topicID);
        }

        public DataSet GetCarriersAsDataSet()
        {
            return Carrier.GetCarriersAsDataSet();
        }

        /// <summary>
        /// Returns the number of feedbacks in the list
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfFeedbacks()
        {
            return feedbacks.Count;
        }

        /// <summary>
        ///  Save feedback to database and list
        ///  
        /// </summary>
        /// <param name="feedback"></param>
        public void AddFeedback(Feedback feedback)
        {
            feedback.Save();
            feedbacks.Add(feedback);
        }

        /// <summary>
        ///  Save feedback to database
        /// </summary>
        /// <param name="feedback"></param>
        public void SaveFeedback(Feedback feedback)
        {
            feedback.Save();
        }


        /// <summary>
        ///  Removes a feedback from the list and also deletes 
        ///  and also from the datbase
        /// </summary>
        /// <param name="feedbackIndex"></param>
        public void DeleteFeedback(int feedbackIndex)
        {
            if(feedbacks.Count > 0)
            {
                feedbacks[feedbackIndex].Delete();
            }
        }

        /// <summary>
        /// Saves a carrier to the database
        /// </summary>
        /// <param name="carrier"></param>
        public void SaveCarrier(Carrier carrier)
        {
            carrier.Save();
        }

        /// <summary>
        /// Checks wether a carrier exists already
        /// </summary>
        /// <param name="carrierName"></param>
        /// <returns></returns>
        public bool CarrierExists(string carrierName)
        {
            return Carrier.CarrierExists(carrierName);
        }

        /// <summary>
        /// Sorts the list by a specific field specify by the user this can
        /// either be First Name, Last Name, or the DOB of a caller. A long
        /// with the field to sort the caller must specify the direction of the 
        /// sort either ascending or descending.
        /// </summary>
        /// <param name="feedbackSortType">The field which the list is to be sorted (ordered) by</param>
        /// <param name="sortDirection">The direction of the sort (ascending or descending)</param>
        public void Sort(FeedbackSortType feedbackSortType, ListSortDirection sortDirection)
        {   

            if(feedbackSortType == FeedbackSortType.FIRST_NAME)
            {
                if(sortDirection == ListSortDirection.Ascending)
                    feedbacks = feedbacks.OrderBy(o => o.firstName).ToList();
                else
                    feedbacks = feedbacks.OrderByDescending(o => o.firstName).ToList();
            }

            if (feedbackSortType == FeedbackSortType.LAST_NAME)
            {
                if (sortDirection == ListSortDirection.Ascending)
                    feedbacks = feedbacks.OrderBy(o => o.lastName).ToList();
                else
                    feedbacks = feedbacks.OrderByDescending(o => o.lastName).ToList();
            }
            if (feedbackSortType == FeedbackSortType.DOB)
            {
                if (sortDirection == ListSortDirection.Ascending)
                    feedbacks = feedbacks.OrderBy(o => o.dateOfBirth).ToList();
                else
                    feedbacks = feedbacks.OrderByDescending(o => o.dateOfBirth).ToList();
            }

        }

        public string GetCarrierNameByID(int id)
        {
            return Carrier.GetCarrier(id).name;
        }

        /// <summary>
        /// Loads all the feedbacks from a list if any by deserializing them into Feedback object,
        /// callers are advice to watch out for exceptions as file IO 
        /// exceptions might occur. 
        /// </summary>
        public void LoadFeedbacks()
        {
            feedbacks = Feedback.GetEntries();
        }

        public void CloseDB()
        {
            DatabaseAccess dbAccess = DatabaseAccess.Instance;
            dbAccess.Close();
        }
        #endregion methods
    }
}
