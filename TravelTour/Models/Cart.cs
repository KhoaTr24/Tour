using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelTour.Models
{
    public class Cart
    {
        dbTravelTourDataContext data = new dbTravelTourDataContext();
        public int iMatour { set; get; }
        public string tTenTour { set; get; }
        public string tAnhbia { set; get; }
        public Double dGiaban { set; get; }
        public int iSoluong { set; get; }
        public string tThoigian { set; get; }
        public string tVanchuyen { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dGiaban; }
        }

        //Khởi tạo giỏ hàng theo mã đth được truyền vào với số lượng mặc định là 1

        public Cart(int MaTour)
        {
            iMatour = MaTour;
            Tour tour = data.Tours.Single(n => n.MaTour == iMatour);
            tTenTour = tour.TenTour;
            tThoigian = tour.Thoigian;
            tVanchuyen = tour.Vanchuyen;
            tAnhbia = tour.Anhbia;
            dGiaban = double.Parse(tour.Giaban.ToString());
            iSoluong = 1;
        }
    }
}