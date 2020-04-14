using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Razorpay.Api;

namespace SJService
{
    public class RazorServices
    {
        private static RazorpayClient client = null;
        public  RazorServices()
        {
            client = new RazorpayClient("rzp_live_NbVAB5UFQWq6FH", "sdSceEU92GFFzGqGpdscJ9dB");
        }
        public Dictionary<string, object> GetPaymentById(string RazorPayId)
        {
            Payment paa = client.Payment.Fetch(RazorPayId);
            return JObject.FromObject(paa).ToObject<Dictionary<string, object>>(); 
        } 
    }
}
