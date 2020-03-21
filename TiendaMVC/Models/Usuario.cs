using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaMVC.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public int IdCuenta { get; set; }
        public byte[] foto { get; set; }
    }
}