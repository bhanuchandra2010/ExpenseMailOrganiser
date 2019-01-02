using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseMonitor
{
    class ApplicationContext
    {
        static ApplicationContext()
        {
            string authority = $"https://login.microsoftonline.com/{Tenant}";
            _clientApp = new PublicClientApplication(ClientId, authority, TokenCacheHelper.GetUserCache());
        }

        // Below are the clientId (Application Id) of your app registration and the tenant information. 
        // You have to replace:
        // - the content of ClientID with the Application Id for your app registration
        // - Te content of Tenant by the information about the accounts allowed to sign-in in your application:
        //   - For Work or School account in your org, use your tenant ID, or domain
        //   - for any Work or School accounts, use organizations
        //   - for any Work or School acounts, or Microsoft personal account, use common
        //   - for Microsoft Personal account, use consumers
        private static string ClientId = "99e81344-e516-43a8-af5d-bd7ddc4bfd22";
        private static string Tenant = "common";

        private static PublicClientApplication _clientApp;

        public static PublicClientApplication PublicClientApp { get { return _clientApp; } }

    }
}
