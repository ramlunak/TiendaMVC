using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TiendaMVC.Class
{
    public class customer_info
    {

        [DataMember]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public float impuesto { get; set; }
        [DataMember]
        public int i_customer { get; set; }
        [DataMember]
        public int i_customer_type { get; set; }

        [DataMember]

        public float balance { get; set; }
        [DataMember]

        public int billing_model { get; set; }
        [DataMember]

        public string activation_date { get; set; }
        [DataMember]

        public string bill_status { get; set; }

        [DataMember]
        public string name { get; set; }
        [DataMember]

        public string iso_4217 { get; set; }
        [DataMember]
        public int i_product { get; set; }
        [DataMember]

        public int i_distributor { get; set; }
        [DataMember]

        public string batch_name { get; set; }
        [DataMember]

        public int control_number { get; set; }
        [DataMember]
        public float opening_balance { get; set; }
        [DataMember]

        public string cont1 { get; set; }
        [DataMember]

        public string h323_password { get; set; }
        [DataMember]

        public string companyname { get; set; }
        [DataMember]

        public string firstname { get; set; }
        [DataMember]

        public string lastname { get; set; }
        [DataMember]

        public string fullname => firstname + " " + lastname;
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

        public string city { get; set; }
        [DataMember]

        public string state { get; set; }

        [DataMember]

        public string country { get; set; }
        [DataMember]

        public string phone1 { get; set; }
        [DataMember]

        public string phone2 { get; set; }
        [DataMember]

        public string email { get; set; }
        [DataMember]

        public string login { get; set; }
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

        public int i_time_zone { get; set; }
        public int i_ui_time_zone { get; set; }
        public string ui_time_zone_name { get; set; }

        public string tax_id { get; set; }

        public float creditiDisponible
        {
            get
            {
                var Total = (float)_Global.ReglaDeTres((float)GetComisionTopUp(), 0, fondosDisponibles);
               return (float)_Global.ReglaDeTres(ComisionDelPadre, Total, 0);

            }
        }

        public float ComisionDelPadre { get; set; }

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
        public int i_account_balance_control_type { get; set; }
        [DataMember]


        public string blocked { get; set; }

        [DataMember]
        public float MontoTransferenciaBancaria { get; set; }

        [DataMember]
        public int transaction_id { get; set; }
        [DataMember]
        public float sale_commission_rate { get; set; }
        [DataMember]
        public string note { get; set; }
        //[DataMember]
        //public float fondosDisponibles { get { return this.credit_limit - balance; } }
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

        public ErrorHandling ErrorHandlingCrearCuenta = new ErrorHandling();


        public bool IsDISTRIB
        {
            get
            {
                var CurrentCustomer = this;

                if (CurrentCustomer.note != null && CurrentCustomer.note != "")
                {
                    var disArray = CurrentCustomer.note.Split(':');
                    if (disArray[1] == "DISTRIB")
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public Double GetComisionTopUp()
        {
            if (note != null)
            {
                var disArray = this.note.Split(':');
                var comision = disArray[4].ToString();
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

        public Double CostoVentaMovil()
        {
            if (note != null)
            {
                var disArray = this.note.Split(':');
                var comision = disArray[4].ToString();
                decimal digito = decimal.Parse(comision, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                return Convert.ToDouble(digito);
            }
            else
                return 0;

        }


        public async Task<ErrorHandling> Validar()
        {
            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { customer_info = this });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.validate_customer_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

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
                        if (ErrorHandling.faultcode == "Server.Customer.duplicate_login")
                        {

                            ErrorHandling.faul = true;
                            ErrorHandling.faultstring = "Ya existe una cuenta registrada con este numero de telefono,por favor pruebe con otro";
                        }

                        if (ErrorHandling.faultcode == "Client.Customer.too_short.customer_info.login")
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
                    var param = JsonConvert.SerializeObject(new { customer_info = this });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.add_customer + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
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
                        if (ErrorHandling.faultcode == "Server.Customer.duplicate_login" || ErrorHandling.faultcode == "Server.Customer.duplicate_id")
                        {

                            ErrorHandling.faul = true;
                            ErrorHandling.faultstring = "Ya existe una cuenta registrada con este número de teléfono,por favor pruebe con otro";
                        }

                        if (ErrorHandling.faultcode == "Client.Customer.too_short.customer_info.login")
                        {

                            ErrorHandling.faul = true;
                            ErrorHandling.faultstring = "El número de teléfono esta mal escrito";
                        }

                        if (ErrorHandling.faultstring == "Failed to add Customer: The service password should not be easy to guess.")
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

        //public async Task<bool> ActualizarAccount()
        //{

        //    using (HttpClient client = new HttpClient())
        //    {
        //        var URL = "";
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        try
        //        {
        //            var param = JsonConvert.SerializeObject(new { account_info = this });
        //            URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.update_account + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
        //            var response = await client.GetAsync(URL);
        //            var json = await response.Content.ReadAsStringAsync();
        //            var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
        //            if (ErrorHandling.faultstring is null)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }

        //}


        public async Task<bool> Actualizar()
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = "";
                    if (this.i_customer_type == 3)
                    {
                        var customer_json = JsonConvert.SerializeObject(this);
                        var distribuidor = JsonConvert.DeserializeObject<customer_infoSin_i_i_distributor>(customer_json);
                        param = JsonConvert.SerializeObject(new { customer_info = distribuidor });
                    }
                    else
                        param = JsonConvert.SerializeObject(new { customer_info = this });

                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.update_customer + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
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

        public async Task<customer_info> Reload()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { i_customer = this.i_customer });
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.get_customer_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        return JsonConvert.DeserializeObject<CustomerObject>(json).customer_info;
                    }
                    return new customer_info();
                }
                catch
                {
                    return new customer_info();
                }

            }

        }

        public async Task<customer_info> GetOnlineCustomerInfo()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    this.i_customer = 260271;
                    var param = JsonConvert.SerializeObject(this);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_account_list + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetStringAsync(URL);
                    return JsonConvert.DeserializeObject<CustomerObject>(response).customer_info;
                }
                catch (Exception ex)
                {

                }
                return this;
            }
        }

        public async Task<customer_info> GetCustomerByUser(string user)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { login = user });
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.get_customer_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        return JsonConvert.DeserializeObject<CustomerObject>(json).customer_info;
                    }
                    else
                        return new customer_info();
                }
                catch (Exception ex)
                {
                    return new customer_info();
                }

            }

        }

        public async Task<customer_info> GetCustomerInfo()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(this);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.get_customer_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        return JsonConvert.DeserializeObject<CustomerObject>(json).customer_info;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }

        }

        public async Task<GetRetailCustomerXDRListResponse> GetCustomerXDR(GetRetailCustomerXDRListRequest GetRetailCustomerXDRListRequest)
        {
            GetRetailCustomerXDRListRequest.i_customer = this.i_customer;
            GetRetailCustomerXDRListRequest.billing_model = 1;
            GetRetailCustomerXDRListRequest.i_service = 1;
            GetRetailCustomerXDRListRequest.get_total = int.MaxValue;
            GetRetailCustomerXDRListRequest.offset = 0;
            GetRetailCustomerXDRListRequest.limit = int.MaxValue;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(GetRetailCustomerXDRListRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.get_customer_xdrs + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GetRetailCustomerXDRListResponse>(Result);

                }
                catch (Exception ex)
                {
                    return new GetRetailCustomerXDRListResponse();
                }

            }
        }
        
        public async Task<MakeCustomerTransactionResponse> MakeTransaction_Manualcharge(double costo, string visible_comment)
        {

            var MakeCustomerTransactionRequest = new MakeCustomerTransactionRequest();
            MakeCustomerTransactionRequest.i_customer = this.i_customer;
            MakeCustomerTransactionRequest.amount = decimal.Round(Convert.ToDecimal(costo), 2);
            MakeCustomerTransactionRequest.action = "Manual charge";
            MakeCustomerTransactionRequest.visible_comment = visible_comment;
            var MakeCustomerTransactionResponse = new MakeCustomerTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(MakeCustomerTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetStringAsync(URL);
                    MakeCustomerTransactionResponse = JsonConvert.DeserializeObject<MakeCustomerTransactionResponse>(response);
                    return MakeCustomerTransactionResponse;
                }
                catch (Exception ex)
                {
                    return MakeCustomerTransactionResponse;
                }
            }

        }

        public async Task<customer_info> GetParent()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {

                    var param = JsonConvert.SerializeObject(new { i_customer = this.i_distributor });
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.get_customer_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetStringAsync(URL);
                    return JsonConvert.DeserializeObject<CustomerObject>(response).customer_info;
                }
                catch (Exception ex)
                {
                    return new customer_info();
                }
            }
        }

        public async Task<customer_info> GetAccountById(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {

                    var param = JsonConvert.SerializeObject(new { i_customer = id });
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.get_customer_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetStringAsync(URL);
                    return JsonConvert.DeserializeObject<CustomerObject>(response).customer_info;
                }
                catch (Exception ex)
                {
                    return new customer_info();
                }
            }
        }

        public async Task<List<customer_info>> GetAccountList(customer_info CurrentCustomer)
        {

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = new GetCustomerListRequest
                    {
                        offset = 0,
                        limit = 10000
                    }.AsJson();

                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.get_customer_list + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        var result = JsonConvert.DeserializeObject<GetCustomerListResponse>(json).customer_list;
                        return result.Where(x => x.i_distributor == CurrentCustomer.i_customer).ToList();
                    }
                    else
                        return new List<customer_info>();
                }
                catch (Exception ex)
                {
                    return new List<customer_info>();
                }

            }

        }

        //public async Task<List<customer_info>> GetCustomerList()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        try
        //        {
        //            var param = new GetAccountListRequest
        //            {
        //                offset = 0,
        //                limit = 100,
        //                i_customer = this.i_customer
        //            }.AsJson();
        //            var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_account_list + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

        //            var response = await client.GetAsync(URL);
        //            var json = await response.Content.ReadAsStringAsync();
        //            var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
        //            if (ErrorHandling.faultstring is null)
        //            {
        //                return JsonConvert.DeserializeObject<GetAccountListResponse>(json).account_list;
        //            }
        //            else
        //                return new List<customer_info>();
        //        }
        //        catch (Exception ex)
        //        {
        //            return new List<customer_info>();
        //        }

        //    }

        //}

        public async Task<string> CambiarPassword(string newPassword)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = new ChangeCustomerPasswordRequest
                    {
                        i_customer = this.i_customer,
                        old_password = this.password,
                        new_password = newPassword
                    }.AsJson();

                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Customer + "/" + _Global.Metodo.change_password + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    //if (ErrorHandling.faultstring is null)
                    //{
                    //    return JsonConvert.DeserializeObject<GetAccountListResponse>(json).account_list;
                    //}
                    //else
                    //    return new List<account_info>();
                    return "";
                }
                catch (Exception ex)
                {
                    return "";
                }

            }

        }

        public async Task<float> GetBalanceCustomer()
        {
            return this.balance;
        }

        [DataMember]
        public float fondosDisponibles { get { return credit_limit - balance; } }

        public async Task<float> GetFondosDisponibles()
        {
            //float fondos = 0;
            //float sumaCreditLimit = 0;
            //float sumaBalances = 0;
            //var listaCuentas = await this.GetAccountList();
            //if (listaCuentas.Any())
            //    sumaCreditLimit = listaCuentas.Where(x => x.login != null).Select(x => x.credit_limit).Sum();
            //    sumaBalances = listaCuentas.Where(x => x.login != null).Select(x => x.balance).Sum();
            //fondos = (this.credit_limit - balance) - sumaCreditLimit + sumaBalances;
            //return fondos;

            // return this.credit_limit - await this.GetBalanceCustomer();

            return credit_limit - balance;
        }

    }

    public class customer_infoSin_i_i_distributor
    {
        [DataMember]

        public string id { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public float impuesto { get; set; }
        [DataMember]
        public int i_customer { get; set; }
        [DataMember]
        public int i_customer_type { get; set; }

        [DataMember]

        public float balance { get; set; }
        [DataMember]

        public int billing_model { get; set; }
        [DataMember]

        public string activation_date { get; set; }
        [DataMember]

        public string bill_status { get; set; }

        [DataMember]
        public string name { get; set; }
        [DataMember]

        public string iso_4217 { get; set; }
        [DataMember]
        public int i_product { get; set; }

        [DataMember]

        public string batch_name { get; set; }
        [DataMember]

        public int control_number { get; set; }
        [DataMember]
        public float opening_balance { get; set; }
        [DataMember]

        public string cont1 { get; set; }
        [DataMember]

        public string h323_password { get; set; }
        [DataMember]

        public string companyname { get; set; }
        [DataMember]

        public string firstname { get; set; }
        [DataMember]

        public string lastname { get; set; }
        [DataMember]

        public string fullname => firstname + " " + lastname;
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

        public string city { get; set; }
        [DataMember]

        public string state { get; set; }

        [DataMember]

        public string country { get; set; }
        [DataMember]

        public string phone1 { get; set; }
        [DataMember]

        public string phone2 { get; set; }
        [DataMember]

        public string email { get; set; }
        [DataMember]

        public string login { get; set; }
        [DataMember]


        public string password { get; set; }


        [DataMember]

        public int i_time_zone { get; set; }
        public int i_ui_time_zone { get; set; }
        public string ui_time_zone_name { get; set; }

        public string tax_id { get; set; }
        [DataMember]

        public float credit_limit { get; set; }
        [DataMember]

        public string faxnum { get; set; }
        [DataMember]
        public string baddr2 { get; set; }


        [DataMember]
        public int i_account_balance_control_type { get; set; }
        [DataMember]


        public string blocked { get; set; }

        [DataMember]
        public float MontoTransferenciaBancaria { get; set; }

        [DataMember]
        public int transaction_id { get; set; }
        [DataMember]
        public float sale_commission_rate { get; set; }
        [DataMember]
        public string note { get; set; }



    }

    public class EnabledSmsDestinatario
    {

        [DataMember]
        public bool destinatario { get; set; }

    }


    public class EnabledPromocionesBySms
    {

        [DataMember]
        public bool promociones { get; set; }

    }

    public class ChangeCustomerPasswordRequest
    {
        [DataMember]
        public int i_customer { get; set; }
        [DataMember]
        public string new_password { get; set; }
        [DataMember]
        public string old_password { get; set; }
    }

    public class ChangePasswordResponse
    {
        [DataMember]
        public int success { get; set; }
        [DataMember]
        public ChangePasswordResponseErrMessages errors { get; set; }
    }

    public class ChangePasswordResponseErrMessages
    {
        [DataMember]
        public string new_password { get; set; }
        [DataMember]
        public string old_password { get; set; }
    }


    public class CustomerObject
    {
        [DataMember]
        public customer_info customer_info { get; set; }
    }

    public class UpdateCustomerPaymentMethodRequest
    {
        [DataMember]
        public int i_customer { get; set; }
        [DataMember]
        public payment_method_info payment_method_info { get; set; }
    }

    public class UpdateCustomerPaymentMethodResponse
    {
        [DataMember]
        public int i_credit_card { get; set; }
    }

    public class GetCustomerPaymentMethodInfo
    {
        [DataMember]
        public int i_customer { get; set; }
    }

    public class MakeCustomerTransactionRequest
    {
        [DataMember]
        public int i_customer { get; set; }
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public decimal amount { get; set; }
        [DataMember]
        public string visible_comment { get; set; }
        [DataMember]
        public string transaction_id { get; set; }

    }

    public class MakeCustomerTransactionResponse
    {

        [DataMember]
        public int i_payment_transaction { get; set; }
        [DataMember]
        public float balance { get; set; }
        [DataMember]
        public string transaction_id { get; set; }
        [DataMember]
        public string authorization { get; set; }
        [DataMember]
        public string result_code { get; set; }
        [DataMember]
        public int i_xdr { get; set; }

    }

    public class GetAccountListRequest
    {
        [DataMember]
        public int offset { get; set; }
        [DataMember]
        public float limit { get; set; }
        [DataMember]
        public int i_customer { get; set; }

    }

    public class GetAccountListResponse
    {
        [DataMember]
        public List<account_info> account_list { get; set; }

    }
    
    public class GetCustomerListRequest
    {
        [DataMember]
        public int offset { get; set; }
        [DataMember]
        public float limit { get; set; }
        [DataMember]
        public int i_customer { get; set; }

    }

    public class GetCustomerListResponse
    {
        [DataMember]
        public List<customer_info> customer_list { get; set; }

    }

    public class CustomerXDRInfo
    {

        public string xdr_type { get; set; }
        public string i_xdr { get; set; }
        public string account_id { get; set; }
        public string CLI { get; set; }
        public string CLD { get; set; }
        public float charged_amount { get; set; }
        public string i_service { get; set; }
        public string service { get; set; }
        public string charged_quantity { get; set; }
        public string country { get; set; }
        public string subdivision { get; set; }
        public string description { get; set; }
        public string disconnect_cause { get; set; }
        public string bill_status { get; set; }
        public string disconnect_reason { get; set; }
        public string connect_time { get; set; }
        public string unix_connect_time { get; set; }
        public string disconnect_time { get; set; }
        public string unix_disconnect_time { get; set; }
        public DateTime bill_time { get; set; }
        public string bit_flags { get; set; }
        public string call_recording_url { get; set; }
        public string call_recording_server_url { get; set; }


    }

    /// <summary>
    /// Crea una instancia de la clase Empleado
    /// </summary>
    /// <param name="i_customer">Mandatorio</param>
    /// <param name="from_date">Mandatorio</param>
    /// <param name="to_date">Mandatorio</param>
    public class GetRetailCustomerXDRListRequest
    {
        public int i_customer { get; set; }
        public int i_service { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string cdr_entity { get; set; }
        public int billing_model { get; set; }
        public int get_total { get; set; }
        public string format { get; set; }
        public int show_unsuccessful { get; set; }

    }

    public class GetRetailCustomerXDRListResponse
    {
        [DataMember]
        public CustomerXDRInfo[] xdr_list { get; set; }
        [DataMember]
        public int total { get; set; }
    }


}
