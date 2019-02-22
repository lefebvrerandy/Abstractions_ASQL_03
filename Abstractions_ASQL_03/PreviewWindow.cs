using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Types;

namespace Abstractions_ASQL_03
{
    public partial class PreviewWindow : Form
    {

        // These two constants are used to define if its 
        // the left user field or the right user field
        const int USER_ONE = 1;
        const int USER_TWO = 2;

        // Below is a definition of all the class variables

        // Connection string for the left user and right user. Initialized in "InitializeVariablesFromParent"
        string connectionStringLeft;
        string connectionStringRight;

        // Database selected for the left user and right user. Initialized in "InitializeVariablesFromParent"
        string selectedDatabaseLeft;
        string selectedDatabaseRight;

        // Table selected for the left user and right user. Initialized in "InitializeVariablesFromParent"
        string selectedTableLeft;
        string selectedTableRight;

        // Which side is currently opening the preview window. Either left or right
        // Initialized in "InitializeVariablesFromParent"
        int whichSide;


        public PreviewWindow()
        {
            InitializeComponent();
            InitializeVariablesFromParent();
            PopulateDataGrid();
            PopulateTitle();
        }

        private void PreviewWindow_Load(object sender, EventArgs e)
        {
            // Getting the height of the title bar
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            // Getting the height and width of the datatableview
            int dgv_width = dataGridView1.Columns.GetColumnsWidth(DataGridViewElementStates.Visible);
            int dgv_height = dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.Visible);

            // If the dgv_width of the grid is greater than half the screen. Set it to half the screen
            // If the dgv_width of the grid is less than one quarter of your screen. Set to one quarter
            if (dgv_width >= (Screen.PrimaryScreen.Bounds.Width / 2))
            {
                dgv_width = (Screen.PrimaryScreen.Bounds.Width / 2);
            }
            if (dgv_width <= (Screen.PrimaryScreen.Bounds.Width / 4))
            {
                dgv_width = (Screen.PrimaryScreen.Bounds.Width / 4);
            }
            this.Width = dgv_width;
            dataGridView1.Width = dgv_width;

            // If the dgv_height of the grid is greater than half the screen. Set it to half the screen
            // If the dgv_height of the grid is less than one quarter of your screen. Set to one quarter
            if (dgv_height >= (Screen.PrimaryScreen.Bounds.Height / 2))
            {
                dgv_height = (Screen.PrimaryScreen.Bounds.Height / 2);
                this.Height = dgv_height;
                dataGridView1.Height = dgv_height;
            }
            if (dgv_height <= (Screen.PrimaryScreen.Bounds.Width / 4))
            {
                dgv_height = (Screen.PrimaryScreen.Bounds.Width / 4);
            }
            this.Height = dgv_height;
            dataGridView1.Height = dgv_height - titleHeight - 10;


        }

        /// <summary>
        /// This method is used to initialize the class variables. These are inherited from the parent form.
        /// </summary>
        private void InitializeVariablesFromParent()
        {
            connectionStringLeft = MainMenuForm.connectionString1Database;
            connectionStringRight = MainMenuForm.connectionString2Database;

            selectedDatabaseLeft = MainMenuForm.selectedCombo1Database;
            selectedDatabaseRight = MainMenuForm.selectedCombo2Database;

            selectedTableLeft = MainMenuForm.selectedCombo1Table;
            selectedTableRight = MainMenuForm.selectedCombo2Table;

            whichSide = MainMenuForm.whichPreview;
        }

