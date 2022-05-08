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
            Models.PaginationSearchInput model = Session["CATEGOTY_SEARCH"] as Models.PaginationSearchInput;
        
            if (model == null)
            {
                string search = Session["CATEGOTY_SEARCH"] as string;
                model = new Models.PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = search != "" ? search : ""
                };

            }
            return View(model);
        }

        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.Category_List(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.CategoryPaginationResult model = new Models.CategoryPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session["CATEGOTY_SEARCH"] = input;
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

            // Ta đang đưa các lỗi vào ModelState
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.CategoryName))
                ModelState.AddModelError("CategoryName", "Tên loại hàng không được để trống");

            if (string.IsNullOrWhiteSpace(model.Description))
                model.Description = "";

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? "Bổ sung loại hàng" : "Thay đổi thông tin loại hàng";
                return View("Create", model);
            }
            // Lưu dữ liệu

            if (model.CategoryID == 0)
            {
                CommonDataService.AddCategory(model);
                Session["CATEGOTY_SEARCH"] = model.CategoryName;
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