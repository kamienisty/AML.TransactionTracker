namespace AML.TransactionTracker.Core.Exceptions
{
    public class InvalidDateException : CoreExceptionBase
    {
        public override string Code => "incorrect_entity_date";

        public InvalidDateException(string message) : base(message)
        {
            
        }
    }
}
