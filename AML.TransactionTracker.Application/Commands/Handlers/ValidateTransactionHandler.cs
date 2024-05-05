using AML.TransactionTracker.Application.Exceptions;
using AML.TransactionTracker.Core.Repositories;
using MediatR;

namespace AML.TransactionTracker.Application.Commands.Handlers
{
    public class ValidateTransactionHandler : HandlerBase<ITransactionRepository>, IRequestHandler<ValidateTransaction>
    {
        public const string ValidationWorkflowName = "TransactionValidation";
        public ValidateTransactionHandler(ITransactionRepository repository) : base(repository)
        {

        }

        public async Task Handle(ValidateTransaction request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Transaction validatoin for {request.TransactionId}");
            var transaction = await _repository.GetAsync(request.TransactionId);

            if (transaction == null || transaction.Validated)
            {
                return;
            }

            var workflow = await _repository.GetRulesForTransactionAsync(ValidationWorkflowName);

            try
            {
                if (workflow.Any(x => x.WorkflowName == ValidationWorkflowName && x.Rules.Any()))
                {
                    var re = new RulesEngine.RulesEngine(workflow.ToArray(), null);
                    var resultList = await re.ExecuteAllRulesAsync(ValidationWorkflowName, transaction, _repository);

                    if (resultList.Any(x => !x.IsSuccess))
                    {
                        var errors = string.Join(',', resultList.Where(x => !x.IsSuccess).Select(x => x.ExceptionMessage));
                        transaction.FlaggedReason = errors;
                        transaction.Flagged = true;
                    }
                }

                transaction.Validated = true;
                await _repository.UpdateAsync(transaction);

            }
            catch (Exception e)
            {
                throw new TransactionValidationException(transaction.Id, e.Message);
            }
        }
    }
}
