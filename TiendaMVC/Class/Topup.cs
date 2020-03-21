#pragma warning disable CS0246 // El nombre del tipo o del espacio de nombres 'Java' no se encontró (¿falta una directiva using o una referencia de ensamblado?)

#pragma warning restore CS0246 // El nombre del tipo o del espacio de nombres 'Java' no se encontró (¿falta una directiva using o una referencia de ensamblado?)
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Xml;

namespace TiendaMVC.Class
{

    public class topupInfo
    {
        public string login { get; set; }
        public int key { get; set; }
        public string md5 { get; set; }
        public string action { get; set; }
        public string destination_msisdn { get; set; }
        public string msisdn { get; set; }
        public string product { get; set; }

        [DefaultValue("yes")]
        public string sender_sms { get; set; }

        public async Task<topupResponse> movil()
        {
            var Login = "teleyuma_deno";
            var Token = "Yj2F8oVRZB";
            var date = DateTime.Now;
            var Key = date.Year + date.Month + date.Day + date.TimeOfDay.Hours + date.TimeOfDay.Minutes + date.TimeOfDay.Seconds + date.TimeOfDay.Milliseconds;

            var md5Code = GetMd5Hash(Login + Token + Key);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://airtime.transferto.com/cgi-bin/shop/");

                //Simulation
                var param = new topupInfo() { login = Login, key = Convert.ToInt32(Key), md5 = md5Code, msisdn = this.msisdn, destination_msisdn = this.destination_msisdn, product = this.product, sender_sms = this.sender_sms, action = this.action };

                try
                {
                    var response = await client.PostAsXmlAsync("topup", param);
                    var Result = await response.Content.ReadAsStringAsync();

                    //cambiar operator por operador para poder cojer el valos (operator es un nombre reservado de c# y no se puede user en una clase como atributo) 
                    Result = Result.Replace("operator", "operador");

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Result);
                    string json = JsonConvert.SerializeXmlNode(doc);
                    return JsonConvert.DeserializeObject<topupResponseObject>(json).TransferTo;
                }
                catch (Exception ex)
                {
                    return new topupResponse { country = this.destination_msisdn };
                }

            }
        }

        public async Task<nautaResponse> nauta()
        {
            var json = JsonConvert.SerializeObject(this);

            using (var client = new HttpClient())
            {
                var request = GetHttpRequestMessage("https://api.transferto.com/v1.1/transactions/fixed_value_recharges", HttpMethod.Post, new StringContent(json));
                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    string result = await response.Content.ReadAsStringAsync();

                    var errores = JsonConvert.DeserializeObject<Errores>(result);
                    if (errores.errors != null)
                    {
                        var nautaResponse = JsonConvert.DeserializeObject<nautaResponse>(result);
                        nautaResponse.erroe_code = errores.errors[0].code.ToString();
                        nautaResponse.error_message = errores.errors[0].message.ToString();
                        return nautaResponse;
                    }
                    else
                    {
                        var nautaResponse = JsonConvert.DeserializeObject<nautaResponse>(result);
                        nautaResponse.erroe_code = "0";
                        nautaResponse.error_message = "";
                        return nautaResponse;
                    }

                }
                catch (Exception ex)
                {
                    var nautaResponse = new nautaResponse();
                    nautaResponse.erroe_code = "-1";
                    nautaResponse.error_message = "Error de conexción";
                    return nautaResponse;
                }

            }

        }

        static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static HttpRequestMessage GetHttpRequestMessage(string url, HttpMethod method = null, HttpContent content = null)
        {
            string api_key = "9f95e245-4ee9-417c-aecb-8afbde62680c";
            string api_secret = "b9900c85-cd83-4a19-a2b4-292e537288a0";

            int epoch = (int)(DateTime.UtcNow - new DateTime(1980, 1, 1)).TotalSeconds;
            string nonce = epoch.ToString();
            string message = api_key + nonce;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(api_secret);
            HMACSHA256 hmac = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmac.ComputeHash(messageBytes);

            string hmac_base64 = Convert.ToBase64String(hashmessage);
            Console.WriteLine("Hash Base64 code is " + hmac_base64);
            if (method == null)
                method = HttpMethod.Get;
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            request.Headers.Add("X-TransferTo-apikey", api_key);
            request.Headers.Add("X-TransferTo-nonce", nonce);
            request.Headers.Add("X-TransferTo-hmac", hmac_base64);
            if (content != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;
            }
            return request;
        }


    }

    public class topupResponseObject
    {
        public topupResponse TransferTo { get; set; }
    }

    public class topupResponse
    {
        public string transactionid { get; set; }
        public string country { get; set; }
        public string countryid { get; set; }
        public string operador { get; set; }
        public string operatorid { get; set; }
        public string destination_msisdn { get; set; }
        public string authentication_key { get; set; }
        public string error_code { get; set; }
        public string error_txt { get; set; }
    }

    public class topupMovil
    {       

        public int idRecarga { get; set; }

        public string numero { get; set; }

        public float monto { get; set; }              

        public string topupResponseCountry { get { return topupResponse.country; } }

        public string topupResponseOperador { get { return topupResponse.operador; } }

        public string topupResponseErrorCode { get { return topupResponse.error_code; } }

        public string ColorSimulacion { get { return "#70AD47"; } }

        public topupResponse topupResponse = new topupResponse();

        public string error
        {
            get
            {
                if (topupResponse.error_code == "0")
                    return "Recarga satisfactoria";
                else return "Error en la recarga";
            }
        }

        public async Task<bool> Simular()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var param = new topupInfo()
                {
                    msisdn = "69999999999",
                    destination_msisdn = this.numero,
                    product = this.monto.ToString(),
                    sender_sms = "yes",
                    action = "simulation"
                };

                var response = await client.PostAsync("topup", param.AsJsonStringContent());
                var Result = await response.Content.ReadAsStringAsync();
                try
                {
                    this.topupResponse = JsonConvert.DeserializeObject<topupResponse>(Result);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public async Task<bool> Recargar(customer_info customer)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var param = new topupInfo()
                {
                    msisdn = customer.phone1,
                    destination_msisdn = this.numero,
                    product = this.monto.ToString(),
                    sender_sms = "yes",
                    action = "topup"
                };

                var response = await client.PostAsync("topup", param.AsJsonStringContent());
                var Result = await response.Content.ReadAsStringAsync();
                try
                {
                    this.topupResponse = JsonConvert.DeserializeObject<topupResponse>(Result);
                    return true;
                }
                catch
                {
                    return false;
                }

            }


        }
    }

    public class nautaUser
    {
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string first_name { get; set; }
        public string email { get; set; }
        public Int64 mobile { get; set; }

    }

    public class nautaInfo
    {
        public string account_number { get; set; }
        public int product_id { get; set; }
        public int external_id
        {
            get
            {
                return Convert.ToInt32(_Global.CodigoVerificacion);
            }

        }
        public string simulation { get; set; }
        public string recipient_sms_notification { get; set; }
        public bool sender_sms_notification { get; set; }
        public nautaUser sender { get; set; }
        public nautaUser recipient { get; set; }
    }

    public class nautaResponse
    {
        public string transaction_id { get; set; }
        public string simulation { get; set; }
        public string status { get; set; }
        public string status_message { get; set; }
        public string date { get; set; }
        public string account_number { get; set; }
        public string external_id { get; set; }
        public string operator_reference { get; set; }
        public string product_id { get; set; }
        public string product { get; set; }
        public string product_desc { get; set; }
        public string product_currency { get; set; }
        public string product_value { get; set; }
        public string local_currency { get; set; }
        public string local_value { get; set; }
        public string operator_id { get; set; }
        public string country_id { get; set; }
        public string country { get; set; }
        public string account_currency { get; set; }
        public string wholesale_price { get; set; }
        public string retail_price { get; set; }
        public string fee { get; set; }
        public nautaUser sender { get; set; }
        public nautaUser recipient { get; set; }

        public string erroe_code { get; set; }
        public string error_message { get; set; }
    }

    public class Errores
    {
        public Error[] errors { get; set; }

    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }

    }

}
