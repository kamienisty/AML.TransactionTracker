namespace AML.TransactionTracker.Application.Exceptions
{
    public class TransactionValidationException : AppExceptionBase
    {
        public override string Code => "tranaction_validatioin_exception";

        public TransactionValidationException(Guid transactionId, string message): base($"An exception occured while validating transaction {transactionId}: {message}")
        {
            
        }
    }
}
