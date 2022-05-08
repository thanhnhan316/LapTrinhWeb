using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV18T1021214.BusinessLayer;
using SV18T1021214.DomainModel;

namespace SV18T1021214.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách quấc gia
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value="",
                Text="--Chọn quấc gia--"
            });

            foreach(var c in CommonDataService.Country_List())
            {
                list.Add(new SelectListItem()
                {
                    Value =c.CountryName,
                    Text = c.CountryName
                });
            }

            return list;
        }
        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Loại hàng --",
            }); ;
            int rowCount = 0;
            foreach (var c in CommonDataService.Category_List(1, 100, "",out rowCount))
            {
                list.Add(new SelectListItem()
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName
                });
            }

            return list;
        }
        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Nhà cung cấp --",
            }); ;
            int rowCount = 0;
            foreach (var c in CommonDataService.Supplier_List(1,10,"",out rowCount))
            {
                list.Add(new SelectListItem()
                {
                    Value = c.SupplierID.ToString(),
                    Text = c.SupplierName
                });
            }

            return list;
        }

    }
}