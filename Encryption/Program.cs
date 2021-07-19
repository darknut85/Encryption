 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            //reading the txt file
            string vr = "hi";
            try
            {
                using (var sr = new StreamReader("Random.txt"))
                {
                    vr = sr.ReadToEnd();
                }
                
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            //Console.WriteLine(vr);

            /////////////////////////////////////////////////////////////////////
            //ENCRYPTION PART
            var key = "Eevee133Sceptile254Mew151Snom???";

            //var str = vr;
            var str = vr;
            Console.WriteLine(str);

            var encrypted = AesOp.Encrypt(key, str);
            Console.WriteLine($"encryped string = { encrypted }");

            var doubleCrypted = AesOp.Encrypt(key, encrypted);
            Console.WriteLine(($"Doube encryped string = { doubleCrypted }"));

            var doubleDecrypted = AesOp.Decrypt(key, doubleCrypted);
            Console.WriteLine($"Double decrypted string = {doubleDecrypted}");

            var decrypted = AesOp.Decrypt(key, doubleDecrypted);
            Console.WriteLine($"decrypted string = {decrypted}");

            Console.ReadKey();
        }
        public class AesOp
        {
            //////////////////////////////////////////////////////////////////
            //encryption process
            public static string Encrypt(string key, string plainText)
            {
                byte[] iv = new byte[16];
                byte[] encrypted;

                if (plainText == null || plainText.Length <= 0)
                {
                    throw new ArgumentNullException("plainText");
                }
                if (key == null || key.Length <= 0)
                {
                    throw new ArgumentNullException("Key");
                }
                if (iv == null || iv.Length <= 0)
                {
                    throw new ArgumentNullException("IV");
                }

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = iv;

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream((Stream)msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter((Stream)csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                            return Convert.ToBase64String(encrypted);
                        }
                    }
                }
            }

            //////////////////////////////////////////////////////////////////
            //decryption process
            public static string Decrypt(string key, string cipherText)
            {

                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                if (cipherText == null || cipherText.Length <= 0)
                {
                    throw new ArgumentNullException("cipherText");
                }
                if (key == null || key.Length <= 0)
                {
                    throw new ArgumentNullException("Key");
                }
                if (iv == null || iv.Length <= 0)
                {
                    throw new ArgumentNullException("IV");
                }

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = iv;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(buffer))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream((Stream)msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader((Stream)csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
