using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021214.DomainModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SupplierID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Photo { get; set; }
    }
}
