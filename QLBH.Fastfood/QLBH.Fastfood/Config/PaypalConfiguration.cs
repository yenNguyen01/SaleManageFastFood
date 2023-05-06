using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH.Fastfood.Config
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;

        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "ATdRYCzrkre-2AIIm_c5FKd4_9q38pYEmLpcZihEdzBE-OsuX0O3Y_hQZ8K9ViDF4UJ224x6p742o9Hm";
            clientSecret = "EMjE9DsXrA_vQeGsNa_GPfD3hwzMnoVtWOrommkKnl3iTgjoZskT4913lZJvyya8h4cIVI2-fNyC6kQd";
        }

        private static Dictionary<string, string> getconfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal  
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken  
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}