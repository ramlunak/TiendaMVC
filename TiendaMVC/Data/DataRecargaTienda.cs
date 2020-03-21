using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiendaMVC.Models;

namespace TiendaMVC.Data
{
    public class DataRecargaTienda
    {
        public List<RecargaTienda> ListarRecargas()
        {
            using (var db = new TiendaMVCContext())
            {
                return db.RecargaTienda.ToList();
            }
        }

        internal void Create(RecargaTienda model)
        {
            using (var db = new TiendaMVCContext())
            {
               db.RecargaTienda.Add(model);
                db.SaveChanges();
            }
        }
    }
}