using SV18T1021214.BusinessLayer;
using SV18T1021214.DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;



namespace SV18T1021214.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("employee")]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Employee
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            Models.PaginationSearchInput model = Session["EMPLOYEE_SEARCH"] as Models.PaginationSearchInput;
            string search = Session["EmployeeName_SEARCH"] as string;
            if (model == null)
            {
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
            var data = CommonDataService.Employee_List(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.EmployeePaginationResult model = new Models.EmployeePaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session["EMPLOYEE_SEARCH"] = input;
            return View(model);
        }

        /// <summary>
        /// Bổ sung nhân viên
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Employee model = new Employee()
            {
                EmployeeID = 0
            };

            ViewBag.Title = "Bổ sung nhân viên ";
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa nhân viên
        /// </summary>
        /// <returns></returns>
        /// 
        /// 

        [Route("edit/{employeeID}")]
        public ActionResult Edit(int employeeID)
        {
            Employee model = CommonDataService.GetEmployee(employeeID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Thay đổi thông tin nhân viên";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Employee model,string dayOfBirth, HttpPostedFileBase uploadPhoto)
        {
            // Kiem tra du lieu dau vao
            if (string.IsNullOrWhiteSpace(model.FirstName))
                ModelState.AddModelError("FirstName", "Ho không được để trống");
            if (string.IsNullOrWhiteSpace(model.LastName))
                ModelState.AddModelError("LastName", "Tên không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email không được để trống");
            if (string.IsNullOrWhiteSpace(model.Notes))
                model.Notes = "";
           
            //Chuyển chuỗi ngày có kiểu DMY sang giá trị ngày để lưu vào model.BirthDate
            try
            {
                model.BirthDate = DateTime.ParseExact(dayOfBirth, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                ModelState.AddModelError("BirthDate", "Ngày sinh "+ dayOfBirth+ " không hợp lệ (Phải có dạng ngày/tháng/năm)");
                dayOfBirth = "";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật nhân viên";
                return View("Create", model);
            }


            //upload anh
            if(uploadPhoto != null)
            {
                string path = Server.MapPath("~/Images/Employees");
                string fileName = $"{DateTime.Now.Ticks}-{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(filePath);
                model.Photo = fileName;
            }

            if (model.EmployeeID == 0)
            {
                CommonDataService.AddEmployee(model);
                Session["EmployeeName_SEARCH"] = model.FirstName;
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateEmployee(model);
                return RedirectToAction("Index");
            }

        }
       

        /// <summary>
        /// Xoá nhân viên
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("delete/{employeeID}")]
        public ActionResult Delete(int employeeID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteEmployee(employeeID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetEmployee(employeeID);
            if (model == null)
                return RedirectToAction("Index");
            ViewBag.title = "Xóa nhân viên";
            return View(model);
        }

       
    }
  
}