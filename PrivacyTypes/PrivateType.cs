using System;

namespace PrivacyTypes
{
    abstract internal class PrivateType<T> : IDisposable
    {
        public PrivateType(T content)
        {
            this._content = content;
        }

        private protected T _content;

        public PrivateType()
        {
        }
        internal T __unsafeGet(PrivateTypeAuthorizationContext context)
        {
            if (context.IsValid) return this._content;
            throw new InvalidOperationException("An invalid authorization context was provided");
        }

        public void Set(T content)
        {
            this._content = content;
        }

        public void Clear()
        {
            this.Set(default(T));
        }

        public override string ToString()
        {
            throw new InvalidOperationException("An attempt was made to get the string's content");
        }

        public void Dispose()
        {
            this._content = default;
        }
    }
}