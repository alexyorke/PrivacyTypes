using System;

namespace PrivacyTypes
{
    class PrivateTypeAuthorizationContext : IDisposable
    {
        private Guid context;
        public PrivateTypeAuthorizationContextPrivacyLevel level { get; }

        public PrivateTypeAuthorizationContext(PrivateTypeAuthorizationContextPrivacyLevel level)
        {
            this.context = Guid.NewGuid();
            this.level = level;
        }
        public bool IsValid => context != Guid.Empty;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context = Guid.Empty;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}