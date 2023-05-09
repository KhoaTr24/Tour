using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelTour.Models;
using System.IO;

namespace TravelTour.Controllers
{
    public class AdminController : Controller
    {
        dbTravelTourDataContext data = new dbTravelTourDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["AccountAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection cl)
        {
            var tendn = cl["usernamead"];
            var pass = cl["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["ThongBaoLoi1"] = "Phải nhập tài khoản";
            }
            else if (String.IsNullOrEmpty(pass))
            {
                ViewData["ThongBaoLoi2"] = "Mật khẩu bắt buộc";
            }

            //Gán giá trị cho đối tượng được tạo
            Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == pass);
            if (ad != null)
            {
                Session["AccountAdmin"] = ad;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.ThongBao = "Tài Khoản hoặc Password không đúng";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Tour()
        {
            if (Session["AccountAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(data.Tours.ToList());
        }
        public ActionResult QLDH()
        {
            if (Session["AccountAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(data.DONDATHANGs.ToList());
        }
        public ActionResult QLTK()
        {
            if (Session["AccountAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(data.KHACHHANGs.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaCDKV = new SelectList(data.CHUDEKVs.ToList().OrderBy(n => n.TenChuDe), "MaCDKV", "TenChuDe");
            ViewBag.MaCDKVNN = new SelectList(data.CHUDEKVNNs.ToList().OrderBy(n => n.TenChuDe), "MaCDKVNN", "TenChuDe");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tour tour, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaCDKV = new SelectList(data.CHUDEKVs.ToList().OrderBy(n => n.TenChuDe), "MaCDKV", "TenChuDe");
            ViewBag.MaCDKVNN = new SelectList(data.CHUDEKVNNs.ToList().OrderBy(n => n.TenChuDe), "MaCDKVNN", "TenChuDe");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chọn 1 ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu file vao duong dan~
                    var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    tour.Anhbia = fileName;
                    //Luu vao sql
                    data.Tours.InsertOnSubmit(tour);
                    data.SubmitChanges();
                }
                return RedirectToAction("Tour", "Admin");
            }
        }
        //HIEN THI CHI TIET
        public ActionResult Detail(int id)
        {
            //Lay tour theo id
            Tour tour = data.Tours.SingleOrDefault(n => n.MaTour == id);
            ViewBag.MaTour = tour.MaTour;
            if (tour == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tour);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Tour tour = data.Tours.SingleOrDefault(n => n.MaTour == id);
            ViewBag.MaTour = tour.MaTour;
            if (tour == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tour);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id) // cho BeginForm nút xác nhận xóa bên view Delete
        {
            Tour tour = data.Tours.SingleOrDefault(n => n.MaTour == id);//Lấy dữ liệu điện thoại từ DB theo id 
            ViewBag.MaTour = tour.MaTour;
            if (tour == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.Tours.DeleteOnSubmit(tour);
            data.SubmitChanges();
            return RedirectToAction("Tour", "Admin");
        }
        //Hết xóa sản phẩm admin

        [HttpGet]
        public ActionResult DeleteTK(int id)
        {
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("DeleteTK")]
        public ActionResult XacnhanxoaTK(int id) // cho BeginForm nút xác nhận xóa bên view Delete
        {
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);//Lấy dữ liệu điện thoại từ DB theo id
            ViewBag.MaKH = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHACHHANGs.DeleteOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("QLTK", "Admin");
        }

        [HttpGet]
        public ActionResult DeleteDH(int id)
        {
            DONDATHANG dh = data.DONDATHANGs.SingleOrDefault(n => n.MaDH == id);
            ViewBag.MaDH = dh.MaDH;
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }

        [HttpPost, ActionName("DeleteDH")]
        public ActionResult XacnhanxoaDH(int id) // cho BeginForm nút xác nhận xóa bên view Delete
        {
            DONDATHANG dh = data.DONDATHANGs.SingleOrDefault(n => n.MaDH == id);
            ViewBag.MaDH = dh.MaDH;
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.DONDATHANGs.DeleteOnSubmit(dh);
            data.SubmitChanges();
            return RedirectToAction("QLDH", "Admin");
        }
        //Hết xóa sản phẩm admin

        [HttpGet]
        public ActionResult EditTK(int id)
        {
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
         
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditTK(KHACHHANG kh)
        {
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            } 
            UpdateModel(kh);
            data.SubmitChanges();
            return RedirectToAction("QLTK", "Admin");
        }

    }
}