using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace TiendaMVC.Class
{
    public class ErrorHandling
    {
        [DataMember]
        [DefaultValue(false)]
        public bool faul { get; set; }
        [DataMember]
        public string faultcode { get; set; }
        [DataMember]
        public string faultstring
        {
            get
           ;
            set;
        }
    }
}
