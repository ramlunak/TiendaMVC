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
    public class account_infoController : BaseController
    {

        private TiendaMVCContext db = new TiendaMVCContext();
        
        public async Task<ActionResult> Actualizar(float? monto_poner, float? monto_quitar, string account_id,string id_cuenta_transrefir, string accion)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {
                try
                {
                    if (accion == "+")
                    {
                        if (monto_poner == 0 || monto_poner == null)
                        {
                            return Json("El monto no puede ser 0.", JsonRequestBehavior.AllowGet);
                        }
                        //else if (monto_poner > await FONDOS())
                        //{
                        //    return Json("No tiene fondos disponibles para asignar esa cantidad.", JsonRequestBehavior.AllowGet);
                        //}
                        else
                        {
                            var customer = (customer_info)Session["CurrentCustomer"];
                            var cuenta = await customer.GetAccountById(account_id);
                            if (cuenta != null && cuenta.i_customer.ToString() == account_id)
                            {
                                ////CALCULAR DIFERENCIA DE SALDO

                                //var porcionDistrubuidor = monto_poner;
                                //var comisionDistribuidor = IMPUESTO;

                                //var TotalSegunComision = (float)_Global.ReglaDeTres(comisionDistribuidor, 0, (float)porcionDistrubuidor);

                                //var DineroFicticio = (float)_Global.ReglaDeTres((float)cuenta.GetComisionTopUp(), TotalSegunComision, 0);
                                
                                ////----------------------------

                                cuenta.credit_limit += (float)monto_poner;
                                cuenta.i_distributor =  await ID_DISTRIBUIDOR();
                                var update = await cuenta.Actualizar();
                                if (update)
                                {
                                    //// actualizar indormacion customer
                                    //((customer_info)Session["CurrentCustomer"]).credit_limit -= (float)monto_poner;
                                    //await ((customer_info)Session["CurrentCustomer"]).Actualizar();
                                    //Session["CurrentCustomer"] = await ((customer_info)Session["CurrentCustomer"]).Reload();
                                    //var fondo =  await FONDOS();                                   
                                    
                                    return Json(customer.fondosDisponibles, JsonRequestBehavior.AllowGet);
                                }
                                else
                                    return Json("No se pudo conectar con el servidor!", JsonRequestBehavior.DenyGet);

                            }
                            else
                                return Json("No se pudo conectar con el servidor!", JsonRequestBehavior.DenyGet);
                        }

                    }
                    else
                    {
                        //Quitar Monto
                        if (monto_quitar == 0 || monto_quitar == null)
                        {
                            return Json("El monto no puede ser 0.", JsonRequestBehavior.AllowGet);
                        }                       
                        else
                        {
                            var customer = (customer_info)Session["CurrentCustomer"];
                            var cuenta_quitar = await customer.GetAccountById(account_id);
                            //var cuenta_poner = await customer.GetAccountById(id_cuenta_transrefir);
                                                       

                            //----------------------------

                            //if (id_cuenta_transrefir == "0")
                            //{
                                //CALCULAR DIFERENCIA DE SALDO
                                                              
                                //var Total = (float)_Global.ReglaDeTres(IMPUESTO, 0, (float)monto_quitar);
                                //var DineroFicticio = (float)_Global.ReglaDeTres((float)cuenta_quitar.GetComisionTopUp(), Total, 0);

                                //Transferir saldo al customer                

                                if (cuenta_quitar != null && cuenta_quitar.i_customer.ToString() == account_id)
                                {
                                    if (monto_quitar > cuenta_quitar.fondosDisponibles)
                                    {
                                        return Json("La cuenta no tiene fondos disponibles para descontar esa cantidad.", JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {                                        
                                        cuenta_quitar.credit_limit -= (float)monto_quitar;
                                        cuenta_quitar.i_distributor = await ID_DISTRIBUIDOR(); 
                                    var update = await cuenta_quitar.Actualizar();
                                        if (update)
                                        {
                                            //// actualizar indormacion customer
                                            //((customer_info)Session["CurrentCustomer"]).credit_limit += (float)monto_quitar;
                                            //await ((customer_info)Session["CurrentCustomer"]).Actualizar();
                                            //Session["CurrentCustomer"] = await ((customer_info)Session["CurrentCustomer"]).Reload();
                                            //var fondo = await FONDOS();

                                            return Json(customer.fondosDisponibles, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                            return Json("No se pudo conectar con el servidor!", JsonRequestBehavior.DenyGet);
                                    }
                                }
                                else
                                    return Json("No se pudo conectar con el servidor!", JsonRequestBehavior.DenyGet);
                            //}
                            //else
                            //{
                            //    //Transferir saldo atra cuenta                               
                            //    if (cuenta_quitar != null && cuenta_quitar.i_customer.ToString() == account_id)
                            //    {
                            //        if (monto_quitar > cuenta_quitar.fondosDisponibles)
                            //        {
                            //            return Json("La cuenta no tiene fondos disponibles para descontar esa cantidad.", JsonRequestBehavior.AllowGet);
                            //        }
                            //        else
                            //        {

                            //            //CALCULAR DIFERENCIA DE SALDO

                            //            //CALCULAR DIFERENCIA DE SALDO

                            //            var DineroFicticioQuitar = monto_quitar;     
                            //            var Total = (float)_Global.ReglaDeTres((float)cuenta_quitar.GetComisionTopUp(), 0, (float)DineroFicticioQuitar);

                            //            var comisionDistribuidor = cuenta_poner.GetComisionTopUp();

                            //            var DineroFicticioPoner = (float)_Global.ReglaDeTres((float)cuenta_poner.GetComisionTopUp(), Total, 0);

                            //            cuenta_quitar.credit_limit -= (float)monto_quitar;
                            //            cuenta_quitar.i_distributor = await ID_DISTRIBUIDOR(); 
                            //            var update_quitar = await cuenta_quitar.Actualizar();

                            //            cuenta_poner.credit_limit += (float)DineroFicticioPoner;
                            //            cuenta_poner.i_distributor = await ID_DISTRIBUIDOR(); 
                            //            var update_poner = await cuenta_poner.Actualizar();

                            //            if (update_quitar && update_poner)
                            //            {
                            //                // actualizar indormacion customer ,aqui no se actualiza el credito del customer                                         
                            //                var fondo = await FONDOS();

                            //                return Json(fondo, JsonRequestBehavior.AllowGet);
                            //            }
                            //            else
                            //                return Json("No se pudo conectar con el servidor!", JsonRequestBehavior.DenyGet);
                            //        }
                            //    }
                            //    else
                            //        return Json("No se pudo conectar con el servidor!", JsonRequestBehavior.DenyGet);
                            //}

                        }

                    }


                }
                catch (Exception ex)

                {
                    return Json("error", JsonRequestBehavior.DenyGet);
                }
            }

            //if (monto > account.balance)
            //{
            //    account.ErrorHandlingCrearCuenta.faul = true;
            //    account.ErrorHandlingCrearCuenta.faultstring = "El monto a transferir no puede ser que el fondo.";

            //}
            //    // actualizar indormacion
            //    await ((customer_info)Session["CurrentCustomer"]).Actualizar();
            //    Session["CurrentCustomer"] = await ((customer_info)Session["CurrentCustomer"]).Reload();


            //var result = await customer_info.MakeTransaction_Manualcharge(monto, "Trnasferencia Saldo");

            //if (result == null)
            //{
            //    customer_info.ErrorHandlingCrearCuenta.faul = true;
            //    customer_info.ErrorHandlingCrearCuenta.faultstring = "No se pudo contectar con el servidor, verifique su conexion a internet o contacte con support.";
            //    return View(customer_info);
            //}

            //if (result.i_xdr == null)
            //{
            //    customer_info.ErrorHandlingCrearCuenta.faul = true;
            //    customer_info.ErrorHandlingCrearCuenta.faultstring = "No se pudo contectar con el servidor, verifique su conexion a internet o contacte con support.";
            //    return View(customer_info);
            //}
            //else
            //{
            //    // actualizar indormacion
            //    await ((customer_info)Session["CurrentCustomer"]).Actualizar();
            //    Session["CurrentCustomer"] = await ((customer_info)Session["CurrentCustomer"]).Reload();

            //}



        }

        // GET: customer_info
        public async Task<ActionResult> Index()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {
                var CurrentCustomer = (customer_info)Session["CurrentCustomer"];
                var hijos = await CurrentCustomer.GetAccountList(CurrentCustomer);
                var hijos_activos = hijos.Where(x => x.login != null).ToList();
                Session["HIJOS_ACTIVOS"] = hijos_activos;
                return View(hijos_activos);
            }
        }
        
        public ActionResult Create()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
                return View(new customer_info());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(customer_info account)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {
                if (ModelState.IsValid)
                {
                    //validar impuesto..................................

                    if (account.impuesto < IMPUESTO)
                    {
                        account.ErrorHandlingCrearCuenta.faul = true;
                        account.ErrorHandlingCrearCuenta.faultstring = "El impuesto de la tienda tiene que ser mayor o igual que su impuesto(" + IMPUESTO + "%)";
                        return View(account);
                    }

                    //validar fondos disponibles

                    //var fondos = await FONDOS();
                    //if (account.LimiteCredito > fondos)
                    //{
                    //    account.ErrorHandlingCrearCuenta.faul = true;
                    //    account.ErrorHandlingCrearCuenta.faultstring = "El credito que desea asignar es mayor que su fondo.";
                    //    return View(account);
                    //}

                    account.note = ":DISTRIB: 2:2:" + account.impuesto + ":";

                    ////CALCULAR DIFERENCIA DE SALDO

                    //var porcionDistrubuidor = account.LimiteCredito;
                    //var comisionDistribuidor = IMPUESTO;

                    //var TotalSegunComision = (float)_Global.ReglaDeTres(comisionDistribuidor,0,porcionDistrubuidor);

                    //var DineroFicticio = (float)_Global.ReglaDeTres(account.impuesto, TotalSegunComision, 0);
                    //account.credit_limit = DineroFicticio;

                    // //----------------------------

                    account.credit_limit = account.LimiteCredito;
                    var resul = await SetAccount(account);
                    if (!resul.faul)
                    {
                        //((customer_info)Session["CurrentCustomer"]).credit_limit -= account.LimiteCredito;
                        //await ((customer_info)Session["CurrentCustomer"]).Actualizar();
                        //Session["CurrentCustomer"] = await ((customer_info)Session["CurrentCustomer"]).Reload();
                        ShowSuccess(resul.faultstring);
                        await FONDOS();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        account.ErrorHandlingCrearCuenta = resul;
                        await FONDOS();

                        return View(account);
                    }
                    
                }
                else
                {
                    return View(account);
                }
            }
        }

        public async Task<ErrorHandling> SetAccount(customer_info account)
        {
           
            try
            {
                var now = DateTime.Now;

                var YY = now;
                var MM = now.Month.ToString();
                var DD = now.Day.ToString();

                if (MM.Length == 1)
                    MM = "0" + MM;
                if (DD.Length == 1)
                    DD = "0" + DD;

                var activationDate = now.Year + "-" + MM + "-" + DD;
                account.ComisionDelPadre = IMPUESTO;
                //id de la cuenta       
                account.opening_balance = 0;
                account.iso_4217 = "USD";
                account.i_distributor =  await ID_DISTRIBUIDOR();
                account.i_customer_type = 1;  //Online customers 
                account.batch_name = ID_CUENTA + "-di-pinless";
                account.billing_model = 1;
                account.i_time_zone = 109;
                account.i_ui_time_zone = 109;
                account.control_number = 1;
                account.h323_password = account.password;
                account.i_product = 22791;
                account.activation_date = activationDate.Trim();
                account.phone1 = account.phone1;
                account.firstname = account.firstname;
                account.name =account.fullname;
                account.lastname = account.lastname;
                account.email = account.email;
                account.login = account.phone1;
                account.password = account.password;
                account.note = account.note;
               
            }
            catch (Exception ex)
            {
                ;
            }

            var valid = await account.Validar();
            if (!valid.faul)
            {

                var result = await account.Crear();
                return result;
            }
            else
                return valid;

        }
        
        // GET: customer_info/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var CurrentCustomer = (customer_info)Session["CurrentCustomer"];
                customer_info customer_info = await CurrentCustomer.GetAccountById(id);
                if (customer_info == null)
                {
                    return HttpNotFound();
                }

                customer_info.ComisionDelPadre = IMPUESTO;
                return View(customer_info);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
