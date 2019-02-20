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
        static public string CheckCredential(string provider, string userName, string password)
        {
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

            return ConnectionString;
        }

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
                    string test = databases.ToString();
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
    }
}
