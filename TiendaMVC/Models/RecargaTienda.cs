using TiendaMVC.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using TiendaMVC.Teleyuma;

namespace TiendaMVC.Models
{
    public class RecargaTienda
    {
        public int id { get; set; }
        public string IdCuenta { get; set; }
        public string TipoRecarga { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int CodigoPais { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Numero { get; set; }
   
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Monto { get; set; }

        [NotMapped]
        public string Nombre { get; set; }

        public decimal Costo
        {
            get
            {
                decimal coste = 0;

                coste = Convert.ToDecimal(((Monto * Impuesto) / 100));

                return coste;
            }
        }
        [NotMapped]
        public string Asociado { get; set; }
        [NotMapped]
        public float CostoXdr { get; set; }
        public float Impuesto { get; set; }
        public string Fecha { get; set; }       
        public string Estado { get; set; }
        public string remitente { get; set; }
        public string Error { get; set; }
        public string ColorError { get; set; }
        public float Importe { get { return ((Monto * Impuesto) / 100); } }

        [NotMapped]
        public topupMovil topupMovil = new topupMovil();

        //public async Task<bool> Simular(string pnone1)
        //{

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var param = new topupInfo()
        //        {
        //            msisdn = pnone1,
        //            destination_msisdn = this.CodigoPais + this.Numero,
        //            product = this.Monto.ToString(),
        //            sender_sms = "yes",
        //            action = "simulation"
        //        };

        //        var response = await client.PostAsync("topup", param.AsJsonStringContent());
        //        var Result = await response.Content.ReadAsStringAsync();
        //        try
        //        {
        //            topupMovil.topupResponse = JsonConvert.DeserializeObject<topupResponse>(Result);
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        //public async Task<bool> recargar(string pnone1)
        //{

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var param = new topupInfo()
        //        {
        //            msisdn = pnone1,
        //            destination_msisdn = this.CodigoPais + this.Numero,
        //            product = this.Monto.ToString(),
        //            sender_sms = "yes",
        //            action = "topup"
        //        };

        //        var response = await client.PostAsync("topup", param.AsJsonStringContent());
        //        var Result = await response.Content.ReadAsStringAsync();
        //        try
        //        {
        //            topupMovil.topupResponse = JsonConvert.DeserializeObject<topupResponse>(Result);
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}


        public async Task<Ding.SendTransferResponse> DingSimular(string phone1)
        {
            var reguest = new Ding.SendTransferRequest {
                AccountNumber = this.CodigoPais + this.Numero,
                SkuCode = "CU_CU_TopUp",
                SendValue = this.Monto,
                ValidateOnly = true,
                DistributorRef = phone1
            };

            return await Ding.SendTransfer(reguest);
        }


        public async Task<Ding.SendTransferResponse> DingRecargar(string phone1)
        {
            var reguest = new Ding.SendTransferRequest
            {
                AccountNumber = this.CodigoPais + this.Numero,
                SkuCode = "CU_CU_TopUp",
                SendValue = this.Monto,
                ValidateOnly = false,
                DistributorRef = phone1
            };

            return await Ding.SendTransfer(reguest);
        }



    }
}