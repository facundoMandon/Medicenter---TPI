using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigraciónTiziRama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Professionals_ProfessionalId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitalsProfessionals_Professionals_ProfessionalsId",
                table: "HospitalsProfessionals");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceProfessionals_Professionals_ProfessionalsId",
                table: "InsuranceProfessionals");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Professionals");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Insurance");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Specialties",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Insurance",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "CoverageType",
                table: "Insurance",
                newName: "TipoCobertura");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Appointments",
                newName: "Hora");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointments",
                newName: "Fecha");

            migrationBuilder.AddColumn<int>(
                name: "AffiliateNumber",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "varchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LicenseNumber",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Specialties",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Insurance",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Hospitals",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Hospitals",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Appointments",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InsuranceId",
                table: "Users",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SpecialtyId",
                table: "Users",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_ProfessionalId",
                table: "Appointments",
                column: "ProfessionalId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalsProfessionals_Users_ProfessionalsId",
                table: "HospitalsProfessionals",
                column: "ProfessionalsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceProfessionals_Users_ProfessionalsId",
                table: "InsuranceProfessionals",
                column: "ProfessionalsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Insurance_InsuranceId",
                table: "Users",
                column: "InsuranceId",
                principalTable: "Insurance",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Specialties_SpecialtyId",
                table: "Users",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_ProfessionalId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitalsProfessionals_Users_ProfessionalsId",
                table: "HospitalsProfessionals");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceProfessionals_Users_ProfessionalsId",
                table: "InsuranceProfessionals");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Insurance_InsuranceId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Specialties_SpecialtyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_InsuranceId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SpecialtyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AffiliateNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Insurance");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Specialties",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TipoCobertura",
                table: "Insurance",
                newName: "CoverageType");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Insurance",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Hora",
                table: "Appointments",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Appointments",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Specialties",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Insurance",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Hospitals",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Hospitals",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "Appointments",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administrators_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    InsuranceId = table.Column<int>(type: "int", nullable: false),
                    affiliate_number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Insurance_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Professionals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    n_matricula = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professionals_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Professionals_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patients",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_SpecialtyId",
                table: "Professionals",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Professionals_ProfessionalId",
                table: "Appointments",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalsProfessionals_Professionals_ProfessionalsId",
                table: "HospitalsProfessionals",
                column: "ProfessionalsId",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceProfessionals_Professionals_ProfessionalsId",
                table: "InsuranceProfessionals",
                column: "ProfessionalsId",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
