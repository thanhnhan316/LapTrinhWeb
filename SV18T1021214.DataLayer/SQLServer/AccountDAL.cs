using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021214.DataLayer.SQLServer
{
    public class AccountDAL : _BaseDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public AccountDAL(string connectionString) : base(connectionString)
        {
        }

        public int Login (string email,string password)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Employees where Email = @Email and Password = @Password";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if(dbReader.Read())
                    result = 1;
                cn.Close();
            }

            return result;
        }

        public bool ChangePassword(string username, string NewPassword)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Employees set Password = @NewPassword where Email = @Email";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@Email", username);
                cmd.Parameters.AddWithValue("@NewPassword", NewPassword);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                //result = cmd.ExecuteNonQuery() > 0;
                result = true;
                cn.Close();
            }

            return result;
        }
    }
}
