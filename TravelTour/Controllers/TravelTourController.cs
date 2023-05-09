using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelTour.Models;

namespace TravelTour.Controllers
{
    public class TravelTourController : Controller
    {
        dbTravelTourDataContext data = new dbTravelTourDataContext();
        // GET: TravelTour
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tour()
        {
            var tour = from cd in data.Tours select cd;
            return View(tour);
        }

        public ActionResult Chudekv()
        {
            var chudekv = from cd in data.CHUDEs select cd;
            return PartialView(chudekv);
        }

        public ActionResult Khuvuc()
        {
            var khuvuc = from cd in data.CHUDEKVs select cd;
            return PartialView(khuvuc);
        }

        public ActionResult Khuvucnn()
        {
            var khuvucnn = from cd in data.CHUDEKVNNs select cd;
            return PartialView(khuvucnn);
        }

        public ActionResult SPTheoKV(int id)
        {
            var tour = from s in data.Tours where s.MaCDKV == id select s;
            return View(tour);
        }

        public ActionResult SPTheoKVNN(int id)
        {
            var tour = from s in data.Tours where s.MaCDKVNN == id select s;
            return View(tour);
        }

        public ActionResult SPTheoNuoc(int id)
        {
            var tour = from s in data.Tours where s.MaCD == id select s;
            return View(tour);
        }
         public ActionResult Detail(int id)
        {
            var tour = from s in data.Tours where s.MaTour == id select s;
            return View(tour.Single());
        }

        public ActionResult Search(string searchString)
        {
            
            var search = from s in data.Tours // lấy toàn bộ liên kết
            select s;
            
            if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                if (searchString == null)
                {
                    Url.Action("Index", "TravelTour");
                }
                search = search.Where(s => s.TenTour.Contains(searchString)); //lọc theo chuỗi tìm kiếm
            }
            return View(search); //trả về kết quả
        }
    }

    
}