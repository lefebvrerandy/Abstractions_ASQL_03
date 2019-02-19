using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Abstractions_ASQL_03
{

    public partial class MainMenuForm : Form
    {
        const int USER_ONE = 1;
        const int USER_TWO = 2;

        public MainMenuForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The first things to load when the window starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            // Hide the status Bars
            lbl_Error_User_1.Hide();
            lbl_Error_User_2.Hide();
            lbl_Copy_Status.Hide();

            // Hide the second user form
            lbl_User_2.Hide();
            lbl_Pass_2.Hide();
            txt_User_2.Hide();
            txt_Pass_2.Hide();
            btn_Signin_2.Hide();

            // focus the first text box on load
            this.ActiveControl = txt_User_1;
        }

        private void btn_Signin_1_Click(object sender, EventArgs e)
        {
            // CHeck that user 1 can successfully connect. If he can, lets prompt the result
            lbl_Error_User_1.Show();
            string test = SignInChecker(USER_ONE);
            if (test != "Failed")
            {
                LabelChanger.ChangeText("Successfully Logged in.", lbl_Error_User_1);
                LabelChanger.ChangeColor("Green", lbl_Error_User_1);
            }
            else
            {
                LabelChanger.ChangeText("Invalid Credentials.", lbl_Error_User_1);
                LabelChanger.ChangeColor("Red", lbl_Error_User_1);
            }
        }

        private void btn_Signin_2_Click(object sender, EventArgs e)
        {
            lbl_Error_User_2.Show();
            string test = SignInChecker(USER_TWO);
            if (test != "Failed")
            {
                LabelChanger.ChangeText("Successfully Logged in.", lbl_Error_User_2);
                LabelChanger.ChangeColor("Green", lbl_Error_User_2);
            }
            else
            {
                LabelChanger.ChangeText("Invalid Credentials.", lbl_User_2);
                LabelChanger.ChangeColor("Red", lbl_Error_User_2);
            }
        }

        /// <summary>
        /// This is the sign in method that will be used to handle to logic of the two different
        /// sign in buttons. We will pass the users number (1 = left side, 2 = right side) to the 
        /// method so that the appropriate fields will be used and changed. We are creating a 
        /// generic method to handle all the logic because we will have two different sign in forms
        /// that will do the exact same thing but with different pieces. Why repeat code, right?
        /// </summary>
        /// <param name="UserNumber">Which User is signing in (1 = Left, 2 = Right)</param>
        /// <returns>Validation</returns>
        private string SignInChecker(int UserNumber)
        {
            bool validation = false;
            string test = "Failed";

            // These fields will be linked to the specific user. User 1 is the left side of the
            // windows form where user 2 is the right side.
            int labelNumberFIeld = UserNumber;
            int userNameField = UserNumber;
            int passwordField = UserNumber;

            if (UserNumber == 2 || UserNumber == 1)
            {
                // Send to "Check Credentials method and return the status to see if it was valid

            //DEBUG
                OleDbConnection conn = new OleDbConnection();
                //DEBUG

                // if valid, change label to text = Success, and color = Green
                // update combobox 1 with all the tables from the required user

                test = "StringToPass(ConnectionString)";

            }
            else
            {
                test = "Failed";
            }

            return test;
        }

        private void btn_Second_Account_Click(object sender, EventArgs e)
        {
            // Hide all unrelated field
            lbl_Or.Hide();
            btn_Second_Account.Hide();

            // Show related fields
            lbl_User_2.Show();
            lbl_Pass_2.Show();
            txt_User_2.Show();
            txt_Pass_2.Show();
            btn_Signin_2.Show();

            // Set the focus to the new field User Name
            this.ActiveControl = txt_User_2;
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            // Hide the second user form
            lbl_User_2.Hide();
            lbl_Pass_2.Hide();
            txt_User_2.Hide();
            txt_Pass_2.Hide();
            btn_Signin_2.Hide();

            // Show related fields
            btn_Second_Account.Show();
            lbl_Or.Show();

            // Retreive information from the first sign in form
            // If successfully logged in, display success

            // else if failed to log in, display error
        }


    }
}
