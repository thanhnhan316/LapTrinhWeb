using System;
using System.Collections.Generic;
using System.IO;
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
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Models.ProductPaginationSearchInput model = Session["PRODUCT_SEARCH"] as Models.ProductPaginationSearchInput;

            if (model == null)
            {
                string search = Session["PRODUCT_SEARCH"] as string;
                model = new Models.ProductPaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = search != null ? search :"",
                    CategoryID = 0,
                    SupplierID = 0
                };

            }
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.ProductPaginationSearchInput input)
        {
            int rowCount = 0;
       
            var data = CommonDataService.Product_List(input.Page, input.PageSize, input.SearchValue, out rowCount,input.CategoryID,input.SupplierID);
            Models.ProductPaginationResult model = new Models.ProductPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data,
                categoryID = input.CategoryID,
                supplierID = input.SupplierID
            };
            Session["PRODUCT_SEARCH"] = input;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Product model = new Product()
            {
                ProductID = 0,
            };
            ViewBag.Title = "Bổ sung mặt hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("edit/{productID}")]
        public ActionResult Edit(int productID)
        {
            Product model = CommonDataService.GetProduct(productID);
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Product model, HttpPostedFileBase uploadPhoto)
        {
            // check valid input
            if (string.IsNullOrWhiteSpace(model.ProductName))
                ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(model.Unit))
                ModelState.AddModelError("Unit", "Đơn vị mặt hàng hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(model.Price))
                ModelState.AddModelError("Price", "Giá mặt hàng không được để trống.");
            else
            {
                string[] subs = (model.Price).Split('.');
                if (subs.Length > 2) ModelState.AddModelError("Price", "Giá mặc hàng không hợp lệ. Vd: 50, 45.5, 199.55");
                else foreach (var sub in subs)
                    {
                        var isNumeric = int.TryParse(sub, out _);
                        if (!isNumeric)
                        {
                            ModelState.AddModelError("Price", "Giá mặc hàng không hợp lệ. Vd: 50, 45.5, 199.55");
                            break;
                        }
                    }
            }
            if (model.CategoryID == 0)
                ModelState.AddModelError("CategoryID", "Vui lòng chọn loại hàng.");
            if (model.SupplierID == 0)
                ModelState.AddModelError("SupplierID", "Vui lòng chọn nhà cung cấp.");
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProductID == 0 ? "Cập nhật thông tin mặt hàng" : "Bổ sung mặt hàng";
                return View(model.ProductID == 0 ? "Create" : "Edit", model);
            }
            // upload file picture
            if (uploadPhoto != null)
            {
                string _FileName = DateTime.Now.Ticks + "-" + Path.GetFileName(uploadPhoto.FileName);
                string _path = Path.Combine(Server.MapPath("~/Images/Products"), _FileName);
                uploadPhoto.SaveAs(_path);
                model.Photo = _FileName;
            }
            if (model.ProductID == 0)
            {
                if (uploadPhoto == null) model.Photo = "";
                CommonDataService.AddProduct(model);
                Session["PRODUCT_SEARCH"] = model.ProductName;
            }
            else
            {   // delete img if update photo
             /*   if (uploadPhoto != null)
                {
                    string fullPath = Server.MapPath("~/Images/Products/" + CommonDataService.GetProduct(model.ProductID).Photo);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }*/
                CommonDataService.UpdateProduct(model);
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("delete/{productID}")]
        public ActionResult Delete(int productID)
        {
            var model = CommonDataService.GetProduct(productID);
            if (Request.HttpMethod == "POST")
            {
                // delete image product
                if (string.IsNullOrWhiteSpace(model.Photo))
                {
                    string fullPath = Server.MapPath("~/Images/Products/" + model.Photo);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }
                // delete image product photo
                foreach (var p in CommonDataService.ListOfProductPhotos(productID))
                {
                    string fullPath = Server.MapPath("~/Images/ProductPhotos/" + p.Photo);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }
                CommonDataService.DeleteProduct(productID);
                return RedirectToAction("Index");
            }
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        [Route("photo/{method}/{productID}/{photoID?}")]
        public ActionResult Photo(string method, int productID, int? photoID)
        {
            ProductPhoto model = new ProductPhoto();
            switch (method)
            {
                case "add":
                    model.PhotoID = 0;
                    ViewBag.Title = "Bổ sung ảnh";
                    break;
                case "edit":
                    model = CommonDataService.GetProductPhoto(photoID.Value);
                    if (model == null)
                        return RedirectToAction("Edit", new { productID = productID });
                    ViewBag.Title = "Thay đổi ảnh";
                    break;
                case "delete":
                    var mode = CommonDataService.GetProductPhoto(photoID.Value);
                    string fullPath = Server.MapPath("~/Images/ProductPhotos/" + mode.Photo);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                    CommonDataService.DeleteProductPhoto(photoID.Value);
                    return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uploadPhoto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePhoto(ProductPhoto model, HttpPostedFileBase uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(model.Description))
                ModelState.AddModelError("Description", "Mô tả/ tiêu đề không được để trống.");
            if (model.DisplayOrder < 0)
                ModelState.AddModelError("DisplayOrder", "Thứ tự không bé hơn 0.");
            else
            {
                foreach (var item in CommonDataService.ListOfProductPhotos(model.ProductID))
                {
                    if (item.DisplayOrder == model.DisplayOrder && item.PhotoID != model.PhotoID)
                    {
                        ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị đã tồn tại.");
                        break;
                    }
                }
            }
            // upload file picture
            if (uploadPhoto != null)
            {
                string _FileName = DateTime.Now.Ticks + "-" + Path.GetFileName(uploadPhoto.FileName);
                string _path = Path.Combine(Server.MapPath("~/Images/ProductPhotos"), _FileName);
                uploadPhoto.SaveAs(_path);
                model.Photo = _FileName;
            }
            else if (model.PhotoID == 0) ModelState.AddModelError("Photo", "Ảnh không được để trống.");

            if (!ModelState.IsValid)
                return View("Photo", model);

            if (model.PhotoID == 0)
                CommonDataService.AddProductPhoto(model);
            else
            {
                if (uploadPhoto != null)
                {
                    string fullPath = Server.MapPath("~/Images/ProductPhotos/" + CommonDataService.GetProductPhoto(model.PhotoID).Photo);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }
                CommonDataService.UpdateProductPhoto(model);
            }
            return RedirectToAction("Edit", new { productID = model.ProductID });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("attribute/{method}/{productID}/{attributeID?}")]
        public ActionResult Attribute(string method, int productID, int? attributeID)
        {
            ProductAttribute model = new ProductAttribute();
            switch (method)
            {
                case "add":
                    model.AttributeID = 0;
                    ViewBag.Title = "Bổ sung thuộc tính";
                    break;
                case "edit":
                    model = CommonDataService.GetProductAttribute(attributeID.Value);
                    if (model == null)
                        return RedirectToAction("Edit", new { productID = productID });
                    ViewBag.Title = "Thay đổi thuộc tính";
                    break;
                case "delete":
                    CommonDataService.DeleteProductAttribute(attributeID.Value);
                    return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uploadPhoto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAttribute(ProductAttribute model, HttpPostedFileBase uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(model.AttributeName))
                ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống.");
            if (string.IsNullOrWhiteSpace(model.AttributeValue))
                ModelState.AddModelError("AttributeValue", "Giá trị thuộc tính không được để trống.");
            if (model.DisplayOrder < 0)
                ModelState.AddModelError("DisplayOrder", "Thứ tự không bé hơn 0.");
            else
            {
                foreach (var item in CommonDataService.ListOfProductAttributes(model.ProductID))
                {
                    if (item.DisplayOrder == model.DisplayOrder && item.AttributeID != model.AttributeID)
                    {
                        ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị đã tồn tại.");
                        break;
                    }
                }
            }
            if (!ModelState.IsValid)
                return View("Attribute", model);
            if (model.AttributeID == 0)
                CommonDataService.AddProductAttribute(model);
            else
                CommonDataService.UpdateProductAttribute(model);
            return RedirectToAction("Edit", new { productID = model.ProductID });
        }
    }

}