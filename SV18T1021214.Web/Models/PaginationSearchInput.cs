using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021214.Web.Models
{
    /// <summary>
    /// Dữ liệu đầu vào tìm kiếm phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        /// <summary>
        /// 
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SearchValue { get; set; }

    }

    public class ProductPaginationSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }

    }
}