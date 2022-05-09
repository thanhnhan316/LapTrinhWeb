using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV18T1021214.DomainModel;

namespace SV18T1021214.DataLayer.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// thêm một mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Product data)
        {
            int result = 0;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products(ProductName, SupplierID, CategoryID, Unit, Price, Photo)
                                    VALUES (@ProductName, @SupplierID, @CategoryID, @Unit, @Price, @Photo)
                                    SELECT SCOPE_IDENTITY()";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@Unit", data.Unit);
                cmd.Parameters.AddWithValue("@Price", data.Price);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);

                result = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return result;
        }
        /// <summary>
        /// đếm số lượng mặt hàng tìm kiếm được 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="category"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public int Count(string searchValue, int category, int supplier)
        {
            int count = 0;
            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT    COUNT(*)
                                    FROM      Products 
                                    WHERE    (@searchValue = N'')
                                         OR ((Products.ProductName LIKE @searchValue)
                                                    AND ((@CategoryID = 0) OR (CategoryID = @CategoryID))
                                                    AND ((@SupplierID = 0) OR (SupplierID = @SupplierID))
                                            )";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@SupplierID", supplier);
                cmd.Parameters.AddWithValue("@CategoryID", category);


                count = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }

            return count;
        }
        /// <summary>
        /// xóa một mặt hàng trong csdl
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public bool Delete(int productID)
        {
            bool result = false;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
                                    DELETE FROM ProductAttributes WHERE ProductID = @ProductID
                                    DELETE FROM ProductPhotos WHERE ProductID = @ProductID
                                    DELETE FROM Products WHERE ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ProductID", productID);

                result = cmd.ExecuteNonQuery() > 0;
                conn.Close();
            }
            return result;
        }
        /// <summary>
        /// lấy thông tin một mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Product Get(int productID)
        {
            Product result = null;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ProductID", productID);
                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Product()
                    {
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        ProductName = Convert.ToString(dbReader["ProductName"]),
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                        Unit = Convert.ToString(dbReader["Unit"]),
                        Price = Convert.ToString(dbReader["Price"]),
                        Photo = Convert.ToString(dbReader["Photo"]),
                    };
                }
                conn.Close();
            }
            return result;
        }
        /// <summary>
        /// check sản phẩm có thể xóa hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool InUsed(int id)
        {
            bool result = false;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT CASE WHEN EXISTS(SELECT * FROM OrderDetails WHERE ProductID = @ProductID) THEN 1 ELSE 0 END";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ProductID", id);

                result = Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                conn.Close();
            }
            return result;
        }
        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool Update(Product data)
        {
            bool result = false;
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products 
                                    SET ProductName = @ProductName, 
                                        SupplierID = @SupplierID, 
                                        CategoryID = @CategoryID,
                                        Unit = @Unit,
                                        Price = @Price,
                                        Photo = @Photo
                                    WHERE ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@Unit", data.Unit);
                cmd.Parameters.AddWithValue("@Price", data.Price);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);

                result = cmd.ExecuteNonQuery() > 0;
                conn.Close();
            }
            return result;
        }
        /// <summary>
        /// Tìm kiếm danh sách sản phẩm theo key, mã loại, mã ncc , phân trang
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public List<Product> List(string searchValue, int pageSize, int page, int categoryID, int supplierID)
        {
            List<Product> data = new List<Product>();
            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
           
            using (SqlConnection conn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT    *
                                    FROM
                                    (
                                        SELECT    ROW_NUMBER() OVER(ORDER BY Products.ProductName) AS RowNumber, Products.*
                                        FROM    Products  
                                        WHERE    (@searchValue = N'')
                                            OR    (
                                                    (Products.ProductName LIKE @searchValue)
                                                    AND ((@CategoryID = 0) OR (CategoryID = @CategoryID))
                                                    AND ((@SupplierID = 0) OR (SupplierID = @SupplierID))
                                                )
                                    ) AS t
                                     WHERE (@pageSize=0) OR (t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new Product()
                    {
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        ProductName = Convert.ToString(result["ProductName"]),
                        CategoryID = Convert.ToInt32(result["CategoryID"]),
                        SupplierID = Convert.ToInt32(result["SupplierID"]),
                        Photo = Convert.ToString(result["Photo"]),
                        Price = Convert.ToString(result["Price"]),
                        Unit = Convert.ToString(result["Unit"]),
                    });
                }
                result.Close();
                conn.Close();
            }
            return data;
        }
    }
}

