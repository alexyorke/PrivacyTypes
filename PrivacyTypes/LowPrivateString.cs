using System;

namespace PrivacyTypes
{
    class LowPrivateString : PrivateType<string>
    {
        public static explicit operator LowPrivateString(string str) => new LowPrivateString(str);
        public LowPrivateString(string str)
        {
            this._content = str;
        }

        public static LowPrivateString Concat(LowPrivateString firstStr, LowPrivateString secondStr,
            PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(new LowPrivateString(firstStr.__unsafeGet(context)), new LowPrivateString(secondStr.__unsafeGet(context)), context);
        }

        public static LowPrivateString ConcatCore(LowPrivateString firstStr, LowPrivateString secondStr, PrivateTypeAuthorizationContext context)
        {
            if (context.level >= PrivateTypeAuthorizationContextPrivacyLevel.LOW)
            {
                return new LowPrivateString(firstStr.__unsafeGet(context) + secondStr.__unsafeGet(context));
            }

            throw new InvalidOperationException(
                "The PrivacyTypeAuthorizationContext level must be equal to or higher than MEDIUM");
        }

        public void Replace(string oldValue, string newValue)
        {
            // clean exceptions must be thrown so that original strings are not leaked
            if (oldValue == null) throw new ArgumentNullException("oldValue cannot be null");
            if (newValue == null) throw new ArgumentNullException("newValue cannot be null");

            try
            {
                this._content = this._content.Replace(oldValue, newValue);
            }
            catch (ArgumentException)
            {
                // remove details of stacktrace by creating a new one
                if (newValue == null) throw new ArgumentException("An ArgumentException was thrown");
            }
        }
    }
}