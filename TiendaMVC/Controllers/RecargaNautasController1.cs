using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TiendaMVC.Class;
using TiendaMVC.Extensions.Alerts;
using TiendaMVC.Models;

namespace TiendaMVC.Controllers
{
    public class RecargaNautasController : BaseController
    {
        private TiendaMVCContext db = new TiendaMVCContext();
               

        public ActionResult Index()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else

                return View(db.RecargaNautas.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Estado == EstadoRecarga.Pendiente.ToString()));

        }
               
        public  ActionResult Create()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
           
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( RecargaNauta recargaNauta)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            if (!await ACTUALIZAR_DATOS())
            {
                ShowDanger("No se pudo conectar con el servidor, verifique su conexion a internet o contacte a support.");
                return View();
            }

            if (ModelState.IsValid)
                {
                    recargaNauta.IdCuenta = ID_CUENTA.ToString();
                    recargaNauta.Impuesto = IMPUESTO;
                    recargaNauta.Fecha = DateTime.Now.ToString();
                    recargaNauta.Estado = EstadoRecarga.Pendiente.ToString();

                    db.RecargaNautas.Add(recargaNauta);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }

                return View(recargaNauta);
            
        }      
        public ActionResult Edit(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RecargaNauta recargaNauta = db.RecargaNautas.Find(id);
                if (recargaNauta == null)
                {
                    return HttpNotFound();
                }
                return View(recargaNauta);
            }
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( RecargaNauta recargaNauta)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {
                if (ModelState.IsValid)
                {
                    recargaNauta.IdCuenta = ID_CUENTA.ToString();
                    recargaNauta.Impuesto = IMPUESTO;
                    recargaNauta.Fecha = DateTime.Now.ToString();
                    recargaNauta.Estado = EstadoRecarga.Pendiente.ToString();

                    db.Entry(recargaNauta).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
                return View(recargaNauta);
            }
        }

       
        public ActionResult Delete(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RecargaNauta recargaNauta = db.RecargaNautas.Find(id);
                if (recargaNauta == null)
                {
                    return HttpNotFound();
                }
                return View(recargaNauta);
            }
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {
                RecargaNauta recargaNauta = db.RecargaNautas.Find(id);
                db.RecargaNautas.Remove(recargaNauta);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
