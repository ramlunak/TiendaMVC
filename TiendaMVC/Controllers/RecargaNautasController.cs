using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TiendaMVC.Class;
using TiendaMVC.Models;

namespace TiendaMVC.Controllers
{
    public class RecargaNautasController : BaseController
    {
        private TiendaMVCContext db = new TiendaMVCContext();

        public async Task<ActionResult> Historial()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            //DateTime dt1 = DateTime.Now;
            //DateTime wkStDt = DateTime.MinValue;
            //wkStDt = dt1.AddDays(1 - Convert.ToDouble(dt1.DayOfWeek));
            //string inicioSemana = wkStDt.Date.ToString("yyyy/MM/dd HH:mm:ss");
            //var dt = DateTime.Parse(inicioSemana);
            var utc_from = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(_Global.FromDate), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");

            //string finSemana = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            //var to = DateTime.Parse(finSemana);
            var utc_to = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(_Global.ToDate), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");
            var facturacion = new Facturacion();

            Session["HistorialFromDate"] = _Global.FromDate;
            Session["HistorialToDate"] = _Global.ToDate;


            GetRetailCustomerXDRListResponse XDRListResponse = new GetRetailCustomerXDRListResponse();
            var Xdrs = new List<CustomerXDRInfo>();

            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                XDRListResponse = await ((customer_info)Session["CurrentCustomer"]).GetCustomerXDR(new GetRetailCustomerXDRListRequest { from_date = utc_from, to_date = utc_to });
                Xdrs = XDRListResponse.xdr_list.ToList();
            }
            else
            {
                var account = ((customer_info)Session["CurrentAccont"]);
                var customer = new customer_info
                {
                    i_customer = account.i_customer
                };

                XDRListResponse = await customer.GetCustomerXDR(new GetRetailCustomerXDRListRequest { from_date = utc_from, to_date = utc_to });
                Xdrs = XDRListResponse.xdr_list.ToList().Where(x => x.account_id == account.i_customer.ToString()).ToList();
            }

            var RecargaNautas = new List<RecargaNauta>();

            var listaCuentas = new List<customer_info>();


            foreach (var item in Xdrs)
            {
                if (item.XdrIsMovil())
                {
                    var nombreCuenta = "";
                    if (item.account_id != null)
                    {
                        if (listaCuentas.Where(x => x.i_customer.ToString() == item.XdrIdAsociado().ToString()).Any())
                        {
                            nombreCuenta = listaCuentas.Where(x => x.i_customer.ToString() == item.XdrIdAsociado().ToString()).First().firstname;
                        }
                        else
                        {
                            var accountInfo = await ((customer_info)Session["CurrentCustomer"]).GetAccountById(item.XdrIdAsociado().ToString());
                            if (accountInfo != null)
                            {
                                nombreCuenta = accountInfo.fullname;
                                listaCuentas.Add(accountInfo);
                            }
                        }
                    }

                    var recarga = new RecargaNauta();
                    recarga.Numero = item.XdrGetNumero();
                    recarga.Monto = item.XdrGetMonto();
                    recarga.CostoXdr = item.XdrGetCosto();
                    recarga.Fecha = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(item.bill_time, "UTC", "US Eastern Standard Time").ToString("yyyy-MM-dd HH:mm:ss");
                    recarga.Asociado = nombreCuenta;
                    RecargaNautas.Add(recarga);
                }
            }

            Session["ListaRecargasNautasHistorial"] = RecargaNautas;

            Session["Registros"] = ((List<RecargaNauta>)Session["ListaRecargasNautasHistorial"]).Count;
            var total = ((List<RecargaNauta>)Session["ListaRecargasNautasHistorial"]).Sum(x => x.CostoXdr);
            Session["Total"] = decimal.Round(Convert.ToDecimal(total), 2);

