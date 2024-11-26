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
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=Gg/bet/com2020;Database=ClinicDB;TrustServerCertificate=True");

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
                .HasConstraintName("FK__Appointme__Docto__60A75C0F");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Appointme__Patie__619B8048");
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
                .HasConstraintName("FK__Disabilit__Docto__5EBF139D");
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
                .HasConstraintName("FK__Districts__Docto__68487DD7");
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
                .HasConstraintName("FK__Doctors__Departm__5CD6CB2B");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("FK__Doctors__Special__5DCAEF64");
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
                .HasConstraintName("FK__Medical_D__Patie__5FB337D6");
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
                .HasConstraintName("FK__Outpatien__Patie__5BE2A6F2");
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
                .HasConstraintName("FK__Patients__Distri__628FA481");
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
                .HasConstraintName("FK__Reasons_F__Patie__6383C8BA");
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
                .HasConstraintName("FK__Registrar__Regis__656C112C");
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
                .HasConstraintName("FK__Registry__Record__66603565");
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
                .HasConstraintName("FK__Schedule__Doctor__6477ECF3");
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
                .HasConstraintName("FK__Tickets__Doctor___6754599E");
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
