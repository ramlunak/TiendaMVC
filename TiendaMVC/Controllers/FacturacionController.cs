using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TiendaMVC.Class;
using TiendaMVC.Models;

namespace TiendaMVC.Controllers
{
    public class FacturacionController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
                       

            //DateTime dt1 = DateTime.UtcNow;
            //DateTime wkStDt = DateTime.MinValue;
            //wkStDt = dt1.AddDays(1 - Convert.ToDouble(dt1.DayOfWeek));
            //string inicioSemana = wkStDt.Date.ToString("yyyy/MM/dd HH:mm:ss");
            //var dt = DateTime.Parse(inicioSemana);           
            //var utc_from = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");
            var utc_from = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(_Global.FromDate), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");


            //string finSemana = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            //var to = DateTime.Parse(finSemana);
            //var utc_to = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(to, "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");
            var utc_to = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(_Global.ToDate), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");

            var facturacion = new Facturacion();

            Session["FacturacionFromDate"] = _Global.FromDate;
            Session["FacturacionToDate"] = _Global.ToDate;

            facturacion.XDRListResponse = await ((customer_info)Session["CurrentCustomer"]).GetCustomerXDR(new GetRetailCustomerXDRListRequest { from_date = utc_from, to_date = utc_to });
            await facturacion.Cargar((customer_info)Session["CurrentCustomer"]);
            


            return View(facturacion);
        }
        
      
    }
}
