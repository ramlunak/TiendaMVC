using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaMVC.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Este campo es obligatorio")]
        public string user { get; set; }
        [Required(ErrorMessage ="Este campo es obligatorio")]            
        public string pass { get; set; }
        public bool error { get; set; }
        public string errorMensaje { get; set; }

    }
}