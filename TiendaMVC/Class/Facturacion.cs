using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaMVC.Class
{
    public class MontoDepositar
    {
        public int PorUstedCantiad { get; set; }
        public float PorUsted { get; set; }
        public int SusAsociadosCantidad { get; set; }
        public float SusAsociados { get; set; }
        public float Comisiones { get; set; }
        public float Total => this.PorUsted + this.SusAsociados - this.Comisiones;
    }

    public class ResumenRecargas
    {
        public int RecargasMoviles { get; set; }
        public float MontoRecargasMoviles { get; set; }
        public int RecargasNautas { get; set; }
        public float MontoRecargasNautas { get; set; }
        public float TotalRecargas => this.RecargasMoviles + this.RecargasNautas;
        public float MontoTotal => this.MontoRecargasMoviles + this.MontoRecargasNautas;
    }

    public class DetalleComisiones
    {
        public string IdCuenta { get; set; }
        public string Nombre { get; set; }
        public float Monto { get; set; }
        public float Comision { get; set; }
    }

    public class Facturacion
    {
        public int id { get; set; }
        public string IdCuenta { get; set; }
        public float DetalleComisionMontoTotal
        {
            get
            {
                return this.DetalleComisiones.Sum(x => x.Monto);
            }
        }
        public float DetalleComisionComisionesTotal
        {
            get
            {
                return this.DetalleComisiones.Sum(x => x.Comision);
            }
        }
        public MontoDepositar MontoDepositar { get; set; }
        public ResumenRecargas ResumenRecargas { get; set; }
        public List<DetalleComisiones> DetalleComisiones { get; set; }
        public List<FacturacionResumen> FacturacionResumen { get; set; }
        public GetRetailCustomerXDRListResponse XDRListResponse { get; set; }

        public async Task Cargar(customer_info CurrentCustomer)
        {
            var montoDepositar = new MontoDepositar();
            var resumenRecargas = new ResumenRecargas();

            float SuMonto = 0;
            float MontoAsociados = 0;
            float MontoComisiones = 0;

            //Detalle de comisiones
            var ArrayCustomerXDRInfo = new List<List<CustomerXDRInfo>>();

            var xdrs = this.XDRListResponse.xdr_list.ToList();

            //var xdrs = XDRS;
            foreach (var item in xdrs)
            {

                var list = new List<CustomerXDRInfo>();
                var sihay = false;

                if (item.XdrIsAccount())
                {
                    foreach (var listaCustomerXDRInfo in ArrayCustomerXDRInfo)
                    {
                        if (listaCustomerXDRInfo.Any())
                        {
                            if (listaCustomerXDRInfo.First().XdrIdAsociado() == item.XdrIdAsociado())
                            {
                                listaCustomerXDRInfo.Add(item);
                                sihay = true;
                            }
                        }
                    }

                    if (!sihay)
                    {
                        list.Add(item);
                        ArrayCustomerXDRInfo.Add(list);
                    }
                }

                //Recargas Moviles
                if (item.XdrIsMovil())
                {
                    //Monto a depositar
                    if (item.XdrIsAccount())
                    {
                        montoDepositar.SusAsociadosCantidad += 1;
                        montoDepositar.SusAsociados += item.charged_amount;
                        montoDepositar.Comisiones += item.XdrGetCustomerComision((float)CurrentCustomer.GetComisionTopUp());
                    }
                    else
                    if (item.XdrIsCustomer())
                    {
                        montoDepositar.PorUstedCantiad += 1;
                        montoDepositar.PorUsted += item.charged_amount;
                    }

                    //Resumen Recargas
                    resumenRecargas.RecargasMoviles += 1;
                    resumenRecargas.MontoRecargasMoviles += item.charged_amount;
                }

                //Recargas Nautas
                if (item.XdrIsNauta())
                {
                    if (item.XdrIsAccount())
                    {
                        montoDepositar.SusAsociadosCantidad += 1;
                        montoDepositar.SusAsociados += item.charged_amount;
                        montoDepositar.Comisiones += item.XdrGetCustomerComision((float)CurrentCustomer.GetComisionTopUp());
                    }
                    else
                    //Monto a depositar
                    if (item.XdrIsCustomer())
                    {
                        montoDepositar.PorUstedCantiad += 1;
                        montoDepositar.PorUsted += item.charged_amount;
                    }

                    resumenRecargas.RecargasNautas += 1;
                    resumenRecargas.MontoRecargasNautas += item.charged_amount;
                }
            }

            this.MontoDepositar = montoDepositar;
            this.ResumenRecargas = resumenRecargas;

            ;

            var detalleComisiones = new List<DetalleComisiones>();
                      
            foreach (var items in ArrayCustomerXDRInfo)
            {
                var detalleComis = new DetalleComisiones();
              
                    var accountInfo = await CurrentCustomer.GetAccountById(items.First().XdrIdAsociado().ToString());
                    if (accountInfo == null)
                    {
                        detalleComis.IdCuenta = items.First().XdrIdAsociado().ToString();
                        detalleComis.Nombre = "Cuenta Eliminada";
                    }
                    else
                    {
                        detalleComis.IdCuenta = accountInfo.i_customer.ToString();
                        detalleComis.Nombre = accountInfo.fullname;
                    }

                    foreach (var xdr in items)
                    {
                        detalleComis.Monto += xdr.charged_amount;
                        detalleComis.Comision += xdr.XdrGetCustomerComision((float)CurrentCustomer.GetComisionTopUp());
                    }
                    detalleComisiones.Add(detalleComis);
                CultureInfo culture = new CultureInfo("en-US");
                DateTime tempDate = Convert.ToDateTime("1/1/2010 12:10:15 PM", culture);
            }

            this.DetalleComisiones = detalleComisiones;
        }

        //public List<FacturacionResumen> facturacionResumen = new List<FacturacionResumen>();

        //public async Task CargarResumen(customer_info CurrentCustomer)
        //{

        //    var xdrs = this.XDRListResponse.xdr_list.ToList();
        //    //var xdrs = XDRS;
        //    foreach (var item in xdrs)
        //    {
        //        var resumenTienda = new FacturacionResumen();
        //        if (item.XdrIdAsociado() != 0)
        //        {
        //            if (this.facturacionResumen.Any())
        //            {

        //            }

        //            //Recargas Moviles
        //            if (item.XdrIsMovil())
        //            {
        //                resumenTienda.Telefono++;
        //            }

        //            //Recargas Nautas
        //            if (item.XdrIsNauta())
        //            {
        //                resumenTienda.Nauta++;
        //            }
        //        }
        //        else
        //        {

        //        }





        //        resumenTienda.Nombre = CurrentCustomer.fullname;
        //        resumenTienda.Rol = RolTienda.Gerente;
        //        resumenTienda.Recargas++;

        //        //Recargas Moviles
        //        if (item.XdrIsMovil())
        //        {
        //            resumenTienda.Telefono++;
        //        }

        //        //Recargas Nautas
        //        if (item.XdrIsNauta())
        //        {
        //            resumenTienda.Nauta++;
        //        }
        //    }

        //    //Detalle de comisiones
        //    var detalleComisiones = new List<DetalleComisiones>();

        //    var grupoCuentas = xdrs.GroupBy(x => x.account_id).ToList().Where(x => x.Key != null).ToList();
        //    foreach (var items in grupoCuentas)
        //    {
        //        var detalleComis = new DetalleComisiones();
        //        if (items.Key != null)
        //        {
        //            var accountInfo = await CurrentCustomer.GetAccountById(items.Key);
        //            if (accountInfo == null)
        //            {
        //                detalleComis.IdCuenta = items.Key.ToString();
        //                detalleComis.Nombre = "Cuenta Eliminada";
        //            }
        //            else
        //            {
        //                detalleComis.IdCuenta = accountInfo.i_customer.ToString();
        //                detalleComis.Nombre = accountInfo.fullname;
        //            }

        //            foreach (var xdr in items)
        //            {
        //                detalleComis.Monto += xdr.charged_amount;
        //                detalleComis.Comision += xdr.XdrGetCustomerComision((float)CurrentCustomer.GetComisionTopUp());
        //            }
        //            detalleComisiones.Add(detalleComis);
        //        }
        //    }
        //    this.DetalleComisiones = detalleComisiones;
        //}

    }

    public class FacturacionResumen
    {
        public RolTienda Rol { get; set; }
        public string IdTienda { get; set; }
        public string Nombre { get; set; }
        public int Recargas { get; set; }
        public int Telefono { get; set; }
        public int Nauta { get; set; }
        public float Monto { get; set; }
        public float Ganancia { get; set; }

    }

}