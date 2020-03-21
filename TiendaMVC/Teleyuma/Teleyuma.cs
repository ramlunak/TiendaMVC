using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TiendaMVC.Teleyuma
{
    public class Teleyuma
    {      
        public class TopUpRequest
        {
            public string AccountNumber { get; set; }
            public string SkuCode { get; set; }
            public float SendValue { get; set; }
            public string DistributorRef { get; set; }
            public bool ValidateOnly { get; set; }

        }

        public class TopUpResponse
        {
            public string AccountNumber { get; set; }
            public string TransferRef { get; set; }
            public string DistributorRef { get; set; }
            public float ReceiveValue { get; set; }
            public float SendValue { get; set; }
            public float ReceiveCurrencyIso { get; set; }
            public float SendCurrencyIso { get; set; }
            public float CommissionApplied { get; set; }
            public string StartedUtc { get; set; }
            public string CompletedUtc { get; set; }
            public string ProcessingState { get; set; }
            public string ResultCode { get; set; }
            public ErrorCode[] ErrorCodes { get; set; }

        }

        public class ErrorCode
        {
            [DataMember]
            public string Code { get; set; }
            [DataMember]
            public string Context { get; set; }

        }

    }
}