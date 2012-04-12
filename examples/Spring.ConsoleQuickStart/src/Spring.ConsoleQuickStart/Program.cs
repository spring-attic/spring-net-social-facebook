using System;
using System.Diagnostics;

using Spring.Social.OAuth2;
using Spring.Social.Facebook.Api;
using Spring.Social.Facebook.Connect;

namespace Spring.ConsoleQuickStart
{
    class Program
    {
        // Register your own Facebook app at https://developers.facebook.com/apps.
        // Set your application id & secret here
        private const string FacebookApiId = TODO;
        private const string FacebookApiSecret = TODO;

        static void Main(string[] args)
        {
            try
            {
                FacebookServiceProvider facebookServiceProvider = new FacebookServiceProvider(FacebookApiId, FacebookApiSecret);

                /* OAuth 'dance' */

                // Authentication using the client-side authorization flow 
                OAuth2Parameters parameters = new OAuth2Parameters()
                {
                    RedirectUrl = "https://www.facebook.com/connect/login_success.html",
                    Scope = "offline_access,publish_stream"
                };
                string authorizationUrl = facebookServiceProvider.OAuthOperations.BuildAuthorizeUrl(GrantType.ImplicitGrant, parameters);
                Console.WriteLine("Redirect user to Facebook for authorization: " + authorizationUrl);
                Process.Start(authorizationUrl);
                Console.WriteLine("Enter 'access_token' query string parameter from success url:");
                string accessToken = Console.ReadLine();

                /* API */

                IFacebook facebook = facebookServiceProvider.GetApi(accessToken);

            }
#if NET_4_0
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                    {
                        if (ex is Spring.Rest.Client.HttpResponseException)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(((Spring.Rest.Client.HttpResponseException)ex).GetResponseBodyAsString());
                            return true;
                        }
                        return false;
                    });
            }
#else
            catch (Spring.Rest.Client.HttpResponseException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.GetResponseBodyAsString());
            }
#endif
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("--- hit <return> to quit ---");
                Console.ReadLine();
            }
        }
    }
}
