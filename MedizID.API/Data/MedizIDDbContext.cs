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
    public DbSet<Appointment> Appointments { get; set; }

    // Facility management
    public DbSet<Installation> Installations { get; set; }
    public DbSet<Poli> Polis { get; set; }
    public DbSet<PoliTimeSlot> PoliTimeSlots { get; set; }
    public DbSet<FacilityStaff> FacilityStaffs { get; set; }
    public DbSet<FacilityPatient> FacilityPatients { get; set; }

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

        // FacilityPatient relationships
        modelBuilder.Entity<FacilityPatient>()
            .HasOne(fp => fp.Facility)
            .WithMany()
            .HasForeignKey(fp => fp.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FacilityPatient>()
            .HasOne(fp => fp.Patient)
            .WithMany()
            .HasForeignKey(fp => fp.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

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
            .HasOne(a => a.Appointment)
            .WithMany()
            .HasForeignKey(a => a.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Odontogram relationships
        modelBuilder.Entity<Odontogram>()
            .HasOne(o => o.Appointment)
            .WithMany()
            .HasForeignKey(o => o.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // STI relationships
        modelBuilder.Entity<STI>()
            .HasOne(s => s.Appointment)
            .WithMany()
            .HasForeignKey(s => s.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // AnamnesisTemplate relationships
        modelBuilder.Entity<AnamnesisTemplate>()
            .HasOne(a => a.Facility)
            .WithMany()
            .HasForeignKey(a => a.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Appointment relationships
        modelBuilder.Entity<Appointment>()
            .HasMany(a => a.Anamnesis)
            .WithOne(a => a.Appointment)
            .HasForeignKey(a => a.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasMany(a => a.Diagnoses)
            .WithOne(d => d.Appointment)
            .HasForeignKey(d => d.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasMany(a => a.Prescriptions)
            .WithOne(p => p.Appointment)
            .HasForeignKey(p => p.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasMany(a => a.LaboratoriumTests)
            .WithOne(l => l.Appointment)
            .HasForeignKey(l => l.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.FacilityPatient)
            .WithMany()
            .HasForeignKey(a => a.FacilityPatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.FacilityDoctor)
            .WithMany()
            .HasForeignKey(a => a.FacilityDoctorId)
            .OnDelete(DeleteBehavior.SetNull);

        // Specialized services relationships to Appointment
        modelBuilder.Entity<AdolescentHealth>()
            .HasOne(a => a.Appointment)
            .WithMany()
            .HasForeignKey(a => a.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FamilyPlanning>()
            .HasOne(f => f.Appointment)
            .WithMany()
            .HasForeignKey(f => f.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaternalChildHealth>()
            .HasOne(m => m.Appointment)
            .WithMany()
            .HasForeignKey(m => m.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Immunization>()
            .HasOne(i => i.Appointment)
            .WithMany()
            .HasForeignKey(i => i.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MedicalProcedure>()
            .HasOne(m => m.Appointment)
            .WithMany()
            .HasForeignKey(m => m.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HIVCounseling>()
            .HasOne(h => h.Appointment)
            .WithMany()
            .HasForeignKey(h => h.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure columns
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<FacilityStaff>()
            .Property(f => f.Role)
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
