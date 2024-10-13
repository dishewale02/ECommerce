
namespace ECommerce.Services.Classes.CryptoService
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class AESEncryptDecryptService
    {
        // The secret passphrase to derive the key and IV
        private static readonly string secret = "YourSuperSecretPassphrase";

        // Salt for key derivation (this should ideally be random and stored securely)
        private static readonly byte[] salt = Encoding.UTF8.GetBytes("S@ltV@lue12345");

        // Method to generate a key and IV from the secret
        private static void GetKeyAndIV(out byte[] key, out byte[] iv)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(secret, salt, 10000)) // 10,000 iterations
            {
                key = rfc2898.GetBytes(16); // 16 bytes for AES-128 key
                iv = rfc2898.GetBytes(16);  // 16 bytes for AES block size
            }
        }

        // Static method to encrypt a string
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText), "The plain text cannot be null or empty.");

            GetKeyAndIV(out byte[] key, out byte[] iv);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }

                    // Return the encrypted bytes from the memory stream.
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // Static method to decrypt a string
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText), "The cipher text cannot be null or empty.");

            GetKeyAndIV(out byte[] key, out byte[] iv);

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the stream and return the string.
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

}
