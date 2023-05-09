﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelTour.Models;
namespace TravelTour.Controllers
{
    public class AccountController : Controller
    {
        dbTravelTourDataContext data = new dbTravelTourDataContext();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var email = collection["Email"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["Nlmatkhau"];
            var dienthoai = collection["Dienthoai"];
            var diachi = collection["Diachi"];
            var ngaysinh = String.Format("{0:dd/MM/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được để trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi4"] = "Bắt buộc";
            }
            if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi5"] = "Phải nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi2"] = "Bắt buộc";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Nhập số điện thoại";
            }
            if (String.IsNullOrEmpty(ngaysinh))
            {
                ViewData["Loi7"] = "Nhập ngày sinh";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DienthoaiKH = (dienthoai);
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                kh.DiachiKH = diachi;
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.Signup();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var email = collection["Email"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(email))
            {
                ViewBag.ThongBaoLoi1 = "Phải Nhập Email";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewBag.ThongBaoLoi2 = "Mật khẩu bắt buộc";
            }
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.Email == email && n.Matkhau == matkhau);
                if (kh != null)
                {
                    Session["Account"] = kh;
                    return RedirectToAction("Index","TravelTour");
                }
                else
                {
                    ViewBag.ThongBao = "Email hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "TravelTour");
        }
    }
}
