using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TiendaMVC.Class;

namespace TiendaMVC.Models
{
    public class RecargaNauta
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

        public int product_id { get; set; }
        public bool sender_sms_notification { get; set; }
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
        public float Importe => ((Monto * Impuesto) / 100);

        public nautaResponse nautaResponse = new nautaResponse();


        public async Task<Ding.SendTransferResponse> DingSimular(object cuenta)
        {

            var phone1 = "";


            if (cuenta is customer_info)
            {
                var customer = cuenta as customer_info;
                phone1 = customer.phone1;
            }
            else
                if (cuenta is account_info)
            {
                var account = cuenta as account_info;
                phone1 = account.phone1;
            }


            var reguest = new Ding.SendTransferRequest
            {
                AccountNumber = this.CodigoPais + this.Numero,
                SkuCode = "CU_NU_TopUp",
                SendValue = this.Monto,
                ValidateOnly = true,
                DistributorRef = phone1
            };

            return await Ding.SendTransfer(reguest);
        }

        public async Task<Ding.SendTransferResponse> DingRecargar(object cuenta)
        {

            var phone1 = "";


            if (cuenta is customer_info)
            {
                var customer = cuenta as customer_info;
                phone1 = customer.phone1;
            }
            else
                if (cuenta is account_info)
            {
                var account = cuenta as account_info;
                phone1 = account.phone1;
            }


            var reguest = new Ding.SendTransferRequest
            {
                AccountNumber = this.CodigoPais + this.Numero,
                SkuCode = "CU_NU_TopUp",
                SendValue = this.Monto,
                ValidateOnly = false,
                DistributorRef = phone1
            };

            return await Ding.SendTransfer(reguest);
        }

        //public async Task<bool> Simular(object cuenta)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var lastname = "";
        //        var firstname = "";
        //        var phone1 = "";
        //        var email = "";


        //        if (cuenta is customer_info)
        //        {
        //            var customer = cuenta as customer_info;
        //            lastname = customer.lastname;
        //            firstname = customer.firstname;
        //            phone1 = customer.phone1;
        //            email = customer.email;

        //        }
        //        else
        //            if (cuenta is account_info)
        //        {
        //            var account = cuenta as account_info;
        //            lastname = account.lastname;
        //            firstname = account.firstname;
        //            phone1 = account.phone1;
        //            email = account.email;
        //        }

        //        var nauta = new nautaInfo
        //        {
        //            account_number = this.Numero,
        //            product_id = this.product_id,
        //            simulation = "1",
        //            recipient_sms_notification = "1",
        //            sender_sms_notification = true,
        //            sender = new nautaUser
        //            {
        //                last_name = lastname,
        //                middle_name = "",
        //                first_name = firstname,
        //                email = email,
        //                mobile = Convert.ToInt64(phone1)
        //            },
        //            recipient = new nautaUser
        //            {
        //                last_name = "",
        //                middle_name = "",
        //                first_name = "",
        //                email = this.Numero,
        //                mobile = 99999999
        //            }
        //        };

        //        try
        //        {
        //            var response = await client.PostAsync("nauta", nauta.AsJsonStringContent());
        //            var Result = await response.Content.ReadAsStringAsync();
        //            var nautaResponse = JsonConvert.DeserializeObject<nautaResponse>(Result);
        //            this.nautaResponse = nautaResponse;
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            var nautaResponse = new nautaResponse();
        //            nautaResponse.erroe_code = "-1";
        //            nautaResponse.error_message = "Error de conección";
        //            this.nautaResponse = nautaResponse;
        //            return false;
        //        }

        //    }
        //}

        //public async Task<bool> Recargar(object cuenta)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var lastname = "";
        //        var firstname = "";
        //        var phone1 = "";
        //        var email = "";


        //        if (cuenta is customer_info)
        //        {
        //            var customer = cuenta as customer_info;
        //            lastname = customer.lastname;
        //            firstname = customer.firstname;
        //            phone1 = customer.phone1;
        //            email = customer.email;

        //        }
        //        else
        //            if (cuenta is account_info)
        //        {
        //            var account = cuenta as account_info;
        //            lastname = account.lastname;
        //            firstname = account.firstname;
        //            phone1 = account.phone1;
        //            email = account.email;
        //        }

        //        var nauta = new nautaInfo
        //        {
        //            account_number = this.Numero,
        //            product_id = this.product_id,
        //            simulation = "0",
        //            recipient_sms_notification = "1",
        //            sender_sms_notification = true,
        //            sender = new nautaUser
        //            {
        //                last_name = lastname,
        //                middle_name = "",
        //                first_name = firstname,
        //                email = email,
        //                mobile = Convert.ToInt64(phone1)
        //            },
        //            recipient = new nautaUser
        //            {
        //                last_name = "",
        //                middle_name = "",
        //                first_name = "",
        //                email = this.Numero,
        //                mobile = 99999999
        //            }
        //        };

        //        try
        //        {
        //            var response = await client.PostAsync("nauta", nauta.AsJsonStringContent());
        //            var Result = await response.Content.ReadAsStringAsync();
        //            var nautaResponse = JsonConvert.DeserializeObject<nautaResponse>(Result);
        //            this.nautaResponse = nautaResponse;
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            var nautaResponse = new nautaResponse();
        //            nautaResponse.erroe_code = "-1";
        //            nautaResponse.error_message = "Error de conección";
        //            this.nautaResponse = nautaResponse;
        //            return false;
        //        }


        //    }

        //}
    }
}