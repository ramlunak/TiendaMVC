#pragma warning disable CS0246 // El nombre del tipo o del espacio de nombres 'Java' no se encontró (¿falta una directiva using o una referencia de ensamblado?)

#pragma warning restore CS0246 // El nombre del tipo o del espacio de nombres 'Java' no se encontró (¿falta una directiva using o una referencia de ensamblado?)
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace TiendaMVC.Class
{
    [DataContract]
    [Serializable]
    public class payment_method_info
    {
        [DataMember]
        public string payment_method { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string cvv { get; set; }
        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string exp_date { get; set; }
        [DataMember]
        public string issue_no { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string iso_3166_1_a2 { get; set; }
        [DataMember]
        public int i_country_subdivision { get; set; }


    }

    public class PaymentMethodObject 
    {
        [DataMember]
        public payment_method_info payment_method_info { get; set; }
    }
       
    public class UpdateAccountPaymentMethodRequest
    {
        [DataMember]
        public int i_account { get; set; }
        [DataMember]
        public payment_method_info payment_method_info { get; set; }
    }

    public class UpdateAccountPaymentMethodResponse
    {
        [DataMember]
        public int i_credit_card { get; set; }
    }

    public class GetAccountPaymentMethodInfo
    {
        [DataMember]
        public int i_account { get; set; }
    }
      

}