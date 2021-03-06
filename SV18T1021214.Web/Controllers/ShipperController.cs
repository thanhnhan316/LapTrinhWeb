using SV18T1021214.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV18T1021214.DomainModel;

namespace SV18T1021214.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("shipper")]
    public class ShipperController : Controller
    {
        /// <summary>
        /// Tim kiem,hien thi và xoá danh sách
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult Index(int page = 1, string searchValue = "")
        {
            Models.PaginationSearchInput model = Session["SHIPPER_SEARCH"] as Models.PaginationSearchInput;
            if (model == null)
            {
                string search = Session["SHIPPER_SEARCH"] as string;

                model = new Models.PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = search != null ? search : ""
                };

            }
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.Shipper_List(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.ShipperPaginationResult model = new Models.ShipperPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session["SHIPPER_SEARCH"] = input;
            return View(model);
        }
        /// <summary>
        /// Bổ sung người giao hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Shipper model = new Shipper()
            {
                ShipperID = 0
            };
            ViewBag.Title = "Bổ sung  người giao hàng ";
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa người giao hàng
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("edit/{shipperID}")]
        public ActionResult Edit(int shipperID)
        {
            Console.Write(shipperID);
            Shipper model = CommonDataService.GetShipper(shipperID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Thay đổi thông tin người giao hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult Save(Shipper model)
        {
            // Ta đang đưa các lỗi vào ModelState
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.ShipperName))
                ModelState.AddModelError("ShipperName", "Tên người giao hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
          

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ShipperID == 0 ? "Bổ sung Người giao hàng" : "Thay đổi thông tin người giao hàng";
                return View("Create", model);
            }
            // Lưu dữ liệu

            if (model.ShipperID == 0)
            {
                CommonDataService.AddShipper(model);
                Session["SHIPPER_SEARCH"] = model.ShipperName;
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateShipper(model);
                return RedirectToAction("Index");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("delete/{shipperID}")]
        public ActionResult Delete(int shipperID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteShipper(shipperID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetShipper(shipperID);
            if (model == null)
                return RedirectToAction("Index");
            ViewBag.Title = "Xóa người giao hàng ";
            return View(model);
        }


    }
}