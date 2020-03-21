using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaMVC.Models
{
    public class Campaign
    {
        public int id { get; set; }
        public string accontId { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public string sms { get; set; }
        public int smsCountError { get; set; }
        public float cost { get; set; }
        public float costBySms { get; set; }
        public float balanceFinal { get; set; }
        public List<string> numbers { get; set; }
        public string path { get; set; }
        [NotMapped]
        public int countSMS
        {
            get
            {
                return Convert.ToInt32(sms.Length / 161) + 1;
            }
        }
    }
}