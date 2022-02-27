using System;

namespace PrivacyTypes
{
    class HighPrivateString : PrivateType<string>
    {
        public static explicit operator HighPrivateString(string str) => new HighPrivateString(str);
        public HighPrivateString(string str)
        {
            this._content = str;
        }

        public HighPrivateString Concat(HighPrivateString str, PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(this, str, context);
        }

        public HighPrivateString Concat(MediumPrivateString str, PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(this, new HighPrivateString(str.__unsafeGet(context)), context);
        }

        public HighPrivateString Concat(LowPrivateString str, PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(this, new HighPrivateString(str.__unsafeGet(context)), context);
        }

        public static HighPrivateString ConcatCore(HighPrivateString firstStr, HighPrivateString secondStr, PrivateTypeAuthorizationContext context)
        {
            if (context.level >= PrivateTypeAuthorizationContextPrivacyLevel.HIGH)
            {
                return new HighPrivateString(firstStr.__unsafeGet(context) + secondStr.__unsafeGet(context));
            }

            throw new InvalidOperationException(
                "The PrivacyTypeAuthorizationContext level must be equal to or higher than HIGH");
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