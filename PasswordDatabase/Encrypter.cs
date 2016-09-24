using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordDatabase
{
    class Encrypter
    {
        public static byte[] GetHashCode128(char[] input)
        {
            var hashAlgNames = new string[] { "RIPEMD160", "SHA1", "SHA256", "SHA384", "SHA512", "MD5" };
            byte[] curHash = Encoding.Unicode.GetBytes(input);
            foreach (var hash in hashAlgNames)
            {
                var hashAlg = HashAlgorithm.Create(hash);
                curHash = hashAlg.ComputeHash(curHash);
            }
            return curHash;
        }

        public static byte[] GetHashCode512(string input)
        {
            var hashAlgNames = new string[] { "MD5", "SHA384", "SHA256", "SHA1", "RIPEMD160", "SHA512" };
            byte[] curHash = Encoding.Unicode.GetBytes(input);
            foreach (var hash in hashAlgNames)
            {
                var hashAlg = HashAlgorithm.Create(hash);
                curHash = hashAlg.ComputeHash(curHash);
            }
            return curHash;
        }

        public static string Encrypt(char[] pass, string data)
        {
            var iv = new AesCryptoServiceProvider().IV;
            var algoritms = new string[] { "Aes", "RC2", "Rijndael", "TripleDES" };
            var dataBytes = Encoding.Unicode.GetBytes(data);
            foreach (var alg in algoritms)
            {
                dataBytes = EncryptAlg(alg, pass, iv, dataBytes);
            }
            var binSerializer = new BinarySerializer();
            binSerializer.AddData(iv);
            binSerializer.AddData(dataBytes);
            return Convert.ToBase64String(binSerializer.Serialize());
        }

        static byte[] EncryptAlg(string alg, char[] pass, byte[] iv, byte[] data)
        {
            var symAlg = SymmetricAlgorithm.Create(alg);
            symAlg.IV = iv.Take(symAlg.IV.Length).ToArray();
            var memory = new MemoryStream();
            CryptoStream encStream = new CryptoStream(
                memory,
                symAlg.CreateEncryptor(GetHashCode128(pass), symAlg.IV),
                CryptoStreamMode.Write);
            encStream.Write(data, 0, data.Length);
            encStream.Close();
            return memory.ToArray();
        }

        public static string Decrypt(char[] pass, string data)
        {
            var algoritms = new string[] { "TripleDES", "Rijndael", "RC2", "Aes" };
            var binSerializer = new BinarySerializer();
            binSerializer.Deserialize(Convert.FromBase64String(data));
            var iv = binSerializer.ReadNext();
            var encrData = binSerializer.ReadNext();
            foreach (var alg in algoritms)
            {
                encrData = DecryptAlg(alg, pass, iv, encrData);
            }
            return Encoding.Unicode.GetString(encrData);
        }

        static byte[] DecryptAlg(string alg, char[] pass, byte[] iv, byte[] data)
        {
            var symAlg = SymmetricAlgorithm.Create(alg);
            symAlg.IV = iv.Take(symAlg.IV.Length).ToArray();
            var memory = new MemoryStream(data);
            CryptoStream encStream = new CryptoStream(
                memory,
                symAlg.CreateDecryptor(GetHashCode128(pass), symAlg.IV),
                CryptoStreamMode.Read);
            var resultBuffer = new List<byte>(400);
            int curByte = 0;
            while ((curByte = encStream.ReadByte()) != -1)
            {
                resultBuffer.Add((byte)curByte);
            }
            encStream.Close();
            return resultBuffer.ToArray();
        }
    }
}
