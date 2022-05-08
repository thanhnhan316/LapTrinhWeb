using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV18T1021214.DomainModel;

namespace SV18T1021214.DataLayer.SQLServer
{
    public class ProductPhotoDAL : _BaseDAL, IPhotoAttributeDAL<ProductPhoto>
    {
        public ProductPhotoDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(ProductPhoto data)
        {
            int result = 0;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO ProductPhotos(ProductID, Photo,Description,DisplayOrder,IsHidden)
                                    VALUES (@ProductID, @Photo,@Description,@DisplayOrder,@IsHidden)
                                    SELECT SCOPE_IDENTITY()";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@IsHidden", data.IsHidden);

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
                cmd.CommandText = @"DELETE FROM ProductPhotos WHERE PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@PhotoID", id);

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
        public ProductPhoto Get(int id)
        {
            ProductPhoto result = null;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT*FROM ProductPhotos WHERE PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@PhotoID", id);
                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new ProductPhoto()
                    {
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        PhotoID = Convert.ToInt32(dbReader["PhotoID"]),
                        Description = Convert.ToString(dbReader["Description"]),
                        Photo = Convert.ToString(dbReader["Photo"]),
                        DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                        IsHidden = Convert.ToBoolean(dbReader["IsHidden"])
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
        public IList<ProductPhoto> List(int productID)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();

            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductPhotos WHERE @ProductID = ProductID ORDER BY DisplayOrder ASC";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@ProductID", productID);


                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new ProductPhoto()
                    {

                        ProductID = Convert.ToInt32(result["ProductID"]),
                        PhotoID = Convert.ToInt32(result["PhotoID"]),
                        Description = Convert.ToString(result["Description"]),
                        Photo = Convert.ToString(result["Photo"]),
                        DisplayOrder = Convert.ToInt32(result["DisplayOrder"]),
                        IsHidden = Convert.ToBoolean(result["IsHidden"])
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
        public bool Update(ProductPhoto data)
        {
            bool result = false;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE ProductPhotos 
                                    SET Photo = @Photo, 
                                        Description = @Description,
                                        DisplayOrder = @DisplayOrder
                                    WHERE PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@PhotoID", data.PhotoID);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@Description", data.Description);

                result = cmd.ExecuteNonQuery() > 0;

                conn.Close();
            }
            return result;
        }
    }
}
