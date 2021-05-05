using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Serialization;

public static class SaveHelper
{
    // Serialise
    public static string Serialise<T>(this T toSerialise)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, toSerialise);
        return writer.ToString();
    }

    // Deserialise
    public static T DeSerialise<T>(this string toDeSerialise)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(toDeSerialise);
        return (T)xml.Deserialize(reader);
    }

    private static string hash = "RhythmGame.apk";

    // Encrypt
    public static string Encrypt(string a_input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(a_input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] result = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(result, 0, result.Length);
            }
        }
    }

    // Decrypt
    public static string Decrypt(string a_input)
    {
        byte[] data = Convert.FromBase64String(a_input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] result = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(result, 0, result.Length);
            }
        }
    }
}
