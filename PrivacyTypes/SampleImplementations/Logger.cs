using System;

namespace PrivacyTypes.SampleImplementations
{
    internal class Logger
    {
        private readonly PrivateTypeAuthorizationContextPrivacyLevel _privateTypeAuthorizationContextPrivacyLevel;

        public Logger(PrivateTypeAuthorizationContextPrivacyLevel privateTypeAuthorizationContextPrivacyLevel)
        {
            this._privateTypeAuthorizationContextPrivacyLevel = privateTypeAuthorizationContextPrivacyLevel;
        }

        public void Log(HighPrivateString contaminatedConcat, PrivateTypeAuthorizationContext context)
        {
            if (this._privateTypeAuthorizationContextPrivacyLevel >= PrivateTypeAuthorizationContextPrivacyLevel.HIGH)
            {
                Console.WriteLine(contaminatedConcat.__unsafeGet(context));
            }
            else
            {
                Console.WriteLine("Not logging secret value because privacy context is too low");
            }
        }

        public void Log(MediumPrivateString contaminatedConcat, PrivateTypeAuthorizationContext context)
        {
            if (this._privateTypeAuthorizationContextPrivacyLevel >= PrivateTypeAuthorizationContextPrivacyLevel.MEDIUM)
            {
                Console.WriteLine(contaminatedConcat.__unsafeGet(context));
            }
            else
            {
                Console.WriteLine("Not logging secret value because privacy context is too low");
            }
        }

        public void Log(LowPrivateString contaminatedConcat, PrivateTypeAuthorizationContext context)
        {
            if (this._privateTypeAuthorizationContextPrivacyLevel >= PrivateTypeAuthorizationContextPrivacyLevel.LOW)
            {
                Console.WriteLine(contaminatedConcat.__unsafeGet(context));
            }
            else
            {
                Console.WriteLine("Not logging secret value because privacy context is too low");
            }
        }
    }
}