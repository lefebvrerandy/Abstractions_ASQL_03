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
    public partial class PreviewWindow : Form
    {

        // These two constants are used to define if its 
        // the left user field or the right user field
        const int USER_ONE = 1;
        const int USER_TWO = 2;


        public PreviewWindow()
        {
            InitializeComponent();
            InitializeDataGrid();
        }

        private void InitializeDataGrid()
        {
            // Check which preview button was pressed. Left or right side.
            int whichSide = MainMenuForm.whichPreview;
            if (whichSide == USER_ONE)
            {
                DataTable dt = SQLLaptop.QuerySelectAll(MainMenuForm.connectionString1Database);
                dataGridView1.DataSource = dt;
                lbl_Title.Text = "You Are Currently Viewing Table: " + MainMenuForm.selectedCombo1Table +
                    ". Database: " + MainMenuForm.selectedCombo1Database + ".";
            }
            else if (whichSide == USER_TWO)
            {
                DataTable dt = SQLLaptop.QuerySelectAll(MainMenuForm.connectionString2Database);
                dataGridView1.DataSource = dt;
                lbl_Title.Text = "You Are Currently Viewing Table: " + MainMenuForm.selectedCombo2Table +
                    ". Database: " + MainMenuForm.selectedCombo2Database + ".";
            }

        }
    }
}
