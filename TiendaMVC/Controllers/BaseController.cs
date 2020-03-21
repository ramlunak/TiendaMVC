using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TiendaMVC.Class;
using TiendaMVC.Extensions.Alerts;
using TiendaMVC.Models;

namespace TiendaMVC.Controllers
{

    public class BaseController : Controller
    {

        public async Task ACTUALIZAR_BALANCE(float monto, string comentario)
        {
            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                await ((customer_info)Session["CurrentCustomer"]).MakeTransaction_Manualcharge((float)monto, comentario);

            }
            else
            {
                await ((customer_info)Session["CurrentAccont"]).MakeTransaction_Manualcharge((float)monto, comentario);
            }

            await FONDOS();
        }

        public async Task ACTUALIZAR_BALANCE(RecargaTienda recarga)
        {
            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                await ((customer_info)Session["CurrentCustomer"]).MakeTransaction_Manualcharge((float)recarga.Costo, "M-" + recarga.Monto + "-" + recarga.CodigoPais + recarga.Numero+"--"+recarga.Nombre);

            }
            else
            {
                await ((customer_info)Session["CurrentAccont"]).MakeTransaction_Manualcharge((float)recarga.Costo, "M-" + recarga.Monto + "-" + recarga.CodigoPais + recarga.Numero + "--" + recarga.Nombre);
            }

            await FONDOS();
        }

        public async Task ACTUALIZAR_BALANCE(RecargaNauta recarga)
        {
            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                await ((customer_info)Session["CurrentCustomer"]).MakeTransaction_Manualcharge((float)recarga.Costo, "Nauta-" + recarga.Monto + "-" + recarga.Numero);

            }
            else
            {
                await ((customer_info)Session["CurrentAccont"]).MakeTransaction_Manualcharge((float)recarga.Costo, "Nauta-" + recarga.Monto + "-" + recarga.Numero);
            }

            await FONDOS();
        }

        public async Task<bool> ACTUALIZAR_DATOS()
        {

            if (Session["TipoTienda"] == null) return false;

            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                Session["CurrentCustomer"] = await ((customer_info)Session["CurrentCustomer"]).GetCustomerInfo();
                if (Session["CurrentCustomer"] == null)
                {
                    return false;
                }
                else return true;
            }
            else
            {
                Session["CurrentAccont"] = await ((customer_info)Session["CurrentAccont"]).GetCustomerInfo();
                var s = (customer_info)Session["CurrentAccont"];
                if (Session["CurrentAccont"] == null)
                {
                    return false;
                }
                else return true;
            }

        }

        public string ID_CUENTA
        {
            get
            {

                if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
                {
                    if ((customer_info)Session["CurrentCustomer"] == null) return "0";
                    return ((customer_info)Session["CurrentCustomer"]).i_customer.ToString();
                }
                else
                {
                    if ((customer_info)Session["CurrentAccont"] == null) return "0";
                    return ((customer_info)Session["CurrentAccont"]).i_customer.ToString();
                }
            }
        }

        public async Task<int> ID_DISTRIBUIDOR()
        {
            
            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                return ((customer_info)Session["CurrentCustomer"]).i_customer;
            }
            else
            {
                var padre = await ((customer_info)Session["CurrentAccont"]).GetParent();
                return padre.i_customer;
            }


        }


        public async Task<float> FONDOS_TIENDAPADRE()
        {           
            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                var tienda = await ((customer_info)Session["CurrentCustomer"]).GetCustomerInfo();
                return tienda.fondosDisponibles;

            }
            else
            {
                var padre = await ((customer_info)Session["CurrentAccont"]).GetParent();
                return padre.fondosDisponibles;
            }

        }


        public string PHONE1
        {
            get
            {
                if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
                {
                    return ((customer_info)Session["CurrentCustomer"]).phone1;
                }
                else
                {
                    return ((customer_info)Session["CurrentAccont"]).phone1;
                }
            }
        }

        public float IMPUESTO
        {
            get
            {
                if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
                {
                    return (float)((customer_info)Session["CurrentCustomer"]).GetComisionTopUp();
                }
                else
                {
                    return (float)((customer_info)Session["CurrentAccont"]).GetComisionTopUp();
                }
            }
        }

        public float TARIFA_SMS
        {
            get
            {
                if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
                {
                    return (float)((customer_info)Session["CurrentCustomer"]).GetTarifaSms();
                }
                else
                {
                    return (float)((customer_info)Session["CurrentAccont"]).GetTarifaSms();
                }
            }
        }

        public async Task<float> FONDOS()
        {

            if (Session["TipoTienda"] == null) return 0;
            await ACTUALIZAR_DATOS();
            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                Session["BALANCE_CUSTOMER"] = ((customer_info)Session["CurrentCustomer"]).balance;
                Session["FONDOS"] = await ((customer_info)Session["CurrentCustomer"]).GetFondosDisponibles();
                return (float)Session["FONDOS"];

            }
            else
            {
                Session["CurrentAccont"] = await ((customer_info)Session["CurrentAccont"]).GetCustomerInfo();
                try
                {
                    Session["BALANCE_CUSTOMER"] = ((customer_info)Session["CurrentAccont"]).balance;


                }
                catch 
                {

                    ;
                }
                if (Session["CurrentAccont"] == null)
                {
                    return 0;
                }

                Session["FONDOS"] = ((customer_info)Session["CurrentAccont"]).fondosDisponibles;
                return (float)Session["FONDOS"];
            }
        }

       
        public bool IsLogin()
        {
            var islogin = true;
            if (Session["TipoTienda"] == null)
            {
                return false;
            }

            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Padre)
            {
                if ((customer_info)Session["CurrentCustomer"] == null)
                    return islogin = false;
                if (((customer_info)Session["CurrentCustomer"]).blocked == "Y")
                {
                    return islogin = false;
                }
            }
            if ((TipoTienda)Session["TipoTienda"] == TipoTienda.Hijo)
            {
                if ((customer_info)Session["CurrentAccont"] == null)
                    return islogin = false;
                if (((customer_info)Session["CurrentAccont"]).blocked == "Y")
                {
                    return islogin = false;
                }
            }
            return islogin;
        }


        public void ShowSuccess(string message, bool dismissable = true)
        {

            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void ShowInformation(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void ShowWarning(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void ShowDanger(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

    }


}