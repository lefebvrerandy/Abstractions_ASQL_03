using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Abstractions_ASQL_03
{
    static class SQLLaptop
    {
        /// <summary>
        /// Given the provider, user name, and password, we now have everything to log into
        /// the server and test the connection. Lets create the string for future reference and
        /// return it if it connects successfully. If it doesnt connect, lets return a string
        /// with the values "Failed" so we know it didnt go through.
        /// </summary>
        /// <param name="provider">The provider to connect to</param>
        /// <param name="userName">The username of the sql account</param>
        /// <param name="password">The password of the sql account</param>
        /// <returns></returns>
        static public string CheckCredential(string provider, string userName, string password)
        {
            // Create the connection string to future reference
            string ConnectionString = "";
            OleDbConnectionStringBuilder connString = new OleDbConnectionStringBuilder();
            if (provider == "SQL Server")
                connString.Provider = "SQLOLEDB";   // If the user chooses SQL Server
            else if (provider == "My SQL")
                connString.Provider = "MySQLProv";  // If the user chooses My SQL
            connString.DataSource = "."; // Currently only handles the computer name as "USER"
//DEBUG START
            if (userName != "")
                connString.Add("User ID", userName);
            else
                connString.Add("User ID", "sa");

            if (password != "")
                connString.Add("Password", password);
            else
                connString.Add("Password", "Conestoga1");
//DEBUG END
            connString.Add("Persist Security Inndo", "True");
            //connString.Add("Initial Catalog", "Northwind");

            // Attempt to connect to the database
            using (OleDbConnection conn = new OleDbConnection(connString.ToString()))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    ConnectionString = connString.ToString();
                }
                catch (Exception e)
                {
                    ConnectionString = e.Message.ToString();
                }
            }

            // Return the string or a "Failed" for future reference
            return ConnectionString;
        }
        
        /// <summary>
        /// Given the connection string and combo box to fill, find the database list
        /// and populate the combobox requested
        /// </summary>
        /// <param name="connectionString">The string used to log into the database</param>
        /// <param name="combobox">The combobox to populate</param>
        static public void loadSchemaList(string connectionString, ComboBox combobox)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string result = "";
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    DataTable databases = conn.GetSchema(System.Data.OleDb.OleDbMetaDataCollectionNames.Catalogs);
                    combobox.Items.Clear();
                    foreach (DataRow database in databases.Rows)
                    {
                        combobox.Items.Add(database[0]);
                    }
                    result = "Success";
                }
                catch (Exception e)
                {
                    result = e.Message.ToString();
                }
            }
        }

        static public void LoadTableList(string connectionString, ComboBox combobox)
        {
            string result = "";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                        new object[] { null, null, null, "TABLE" });
                    combobox.Items.Clear();
                    foreach (DataRow table in tables.Rows)
                    {
                        combobox.Items.Add(table["TABLE_NAME"].ToString().Trim());
                    }
                }
                catch (Exception e)
                {
                    result = e.Message.ToString();
                }
            }
        }

        static public DataTable QuerySelectAll(string connectionString)
        {
            string result;
            DataTable dt = new DataTable();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM " + MainMenuForm.selectedCombo1Table);
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
                catch (Exception e)
                {
                    result = e.Message.ToString();
                }
            }
            return dt;
        }
    }
}
