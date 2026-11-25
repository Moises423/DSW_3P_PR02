using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DSW_3P_PR02.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Videojuego = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "Latin1_General_100_CI_AS"),
                    Clase = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    Poder = table.Column<int>(type: "int", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ImagenUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personajes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Personajes",
                columns: new[] { "Id", "Clase", "Disponible", "ImagenUrl", "Nivel", "Nombre", "Poder", "Videojuego" },
                values: new object[,]
                {
                    { 1, "Exploradora", true, null, 95, "Lara Croft", 90, "Tomb Raider" },
                    { 2, "Espadachín", true, null, 85, "Link", 88, "The Legend of Zelda" },
                    { 3, "Cazarrecompensas", true, null, 92, "Samus Aran", 96, "Metroid" },
                    { 4, "Brujo", true, null, 90, "Geralt de Rivia", 93, "The Witcher 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personajes");
        }
    }
}
