using System;

namespace PrivacyTypes.SampleImplementations
{
    internal class YourApp
    {
        public static string GetSecretValueFromVault(string highSecretKey1)
        {
            return Guid.NewGuid().ToString();
        }
    }
}