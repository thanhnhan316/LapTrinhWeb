using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SV18T1021214.DomainModel;


namespace SV18T1021214.Web.Models
{
    /// <summary>
    /// kết quả tìm kiếm, phân trang của mặt hàng
    /// </summary>
    public class ProductPaginationResult : BasePaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Product> Data { get; set; }
        public int categoryID { get; set; }
        public int supplierID { get; set; }

    }
}