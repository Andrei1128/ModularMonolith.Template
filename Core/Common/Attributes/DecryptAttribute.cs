using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Security.Cryptography;

namespace Core.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DecryptAttribute : Attribute { }

public class DecryptConfigurationSource(IConfiguration _configuration) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) => new DecryptConfigurationProvider(_configuration);
}

public class DecryptConfigurationProvider(IConfiguration _configuration) : ConfigurationProvider
{
    private readonly byte[] AesKey = [112, 97, 114, 111, 108, 97, 115, 101, 99, 114, 101, 116, 97, 99, 97, 114, 101, 110, 117, 97, 114, 101, 109, 111, 97, 114, 116, 101, 101, 118, 101, 114];
    private readonly byte[] AesIV = [118, 101, 99, 116, 111, 114, 103, 114, 97, 102, 105, 99, 114, 111, 115, 117];

    public override void Load()
    {
        var classesWithDecryptableProperties = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.GetCustomAttributes(typeof(DecryptAttribute), true).Length > 0);

        foreach (var type in classesWithDecryptableProperties)
        {
            var section = _configuration.GetSection(type.Name);

            if (section.Exists())
            {
                foreach (var prop in section.GetChildren())
                {
                    var property = type.GetProperty(prop.Key);

                    if (property != null && Attribute.IsDefined(property, typeof(DecryptAttribute)))
                    {
                        string? encryptedValue = prop.Value;
                        Data[prop.Key] = Decrypt(encryptedValue);
                    }
                    else
                    {
                        Data[prop.Key] = prop.Value;
                    }
                }
            }
        }
    }

    private string? Decrypt(string? value)
    {
        if (value is null)
        {
            return null;
        }

        byte[] encryptedBytes = Convert.FromBase64String(value);

        using var aes = Aes.Create();

        aes.Key = AesKey;
        aes.IV = AesIV;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var msDecrypt = new MemoryStream(encryptedBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }
}