using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Models;

public partial class ClinicDbContext : DbContext
{
    public ClinicDbContext()
    {
    }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<DisabilitySheet> DisabilitySheets { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }

    public virtual DbSet<MedicalDocument> MedicalDocuments { get; set; }

    public virtual DbSet<OutpatientCard> OutpatientCards { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<ReasonsForVisit> ReasonsForVisits { get; set; }

    public virtual DbSet<RecordingMethod> RecordingMethods { get; set; }

    public virtual DbSet<Registrar> Registrars { get; set; }

    public virtual DbSet<Registry> Registries { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-KEH7DQA;Database=ClinicDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC2710CC7DFD");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DoctorId).HasColumnName("Doctor_ID");
            entity.Property(e => e.PatientId).HasColumnName("Patient_ID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Appointments_Doctors");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Appointments_Patients");
        });

        modelBuilder.Entity<DisabilitySheet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Disabili__3214EC278250AC62");

            entity.ToTable("Disability_Sheets");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DoctorId).HasColumnName("Doctor_ID");
            entity.Property(e => e.IssueDate).HasColumnName("Issue_Date");
            entity.Property(e => e.SheetNumber)
                .HasMaxLength(20)
                .HasColumnName("Sheet_Number");

            entity.HasOne(d => d.Doctor).WithMany(p => p.DisabilitySheets)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Disability_Sheets_Doctors");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__3214EC2766EC7D4F");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DistrictNumber).HasColumnName("District_Number");
            entity.Property(e => e.DoctorId).HasColumnName("Doctor_ID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Districts)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Districts_Doctors");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Doctors__3214EC27DF58AED1");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("Middle_Name");
            entity.Property(e => e.RegistryId).HasColumnName("Registry_ID");
            entity.Property(e => e.SpecialtyId).HasColumnName("Specialty_ID");

            entity.HasOne(d => d.Registry).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.RegistryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Doctors_Registry");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Doctors_DoctorSpecialties");
        });

        modelBuilder.Entity<DoctorSpecialty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Doctor_S__3214EC270DD9AA0F");

            entity.ToTable("Doctor_Specialties");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<MedicalDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medical___3214EC27A387931C");

            entity.ToTable("Medical_Documents");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PatientId).HasColumnName("Patient_ID");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalDocuments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Medical_Documents_Patients");
        });

        modelBuilder.Entity<OutpatientCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Outpatie__3214EC27A8BDBD3E");

            entity.ToTable("Outpatient_Cards");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(20)
                .HasColumnName("Card_Number");
            entity.Property(e => e.PatientId).HasColumnName("Patient_ID");

            entity.HasOne(d => d.Patient).WithMany(p => p.OutpatientCards)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Outpatient_Cards_Patients");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Patients__3214EC27ADEE42F9");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.BirthDate).HasColumnName("Birth_Date");
            entity.Property(e => e.DistrictId).HasColumnName("District_ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("Middle_Name");
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.District).WithMany(p => p.Patients)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_Patients_Districts");
        });

        modelBuilder.Entity<ReasonsForVisit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reasons___3214EC274C5AA7C7");

            entity.ToTable("Reasons_For_Visit");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.PatientId).HasColumnName("Patient_ID");

            entity.HasOne(d => d.Patient).WithMany(p => p.ReasonsForVisits)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Reasons_For_Visit_Patients");
        });

        modelBuilder.Entity<RecordingMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recordin__3214EC27A150B754");

            entity.ToTable("Recording_Methods");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Registrar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registra__3214EC27A6DC454C");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("Middle_Name");
            entity.Property(e => e.RegistryId).HasColumnName("Registry_ID");

            entity.HasOne(d => d.Registry).WithMany(p => p.Registrars)
                .HasForeignKey(d => d.RegistryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Registrars_Registry");
        });

        modelBuilder.Entity<Registry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registry__3214EC27E465FD10");

            entity.ToTable("Registry");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.DepartmentNumber).HasColumnName("Department_Number");
            entity.Property(e => e.Head).HasMaxLength(100);
            entity.Property(e => e.HouseNumber)
                .HasMaxLength(10)
                .HasColumnName("House_Number");
            entity.Property(e => e.OrganizationType)
                .HasMaxLength(100)
                .HasColumnName("Organization_Type");
            entity.Property(e => e.RecordingMethodId).HasColumnName("Recording_Method_ID");
            entity.Property(e => e.Street).HasMaxLength(100);
            entity.Property(e => e.SubdivisionName)
                .HasMaxLength(100)
                .HasColumnName("Subdivision_Name");

            entity.HasOne(d => d.RecordingMethod).WithMany(p => p.Registries)
                .HasForeignKey(d => d.RecordingMethodId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Registry_Recording_Methods");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0787B9E1CB");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC27230BF89A");

            entity.ToTable("Schedule");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(20)
                .HasColumnName("Day_Of_Week");
            entity.Property(e => e.DoctorId).HasColumnName("Doctor_ID");
            entity.Property(e => e.EndTime).HasColumnName("End_Time");
            entity.Property(e => e.StartTime).HasColumnName("Start_Time");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Schedule_Doctors");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC27F0C49F75");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AppointmentDate).HasColumnName("Appointment_Date");
            entity.Property(e => e.AppointmentTime).HasColumnName("Appointment_Time");
            entity.Property(e => e.DoctorId).HasColumnName("Doctor_ID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Tickets_Doctors");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0796F447B5");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleId__19DFD96B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
