using System;

namespace PrivacyTypes
{
    class MediumPrivateString : PrivateType<string>
    {
        public static explicit operator MediumPrivateString(string str) => new MediumPrivateString(str);
        public MediumPrivateString(string str)
        {
            this._content = str;
        }
        public static MediumPrivateString Concat(MediumPrivateString firstStr, MediumPrivateString secondStr,
            PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(new MediumPrivateString(firstStr.__unsafeGet(context)), new MediumPrivateString(secondStr.__unsafeGet(context)), context);
        }

        public static MediumPrivateString Concat(MediumPrivateString firstStr, LowPrivateString secondStr,
            PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(new MediumPrivateString(firstStr.__unsafeGet(context)), new MediumPrivateString(secondStr.__unsafeGet(context)), context);
        }

        public static MediumPrivateString Concat(LowPrivateString firstStr, MediumPrivateString secondStr,
            PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(new MediumPrivateString(firstStr.__unsafeGet(context)), new MediumPrivateString(secondStr.__unsafeGet(context)), context);
        }

        public static MediumPrivateString ConcatCore(MediumPrivateString firstStr, MediumPrivateString secondStr, PrivateTypeAuthorizationContext context)
        {
            if (context.IsValid && context.level >= PrivateTypeAuthorizationContextPrivacyLevel.MEDIUM)
            {
                return new MediumPrivateString(firstStr.__unsafeGet(context) + secondStr.__unsafeGet(context));
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