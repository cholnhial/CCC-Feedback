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
using CCC_Feedback.forms;
using CCC_Feedback_DB;

namespace CCC__Feedback
{
    public partial class frmFeedback : Form
    {

        #region vars
        private Feedback enteredFeedback;
        private FeedbackManager feedbackManager;
        #endregion vars

        public frmFeedback(FeedbackManager fmanager)
        {
            enteredFeedback = new Feedback();
            InitializeComponent();
            feedbackManager = fmanager;
            InitializeCarriersComboBox();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtFirstName.Text == "" && txtLastName.Text == "" && txtDescription.Text == "")
            {
                MessageBox.Show(this, "Please fill in all the fields", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            } else
            {
                enteredFeedback.carrierID = (int) cboCarriers.SelectedValue;
                enteredFeedback.firstName = txtFirstName.Text;
                enteredFeedback.lastName = txtLastName.Text;
                enteredFeedback.dateOfBirth = dtpDob.Value;
                enteredFeedback.description = txtDescription.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
         
        }

        private void btnNewCarrier_Click(object sender, EventArgs e)
        {
            frmNewCarrier frmNewCarrier = new frmNewCarrier();
            DialogResult dialogResult = frmNewCarrier.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                string carrierName = frmNewCarrier.GetEnteredCarrierName();
                if (feedbackManager.CarrierExists(carrierName))
                {
                    DisplayErrorMessage("Carrier Already Exists", "Please choose another carrier name.");
                }
                else
                {
                    Carrier newCarrier = new Carrier();
                    newCarrier.name = carrierName;
                    feedbackManager.SaveCarrier(newCarrier);
                    InitializeCarriersComboBox();
                }
            }
        }

        #region methods
        private void DisplayErrorMessage(string title, string msg)
        {
            MessageBox.Show(this, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DisplayInformationMessage(string title, string msg)
        {
            MessageBox.Show(this, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public Feedback GetEnteredFeedback()
        {
            return enteredFeedback;
        }

        public void InitializeCarriersComboBox()
        {
            cboCarriers.DataSource = feedbackManager.GetCarriersAsDataSet().Tables[0];
            cboCarriers.DisplayMember = "Name";
            cboCarriers.ValueMember = "CarrierID";
        }
        #endregion methods


    }
}