            return View((List<RecargaNauta>)Session["ListaRecargasNautasHistorial"]);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Historial(string datepickerInicio, string datepickerFin, string txtMonto, string txtNumero)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            if (datepickerInicio == "" && datepickerFin == "" && txtMonto == "" && txtNumero == "") return View((List<RecargaNauta>)Session["ListaRecargasNautasHistorial"]);
            var filtro = (List<RecargaNauta>)Session["ListaRecargasNautasHistorial"];
            if (Session["ListaRecargasNautasHistorial"] == null)
                return View(filtro);
            CultureInfo MyCultureInfo = new CultureInfo("en-US");
            if (datepickerInicio != "")
                filtro = filtro.Where(x => DateTime.Parse(x.Fecha.Substring(0, 10), MyCultureInfo) >= DateTime.Parse(datepickerInicio, MyCultureInfo)).ToList();
            if (datepickerFin != "")
                filtro = filtro.Where(x => DateTime.Parse(x.Fecha.Substring(0, 10), MyCultureInfo) <= DateTime.Parse(datepickerFin, MyCultureInfo)).ToList();
            if (txtMonto != "")
                filtro = filtro.Where(x => x.Monto == Convert.ToInt32(txtMonto)).ToList();
            if (txtNumero != "")
                filtro = filtro.Where(x => x.Numero.Contains(txtNumero)).ToList();

            Session["Registros"] = filtro.Count;
            Session["Total"] = decimal.Round(Convert.ToDecimal(filtro.Sum(x => x.CostoXdr)), 2);

            Session["datepickerInicio"] = datepickerInicio;
            Session["datepickerFin"] = datepickerFin;
            Session["txtMonto"] = txtMonto;
            Session["txtNumero"] = txtNumero;


