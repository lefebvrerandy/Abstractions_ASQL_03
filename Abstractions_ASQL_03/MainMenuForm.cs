using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        static int HowManyTimes = 0;

        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            lbl_Error_User_1.Hide();
            lbl_Error_User_2.Hide();
            lbl_Copy_Status.Hide();
        }

        private void btn_Signin_1_Click(object sender, EventArgs e)
        {
            SignInChecker(USER_ONE);   
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
        private bool SignInChecker(int UserNumber)
        {
            bool validation = false;

            // These fields will be linked to the specific user. User 1 is the left side of the
            // windows form where user 2 is the right side.
            int labelNumberFIeld = UserNumber;
            int userNameField = UserNumber;
            int passwordField = UserNumber;

            if (UserNumber == 2 || UserNumber == 1)
            {
                // Send to "Check Credentials method and return the status to see if it was valid

                // if valid, change label to text = Success, and color = Green
                // update combobox 1 with all the tables from the required user

                // else change label text = Invalid Credentials, color = Red
            }
            else
            {
                validation = false;
            }

            return validation;
        }
    }
}
