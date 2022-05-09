using SV18T1021214.BusinessLayer;
using SV18T1021214.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021214.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>

    [Authorize]
    [RoutePrefix("supplier")]
    public class SupplierController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult Index()
        {
            Models.PaginationSearchInput model = Session["SUPPLIER_SEARCH"] as Models.PaginationSearchInput;
            if (model == null)
            {
                string search = Session["SUPPLIER_SEARCH"] as string;
                model = new Models.PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = search!=null ? search: ""
                };

            }
            return View(model);
        }
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.Supplier_List(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.ShupplierPaginationResult model = new Models.ShupplierPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session["SUPPLIER_SEARCH"] = input;
            return View(model);
        }
        /// <summary>
        /// Bổ sung nhà cung cấp.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Supplier model = new Supplier()
            {
                SupplierID = 0
            };

            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa nhà cung cấp
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("edit/{supplierID}")]
        public ActionResult Edit(int supplierID)
        {
            Supplier model = CommonDataService.GetSupplier(supplierID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Thay đổi thông tin nhà cung cấp";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Supplier model)
        {
            // Ta đang đưa các lỗi vào ModelState
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.SupplierName))
                ModelState.AddModelError("SupplierName", "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError("ContactName", "Tên liên lạc không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError("Address", "Địa chỉ không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
            if (string.IsNullOrWhiteSpace(model.Country))
                ModelState.AddModelError("Country", "Tên quốc gia không được để trống");

            if (string.IsNullOrWhiteSpace(model.City))
                model.City = "";
            if (string.IsNullOrWhiteSpace(model.PostalCode))
                model.PostalCode = "";

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.SupplierID == 0 ? "Bổ sung khách hàng" : "Thay đổi thông tin khách hàng";
                return View("Create", model);
            }

            // Lưu dữ liệu
            if (model.SupplierID == 0)
            {
                CommonDataService.AddSupplier(model);
                Session["SUPPLIER_SEARCH"] = model.SupplierName;

                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateSupplier(model);
                return RedirectToAction("Index");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("delete/{supplierID}")]
        public ActionResult Delete(int supplierID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteSupplier(supplierID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetSupplier(supplierID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}