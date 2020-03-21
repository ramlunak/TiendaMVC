using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TiendaMVC.Class
{
    public class EPais
    {
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string NombreMasPrefijo
        {
            get
            {
                return "(" + this.PrefijoTelefonico + ") " + this.Nombre;
            }
        }
        [DataMember]
        public string PrefijoTelefonico { get; set; }
        [DataMember]
        public string iso2 { get; set; }
        [DataMember]
        public string image { get { return iso2.ToLower(); } }
    }

    public class Pais
    {
        public static string GetPrefijo(string nombre)
        {
            return GetList().First(x => x.Nombre.ToLower() == nombre.ToLower()).PrefijoTelefonico.ToString();
        }

        public static List<EPais> GetList()
        {
            var list = new List<EPais>();
            var pais = new EPais();

            // llenar lista de paises con prefijo         
            pais.iso2 = "US"; pais.Nombre = "United States of America"; pais.PrefijoTelefonico = "1"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CU"; pais.Nombre = "Cuba"; pais.PrefijoTelefonico = "53"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AF"; pais.Nombre = "Afghanistan"; pais.PrefijoTelefonico = "93"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AL"; pais.Nombre = "Albania"; pais.PrefijoTelefonico = "355"; list.Add(pais); pais = new EPais();
            pais.iso2 = "DE"; pais.Nombre = "Germany"; pais.PrefijoTelefonico = "49"; list.Add(pais); pais = new EPais();
            pais.iso2 = "DZ"; pais.Nombre = "Algeria"; pais.PrefijoTelefonico = "213"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AD"; pais.Nombre = "Andorra"; pais.PrefijoTelefonico = "376"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AO"; pais.Nombre = "Angola"; pais.PrefijoTelefonico = "244"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AI"; pais.Nombre = "Anguilla"; pais.PrefijoTelefonico = "1264"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AQ"; pais.Nombre = "Antarctica"; pais.PrefijoTelefonico = "672"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AG"; pais.Nombre = "Antigua and Barbuda"; pais.PrefijoTelefonico = "1268"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AN"; pais.Nombre = "Netherlands Antilles"; pais.PrefijoTelefonico = "599"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SA"; pais.Nombre = "Saudi Arabia"; pais.PrefijoTelefonico = "966"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AR"; pais.Nombre = "Argentina"; pais.PrefijoTelefonico = "54"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AM"; pais.Nombre = "Armenia"; pais.PrefijoTelefonico = "374"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AW"; pais.Nombre = "Aruba"; pais.PrefijoTelefonico = "297"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AU"; pais.Nombre = "Australia"; pais.PrefijoTelefonico = "61"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AT"; pais.Nombre = "Austria"; pais.PrefijoTelefonico = "43"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AZ"; pais.Nombre = "Azerbaijan"; pais.PrefijoTelefonico = "994"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BE"; pais.Nombre = "Belgium"; pais.PrefijoTelefonico = "32"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BS"; pais.Nombre = "Bahamas"; pais.PrefijoTelefonico = "1242"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BH"; pais.Nombre = "Bahrain"; pais.PrefijoTelefonico = "973"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BD"; pais.Nombre = "Bangladesh"; pais.PrefijoTelefonico = "880"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BB"; pais.Nombre = "Barbados"; pais.PrefijoTelefonico = "1246"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BZ"; pais.Nombre = "Belize"; pais.PrefijoTelefonico = "501"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BJ"; pais.Nombre = "Benin"; pais.PrefijoTelefonico = "229"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BT"; pais.Nombre = "Bhutan"; pais.PrefijoTelefonico = "975"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BY"; pais.Nombre = "Belarus"; pais.PrefijoTelefonico = "375"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MM"; pais.Nombre = "Myanmar"; pais.PrefijoTelefonico = "95"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BO"; pais.Nombre = "Bolivia"; pais.PrefijoTelefonico = "591"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BA"; pais.Nombre = "Bosnia and Herzegovina"; pais.PrefijoTelefonico = "387"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BW"; pais.Nombre = "Botswana"; pais.PrefijoTelefonico = "267"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BR"; pais.Nombre = "Brazil"; pais.PrefijoTelefonico = "55"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BN"; pais.Nombre = "Brunei"; pais.PrefijoTelefonico = "673"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BG"; pais.Nombre = "Bulgaria"; pais.PrefijoTelefonico = "359"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BF"; pais.Nombre = "Burkina Faso"; pais.PrefijoTelefonico = "226"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BI"; pais.Nombre = "Burundi"; pais.PrefijoTelefonico = "257"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CV"; pais.Nombre = "Cape Verde"; pais.PrefijoTelefonico = "238"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KH"; pais.Nombre = "Cambodia"; pais.PrefijoTelefonico = "855"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CM"; pais.Nombre = "Cameroon"; pais.PrefijoTelefonico = "237"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CA"; pais.Nombre = "Canada"; pais.PrefijoTelefonico = "1"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TD"; pais.Nombre = "Chad"; pais.PrefijoTelefonico = "235"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CL"; pais.Nombre = "Chile"; pais.PrefijoTelefonico = "56"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CN"; pais.Nombre = "China"; pais.PrefijoTelefonico = "86"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CY"; pais.Nombre = "Cyprus"; pais.PrefijoTelefonico = "357"; list.Add(pais); pais = new EPais();
            pais.iso2 = "VA"; pais.Nombre = "Vatican City State"; pais.PrefijoTelefonico = "39"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CO"; pais.Nombre = "Colombia"; pais.PrefijoTelefonico = "57"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KM"; pais.Nombre = "Comoros"; pais.PrefijoTelefonico = "269"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CG"; pais.Nombre = "Congo"; pais.PrefijoTelefonico = "242"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CD"; pais.Nombre = "Congo"; pais.PrefijoTelefonico = "243"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KP"; pais.Nombre = "North Korea"; pais.PrefijoTelefonico = "850"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KR"; pais.Nombre = "South Korea"; pais.PrefijoTelefonico = "82"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CI"; pais.Nombre = "Ivory Coast"; pais.PrefijoTelefonico = "225"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CR"; pais.Nombre = "Costa Rica"; pais.PrefijoTelefonico = "506"; list.Add(pais); pais = new EPais();
            pais.iso2 = "HR"; pais.Nombre = "Croatia"; pais.PrefijoTelefonico = "385"; list.Add(pais); pais = new EPais();
            pais.iso2 = "DK"; pais.Nombre = "Denmark"; pais.PrefijoTelefonico = "45"; list.Add(pais); pais = new EPais();
            pais.iso2 = "DM"; pais.Nombre = "Dominica"; pais.PrefijoTelefonico = "1767"; list.Add(pais); pais = new EPais();
            pais.iso2 = "EC"; pais.Nombre = "Ecuador"; pais.PrefijoTelefonico = "593"; list.Add(pais); pais = new EPais();
            pais.iso2 = "EG"; pais.Nombre = "Egypt"; pais.PrefijoTelefonico = "20"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SV"; pais.Nombre = "El Salvador"; pais.PrefijoTelefonico = "503"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AE"; pais.Nombre = "United Arab Emirates"; pais.PrefijoTelefonico = "971"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ER"; pais.Nombre = "Eritrea"; pais.PrefijoTelefonico = "291"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SK"; pais.Nombre = "Slovakia"; pais.PrefijoTelefonico = "421"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SI"; pais.Nombre = "Slovenia"; pais.PrefijoTelefonico = "386"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ES"; pais.Nombre = "Spain"; pais.PrefijoTelefonico = "34"; list.Add(pais); pais = new EPais();
            pais.iso2 = "EE"; pais.Nombre = "Estonia"; pais.PrefijoTelefonico = "372"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ET"; pais.Nombre = "Ethiopia"; pais.PrefijoTelefonico = "251"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PH"; pais.Nombre = "Philippines"; pais.PrefijoTelefonico = "63"; list.Add(pais); pais = new EPais();
            pais.iso2 = "FI"; pais.Nombre = "Finland"; pais.PrefijoTelefonico = "358"; list.Add(pais); pais = new EPais();
            pais.iso2 = "FJ"; pais.Nombre = "Fiji"; pais.PrefijoTelefonico = "679"; list.Add(pais); pais = new EPais();
            pais.iso2 = "FR"; pais.Nombre = "France"; pais.PrefijoTelefonico = "33"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GA"; pais.Nombre = "Gabon"; pais.PrefijoTelefonico = "241"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GM"; pais.Nombre = "Gambia"; pais.PrefijoTelefonico = "220"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GE"; pais.Nombre = "Georgia"; pais.PrefijoTelefonico = "995"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GH"; pais.Nombre = "Ghana"; pais.PrefijoTelefonico = "233"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GI"; pais.Nombre = "Gibraltar"; pais.PrefijoTelefonico = "350"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GD"; pais.Nombre = "Grenada"; pais.PrefijoTelefonico = "1473"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GR"; pais.Nombre = "Greece"; pais.PrefijoTelefonico = "30"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GL"; pais.Nombre = "Greenland"; pais.PrefijoTelefonico = "299"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GP"; //pais.Nombre = "Guadeloupe"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "GU"; pais.Nombre = "Guam"; pais.PrefijoTelefonico = "1671"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GT"; pais.Nombre = "Guatemala"; pais.PrefijoTelefonico = "502"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GF"; //pais.Nombre = "French Guiana"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "GG"; //.Nombre = "Guernsey"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "GN"; pais.Nombre = "Guinea"; pais.PrefijoTelefonico = "224"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GQ"; pais.Nombre = "Equatorial Guinea"; pais.PrefijoTelefonico = "240"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GW"; pais.Nombre = "Guinea-Bissau"; pais.PrefijoTelefonico = "245"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GY"; pais.Nombre = "Guyana"; pais.PrefijoTelefonico = "592"; list.Add(pais); pais = new EPais();
            pais.iso2 = "HT"; pais.Nombre = "Haiti"; pais.PrefijoTelefonico = "509"; list.Add(pais); pais = new EPais();
            pais.iso2 = "HN"; pais.Nombre = "Honduras"; pais.PrefijoTelefonico = "504"; list.Add(pais); pais = new EPais();
            pais.iso2 = "HK"; pais.Nombre = "Hong Kong"; pais.PrefijoTelefonico = "852"; list.Add(pais); pais = new EPais();
            pais.iso2 = "HU"; pais.Nombre = "Hungary"; pais.PrefijoTelefonico = "36"; list.Add(pais); pais = new EPais();
            pais.iso2 = "IN"; pais.Nombre = "India"; pais.PrefijoTelefonico = "91"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ID"; pais.Nombre = "Indonesia"; pais.PrefijoTelefonico = "62"; list.Add(pais); pais = new EPais();
            pais.iso2 = "IR"; pais.Nombre = "Iran"; pais.PrefijoTelefonico = "98"; list.Add(pais); pais = new EPais();
            pais.iso2 = "IQ"; pais.Nombre = "Iraq"; pais.PrefijoTelefonico = "964"; list.Add(pais); pais = new EPais();
            pais.iso2 = "IE"; pais.Nombre = "Ireland"; pais.PrefijoTelefonico = "353"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BV"; //pais.Nombre = "Bouvet Island"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "IM"; pais.Nombre = "Isle of Man"; pais.PrefijoTelefonico = "44"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CX"; pais.Nombre = "Christmas Island"; pais.PrefijoTelefonico = "61"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NF"; //pais.Nombre = "Norfolk Island"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "IS"; pais.Nombre = "Iceland"; pais.PrefijoTelefonico = "354"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BM"; pais.Nombre = "Bermuda Islands"; pais.PrefijoTelefonico = "1441"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KY"; pais.Nombre = "Cayman Islands"; pais.PrefijoTelefonico = "1345"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CC"; pais.Nombre = "Cocos(Keeling) Islands"; pais.PrefijoTelefonico = "61"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CK"; pais.Nombre = "Cook Islands"; pais.PrefijoTelefonico = "682"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AX"; //pais.Nombre = "Åland Islands"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "FO"; pais.Nombre = "Faroe Islands"; pais.PrefijoTelefonico = "298"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GS"; pais.Nombre = "South Georgia and the South Sandwich Islands"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "HM"; //pais.Nombre = "Heard Island and McDonald Islands"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "MV"; pais.Nombre = "Maldives"; pais.PrefijoTelefonico = "960"; list.Add(pais); pais = new EPais();
            pais.iso2 = "FK"; pais.Nombre = "Falkland Islands(Malvinas)"; pais.PrefijoTelefonico = "500"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MP"; pais.Nombre = "Northern Mariana Islands"; pais.PrefijoTelefonico = "1670"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MH"; pais.Nombre = "Marshall Islands"; pais.PrefijoTelefonico = "692"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PN"; pais.Nombre = "Pitcairn Islands"; pais.PrefijoTelefonico = "870"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SB"; pais.Nombre = "Solomon Islands"; pais.PrefijoTelefonico = "677"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TC"; pais.Nombre = "Turks and Caicos Islands"; pais.PrefijoTelefonico = "1649"; list.Add(pais); pais = new EPais();
            pais.iso2 = "UM"; //pais.Nombre = "United States Minor Outlying Islands"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "VG"; pais.Nombre = "Virgin Islands"; pais.PrefijoTelefonico = "1284"; list.Add(pais); pais = new EPais();
            pais.iso2 = "VI"; pais.Nombre = "United States Virgin Islands"; pais.PrefijoTelefonico = "1340"; list.Add(pais); pais = new EPais();
            pais.iso2 = "IL"; pais.Nombre = "Israel"; pais.PrefijoTelefonico = "972"; list.Add(pais); pais = new EPais();
            pais.iso2 = "IT"; pais.Nombre = "Italy"; pais.PrefijoTelefonico = "39"; list.Add(pais); pais = new EPais();
            pais.iso2 = "JM"; pais.Nombre = "Jamaica"; pais.PrefijoTelefonico = "1876"; list.Add(pais); pais = new EPais();
            pais.iso2 = "JP"; pais.Nombre = "Japan"; pais.PrefijoTelefonico = "81"; list.Add(pais); pais = new EPais();
            pais.iso2 = "JE"; //pais.Nombre = "Jersey"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "JO"; pais.Nombre = "Jordan"; pais.PrefijoTelefonico = "962"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KZ"; pais.Nombre = "Kazakhstan"; pais.PrefijoTelefonico = "7"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KE"; pais.Nombre = "Kenya"; pais.PrefijoTelefonico = "254"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KG"; pais.Nombre = "Kyrgyzstan"; pais.PrefijoTelefonico = "996"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KI"; pais.Nombre = "Kiribati"; pais.PrefijoTelefonico = "686"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KW"; pais.Nombre = "Kuwait"; pais.PrefijoTelefonico = "965"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LB"; pais.Nombre = "Lebanon"; pais.PrefijoTelefonico = "961"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LA"; pais.Nombre = "Laos"; pais.PrefijoTelefonico = "856"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LS"; pais.Nombre = "Lesotho"; pais.PrefijoTelefonico = "266"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LV"; pais.Nombre = "Latvia"; pais.PrefijoTelefonico = "371"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LR"; pais.Nombre = "Liberia"; pais.PrefijoTelefonico = "231"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LY"; pais.Nombre = "Libya"; pais.PrefijoTelefonico = "218"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LI"; pais.Nombre = "Liechtenstein"; pais.PrefijoTelefonico = "423"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LT"; pais.Nombre = "Lithuania"; pais.PrefijoTelefonico = "370"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LU"; pais.Nombre = "Luxembourg"; pais.PrefijoTelefonico = "352"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MX"; pais.Nombre = "Mexico"; pais.PrefijoTelefonico = "52"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MC"; pais.Nombre = "Monaco"; pais.PrefijoTelefonico = "377"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MO"; pais.Nombre = "Macao"; pais.PrefijoTelefonico = "853"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MK"; pais.Nombre = "Macedonia"; pais.PrefijoTelefonico = "389"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MG"; pais.Nombre = "Madagascar"; pais.PrefijoTelefonico = "261"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MY"; pais.Nombre = "Malaysia"; pais.PrefijoTelefonico = "60"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MW"; pais.Nombre = "Malawi"; pais.PrefijoTelefonico = "265"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ML"; pais.Nombre = "Mali"; pais.PrefijoTelefonico = "223"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MT"; pais.Nombre = "Malta"; pais.PrefijoTelefonico = "356"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MA"; pais.Nombre = "Morocco"; pais.PrefijoTelefonico = "212"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MQ"; pais.Nombre = "Martinique"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "MU"; pais.Nombre = "Mauritius"; pais.PrefijoTelefonico = "230"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MR"; pais.Nombre = "Mauritania"; pais.PrefijoTelefonico = "222"; list.Add(pais); pais = new EPais();
            pais.iso2 = "YT"; pais.Nombre = "Mayotte"; pais.PrefijoTelefonico = "262"; list.Add(pais); pais = new EPais();
            pais.iso2 = "FM"; pais.Nombre = "Estados Federados de"; pais.PrefijoTelefonico = "691"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MD"; pais.Nombre = "Moldova"; pais.PrefijoTelefonico = "373"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MN"; pais.Nombre = "Mongolia"; pais.PrefijoTelefonico = "976"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ME"; pais.Nombre = "Montenegro"; pais.PrefijoTelefonico = "382"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MS"; pais.Nombre = "Montserrat"; pais.PrefijoTelefonico = "1664"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MZ"; pais.Nombre = "Mozambique"; pais.PrefijoTelefonico = "258"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NA"; pais.Nombre = "Namibia"; pais.PrefijoTelefonico = "264"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NR"; pais.Nombre = "Nauru"; pais.PrefijoTelefonico = "674"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NP"; pais.Nombre = "Nepal"; pais.PrefijoTelefonico = "977"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NI"; pais.Nombre = "Nicaragua"; pais.PrefijoTelefonico = "505"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NE"; pais.Nombre = "Niger"; pais.PrefijoTelefonico = "227"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NG"; pais.Nombre = "Nigeria"; pais.PrefijoTelefonico = "234"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NU"; pais.Nombre = "Niue"; pais.PrefijoTelefonico = "683"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NO"; pais.Nombre = "Norway"; pais.PrefijoTelefonico = "47"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NC"; pais.Nombre = "New Caledonia"; pais.PrefijoTelefonico = "687"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NZ"; pais.Nombre = "New Zealand"; pais.PrefijoTelefonico = "64"; list.Add(pais); pais = new EPais();
            pais.iso2 = "OM"; pais.Nombre = "Oman"; pais.PrefijoTelefonico = "968"; list.Add(pais); pais = new EPais();
            pais.iso2 = "NL"; pais.Nombre = "Netherlands"; pais.PrefijoTelefonico = "31"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PK"; pais.Nombre = "Pakistan"; pais.PrefijoTelefonico = "92"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PW"; pais.Nombre = "Palau"; pais.PrefijoTelefonico = "680"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PS"; //pais.Nombre = "Palestine"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "PA"; pais.Nombre = "Panama"; pais.PrefijoTelefonico = "507"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PG"; pais.Nombre = "Papua New Guinea"; pais.PrefijoTelefonico = "675"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PY"; pais.Nombre = "Paraguay"; pais.PrefijoTelefonico = "595"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PE"; pais.Nombre = "Peru"; pais.PrefijoTelefonico = "51"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PF"; //pais.Nombre = "French Polynesia"; pais.PrefijoTelefonico = "689"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PL"; pais.Nombre = "Poland"; pais.PrefijoTelefonico = "48"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PT"; pais.Nombre = "Portugal"; pais.PrefijoTelefonico = "351"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PR"; pais.Nombre = "Puerto Rico"; pais.PrefijoTelefonico = "1"; list.Add(pais); pais = new EPais();
            pais.iso2 = "QA"; pais.Nombre = "Qatar"; pais.PrefijoTelefonico = "974"; list.Add(pais); pais = new EPais();
            pais.iso2 = "GB"; pais.Nombre = "United Kingdom"; pais.PrefijoTelefonico = "44"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CF"; pais.Nombre = "Central African Republic"; pais.PrefijoTelefonico = "236"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CZ"; pais.Nombre = "Czech Republic"; pais.PrefijoTelefonico = "420"; list.Add(pais); pais = new EPais();
            pais.iso2 = "_DO"; pais.Nombre = "Dominican Republic"; pais.PrefijoTelefonico = "1809"; list.Add(pais); pais = new EPais();
            pais.iso2 = "RE"; //pais.Nombre = "Réunion"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "RW"; pais.Nombre = "Rwanda"; pais.PrefijoTelefonico = "250"; list.Add(pais); pais = new EPais();
            pais.iso2 = "RO"; pais.Nombre = "Romania"; pais.PrefijoTelefonico = "40"; list.Add(pais); pais = new EPais();
            pais.iso2 = "RU"; pais.Nombre = "Russia"; pais.PrefijoTelefonico = "7"; list.Add(pais); pais = new EPais();
            pais.iso2 = "EH"; //pais.Nombre = "Western Sahara"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "WS"; pais.Nombre = "Samoa"; pais.PrefijoTelefonico = "685"; list.Add(pais); pais = new EPais();
            pais.iso2 = "AS"; pais.Nombre = "American Samoa"; pais.PrefijoTelefonico = "1684"; list.Add(pais); pais = new EPais();
            pais.iso2 = "BL"; pais.Nombre = "Saint Barthélemy"; pais.PrefijoTelefonico = "590"; list.Add(pais); pais = new EPais();
            pais.iso2 = "KN"; pais.Nombre = "Saint Kitts and Nevis"; pais.PrefijoTelefonico = "1869"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SM"; pais.Nombre = "San Marino"; pais.PrefijoTelefonico = "378"; list.Add(pais); pais = new EPais();
            pais.iso2 = "MF"; pais.Nombre = "Saint Martin(French part)"; pais.PrefijoTelefonico = "1599"; list.Add(pais); pais = new EPais();
            pais.iso2 = "PM"; pais.Nombre = "Saint Pierre and Miquelon"; pais.PrefijoTelefonico = "508"; list.Add(pais); pais = new EPais();
            pais.iso2 = "VC"; pais.Nombre = "Saint Vincent and the Grenadines"; pais.PrefijoTelefonico = "1784"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SH"; pais.Nombre = "Ascensión y Tristán de Acuña"; pais.PrefijoTelefonico = "290"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LC"; pais.Nombre = "Saint Lucia"; pais.PrefijoTelefonico = "1758"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ST"; pais.Nombre = "Sao Tome and Principe"; pais.PrefijoTelefonico = "239"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SN"; pais.Nombre = "Senegal"; pais.PrefijoTelefonico = "221"; list.Add(pais); pais = new EPais();
            pais.iso2 = "RS"; pais.Nombre = "Serbia"; pais.PrefijoTelefonico = "381"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SC"; pais.Nombre = "Seychelles"; pais.PrefijoTelefonico = "248"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SL"; pais.Nombre = "Sierra Leone"; pais.PrefijoTelefonico = "232"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SG"; pais.Nombre = "Singapore"; pais.PrefijoTelefonico = "65"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SY"; pais.Nombre = "Syria"; pais.PrefijoTelefonico = "963"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SO"; pais.Nombre = "Somalia"; pais.PrefijoTelefonico = "252"; list.Add(pais); pais = new EPais();
            pais.iso2 = "LK"; pais.Nombre = "Sri Lanka"; pais.PrefijoTelefonico = "94"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ZA"; pais.Nombre = "South Africa"; pais.PrefijoTelefonico = "27"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SD"; pais.Nombre = "Sudan"; pais.PrefijoTelefonico = "249"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SE"; pais.Nombre = "Sweden"; pais.PrefijoTelefonico = "46"; list.Add(pais); pais = new EPais();
            pais.iso2 = "CH"; pais.Nombre = "Switzerland"; pais.PrefijoTelefonico = "41"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SR"; pais.Nombre = "Suriname"; pais.PrefijoTelefonico = "597"; list.Add(pais); pais = new EPais();
            pais.iso2 = "SJ"; //pais.Nombre = "Svalbard and Jan Mayen"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "SZ"; pais.Nombre = "Swaziland"; pais.PrefijoTelefonico = "268"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TJ"; pais.Nombre = "Tajikistan"; pais.PrefijoTelefonico = "992"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TH"; pais.Nombre = "Thailand"; pais.PrefijoTelefonico = "66"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TW"; pais.Nombre = "Taiwan"; pais.PrefijoTelefonico = "886"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TZ"; pais.Nombre = "Tanzania"; pais.PrefijoTelefonico = "255"; list.Add(pais); pais = new EPais();
            pais.iso2 = "IO"; //pais.Nombre = "British Indian Ocean Territory"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "TF"; //pais.Nombre = "French Southern Territories"; pais.PrefijoTelefonico = ""; list.Add(pais); pais = new EPais();
            pais.iso2 = "TL"; pais.Nombre = "East Timor"; pais.PrefijoTelefonico = "670"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TG"; pais.Nombre = "Togo"; pais.PrefijoTelefonico = "228"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TK"; pais.Nombre = "Tokelau"; pais.PrefijoTelefonico = "690"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TO"; pais.Nombre = "Tonga"; pais.PrefijoTelefonico = "676"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TT"; pais.Nombre = "Trinidad and Tobago"; pais.PrefijoTelefonico = "1868"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TN"; pais.Nombre = "Tunisia"; pais.PrefijoTelefonico = "216"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TM"; pais.Nombre = "Turkmenistan"; pais.PrefijoTelefonico = "993"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TR"; pais.Nombre = "Turkey"; pais.PrefijoTelefonico = "90"; list.Add(pais); pais = new EPais();
            pais.iso2 = "TV"; pais.Nombre = "Tuvalu"; pais.PrefijoTelefonico = "688"; list.Add(pais); pais = new EPais();
            pais.iso2 = "UA"; pais.Nombre = "Ukraine"; pais.PrefijoTelefonico = "380"; list.Add(pais); pais = new EPais();
            pais.iso2 = "UG"; pais.Nombre = "Uganda"; pais.PrefijoTelefonico = "256"; list.Add(pais); pais = new EPais();
            pais.iso2 = "UY"; pais.Nombre = "Uruguay"; pais.PrefijoTelefonico = "598"; list.Add(pais); pais = new EPais();
            pais.iso2 = "UZ"; pais.Nombre = "Uzbekistan"; pais.PrefijoTelefonico = "998"; list.Add(pais); pais = new EPais();
            pais.iso2 = "VU"; pais.Nombre = "Vanuatu"; pais.PrefijoTelefonico = "678"; list.Add(pais); pais = new EPais();
            pais.iso2 = "VE"; pais.Nombre = "Venezuela"; pais.PrefijoTelefonico = "58"; list.Add(pais); pais = new EPais();
            pais.iso2 = "VN"; pais.Nombre = "Vietnam"; pais.PrefijoTelefonico = "84"; list.Add(pais); pais = new EPais();
            pais.iso2 = "WF"; pais.Nombre = "Wallis and Futuna"; pais.PrefijoTelefonico = "681"; list.Add(pais); pais = new EPais();
            pais.iso2 = "YE"; pais.Nombre = "Yemen"; pais.PrefijoTelefonico = "967"; list.Add(pais); pais = new EPais();
            pais.iso2 = "DJ"; pais.Nombre = "Djibouti"; pais.PrefijoTelefonico = "253"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ZM"; pais.Nombre = "Zambia"; pais.PrefijoTelefonico = "260"; list.Add(pais); pais = new EPais();
            pais.iso2 = "ZW"; pais.Nombre = "Zimbabwe"; pais.PrefijoTelefonico = "263"; list.Add(pais);

            //--------------
            return list;
        }



    }

}
