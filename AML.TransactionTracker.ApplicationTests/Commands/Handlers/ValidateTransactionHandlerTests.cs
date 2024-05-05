using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Commands.Handlers;
using AML.TransactionTracker.Core.Entities;
using AML.TransactionTracker.Core.Repositories;
using Moq;
using NUnit.Framework;
using RulesEngine.Models;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.ApplicationTests.Commands.Handlers
{
    public class ValidateTransactionHandlerTests
    {
        private Mock<ITransactionRepository> _repositoryMock = new Mock<ITransactionRepository>();

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ITransactionRepository>();
        }

        [Test]
        public async Task Handle_DoesNotProcessTransaction_WhenTransactionIsAlreadyValidated()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da3");

            var command = new ValidateTransaction(id);
            var transaction = new Transaction(id, 100, Currency.USD, TransactionType.Deposit, Guid.NewGuid(),
                DateTime.Now, "description", false, true);

            _repositoryMock.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(transaction));

            var handler = new ValidateTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.GetRulesForTransactionAsync(It.IsAny<string>()), Times.Never);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Test]
        public async Task Handle_DoesNotProcessTransaction_WhenTransactionDoesNotExists()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da3");

            var command = new ValidateTransaction(id);
           
            _repositoryMock.Setup(x => x.GetAsync(id)).Returns(Task.FromResult((Transaction)null));

            var handler = new ValidateTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.GetRulesForTransactionAsync(It.IsAny<string>()), Times.Never);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Test]
        public async Task Handle_MarksTransactionAsValidated_WhenThereIsNoWorkflow()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da3");

            var command = new ValidateTransaction(id);
            var transaction = new Transaction(id, 100, Currency.USD, TransactionType.Deposit, Guid.NewGuid(),
                DateTime.Now, "description");

            _repositoryMock.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(transaction));
            _repositoryMock.Setup(x => x.GetRulesForTransactionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Workflow>().AsEnumerable()));

            var handler = new ValidateTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.GetRulesForTransactionAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.Is<Transaction>( x => x.Id == id && x.Validated)), Times.Once);
        }


        [Test]
        public async Task Handle_MarksTransactionAsValidated_WhenThereAreNoRules()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da3");

            var command = new ValidateTransaction(id);
            var transaction = new Transaction(id, 100, Currency.USD, TransactionType.Deposit, Guid.NewGuid(),
                DateTime.Now, "description");

            _repositoryMock.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(transaction));
            _repositoryMock.Setup(x => x.GetRulesForTransactionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(
                    new List<Workflow> 
                        { 
                            new Workflow { 
                                WorkflowName = ValidateTransactionHandler.ValidationWorkflowName,
                                RuleExpressionType = RuleExpressionType.LambdaExpression,
                                Rules = new List<Rule>()
                            } 
                        }.AsEnumerable()));

            var handler = new ValidateTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.GetRulesForTransactionAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(
                It.Is<Transaction>(
                    x => x.Id == id && 
                        x.Validated && 
                        !x.Flagged &&
                        string.IsNullOrEmpty(x.FlaggedReason))),
                Times.Once);
        }

        [Test]
        public async Task Handle_TransactionIsNotFlagged_WhenRulesAreSuccessfull()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da3");

            var command = new ValidateTransaction(id);
            var transaction = new Transaction(id, 100, Currency.USD, TransactionType.Deposit, Guid.NewGuid(),
                DateTime.Now, "description");

            _repositoryMock.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(transaction));
            _repositoryMock.Setup(x => x.GetRulesForTransactionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(
                    new List<Workflow>
                        {
                            new Workflow {
                                WorkflowName = ValidateTransactionHandler.ValidationWorkflowName,
                                RuleExpressionType = RuleExpressionType.LambdaExpression,
                                Rules = new List<Rule>
                                {
                                    new Rule
                                    {
                                        RuleName = "rule1",
                                        ErrorMessage = "error",
                                        Expression = " 1 == 1"
                                    }
                                }
                            }
                        }.AsEnumerable()));

            var handler = new ValidateTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.GetRulesForTransactionAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(
                It.Is<Transaction>(
                    x => x.Id == id &&
                        x.Validated &&
                        !x.Flagged &&
                        string.IsNullOrEmpty(x.FlaggedReason))),
                Times.Once);
            _repositoryMock.Verify(x => x.AddRuleViolationsAsync(It.IsAny<IEnumerable<RuleViolation>>()), Times.Never);
        }

        [Test]
        public async Task Handle_MarksTransactionAsFlagged_WhenRulesFail()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da3");

            var command = new ValidateTransaction(id);
            var transaction = new Transaction(id, 100, Currency.USD, TransactionType.Deposit, Guid.NewGuid(),
                DateTime.Now, "description");

            _repositoryMock.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(transaction));
            _repositoryMock.Setup(x => x.GetRulesForTransactionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(
                    new List<Workflow>
                        {
                            new Workflow {
                                WorkflowName = ValidateTransactionHandler.ValidationWorkflowName,
                                RuleExpressionType = RuleExpressionType.LambdaExpression,
                                Rules = new List<Rule>
                                {
                                    new Rule
                                    {
                                        RuleName = "rule1",
                                        ErrorMessage = "error",
                                        Expression = " 1 != 1"
                                    }
                                }
                            }
                        }.AsEnumerable()));

            var handler = new ValidateTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.GetRulesForTransactionAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(
                It.Is<Transaction>(
                    x => x.Id == id &&
                        x.Validated &&
                        x.Flagged &&
                        x.FlaggedReason == "error")),
                Times.Once);
            _repositoryMock.Verify(x => x.AddRuleViolationsAsync(It.Is<IEnumerable<RuleViolation>>(y => y.Count() == 1)), Times.Once);
        }

        [Test]
        public async Task Handle_CorrectlyFillsFlaggedReason_WhenMoreThanOneRuleFails()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da3");

            var command = new ValidateTransaction(id);
            var transaction = new Transaction(id, 100, Currency.USD, TransactionType.Deposit, Guid.NewGuid(),
                DateTime.Now, "description");

            _repositoryMock.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(transaction));
            _repositoryMock.Setup(x => x.GetRulesForTransactionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(
                    new List<Workflow>
                        {
                            new Workflow {
                                WorkflowName = ValidateTransactionHandler.ValidationWorkflowName,
                                RuleExpressionType = RuleExpressionType.LambdaExpression,
                                Rules = new List<Rule>
                                {
                                    new Rule
                                    {
                                        RuleName = "rule1",
                                        ErrorMessage = "error1",
                                        Expression = " 1 != 1"
                                    },
                                    new Rule
                                    {
                                        RuleName = "rule2",
                                        ErrorMessage = "error2",
                                        Expression = " 1 != 1"
                                    },
                                    new Rule
                                    {
                                        RuleName = "rule3",
                                        ErrorMessage = "error3",
                                        Expression = " 1 == 1"
                                    }
                                }
                            }
                        }.AsEnumerable()));

            var handler = new ValidateTransactionHandler(_repositoryMock.Object);

            await handler.Handle(command, new CancellationToken());

            _repositoryMock.Verify(x => x.GetRulesForTransactionAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(
                It.Is<Transaction>(
                    x => x.Id == id &&
                        x.Validated &&
                        x.Flagged &&
                        x.FlaggedReason == "error1,error2")),
                Times.Once);
            _repositoryMock.Verify(x => x.AddRuleViolationsAsync(It.Is<IEnumerable<RuleViolation>>(y => y.Count() == 2)), Times.Once);
        }
    }
}
