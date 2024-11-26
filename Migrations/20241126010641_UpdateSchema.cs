using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Сlinic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctor_Specialties",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Doctor_S__3214EC270DD9AA0F", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Recording_Methods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Recordin__3214EC27A150B754", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3214EC0787B9E1CB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registry",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Subdivision_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Department_Number = table.Column<int>(type: "integer", nullable: true),
                    Head = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Organization_Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    House_Number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Recording_Method_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registry__3214EC27E465FD10", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Registry__Record__66603565",
                        column: x => x.Recording_Method_ID,
                        principalTable: "Recording_Methods",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3214EC0796F447B5", x => x.Id);
                    table.ForeignKey(
                        name: "FK__User__RoleId__19DFD96B",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Last_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    First_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Middle_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Specialty_ID = table.Column<int>(type: "integer", nullable: true),
                    Registry_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Doctors__3214EC27DF58AED1", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Doctors__Departm__5CD6CB2B",
                        column: x => x.Registry_ID,
                        principalTable: "Registry",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__Doctors__Special__5DCAEF64",
                        column: x => x.Specialty_ID,
                        principalTable: "Doctor_Specialties",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Registrars",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Last_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    First_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Middle_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Registry_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registra__3214EC27A6DC454C", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Registrar__Regis__656C112C",
                        column: x => x.Registry_ID,
                        principalTable: "Registry",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Disability_Sheets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Sheet_Number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Issue_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Doctor_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Disabili__3214EC278250AC62", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Disabilit__Docto__5EBF139D",
                        column: x => x.Doctor_ID,
                        principalTable: "Doctors",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    District_Number = table.Column<int>(type: "integer", nullable: true),
                    Doctor_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__District__3214EC2766EC7D4F", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Districts__Docto__68487DD7",
                        column: x => x.Doctor_ID,
                        principalTable: "Doctors",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Doctor_ID = table.Column<int>(type: "integer", nullable: true),
                    Day_Of_Week = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Start_Time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    End_Time = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedule__3214EC27230BF89A", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Schedule__Doctor__6477ECF3",
                        column: x => x.Doctor_ID,
                        principalTable: "Doctors",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Doctor_ID = table.Column<int>(type: "integer", nullable: true),
                    Appointment_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Appointment_Time = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tickets__3214EC27F0C49F75", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Tickets__Doctor___6754599E",
                        column: x => x.Doctor_ID,
                        principalTable: "Doctors",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Last_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    First_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Middle_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Birth_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    District_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Patients__3214EC27ADEE42F9", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Patients__Distri__628FA481",
                        column: x => x.District_ID,
                        principalTable: "Districts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Doctor_ID = table.Column<int>(type: "integer", nullable: true),
                    Patient_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Appointm__3214EC2710CC7DFD", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Appointme__Docto__60A75C0F",
                        column: x => x.Doctor_ID,
                        principalTable: "Doctors",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__Appointme__Patie__619B8048",
                        column: x => x.Patient_ID,
                        principalTable: "Patients",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Medical_Documents",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Patient_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medical___3214EC27A387931C", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Medical_D__Patie__5FB337D6",
                        column: x => x.Patient_ID,
                        principalTable: "Patients",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Outpatient_Cards",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Card_Number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Patient_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Outpatie__3214EC27A8BDBD3E", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Outpatien__Patie__5BE2A6F2",
                        column: x => x.Patient_ID,
                        principalTable: "Patients",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Reasons_For_Visit",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Patient_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reasons___3214EC274C5AA7C7", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Reasons_F__Patie__6383C8BA",
                        column: x => x.Patient_ID,
                        principalTable: "Patients",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Doctor_ID",
                table: "Appointments",
                column: "Doctor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Patient_ID",
                table: "Appointments",
                column: "Patient_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Disability_Sheets_Doctor_ID",
                table: "Disability_Sheets",
                column: "Doctor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Doctor_ID",
                table: "Districts",
                column: "Doctor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Registry_ID",
                table: "Doctors",
                column: "Registry_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Specialty_ID",
                table: "Doctors",
                column: "Specialty_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Medical_Documents_Patient_ID",
                table: "Medical_Documents",
                column: "Patient_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Outpatient_Cards_Patient_ID",
                table: "Outpatient_Cards",
                column: "Patient_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_District_ID",
                table: "Patients",
                column: "District_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reasons_For_Visit_Patient_ID",
                table: "Reasons_For_Visit",
                column: "Patient_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Registrars_Registry_ID",
                table: "Registrars",
                column: "Registry_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Registry_Recording_Method_ID",
                table: "Registry",
                column: "Recording_Method_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Doctor_ID",
                table: "Schedule",
                column: "Doctor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Doctor_ID",
                table: "Tickets",
                column: "Doctor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Disability_Sheets");

            migrationBuilder.DropTable(
                name: "Medical_Documents");

            migrationBuilder.DropTable(
                name: "Outpatient_Cards");

            migrationBuilder.DropTable(
                name: "Reasons_For_Visit");

            migrationBuilder.DropTable(
                name: "Registrars");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Registry");

            migrationBuilder.DropTable(
                name: "Doctor_Specialties");

            migrationBuilder.DropTable(
                name: "Recording_Methods");
        }
    }
}
