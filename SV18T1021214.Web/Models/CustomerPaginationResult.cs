using SV18T1021214.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021214.Web.Models
{
    /// <summary>
    /// kết quả tìm kiếm, phân trang của khach hàng
    /// </summary>
    public class CustomerPaginationResult: BasePaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Customer>  Data { get; set; }

       
    }
}