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
using CCC__Feedback.forms;
using CCC_Feedback_DB;

namespace CCC__Feedback
{
    public partial class frmMain : Form
    {

        #region vars
        private FeedbackManager feedbackManager;
        private int currentFeedbackIndex;
        private string[] sortTypesList = { "First Name", "Last Name", "DOB" };
        #endregion vars

        #region init
        public frmMain()
        {
            currentFeedbackIndex = 0;
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                feedbackManager = new FeedbackManager();
                LoadCarriers();
            }
            catch (Exception ex)
            {
                HandleCriticalException(ex);
                return;
            }

            if (feedbackManager.GetNumberOfFeedbacks() > 0)
            {
                ShowFirstFeedback();
            } else
            {
                ToggleControls(false);
            }

            
            cboSort.DataSource = sortTypesList;
            radBtnAscending.Checked = true;

        }
        #endregion init

        #region events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmFeedback frm = new frmFeedback(feedbackManager);
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.OK)
            {
                Feedback feedback = frm.GetEnteredFeedback();
                try
                {
                    feedbackManager.AddFeedback(feedback);
                }
                catch(Exception ex)
                {
                    HandleCriticalException(ex);
                    return;
                }

                ShowLastFeedback();
                ToggleControls(true);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {   

            // List begins at 0, so minus 1 prevents index overflow
            if(currentFeedbackIndex < feedbackManager.GetNumberOfFeedbacks() - 1)
            {
                currentFeedbackIndex++;
                ShowFeedback(currentFeedbackIndex);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if(currentFeedbackIndex > 0)
            {
                currentFeedbackIndex--;
                ShowFeedback(currentFeedbackIndex);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            ShowFirstFeedback();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            ShowLastFeedback();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
         
            if(currentFeedbackIndex > 0)
            {
                feedbackManager.DeleteFeedback(currentFeedbackIndex);
                currentFeedbackIndex--;
                ShowFeedback(currentFeedbackIndex);
            } else if(currentFeedbackIndex == 0)
            {
                feedbackManager.DeleteFeedback(currentFeedbackIndex);
                if (feedbackManager.GetNumberOfFeedbacks() > 0)
                {
                    ShowFeedback(currentFeedbackIndex);
                }
            }

            if(feedbackManager.GetNumberOfFeedbacks() == 0)
            {
                ToggleControls(false);
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            Dictionary<string, FeedbackSortType> sortTypes = new Dictionary<string, FeedbackSortType>()
            {
                {"First Name", FeedbackSortType.FIRST_NAME },
                {"Last Name", FeedbackSortType.LAST_NAME },
                {"DOB", FeedbackSortType.DOB }
            };

            string selectedSortType = sortTypesList[cboSort.SelectedIndex];
            FeedbackSortType sortType = sortTypes[selectedSortType];

            if(radBtnAscending.Checked)
            {
                feedbackManager.Sort(sortType, ListSortDirection.Ascending);
            } else
            {
                feedbackManager.Sort(sortType, ListSortDirection.Descending);
            }

            ShowFirstFeedback();
         
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                frmSearch frm = new frmSearch(feedbackManager);
                DialogResult res = frm.ShowDialog();
                if (res == DialogResult.OK)
                {
                    int selectedFeedbackID = frm.GetSelectedFeedbackID();
                    List<Feedback> feedbacks = feedbackManager.GetFeedbacks();

                    for (int i = 0; i < feedbacks.Count; i++)
                    {
                        if (feedbacks[i].feedbackID == selectedFeedbackID)
                        {
                            ShowFeedback(i);
                            break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                HandleCriticalException(ex);
            }
          
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Feedback feedback = feedbackManager.GetFeedbacks()[currentFeedbackIndex];

            feedback.firstName = txtFirstName.Text;
            feedback.lastName = txtFirstName.Text;
            feedback.dateOfBirth = dtpDob.Value;
            feedback.description = txtDescription.Text;
            feedbackManager.SaveFeedback(feedback);
        }
        #endregion events

        #region methods
        private void DisableUnwantedNavButtons()
        {
            if (currentFeedbackIndex == 0 && feedbackManager.GetNumberOfFeedbacks() > 1)
            {
                btnPrev.Enabled = false;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
                btnFirst.Enabled = false;
            }
            else if (currentFeedbackIndex == feedbackManager.GetNumberOfFeedbacks() - 1)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnPrev.Enabled = true;
                btnFirst.Enabled = true;
            }
            else if (feedbackManager.GetNumberOfFeedbacks() == 0)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            } else
            {
                btnNext.Enabled = true;
                btnLast.Enabled = true;
                btnPrev.Enabled = true;
                btnFirst.Enabled = true;
            }
        }

        private void ShowFirstFeedback()
        {
            ShowFeedback(0);
        }

        private void ShowLastFeedback()
        {
            ShowFeedback(feedbackManager.GetNumberOfFeedbacks() - 1);

        }

        private void ShowFeedback(int index)
        {
            List<Feedback> feedbacks = feedbackManager.GetFeedbacks();
            Feedback feedback = feedbacks[index];
            cboCarriers.SelectedValue = feedback.carrierID;
            txtFirstName.Text = feedback.firstName;
            txtLastName.Text = feedback.lastName;
            txtDescription.Text = feedback.description;
            dtpDob.Value = (DateTime) feedback.dateOfBirth;

            currentFeedbackIndex = index;
     
            DisableUnwantedNavButtons();
        }

        private void HandleCriticalException(Exception e)
        {
            MessageBox.Show(e.Message, "An error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }
        
        /// <summary>
        /// When there's no data some controls need to be disabled
        /// </summary>
        private void ToggleControls(bool enabled)
        {

            btnNext.Enabled = enabled;
            btnPrev.Enabled = enabled;
            btnLast.Enabled = enabled;
            btnFirst.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnDelete.Enabled = enabled;
            btnSort.Enabled = enabled;
            radBtnAscending.Enabled = enabled;
            radBtnDescending.Enabled = enabled;
            txtDescription.Enabled = enabled;
            txtFirstName.Enabled = enabled;
            txtLastName.Enabled = enabled;
            txtDescription.Enabled = enabled;
            dtpDob.Enabled = enabled;
            btnSearch.Enabled = enabled;
            cboSort.Enabled = enabled;
            cboCarriers.Enabled = enabled;
        }

        private void LoadCarriers()
        {
            cboCarriers.DataSource = feedbackManager.GetCarriersAsDataSet().Tables[0];
            cboCarriers.DisplayMember = "Name";
            cboCarriers.ValueMember = "CarrierID";
        }
        #endregion methods
    }
}
