using System;
using System.Data;
using System.Data.Entity;

namespace Data.EF
{
    public interface IDatabaseTransaction : IDisposable
    {
        event EventHandler EndTransaction;

        void Commit();

        void Rollback();
    }

    public class DatabaseTransaction : IDatabaseTransaction
    {
        private bool transactionEndNotifaction;
        private readonly DbContextTransaction transaction;

        public event EventHandler EndTransaction;

        public DatabaseTransaction(DbContext context, IsolationLevel isolationLevel)
        {
            transactionEndNotifaction = false;
            transaction = context.Database.BeginTransaction(isolationLevel);
        }

        public DatabaseTransaction(DbContext context)
        {
            transactionEndNotifaction = false;
            transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
            OnEndTransaction();
        }

        public void Rollback()
        {
            transaction.Rollback();
            OnEndTransaction();
        }

        public void Dispose()
        {
            if (!transactionEndNotifaction)
                OnEndTransaction();
            transaction.Dispose();
        }

        protected virtual void OnEndTransaction()
        {
            if (transactionEndNotifaction) return;

            transactionEndNotifaction = true;

            EndTransaction?.Invoke(this, new EventArgs() { });
        }
    }
}