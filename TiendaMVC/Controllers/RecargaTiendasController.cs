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
    public class RecargaTiendasController : BaseController
    {
        private TiendaMVCContext db = new TiendaMVCContext();

        public async Task<ActionResult> Historial()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            var utc_from = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(_Global.FromDate), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");
            //var utc_from = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse("24/02/2020"), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");
            
            var utc_to = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(_Global.ToDate), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");
            //var utc_to = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse("28/02/2020"), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");
            var facturacion = new Facturacion();

            Session["HistorialFromDate"] = _Global.FromDate;
            Session["HistorialToDate"] = _Global.ToDate;

            GetRetailCustomerXDRListResponse XDRListResponse = new GetRetailCustomerXDRListResponse();
            var Xdrs = new List<CustomerXDRInfo>();

            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                try
                {
                    XDRListResponse = await ((customer_info)Session["CurrentCustomer"]).GetCustomerXDR(new GetRetailCustomerXDRListRequest { from_date = utc_from, to_date = utc_to });
                    Xdrs = XDRListResponse.xdr_list.ToList();
                }
                catch (Exception ex)
                {
                   ;
                }
            }
            else
            {
                var account = ((customer_info)Session["CurrentAccont"]);               

                XDRListResponse = await account.GetCustomerXDR(new GetRetailCustomerXDRListRequest { from_date = utc_from, to_date = utc_to });
                Xdrs = XDRListResponse.xdr_list.ToList();
            }

            var RecargaTiendas = new List<RecargaTienda>();

            var listaCuentas = new List<customer_info>();


            foreach (var item in Xdrs)
            {
                if (item.XdrIsMovil() || item.XdrIsNauta())
                {
                    var nombreCuenta = "";
                    if (item.XdrIdAsociado() != 0)
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


                    var recarga = new RecargaTienda();
                    recarga.Numero = item.XdrGetNumero();
                    recarga.Nombre = item.XdrGetNombre();
                    recarga.Monto = item.XdrGetMonto();
                    recarga.CostoXdr = item.XdrGetCosto();
                    recarga.Fecha = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(item.bill_time, "UTC", "US Eastern Standard Time").ToString("yyyy-MM-dd HH:mm:ss");
                    recarga.Asociado = nombreCuenta;
                    RecargaTiendas.Add(recarga);
                }
            }

            Session["ListaRecargasHistorial"] = RecargaTiendas;

            Session["Registros"] = ((List<RecargaTienda>)Session["ListaRecargasHistorial"]).Count;
            var total = ((List<RecargaTienda>)Session["ListaRecargasHistorial"]).Sum(x => x.CostoXdr);
            Session["Total"] = decimal.Round(Convert.ToDecimal(total), 2);

            return View((List<RecargaTienda>)Session["ListaRecargasHistorial"]);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Historial(string datepickerInicio, string datepickerFin, string txtMonto, string txtNumero)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            CultureInfo MyCultureInfo = new CultureInfo("en-US");
           
            var dataInicio = DateTime.Parse(datepickerInicio, MyCultureInfo).ToShortDateString();
            var dataFin = DateTime.Parse(datepickerFin, MyCultureInfo).ToShortDateString();

            var valueInicio = DateTime.Parse(dataInicio).AddHours(00).AddMinutes(00).AddSeconds(00).ToString("yyyy-MM-dd HH:mm:ss"); ;
            var utc_from = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(valueInicio), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");

         
            var valueFin = DateTime.Parse(dataFin).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss"); ;
            var utc_to = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(valueFin), "US Eastern Standard Time", "UTC").ToString("yyyy-MM-dd HH:mm:ss");
          
            var facturacion = new Facturacion();

            Session["HistorialFromDate"] = utc_from;
            Session["HistorialToDate"] = utc_to;

            GetRetailCustomerXDRListResponse XDRListResponse = new GetRetailCustomerXDRListResponse();
            var Xdrs = new List<CustomerXDRInfo>();

            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                try
                {
                    XDRListResponse = await ((customer_info)Session["CurrentCustomer"]).GetCustomerXDR(new GetRetailCustomerXDRListRequest { from_date = utc_from, to_date = utc_to });
                    Xdrs = XDRListResponse.xdr_list.ToList();
                }
                catch (Exception ex)
                {
                    ;
                }
            }
            else
            {
                var account = ((customer_info)Session["CurrentAccont"]);

                XDRListResponse = await account.GetCustomerXDR(new GetRetailCustomerXDRListRequest { from_date = utc_from, to_date = utc_to });
                Xdrs = XDRListResponse.xdr_list.ToList();
            }

            var RecargaTiendas = new List<RecargaTienda>();

            var listaCuentas = new List<customer_info>();


            foreach (var item in Xdrs)
            {
                if (item.XdrIsMovil() || item.XdrIsNauta())
                {
                    var nombreCuenta = "";
                    if (item.XdrIdAsociado() != 0)
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


                    var recarga = new RecargaTienda();
                    recarga.Numero = item.XdrGetNumero();
                    recarga.Nombre = item.XdrGetNombre();
                    recarga.Monto = item.XdrGetMonto();
                    recarga.CostoXdr = item.XdrGetCosto();
                    recarga.Fecha = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(item.bill_time, "UTC", "US Eastern Standard Time").ToString("yyyy-MM-dd HH:mm:ss");
                    recarga.Asociado = nombreCuenta;
                    RecargaTiendas.Add(recarga);
                }
            }

            Session["ListaRecargasHistorial"] = RecargaTiendas;

            Session["Registros"] = ((List<RecargaTienda>)Session["ListaRecargasHistorial"]).Count;
            var total = ((List<RecargaTienda>)Session["ListaRecargasHistorial"]).Sum(x => x.CostoXdr);
            Session["Total"] = decimal.Round(Convert.ToDecimal(total), 2);

            return View((List<RecargaTienda>)Session["ListaRecargasHistorial"]);

        }
             

        //public ActionResult Historial(string datepickerInicio, string datepickerFin, string txtMonto, string txtNumero)
        //{
        //    if (!IsLogin()) return RedirectToAction("Index", "Login");

        //    if (datepickerInicio == "" && datepickerFin == "" && txtMonto == "" && txtNumero == "") return View((List<RecargaTienda>)Session["ListaRecargasHistorial"]);
        //    var filtro = (List<RecargaTienda>)Session["ListaRecargasHistorial"];
        //    if (Session["ListaRecargasHistorial"] == null)
        //        return View(filtro);
        //    CultureInfo MyCultureInfo = new CultureInfo("en-US");
        //    if (datepickerInicio != "")
        //        filtro = filtro.Where(x => DateTime.Parse(x.Fecha.Substring(0, 10), MyCultureInfo) >= DateTime.Parse(datepickerInicio, MyCultureInfo)).ToList();
        //    if (datepickerFin != "")
        //        filtro = filtro.Where(x => DateTime.Parse(x.Fecha.Substring(0, 10), MyCultureInfo) <= DateTime.Parse(datepickerFin, MyCultureInfo)).ToList();
        //    if (txtMonto != "")
        //        filtro = filtro.Where(x => x.Monto == Convert.ToInt32(txtMonto)).ToList();
        //    if (txtNumero != "")
        //        filtro = filtro.Where(x => x.Numero.Contains(txtNumero)).ToList();

        //    Session["Registros"] = filtro.Count;
        //    Session["Total"] = decimal.Round(Convert.ToDecimal(filtro.Sum(x => x.CostoXdr)), 2);

        //    Session["datepickerInicio"] = datepickerInicio;
        //    Session["datepickerFin"] = datepickerFin;
        //    Session["txtMonto"] = txtMonto;
        //    Session["txtNumero"] = txtNumero;


        //    return View(filtro);
        //}

        public ActionResult Filtrar()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            Session["ListaRecargasHistorial"] = new List<RecargaTienda>();
            return RedirectToAction("Historial", "", (List<RecargaTienda>)Session["ListaRecargasHistorial"]);
        }

        public ActionResult Index()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            Session["HistorialFromDate"] = _Global.FromDate;
            Session["HistorialToDate"] = _Global.ToDate;

            Session["ListaRecargasMoviles"] = new List<RecargaTienda>();
            var recargasConError = db.RecargaTienda.Where(x => x.IdCuenta == ID_CUENTA.ToString() && x.Estado == EstadoRecarga.Error.ToString()).ToList();
            foreach (var item in recargasConError)
            {
                ((List<RecargaTienda>)Session["ListaRecargasMoviles"]).Add(item);
            }

            Historial();

            return View();
        }

        public ActionResult Create()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            return View();
        }
        
        public async Task<ActionResult> Agregar(int CodigoPais, string Numero, string Nombre, int Monto, string remitente)
        {

            if (!IsLogin()) return RedirectToAction("Index", "Login");

            if ((CodigoPais == null || CodigoPais == 0) || (Numero == null || Numero == "") || (Monto == null || Monto == 0))
                return Json(new ErrorHandling { faul = true, faultstring = "Complete los datos de la recarga." }, JsonRequestBehavior.AllowGet);

            //Validaciones para cuba
            if (CodigoPais == 53)
            {
                var array = Numero.ToCharArray();
                if (array[0].ToString() != "5" || Numero.Length > 8 || Numero.Length < 8)
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
                Nombre = Nombre,
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
                decimal fondo = 0;
                try
                {
                    fondo = Convert.ToDecimal((float)Session["FONDOS"]);
                }
                catch
                {
                    return Json(new ErrorHandling { faul = true, faultstring = "No tiene fondos disponibles para agregar esta recarga." }, JsonRequestBehavior.AllowGet);

                }

                if (ListaRecargasMoviles.Sum(x => x.Costo) + recarga.Costo > fondo)
                {
                    return Json(new ErrorHandling { faul = true, faultstring = "No tiene fondos disponibles para agregar esta recarga." }, JsonRequestBehavior.AllowGet);

                }
            }
            ListaRecargasMoviles.Add(recarga);
            Session["ListaRecargasMoviles"] = ListaRecargasMoviles;
            return Json(new{ duplicado = true,count = 1, recarga = recarga }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ValidarVecesSemanal(int CodigoPais, string Numero, string Nombre, int Monto, string remitente)
        {

            if (!IsLogin()) return RedirectToAction("Index", "Login");

            if ((CodigoPais == null || CodigoPais == 0) || (Numero == null || Numero == "") || (Monto == null || Monto == 0))
                return Json(new ErrorHandling { faul = true, faultstring = "Complete los datos de la recarga." }, JsonRequestBehavior.AllowGet);

            var lista = ((List<RecargaTienda>)Session["ListaRecargasHistorial"]);
            int count = lista.Where(x => x.Numero == CodigoPais.ToString() + Numero).Count();
            bool duplicado = false;
            string vcs = " vez";
            if(count == 1)
            {
                duplicado = true;
            }
            else
               if (count > 1)
            {
                duplicado = true;
                vcs = " veces";
            }

            string msg = "Este número ha sido recargado " + count + " " + vcs+" esta semana, usted desea continuar?";

            return Json(new { duplicado = duplicado, msg = msg}, JsonRequestBehavior.AllowGet);
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
                    var item = ((List<RecargaTienda>)Session["ListaRecargasMoviles"]).First(x => x.id == id);
                    ((List<RecargaTienda>)Session["ListaRecargasMoviles"]).Remove(item);

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

        public ActionResult GetListaRecargas()
        {
            if (Session["ListaRecargasMoviles"] == null)
                return Json("ListaVacia", JsonRequestBehavior.AllowGet);

            if (!((List<RecargaTienda>)Session["ListaRecargasMoviles"]).Any())
                return Json("ListaVacia", JsonRequestBehavior.AllowGet);
            else
                return Json((List<RecargaTienda>)Session["ListaRecargasMoviles"], JsonRequestBehavior.AllowGet);
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

            if (accion == "reservar")
            {

                var ListaRecargas = ((List<RecargaTienda>)Session["ListaRecargasMoviles"]);

                if (!ListaRecargas.Any())
                    return Json("ListaVacia", JsonRequestBehavior.AllowGet);

                var listaErrores = new List<RecargaTienda>();

                //Para guardar lista de respuestas de sms
                var smsResponse = new List<innoverit>();

                if (ListaRecargas.Any())
                {
                    try
                    {
                        foreach (var item in ListaRecargas)
                        {

                            Ding.SendTransferResponse result = new Ding.SendTransferResponse();

                            result = await item.DingSimular(PHONE1);

                            if (result != null)
                            {
                                if (result.ResultCode == "1")
                                {
                                    item.Estado = EstadoRecarga.Reservada.ToString();
                                    item.Error = "";
                                    item.ColorError = "";

                                    db.RecargaTienda.Add(item);
                                    try
                                    {
                                        db.SaveChanges();
                                    }
                                    catch
                                    {
                                        item.Error = "No se pudo añadir a la lista de recargas reservas";
                                        item.ColorError = Colores.errorRecarga;
                                        item.Estado = EstadoRecarga.Error.ToString();
                                        listaErrores.Add(item);
                                    }
                                }
                                else
                                {
                                    if (result.ResultCode == "5")
                                    {

                                        //item.Error = "Contacte a support";
                                        //item.ColorError = Colores.errorRecarga;
                                        //item.Estado = EstadoRecarga.Error.ToString();
                                        //listaErrores.Add(item);
                                        item.Estado = EstadoRecarga.Reservada.ToString();
                                        item.Error = "";
                                        item.ColorError = "";

                                        db.RecargaTienda.Add(item);
                                        try
                                        {
                                            db.SaveChanges();
                                        }
                                        catch
                                        {
                                            item.Error = "No se pudo añadir a la lista de recargas reservas";
                                            item.ColorError = Colores.errorRecarga;
                                            item.Estado = EstadoRecarga.Error.ToString();
                                            listaErrores.Add(item);
                                        }

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
                Session["ListaRecargasMoviles"] = listaErrores;

                return Json(listaErrores, JsonRequestBehavior.AllowGet);
            }


            if (accion == "recargar")
            {
                if (!IsLogin()) return RedirectToAction("Index", "Login");

                var ListaRecargas = ((List<RecargaTienda>)Session["ListaRecargasMoviles"]);

                if (!ListaRecargas.Any())
                    return Json("ListaVacia", JsonRequestBehavior.AllowGet);

                var listaErrores = new List<RecargaTienda>();

                //Para guardar lista de respuestas de sms
                var smsResponse = new List<innoverit>();

                if (ListaRecargas.Any())
                {
                    try
                    {
                        foreach (var item in ListaRecargas)
                        {
                            await Task.Delay(5000);

                            Ding.SendTransferResponse result = new Ding.SendTransferResponse();

                            if (_Global.ModoPrueba)
                                result = await item.DingSimular(PHONE1);
                            else
                                result = await item.DingRecargar(PHONE1);
                                                                                   

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

                                        await padre.MakeTransaction_Manualcharge(DineroReal, "M-" + item.Monto + "-" + item.CodigoPais + item.Numero + "-" + ID_CUENTA+"-"+item.Nombre);

                                        await ACTUALIZAR_BALANCE(item);
                                        await FONDOS();
                                    }

                                    //Mandar sms
                                    if (remitente != "" && remitente != null)
                                    {
                                        var sms = new innoverit_sms
                                        {
                                            NumeroTelefono = remitente,
                                            SMS = "Usted envio una recarga al numero " + item.Numero + " con un monto de $" + item.Monto.To_0_00() + ",transaccion " + result.TransferRecord.TransferId.TransferRef + ".Gracias por su compra!!3054477549 teleyuma.com."
                                            
                                        };
                                        var res = new innoverit();
                                        res = await sms.Enviar();
                                        smsResponse.Add(res);
                                    }
                                }
                                else
                                {
                                    if (result.ResultCode == "5")
                                    {
                                        item.Error = "Contacte a support";
                                        item.ColorError = "Orange";
                                        item.Estado = EstadoRecarga.Error.ToString();
                                        listaErrores.Add(item);
                                    }
                                    else
                                    {
                                        item.Error = result.ErrorCodes[0].Code;
                                        item.ColorError = "Orange";
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
                Session["ListaRecargasMoviles"] = listaErrores;

               await Historial();

                if (listaErrores.Any())
                return Json(listaErrores, JsonRequestBehavior.AllowGet);
                else return Json("ok", JsonRequestBehavior.AllowGet);
            }

            await Historial();
            return Json("Error", JsonRequestBehavior.AllowGet);

        }

    }
}
