/*
 * Developer:   Randy Lefebvre
 * Course:      Advanced SQL - PROG 3070
 * Description: This class is in change of all the features behind the MainMenuForm.
 *              It handles all the data manipulation and button events as well as calls
 *              both the SQLLaptop class for SQL connection, and PreviewWindow form to show
 *              the table.
 *              The approach I took here was to best eliminate User error. It fills the combo
 *              boxes with the data found. This wasnt exactly what the assignment requirements asked
 *              for, but I figure that if I spent the extra time to develop an application that is alittle
 *              more useful, I could use it for everyday purpose in the future. I hope that this doesn't
 *              effect my mark that much.
 */

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

        // These two public strings are used to hold the databases selected in both
        // combo boxes.
        public static string selectedCombo1Database;
        public static string selectedCombo2Database;

        // These two public strings are used to hold the tables selected in both
        // combo boxes
        public static string selectedCombo1Table;
        public static string selectedCombo2Table;

        // These two connection strings are used for the left and right side user.
        string connectionString1;
        string connectionString2;

        // These two public strings hold the connection strings including the database as
        // Initial Catalog.
        public static string connectionString1Database;
        public static string connectionString2Database;

        // This class variable is used for the next window "Preview Window" form that will open up
        // if the user presses the preview button. This just tells which user (left or right) has 
        // requested the preview
        public static int whichPreview;

        // This is a flag that toggles the assignment requirement check box. We default it to on since
        // it is an assignment after all. If the user toggles it off, this provides some extra features.
        // Not all non-assignment features have been fully tested. You've been warned.
        bool assignmentRequirement;

        public MainMenuForm()
        {
            InitializeComponent();
            PopulateProviderComboBox();
            DatabaseComboChanged();
            TableComboChanged();
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
            // Hide the second user on first load
            HideSecondUserLogin();

            // Hide all the error labels
            lbl_Error_User_1.Hide();
            lbl_Error_User_2.Hide();
            lbl_Error_Copy.Hide();

            // Make the comboboxes not editable by the user
            cb_DataBase_1.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_DataBase_2.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_Provider_1.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_Provider_2.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_Table_1.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_Table_2.DropDownStyle = ComboBoxStyle.DropDownList;

            // focus the first text box on load
            this.ActiveControl = txt_User_1;

            // Default the assignment requirements to YES
            check_Assign_Require.Checked = true;
        }

        /// <summary>
        /// This method just populates the combo box for the provider
        /// </summary>
        private void PopulateProviderComboBox()
        {
            cb_Provider_1.Items.Add("SQL Server");
            cb_Provider_1.Items.Add("My SQL");
            cb_Provider_1.Items.Add("Access");

            cb_Provider_2.Items.Add("SQL Server");
            cb_Provider_2.Items.Add("My SQL");
            cb_Provider_2.Items.Add("Access");
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
            // Check that user 1 can successfully connect. If he can, lets prompt the result
            lbl_Error_User_1.Show();
            string connectionString = SignInChecker(USER_ONE);
            if (connectionString != "Failed")
            {
                LabelChanger.ChangeText("Successfully Logged in.", lbl_Error_User_1);
                LabelChanger.ChangeColor("Green", lbl_Error_User_1);
                SQLLaptop.LoadSchemaList(connectionString, cb_DataBase_1);
                connectionString1 = connectionString;
                if (assignmentRequirement)
                {
                    connectionString2 = connectionString1;
                    SQLLaptop.LoadSchemaList(connectionString, cb_DataBase_2);
                }
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
                SQLLaptop.LoadSchemaList(connectionString, cb_DataBase_2);
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
        /// This method is used to control the second account button click.
        /// Currently only support in non-assignment requirement mode. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Second_Account_Click(object sender, EventArgs e)
        {
            // Show all the fields for the second user
            ShowSecondUserLogin();

            // Set the focus to the new field User Name
            this.ActiveControl = txt_User_2;

            // Delete the contents of second user that are already stored
            selectedCombo2Database = null;
            selectedCombo2Table = null;
            connectionString2Database = null;
            cb_DataBase_2.Items.Clear();
            cb_Table_2.Items.Clear();
        }

        /// <summary>
        /// This method is support in non-assignment mode. Is just duplicates the first account
        /// into the second account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Same_Account(object sender, EventArgs e)
        {
            HideSecondUserLogin();

            // Retrieve information from the first sign in form
            string connectionString = SignInChecker(USER_ONE);
            lbl_Error_User_2.Show();
            // If successfully logged in, display success
            if (connectionString != "Failed")
            {
                SQLLaptop.LoadSchemaList(connectionString, cb_DataBase_2);
                LabelChanger.ChangeText("Successfully Logged in.", lbl_Error_User_2);
                LabelChanger.ChangeColor("Green", lbl_Error_User_2);
                connectionString2 = connectionString;
            }
            else
            {
                LabelChanger.ChangeText("Invalid Credentials.", lbl_User_2);
                LabelChanger.ChangeColor("Red", lbl_Error_User_2);
            }
            // else if failed to log in, display error
        }

        /// <summary>
        /// This method is used to control the event for the button click event.
        /// it just sets the flag as to which user is opening the window, and then opens
        /// the preview window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Preview_Click(object sender, EventArgs e)
        {
            // Check to see which preview button was pressed
            if (sender == btn_Preview_1)
            {
                // Ensure there has been selections made
                if ((string)cb_Table_1.SelectedItem != null)
                {
                    whichPreview = USER_ONE;
                    PreviewWindow pw = new PreviewWindow();
                    pw.Show();
                }

            }
            // Same thing as the above if statement but for the right side
            else if (sender == btn_Preview_2)
            {
                if ((string)cb_Table_2.SelectedItem != null)
                {
                    whichPreview = USER_TWO;
                    PreviewWindow pw = new PreviewWindow();
                    pw.Show();
                }
            }
        }

        /// <summary>
        /// This is the main method that does the copying. It uses a few features inside the SQLlaptop
        /// class to work with the database and figure out results. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Transfer_Click(object sender, EventArgs e)
        {
            bool shouldCopy = false;
            bool emptyTable = false;

            // Check to make sure all fields are selected
            if (AllFieldsFilled())
            {
                if (!assignmentRequirement)
                {
                    string selectedTable = (string)cb_Table_2.SelectedItem;
                    if (SQLLaptop.TableContainsContent(connectionString2Database, selectedTable))
                    {
                        emptyTable = false;
                    }
                    else
                    {
                        // Table 2 is currently empty. Just copy over data
                        emptyTable = true;
                        shouldCopy = true;
                    }
                }
                else
                {
                    if (connectionString1Database != connectionString2Database)
                    {
                        emptyTable = true;
                        shouldCopy = false;
                    }

                }

                if (!emptyTable)
                {
                    DialogResult result = System.Windows.Forms.DialogResult.No;
                    //Prompt that current table will be over-written with message box
                    result = MessageBox.Show("Are you sure you'd like to copy the first table to the second table?",
                        "Copy",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        shouldCopy = true;
                    }
                }
            }
            
            if (shouldCopy)
            {
                //Copy content from table 1 to table 2
                CopyContent();
            }
            else if ((emptyTable == true) && (shouldCopy == false))
            {
                lbl_Error_Copy.Show();
                lbl_Error_Copy.Text = "Database do not match. This is do-able via disables the assignment requirement checkbox";
            }
            else
            {
                lbl_Error_Copy.Show();
                lbl_Error_Copy.Text = "You are missing some selections.. Please choose a database and table for source and destination";
            }
        }

        /// <summary>
        /// This method handles the assignment requirement checkbox. If the requirement button is toggled on or
        /// off, it will show/hide the correct information.
        /// </summary>
        private void HandleAssignmentRequirements()
        {
            // If we are following the assignment requirements, we need to disable a few buttons
            // text fields, and data that is linked to those
            // Otherwise lets duplicate all info
            if (assignmentRequirement == true)
            {
                HideSecondUserLogin();
                btn_Second_Account.Hide();
                lbl_Or.Hide();
                btn_Copy.Hide();
                lbl_Error_User_2.Hide();
                cb_DataBase_2.Items.Clear();
                cb_Table_2.Items.Clear();
                connectionString2 = connectionString1;
                connectionString2Database = connectionString1Database;
                selectedCombo2Database = selectedCombo1Database;
                selectedCombo2Table = selectedCombo1Table;
                lbl_Error_Copy.Hide();
                if (connectionString2 != null && connectionString2Database != null)
                {
                    SQLLaptop.LoadSchemaList(connectionString2, cb_DataBase_2);
                    SQLLaptop.LoadSchemaList(connectionString2Database, cb_Table_2);
                }

            }
            else
            {
                btn_Second_Account.Show();
                lbl_Or.Show();
                btn_Copy.Show();
                lbl_Error_Copy.Hide();
                connectionString2 = null;
                connectionString2Database = null;
                selectedCombo2Database = null;
                selectedCombo2Table = null;
                cb_DataBase_2.Items.Clear();
                cb_Table_2.Items.Clear();
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
            string connectionString = "Failed";

            // These fields will be linked to the specific user. User 1 is the left side of the
            // windows form where user 2 is the right side.
            int userNumber = UserNumber;

            if (UserNumber == 2 || UserNumber == 1)
            {
                // Send to "Check Credentials method and return the status to see if it was valid
                if (userNumber == 1)
                {
                    connectionString = SQLLaptop.CreateConnectionString(cb_Provider_1.Text, txt_User_1.Text.ToString(), txt_Pass_1.Text.ToString(), txt_DataSource_1.Text.ToString());
                }
                else
                {
                    connectionString = SQLLaptop.CreateConnectionString(cb_Provider_2.Text, txt_User_2.Text.ToString(), txt_Pass_2.Text.ToString(), txt_DataSource_2.Text.ToString());
                }

                if (!SQLLaptop.CheckCredential(connectionString))
                {
                    connectionString = "Failed";
                }
            }
            else
            {
                connectionString = "Failed";
            }

            return connectionString;
        }

        /// <summary>
        /// This method controls the hiding of the second users login field. If the copy credentials 
        /// button is triggered, this method will be called to hide all the extra fields.
        /// </summary>
        private void HideSecondUserLogin()
        {
            // Hide the second user form
            lbl_User_2.Hide();
            lbl_Pass_2.Hide();
            txt_User_2.Hide();
            txt_Pass_2.Hide();
            lbl_Provider_2.Hide();
            cb_Provider_2.Hide();
            btn_Signin_2.Hide();
            lbl_DataSource_2.Hide();
            txt_DataSource_2.Hide();


            // Show related fields
            btn_Second_Account.Show();
            lbl_Or.Show();
        }

        /// <summary>
        /// This method is used to show the second user information fields. If the user presses
        /// Second account, this method will be called to hide/show the right fields.
        /// </summary>
        private void ShowSecondUserLogin()
        {
            // Hide all unrelated field
            lbl_Or.Hide();
            btn_Second_Account.Hide();
            lbl_Error_User_2.Hide();

            // Show related fields
            lbl_User_2.Show();
            lbl_Pass_2.Show();
            txt_User_2.Show();
            txt_Pass_2.Show();
            lbl_Provider_2.Show();
            cb_Provider_2.Show();
            btn_Signin_2.Show();
            lbl_DataSource_2.Show();
            txt_DataSource_2.Show();
        }

        /// <summary>
        /// This method is called once during the Initialization stage once.
        /// It sets up the event handlers for both databases combo boxes.
        /// </summary>
        private void DatabaseComboChanged()
        {
            cb_DataBase_1.SelectedIndexChanged +=
                new System.EventHandler(Database_ComboBox_SelectedIndexChanged);
            cb_DataBase_2.SelectedIndexChanged +=
                new System.EventHandler(Database_ComboBox_SelectedIndexChanged);
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
        private void Database_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            if (sender == cb_DataBase_1)
            {
                selectedCombo1Database = (string)combo.SelectedItem;
                selectedCombo1Table = null;
                lbl_Error_Copy.Hide();
                cb_Table_1.Items.Clear();

                // append the old string with the database and store into the global string
                connectionString1Database = connectionString1 + ";Initial Catalog=" + selectedCombo1Database;
                SQLLaptop.LoadTableList(connectionString1Database, cb_Table_1);

                if (assignmentRequirement)
                {
                    connectionString2Database = connectionString1Database;
                }
            }
            else if (sender == cb_DataBase_2)
            {
                selectedCombo2Database = (string)combo.SelectedItem;
                selectedCombo2Table = null;
                cb_Table_2.Items.Clear();

                // append the old string with the database and store into the global string
                connectionString2Database = connectionString2 + ";Initial Catalog=" + selectedCombo2Database;

                // Add the first entry as "New Table"
                if (!assignmentRequirement)
                {
                    cb_Table_2.Items.Add("New Table...");
                }
                else if (assignmentRequirement && (connectionString1Database == connectionString2Database))
                {
                    cb_Table_2.Items.Add("New Table...");
                }
                SQLLaptop.LoadTableList(connectionString2Database, cb_Table_2);
            }
        }

        /// <summary>
        /// This method just sets events to the table combo boxes.
        /// </summary>
        private void TableComboChanged()
        {
            cb_Table_1.SelectedIndexChanged +=
                new System.EventHandler(Table_ComboBox_SelectedIndexChanged);
            cb_Table_2.SelectedIndexChanged +=
                new System.EventHandler(Table_ComboBox_SelectedIndexChanged);
        }

        /// <summary>
        /// This method handles the event of the table change. It will set the class variables once
        /// the selection has been made
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            lbl_Error_Copy.Hide();
            if (combo == cb_Table_1)
            {
                selectedCombo1Table = (string)combo.SelectedItem;
            }
            else if (combo == cb_Table_2)
            {
                if ((string)combo.SelectedItem == "New Table...")
                {
                    Table2Unlock(combo);
                }
                selectedCombo2Table = (string)combo.SelectedItem;
            }
        }

        /// <summary>
        /// Check to see if there is data stored in all the fields there should be
        /// To be successful, we want data in:
        /// 
        ///public static string selectedCombo1Database;
        ///public static string selectedCombo2Database;
        ///public static string selectedCombo1Table;
        ///public static string selectedCombo2Table;
        /// </summary>
        private bool AllFieldsFilled()
        {
            bool allFieldsAreFilled = false;

            if (selectedCombo1Database != null)
            {
                if (selectedCombo2Database != null)
                {
                    if (selectedCombo1Table != null)
                    {
                        if (selectedCombo2Table != null)
                        {
                            allFieldsAreFilled = true;
                        }
                    }
                }
            }

            return allFieldsAreFilled;
        }

        /// <summary>
        /// This is the main method that does the copying. It calls the SQLLaptop class
        /// to do the inserting and the return result is the rows affected by the call.
        /// If there are more than 0 rows, it will let the user know how many rows were affect.
        /// If not it will display the error and a tip
        /// </summary>
        /// <returns></returns>
        private bool CopyContent()
        {
            bool copyStatus = false;

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();

            int rowsAffected = SQLLaptop.InsertInto(connectionString1Database,
                selectedCombo1Table, selectedCombo2Table, selectedCombo1Database, selectedCombo2Database);
            if (rowsAffected >= 1)
            {
                lbl_Error_Copy.Show();
                LabelChanger.ChangeColor("Green", lbl_Error_Copy);
                LabelChanger.ChangeText("Successfully Copied. " + rowsAffected + " rows have been affected.", lbl_Error_Copy);
            }
            else
            {
                // Clear the new table that didnt work
                lbl_Error_Copy.Show();
                LabelChanger.ChangeColor("Red", lbl_Error_Copy);
                LabelChanger.ChangeText("Error when copying the tables. Double check your entered schema name and table. (eg. dbo.test)", lbl_Error_Copy);
                cb_Table_2.SelectedIndex = -1;
            }

            return copyStatus;
        }

        /// <summary>
        /// This method is called once the checkbox for assignment requirements has been pressed it not.
        /// it just sets the class variable depending on the checkmark.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_Assign_Require_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked == true)
            {
                assignmentRequirement = true;
            }
            else
            {
                assignmentRequirement = false;
            }
            HandleAssignmentRequirements();
        }

        /// <summary>
        /// This method unlocks the combobox so the user can enter their desired table name
        /// </summary>
        /// <param name="combo"></param>
        private void Table2Unlock(ComboBox combo)
        {
            int index = combo.SelectedIndex;
            string newText = string.Empty;
            combo.DropDownStyle = ComboBoxStyle.DropDown;
            combo.SelectedText = "Enter Table Name";
        }

        /// <summary>
        /// This method handles the key press event. This is was gathered from:
        /// https://www.codeproject.com/Questions/221538/to-know-enter-is-pressed-on-leave-event-of-combobo
        /// By the author: 
        /// 2irfanshaikh on 5-Jul-11 22:29pm
        /// 
        /// This method is used to handle when a user entered a new text into the combo box. It will
        /// add the table to the combo box and select it.
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    if (cb_Table_2.DropDownStyle == ComboBoxStyle.DropDown)
                    {
                        cb_Table_2.Items.Insert(1, cb_Table_2.Text);
                        cb_Table_2.DropDownStyle = ComboBoxStyle.DropDownList;
                        cb_Table_2.SelectedIndex = 1;
                    }

                    break;
                case Keys.Space:
                    return base.ProcessDialogKey(Keys.Tab);
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
