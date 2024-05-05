namespace AML.TransactionTracker.Application.Exceptions
{
    public class EmptyParametersException : AppExceptionBase
    {
        public override string Code => "empty_parameters";
        public EmptyParametersException(string message) : base(message)
        {

        }
    }
}
