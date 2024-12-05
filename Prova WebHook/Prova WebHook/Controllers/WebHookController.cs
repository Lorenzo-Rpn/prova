using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.AndroidPublisher.v3.Data;
using static System.Net.WebRequestMethods;
using Prova_WebHook.DTO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Prova_WebHook.Services;

namespace Prova_WebHook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebHookController : Controller
    {
        /// <summary>/// Metodo che riceve notifiche da Google Pub/Sub/// </summary>/// <param name="pubSubMessage"></param>/// <returns></returns>
        [HttpPost("RiceviNotificaGoogle")]
        public async Task<IActionResult> ValidateNotificatioGoogle([FromBody] Root pubSubMessage)
        {
            try
            {
                /*
                // Log del messaggio ricevuto
                 Console.WriteLine($"Raw message: {pubSubMessage}");
                 //Decodifica il messaggio Base64
                 var decodedMessage = Encoding.UTF8.GetString( Convert.FromBase64String(pubSubMessage.message.data));
                Console.WriteLine($"Decoded message: {decodedMessage}"); 
                // Converto la stringa in un JSON
                var dataJson = JsonSerializer.Deserialize<DataJson>(decodedMessage); 
                Console.WriteLine($"Deserialized data: {dataJson}");
*/
                var purchaseToken = "nnu";
                // dataJson.subscriptionNotification.purchaseToken;
                var packageName = "gggg";
                //dataJson.packageName;
                /* 
                 string url = $"https://androidpublisher.googleapis.com/androidpublisher/v3/applications/{packageName}/purchases/subscriptionsv2/tokens/{purchaseToken}";

                 // Creazione dell'istanza di HttpClient
                 using (HttpClient client = new HttpClient())
                 {

                     HttpResponseMessage response = await client.GetAsync(url);

                     // Verifica che la richiesta abbia avuto successo (status code 200-299)
                     if (response.IsSuccessStatusCode)
                     {
                         // Leggi il contenuto della risposta come stringa
                         string content = await response.Content.ReadAsStringAsync();
                         Console.WriteLine("Risultato della chiamata GET:");
                         Console.WriteLine(content);
                     }
                     else
                     {
                         Console.WriteLine($"Errore nella chiamata GET: {response.StatusCode}");
                         return BadRequest(response.StatusCode + " " + response.Content);
                     }*/

                string credentialsPath = "path_to_your_credentials_file.json";

                // Ottieni l'access token tramite OAuth 2.0
                var credential = GoogleCredential.FromFile(credentialsPath)
                    .CreateScoped(AndroidPublisherService.Scope.Androidpublisher);

                // Crea un servizio API AndroidPublisher
                var service = new AndroidPublisherService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Your App Name"
                });

                var request = service.Purchases.Subscriptionsv2.Get(packageName, purchaseToken);

                var response = await request.ExecuteAsync();

                Console.WriteLine("Response: " + response);

                return Ok();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RiceviNotificaApple")]
        public async Task<IActionResult> ValidateNotificationApple([FromBody] string notificationToken)
        {
            
            ValidateNotificationJwt validate = new ValidateNotificationJwt();

            AppleNotification apn = validate.ValidateAppleNotification(notificationToken);

            if (apn == null)
            {

                return BadRequest();
            
            }

            return Ok(apn);

        }
    }
}
