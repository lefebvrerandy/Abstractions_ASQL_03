using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Abstractions_ASQL_03
{
    class SQL
    {
        private string userName;
        private string password;


        public bool CheckAuthentication(string userName, string password, ref string[] returnResult)
        {
            bool valid = false;

            SqlConnectionStringBuilder connString = new SqlConnectionStringBuilder();
            connString.DataSource = "localhost";
            connString.UserID = userName;
            connString.Password = password;
            connString.InitialCatalog = "master";

            using (SqlConnection conn = new SqlConnection(connString.ConnectionString))
            {
                try
                {
                    conn.Open();
                    valid = true;
                    returnResult[0] = "Connecting...";
                }
                catch(SqlException e)
                {
                    valid = false;
                    //string exceptionToSplit = e.ToString();
                    //string[] resultArray = exceptionToSplit.Split('\n');
                    //returnResult = resultArray[0];
                    if (e.Errors.Count <= 5)
                    {
                        for( int i = 0; i < e.Errors.Count; i++)
                        { 
                            returnResult[i] = e.Errors[i].Message;
                            i++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            returnResult[i] = e.Errors[i].Message;
                            i++;
                        }
                    }

                }

                return valid;
            }
        }

    }
}
