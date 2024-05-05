using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AML.TransactionTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRulesEngine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    WorkflowName = table.Column<string>(type: "TEXT", nullable: false),
                    RuleExpressionType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.WorkflowName);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    RuleName = table.Column<string>(type: "TEXT", nullable: false),
                    Properties = table.Column<string>(type: "TEXT", nullable: true),
                    Operator = table.Column<string>(type: "TEXT", nullable: true),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    RuleExpressionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Expression = table.Column<string>(type: "TEXT", nullable: true),
                    Actions = table.Column<string>(type: "TEXT", nullable: true),
                    SuccessEvent = table.Column<string>(type: "TEXT", nullable: true),
                    RuleNameFK = table.Column<string>(type: "TEXT", nullable: true),
                    WorkflowName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.RuleName);
                    table.ForeignKey(
                        name: "FK_Rules_Rules_RuleNameFK",
                        column: x => x.RuleNameFK,
                        principalTable: "Rules",
                        principalColumn: "RuleName");
                    table.ForeignKey(
                        name: "FK_Rules_Workflows_WorkflowName",
                        column: x => x.WorkflowName,
                        principalTable: "Workflows",
                        principalColumn: "WorkflowName");
                });

            migrationBuilder.CreateTable(
                name: "ScopedParam",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Expression = table.Column<string>(type: "TEXT", nullable: true),
                    RuleName = table.Column<string>(type: "TEXT", nullable: true),
                    WorkflowName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopedParam", x => x.Name);
                    table.ForeignKey(
                        name: "FK_ScopedParam_Rules_RuleName",
                        column: x => x.RuleName,
                        principalTable: "Rules",
                        principalColumn: "RuleName");
                    table.ForeignKey(
                        name: "FK_ScopedParam_Workflows_WorkflowName",
                        column: x => x.WorkflowName,
                        principalTable: "Workflows",
                        principalColumn: "WorkflowName");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rules_RuleNameFK",
                table: "Rules",
                column: "RuleNameFK");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_WorkflowName",
                table: "Rules",
                column: "WorkflowName");

            migrationBuilder.CreateIndex(
                name: "IX_ScopedParam_RuleName",
                table: "ScopedParam",
                column: "RuleName");

            migrationBuilder.CreateIndex(
                name: "IX_ScopedParam_WorkflowName",
                table: "ScopedParam",
                column: "WorkflowName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScopedParam");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Workflows");
        }
    }
}