        /// <summary>
        /// This method is used to populate the datagrid inside of the form. This method checks
        /// which side (left or right) is calling it, and then populates it.
        /// </summary>
        private bool PopulateDataGrid()
        {
            bool containsInvalidDatatypes = false;
            int rowCount = 0;
            DataTable dt = null;
            Dictionary<DataColumn, string> invalidColumns = new Dictionary<DataColumn, string>();
            string columnName = string.Empty;
            DataColumn invalidColumn;

            // Check which preview button was pressed. Left or right side. Send the query to the SQL
            // class to SELECT all. 
            if (whichSide == USER_ONE)
            {
                dt = SQLLaptop.QuerySelectAll(connectionStringLeft, selectedTableLeft);
            }
            else if (whichSide == USER_TWO)
            {
                dt = SQLLaptop.QuerySelectAll(connectionStringRight, selectedTableRight);
            }

            // We want to seach the columns for the datatype Byte[]. Currently our 
            // datagridview cannot support some Byte[]. Lets not display them.
            // We will warn the user if a Byte[] column is found. From here, we will give
            // the user the choice to convert the column and row datatypes to something viewable (a 
            // string in our case). This WILL take a while depending on how many rows there are. 
            // This may also cause exceptions which we will catch. 
            foreach (DataColumn x in dt.Columns)
            {
                if (x.DataType == typeof(Byte[]))
                {
                    invalidColumn = x;
                    columnName = x.ColumnName;
                    invalidColumns.Add(invalidColumn, columnName);

                    // Count how many rows this table contains to display in the message box.
                    if (!containsInvalidDatatypes)
                    {
                        rowCount = dt.Rows.Count;
                    }
                    containsInvalidDatatypes = true;
                }
            }

            // The user will have 2 possible message boxes that will pop up. The first being an option
            // to change the datatable to make it viewable. The second will be a message saying its 
            // impossible to open the preview because of the row count.
            DialogResult result = System.Windows.Forms.DialogResult.No;
            if (containsInvalidDatatypes && (rowCount <= 200))
            {
                result = MessageBox.Show("This table contains a column or columns that have an non-viewable data type." +
                    "We have the ability to convert that columns rows to blank entries. This will make the rest of the data viewable." +
                    "Be Warned though.. This could take a VERY long time depending on the amount of rows.\n" +
                    "We suggest only doing this if the row count is less than 50. \n" +
                    "The row count for this Table is " + rowCount + ".", "WARNING: Invalid data type in a column detected.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else if (containsInvalidDatatypes)
            {
                result = MessageBox.Show("Currently this application does not support the ability to display a data table," +
                    "that has an invalid data type in one or more of the columns and more than 200 rows." +
                    "\nSorry for the inconvience." +
                    "To better understand why, your tables row count is below.\n" +
                    "The row count for this Table is " + rowCount + ".", "ERROR: Invalid data type in a column detected.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // We will only convert the table if the user clicks "YES"
            // We will display a "temp" table so we dont change the main table.
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                DataTable tempTable = ChangeColumnDataType(dt, columnName, typeof(string));
                dataGridView1.DataSource = tempTable;
            }
            // We will only display the result if either
            // 1. The user agrees to converting the column
            // 2. The user's chosen table does not contain unsupported datatypes.
            if (!containsInvalidDatatypes)
            {
                dataGridView1.DataSource = dt;
            }

            return containsInvalidDatatypes;
        }

        public static DataTable ChangeColumnDataType(DataTable table, string columnname, Type newtype)
        {
            DataTable newTable = table;

            if (newTable.Columns.Contains(columnname) == false)
                newTable = null;

            DataColumn column = newTable.Columns[columnname];
            if (column.DataType == newtype)
                newTable = null;

            try
            {
                DataColumn newcolumn = new DataColumn("temporary", newtype);
                newTable.Columns.Add(newcolumn);

                foreach (DataRow row in newTable.Rows)
                {
                    try
                    {
                        row["temporary"] = Convert.ChangeType(row[columnname], newtype);
                    }
                    catch { }
                }
                newcolumn.SetOrdinal(column.Ordinal);
                newTable.Columns.Remove(columnname);
                newcolumn.ColumnName = columnname;
            }
            catch (Exception)
            {
            }
            return newTable;
        }

        private void PopulateTitle()
        {
            // Check which preview button was pressed. Left or right side.
            if (whichSide == USER_ONE)
            {
                this.Text = "Table: [" + selectedTableLeft +
                    "] - Database: [" + selectedDatabaseLeft + "]";
            }
            else if (whichSide == USER_TWO)
            {
                this.Text = "Table: [" + selectedTableRight +
                    "] - Database: [" + selectedDatabaseRight + "]";
            }
        }

    }
}
