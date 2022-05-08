using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV18T1021214.DomainModel;

namespace SV18T1021214.DataLayer.SQLServer
{
    public class ProductAttributeDAL : _BaseDAL, IPhotoAttributeDAL<ProductAttribute>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductAttributeDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(ProductAttribute data)
        {
            int result = 0;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO ProductAttributes(AttributeName,AttributeValue,DisplayOrder,ProductID)
                                    VALUES (@AttributeName,@AttributeValue,@DisplayOrder,@ProductID)
                                    SELECT SCOPE_IDENTITY()";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@AttributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@AttributeValue", data.AttributeValue);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);

                result = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool result = false;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM ProductAttributes WHERE AttributeID = @AttributeID ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@AttributeID", id);

                result = cmd.ExecuteNonQuery() > 0;

                conn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductAttribute Get(int id)
        {
            ProductAttribute result = null;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT*FROM ProductAttributes WHERE AttributeID = @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@AttributeID", id);
                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new ProductAttribute()
                    {
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                        AttributeName = Convert.ToString(dbReader["AttributeName"]),
                        AttributeValue = Convert.ToString(dbReader["AttributeValue"]),
                        DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                    };
                }
                conn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public IList<ProductAttribute> List(int productID)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();

            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductAttributes WHERE ProductID = @ProductID ORDER BY DisplayOrder ASC";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@ProductID", productID);


                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new ProductAttribute()
                    {

                        ProductID = Convert.ToInt32(result["ProductID"]),
                        AttributeID = Convert.ToInt32(result["AttributeID"]),
                        AttributeName = Convert.ToString(result["AttributeName"]),
                        AttributeValue = Convert.ToString(result["AttributeValue"]),
                        DisplayOrder = Convert.ToInt32(result["DisplayOrder"]),
                    });
                }
                result.Close();
                conn.Close();
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(ProductAttribute data)
        {
            bool result = false;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE ProductAttributes 
                                    SET AttributeName = @AttributeName, 
                                        AttributeValue = @AttributeValue,
                                        DisplayOrder = @DisplayOrder
                                    WHERE AttributeID = @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@AttributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@AttributeID", data.AttributeID);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@AttributeValue", data.AttributeValue);

                result = cmd.ExecuteNonQuery() > 0;

                conn.Close();
            }
            return result;
        }
    }
}
