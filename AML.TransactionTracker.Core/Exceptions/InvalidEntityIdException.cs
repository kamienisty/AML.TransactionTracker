namespace AML.TransactionTracker.Core.Exceptions
{
    public class InvalidEntityIdException : CoreExceptionBase
    {
        public override string Code => "invalid_entity_id";
        public InvalidEntityIdException(Guid id) : base($"Invalid entity id: {id}")
        {
        }        
    }
}
