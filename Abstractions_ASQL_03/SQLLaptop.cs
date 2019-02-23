/*
 * Developer:   Randy Lefebvre
 * Course:      Advanced SQL - PROG 3070
 * Description: This class is in change of all the SQL handling. Majority of the classes
 *              ask for the connection string, and then the data container to store the 
 *              newly gather information. This is the only class that will communicate with
 *              the SQL databases. 
 */

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
        /// This method creates the connection string. I apologize for not using SET standards here
        /// for the brackets in the if and elses. They are not there for clarity. Everything is pretty
        /// straight and forward.
        /// </summary>
        /// <param name="provider">The provider to connect to</param>
        /// <param name="userName">The username of the sql account</param>
        /// <param name="password">The password of the sql account</param>
        /// <param name="dataSource">The password of the sql account</param>
        /// <returns></returns>
        static public string CreateConnectionString(string provider, string userName, string password, string dataSource = null)
        {
            // Create the connection string to future reference
            OleDbConnectionStringBuilder connString = new OleDbConnectionStringBuilder();
            if (provider == "SQL Server")
                connString.Provider = "SQLOLEDB";   // If the user chooses SQL Server
            else if (provider == "My SQL")
                connString.Provider = "MySQLProv";  // If the user chooses My SQL
            else if (provider == "Access")
                connString.Provider = "Microsoft.ACE.OLEDB.12.0";
            ////else
            ////    connString.Provider = "SQLOLEDB";

            if (dataSource != null)
                connString.DataSource = dataSource;


            if (userName != "")
                connString.Add("User ID", userName);
            ////else
            ////    connString.Add("User ID", "sa");

            if (password != "")
                connString.Add("Password", password);
            ////else
            ////    connString.Add("Password", "Conestoga1");

            connString.Add("Persist Security Info", "True");

            return connString.ToString();
        }


        /// <summary>
        /// This method just trys to open the connection. If success, the user can log in
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns></returns>
        static public bool CheckCredential(string connectionString)
        {
            bool connected = false;
            // Attempt to connect to the database
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    connected = true;
                }
                catch (Exception e)
                {
                    connected = false;
                }
            }

            return connected;
        }

        
        /// <summary>
        /// Given the connection string and combo box to fill, find the database list
        /// and populate the combobox requested
        /// </summary>
        /// <param name="connectionString">The string used to log into the database</param>
        /// <param name="combobox">The combobox to populate</param>
        static public void LoadSchemaList(string connectionString, ComboBox combobox)
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

        /// <summary>
        /// This method just loads the table list. It takes the connection string as well as
        /// which combo box needs to be loaded with the tables.
        /// </summary>
        /// <param name="connectionString">The connection string to connect to</param>
        /// <param name="combobox">The combo box to load up</param>
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
                    foreach (DataRow table in tables.Rows)
                    {
                        combobox.Items.Add(table["TABLE_SCHEMA"].ToString() + "." + table["TABLE_NAME"].ToString());
                    }
                }
                catch (Exception e)
                {
                    result = e.Message.ToString();
                }
            }
        }

        /// <summary>
        /// This method just calls a SELECT * From the table specified. It then returns
        /// the datatable that was received from the select *
        /// </summary>
        /// <param name="connectionString">The connection string to connect to</param>
        /// <param name="table">The table that is required to be loaded</param>
        /// <returns></returns>
        static public DataTable QuerySelectAll(string connectionString, string table)
        {
            string result;
            DataTable dt = new DataTable();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand(@"SELECT * FROM " + table);
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception e)
                {
                    result = e.Message.ToString();
                }
            }
            return dt;
        }

        /// <summary>
        /// This method we check if the tables in the Right side contains content
        /// If it does, we want to return a true. If it doesnt, we want to return a false
        /// </summary>
        static public bool TableContainsContent(string connectionString, string table)
        {
            bool tableContainsConect = true;

            DataTable dt = new DataTable();
            dt = QuerySelectAll(connectionString, table);
            int rowCount = dt.Rows.Count;

            if (rowCount <= 0)
            {
                tableContainsConect = false;
            }

            return tableContainsConect;
        }

        /// <summary>
        /// This is a work in progress.. This is a work in progress.. This is a work in progress..
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        //static public DataTable CreateTable(string connectionString, string table)
        //{
        //    string result;
        //    DataTable dt = new DataTable();
        //    using (OleDbConnection conn = new OleDbConnection(connectionString))
        //    {
        //        OleDbCommand cmd = new OleDbCommand(@"CREATE TABLE " + table + "(placeHolder Int)");
        //        cmd.Connection = conn;
        //        try
        //        {
        //            conn.Open();
        //            dt.Load(cmd.ExecuteReader());
        //        }
        //        catch (Exception e)
        //        {
        //            result = e.Message.ToString();
        //        }
        //    }
        //    return dt;
        //}


        /// <summary>
        /// This method preforms the copy process. It just calls a SELECT * INTO destination FROM source.
        /// It then returns a result of the rows affected. Using transaction. If an errors has happened, a 
        /// roll back will be preformed.
        /// </summary>
        /// <param name="connectionStringSource">The connection string to connect to</param>
        /// <param name="sourceTable">The table from the source</param>
        /// <param name="destinationTable">The table from the destination</param>
        /// <param name="sourceDatabase">The database from the source</param>
        /// <param name="destinationDatabase">The database from the destination</param>
        /// <returns></returns>
        static public int InsertInto(string connectionStringSource, 
            string sourceTable, string destinationTable, string sourceDatabase, string destinationDatabase)
        {
            int status = 0;

            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection connSource = new OleDbConnection(connectionStringSource);
            cmd.Connection = connSource;
            connSource.Open();


            using (OleDbTransaction trans = connSource.BeginTransaction())
            {
                try
                {
                    
                    cmd.Transaction = trans;
                    cmd.CommandText = "SELECT * INTO " + destinationDatabase + "." + destinationTable + " FROM " + sourceDatabase + "." + sourceTable;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    trans.Commit();
                    status = rowsAffected;
                }
                catch (Exception error)
                {
                    trans.Rollback();
                    MessageBox.Show(error.Message);
                }
            }
            connSource.Close();
            return status;
        }

        /// <summary>
        /// This is a work in progress... This is a work in progress... This is a work in progress...
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="table"></param>
        //static public void DeleteTable(string connectionString, string table)
        //{

        //}
    }
}
