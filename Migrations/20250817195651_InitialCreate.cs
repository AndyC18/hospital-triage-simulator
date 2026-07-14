using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace realNEA.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SymptomQuestions",
                columns: table => new
                {
                    SymptomQuestionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Question = table.Column<string>(type: "TEXT", nullable: true),
                    SeverityScore = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomQuestions", x => x.SymptomQuestionId);
                });

            migrationBuilder.CreateTable(
                name: "SymptomAnswers",
                columns: table => new
                {
                    SymptomAnswerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SymptomQuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    NextQuestionID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomAnswers", x => x.SymptomAnswerId);
                    table.ForeignKey(
                        name: "FK_SymptomAnswers_SymptomQuestions_NextQuestionID",
                        column: x => x.NextQuestionID,
                        principalTable: "SymptomQuestions",
                        principalColumn: "SymptomQuestionId");
                    table.ForeignKey(
                        name: "FK_SymptomAnswers_SymptomQuestions_SymptomQuestionId",
                        column: x => x.SymptomQuestionId,
                        principalTable: "SymptomQuestions",
                        principalColumn: "SymptomQuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SymptomAnswers_NextQuestionID",
                table: "SymptomAnswers",
                column: "NextQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomAnswers_SymptomQuestionId",
                table: "SymptomAnswers",
                column: "SymptomQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SymptomAnswers");

            migrationBuilder.DropTable(
                name: "SymptomQuestions");
        }
    }
}
