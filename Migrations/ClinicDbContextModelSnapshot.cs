﻿// <auto-generated />
using System;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Сlinic.Migrations
{
    [DbContext(typeof(ClinicDbContext))]
    partial class ClinicDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Clinic.Models.DisabilitySheet", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<int?>("DoctorId")
                        .HasColumnType("integer")
                        .HasColumnName("Doctor_ID");

                    b.Property<DateOnly?>("IssueDate")
                        .HasColumnType("date")
                        .HasColumnName("Issue_Date");

                    b.Property<string>("SheetNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Sheet_Number");

                    b.HasKey("Id")
                        .HasName("PK__Disabili__3214EC278250AC62");

                    b.HasIndex("DoctorId");

                    b.ToTable("Disability_Sheets", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<int?>("DistrictNumber")
                        .HasColumnType("integer")
                        .HasColumnName("District_Number");

                    b.Property<int?>("DoctorId")
                        .HasColumnType("integer")
                        .HasColumnName("Doctor_ID");

                    b.HasKey("Id")
                        .HasName("PK__District__3214EC2766EC7D4F");

                    b.HasIndex("DoctorId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("Clinic.Models.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("First_Name");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Last_Name");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Middle_Name");

                    b.Property<int?>("RegistryId")
                        .HasColumnType("integer")
                        .HasColumnName("Registry_ID");

                    b.Property<int?>("SpecialtyId")
                        .HasColumnType("integer")
                        .HasColumnName("Specialty_ID");

                    b.HasKey("Id")
                        .HasName("PK__Doctors__3214EC27DF58AED1");

                    b.HasIndex("RegistryId");

                    b.HasIndex("SpecialtyId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Clinic.Models.DoctorSpecialty", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasName("PK__Doctor_S__3214EC270DD9AA0F");

                    b.ToTable("Doctor_Specialties", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.MedicalDocument", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int?>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("Patient_ID");

                    b.HasKey("Id")
                        .HasName("PK__Medical___3214EC27A387931C");

                    b.HasIndex("PatientId");

                    b.ToTable("Medical_Documents", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.OutpatientCard", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("CardNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Card_Number");

                    b.Property<int?>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("Patient_ID");

                    b.HasKey("Id")
                        .HasName("PK__Outpatie__3214EC27A8BDBD3E");

                    b.HasIndex("PatientId");

                    b.ToTable("Outpatient_Cards", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("Birth_Date");

                    b.Property<int?>("DistrictId")
                        .HasColumnType("integer")
                        .HasColumnName("District_ID");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("First_Name");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Last_Name");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Middle_Name");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id")
                        .HasName("PK__Patients__3214EC27ADEE42F9");

                    b.HasIndex("DistrictId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Clinic.Models.ReasonsForVisit", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("Patient_ID");

                    b.HasKey("Id")
                        .HasName("PK__Reasons___3214EC274C5AA7C7");

                    b.HasIndex("PatientId");

                    b.ToTable("Reasons_For_Visit", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.RecordingMethod", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasName("PK__Recordin__3214EC27A150B754");

                    b.ToTable("Recording_Methods", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.Registrar", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("First_Name");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Last_Name");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Middle_Name");

                    b.Property<int?>("RegistryId")
                        .HasColumnType("integer")
                        .HasColumnName("Registry_ID");

                    b.HasKey("Id")
                        .HasName("PK__Registra__3214EC27A6DC454C");

                    b.HasIndex("RegistryId");

                    b.ToTable("Registrars");
                });

            modelBuilder.Entity("Clinic.Models.Registry", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("City")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int?>("DepartmentNumber")
                        .HasColumnType("integer")
                        .HasColumnName("Department_Number");

                    b.Property<string>("Head")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("HouseNumber")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("House_Number");

                    b.Property<string>("OrganizationType")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Organization_Type");

                    b.Property<int?>("RecordingMethodId")
                        .HasColumnType("integer")
                        .HasColumnName("Recording_Method_ID");

                    b.Property<string>("Street")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SubdivisionName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Subdivision_Name");

                    b.HasKey("Id")
                        .HasName("PK__Registry__3214EC27E465FD10");

                    b.HasIndex("RecordingMethodId");

                    b.ToTable("Registry", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id")
                        .HasName("PK__Role__3214EC0787B9E1CB");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<string>("DayOfWeek")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Day_Of_Week");

                    b.Property<int?>("DoctorId")
                        .HasColumnType("integer")
                        .HasColumnName("Doctor_ID");

                    b.Property<TimeOnly?>("EndTime")
                        .HasColumnType("time without time zone")
                        .HasColumnName("End_Time");

                    b.Property<TimeOnly?>("StartTime")
                        .HasColumnType("time without time zone")
                        .HasColumnName("Start_Time");

                    b.HasKey("Id")
                        .HasName("PK__Schedule__3214EC27230BF89A");

                    b.HasIndex("DoctorId");

                    b.ToTable("Schedule", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    b.Property<DateOnly?>("AppointmentDate")
                        .HasColumnType("date")
                        .HasColumnName("Appointment_Date");

                    b.Property<TimeOnly?>("AppointmentTime")
                        .HasColumnType("time without time zone")
                        .HasColumnName("Appointment_Time");

                    b.Property<int?>("DoctorId")
                        .HasColumnType("integer")
                        .HasColumnName("Doctor_ID");

                    b.Property<int?>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("Patient_ID");

                    b.HasKey("Id")
                        .HasName("PK__Tickets__3214EC27F0C49F75");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Clinic.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("PK__User__3214EC0796F447B5");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Clinic.Models.DisabilitySheet", b =>
                {
                    b.HasOne("Clinic.Models.Doctor", "Doctor")
                        .WithMany("DisabilitySheets")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Disability_Sheets_Doctors");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Clinic.Models.District", b =>
                {
                    b.HasOne("Clinic.Models.Doctor", "Doctor")
                        .WithMany("Districts")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Districts_Doctors");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Clinic.Models.Doctor", b =>
                {
                    b.HasOne("Clinic.Models.Registry", "Registry")
                        .WithMany("Doctors")
                        .HasForeignKey("RegistryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Doctors_Registry");

                    b.HasOne("Clinic.Models.DoctorSpecialty", "Specialty")
                        .WithMany("Doctors")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Doctors_DoctorSpecialties");

                    b.Navigation("Registry");

                    b.Navigation("Specialty");
                });

            modelBuilder.Entity("Clinic.Models.MedicalDocument", b =>
                {
                    b.HasOne("Clinic.Models.Patient", "Patient")
                        .WithMany("MedicalDocuments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Medical_Documents_Patients");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Clinic.Models.OutpatientCard", b =>
                {
                    b.HasOne("Clinic.Models.Patient", "Patient")
                        .WithMany("OutpatientCards")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Outpatient_Cards_Patients");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Clinic.Models.Patient", b =>
                {
                    b.HasOne("Clinic.Models.District", "District")
                        .WithMany("Patients")
                        .HasForeignKey("DistrictId")
                        .HasConstraintName("FK_Patients_Districts");

                    b.Navigation("District");
                });

            modelBuilder.Entity("Clinic.Models.ReasonsForVisit", b =>
                {
                    b.HasOne("Clinic.Models.Patient", "Patient")
                        .WithMany("ReasonsForVisits")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Reasons_For_Visit_Patients");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Clinic.Models.Registrar", b =>
                {
                    b.HasOne("Clinic.Models.Registry", "Registry")
                        .WithMany("Registrars")
                        .HasForeignKey("RegistryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Registrars_Registry");

                    b.Navigation("Registry");
                });

            modelBuilder.Entity("Clinic.Models.Registry", b =>
                {
                    b.HasOne("Clinic.Models.RecordingMethod", "RecordingMethod")
                        .WithMany("Registries")
                        .HasForeignKey("RecordingMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Registry_Recording_Methods");

                    b.Navigation("RecordingMethod");
                });

            modelBuilder.Entity("Clinic.Models.Schedule", b =>
                {
                    b.HasOne("Clinic.Models.Doctor", "Doctor")
                        .WithMany("Schedules")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Schedule_Doctors");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Clinic.Models.Ticket", b =>
                {
                    b.HasOne("Clinic.Models.Doctor", "Doctor")
                        .WithMany("Tickets")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Tickets_Doctors");

                    b.HasOne("Clinic.Models.Patient", "Patient")
                        .WithMany("Tickets")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Tickets_Patients");

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Clinic.Models.User", b =>
                {
                    b.HasOne("Clinic.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__User__RoleId__19DFD96B");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Clinic.Models.District", b =>
                {
                    b.Navigation("Patients");
                });

            modelBuilder.Entity("Clinic.Models.Doctor", b =>
                {
                    b.Navigation("DisabilitySheets");

                    b.Navigation("Districts");

                    b.Navigation("Schedules");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Clinic.Models.DoctorSpecialty", b =>
                {
                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("Clinic.Models.Patient", b =>
                {
                    b.Navigation("MedicalDocuments");

                    b.Navigation("OutpatientCards");

                    b.Navigation("ReasonsForVisits");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Clinic.Models.RecordingMethod", b =>
                {
                    b.Navigation("Registries");
                });

            modelBuilder.Entity("Clinic.Models.Registry", b =>
                {
                    b.Navigation("Doctors");

                    b.Navigation("Registrars");
                });

            modelBuilder.Entity("Clinic.Models.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
