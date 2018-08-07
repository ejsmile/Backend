using System;
using System.Data;

namespace Data.EF
{
    public static class DatabaseExtensions
    {
        public static T ExecuteInTransaction<T>(this DatabaseContext context, Func<T> act) => ExecuteInTransaction(context, IsolationLevel.Snapshot, act);

        public static T ExecuteInTransaction<T>(this DatabaseContext context, IsolationLevel isolationLevel, Func<T> act)
        {
            using (var transaction = context.BeginTransaction(isolationLevel))
            {
                try
                {
                    var result = act.Invoke();
                    context.SaveChanges();
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public static void ExecuteInTransaction(this DatabaseContext context, Action act) => ExecuteInTransaction(context, IsolationLevel.Snapshot, act);

        public static void ExecuteInTransaction(this DatabaseContext context, IsolationLevel isolationLevel, Action act)
        {
            using (var transaction = context.BeginTransaction(isolationLevel))
            {
                try
                {
                    act.Invoke();
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}