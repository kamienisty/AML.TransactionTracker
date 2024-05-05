namespace AML.TransactionTracker.Core.Entities
{
    public class RuleViolation
    {
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public string RuleName { get; set; }
        public DateTime Date { get; set; }
        public string RuleExpression { get; set; }

        public RuleViolation(Guid transactionId, string ruleName, DateTime date, string ruleExpression)
        {
            Id = 0;
            TransactionId = transactionId;
            RuleName = ruleName;
            Date = date;
            RuleExpression = ruleExpression;
        }
    }
}
