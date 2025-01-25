
using System;
using System.Threading;

namespace Redbox.HAL.Component.Model.Threading
{
    public struct WithUpgradeableReadLock : IDisposable
    {
        private readonly ReaderWriterLockSlim TheLock;
        private bool Disposed;

        public void Dispose()
        {
            if (this.Disposed)
                return;
            this.Disposed = true;
            this.TheLock.ExitUpgradeableReadLock();
        }

        public WithUpgradeableReadLock(ReaderWriterLockSlim _lock)
          : this()
        {
            this.Disposed = false;
            this.TheLock = _lock;
            this.TheLock.EnterUpgradeableReadLock();
        }
    }
}
