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
    }
}