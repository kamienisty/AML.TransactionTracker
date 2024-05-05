namespace AML.TransactionTracker.Core.Exceptions
{
    public abstract class CoreExceptionBase: Exception
    {
        public abstract string Code { get;  }
        protected CoreExceptionBase(string message) : base(message)
        {

        }
    }
}
