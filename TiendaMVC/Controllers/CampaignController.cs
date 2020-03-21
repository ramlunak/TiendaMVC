using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TiendaMVC.Class;
using TiendaMVC.Models;

namespace TiendaMVC.Controllers
{
    public class CampaignController : BaseController
    {
        private readonly TiendaMVCContext db = new TiendaMVCContext();

        public ActionResult Index()
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");
            return View(db.Campaing.Where(x => x.accontId == ID_CUENTA.ToString()).ToList());
        }

        public static List<string> ListaSMS = new List<string>();
        public static int TotalSMS = 0;
        public static int SMSEnviados = 0;       
        public static string RutaFile;
        public static Campaign Campaign = new Campaign();
        public async Task<ActionResult> EnviarSMS()
        {
            if (SMSEnviados == 0) TotalSMS = ListaSMS.Count;
            if (ListaSMS.Any())
            {
                try
                {
                    //await Task.Delay(2000);
                    //Campaign.balanceFinal += 1;
                    //Campaign.cost += 2;
                    var sms = new innoverit_sms
                    {
                        NumeroTelefono = ListaSMS.First(),
                        SMS = Campaign.sms
                    };

                    var responce = await sms.Enviar();

                    if (responce.error != null)
                    {
                        Campaign.smsCountError++;
                    }
                    else
                    {
                        Campaign.balanceFinal = (float)Convert.ToDecimal(responce.balance, CultureInfo.CreateSpecificCulture("en-US"));
                        Campaign.cost += Campaign.countSMS * (float)Convert.ToDecimal((responce.costo), CultureInfo.CreateSpecificCulture("en-US")) + Campaign.costBySms;
                    }

                    ListaSMS.RemoveAt(0);
                    SMSEnviados++;

                    return Json(new { Total = TotalSMS, Enviados = SMSEnviados }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                   
                    // guardar en la base si se mandaron sms

                    return Json(new { fin = "si", respuesta = ex.Message }, JsonRequestBehavior.AllowGet);

                }
                
            }
            else
            {
              
                // guardar en la base
                return Json(new { fin = "si", respuesta = "No hay numeros en la lista" }, JsonRequestBehavior.AllowGet);
            }


        }

        public async Task<ActionResult> Guardar()
        {
            var campaign = Campaign;
            if (SMSEnviados == 0)
            {               
                return Json(new { fin = "si", respuesta = "No se pudo enviar los sms, revise la lista de numeros" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Session["EditarPromocion"] != null)
                {
                    var promocion = (Campaign)Session["EditarPromocion"];
                    campaign.id = promocion.id;
                    campaign.date = promocion.date;
                    db.Entry(campaign).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
                else
                {
                    db.Campaing.Add(campaign);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }
                //  Enviar sms admin
                var totalSms = SMSEnviados;
               var numero = "17862719040";
                //var numero = "5355043317";
                var supportSms = new innoverit_sms
                {
                    NumeroTelefono = numero,
                    SMS = "La cuenta con id: " + campaign.accontId + " ha realizado una promocion con el nombre:'" + campaign.name + "' con un total de: " + totalSms + " sms para un monto de: $" + campaign.cost.To_0_00() + " USD, Su balance es de: $" + campaign.balanceFinal.To_0_00() + " USD",
                };
                await supportSms.Enviar();

                await ACTUALIZAR_BALANCE(campaign.cost, "promocion-" + totalSms + "-" + campaign.balanceFinal.To_0_00());

                Session["EditarPromocion"] = null;

                Session["CampaignName"] = null;
                Session["CampaignSms"] = null;



                SMSEnviados = 0;
                TotalSMS = 0;
                campaign = new Campaign();

                return Json(new { fin = "si", respuesta = "La promoción se ha enviado." }, JsonRequestBehavior.AllowGet);
            }




        }


        public async Task<ActionResult> Cargar(HttpPostedFileBase file, string CampaignName, string CampaignSms)
        {
            if (!IsLogin()) return RedirectToAction("Index", "Login");

            Session["CampaignName"] = CampaignName;
            Session["CampaignSms"] = CampaignSms;

            if (CampaignName == "" || CampaignName == null || CampaignSms == "" || CampaignSms == null || CampaignSms == "\r\n")
            {
                Session["msg"] = "Complete el formulario";
                return RedirectToAction("Index", "Campaign");
            }


            if (file != null)
            {
                if (!file.GetNumberList().Any())
                {
                    Session["msg"] = "El archivo no contiene una lista de numeros teléfonicos.";
                    return RedirectToAction("Index", "Campaign");
                }
            }




            //insertar nuevo file

            var lista = new List<string>();
            var ruta = "";

            //modificar promocion
            if (Session["EditarPromocion"] != null && file == null)
            {
                RutaFile = ruta = ((Campaign)Session["EditarPromocion"]).path;
                ListaSMS = lista = ruta.GetNumberList();
                // goto etk_enviar_sms;


                Campaign.accontId = ID_CUENTA;
                Campaign.name = CampaignName;
                Campaign.sms = CampaignSms;
                Campaign.path = ruta;
                Campaign.date = DateTime.Now;
                Campaign.numbers = lista;
                
                Campaign.costBySms = TARIFA_SMS;
                Campaign.cost = (float)0;

                Session["msg"] = "El archivo se ha cargado correctamente en breve se enviará su campaña.";
                Session["enviar"] = "si";
               
                return RedirectToAction("Index", "Campaign", db.Campaing.Where(x => x.accontId == ID_CUENTA.ToString()).ToList());
            }
            else
            if (Session["EditarPromocion"] != null && file != null)
            {

                var result = ((Campaign)Session["EditarPromocion"]).path.DeleteFile();
                if (!result)
                {
                    Session["msg"] = "No se pudo acceder al archivo.";
                    return RedirectToAction("Index", "Campaign");
                }
                else
                {
                    var FileName = Path.GetFileName(file.FileName);

                    var ext = "";
                    if (FileName.Contains(".txt"))
                        ext = ".txt";
                    if (FileName.Contains(".csv"))
                        ext = ".csv";

                    ruta = Server.MapPath("~/Files/" + ((Campaign)Session["EditarPromocion"]).name + ext);

                    file.SaveAs(ruta);
                }


                Campaign.accontId = ID_CUENTA;
                Campaign.name = CampaignName;
                Campaign.sms = CampaignSms;
                Campaign.path = ruta;
                Campaign.date = DateTime.Now;
                Campaign.numbers = lista;


                Campaign.costBySms = TARIFA_SMS;
                Campaign.cost = (float)0;

            }
            else
                if (Session["EditarPromocion"] == null)
            {
                if (file == null)
                {
                    Session["msg"] = "Seleccione el archivo";
                    return RedirectToAction("Index", "Campaign");
                }


                if (db.Campaing.Where(x => x.name == CampaignName).Any())
                {
                    Session["msg"] = "Ya existe una promoción con este nombre.";
                    return RedirectToAction("Index", "Campaign");
                }

                var FileName = Path.GetFileName(file.FileName);
                var ext = "";
                if (FileName.Contains(".txt"))
                    ext = ".txt";
                if (FileName.Contains(".csv"))
                    ext = ".csv";
                RutaFile = ruta = Server.MapPath("~/Files/" + CampaignName + ext);
                try
                {

                    file.SaveAs(ruta);


                    Campaign.accontId = ID_CUENTA;
                    Campaign.name = CampaignName;
                    Campaign.sms = CampaignSms;
                    Campaign.path = ruta;
                    Campaign.date = DateTime.Now;
                    Campaign.numbers = lista;


                    Campaign.costBySms = TARIFA_SMS;
                    Campaign.cost = (float)0;

                }
                catch (Exception)
                {
                    Session["msg"] = "El sistema no tiene permisos para habrir el archivo.";
                    return RedirectToAction("Index", "Campaign");
                }

                ListaSMS = lista = ruta.GetNumberList();
                if (!lista.Any())
                {
                    ruta.DeleteFile();
                    Session["msg"] = "El archivo no contiene una lista de numeros teléfonicos.";
                    return RedirectToAction("Index", "Campaign");
                }
                else
                {

                    Campaign.accontId = ID_CUENTA;
                    Campaign.name = CampaignName;
                    Campaign.sms = CampaignSms;
                    Campaign.path = ruta;
                    Campaign.date = DateTime.Now;
                    Campaign.numbers = lista;


                    Campaign.costBySms = TARIFA_SMS;
                    Campaign.cost = (float)0;
                    //enviar sms
                    Session["msg"] = "El archivo se ha cargado correctamente en breve se enviará su campaña.";
                    Session["enviar"] = "si";
                    return RedirectToAction("Index", "Campaign", db.Campaing.Where(x => x.accontId == ID_CUENTA.ToString()).ToList());
                }
                ;
            }


            Campaign.accontId = ID_CUENTA;
            Campaign.name = CampaignName;
            Campaign.sms = CampaignSms;
            Campaign.path = ruta;
            Campaign.date = DateTime.Now;
            Campaign.numbers = lista;


            Campaign.costBySms = TARIFA_SMS;
            Campaign.cost = (float)0;

            Session["msg"] = "El archivo se ha cargado correctamente en breve se enviará su campaña.";
            Session["enviar"] = "si";
            return RedirectToAction("Index", "Campaign", db.Campaing.Where(x => x.accontId == ID_CUENTA.ToString()).ToList());

            // etk_enviar_sms:


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

                Campaign promocion = db.Campaing.Find(id);

                if (promocion == null)
                {
                    return HttpNotFound();
                }

                Session["EditarPromocion"] = promocion;
                return RedirectToAction("Index", "Campaign");

            }
        }


    }
}