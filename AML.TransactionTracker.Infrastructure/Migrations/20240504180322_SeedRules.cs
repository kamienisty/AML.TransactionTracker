using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AML.TransactionTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Workflows",
                columns: new[] { "WorkflowName", "RuleExpressionType" },
                values: new object[] { "TransactionValidation", 0 });

            migrationBuilder.InsertData(
                table: "Rules",
                columns: new[] { "RuleName", "Properties", "Operator", "ErrorMessage", "Enabled", "RuleExpressionType",
                    "Expression", "Actions", "SuccessEvent", "RuleNameFK", "WorkflowName"},
                values: new object[,] { 
                    {
                        "AmountGreaterThan", "{}", null, "Transacition amount exceeded warning threshold",
                        true, 0, "input1.Amount < 1000 AND input1.TransactionCurrency == 0 OR  input1.TransactionCurrency == 1 AND input1.Amount < 1200 OR  input1.TransactionCurrency == 2 AND input1.Amount < 4000",
                        "{}", "1", null, "TransactionValidation" 
                    },
                    {
                        "AmountOfTransactions", "{}", null, "Transaction number exceeded warning threshold",
                        true, 0, "input2.GetNumberOfTransactionsSinceDate(input1.Date.AddMinutes(-3), input1.CustomerId).Result < 10",
                        "{}", "1", null, "TransactionValidation"
                    },

                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "RuleName",
                keyValues: new[] { "AmountGreaterThan", "AmountOfTransactions" });

            migrationBuilder.DeleteData(
                table: "Workflows",
                keyColumn: "WorkflowName",
                keyValue: "TransactionValidation");
        }
    }
}
