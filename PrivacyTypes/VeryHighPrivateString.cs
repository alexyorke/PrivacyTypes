using System;

namespace PrivacyTypes
{
    class VeryHighPrivateString : PrivateType<string>
    {
        public VeryHighPrivateString(string a, PrivateTypeAuthorizationContext context)
        {
            this._content = a;
        }

        public VeryHighPrivateString Concat(VeryHighPrivateString a, PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(this, a, context);
        }

        public VeryHighPrivateString Concat(HighPrivateString a, PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(this, new VeryHighPrivateString(a.__unsafeGet(context), context), context);
        }

        public VeryHighPrivateString Concat(MediumPrivateString a, PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(this, new VeryHighPrivateString(a.__unsafeGet(context), context), context);
        }

        public VeryHighPrivateString Concat(LowPrivateString a, PrivateTypeAuthorizationContext context)
        {
            return ConcatCore(this, new VeryHighPrivateString(a.__unsafeGet(context), context), context);
        }

        public static VeryHighPrivateString ConcatCore(VeryHighPrivateString a, VeryHighPrivateString b, PrivateTypeAuthorizationContext context)
        {
            if (context.IsValid && context.level >= PrivateTypeAuthorizationContextPrivacyLevel.HIGH)
            {
                return new VeryHighPrivateString(a.__unsafeGet(context) + b.__unsafeGet(context), context);
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