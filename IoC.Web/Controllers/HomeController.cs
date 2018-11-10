using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IoC.Web.Models;
using IoC.Interfaces;

namespace IoC.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IHotelServices _IHotelServices;
        public HomeController(IHotelServices hotelServices)
        {
            this._IHotelServices = hotelServices;
        }
        public IActionResult Index()
        {
            var hotelList = this._IHotelServices.GetHotelAll();
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
