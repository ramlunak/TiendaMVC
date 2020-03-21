using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TiendaMVC.Class
{
    public class account_info
    {
        //solo para capturar el valor del campo de texto impuesto wue viene de la vista al conrolador
        [DataMember]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public float impuesto { get; set; }

        [DataMember]
        public int i_account { get; set; }
        [DataMember]

        public string id { get; set; }
        [DataMember]

        public int i_customer { get; set; }
        [DataMember]

        public int billing_model { get; set; }
        [DataMember]

        public string activation_date { get; set; }
        [DataMember]

        public string bill_status { get; set; }
        [DataMember]

        public int i_time_zone { get; set; }
        [DataMember]

        public string faxnum { get; set; }
        [DataMember]

        public bool EnabledSmsDestinatario
        {
            get
            {
                if (faxnum == "" || faxnum == null)
                    return false;
                try
                {
                    var value = JsonConvert.DeserializeObject<EnabledSmsDestinatario>(faxnum).destinatario;
                    return value;
                }
                catch
                {

                    return false;
                }

            }
        }

        public string baddr2 { get; set; }

        public bool EnabledPromociones
        {
            get
            {
                if (baddr2 == "" || baddr2 == null)
                    return false;
                try
                {
                    var value = JsonConvert.DeserializeObject<EnabledPromocionesBySms>(baddr2).promociones;
                    return value;
                }
                catch
                {
                    return false;
                }

            }
        }

        [DataMember]
        public int i_product { get; set; }
        [DataMember]

        public int i_distributor { get; set; }

        [DataMember]

        public string batch_name { get; set; }
        [DataMember]

        public int control_number { get; set; }
        [DataMember]

        public string iso_4217 { get; set; }

        public float opening_balance { get; set; }

        private float _credit_limit { get; set; }
        [DataMember]

        public float credit_limit
        {
            get
            {
                return (float)decimal.Round(Convert.ToDecimal(_credit_limit), 5);
            }
            set
            {
                _credit_limit = (float)decimal.Round(Convert.ToDecimal(value), 5);
            }
        }

        [DataMember]
        public float fondosDisponibles { get { return credit_limit - balance; } }
        [DataMember]

        public float balance { get; set; }
        [DataMember]
        public int i_account_balance_control_type { get; set; }

        [DataMember]

        public decimal balance2 { get { return decimal.Round(Convert.ToDecimal(balance), 2); } }

        [DataMember]

        public string login { get; set; }

        [DataMember]

        public string blocked { get; set; }


        [DataMember]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "La contraseña debe tener mas de 5 caracteres y menos de 17")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,22}$", ErrorMessage = "La contraseña debe contener dígitos, al menos una letra miniscula y una mayuscula y un caracter especial(Ejemplo:Fulano2* , w201821A*)")]
        public string password { get; set; }

        [DataMember]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }


        [DataMember]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string firstname { get; set; }
        [DataMember]

        public string lastname { get; set; }

        [DataMember]

        public string fullname { get { return firstname + " " + lastname; } }

        [DataMember]

        public string iniciales
        {
            get
            {
                var inicials = "";
                try
                {
                    var inicial_nombre = firstname.ToCharArray()[0].ToString().ToUpper();
                    inicials += inicial_nombre;

                }
                catch
                {

                }

                try
                {
                    var inicial_apellido = lastname.ToCharArray()[0].ToString().ToUpper();
                    inicials += inicial_apellido;
                }
                catch
                {

                }


                return inicials;
            }
        }
        [DataMember]

        public string cont1 { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El número esta mal escrito")]
        public string phone1 { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string email { get; set; }
        [DataMember]

        public string country { get; set; }
        [DataMember]
        public string note { get; set; }
        [DataMember]
        public string phone2 { get; set; }

        [DataMember]
        public float LimiteCredito
        {
            get
            {
                try
                {
                    return (float)Convert.ToDecimal(phone2);
                }
                catch
                {

                    return 0;
                }
            }
            set
            {
                phone2 = value.ToString();
            }

        }
        [DataMember]

        public string h323_password { get; set; }
        [DataMember]

        public string ecommerce_enabled = "Y";

        [DataMember]
        public MakeAccountTransactionResponse AccountTransactionResponse = new MakeAccountTransactionResponse();

        [DataMember]

        public ErrorHandling ErrorHandlingCrearCuenta = new ErrorHandling();

        public Double GetComisionTopUp()
        {
            if (note != null)
            {
                var disArray = this.note.Split(':');
                var comision = "100";
                try
                {
                    comision = disArray[4].ToString();
                }
                catch
                {
                }

                decimal digito = decimal.Parse(comision, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                return Convert.ToDouble(digito);
            }
            else
                return 0;
        }

        public float GetTarifaSms()
        {
            if (note != null)
            {
                var disArray = this.note.Split(':');
                var tarifa = disArray[2].ToString();
                decimal digito = decimal.Parse(tarifa, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                return (float)digito;
            }
            else
                return 0;
        }

        public bool IsDISTRIB
        {

            get
            {
                var CurrentAccont = this;
                if (CurrentAccont.note != null && CurrentAccont.note != "")
                {
                    var disArray = CurrentAccont.note.Split(':');
                    if (disArray[1] == "DISTRIB")
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        //METODOS

        public async Task<ErrorHandling> Validar()
        {
            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = this });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.validate_account_info + "/" + _Global.AuthInfoAdminJson + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        return new ErrorHandling
                        {
                            faul = false

                        };
                    }
                    else
                    {
                        if (ErrorHandling.faultcode == "Server.Account.duplicate_login")
                        {

                            ErrorHandling.faul = true;
                            ErrorHandling.faultstring = "Ya existe una cuenta registrada con este numero de telefono,por favor pruebe con otro";
                        }

                        if (ErrorHandling.faultcode == "Client.Account.too_short.account_info.login")
                        {

                            ErrorHandling.faul = true;
                            ErrorHandling.faultstring = "El numero de numero de telefono esta mal escrito";
                        }

                        return ErrorHandling;
                    }

                }
                catch (Exception ex)
                {

                    return new ErrorHandling
                    {
                        faul = true,
                        faultstring = "No se pudo conectar con el servidor,compruebe su conexión o contacte a support"
                    };
                }

            }

        }

        public async Task<ErrorHandling> Crear()
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = this });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.add_account + "/" + _Global.AuthInfoAdminJson + "/" + param;
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {

                        return new ErrorHandling
                        {
                            faul = false,
                            faultstring = "Cuenta agregada correctamente"

                        };

                    }
                    else
                    {
                        if (ErrorHandling.faultcode == "Server.Account.duplicate_login" || ErrorHandling.faultcode == "Server.Account.duplicate_id")
                        {

                            ErrorHandling.faul = true;
                            ErrorHandling.faultstring = "Ya existe una cuenta registrada con este numero de telefono,por favor pruebe con otro";
                        }

                        if (ErrorHandling.faultcode == "Client.Account.too_short.account_info.login")
                        {

                            ErrorHandling.faul = true;
                            ErrorHandling.faultstring = "El numero de numero de telefono esta mal escrito";
                        }

                        if (ErrorHandling.faultstring == "Failed to add Account: The service password should not be easy to guess.")
                        {

                            ErrorHandling.faul = true;
                            ErrorHandling.faultstring = "La clave de la cuenta es insegura, por favor escriba otra";
                        }

                        return ErrorHandling;
                    }
                }
                catch (Exception ex)
                {
                    return new ErrorHandling
                    {
                        faul = true,
                        faultstring = "No se pudo conectar con el servidor,compruebe su conexión o contacte a support"
                    };
                }



            }

        }

        public async Task<bool> ActualizarAccount()
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = this });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.update_account + "/" + _Global.AuthInfoAdminJson + "/" + param;
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch
                {
                    return false;
                }

            }

        }

        public async Task<account_info> GetGetAccountByUser(string user)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { login = user });
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_account_info + "/" + _Global.AuthInfoAdminJson + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        return JsonConvert.DeserializeObject<AccountObject>(json).account_info;
                    }
                    else
                        return new account_info();
                }
                catch (Exception ex)
                {
                    return new account_info();
                }

            }

        }

        public async Task<account_info> GetAccountInfo()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(this);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_account_info + "/" + _Global.AuthInfoAdminJson + "/" + param;
                    var response = await client.GetStringAsync(URL);
                    return JsonConvert.DeserializeObject<AccountObject>(response).account_info;

                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        public async Task<GetAccountXDRListResponse> GetAccountXDR(GetAccountXDRListRequest GetAccountXDRListRequest)
        {
            GetAccountXDRListRequest.i_account = this.i_account.ToString();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(GetAccountXDRListRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_xdr_list + "/" + _Global.AuthInfoAdminJson + "/" + param;
                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GetAccountXDRListResponse>(Result);

                }
                catch (Exception ex)
                {

                }
                return new GetAccountXDRListResponse();
            }
        }

        public async Task<MakeAccountTransactionResponse> MakeTransaction_Manualcredit(float costo, string visible_comment)
        {
            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();
            MakeAccountTransactionRequest.i_account = this.i_account;
            MakeAccountTransactionRequest.amount = decimal.Round(Convert.ToDecimal(costo), 2);
            MakeAccountTransactionRequest.action = "Manual credit";
            MakeAccountTransactionRequest.visible_comment = visible_comment;

            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + _Global.AuthInfoAdminJson + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    MakeAccountTransactionResponse = JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);
                    return MakeAccountTransactionResponse;

                }
                catch
                {
                    return null;
                }
            }

        }

        public async Task<MakeAccountTransactionResponse> MakeTransaction_Manualcharge(float costo, string visible_comment)
        {
            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();
            MakeAccountTransactionRequest.i_account = this.i_account;
            MakeAccountTransactionRequest.amount = decimal.Round(Convert.ToDecimal(costo), 2);
            MakeAccountTransactionRequest.action = "Manual charge";
            MakeAccountTransactionRequest.visible_comment = visible_comment;

            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + _Global.AuthInfoAdminJson + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    MakeAccountTransactionResponse = JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);
                    return MakeAccountTransactionResponse;

                }
                catch
                {
                    return null;
                }
            }

        }


    }
    public class AccountObject
    {
        [DataMember]
        public account_info account_info { get; set; }
    }

    public class GetAccountXDRListRequest
    {

        [DataMember]
        public string i_account { get; set; }
        [DataMember]
        public string from_date { get; set; }
        [DataMember]
        public string to_date { get; set; }

    }

    public class GetAccountXDRListResponse
    {
        [DataMember]
        public XDRInfo[] xdr_list { get; set; }
        [DataMember]
        public int total { get; set; }
    }


    public class MakeAccountTransactionRequest
    {
        [DataMember]
        public int i_account { get; set; }
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public decimal amount { get; set; }
        [DataMember]
        public string visible_comment { get; set; }
        [DataMember]
        public string transaction_id { get; set; }

    }

    public class MakeAccountTransactionResponse
    {

        [DataMember]
        public string i_payment_transaction { get; set; }
        [DataMember]
        public float balance { get; set; }
        [DataMember]
        public string transaction_id { get; set; }
        [DataMember]
        public string authorization { get; set; }
        [DataMember]
        public string result_code { get; set; }
        [DataMember]
        public string i_xdr { get; set; }

    }


}
