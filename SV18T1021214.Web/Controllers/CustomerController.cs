using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV18T1021214.BusinessLayer;
using SV18T1021214.DomainModel;

namespace SV18T1021214.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("customer")]
    public class CustomerController : Controller
    {
        /// <summary>
        /// Tìm kiếm,hiển thị danh sách 
        /// </summary>
        /// <returns></returns>
        ///
        /// <summary>
        /// giao diện tìm kiếm
        /// </summary>
        /// <returns></returns>
        // GET: Customer
        public ActionResult Index()
        {
            Models.PaginationSearchInput model = Session["CUSTOMER_SEARCH"] as Models.PaginationSearchInput;
           
            if (model == null)
            {
                string search = Session["CUSTOMER_SEARCH"] as string;
                model = new Models.PaginationSearchInput() { 
                    Page = 1,
                    PageSize = 10,
                    SearchValue = search != null ? search :""
                };

            }
            return View(model);
        }

        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.Customer_List(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.CustomerPaginationResult model = new Models.CustomerPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session["CUSTOMER_SEARCH"] = input;
            return View(model);
        }

        /// <summary>
        /// Giao dien bo sung
        /// Tim kiếm và hiển thị danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Customer model = new Customer()
            {
                CustomerID = 0
            };
            ViewBag.Title = "Bổ sung  khách hàng ";
            return View(model);
        }
        /// <summary>
        /// Chinh Sua Giao Dien
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("edit/{customerID}")]
        public ActionResult Edit(int customerID)
        {
            Customer model = CommonDataService.GetCustomer(customerID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Thay đổi thông tin khách hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Customer model)
        {
            // Ta đang đưa các lỗi vào ModelState
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.CustomerName))
                ModelState.AddModelError("CustomerName", "Tên khách hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError("Address", "Địa chỉ không được để trống");
            if (string.IsNullOrWhiteSpace(model.Country))
                ModelState.AddModelError("Country", "Quốc gia không được để trống");
            if (string.IsNullOrWhiteSpace(model.City))
                model.City = "";
            if (string.IsNullOrWhiteSpace(model.PostalCode))
                model.PostalCode = "";

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CustomerID == 0 ? "Bổ sung khách hàng" : "Thay đổi thông tin khách hàng";
                return View("Create",model);
            }
            // Lưu dữ liệu
            if (model.CustomerID == 0)
            {
                CommonDataService.AddCustomer(model);
                Session["CUSTOMER_SEARCH"] = model.CustomerName;
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateCustomer(model);
                return RedirectToAction("Index");
            }

        }
        /// <summary>
        /// Xoa
        /// </summary>
        /// <returns></returns>
        /// 

        [Route("delete/{customerID}")]
        public ActionResult Delete(int customerID)
        {
            ViewBag.Title = "Xóa khach hàng";
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCustomer(customerID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCustomer(customerID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}