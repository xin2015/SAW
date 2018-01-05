using SAW.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.CryptoTransverters
{
    /// <summary>
    /// 对称加密
    /// </summary>
    public class SymmetricalEncryption : IEncrypt, IDecrypt
    {
        public static SymmetricalEncryption Default { get; set; }
        static SymmetricalEncryption()
        {
            Default = new SymmetricalEncryption("ABC!@2017Key");
        }

        protected HashAlgorithm HA { get; set; }
        protected SymmetricAlgorithm SA { get; set; }

        public SymmetricalEncryption()
        {
            HA = GetHashAlgorithm();
            SA = GetSymmetricAlgorithm();
        }

        public SymmetricalEncryption(string key)
        {
            HA = GetHashAlgorithm();
            SA = GetSymmetricAlgorithm();
            byte[] tdesKey = GetKey(key);
            byte[] tdesIV = GetIV(tdesKey);
            SA.Key = tdesKey;
            SA.IV = tdesIV;
        }

        public SymmetricalEncryption(byte[] tdesKey)
        {
            HA = GetHashAlgorithm();
            SA = GetSymmetricAlgorithm();
            byte[] tdesIV = GetIV(tdesKey);
            SA.Key = tdesKey;
            SA.IV = tdesIV;
        }

        public SymmetricalEncryption(byte[] tdesKey, byte[] tdesIV)
        {
            HA = GetHashAlgorithm();
            SA = GetSymmetricAlgorithm();
            SA.Key = tdesKey;
            SA.IV = tdesIV;
        }

        protected virtual HashAlgorithm GetHashAlgorithm()
        {
            return SHA256.Create();
        }

        protected virtual SymmetricAlgorithm GetSymmetricAlgorithm()
        {
            return Aes.Create();
        }

        protected virtual byte[] GetKey(string key)
        {
            return HA.ComputeHash(key.FromUTF8String());
        }

        protected virtual byte[] GetIV(byte[] key)
        {
            return key.Take(16).ToArray();
        }

        #region Encrypt
        /// <summary>
        /// 使用Aes进行对称加密
        /// </summary>
        /// <param name="inputBuffer">待加密的字节数组</param>
        /// <param name="tdesKey">Aes算法的密钥</param>
        /// <param name="tdesIV">Aes算法的初始化向量</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] inputBuffer, byte[] tdesKey, byte[] tdesIV)
        {
            return SA.CreateEncryptor(tdesKey, tdesIV).TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        }

        /// <summary>
        /// 使用Aes进行对称加密，密钥与初始化向量关联
        /// </summary>
        /// <param name="inputBuffer">待加密的字节数组</param>
        /// <param name="tdesKey">Aes算法的密钥字节数组</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] inputBuffer, byte[] tdesKey)
        {
            byte[] tdesIV = GetIV(tdesKey);
            return Encrypt(inputBuffer, tdesKey, tdesIV);
        }

        /// <summary>
        /// 使用Aes进行对称加密，密钥与初始化向量关联
        /// </summary>
        /// <param name="inputBuffer">待加密的字节数组</param>
        /// <param name="key">Aes算法的密钥字符串（UTF8）</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] inputBuffer, string key)
        {
            byte[] tdesKey = GetKey(key);
            return Encrypt(inputBuffer, tdesKey);
        }

        /// <summary>
        /// 使用Aes进行对称加密，密钥和初始化向量为默认值
        /// </summary>
        /// <param name="inputBuffer">待加密的字节数组</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] inputBuffer)
        {
            return SA.CreateEncryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        }

        /// <summary>
        /// 使用Aes进行对称加密
        /// </summary>
        /// <param name="inString">待加密的字符串（UTF8）</param>
        /// <param name="tdesKey">Aes算法的密钥</param>
        /// <param name="tdesIV">Aes算法的初始化向量</param>
        /// <returns>加密后的字符串（Base64）</returns>
        public string Encrypt(string inString, byte[] tdesKey, byte[] tdesIV)
        {
            byte[] inputBuffer = inString.FromUTF8String();
            return Encrypt(inputBuffer, tdesKey, tdesIV).ToBase64String();
        }

        /// <summary>
        /// 使用Aes进行对称加密，密钥与初始化向量关联
        /// </summary>
        /// <param name="inString">待加密的字符串（UTF8）</param>
        /// <param name="tdesKey">Aes算法的密钥字节数组</param>
        /// <returns>加密后的字符串（Base64）</returns>
        public string Encrypt(string inString, byte[] tdesKey)
        {
            byte[] tdesIV = GetIV(tdesKey);
            return Encrypt(inString, tdesKey, tdesIV);
        }

        /// <summary>
        /// 使用Aes进行对称加密，密钥与初始化向量关联
        /// </summary>
        /// <param name="inString">待加密的字符串（UTF8）</param>
        /// <param name="key">Aes算法的密钥字符串（UTF8）</param>
        /// <returns>加密后的字符串（Base64）</returns>
        public string Encrypt(string inString, string key)
        {
            byte[] tdesKey = GetKey(key);
            return Encrypt(inString, tdesKey);
        }

        /// <summary>
        /// 使用Aes进行对称加密，密钥和初始化向量为默认值
        /// </summary>
        /// <param name="inString">待加密的字符串（UTF8）</param>
        /// <returns>加密后的字符串（Base64）</returns>
        public string Encrypt(string inString)
        {
            byte[] inputBuffer = inString.FromUTF8String();
            return Encrypt(inputBuffer).ToBase64String();
        }
        #endregion
        #region Decrypt
        /// <summary>
        /// 使用Aes进行对称解密
        /// </summary>
        /// <param name="inputBuffer">待解密的字节数组</param>
        /// <param name="tdesKey">Aes算法的密钥</param>
        /// <param name="tdesIV">Aes算法的初始化向量</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] inputBuffer, byte[] tdesKey, byte[] tdesIV)
        {
            return SA.CreateDecryptor(tdesKey, tdesIV).TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        }

        /// <summary>
        /// 使用Aes进行对称解密，密钥与初始化向量关联
        /// </summary>
        /// <param name="inputBuffer">待解密的字节数组</param>
        /// <param name="tdesKey">Aes算法的密钥字节数组</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] inputBuffer, byte[] tdesKey)
        {
            byte[] tdesIV = GetIV(tdesKey);
            return Decrypt(inputBuffer, tdesKey, tdesIV);
        }

        /// <summary>
        /// 使用Aes进行对称解密，密钥与初始化向量关联
        /// </summary>
        /// <param name="inputBuffer">待解密的字节数组</param>
        /// <param name="key">Aes算法的密钥字符串（UTF8）</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] inputBuffer, string key)
        {
            byte[] tdesKey = GetKey(key);
            return Decrypt(inputBuffer, tdesKey);
        }

        /// <summary>
        /// 使用Aes进行对称解密，密钥和初始化向量为默认值
        /// </summary>
        /// <param name="inputBuffer">待解密的字节数组</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] inputBuffer)
        {
            return SA.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        }

        /// <summary>
        /// 使用Aes进行对称解密
        /// </summary>
        /// <param name="inString">待解密的字符串（Base64）</param>
        /// <param name="tdesKey">Aes算法的密钥</param>
        /// <param name="tdesIV">Aes算法的初始化向量</param>
        /// <returns>解密后的字符串（UTF8）</returns>
        public string Decrypt(string inString, byte[] tdesKey, byte[] tdesIV)
        {
            byte[] inputBuffer = inString.FromBase64String();
            return Decrypt(inputBuffer, tdesKey, tdesIV).ToUTF8String();
        }

        /// <summary>
        /// 使用Aes进行对称解密，密钥与初始化向量关联
        /// </summary>
        /// <param name="inString">待解密的字符串（Base64）</param>
        /// <param name="tdesKey">Aes算法的密钥字节数组</param>
        /// <returns>解密后的字符串（UTF8）</returns>
        public string Decrypt(string inString, byte[] tdesKey)
        {
            byte[] tdesIV = GetIV(tdesKey);
            return Decrypt(inString, tdesKey, tdesIV);
        }

        /// <summary>
        /// 使用Aes进行对称解密，密钥与初始化向量关联
        /// </summary>
        /// <param name="inString">待解密的字符串（Base64）</param>
        /// <param name="key">Aes算法的密钥字符串（UTF8）</param>
        /// <returns>解密后的字符串（UTF8）</returns>
        public string Decrypt(string inString, string key)
        {
            byte[] tdesKey = GetKey(key);
            return Decrypt(inString, tdesKey);
        }

        /// <summary>
        /// 使用Aes进行对称解密，密钥和初始化向量为默认值
        /// </summary>
        /// <param name="inString">待解密的字符串（Base64）</param>
        /// <returns>解密后的字符串（UTF8）</returns>
        public string Decrypt(string inString)
        {
            byte[] inputBuffer = inString.FromBase64String();
            return Decrypt(inputBuffer).ToUTF8String();
        }
        #endregion
    }
}
