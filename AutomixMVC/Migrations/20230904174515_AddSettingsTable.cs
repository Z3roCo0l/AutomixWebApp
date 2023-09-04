using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AutomixMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_FoodItems_FoodId",
                table: "BasketItem");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FoodItems",
                newName: "FoodId");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FoodId",
                table: "BasketItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[] { 1, "DefaultMenuPrice", "1490.0" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "qKCJcAPHtLQakTUAfhEAKgSQMDlt+blLGQM5wsbTBEFw/8woHj2SH4GKZu8FxR1I9ZQZykSQp5VFp3IoSBy3Rg==");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_FoodItems_FoodId",
                table: "BasketItem",
                column: "FoodId",
                principalTable: "FoodItems",
                principalColumn: "FoodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_FoodItems_FoodId",
                table: "BasketItem");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.RenameColumn(
                name: "FoodId",
                table: "FoodItems",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<int>(
                name: "FoodId",
                table: "BasketItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "E9f9VQvS8f6QmILyH2iiMtiJFitEjHhNLGJ4l3Foi0cQg3vFVIhm4VBMRYBzMrJUxb3syJqmK62HP33/0e/VIA==");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_FoodItems_FoodId",
                table: "BasketItem",
                column: "FoodId",
                principalTable: "FoodItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
