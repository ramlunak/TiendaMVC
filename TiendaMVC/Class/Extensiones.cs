using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace TiendaMVC.Class
{
    public enum FileExtention
    {
        txt,
        csv,
        undefinned
    }

    public static class Extensions
    {
        public static StringContent AsJsonStringContent(this object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        }


        private static bool IsNumeric(this string sValue)
        {
            return Regex.IsMatch(sValue, "^[0-9]+$");
        }


        public static bool DeleteFile(this string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        System.IO.File.Delete(path);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static FileExtention GetFileExtention(this string path)
        {
            if (path.Contains(".txt"))
                return FileExtention.txt;
            else
                if (path.Contains(".csv"))
                return FileExtention.csv;
            else
                return FileExtention.undefinned;
        }

        public static FileExtention GetFileExtention(this HttpPostedFileBase path)
        {
            if (path.ContentType == "text/plain")
                return FileExtention.txt;
            else
                if (path.ContentType == "application/vnd.ms-excel")
                return FileExtention.csv;
            else
                return FileExtention.undefinned;
        }



        public static List<string> GetNumberList(this HttpPostedFileBase file)
        {
            string contenido = new StreamReader(file.InputStream).ReadToEnd();
            string[] arraytxt = contenido.Split(',');
            string[] arraycsv = contenido.Split(';');
            var extencion = file.GetFileExtention();
            var texto = new List<string>();
            var list = new List<string>();
            if (extencion == FileExtention.txt)
            {
                texto = arraytxt.ToList();
            }
            else if (extencion == FileExtention.csv)
            {
                texto = arraycsv.ToList();
            }
            else if (extencion == FileExtention.undefinned)
            {
                return new List<string>();
            }

           
            foreach (var item in texto.ToList())
            {
                var split_item = item.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").ToString();
                if (split_item == "" || !split_item.IsNumeric() || split_item.Length < 6)
                {

                }
                else
                    list.Add(split_item);
            }
            return list;

            

            
        }


        public static List<string> GetNumberList(this string path)
        {
            var extencion = path.GetFileExtention();

            if (extencion == FileExtention.txt)
            {
                StreamReader objReader = new StreamReader(path);
                var texto = objReader.ReadToEnd();
                string[] array = texto.Split(',');

                var list = new List<string>();
                foreach (var item in array.ToList())
                {
                   var split_item = item.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").ToString();
                    if (split_item == "" || !split_item.IsNumeric() || split_item.Length < 6)
                    { }
                    else
                        list.Add(split_item);
                }
                return list;
            }
            else if (extencion == FileExtention.csv)
            {

                using (TextFieldParser csvParser = new TextFieldParser(path))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvParser.ReadLine();

                    var lista = new List<string>();


                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();

                        foreach (var item in fields)
                        {
                            string[] array = item.Split(';');
                            var lista2 = array.ToList();
                            var count = lista2.Count;
                            if (count > 1)
                            {
                                foreach (var item2 in lista2)
                                {
                                    if (item2.IsNumeric() && item2.Length > 6)
                                    {
                                        lista.Add(item2);
                                    }
                                }
                            }
                            if (item.IsNumeric() && item.Length > 6)
                            {
                                lista.Add(item);
                            }
                        }

                    }

                    return lista;
                }

            }

            return new List<string>();
        }

        public static string AsJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static int ToInt(this object o)
        {
            return Convert.ToInt32(o);
        }

        public static string To_0_00(this object o)
        {
            try
            {
                var dval = Convert.ToDecimal(o);
                return dval.ToString("F", CultureInfo.CreateSpecificCulture("en-US"));
            }
            catch
            {
                return o.ToString();
            }
        }

        public static string To_MM_DD_YYYY(this DateTime o)
        {
            try
            {

                return o.ToString("MM/dd/yyyy h:mm:ss tt", CultureInfo.CreateSpecificCulture("en-US"));
            }
            catch
            {
                return o.ToString();
            }
        }

        public static string To_MM_DD_YYYY(this string o)
        {
            try
            {
                var fecha = DateTime.Parse(o);
                return fecha.ToString("MM/dd/yyyy h:mm:ss tt", CultureInfo.CreateSpecificCulture("en-US"));
            }
            catch
            {
                return o.ToString();
            }
        }

        public static DateTime To_DateTime_MM_DD_YYYY(this string o)
        {
            try
            {

                return DateTime.ParseExact(o, "MM/dd/yyyy h:mm:ss tt",
                                    new CultureInfo("en-US"),
                                    DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime To_FirstDay(this DateTime o)
        {
            try
            {
                var ahora = o;
                var diaSemana = ahora.DayOfWeek;
                var diaToDouble = Convert.ToDouble(diaSemana);

                var lunes = new DateTime();
                if (diaToDouble == 0)
                {
                    lunes = ahora.AddDays(-6);
                }
                else
                if (diaToDouble > 1)
                {
                    lunes = ahora.AddDays(1 - diaToDouble);
                }
                return lunes;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string GetDateFormat_YYMMDD(this DateTime DateTime, string Hora = "inicio")
        {
            var YY = DateTime.Year.ToString();
            var MM = DateTime.Month.ToString();
            var DD = DateTime.Day.ToString();

            if (MM.Length == 1)
                MM = "0" + MM;
            if (DD.Length == 1)
                DD = "0" + DD;

            if (Hora == "inicio")
                return YY + "-" + MM + "-" + DD + " 00:00:00";
            else
                return YY + "-" + MM + "-" + DD + " 23:59:59";
        }

        public static bool XdrIsMovil(this CustomerXDRInfo xdr)
        {
            try
            {
                var array = xdr.CLD.Split('-');
                if (array.Length == 0)
                    return false;
                else
                {
                    var tipo = array[0].Replace(" ", "");
                    if (tipo == "M" || tipo == "Movil")
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool XdrIsNauta(this CustomerXDRInfo xdr)
        {
            try
            {
                var array = xdr.CLD.Split('-');
                if (array.Length == 0)
                    return false;
                else
                {
                    var tipo = array[0].Replace(" ", "");
                    if (tipo == "Nauta")
                        return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public static int XdrGetMonto(this CustomerXDRInfo xdr)
        {
            try
            {
                var array = xdr.CLD.Split('-');
                if (array.Length == 0)
                    return 0;
                else
                {
                    var monto = array[1].Replace(" ", "");
                    return monto.ToInt();
                }
            }
            catch (Exception)
            {
                return 0;
            }


        }

        public static float XdrGetCosto(this CustomerXDRInfo xdr)
        {
            try
            {
                return xdr.charged_amount;
            }
            catch (Exception)
            {
                return 0;
            }


        }

        public static string XdrGetNumero(this CustomerXDRInfo xdr)
        {
            try
            {
                var array = xdr.CLD.Split('-');
                if (array.Length == 0)
                    return "";
                else
                {
                    var numero = array[2].Replace(" ", "");
                    return numero;
                }
            }
            catch (Exception)
            {
                return "";
            }


        }

        public static bool XdrIsCustomer(this CustomerXDRInfo xdr)
        {
            if (xdr.xdr_type == "customer")
                return true;
            else return false;

        }

        public static bool XdrIsAccount(this CustomerXDRInfo xdr)
        {
            try
            {
                var array = xdr.CLD.Split('-');
                if (array.Length == 0)
                    return false;
                else
                {
                    var id = array[3].Replace(" ", "");
                    if (string.IsNullOrEmpty(id))
                        return false;
                    else return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static int XdrIdAsociado(this CustomerXDRInfo xdr)
        {
            try
            {
                var array = xdr.CLD.Split('-');
                if (array.Length == 0)
                    return 0;
                else
                {
                    var id = array[3].Replace(" ", "");
                    return id.ToInt();
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static string XdrGetNombre(this CustomerXDRInfo xdr)
        {
            try
            {
                var array = xdr.CLD.Split('-');
                if (array.Length == 0)
                    return "";
                else
                {
                   return array[4];
                 }
            }
            catch (Exception)
            {
                return "";
            }

        }

        public static float XdrGetCustomerComision(this CustomerXDRInfo xdr, float descuentoCustomer)
        {
            //if (xdr.XdrIsCustomer())
            //    return 0;
            //else
            //{
                var montoRecarga = xdr.XdrGetMonto();
                var costoPorRecargaCustomer = montoRecarga * descuentoCustomer / 100;
                return montoRecarga - costoPorRecargaCustomer;
            //}

        }

    }

    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null, string ContentType = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.ContentType = ContentType;
            this.InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }

        public override string ContentType { get; }

        public override Stream InputStream { get; }
    }

}
