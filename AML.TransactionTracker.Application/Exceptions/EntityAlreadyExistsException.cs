namespace AML.TransactionTracker.Application.Exceptions
{
    public class EntityAlreadyExistsException : AppExceptionBase
    {
        public override string Code => "entity_already_exists";
        public EntityAlreadyExistsException(Guid id, Type type) : base($"Entity {type} with Id: {id} already exists")
        {
        }
    }
}
