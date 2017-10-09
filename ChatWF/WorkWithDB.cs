using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWF
{
    class WorkWithDB
    {
        SqlConnection conconnection;
        public WorkWithDB(string connectionStr)
        {
            conconnection = new SqlConnection(connectionStr);
        }

        public bool IsAvailabilityDB()
        {
            conconnection.Open();
            string strCommand = String.Format($"SELECT db_id('RegistrationUser');");
            var dbCommand = new SqlCommand(strCommand, this.conconnection);
            var reder = dbCommand.ExecuteScalar();
            conconnection.Close();
            if ((int)reder == null) return false;
            else return true;
        }
        public void CreateTable()
        {

            conconnection.Open();
            string strCommand = String.Format($"CREATE TABLE RegistrationTable (ID INT IDENTITY PRIMARY KEY,UserLogin NVARCHAR(30) UNIQUE NOT NULL,UserPassword NVARCHAR(30) NOT NULL,UserName NVARCHAR(30) NULL,UserSurname NVARCHAR(30) NULL,UserDOP NVARCHAR(30) NULL,UserDepartmen NVARCHAR(30) NULL); ");
            var dbCommand = new SqlCommand(strCommand, this.conconnection);
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }
            conconnection.Close();
        }
        public int RegistrationUser(object login, object password, object name, object surname, object dob, object department)
        {
            int numberOfChangedRows;
            conconnection.Open();
            string strCommand = String.Format($"INSERT INTO RegistrationTable VALUES ('{login}','{password}','{name}','{surname}','{dob}','{department}')");
            var dbCommand = new SqlCommand(strCommand, this.conconnection);
            numberOfChangedRows = dbCommand.ExecuteNonQuery();
            conconnection.Close();
            return numberOfChangedRows;
        }
        public bool LoginValidate(string nameItem)
        {
            conconnection.Open();
            string strCommand = String.Format($"SELECT COUNT(DISTINCT UserLogin) FROM RegistrationTable WHERE UserLogin = '{nameItem}';");
            var dbCommand = new SqlCommand(strCommand, this.conconnection);
            var reder = dbCommand.ExecuteScalar();
            conconnection.Close();
            if ((int)reder == 1) return true;
            else return false;
        }
    }
}