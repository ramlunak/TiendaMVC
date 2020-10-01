using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TiendaMVC.Class;
using TiendaMVC.Models;

namespace TiendaMVC.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            Session["CurrentCustomer"] = null;
            Session["CurrentAccont"] = null;
            Session["ListaRecargasMoviles"] = new List<RecargaTienda>();
            Session["ListaRecargasNautas"] = new List<RecargaNauta>();
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public async Task<ActionResult> Index(Login login)
        {

            Session["CurrentCustomer"] = null;
            Session["CurrentAccont"] = null;
            Session["TipoTienda"] = TipoTienda.Padre;


            Session["CurrentCustomer"] = await new customer_info().GetCustomerByUser(login.user);
            var CurrentCustomer = (customer_info)Session["CurrentCustomer"];


            if (CurrentCustomer != null && CurrentCustomer.i_customer != 0)
            {
                if (CurrentCustomer.password != login.pass)
                {

                    login.error = true;
                    login.errorMensaje = "Contraseña incorrecta";
                    return Json(login);

                }
                else
                if (CurrentCustomer.blocked == "Y")
                {
                    login.error = true;
                    login.errorMensaje = "Usuario bloqueado, contacte a Support";
                    return Json(login);

                }
                else
                if (CurrentCustomer.bill_status != "O")
                {
                    login.error = true;
                    login.errorMensaje = "Usuario bloqueado, contacte a Support";
                    return Json(login);

                }
                else
                if (CurrentCustomer.IsDISTRIB)
                {
                    if (CurrentCustomer.i_customer_type == 3)
                    {
                        Session["TipoTienda"] = TipoTienda.Padre;

                    }
                    else
                    {
                        Session["TipoTienda"] = TipoTienda.Hijo;

                        Session["CurrentAccont"] = CurrentCustomer;
                        var dis = await ID_DISTRIBUIDOR();
                        ;

                    }

                    await FONDOS();
                   
                    login.error = false;
                    return Json(login);

                }
                else
                {
                    login.error = true;
                    login.errorMensaje = "Usuario sin permisos, contacte a Support";
                    return Json(login);

                }
            }
            else
            {
                login.error = true;
                login.errorMensaje = "El Usuario no existe, contacte a Support";
                return Json(login);
            }
            //else
            //{

            //    #region --------------------------Para las tiendas hijos-----------------------------------------------------------

            //    Session["TipoTienda"] = TipoTienda.Hijo;

            //    Session["CurrentAccont"] = CurrentCustomer;
            //    var CurrentAccont = (customer_info)Session["CurrentAccont"];

            //    if (CurrentAccont != null && CurrentAccont.i_customer != 0)
            //    {
            //        if (CurrentAccont.password != login.pass)
            //        {
            //            login.error = true;
            //            login.errorMensaje = "La Contraseña es incorrecta";
            //            return View(login);

            //        }
            //        else
            //        if (CurrentAccont.blocked == "Y")
            //        {
            //            login.error = true;
            //            login.errorMensaje = "Usuario bloqueado, contacte a Support";
            //            return View(login);

            //        }
            //        else
            //        if (CurrentAccont.bill_status != "O")
            //        {
            //            login.error = true;
            //            login.errorMensaje = "Usuario bloqueado, contacte a Support";
            //            return View(login);

            //        }
            //        else
            //        if (CurrentAccont.IsDISTRIB)
            //        {
            //            Session["TipoTienda"] = TipoTienda.Hijo;
            //            await FONDOS();
            //            return RedirectToAction("Create", "RecargaTiendas");

            //        }
            //        else
            //        {
            //            login.error = true;
            //            login.errorMensaje = "Usuario sin permisos, contacte a Support";
            //            return View(login);

            //        }
            //    }

            //    else
            //    {
            //        login.error = true;
            //        login.errorMensaje = "El Usuario no existe, contacte a Support";
            //        return View(login);

            //    }
            //    #endregion
            //}


        }


        public void Verificar()
        {



        }


        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
