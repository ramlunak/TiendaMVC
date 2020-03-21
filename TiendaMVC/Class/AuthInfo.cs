using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TiendaMVC.Class
{
        
    public class AuthInfo
    {
        [DataMember]
        public string session_id { get; set; }
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string password { get; set; }
        
    }
}
