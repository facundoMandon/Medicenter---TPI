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
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Professional_ProfessionalId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitalProfessional_Professional_ProfessionalId",
                table: "HospitalProfessional");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceProfessional_Professional_ProfessionalId",
                table: "InsuranceProfessional");

            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Professional");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Insurance");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Hospital");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Hospital");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Specialties",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Insurance",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CoverageType",
                table: "Insurance",
                newName: "MedicalCoverageType");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Appointment",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointment",
                newName: "Fecha");

            migrationBuilder.AddColumn<int>(
                name: "AffiliateNumber",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "User",
                type: "varchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LicenseNumber",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Specialties",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Insurance",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Hospital",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Hospital",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Appointment",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_InsuranceId",
                table: "User",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SpecialtyId",
                table: "User",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_ProfessionalId",
                table: "Appointment",
                column: "ProfessionalId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalProfessional_User_ProfessionalId",
                table: "HospitalProfessional",
                column: "ProfessionalId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceProfessional_User_ProfessionalId",
                table: "InsuranceProfessional",
                column: "ProfessionalId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Insurance_InsuranceId",
                table: "User",
                column: "InsuranceId",
                principalTable: "Insurance",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Specialties_SpecialtyId",
                table: "User",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_ProfessionalId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitalProfessional_User_ProfessionalId",
                table: "HospitalProfessional");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceProfessional_User_ProfessionalId",
                table: "InsuranceProfessional");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Insurance_InsuranceId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Specialties_SpecialtyId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_InsuranceId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_SpecialtyId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AffiliateNumber",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "User");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Insurance");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Hospital");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Hospital");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Specialties",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "MedicalCoverageType",
                table: "Insurance",
                newName: "CoverageType");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Insurance",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Appointment",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Appointment",
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
                name: "Address",
                table: "Hospital",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Hospital",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "Appointment",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administrator_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    InsuranceId = table.Column<int>(type: "int", nullable: false),
                    affiliate_number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Insurance_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Professional",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    n_matricula = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professional", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professional_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Professional_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_InsuranceId",
                table: "Patient",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_Professional_SpecialtyId",
                table: "Professional",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Professional_ProfessionalId",
                table: "Appointment",
                column: "ProfessionalId",
                principalTable: "Professional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalProfessional_Professional_ProfessionalId",
                table: "HospitalProfessional",
                column: "ProfessionalId",
                principalTable: "Professional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceProfessional_Professional_ProfessionalId",
                table: "InsuranceProfessional",
                column: "ProfessionalId",
                principalTable: "Professional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
