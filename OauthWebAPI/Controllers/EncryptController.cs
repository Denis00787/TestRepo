using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OauthWebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EncryptController:ControllerBase
	{
        private static IWebHostEnvironment _hostEnvironment;
        public EncryptController(IWebHostEnvironment hostEnvironment)
		{
            _hostEnvironment = hostEnvironment;
		}

        [HttpGet("Encrypt")]
        public ActionResult<string> EncryptData(string data)
        {
            var result = EncryptUsingCertificate(data);
            return Ok(result);
        }

        [HttpGet("Decrypt")]
        public ActionResult<string> DecryptData(string data)
        {
            var result = DecryptUsingCertificate(data);
            return Ok(result);
        }

        private string EncryptUsingCertificate(string data)
        {
            try
            {
                byte[] byteData = Encoding.UTF8.GetBytes(data);
                string path = Path.Combine(_hostEnvironment.WebRootPath, "mycert.pem");
                var collection = new X509Certificate2Collection();
                collection.Import(path);
                var certificate = collection[0];
                var output = "";
                using (RSA csp = (RSA)certificate.PublicKey.Key)
                {
                    byte[] bytesEncrypted = csp.Encrypt(byteData, RSAEncryptionPadding.OaepSHA1);
                    output = Convert.ToBase64String(bytesEncrypted);
                }
                return output;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string DecryptUsingCertificate(string data)
        {
            try
            {
                byte[] byteData = Convert.FromBase64String(data);
                string path = Path.Combine(_hostEnvironment.WebRootPath, "mycertprivatekey.pfx");
                var Password = "123"; //Note This Password is That Password That We Have Put On Generate Keys  
                var collection = new X509Certificate2Collection();
                collection.Import(System.IO.File.ReadAllBytes(path), Password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                X509Certificate2 certificate = new X509Certificate2();
                certificate = collection[0];
                foreach (var cert in collection)
                {
                    if (cert.FriendlyName.Contains("my certificate"))
                    {
                        certificate = cert;
                    }
                }
                if (certificate.HasPrivateKey)
                {
                    RSA csp = (RSA)certificate.PrivateKey;
                    var privateKey = certificate.PrivateKey as RSACryptoServiceProvider;
                    var keys = Encoding.UTF8.GetString(csp.Decrypt(byteData, RSAEncryptionPadding.OaepSHA1));
                    return keys;
                }
            }
            catch (Exception ex) { }
            return null;
        }
    }
}

