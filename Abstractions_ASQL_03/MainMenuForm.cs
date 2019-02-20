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
        // These two constants are used to define if its 
        // the left user field or the right user field
        const int USER_ONE = 1;
        const int USER_TWO = 2;

        string selectedCombo1Database;
        string selectedCombo2Database;

        string connectionString1;
        string connectionString2;

        string connectionString1Database;
        string connectionString2Database;

        public MainMenuForm()
        {
            InitializeComponent();
            DatabaseComboChanged();
        }

        /// <summary>
        /// The first things to load when the window starts. We will start
        /// by hiding all the second user fields and setting the focus to the
        /// first text box. The text boxes and buttons are indexed, so pressing
        /// tab should navigate to the appropriate field. We also set the properties 
        /// of the comboboxes to make them not editable by the user.
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
            lbl_Provider_2.Hide();
            comboBox4.Hide();
            btn_Signin_2.Hide();

            // Make the comboboxes not editable by the user
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            combo_Table_1.DropDownStyle = ComboBoxStyle.DropDownList;
            combo_Table_2.DropDownStyle = ComboBoxStyle.DropDownList;

            // focus the first text box on load
            this.ActiveControl = txt_User_1;
        }

        /// <summary>
        /// This method handles the event that the sign in button on the left side is pressed.
        /// We will check to see if the user can sign in. If they can, lets notify them that
        /// their connection was successful and load the combobox with the database list on the left.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Signin_1_Click(object sender, EventArgs e)
        {
            // CHeck that user 1 can successfully connect. If he can, lets prompt the result
            lbl_Error_User_1.Show();
            string connectionString = SignInChecker(USER_ONE);
            if (connectionString != "Failed")
            {
                LabelChanger.ChangeText("Successfully Logged in.", lbl_Error_User_1);
                LabelChanger.ChangeColor("Green", lbl_Error_User_1);
                SQLLaptop.loadSchemaList(connectionString, comboBox1);
                connectionString1 = connectionString;
            }
            else
            {
                LabelChanger.ChangeText("Invalid Credentials.", lbl_Error_User_1);
                LabelChanger.ChangeColor("Red", lbl_Error_User_1);
                connectionString1 = "Failed";
            }
        }

        /// <summary>
        /// This method handles the sign in button on the right side. Its pretty identicle to the above
        /// method, but handles the right side fields and right side combo box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Signin_2_Click(object sender, EventArgs e)
        {
            lbl_Error_User_2.Show();
            // Find the provider. If none is selected, loop through them all


            string connectionString = SignInChecker(USER_TWO);
            if (connectionString != "Failed")
            {
                LabelChanger.ChangeText("Successfully Logged in.", lbl_Error_User_2);
                LabelChanger.ChangeColor("Green", lbl_Error_User_2);
                SQLLaptop.loadSchemaList(connectionString, comboBox2);
                connectionString2 = connectionString;
            }
            else
            {
                LabelChanger.ChangeText("Invalid Credentials.", lbl_User_2);
                LabelChanger.ChangeColor("Red", lbl_Error_User_2);
                connectionString2 = "Failed";
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
            string ConnectionString = "Failed";

            // These fields will be linked to the specific user. User 1 is the left side of the
            // windows form where user 2 is the right side.
            int userNumber = UserNumber;

            if (UserNumber == 2 || UserNumber == 1)
            {
                // Send to "Check Credentials method and return the status to see if it was valid
                if (userNumber == 1)
                    ConnectionString = SQLLaptop.CheckCredential("SQL Server", txt_User_1.Text.ToString(), txt_Pass_1.Text.ToString());
                else
                    ConnectionString = SQLLaptop.CheckCredential("SQL Server", txt_User_2.Text.ToString(), txt_Pass_2.Text.ToString());

                // if valid, change label to text = Success, and color = Green
                // update combobox 1 with all the tables from the required user

            }
            else
            {
                ConnectionString = "Failed";
            }

            return ConnectionString;
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
            lbl_Provider_2.Show();
            comboBox4.Show();
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
            lbl_Provider_2.Hide();
            comboBox4.Hide();
            btn_Signin_2.Hide();

            // Show related fields
            btn_Second_Account.Show();
            lbl_Or.Show();



            // Retreive information from the first sign in form
            string connectionString = SignInChecker(USER_ONE);
            lbl_Error_User_2.Show();
            // If successfully logged in, display success
            if (connectionString != "Failed")
            {
                SQLLaptop.loadSchemaList(connectionString, comboBox2);
                LabelChanger.ChangeText("Successfully Logged in.", lbl_Error_User_2);
                LabelChanger.ChangeColor("Green", lbl_Error_User_2);
            }
            else
            {
                LabelChanger.ChangeText("Invalid Credentials.", lbl_User_2);
                LabelChanger.ChangeColor("Red", lbl_Error_User_2);
            }



            // else if failed to log in, display error
        }

        /// <summary>
        /// This method is called once during the Initialization stage once.
        /// It sets up the event handlers for both databases combo boxes.
        /// </summary>
        private void DatabaseComboChanged()
        {
            comboBox1.SelectedIndexChanged +=
                new System.EventHandler(ComboBox_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged +=
                new System.EventHandler(ComboBox_SelectedIndexChanged);
        }

        /// <summary>
        /// Event handler for selection changes in the Database combo boxes on both the left
        /// and right side. Once a selection has been made or chosen for the first time, 
        /// we will find out the selection and append it to the end of the old connection string.
        /// From there, we will set the initial catalog to our chosen database. Then we will
        /// load the contents of the table combo boxes
        /// </summary>
        /// <param name="sender">The combo box that triggered the event</param>
        /// <param name="e"></param>
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            string selectedCombo1Database = (string)combo.SelectedItem;
            if (sender == comboBox1)
            {
                selectedCombo1Database = (string)combo.SelectedItem;
                // append the old string with the database and store into the global string
                connectionString1Database = connectionString1 + ";Initial Catalog=" + selectedCombo1Database;
                SQLLaptop.LoadTableList(connectionString1Database, combo_Table_1);
            }
            else if (sender == comboBox2)
            {
                selectedCombo2Database = (string)combo.SelectedItem;
                // append the old string with the database and store into the global string
                connectionString2Database = connectionString2 + ";Initial Catalog=" + selectedCombo2Database;
                SQLLaptop.LoadTableList(connectionString2Database, combo_Table_2);
            }
        }

    }
}
