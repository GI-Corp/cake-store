namespace Shared.Common.Encryption.Interfaces;

public interface IEncryptionService
{
    string EncryptRijndael(string text);

    string DecryptRijndael(string cipherText);

}