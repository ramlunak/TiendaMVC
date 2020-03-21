using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TiendaMVC.Class;
using TiendaMVC.Models;

namespace TiendaMVC.Controllers
{
    public class RecargaReservadasController : BaseController
    {
        private TiendaMVCContext db = new TiendaMVCContext();



        public ActionResult Index()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
                return View(db.RecargaTienda.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Estado == EstadoRecarga.Reservada.ToString()));

        }

        public ActionResult Create()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            return View();
        }




        public async Task<ActionResult> Agregar(int CodigoPais, string Numero, int Monto, string remitente)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            if ((CodigoPais == null || CodigoPais == 0) || (Numero == null || Numero == "") || (Monto == null || Monto == 0))
                return Json(new ErrorHandling { faul = true, faultstring = "Complete los datos de la recarga." }, JsonRequestBehavior.AllowGet);

            //Validaciones para cuba
            if (CodigoPais == 53)
            {
                var array = Numero.ToCharArray();
                if (array[0].ToString() != "5" || Numero.Length > 8)
                    return Json(new ErrorHandling { faul = true, faultstring = "El número debe tener el formato 5xxxxxxx" }, JsonRequestBehavior.AllowGet);
            }

            if (Numero.Length < 5)
            {
                return Json(new ErrorHandling { faul = true, faultstring = "El número está mal escrito." }, JsonRequestBehavior.AllowGet);
            }


            var ListaRecargasMoviles = ((List<RecargaTienda>)Session["ListaRecargasMoviles"]);
            var id = ListaRecargasMoviles.Count + 1;
            if (ListaRecargasMoviles.Where(x => x.id == id).Any())
                id++;

            var recarga = new RecargaTienda
            {
                id = id,
                IdCuenta = ID_CUENTA,
                CodigoPais = CodigoPais,
                Numero = Numero,
                remitente = remitente,
                Monto = Monto,
                Impuesto = IMPUESTO,
                Fecha = DateTime.Now.To_MM_DD_YYYY()
            };

            if (ListaRecargasMoviles.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Numero == Numero).Any())
            {
                return Json(new ErrorHandling { faul = true, faultstring = "El número ya existe,espere 5 minitos para recargar el mismo número." }, JsonRequestBehavior.AllowGet);
            }
            else
            if (ListaRecargasMoviles.Count >= 10)
            {
                return Json(new ErrorHandling { faul = true, faultstring = "No se admiten mas de 10 recargas por al mismo tiempo." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var fondo = Convert.ToDecimal((float)Session["FONDOS"]);
                if (ListaRecargasMoviles.Sum(x => x.Costo) + recarga.Costo > fondo)
                {
                    return Json(new ErrorHandling { faul = true, faultstring = "No tiene fondos disponibles para agregar esta recarga." }, JsonRequestBehavior.AllowGet);

                }
            }
            ListaRecargasMoviles.Add(recarga);
            Session["ListaRecargasMoviles"] = ListaRecargasMoviles;
            return Json(recarga, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CambiarPassword(string pass)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            var values = pass.Replace('[', ' ').Replace('/', ' ').Replace(']', ' ').Replace('"', ' ').Replace(" ", "");
            String[] array = values.Split(',');


            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                if (((customer_info)Session["CurrentCustomer"]).password == array[0])
                {
                    if (array[1] != array[2])
                    {
                        return Json(new { status = "error", sms = "El campo confirmar contraseña no coincide con la nueva contraseña." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Cambiar Contraseña
                        ((customer_info)Session["CurrentCustomer"]).password = array[1];
                        var result = await ((customer_info)Session["CurrentCustomer"]).Actualizar();
                        if (result)
                            return Json(new { status = "ok", sms = "Su contraseña ha sido cambiada correctamente." }, JsonRequestBehavior.AllowGet);
                        else
                        {

                            ((customer_info)Session["CurrentCustomer"]).password = array[0];
                            return Json(new { status = "error", sms = "No se pudo cambiar su contraseña, contacte con support." }, JsonRequestBehavior.AllowGet);

                        }

                    }
                }
                else
                {
                    return Json(new { status = "error", sms = "La contaseña actual no es correcta" }, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                if (((customer_info)Session["CurrentAccont"]).password == array[0])
                {
                    if (array[1] != array[2])
                    {
                        return Json(new { status = "error", sms = "El campo confirmar contraseña no coincide con la nueva contraseña." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Cambiar Contraseña
                        ((customer_info)Session["CurrentAccont"]).password = array[1];
                        ((customer_info)Session["CurrentAccont"]).i_distributor = await ID_DISTRIBUIDOR();

                        var result = await ((customer_info)Session["CurrentAccont"]).Actualizar();
                        if (result)
                            return Json(new { status = "ok", sms = "Su contraseña ha sido cambiada correctamente." }, JsonRequestBehavior.AllowGet);
                        else
                        {
                            ((customer_info)Session["CurrentAccont"]).password = array[0];
                            return Json(new { status = "error", sms = "No se pudo cambiar su contraseña, contacte con support." }, JsonRequestBehavior.AllowGet);

                        }

                    }
                }
                else
                {
                    return Json(new { status = "error", sms = "La contaseña actual no es correcta" }, JsonRequestBehavior.AllowGet);
                }


            }

            if (!IsLogin()) return RedirectToAction("Index", "Login");
            return Json(new { ok = false, statusText = "no funciona" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RecargaTienda recargaTienda)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            if (!await ACTUALIZAR_DATOS())
            {
                ShowDanger("No se pudo conectar con el servidor, verifique su conexion a internet o contacte a support.");
                return View();
            }

            var result = db.RecargaTienda.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Estado == EstadoRecarga.Pendiente.ToString() && x.Numero == recargaTienda.Numero).ToList();

            if (result.Any())
            {
                ShowDanger("El número ya existe,espere 5 minitos para recargar el mismo número");
                return RedirectToAction("Create");
            }

            if (result.Count >= 10)
            {
                ShowDanger("No se admiten mas de 10 recargas por al mismo tiempo.");
                return RedirectToAction("Create");
            }
            else
            if (ModelState.IsValid)
            {
                recargaTienda.IdCuenta = ID_CUENTA.ToString();
                recargaTienda.TipoRecarga = "M";
                recargaTienda.Impuesto = IMPUESTO;
                recargaTienda.Fecha = DateTime.Now.ToString();
                recargaTienda.Estado = EstadoRecarga.Pendiente.ToString();

                decimal sumaCosto = 0;
                var listaRecargas = db.RecargaTienda.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Estado == EstadoRecarga.Pendiente.ToString()).ToList();
                if (listaRecargas.Any())
                    sumaCosto = listaRecargas.Select(x => x.Costo).Sum();

                var fondos = await FONDOS();
                if (fondos < (float)(recargaTienda.Costo + sumaCosto))
                {
                    ShowDanger("No tiene fondos disponibles para agregar esta recarga.");
                    return RedirectToAction("Create");
                }

                db.RecargaTienda.Add(recargaTienda);
                try
                {
                    db.SaveChanges();
                    ShowSuccess("Acción Completada con éxito");
                }
                catch (Exception ex)
                {
                    ShowDanger("No se pudo completar la acción.");
                }

                return RedirectToAction("Create");
            }

            return View(recargaTienda);

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

                RecargaTienda recargaTienda = db.RecargaTienda.Find(id);

                if (recargaTienda == null)
                {
                    return HttpNotFound();
                }
                return View(recargaTienda);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecargaTienda recargaTienda)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {

                if (ModelState.IsValid)
                {
                    recargaTienda.IdCuenta = ID_CUENTA.ToString();
                    recargaTienda.TipoRecarga = "M";
                    recargaTienda.Impuesto = IMPUESTO;
                    recargaTienda.Fecha = DateTime.Now.ToString();
                    recargaTienda.Estado = EstadoRecarga.Pendiente.ToString();

                    db.Entry(recargaTienda).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        ShowSuccess("Acción Completada con éxito");
                    }
                    catch (Exception ex)
                    {

                        ShowDanger("No se pudo completar la acción.");
                        return View(recargaTienda);
                    }

                    return RedirectToAction("Create");
                }
                return View(recargaTienda);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {
                RecargaTienda recargaTienda = db.RecargaTienda.Find(id);
                try
                {
                    db.RecargaTienda.Remove(recargaTienda);
                    try
                    {
                        db.SaveChanges();
                       return RedirectToAction("Create", "RecargaReservadas");
                        //return Json("delete", JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Create", "RecargaReservadas");
                        //return Json("error", JsonRequestBehavior.DenyGet);
                    }
                                     
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Create", "RecargaReservadas");
                    //  return Json("error", JsonRequestBehavior.DenyGet);
                }
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