            return View(filtro);
        }

        public ActionResult Filtrar()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            Session["ListaRecargasNautasHistorial"] = new List<RecargaNauta>();
            return RedirectToAction("Historial", "", (List<RecargaNauta>)Session["ListaRecargasNautasHistorial"]);
        }

        public ActionResult Index()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            var recargasConError = db.RecargaNautas.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Estado == EstadoRecarga.Error.ToString()).ToList();
            foreach (var item in recargasConError)
            {
                ((List<RecargaNauta>)Session["ListaRecargasNautas"]).Add(item);
            }
            return View();
        }

        public ActionResult Create()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            return View();
        }


        public async Task<ActionResult> Agregar(string Numero, int product_id, string remitente)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            if ((Numero == null || Numero == "") || (product_id == null || product_id == 0))
                return Json(new ErrorHandling { faul = true, faultstring = "Complete los datos de la recarga." }, JsonRequestBehavior.AllowGet);

            var ListaRecargasNautas = ((List<RecargaNauta>)Session["ListaRecargasNautas"]);
            var id = ListaRecargasNautas.Count + 1;
            if (ListaRecargasNautas.Where(x => x.id == id).Any())
                id++;
            var Monto = 0;
            if (product_id == 5) Monto = 5;
            if (product_id == 10) Monto = 10;
            if (product_id == 12) Monto = 12;
            if (product_id == 14) Monto = 14;
            if (product_id == 15) Monto = 15;
            if (product_id == 16) Monto = 16;
            if (product_id == 18) Monto = 18;
            if (product_id == 20) Monto = 20;
            if (product_id == 25) Monto = 25;
            if (product_id == 30) Monto = 30;
            if (product_id == 35) Monto = 35;
            if (product_id == 40) Monto = 40;
            if (product_id == 45) Monto = 45;         
            if (product_id == 50) Monto = 50;

            var recarga = new RecargaNauta
            {
                product_id = product_id,
                sender_sms_notification = true,
                id = id,
                IdCuenta = ID_CUENTA,
                CodigoPais = 53,
                Numero = Numero + "@nauta.com.cu",
                remitente = remitente,
                Monto = Monto,
                Impuesto = IMPUESTO,
                Fecha = DateTime.Now.To_MM_DD_YYYY()
            };

            if (ListaRecargasNautas.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Numero == Numero).Any())
            {
                return Json(new ErrorHandling { faul = true, faultstring = "El usuario ya existe,espere 5 minitos para recargar el mismo usuario." }, JsonRequestBehavior.AllowGet);
            }
            else
            if (ListaRecargasNautas.Count >= 10)
            {
                return Json(new ErrorHandling { faul = true, faultstring = "No se admiten mas de 10 recargas por al mismo tiempo." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                decimal fondo = 0;
                try
                {
                    fondo = Convert.ToDecimal((float)Session["FONDOS"]);
                }
                catch
                {
                    return Json(new ErrorHandling { faul = true, faultstring = "No tiene fondos disponibles para agregar esta recarga." }, JsonRequestBehavior.AllowGet);

                }

                if (ListaRecargasNautas.Sum(x => x.Costo) + recarga.Costo > fondo)
                {
                    return Json(new ErrorHandling { faul = true, faultstring = "No tiene fondos disponibles para agregar esta recarga." }, JsonRequestBehavior.AllowGet);

                }
            }
            ListaRecargasNautas.Add(recarga);
            Session["ListaRecargasNautas"] = ListaRecargasNautas;
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
        public async Task<ActionResult> Create(RecargaNauta RecargaNauta)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            if (!await ACTUALIZAR_DATOS())
            {
                ShowDanger("No se pudo conectar con el servidor, verifique su conexion a internet o contacte a support.");
                return View();
            }

            var result = db.RecargaNautas.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Estado == EstadoRecarga.Pendiente.ToString() && x.Numero == RecargaNauta.Numero).ToList();

            if (result.Any())
            {
                ShowDanger("El usuario ya existe,espere 5 minitos para recargar el mismo usuario");
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
                RecargaNauta.IdCuenta = ID_CUENTA.ToString();
                RecargaNauta.TipoRecarga = "N";
                RecargaNauta.Impuesto = IMPUESTO;
                RecargaNauta.Fecha = DateTime.Now.ToString();
                RecargaNauta.Estado = EstadoRecarga.Pendiente.ToString();

                decimal sumaCosto = 0;
                var ListaRecargasNautas = db.RecargaNautas.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Estado == EstadoRecarga.Pendiente.ToString()).ToList();
                if (ListaRecargasNautas.Any())
                    sumaCosto = ListaRecargasNautas.Select(x => x.Costo).Sum();

                var fondos = await FONDOS();
                if (fondos < (float)(RecargaNauta.Costo + sumaCosto))
                {
                    ShowDanger("No tiene fondos disponibles para agregar esta recarga.");
                    return RedirectToAction("Create");
                }

                db.RecargaNautas.Add(RecargaNauta);
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

            return View(RecargaNauta);

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

                RecargaNauta RecargaNauta = db.RecargaNautas.Find(id);

                if (RecargaNauta == null)
                {
                    return HttpNotFound();
                }
                return View(RecargaNauta);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecargaNauta RecargaNauta)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {

                if (ModelState.IsValid)
                {
                    RecargaNauta.IdCuenta = ID_CUENTA.ToString();
                    RecargaNauta.TipoRecarga = "M";
                    RecargaNauta.Impuesto = IMPUESTO;
                    RecargaNauta.Fecha = DateTime.Now.ToString();
                    RecargaNauta.Estado = EstadoRecarga.Pendiente.ToString();

                    db.Entry(RecargaNauta).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        ShowSuccess("Acción Completada con éxito");
                    }
                    catch (Exception ex)
                    {

                        ShowDanger("No se pudo completar la acción.");
                        return View(RecargaNauta);
                    }

                    return RedirectToAction("Create");
                }
                return View(RecargaNauta);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {
                RecargaNauta RecargaNauta = db.RecargaNautas.Find(id);
                try
                {
                    var item = ((List<RecargaNauta>)Session["ListaRecargasNautas"]).First(x => x.id == id);
                    ((List<RecargaNauta>)Session["ListaRecargasNautas"]).Remove(item);

                    return Json("delete", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("error", JsonRequestBehavior.DenyGet);
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

        public ActionResult GetListaRecargasNautas()
        {
            if (Session["ListaRecargasNautas"] == null)
                return Json("ListaVacia", JsonRequestBehavior.AllowGet);

            if (!((List<RecargaNauta>)Session["ListaRecargasNautas"]).Any())
                return Json("ListaVacia", JsonRequestBehavior.AllowGet);
            else
                return Json((List<RecargaNauta>)Session["ListaRecargasNautas"], JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ActualizarFondos()
        {
            var fondoActual = (float)Session["FONDOS"];
            var newFondo = await FONDOS();

            var sms = "Sus fondos han sido actualizados.";

            var diferencia = newFondo - fondoActual;

            if (fondoActual != newFondo && diferencia > 0)
            {
                sms = "Usted ha recibido una transferencia de $" + diferencia.To_0_00() + " USD";
            }
            else
            if (diferencia < 0)
            {
                sms = "A su fondos se le a descontado $" + (diferencia * -1).To_0_00() + " USD";
            }
            return Json(sms, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> RecargarLista(string remitente, string accion)
        {

            if (accion == "recargar")
            {
                if (!IsLogin()) return RedirectToAction("Index", "Login");

                var ListaRecargasNautas = ((List<RecargaNauta>)Session["ListaRecargasNautas"]);

                if (!ListaRecargasNautas.Any())
                    return Json("ListaVacia", JsonRequestBehavior.AllowGet);

                var listaErrores = new List<RecargaNauta>();

                //Para guardar lista de respuestas de sms
                var smsResponse = new List<innoverit>();

                if (ListaRecargasNautas.Any())
                {
                    try
                    {
                        foreach (var item in ListaRecargasNautas)
                        {

                            Ding.SendTransferResponse result = new Ding.SendTransferResponse();

                            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
                            {
                                if (_Global.ModoPrueba)
                                    result = await item.DingSimular((customer_info)Session["CurrentCustomer"]);
                                else
                                    result = await item.DingRecargar((customer_info)Session["CurrentCustomer"]);
                            }
                            else
                            {
                                if (_Global.ModoPrueba)
                                    result = await item.DingSimular((customer_info)Session["CurrentAccont"]);
                                else
                                    result = await item.DingRecargar((customer_info)Session["CurrentAccont"]);
                            }

                            if (result != null)
                            {
                               
                                if (result.ResultCode == "1")
                                {
                                    if (await FONDOS() < (float)item.Costo || await FONDOS_TIENDAPADRE() < (float)item.Costo)
                                    {
                                        item.Estado = EstadoRecarga.Error.ToString();
                                        item.Error = "Contacte a support";
                                        item.ColorError = "Orange";
                                        listaErrores.Add(item);
                                    }
                                    else
                                    {

                                        if (_Global.ModoPrueba)
                                        {
                                            item.Estado = EstadoRecarga.Simulada.ToString();
                                            item.Error = "";
                                            item.ColorError = "";
                                        }
                                        else
                                        {
                                            item.Estado = EstadoRecarga.Recargada.ToString();
                                            item.Error = "";
                                            item.ColorError = "";
                                        }

                                        var padre = await ((customer_info)Session["CurrentCustomer"]).GetParent();

                                        // CALCULAR DIFERENCIA DE SALDO

                                        var Total = (float)_Global.ReglaDeTres(IMPUESTO, 0, (float)item.Costo);
                                        var DineroReal = (float)_Global.ReglaDeTres((float)padre.GetComisionTopUp(), Total, 0);

                                        await padre.MakeTransaction_Manualcharge(DineroReal, "Movil-" + item.Monto + "-" + item.CodigoPais + item.Numero + "-" + ID_CUENTA);

                                        await ACTUALIZAR_BALANCE(item);
                                        await FONDOS();

                                        //Mandar sms
                                        if (remitente != "" && remitente != null)
                                        {
                                            var sms = new innoverit_sms
                                            {
                                                NumeroTelefono = remitente,
                                                SMS = "Usted envio una recarga al la cuenta " + item.Numero + " con un monto de $" + item.Monto.To_0_00() + ",transaccion " + result.TransferRecord.TransferId.TransferRef + ".Gracias por su compra!!3054477549 teleyuma.com."

                                            };
                                            var res = new innoverit();
                                            res = await sms.Enviar();
                                            smsResponse.Add(res);
                                        }

                                    }
                                }
                                else
                                {
                                    if(result.ResultCode == "5")
                                    {
                                        item.Error = "Contacte a support";
                                        item.ColorError = Colores.errorRecarga;
                                        item.Estado = EstadoRecarga.Error.ToString();
                                        listaErrores.Add(item);
                                    }
                                    else
                                    {
                                        item.Error = result.ErrorCodes[0].Code;
                                        item.ColorError = Colores.errorRecarga;
                                        item.Estado = EstadoRecarga.Error.ToString();
                                        listaErrores.Add(item);
                                    }
                                   
                                }

                            }
                            else
                            {
                                return Json("Error", JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ;
                    }

                }
                else
                {
                    return Json("ListaVacia", JsonRequestBehavior.AllowGet);
                }
                await FONDOS();
                Session["ListaRecargasNautas"] = listaErrores;

                return Json(listaErrores, JsonRequestBehavior.AllowGet);
            }


            return Json("Error", JsonRequestBehavior.AllowGet);

        }

    }
}
