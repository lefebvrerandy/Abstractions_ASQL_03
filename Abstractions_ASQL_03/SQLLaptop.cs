using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            connString.DataSource = "USER"; // Currently only handles the computer name as "USER"
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
    }
}
