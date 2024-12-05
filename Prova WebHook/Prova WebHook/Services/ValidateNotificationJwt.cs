using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using Prova_WebHook.DTO;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Prova_WebHook.Services
{
    public class ValidateNotificationJwt
    {
        public AppleNotification ValidateAppleNotification(string notificationToken)
        {

            try
            {

                var (headerJsonNT, payloadJsonNT) = DivideToken(notificationToken);

                JArray x5cArray = (JArray)headerJsonNT["x5c"];

                X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String(x5cArray[0].ToString()));

                var publicKey = certificate.GetECDsaPublicKey();

                ValidateToken(publicKey, notificationToken);

                Console.WriteLine("Parte 1 valida");

                string signedTI = //"jFkR2h2Y21sMGVURVRNQkVHQTFVRUNnd0tRWEJ3YkdVZ1NXNWpMakVMTUFrR0ExVUVCaE1DVlZNd0hoY05NakV3TXpFM01qQXpOekV3V2hjTk16WXdNekU1TURBd01EQXdXakIxTVVRd1FnWURWUVFERER0QmNIQnNaU0JYYjNKc1pIZHBaR1VnUkdWMlpXeHZjR1Z5SUZKbGJHRjBhVzl1Y3lCRFpYSjBhV1pwWTJGMGFXOXVJRUYxZEdodmNtbDBlVEVMTUFrR0ExVUVDd3dDUnpZeEV6QVJCZ05WQkFvTUNrRndjR3hsSUVsdVl5NHhDekFKQmdOVkJBWVRBbFZUTUhZd0VBWUhLb1pJemowQ0FRWUZLNEVFQUNJRFlnQUVic1FLQzk0UHJsV21aWG5YZ3R4emRWSkw4VDBTR1luZ0RSR3BuZ24zTjZQVDhKTUViN0ZEaTRiQm1QaENuWjMvc3E2UEYvY0djS1hXc0w1dk90ZVJoeUo0NXgzQVNQN2NPQithYW85MGZjcHhTdi9FWkZibmlBYk5nWkdoSWhwSW80SDZNSUgzTUJJR0ExVWRFd0VCL3dRSU1BWUJBZjhDQVFBd0h3WURWUjBqQkJnd0ZvQVV1N0Rlb1ZnemlKcWtpcG5ldnIzcnI5ckxKS3N3UmdZSUt3WUJCUVVIQVFFRU9qQTRNRFlHQ0NzR0FRVUZCekFCaGlwb2RIUndPaTh2YjJOemNDNWhjSEJzWlM1amIyMHZiMk56Y0RBekxXRndjR3hsY205dmRHTmhaek13TndZRFZSMGZCREF3TGpBc29DcWdLSVltYUhSMGNEb3ZMMk55YkM1aGNIQnNaUzVqYjIwdllYQndiR1Z5YjI5MFkyRm5NeTVqY213d0hRWURWUjBPQkJZRUZEOHZsQ05SMDFESm1pZzk3YkI4NWMrbGtHS1pNQTRHQTFVZER3RUIvd1FFQXdJQkJqQVFCZ29xaGtpRzkyTmtCZ0lCQkFJRkFEQUtCZ2dxaGtqT1BRUURBd05vQURCbEFqQkFYaFNxNUl5S29nTUNQdHc0OTBCYUI2NzdDYUVHSlh1ZlFCL0VxWkdkNkNTamlDdE9udU1UYlhWWG14eGN4ZmtDTVFEVFNQeGFyWlh2TnJreFUzVGtVTUkzM3l6dkZWVlJUNHd4V0pDOTk0T3NkY1o0K1JHTnNZRHlSNWdtZHIwbkRHZz0iLCJNSUlDUXpDQ0FjbWdBd0lCQWdJSUxjWDhpTkxGUzVVd0NnWUlLb1pJemowRUF3TXdaekViTUJrR0ExVUVBd3dTUVhCd2JHVWdVbTl2ZENCRFFTQXRJRWN6TVNZd0pBWURWUVFMREIxQmNIQnNaU0JEWlhKMGFXWnBZMkYwYVc5dUlFRjFkR2h2Y21sMGVURVRNQkVHQTFVRUNnd0tRWEJ3YkdVZ1NXNWpMakVMTUFrR0ExVUVCaE1DVlZNd0hoY05NVFF3TkRNd01UZ3hPVEEyV2hjTk16a3dORE13TVRneE9UQTJXakJuTVJzd0dRWURWUVFEREJKQmNIQnNaU0JTYjI5MElFTkJJQzBnUnpNeEpqQWtCZ05WQkFzTUhVRndjR3hsSUVObGNuUnBabWxqWVhScGIyNGdRWFYwYUc5eWFYUjVNUk13RVFZRFZRUUtEQXBCY0hCc1pTQkpibU11TVFzd0NRWURWUVFHRXdKVlV6QjJNQkFHQnlxR1NNNDlBZ0VHQlN1QkJBQWlBMklBQkpqcEx6MUFjcVR0a3lKeWdSTWMzUkNWOGNXalRuSGNGQmJaRHVXbUJTcDNaSHRmVGpqVHV4eEV0WC8xSDdZeVlsM0o2WVJiVHpCUEVWb0EvVmhZREtYMUR5eE5CMGNUZGRxWGw1ZHZNVnp0SzUxN0lEdll1VlRaWHBta09sRUtNYU5DTUVBd0hRWURWUjBPQkJZRUZMdXczcUZZTTRpYXBJcVozcjY5NjYvYXl5U3JNQThHQTFVZEV3RUIvd1FGTUFNQkFmOHdEZ1lEVlIwUEFRSC9CQVFEQWdFR01Bb0dDQ3FHU000OUJBTURBMmdBTUdVQ01RQ0Q2Y0hFRmw0YVhUUVkyZTN2OUd3T0FFWkx1Tit5UmhIRkQvM21lb3locG12T3dnUFVuUFdUeG5TNGF0K3FJeFVDTUcxbWloREsxQTNVVDgyTlF6NjBpbU9sTTI3amJkb1h0MlFmeUZNbStZaGlkRGtMRjF2TFVhZ002QmdENTZLeUtBPT0iXX0.eyJ0cmFuc2FjdGlvbklkIjoiMjAwMDAwMDc5NTU3ODc4OSIsIm9yaWdpbmFsVHJhbnNhY3Rpb25JZCI6IjIwMDAwMDA3OTU1Nzg3ODkiLCJidW5kbGVJZCI6ImNvbS5kaWdpdGFsaXR5LkdyYWZvbG9nSUEiLCJwcm9kdWN0SWQiOiJCT09TVCIsInB1cmNoYXNlRGF0ZSI6MTczMzMxMDExMzAwMCwib3JpZ2luYWxQdXJjaGFzZURhdGUiOjE3MzMzMTAxMTMwMDAsInF1YW50aXR5IjoxLCJ0eXBlIjoiQ29uc3VtYWJsZSIsImluQXBwT3duZXJzaGlwVHlwZSI6IlBVUkNIQVNFRCIsInNpZ25lZERhdGUiOjE3MzMzMTAxMjUwMDMsImVudmlyb25tZW50IjoiU2FuZGJveCIsInRyYW5zYWN0aW9uUmVhc29uIjoiUFVSQ0hBU0UiLCJzdG9yZWZyb250IjoiSVRBIiwic3RvcmVmcm9udElkIjoiMTQzNDUwIiwicHJpY2UiOjI5OTAsImN1cnJlbmN5IjoiRVVSIn0.hdqwyJWlcQVtfi15Twvlu0IbUQVoNh8w7MAZmV-tAyDFHcsJN0qpR93-XpcYdGzIDOoxZtWtxWTpHgcfVxj9q";
                payloadJsonNT["data"]["signedTransactionInfo"].ToString();

                var (headerJsonSTI, payloadJsonSTI) = DivideToken(signedTI);

                x5cArray = (JArray)headerJsonSTI["x5c"];

                certificate = new X509Certificate2(Convert.FromBase64String(x5cArray[0].ToString()));

                publicKey = certificate.GetECDsaPublicKey();

                ValidateToken(publicKey, signedTI);

                Console.WriteLine("Parte 2 valida ");

                AppleNotification apnot = new AppleNotification
                {
                    originalTransactionId = payloadJsonSTI["originalTransactionId"].ToString(),
                    notificationType = payloadJsonNT["notificationType"].ToString()
                };

                return apnot;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }

        }
private byte[] DecodeBase64Url(string input)
{
    // Decodifica un valore Base64 URL
    string base64 = input.Replace('_', '/').Replace('-', '+');
    switch (base64.Length % 4)
    {
        case 2: base64 += "=="; break;
        case 3: base64 += "="; break;
    }
    return Convert.FromBase64String(base64);
}

private void ValidateToken(ECDsa publicKey, string jwt)
{

    var tokenHandler = new JwtSecurityTokenHandler();

    var validationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        IssuerSigningKey = new ECDsaSecurityKey(publicKey),
        ClockSkew = TimeSpan.Zero
    };

    tokenHandler.ValidateToken(jwt, validationParameters, out SecurityToken validatedToken);
}

private (JObject, JObject) DivideToken(string jwt)
{
    string[] parts = jwt.Split(".");

    string enHeader = parts[0];
    //Console.WriteLine(enHeader);
    string enPayload = parts[1];
    //Console.WriteLine("\n" + enPayload);
    string enSignature = parts[2];

    var payload = Encoding.UTF8.GetString(DecodeBase64Url(enPayload));

    var payloadJson = JObject.Parse(payload);

    var header = Encoding.UTF8.GetString(DecodeBase64Url(enHeader));

    var headerJson = JObject.Parse(header);

    return (headerJson, payloadJson);
}




    }
}
