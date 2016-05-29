using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCC__Feedback.classes;
using CCC_Feedback_DB;

namespace CCC__Feedback.forms
{
    public partial class frmSearch : Form
    {
        #region vars
        private DataTable searchResultsTable;
        private FeedbackManager feedbackManager;
        #endregion vars

        #region init
        public frmSearch(FeedbackManager _feedbackManager)
        {
            InitializeComponent();
            feedbackManager = _feedbackManager;
            searchResultsTable = new DataTable();

            cboCarrier.DataSource = feedbackManager.GetCarriersAsDataSet().Tables[0];
            cboCarrier.DisplayMember = "Name";
            cboCarrier.ValueMember = "CarrierID";

            // Add Columns to search results data grid view
            string[] columnNames = {"", "First Name", "Last Name", "DOB", "Carrier", "Description"};
            foreach(var columnName in columnNames)
            {
              searchResultsTable.Columns.Add(columnName);
            }

        }
        #endregion init

        #region events
        private void frmSearch_Load(object sender, EventArgs e)
        {
            dgvSearchResults.DataSource = searchResultsTable;

            // Disable sorting
            foreach (DataGridViewColumn column in dgvSearchResults.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // The first column should be hidden, it's just for identifying which 
            // feedback is selected and then displaying it on the main window
            dgvSearchResults.Columns[0].Visible = false;

            // Disable the 'View Selected' button in the mean time
            EnableOrDisableViewSelectedButton();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            List<Feedback> results = feedbackManager.SearchFeedbacks(txtFirstName.Text, txtLastName.Text, 
                dtpDob.Value, (int) cboCarrier.SelectedValue);

            searchResultsTable.Rows.Clear();
           foreach(var feedback in results)
            {
                string[] row = new string[6];


                row[0] = feedback.feedbackID.ToString();
                row[1] = feedback.firstName;
                row[2] = feedback.lastName;
                row[3] = ((DateTime) feedback.dateOfBirth).ToString("dd/MM/yyyy");
                row[4] = feedbackManager.GetCarrierNameByID(feedback.carrierID);
                row[5] = feedback.description;

                searchResultsTable.Rows.Add(row);

            }

            EnableOrDisableViewSelectedButton();
        }


        private void btnViewSelected_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion events

        #region methods
        /// <summary>
        /// Returns the primary key of the selected feedback
        /// </summary>
        /// <returns>The unique id</returns>
        public int GetSelectedFeedbackID()
        {
            return int.Parse(dgvSearchResults.SelectedRows[0].Cells[0].Value.ToString());
        }

        /// <summary>
        /// To avoid the application having unintended consequence the 'View Selected' button
        /// will be disabled when no results are in the data grid view
        /// </summary>
        private void EnableOrDisableViewSelectedButton()
        {
            if(searchResultsTable.Rows.Count == 0)
            {
                btnViewSelected.Enabled = false;
            } else
            {
                btnViewSelected.Enabled = true;
            }
        }
        #endregion methods

    }
}
