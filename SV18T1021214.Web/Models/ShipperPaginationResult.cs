using SV18T1021214.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021214.Web.Models
{
    /// <summary>
    /// kết quả tìm kiếm phân trang của khách hàng
    /// </summary>
    public class ShipperPaginationResult : BasePaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Shipper> Data { get; set; }
    }
}