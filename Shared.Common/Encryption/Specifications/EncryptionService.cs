using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Shared.Common.Authorization.Encryption;
using Shared.Common.Encryption.Interfaces;

namespace Shared.Common.Encryption.Specifications;

public class EncryptionService : IEncryptionService
{
    private readonly EncryptionOptions _encryptionOptions;

    public EncryptionService(IOptions<EncryptionOptions> encryptionOptions)
    {
        _encryptionOptions = encryptionOptions.Value ??
                             throw new ArgumentNullException(nameof(encryptionOptions));
    }
    
    internal const string InputKey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";

    #region Rijndael Encryption

    /// <summary>
    /// Encrypt the given text and give the byte array back as a BASE64 string
    /// </summary>
    /// <param name="text" />The text to encrypt
    /// <param name="salt" />The password salt
    /// <returns>The encrypted text</returns>
    internal static string EncryptRijndael(string text, string salt)
    {
        if (string.IsNullOrWhiteSpace(text)) 
            throw new ArgumentNullException(nameof(text));

        using var aesAlg = NewRijndaelManaged(salt);
        using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using var swEncrypt = new StreamWriter(csEncrypt);
            swEncrypt.Write(text);
        }

        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public string EncryptRijndael(string text)
    {
        return EncryptRijndael(text, _encryptionOptions.EncryptionKey);
    }

    #endregion

    #region Rijndael Dycryption

    /// <summary>
    /// Checks if a string is base64 encoded
    /// </summary>
    /// <param name="base64String" />The base64 encoded string
    /// <returns>Base64 encoded string</returns>
    public static bool IsBase64String(string base64String)
    {
        base64String = base64String.Trim();
        return base64String.Length % 4 == 0 &&
               Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }

    public string DecryptRijndael(string cipherText)
    {
        return DecryptRijndael(cipherText, _encryptionOptions.EncryptionKey);
    }

    /// <summary>
    /// Decrypts the given text
    /// </summary>
    /// <param name="cipherText" />The encrypted BASE64 text
    /// <param name="salt" />The pasword salt
    /// <returns>The decrypted text</returns>
    internal static string DecryptRijndael(string cipherText, string salt)
    {
        if (string.IsNullOrWhiteSpace(cipherText)) 
            throw new ArgumentNullException(nameof(cipherText));

        if (!IsBase64String(cipherText)) 
            throw new Exception("The cipherText input parameter is not base64 encoded");

        string text;

        using var aesAlg = NewRijndaelManaged(salt);
        using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        var cipher = Convert.FromBase64String(cipherText);

        using (var msDecrypt = new MemoryStream(cipher))
        {
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            text = srDecrypt.ReadToEnd();
        }

        return text;
    }

    #endregion

    #region NewRijndaelManaged

    /// <summary>
    /// Create a new RijndaelManaged class and initialize it
    /// </summary>
    /// <param name="salt" />The pasword salt
    /// <returns></returns>
    private static Aes NewRijndaelManaged(string salt)
    {
        if (salt == null) 
            throw new ArgumentNullException(nameof(salt));
        
        var saltBytes = Encoding.ASCII.GetBytes(salt);

        using var key = new Rfc2898DeriveBytes(InputKey, saltBytes);

        var aesAlg = Aes.Create();
        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
        aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

        return aesAlg;
    }

    #endregion
}