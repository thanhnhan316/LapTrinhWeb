﻿using SV18T1021214.BusinessLayer;
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
            ViewBag.Title = "Nhân viên";
            int pageSize = 5;
            int rowCount = 0;
            var data = CommonDataService.Employee_List(page, pageSize, searchValue, out rowCount);
            Models.EmployeePaginationResult model = new Models.EmployeePaginationResult()
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
        public ActionResult Save(Employee model)
        {
            // Ta đang đưa các lỗi vào ModelState
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.FirstName))
                ModelState.AddModelError("FirstName", "Họ tên đệm không được để trống");
            if (string.IsNullOrWhiteSpace(model.LastName))
                ModelState.AddModelError("LastName", "Tên không được để trống");
           /* if (string.IsNullOrWhiteSpace(model.BirthDate))
                ModelState.AddModelError("Address", "Địa chỉ không được để trống");*/
            if (string.IsNullOrWhiteSpace(model.Photo))
                ModelState.AddModelError("Photo", "Ảnh không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email không được để trống");
          
            if (string.IsNullOrWhiteSpace(model.Notes))
                model.Notes = "";
           

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "Thay đổi thông tin nhân viên";
                return View("Create", model);
            }
            // Lưu dữ liệu

            if (model.EmployeeID == 0)
            {
                CommonDataService.AddEmployee(model);
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
            return View(model);
        }
    }
}