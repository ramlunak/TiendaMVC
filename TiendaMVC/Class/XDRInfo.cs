using System;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Reflection;
namespace TiendaMVC.Class
{
    public enum xdr_ype
    {
        [Description("sdfsd sdfs dfsdf ")]
        customer,
        account
    }

    public class XDRInfo
    {
        [DataMember]
        public string i_service { get; set; }
        [DataMember]
        public string subdivision { get; set; }
        [DataMember]
        public string disconnect_reason { get; set; }
        [DataMember]
        public string i_xdr { get; set; }
        [DataMember]
        public string CLD { get; set; }
        [DataMember]
        public string call_recording_server_url { get; set; }
        [DataMember]
        public string connect_time { get; set; }
        [DataMember]
        public string CLI { get; set; }
        [DataMember]
        public float charged_amount { get; set; }
        [DataMember]
        public decimal charged_amount2
        {
            get
            {
                var value = decimal.Round(Convert.ToDecimal(charged_amount), 2);
                if (value < 0)
                    return (value * -1);
                return value;
            }
        }
        [DataMember]
        public string bill_time { get; set; }
        [DataMember]
        public string xdr_type { get; set; }
        [DataMember]
        public string service { get; set; }
        [DataMember]
        public string bit_flags { get; set; }
        [DataMember]
        public string unix_connect_time { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string bill_status { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string account_id { get; set; }
        [DataMember]
        public string unix_disconnect_time { get; set; }
        [DataMember]
        public string disconnect_cause { get; set; }
        [DataMember]
        public string charged_quantity { get; set; }
        [DataMember]
        public string call_recording_url { get; set; }
        [DataMember]
        public string disconnect_time { get; set; }

    }

}