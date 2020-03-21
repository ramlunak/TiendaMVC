using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TiendaMVC.Class
{

    [DataContract(Namespace = "http://www.TeleYuma.com")]
    [Serializable]
    public class MessageResponse
    {
        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract(Namespace = "http://www.TeleYuma.com")]
    [Serializable]
    public class innoverit_sms
    {
        [DataMember]
        public string NumeroTelefono { get; set; }
        [DataMember]
        public string SMS { get; set; }

        public async Task<innoverit> Enviar()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("apiKey", "a6V9NPooCNWzGaaEMsvPvQ==");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                var URL = "https://www.innoverit.com/api/smssend/?apikey=04a26d8f1534598bdf73fb93a0025867&number=+" + this.NumeroTelefono + "&content=" + this.SMS;

                try
                {
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<innoverit>(json);
                }
                catch (Exception ex)
                {
                    return new innoverit
                    {
                        delivery_status = "-1",
                        error = "No se pudo establecer conexión con el servidor."
                    };

                }

            }

        }

    }


}
