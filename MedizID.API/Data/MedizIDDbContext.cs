using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MedizID.API.Models;

namespace MedizID.API.Data;

public class MedizIDDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public MedizIDDbContext(DbContextOptions<MedizIDDbContext> options)
        : base(options)
    {
    }

    // Core entities
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }

    // Facility management
    public DbSet<Installation> Installations { get; set; }
    public DbSet<Poli> Polis { get; set; }
    public DbSet<PoliTimeSlot> PoliTimeSlots { get; set; }
    public DbSet<FacilityStaff> FacilityStaffs { get; set; }

    // Medical records components
    public DbSet<Anamnesis> Anamnesis { get; set; }
    public DbSet<AnamnesisTemplate> AnamnesisTemplates { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Laboratorium> LaboratoriumTests { get; set; }

    // Pharmacy
    public DbSet<Drug> Drugs { get; set; }
    public DbSet<DrugCategory> DrugCategories { get; set; }
    public DbSet<DrugInteraction> DrugInteractions { get; set; }

    // Medical equipment
    public DbSet<MedicalEquipment> MedicalEquipments { get; set; }
    public DbSet<MedicalEquipmentType> MedicalEquipmentTypes { get; set; }

    // Medical reference data
    public DbSet<ICD10Code> ICD10Codes { get; set; }
    public DbSet<Symptom> Symptoms { get; set; }

    // AI Recommendations
    public DbSet<AIRecommendation> AIRecommendations { get; set; }

    // Specialized medical services
    public DbSet<FamilyPlanning> FamilyPlannings { get; set; }
    public DbSet<MaternalChildHealth> MaternalChildHealths { get; set; }
    public DbSet<Immunization> Immunizations { get; set; }
    public DbSet<MedicalProcedure> MedicalProcedures { get; set; }
    public DbSet<Odontogram> Odontograms { get; set; }
    public DbSet<AdolescentHealth> AdolescentHealths { get; set; }
    public DbSet<HIVCounseling> HIVCounselings { get; set; }
    public DbSet<STI> STIs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Facility relationships
        modelBuilder.Entity<Facility>()
            .HasMany(f => f.Users)
            .WithOne(u => u.Facility)
            .HasForeignKey(u => u.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Facility>()
            .HasMany(f => f.Departments)
            .WithOne(d => d.Facility)
            .HasForeignKey(d => d.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Facility>()
            .HasMany(f => f.Patients)
            .WithOne(p => p.Facility)
            .HasForeignKey(p => p.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Facility>()
            .HasMany(f => f.Appointments)
            .WithOne(a => a.Facility)
            .HasForeignKey(a => a.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Installation relationships
        modelBuilder.Entity<Installation>()
            .HasOne(i => i.Facility)
            .WithMany()
            .HasForeignKey(i => i.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Poli relationships
        modelBuilder.Entity<Poli>()
            .HasOne(p => p.Facility)
            .WithMany()
            .HasForeignKey(p => p.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Poli>()
            .HasOne(p => p.Installation)
            .WithMany()
            .HasForeignKey(p => p.InstallationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Poli>()
            .HasMany(p => p.TimeSlots)
            .WithOne(t => t.Poli)
            .HasForeignKey(t => t.PoliId)
            .OnDelete(DeleteBehavior.Cascade);

        // PoliTimeSlot relationships
        modelBuilder.Entity<PoliTimeSlot>()
            .HasOne(t => t.Staff)
            .WithMany()
            .HasForeignKey(t => t.StaffId)
            .OnDelete(DeleteBehavior.SetNull);

        // FacilityStaff relationships
        modelBuilder.Entity<FacilityStaff>()
            .HasOne(f => f.Facility)
            .WithMany()
            .HasForeignKey(f => f.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FacilityStaff>()
            .HasOne(f => f.Staff)
            .WithMany()
            .HasForeignKey(f => f.StaffId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FacilityStaff>()
            .HasOne(f => f.Department)
            .WithMany()
            .HasForeignKey(f => f.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        // Drug relationships
        modelBuilder.Entity<Drug>()
            .HasOne(d => d.Category)
            .WithMany(c => c.Drugs)
            .HasForeignKey(d => d.DrugCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // MedicalEquipment relationships
        modelBuilder.Entity<MedicalEquipment>()
            .HasOne(m => m.EquipmentType)
            .WithMany(e => e.Equipment)
            .HasForeignKey(m => m.EquipmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalEquipment>()
            .HasOne(m => m.Facility)
            .WithMany()
            .HasForeignKey(m => m.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        // AIRecommendation relationships
        modelBuilder.Entity<AIRecommendation>()
            .HasOne(a => a.MedicalRecord)
            .WithMany()
            .HasForeignKey(a => a.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // Odontogram relationships
        modelBuilder.Entity<Odontogram>()
            .HasOne(o => o.MedicalRecord)
            .WithMany()
            .HasForeignKey(o => o.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // STI relationships
        modelBuilder.Entity<STI>()
            .HasOne(s => s.MedicalRecord)
            .WithMany()
            .HasForeignKey(s => s.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // AnamnesisTemplate relationships
        modelBuilder.Entity<AnamnesisTemplate>()
            .HasOne(a => a.Facility)
            .WithMany()
            .HasForeignKey(a => a.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Patient relationships
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.MedicalRecords)
            .WithOne(m => m.Patient)
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Appointment relationships
        modelBuilder.Entity<Appointment>()
            .HasMany(a => a.MedicalRecords)
            .WithOne(m => m.Appointment)
            .HasForeignKey(m => m.AppointmentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany()
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.SetNull);

        // MedicalRecord relationships
        modelBuilder.Entity<MedicalRecord>()
            .HasMany(m => m.Anamnesis)
            .WithOne(a => a.MedicalRecord)
            .HasForeignKey(a => a.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MedicalRecord>()
            .HasMany(m => m.Diagnoses)
            .WithOne(d => d.MedicalRecord)
            .HasForeignKey(d => d.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MedicalRecord>()
            .HasMany(m => m.Prescriptions)
            .WithOne(p => p.MedicalRecord)
            .HasForeignKey(p => p.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MedicalRecord>()
            .HasMany(m => m.LaboratoriumTests)
            .WithOne(l => l.MedicalRecord)
            .HasForeignKey(l => l.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure columns
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Facility>()
            .Property(f => f.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Appointment>()
            .Property(a => a.Status)
            .HasConversion<string>();

        modelBuilder.Entity<HIVCounseling>()
            .Property(h => h.VisitStatus)
            .HasConversion<string>();

        modelBuilder.Entity<HIVCounseling>()
            .Property(h => h.ClientStatus)
            .HasConversion<string>();

        modelBuilder.Entity<HIVCounseling>()
            .Property(h => h.RiskGroup)
            .HasConversion<string>();

        modelBuilder.Entity<STI>()
            .Property(s => s.VisitStatus)
            .HasConversion<string>();

        modelBuilder.Entity<STI>()
            .Property(s => s.RiskGroup)
            .HasConversion<string>();
    }
}

public class ApplicationRole : Microsoft.AspNetCore.Identity.IdentityRole<Guid>
{
}
