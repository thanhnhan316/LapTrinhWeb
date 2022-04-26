using SV18T1021214.BusinessLayer;
using SV18T1021214.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SV18T1021214.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    
    [Authorize]
    [RoutePrefix("category")]
    public class CategoryController: Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
       public ActionResult Index(int page = 1, string searchValue = "")
        {
            ViewBag.Title = "Loại hàng";
            int pageSize = 5;
            int rowCount = 0;
            var data = CommonDataService.Category_List(page, pageSize, searchValue, out rowCount);
            Models.CategoryPaginationResult model = new Models.CategoryPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data,

            };

            return View(model);
        }

        /// <summary>
        /// Bổ sung loại hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Category model = new Category()
            {
                CategoryID = 0
            };
            ViewBag.Title = "Bổ sung  loại hàng ";
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa loại hàng
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("edit/{categoryID}")]
        public ActionResult Edit(int categoryID)
        {
            Category model = CommonDataService.GetCategory(categoryID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Thay đổi thông tin loaị hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Category model)
        {
            //TODO: Kiểm tra dữ liệu đầu vào

            if (model.CategoryID == 0)
            {
                CommonDataService.AddCategory(model);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateCategory(model);
                return RedirectToAction("Index");
            }

        }
        /// <summary>
        /// Xoá Loại Hàng
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("delete/{categoryID}")]
        public ActionResult Delete(int categoryID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCategory(categoryID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCategory(categoryID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}