using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TiendaMVC.Models
{
    public class RecargaReservada
    {
        public int id { get; set; }
        public string IdCuenta { get; set; }
        public string TipoRecarga { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int CodigoPais { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Numero { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Monto { get; set; }
        public decimal Costo
        {
            get
            {
                decimal coste = 0;

                coste = Convert.ToDecimal(((Monto * Impuesto) / 100));

                return coste;
            }
        }
        [NotMapped]
        public string Asociado { get; set; }
        [NotMapped]
        public float CostoXdr { get; set; }
        public float Impuesto { get; set; }
        public string Fecha { get; set; }
        public string Estado { get; set; }
        public string remitente { get; set; }
        public string Error { get; set; }
        public string ColorError { get; set; }
        public float Importe { get { return ((Monto * Impuesto) / 100); } }
    }
}