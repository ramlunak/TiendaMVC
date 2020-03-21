using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace TiendaMVC.Class
{

    public static class Colores
    {
        public const string errorRecarga = "#FA8258";
    }


    public enum TipoTienda
    {
        Padre,
        Hijo
    }

    public enum RolTienda
    {
        Gerente,
        Asociado
    }

    public enum TipoTransaction
    {
        New,
        Edit,
        Select
    }

    public enum EstadoRecarga
    {
        Pendiente,
        Recargada,
        Reservada,
        ErrorReserva,
        Error,
        Simulada
    }

    public class NotificacionesMenu
    {
        public int recargar { get; set; }
        public int reservas { get; set; }
        public int historial { get; set; }
        public int asociados { get; set; }
        public int nauta { get; set; }

        public string TipoRecarga = "M";
    }

    public static class _Global
    {
        public const bool ModoPrueba = false;
        public static float ReglaDeTres(float porciento = 0, float total = 0, float porcion = 0)
        {

            try
            {

                int countValidator = 0;
                if (total is 0) countValidator++; if (porciento is 0) countValidator++; if (porcion is 0) countValidator++;
                if (countValidator != 1)
                    return 0;
                else
                {
                    if (porciento is 0)
                    {
                        return porcion * 100 / total;
                    }
                    else if (total is 0)
                    {
                        return porcion * 100 / porciento;
                    }
                    else
                    {
                        return porciento * total / 100;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }



        public static string FromDate
        {
            get
            {
                int thisWeekNumber = GetIso8601WeekOfYear(DateTime.Today);

                DateTime firstDayOfWeek = FirstDateOfWeek(DateTime.Now.Year, thisWeekNumber, CultureInfo.CurrentCulture);
                return firstDayOfWeek.Date.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        public static string ToDate
        {
            get
            {
                var first = DateTime.Parse(FromDate);
                var ultimoSemana = first.AddDays(6);
                return ultimoSemana.Date.ToString("yyyy/MM/dd HH:mm:ss");

            }
        }

        //public static string FromDate
        //{
        //    get
        //    {
        //        return "2019/09/09 00:00:00";
        //    }
        //}

        //public static string ToDate
        //{
        //    get
        //    {
        //        return "2019/09/15 23:59:00";
        //    }
        //}

        public static string GetDateFormat_YYMMDD(DateTime DateTime, string Hora = "inicio")
        {
            var YY = DateTime.Year.ToString();
            var MM = DateTime.Month.ToString();
            var DD = DateTime.Day.ToString();

            if (MM.Length == 1)
                MM = "0" + MM;
            if (DD.Length == 1)
                DD = "0" + DD;

            if (Hora == "inicio")
                return YY + "-" + MM + "-" + DD + " 00:00:00";
            else
                return YY + "-" + MM + "-" + DD + " 23:59:59";
        }

      

        public static customer_info Royber_Tienda_1 = new customer_info
        {
            firstname = "Royber",
            iso_4217 = "USD",
            state = "",
            email = "royberariasmoreo@gmail.com",
            password = "Rr2018*",
            blocked = "N",
            name = "Royber_Tienda_1",
            baddr2 = "{promociones:true}",
            login = "55043317",
            balance = (float)126.75,
            phone1 = "5355043317",
            phone2 = "",
            lastname = "Arias Moreno",
            city = "",
            country = "",
            note = ":DISTRIB:0.02:2:97.5:",
            i_customer = 273601,
            credit_limit = 500
        };

        public static customer_info TiendaPrueba1 = new customer_info
        {
            firstname = "TiendaPrueba1",
            iso_4217 = "USD",
            state = "",
            baddr2 = "{promociones:true}",
            email = "royberariasmoreo@gmail.com",
            password = "Rr2018*",
            blocked = "N",
            name = "Royber_Tienda_1",
            login = "55043317",
            balance = (float)126.75,
            phone1 = "5355043317",
            phone2 = "",
            lastname = "Prueba",
            city = "",
            country = "",
            note = ":DISTRIB:0.02:2:97.5:",
            i_customer = 281109,
            credit_limit = 500
        };

        public static customer_info Royber_Tienda_2 = new customer_info
        {
            firstname = "Luis",
            iso_4217 = "USD",
            state = "",
            email = "royberariasmoreo@gmail.com",
            password = "Royber2018*",
            blocked = "N",
            name = "Royber_Tienda_2",
            login = "55043317",
            balance = (float)468,
            phone1 = "5355043317",
            phone2 = "",
            lastname = "",
            city = "",
            country = "",
            note = ":DISTRIB:0.02:2:97.5:",
            i_customer = 281109,
            credit_limit = 1000
        };

        public static string AuthInfoAdminJson
        {
            get
            {
                var admin = new AuthInfo
                {
                   session_id= "9a5d0bcc0236d542cccc6cf158840562"
                };
                return JsonConvert.SerializeObject(admin);
            }
        }

    
        public static string BaseUrlServicio = "http://smsteleyuma.azurewebsites.net/Service1.svc/";

        public static string BaseUrlAdmin = "https://mybilling.teleyuma.com/rest/";

        public static string BaseUrlCliente = "https://mybilling.teleyuma.com:8444/rest/";

        public static string CodigoVerificacion
        {
            get
            {
                var Codigo = "";

                var ran = new Random();
                var numeros = "123456789".ToCharArray();
                for (int x = 0; x < 4; x++)
                {
                    var ranNumero = ran.Next(numeros.Length);
                    var number = numeros[ranNumero].ToString();
                    Codigo += number;
                }
                return Codigo;
            }

        }

        public class Servicio
        {
            public const string Session = "Session";
            public const string Customer = "Customer";
            public const string Account = "Account";
        }

        public class Metodo
        {
            //Comun

            public const string change_password = "change_password ";

            #region  Session

            public const string login = "login";

            #endregion

            #region  Customer

            public const string get_customer_info = "get_customer_info";
            public const string get_customer_list = "get_customer_list";
            public const string add_customer = "add_customer";
            public const string update_customer = "update_customer";
            public const string make_transaction = "make_transaction";
            public const string get_payment_method_info = "get_payment_method_info";
            public const string update_payment_method = "update_payment_method";
            public const string update_service_features = "update_service_features";
            public const string get_customer_xdrs = "get_customer_xdrs";
            public const string validate_customer_info = "validate_customer_info";


            #endregion

            #region  Account
            public const string get_account_info = "get_account_info";
            public const string get_account_list = "get_account_list";
            public const string add_account = "add_account";
            public const string update_account = "update_account";
            public const string validate_account_info = "validate_account_info";
            public const string get_xdr_list = "get_xdr_list";

            #endregion


        }

        public static string GetMd5Hash(string input)
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

        public static async Task<T> Get<T>(string metodo)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("apiKey", "a6V9NPooCNWzGaaEMsvPvQ==");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(BaseUrlServicio + metodo);
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
        }

        public static async Task<T> Post<T>(string metodo, object entity)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("apiKey", "a6V9NPooCNWzGaaEMsvPvQ==");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.PostAsync(BaseUrlServicio + metodo, entity.AsJsonStringContent());
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
        }

    }



}
