//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Abstractions_ASQL_03
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//            txt_Password.PasswordChar = '*';
//        }

//        private void btn_SignIn_Click(object sender, EventArgs e)
//        {
//            var userName = txt_UserName.Text;
//            var password = txt_Password.Text;
//            string[] result = new string[5];
//            SQL SQLClass = new SQL();

//            if(SQLClass.CheckAuthentication(userName, password, ref result))
//            {
//                //Open main window and send in the connection string
//            }
//            else
//            {
//                string totalResult = "";
//                int i = 0;
//                totalResult = "Error(s) have occured.. See below..\n\n";
//                while(result[i] != null)
//                {
//                    totalResult += result[i].ToString();
//                    totalResult += "\n";
//                    i++;
//                }
//                label1.Text = totalResult;
//            }

//            //label1.Text = userName + " " + password;
//        }
//    }
//}
