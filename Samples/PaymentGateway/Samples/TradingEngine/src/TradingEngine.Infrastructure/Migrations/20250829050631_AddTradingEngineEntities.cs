using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTradingEngineEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "deliverrecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliverrecord", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Paid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<int>(type: "int", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiskControls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "风险控制ID", collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "用户ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalPositionLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "总持仓限制"),
                    DailyLossLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "日损失限制"),
                    CurrentPosition = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "当前持仓"),
                    DailyLoss = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "当日损失"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否激活"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    LastAssessmentAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "最后评估时间"),
                    RowVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskControls", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Settlements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "结算ID", collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "用户ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SettlementType = table.Column<int>(type: "int", nullable: false, comment: "结算类型：1-交易结算，2-分红结算，3-费用结算，4-保证金调整"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "结算总金额"),
                    SettlementDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "结算日期"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "结算状态：1-待处理，2-处理中，3-已完成，4-失败，5-已取消"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "处理时间"),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "完成时间"),
                    FailureReason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "失败原因")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RowVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlements", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "交易ID", collation: "ascii_general_ci"),
                    Symbol = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "交易标的")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TradeType = table.Column<int>(type: "int", nullable: false, comment: "交易类型：1-买入，2-卖出"),
                    Quantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "交易数量"),
                    Price = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "交易价格"),
                    ExecutedQuantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "已执行数量"),
                    RemainingQuantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "剩余数量"),
                    TotalValue = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "交易总值"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "交易状态：1-待处理，2-已执行，3-失败，4-已取消，5-部分成交"),
                    UserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "用户ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    ExecutedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "执行时间"),
                    FailureReason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "失败原因")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RowVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiskAssessments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "风险评估ID", collation: "ascii_general_ci"),
                    Symbol = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "交易标的")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "交易数量"),
                    Price = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "交易价格"),
                    TradeType = table.Column<int>(type: "int", nullable: false, comment: "交易类型：1-买入，2-卖出"),
                    RiskLevel = table.Column<int>(type: "int", nullable: false, comment: "风险等级：1-低，2-中，3-高，4-严重"),
                    RiskTypes = table.Column<string>(type: "longtext", nullable: false, comment: "风险类型列表")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false, comment: "风险描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssessedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "评估时间"),
                    RiskControlId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskAssessments_RiskControls_RiskControlId",
                        column: x => x.RiskControlId,
                        principalTable: "RiskControls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SettlementItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "结算项目ID", collation: "ascii_general_ci"),
                    ReferenceId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "关联引用ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Symbol = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "交易标的")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "数量"),
                    Price = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false, comment: "价格"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "结算金额"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    SettlementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettlementItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettlementItems_Settlements_SettlementId",
                        column: x => x.SettlementId,
                        principalTable: "Settlements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessments_RiskControlId",
                table: "RiskAssessments",
                column: "RiskControlId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControls_IsActive",
                table: "RiskControls",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControls_UserId",
                table: "RiskControls",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SettlementItems_SettlementId",
                table: "SettlementItems",
                column: "SettlementId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_SettlementDate",
                table: "Settlements",
                column: "SettlementDate");

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_SettlementType",
                table: "Settlements",
                column: "SettlementType");

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_Status",
                table: "Settlements",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_UserId",
                table: "Settlements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_CreatedAt",
                table: "Trades",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_Status",
                table: "Trades",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_Symbol",
                table: "Trades",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_UserId",
                table: "Trades",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deliverrecord");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "RiskAssessments");

            migrationBuilder.DropTable(
                name: "SettlementItems");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "RiskControls");

            migrationBuilder.DropTable(
                name: "Settlements");
        }
    }
}
