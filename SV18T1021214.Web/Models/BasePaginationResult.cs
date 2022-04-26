using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021214.Web.Models
{
    /// <summary>
    /// lớp cơ sở (lớp cha) cho các lớp lưu trũ các dữ  liệu
    /// liên quan đến truy vấn phân trang
    /// </summary>
    public class BasePaginationResult
    {
        /// <summary>
        /// trang cần xem
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// số dòng trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// giá trị tìm kiếm
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// tổng số dòng
        /// </summary>
        public int RowCount { get; set; }
        
        public int PageCount {
            get
            {
                if (PageSize == 0)
                    return 1;
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;

                return p;

            } }
    }
}