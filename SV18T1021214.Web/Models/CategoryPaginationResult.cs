using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SV18T1021214.DomainModel;

namespace SV18T1021214.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CategoryPaginationResult : BasePaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Category> Data { get; set; }
}
}