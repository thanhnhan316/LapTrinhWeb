using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SV18T1021214.DataLayer.SQLServer
{
    /// <summary>
    /// lớp cơ sở cho các xử lý dữ liệu trên Sql server
    /// </summary>
    public abstract  class _BaseDAL
    {
        /// <summary>
        /// 
        /// </summary>
        protected string _connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public _BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Tạo mở kết nối CSDL
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = _connectionString;
            cn.Open();
            return cn;
        }
    }
}
