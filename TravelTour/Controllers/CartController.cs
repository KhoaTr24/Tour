using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelTour.Models;

namespace TravelTour.Controllers
{
    public class CartController : Controller
    {

        dbTravelTourDataContext data = new dbTravelTourDataContext();

        //Lay gio hang, cập nhật giỏ hàng nếu có thông qua Session["Cart"]
        public List<Cart> Laygiohang()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }

        //Them gio hang
        public ActionResult AddCart(int iMaTour, string strURL)
        {
            //Lay session gio hang
            List<Cart> lstCart = Laygiohang();
            //Kiem tra có tồn tại chưa, nếu có +1 vào số lượng
            Cart sanpham = lstCart.Find(n => n.iMatour == iMaTour);
            if (sanpham == null)
            {
                sanpham = new Cart(iMaTour);
                lstCart.Add(sanpham);
                return RedirectToAction("Cart", "Cart");
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        //Tổng số lượng

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                iTongSoLuong = lstCart.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }

        //Tong Tien
        private double Tongtien()
        {
            double iTongtien = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                iTongtien = lstCart.Sum(n => n.dThanhtien);
            }
            return iTongtien;
        }
        [HttpGet]
        public ActionResult Cart()
        {

            List<Cart> lstCart = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();
            return View(lstCart);
        }

        [HttpPost]
        public ActionResult Cart(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Account"];
            List<Cart> gh = Laygiohang();
            if (Session["Account"] != null)
            {
                ddh.MaKH = kh.MaKH;
                ddh.HoTen = kh.HoTen;
                ddh.DienthoaiKH = kh.DienthoaiKH;
                ddh.DiachiKH = kh.DiachiKH;
                ddh.Email = kh.Email;
                ddh.Tinhtranggiaohang = false;
                ddh.Dathanhtoan = false;
                data.DONDATHANGs.InsertOnSubmit(ddh);
                data.SubmitChanges();
            
                Session["Cart"] = null;
            }
            if(Session["Account"]==null)
            {
                var hoten = collection["HoTenKH"];
                var email = collection["Email"];
                var dienthoai = collection["Dienthoai"];
                var diachi = collection["Diachi"];
                ddh.HoTen = hoten;
                ddh.Email = email;
                ddh.Tinhtranggiaohang = false;
                ddh.Dathanhtoan = false;
                ddh.DienthoaiKH = dienthoai;
                ddh.DiachiKH = diachi;
                data.DONDATHANGs.InsertOnSubmit(ddh);
                data.SubmitChanges();
                Session["Cart"] = null;
            }
            return RedirectToAction("ConfirmCart", "Cart");
        }

        public ActionResult ConfirmCart()
        {
            return View();
        }

        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
    }
}